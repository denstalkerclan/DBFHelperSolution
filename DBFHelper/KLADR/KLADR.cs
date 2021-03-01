using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
    /// <summary>
    /// объекты c 1-го по 4-й уровень классификации (регионы; районы (улусы); города, посёлки городского типа, сельсоветы; сельские населённые пункты)
    /// </summary>
    [DebuggerDisplay("{CODE};{NAME}")]
    public class KLADR : STREET
    {
        /// <summary>
        /// Статус объекта
        /// </summary>
        public string STATUS { get; set; }

        public override bool IsActual => CODE?.Length == 13 && CODE.EndsWith("00");
    }
}
