using DBFHelper;
using DBFHelper.Enums;
using DBFHelper.KLADR;
using System;
using System.Linq;

namespace DBHelperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new Helper("https://fias.nalog.ru/WebServices/Public/DownloadService.asmx");
            helper.GetData(DateTime.MinValue, TypeData.Kladr47Z);
            var res = helper.GetListObject<KLADR>().Where(x => x.IsActual).ToList();
            res.Take(100).ToList().ForEach(x => Console.WriteLine($"{x.CODE};{x.NAME}"));

            Console.ReadKey();
        }
    }
}
