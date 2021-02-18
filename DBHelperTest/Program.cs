using DBFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new Helper("https://fias.nalog.ru/WebServices/Public/DownloadService.asmx");
            helper.GetData(DateTime.MinValue, TypeData.Kladr47Z);
            var res = helper.GetListObject<KLADR>();
        }
    }
}
