using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
    /// <summary>
    /// объекты с краткими наименованиями типов адресных объектов
    /// </summary>
    [DebuggerDisplay("{LEVEL};{SCNAME};{SOCRNAME};{KOD_T_ST}")]
    public class SOCRBASE
    {
        /// <summary>
        /// Уровень объекта
        /// </summary>
        public string LEVEL { get; set; }
        /// <summary>
        /// Сокращенное наименование типа объекта
        /// </summary>
        public string SCNAME { get; set; }
        /// <summary>
        /// Полное наименование типа объекта
        /// </summary>
        public string SOCRNAME { get; set; }
        /// <summary>
        /// Код типа объекта
        /// </summary>
        public string KOD_T_ST { get; set; }
    }
}
