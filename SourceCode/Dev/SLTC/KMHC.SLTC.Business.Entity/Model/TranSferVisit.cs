﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class TranSferVisit
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<System.DateTime> TRANSDATE { get; set; }
        public string DEPTNO { get; set; }
        public string DIAGNOSIS { get; set; }
        public Nullable<int> DISEASECOUNT { get; set; }
        public string CURRENTISSUE { get; set; }
        public Nullable<int> HEIGHT { get; set; }
        public Nullable<int> WEIGHT { get; set; }
        public Nullable<int> KNEELENGTH { get; set; }
        public Nullable<int> WEIGHT_S { get; set; }
        public Nullable<int> BMI { get; set; }
        public Nullable<int> BODYTEMP { get; set; }
        public Nullable<int> PULSE { get; set; }
        public Nullable<int> BP { get; set; }
        public Nullable<int> BLOODOXYGEN { get; set; }
        public Nullable<int> BS { get; set; }
        public Nullable<int> EKG { get; set; }
        public string EAT { get; set; }
        public string SWALLOW { get; set; }
        public string DIETMODE { get; set; }
        public string STOMACH { get; set; }
        public string FOODTABOO { get; set; }
        public string ACTIVITY { get; set; }
        public string CURRENTDIET { get; set; }
        public string CURRENTDIETSTATE { get; set; }
        public string FOODINTAKE { get; set; }
        public string WATER { get; set; }
        public string NUTRITION { get; set; }
        public string SNACKS { get; set; }
        public string PIPLELINE { get; set; }
        public string MEDICINE { get; set; }
        public string CHECKDESC { get; set; }
        public Nullable<System.DateTime> ASSESSDATE { get; set; }
        public string ASSESSBY { get; set; }
        public Nullable<System.DateTime> REPLYDATE { get; set; }
        public string SUPERVISOR { get; set; }
        public string SUGGESTSUMMARY { get; set; }
        public string PROCESSDESC { get; set; }
        public string PROCESSRESULTS { get; set; }
        public string CONSULTDEPT { get; set; }
        public string CONSULTTYPE { get; set; }
        public Nullable<System.DateTime> CONSULTDATE { get; set; }
        public string CONSULTITEM { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }


    }
}
