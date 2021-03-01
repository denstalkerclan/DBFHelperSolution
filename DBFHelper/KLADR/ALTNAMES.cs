using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
    /// <summary>
    /// сведения о соответствии кодов записей со старыми и новыми наименованиями адресных объектов,
    /// а также сведения о соответствии кодов адресных объектов до и после их переподчинения
    /// </summary>
    [DebuggerDisplay("{OLDCODE};{NEWCODE};{LEVEL}")]
    public class ALTNAMES
    {
        /// <summary>
        /// Старый код
        /// </summary>
        public string OLDCODE { get; set; }
        /// <summary>
        /// Новый код
        /// </summary>
        public string NEWCODE { get; set; }
        /// <summary>
        /// Уровень объекта
        /// </summary>
        public string LEVEL { get; set; }
    }
}
