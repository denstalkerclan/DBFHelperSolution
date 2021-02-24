using SevenZipExtractor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Concurrent;
using DBFHelper.FIASService;
using System.Diagnostics;
using System.ServiceModel;
using DBFHelper.Enums;
using DBFHelper.KLADR;

namespace DBFHelper
{
    public class Helper
    {
        private bool _init = false;
        public DownloadFileInfo InfoObject;
        public List<DownloadFileInfo> AllInfoObject;
        private string _url = string.Empty;
        private List<FileItem> _downloadHistory = new List<FileItem>();
        private string _outputDirectory;
        public ConcurrentDictionary<string, IEnumerable<object>> Results = new ConcurrentDictionary<string, IEnumerable<object>>();

        public Helper(string url)
        {
            _url = url;
            Init();
        }

        private void Init()
        {
            Trace.TraceInformation($"Init: " + _url);
            var uri = new Uri(_url);

            HttpBindingBase binding = uri.Scheme == Uri.UriSchemeHttps ? new BasicHttpsBinding() { MaxReceivedMessageSize = 1073741824 } as HttpBindingBase : new BasicHttpBinding() { MaxReceivedMessageSize = 1073741824 };
            using (var service = new DownloadServiceClient(binding, new EndpointAddress(uri)))
            {
                AllInfoObject = service.GetAllDownloadFileInfo();
                InfoObject = service.GetLastDownloadFileInfo();
                _init = true;
            }
        }

        private DateTime? GetVersionDate(TypeData typeData)
        {
            if (_init)
            {
                DateTime? res = null;
                switch (typeData)
                {
                    case TypeData.Kladr47Z:
                        var info = AllInfoObject.Where(x => !string.IsNullOrEmpty(x.Kladr47ZUrl)).OrderBy(x => DateTime.Parse(x.Date)).LastOrDefault();
                        res = DateTime.Parse(info.Date);
                        break;
                    default:
                        res = DateTime.Parse(InfoObject.Date);
                        break;
                }
                return res;
            }
            else
                return null;
        }

        public void GetData(DateTime dateLastUpdate, TypeData typeData)
        {
            Trace.TraceInformation($"GetData: {dateLastUpdate.ToShortDateString()} {typeData.ToString()}");
            if (!_init)
                Init();

            if (dateLastUpdate >= GetVersionDate(typeData).GetValueOrDefault((DateTime)SqlDateTime.MinValue))
                return;
            var load = false;

            switch (typeData)
            {
                case TypeData.FiasDbf:
                    if (dateLastUpdate > SqlDateTime.MinValue)
                        load = LoadDifFIAS(dateLastUpdate, typeData);
                    else
                        load = LoadFullFIAS(GetVersionDate(typeData).GetValueOrDefault(DateTime.Today), typeData);
                    break;
                case TypeData.Kladr47Z:
                    load = LoadFullKLADR(GetVersionDate(typeData).GetValueOrDefault(DateTime.Today), typeData);
                    break;
            }
            if (load)
            {
                Trace.TraceInformation($"GetData: load");
                ReadDBF(_outputDirectory);
            }
            else
                Trace.TraceInformation($"GetData: not load");
        }

        private string GetTempDir()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), "LoadKLADR");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Trace.TraceInformation($"TempDir: {path}");

            return path;
        }

        private string GetTempFileName(DateTime date, TypeData typeData, bool full)
        {
            var path = Path.Combine(GetTempDir(), $"{typeData.ToString()}{(full ? "Full" : "Delta")}{date.ToString("ddMMyyyy")}.{(typeData == TypeData.FiasDbf ? "zip" : "7Z")}");


            Trace.TraceInformation($"TempFileName: {path}");
            return path;
        }

        private string GetDirectoryUnZip(string filePath)
        {
            _outputDirectory = filePath.Replace(Path.GetExtension(filePath), "");
            Trace.TraceInformation($"DirectoryUnZip: {_outputDirectory}");
            return _outputDirectory;
        }

        private bool LoadFullKLADR(DateTime dateLoad, TypeData typeData)
        {
            try
            {
                var name = GetTempFileName(dateLoad, typeData, true);
                var info = AllInfoObject.Where(x => !string.IsNullOrEmpty(x.Kladr47ZUrl)).OrderBy(x => DateTime.Parse(x.Date)).LastOrDefault();
                if (DownloadFile(info.Kladr47ZUrl, name))
                {
                    UnZip(name);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return false;
        }

        private bool LoadFullFIAS(DateTime dateLoad, TypeData typeData)
        {
            try
            {
                var name = GetTempFileName(dateLoad, typeData, true);
                var info = AllInfoObject.Where(x => !string.IsNullOrEmpty(x.FiasCompleteDbfUrl)).OrderByDescending(x => DateTime.Parse(x.Date)).LastOrDefault();
                if (DownloadFile(info.FiasCompleteDbfUrl, name))
                {
                    UnZip(name);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return false;
        }

        private bool LoadDifFIAS(DateTime dateLastUpdate, TypeData typeData)
        {
            try
            {
                var name = GetTempFileName(dateLastUpdate, typeData, false);
                var info = AllInfoObject.Where(x => !string.IsNullOrEmpty(x.FiasDeltaDbfUrl)).OrderByDescending(x => DateTime.Parse(x.Date)).LastOrDefault();
                if (DownloadFile(info.FiasDeltaDbfUrl, name))
                {
                    UnZip(name);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return false;
        }

        private bool DownloadFile(string uri, string file)
        {
            try
            {

                if (File.Exists(file))
                    return true;
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(uri, file);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return false;
            }
        }

        private void UnZip(string name)
        {
            Trace.TraceInformation($"UnZip: {name}");
            var dirPath = GetDirectoryUnZip(name);

            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);

            Directory.CreateDirectory(dirPath);

            using (ArchiveFile archiveFile = new ArchiveFile(name))
            {
                archiveFile.Extract(dirPath);
            }
        }

        private void ReadDBF(string name)
        {
            var encodes = System.Text.Encoding.GetEncoding(866);

            Directory.GetFiles(name).AsParallel().ForAll(file =>
            {
                using (var dbf = new Reader(file, encodes))
                {
                    IEnumerable<object> list = null;
                    switch (Path.GetFileNameWithoutExtension(file))
                    {
                        case nameof(ALTNAMES):
                            list = dbf.ReadToObject<ALTNAMES>();
                            break;
                        case nameof(DOMA):
                            list = dbf.ReadToObject<DOMA>();
                            break;
                        case nameof(FLAT):
                            list = dbf.ReadToObject<FLAT>();
                            break;
                        case nameof(KLADR.KLADR):
                            list = dbf.ReadToObject<KLADR.KLADR>();
                            break;
                        case nameof(NAMEMAP):
                            list = dbf.ReadToObject<NAMEMAP>();
                            break;
                        case nameof(SOCRBASE):
                            list = dbf.ReadToObject<SOCRBASE>();
                            break;
                        case nameof(STREET):
                            list = dbf.ReadToObject<STREET>();
                            break;
                    }

                    Results.AddOrUpdate(Path.GetFileNameWithoutExtension(file), list, (x, y) => y = list);
                }
            });
        }

        public IEnumerable<T> GetListObject<T>()
        {
            var name = typeof(T).Name;
            return Results.ContainsKey(name)?Results[name].OfType<T>() : new List<T>();
        }


    }

    public class FileItem
    {
        public string RemoteUri;

        public string SavePath;

        public string Date;

        public string RefId;

        public string Name;

        public string Extention;

        public long Size;
    }
}
