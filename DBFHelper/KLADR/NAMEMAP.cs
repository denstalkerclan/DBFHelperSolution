using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
    [DebuggerDisplay("{NAME};{SHNAME};{SCNAME}")]
    public class NAMEMAP
    {
        public string NAME { get; set; }
        public string SHNAME { get; set; }
        public string SCNAME { get; set; }
    }
}
