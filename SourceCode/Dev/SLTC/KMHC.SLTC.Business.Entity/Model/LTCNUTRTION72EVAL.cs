using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class LTCNUTRTION72EVAL 
    {

        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<System.DateTime> RECORDDATE { get; set; }
        public string RECORDBY { get; set; }
        public string RECORDNAME { get; set; }
        public Nullable<decimal> CURRENTWEIGHT { get; set; }
        public Nullable<decimal> IDEALWEIGHT { get; set; }
        public Nullable<int> HEIGHT { get; set; }
        public Nullable<decimal> BMI { get; set; }
        public string DIETARY { get; set; }
        public string FEEDING { get; set; }
        public string BREAKFAST { get; set; }
        public string LUNCH { get; set; }
        public string DINNER { get; set; }
        public string SNACK { get; set; }
        public string LIKEFOOD { get; set; }
        public string NOTLIKEFOOD { get; set; }
        public string ALLERGICFOOD { get; set; }
        public string GASTROINTESTINAL { get; set; }
        public string FUNCTIONALEVAL { get; set; }
        public string FATREDUCTION { get; set; }
        public string MUSCLEWEAK { get; set; }
        public string EDEMA { get; set; }
        public string ASCITES { get; set; }
        public string BEDSORE { get; set; }
        public string BEDSORELEVEL { get; set; }
        public string EVALRESULT { get; set; }
        public string ORGID { get; set; }
        public string CHEW { get; set; }
        public string SWALLOW { get; set; }

    }
}





