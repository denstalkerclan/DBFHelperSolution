using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
    /// <summary>
    /// объекты 6-го уровня классификации (номера домов, улиц, городов и населённых пунктов)
    /// </summary>
    public class DOMA : STREET
    {
        /// <summary>
        /// Корпус дома
        /// </summary>
        public string KORP { get; set; }

        public override bool IsActual => CODE?.Length == 19;
    }
}
