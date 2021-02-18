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
                        case nameof(KLADR):
                            list = dbf.ReadToObject<KLADR>();
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

    public enum TypeData
    {
        FiasDbf,
        Kladr47Z
    }


    public enum LevelObject
    {
        Region,
        RAYON,
        CITY,
        VILLAGE,
        STRUCT,
        STREET,
        HOUSE,
        ROOM,
        NONE
    }


    public abstract class BaseKLADR
    {
        /*СС+РРР+ГГГ+ППП+СССС+УУУУ+ДДДД(или ЗЗЗЗ)+ОООО, где:

        СС – код субъекта Российской Федерации  – региона
        РРР – код района;
        ГГГ – код города;
        ППП код населенного пункта;
        СССС - код элемента планировочной структуры;
        УУУУ - код улицы;
        ДДДД(или ЗЗЗЗ).  ДДДД тип и номер здания, сооружения, объекта незавершенного строительства в случае адресации домов.ЗЗЗЗ - номер земельного участка в случае адресации земельных участков;
        ОООО - тип и номер помещения в пределах здания, сооружения*/

        public BaseKLADR()
        {
            Childs = new List<BaseKLADR>();
        }

        public string CODE { get; set; }
        public int REGION
        { 
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 2 ? "0" : CODE.Substring(0, 2));
            }
        }
        public int RAYON
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 6 ? "0" : CODE.Substring(2, 3));
            }
        }
        public int CITY
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 9 ? "0" : CODE.Substring(5, 3));
            }
        }
        public int VILLAGE
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 12 ? "0" : CODE.Substring(8, 3));
            }
        }
        public int STRUCT
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 16 ? "0" : CODE.Substring(11, 4));
            }
        }
        public int STREET
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 20 ? "0" : CODE.Substring(15, 4));
            }
        }
        public int HOUSE
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 24 ? "0" : CODE.Substring(19, 4));
            }
        }
        public int ROOM
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 28 ? "0" : CODE.Substring(23, 4));
            }
        }

        public bool IsRegion => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT == 0 && VILLAGE == 0 && CITY == 0 && RAYON == 0 && REGION > 0;
        public bool IsRAYON => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT == 0 && VILLAGE == 0 && CITY == 0 && RAYON > 0 && REGION > 0;
        public bool IsCITY => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT == 0 && VILLAGE == 0 && CITY > 0 && RAYON >= 0 && REGION >= 0;
        public bool IsVILLAGE => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT == 0 && VILLAGE > 0 && CITY >= 0 && RAYON >= 0 && REGION > 0;
        public bool IsSTRUCT => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT > 0 && VILLAGE >= 0 && CITY >= 0 && RAYON >= 0 && REGION > 0;
        public bool IsSTREET => ROOM == 0 && HOUSE == 0 && STREET > 0 && VILLAGE >= 0 && CITY >= 0 && RAYON > 0 && REGION > 0;
        public bool IsHOUSE => ROOM == 0 && HOUSE > 0 && STREET >= 0 && VILLAGE >= 0 && CITY >= 0 && RAYON >= 0 && REGION > 0;
        public bool IsROOM => ROOM > 0 && HOUSE >= 0 && STREET >= 0 && VILLAGE >= 0 && CITY >= 0 && RAYON >= 0 && REGION > 0;

        public LevelObject LevelEnum
        {
            get
            {
                if (IsRegion)
                    return LevelObject.Region;
                else if (IsRAYON)
                    return LevelObject.RAYON;
                else if (IsCITY)
                    return LevelObject.CITY;
                else if (IsVILLAGE)
                    return LevelObject.VILLAGE;
                else if (IsSTRUCT)
                    return LevelObject.STRUCT;
                else if (IsSTREET)
                    return LevelObject.STREET;
                else if (IsHOUSE)
                    return LevelObject.HOUSE;
                else if (IsROOM)
                    return LevelObject.ROOM;
                else
                    return LevelObject.NONE;

            }
        }

        public List<BaseKLADR> Childs { get; set; }


        public void FillChild(IEnumerable<BaseKLADR> coll)
        {
            if(LevelEnum == LevelObject.Region)
                Childs.AddRange(coll.Where(x=>x.REGION == this.REGION && (x.IsRAYON || (x.IsCITY && x.RAYON == 0) || (x.IsVILLAGE && x.RAYON == 0))));
            else if(LevelEnum == LevelObject.RAYON)
                Childs.AddRange(coll.Where(x => x.REGION == this.REGION && x.RAYON == this.RAYON && (x.IsCITY || x.IsVILLAGE)));
            else if (LevelEnum == LevelObject.CITY)
                Childs.AddRange(coll.Where(x => x.CITY == this.CITY && (x.IsSTRUCT || x.IsSTREET)));
            else if (LevelEnum == LevelObject.VILLAGE)
                Childs.AddRange(coll.Where(x => x.VILLAGE == this.VILLAGE && (x.IsSTRUCT || x.IsSTREET)));
            else if (LevelEnum == LevelObject.STREET)
                Childs.AddRange(coll.Where(x => x.STREET == this.STREET && x.IsHOUSE));


            foreach(var child in Childs)
            {
                child.FillChild(coll);
            }
        }
    }

    public class ALTNAMES : BaseKLADR
    {

        public string OLDCODE { get; set; }
        public string NEWCODE { get; set; }
        public string LEVEL { get; set; }
    }
    public class DOMA : BaseKLADR
    {
        public string NAME { get; set; }
        public string KORP { get; set; }
        public string SOCR { get; set; }
        public string INDEX { get; set; }
        public string GNINMB { get; set; }
        public string UNO { get; set; }
        public string OCATD { get; set; }
    }
    public class FLAT : BaseKLADR
    {
        public string NP { get; set; }
        public string GNINMB { get; set; }
        public string NAME { get; set; }
        public string INDEX { get; set; }
        public string UNO { get; set; }
    }
    public class KLADR : BaseKLADR
    {
        public string NAME { get; set; }
        public string SOCR { get; set; }
        public string INDEX { get; set; }
        public string GNINMB { get; set; }
        public string UNO { get; set; }
        public string OCATD { get; set; }
        public string STATUS { get; set; }
    }
    public class NAMEMAP : BaseKLADR
    {
        public string NAME { get; set; }
        public string SHNAME { get; set; }
        public string SCNAME { get; set; }
    }
    public class SOCRBASE
    {
        public string LEVEL { get; set; }
        public string SCNAME { get; set; }
        public string SOCRNAME { get; set; }
        public string KOD_T_ST { get; set; }
    }
    public class STREET : BaseKLADR
    {
        public string NAME { get; set; }
        public string SOCR { get; set; }
        public string INDEX { get; set; }
        public string GNINMB { get; set; }
        public string UNO { get; set; }
        public string OCATD { get; set; }
    }

    public class KLADROBJECT
    {
        public KLADROBJECT Owner { get; set; }

        public string LEVEL { get; set; }
        public string CODE { get; set; }
        public string SOCRNAME { get; set; }
        public string FULLNAME { get; set; }
        public string FULLTYPENAME { get; set; }
        public string SOCRTYPENAME { get; set; }
        public string OCATD { get; set; }
        public string STATUS { get; set; }
        public string INDEX { get; set; }
        public string GNINMB { get; set; }

        public List<KLADROBJECT> Childs { get; set; }

    }
}
