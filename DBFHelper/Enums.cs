using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.Enums
{
    public enum TypeData
    {
        FiasDbf,
        Kladr47Z
    }

    public enum LevelObject
    {
        /// <summary>
        /// Регион
        /// </summary>
        Region,
        /// <summary>
        /// Район
        /// </summary>
        RAYON,
        /// <summary>
        /// Город
        /// </summary>
        CITY,
        /// <summary>
        /// Поселок
        /// </summary>
        VILLAGE,
        /// <summary>
        /// 
        /// </summary>
        STRUCT,
        /// <summary>
        /// Улица
        /// </summary>
        STREET,
        /// <summary>
        /// Дом
        /// </summary>
        HOUSE,
        /// <summary>
        /// Квартира/Офис
        /// </summary>
        ROOM,
        NONE
    }
}
