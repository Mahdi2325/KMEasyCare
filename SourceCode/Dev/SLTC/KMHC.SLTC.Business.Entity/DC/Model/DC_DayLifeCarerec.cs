using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DayLife {

        public DC_DayLifeCarerec DayLifeRec { get; set; }
        public List<DC_DayLifeCaredtl> DayLifeCaredtl { get; set; }
    }

    public class DayLife2
    {
        public DayLifeWeeks DayLifeRec { get; set; }
        public List<DC_DayLifeCaredtl> DayLifeCaredtl { get; set; }

    }

   public class DC_DayLifeCarerec
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNO { get; set; }
        public string RESIDENTNO { get; set; }
        public string REGNAME { get; set; }
        public string SEX { get; set; }
        public string NURSEAIDES { get; set; }
        public string WEEKNUMBER { get; set; }
        public string CONTACTMATTERS { get; set; }
        public string FAMILYMESSAGE { get; set; }
        public string SOCIALWORKER { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public Nullable<System.DateTime> CREATEBY { get; set; }
        public Nullable<System.DateTime> WEEKSTARTDATE { get; set; }
        public Nullable<bool> DELFLAG { get; set; }
        public Nullable<System.DateTime> DELDATE { get; set; }
        public string ORGID { get; set; }
        public List<DC_DayLifeCaredtl> DayLifeCaredtl { get; set; }
    }

   public class DayLifeWeeks
   {



       public long ID { get; set; }
       public Nullable<long> FEENO { get; set; }
       public string REGNO { get; set; }
       public string RESIDENTNO { get; set; }
       public string REGNAME { get; set; }
       public string SEX { get; set; }
       public string NURSEAIDES { get; set; }
       public string WEEKNUMBER { get; set; }
       public string CONTACTMATTERS { get; set; }
       public string FAMILYMESSAGE { get; set; }
       public string SOCIALWORKER { get; set; }
       public Nullable<System.DateTime> CREATEDATE { get; set; }
       public Nullable<System.DateTime> CREATEBY { get; set; }
       public Nullable<System.DateTime> WEEKSTARTDATE { get; set; }
       public Nullable<bool> DELFLAG { get; set; }
       public Nullable<System.DateTime> DELDATE { get; set; }
       public string ORGID { get; set; }
       public List<DC_DayLifeCaredtl> DayLifeCaredtl { get; set; }


       public Nullable<System.DateTime> WEEK1 { get; set; }
       public Nullable<System.DateTime> WEEK2 { get; set; }

       public Nullable<System.DateTime> WEEK3 { get; set; }

       public Nullable<System.DateTime> WEEK4 { get; set; }

       public Nullable<System.DateTime> WEEK5 { get; set; }


       public string Res { get; set; }

       public string Nur { get; set; }


      

   }
}
