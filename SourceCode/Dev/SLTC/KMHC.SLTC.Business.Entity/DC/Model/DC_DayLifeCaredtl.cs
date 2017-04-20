using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DC_DayLifeCaredtl
    {
       
        public long SEQNO { get; set; }
        public Nullable<long> ID { get; set; }
        public string TEA9 { get; set; }
        public string SNACKTEA9 { get; set; }
        public string LUNCH { get; set; }
        public string SOUPAMOUNT { get; set; }
        public string TEA14 { get; set; }
        public string SNACKTEA1530 { get; set; }
        public string NOONBREAK { get; set; }
        public string BRUSHINGTEETH { get; set; }
        public string PERINEALWASHING { get; set; }
        public string OTHERCLEAN { get; set; }
        public string SHITAMOUNT { get; set; }
        public string SHITCOLOR { get; set; }
        public string SHITNATURE { get; set; }
        public string URINECOLOR { get; set; }
        public string TOILET { get; set; }
        public string TOILETTIME { get; set; }
        public string EQUIPMENT { get; set; }
        public Nullable<System.DateTime> RECORDDATE { get; set; }
        public string DAYOFWEEK { get; set; }
        /// <summary>
        ///是否休假
        /// </summary>
        public string HOLIDAYFLAG { get; set; }
    }
}
