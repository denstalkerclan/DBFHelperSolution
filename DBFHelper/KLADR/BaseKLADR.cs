using DBFHelper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper.KLADR
{
    /// <summary>
    /// Базовый класс КЛАДР
    /// </summary>
    public abstract class BaseKLADR
    {
        /*СС+РРР+ГГГ+ППП+СССС+УУУУ+ДДДД(или ЗЗЗЗ)+ОООО, где:

        СС – код субъекта Российской Федерации  – региона
        РРР – код района;
        ГГГ – код города;
        ППП код населенного пункта;
        СССС - код элемента планировочной структуры;
        УУУУ - код улицы;
        ДДДД(или ЗЗЗЗ).  ДДДД тип и номер здания, сооружения, объекта незавершенного строительства в случае адресации домов.ЗЗЗЗ - номер земельного участка в случае адресации земельных участков;
        ОООО - тип и номер помещения в пределах здания, сооружения*/

        public BaseKLADR()
        {
            Childs = new List<BaseKLADR>();
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// Код
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string INDEX { get; set; }
        /// <summary>
        /// Код ИФНС
        /// </summary>
        public string GNINMB { get; set; }
        /// <summary>
        /// Код территориального участка ИФНС
        /// </summary>
        public string UNO { get; set; }

        /// <summary>
        /// Актуальность записи
        /// </summary>
        public abstract bool IsActual { get; }

        public int REGION
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 2 ? "0" : CODE.Substring(0, 2));
            }
        }
        public int RAYON
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 6 ? "0" : CODE.Substring(2, 3));
            }
        }
        public int CITY
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 9 ? "0" : CODE.Substring(5, 3));
            }
        }
        public int VILLAGE
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 12 ? "0" : CODE.Substring(8, 3));
            }
        }
        public int STRUCT
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 16 ? "0" : CODE.Substring(11, 4));
            }
        }
        public int STREET
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 20 ? "0" : CODE.Substring(15, 4));
            }
        }
        public int HOUSE
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 24 ? "0" : CODE.Substring(19, 4));
            }
        }
        public int ROOM
        {
            get
            {
                return int.Parse(string.IsNullOrEmpty(CODE) || CODE.Length < 28 ? "0" : CODE.Substring(23, 4));
            }
        }

        public bool IsRegion => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT == 0 && VILLAGE == 0 && CITY == 0 && RAYON == 0 && REGION > 0;
        public bool IsRAYON => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT == 0 && VILLAGE == 0 && CITY == 0 && RAYON > 0 && REGION > 0;
        public bool IsCITY => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT == 0 && VILLAGE == 0 && CITY > 0 && RAYON >= 0 && REGION >= 0;
        public bool IsVILLAGE => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT == 0 && VILLAGE > 0 && CITY >= 0 && RAYON >= 0 && REGION > 0;
        public bool IsSTRUCT => ROOM == 0 && HOUSE == 0 && STREET == 0 && STRUCT > 0 && VILLAGE >= 0 && CITY >= 0 && RAYON >= 0 && REGION > 0;
        public bool IsSTREET => ROOM == 0 && HOUSE == 0 && STREET > 0 && VILLAGE >= 0 && CITY >= 0 && RAYON > 0 && REGION > 0;
        public bool IsHOUSE => ROOM == 0 && HOUSE > 0 && STREET >= 0 && VILLAGE >= 0 && CITY >= 0 && RAYON >= 0 && REGION > 0;
        public bool IsROOM => ROOM > 0 && HOUSE >= 0 && STREET >= 0 && VILLAGE >= 0 && CITY >= 0 && RAYON >= 0 && REGION > 0;

        public LevelObject LevelEnum
        {
            get
            {
                if (IsRegion)
                    return LevelObject.Region;
                else if (IsRAYON)
                    return LevelObject.RAYON;
                else if (IsCITY)
                    return LevelObject.CITY;
                else if (IsVILLAGE)
                    return LevelObject.VILLAGE;
                else if (IsSTRUCT)
                    return LevelObject.STRUCT;
                else if (IsSTREET)
                    return LevelObject.STREET;
                else if (IsHOUSE)
                    return LevelObject.HOUSE;
                else if (IsROOM)
                    return LevelObject.ROOM;
                else
                    return LevelObject.NONE;

            }
        }

        public List<BaseKLADR> Childs { get; set; }


        public void FillChild(IEnumerable<BaseKLADR> coll)
        {
            if (LevelEnum == LevelObject.Region)
                Childs.AddRange(coll.Where(x => x.REGION == this.REGION && (x.IsRAYON || (x.IsCITY && x.RAYON == 0) || (x.IsVILLAGE && x.RAYON == 0))));
            else if (LevelEnum == LevelObject.RAYON)
                Childs.AddRange(coll.Where(x => x.REGION == this.REGION && x.RAYON == this.RAYON && (x.IsCITY || x.IsVILLAGE)));
            else if (LevelEnum == LevelObject.CITY)
                Childs.AddRange(coll.Where(x => x.CITY == this.CITY && (x.IsSTRUCT || x.IsSTREET)));
            else if (LevelEnum == LevelObject.VILLAGE)
                Childs.AddRange(coll.Where(x => x.VILLAGE == this.VILLAGE && (x.IsSTRUCT || x.IsSTREET)));
            else if (LevelEnum == LevelObject.STREET)
                Childs.AddRange(coll.Where(x => x.STREET == this.STREET && x.IsHOUSE));


            foreach (var child in Childs)
            {
                child.FillChild(coll);
            }
        }
    }
}
