using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
    /// <summary>
    /// объекты 5-го уровня классификации (улицы городов и населённых пунктов)
    /// </summary>
    public class STREET : BaseKLADR
    {
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string SOCR { get; set; }
        /// <summary>
        /// Код ОКАТО
        /// </summary>
        public string OCATD { get; set; }

        public override bool IsActual => CODE?.Length == 15 && CODE.EndsWith("00");
    }
}
