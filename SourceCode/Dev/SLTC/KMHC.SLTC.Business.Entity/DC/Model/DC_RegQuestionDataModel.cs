using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_RegQuestionDataModel
    {
        public int ID { get; set; }
        public int? ITEMID { get; set; }
        public string DESCRIPTION { get; set; }
        public int? ITEMVALUE { get; set; }
        public int? SEQ { get; set; }
        public int EVALRECID { get; set; }
        public int? QUESTIONID { get; set; }
        public string ORGID { get; set; }
        public int? SHOWNUMBER { get; set; }
        public string ITEMNAME { get; set; }

        public string RESIDENTNO { get; set; }
        public int? AGE { get; set; }
        public string NAME
        {
            get;
            set;
        }
        public DateTime? INDATE { get; set; }
        public DateTime? BIRTHDATE { get; set; }
        public string SEX { get; set; }
        public string EVALRESULT { get; set; }
        public float? SCORE { get; set; }
        public long? FEENO { get; set; }
    }
}
