using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
    /// <summary>
    /// объекты 7-го уровня классификации (номера квартир домов)
    /// </summary>
    public class FLAT : BaseKLADR
    {
        /// <summary>
        /// Номер подъезда дома 
        /// </summary>
        public string NP { get; set; }

        public override bool IsActual => CODE?.Length == 23;
    }
}
