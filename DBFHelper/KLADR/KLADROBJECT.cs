using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
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
