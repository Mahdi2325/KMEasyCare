﻿using KMHC.SLTC.Business.Entity.DC.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{

    public class AbNormaleMotionRec
    { 

        public ABFilter ab { get; set; }

        public List<DC_AbNormaleMotionRec> AbNormaleMotionlist { get; set; }
    
    }
   public  class DC_AbNormaleMotionRec
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNAME { get; set; }
        public string RESIDENTNO { get; set; }
        public string SEX { get; set; }
        public string NURSEAIDES { get; set; }
        public Nullable<int> YEAR { get; set; }
        public Nullable<int> MONTH { get; set; }
        public Nullable<System.DateTime> RECORDDATE { get; set; }
        public Nullable<int> DELUSION { get; set; }
        public Nullable<int> VISUALILLUSION { get; set; }
        public Nullable<int> MISDEEM { get; set; }
        public Nullable<int> REPEATASKING { get; set; }
        public Nullable<int> REPEATLANGUAGE { get; set; }
        public Nullable<int> REPEATBEHAVIOR { get; set; }
        public Nullable<int> VERBALATTACK { get; set; }
        public Nullable<int> BODYATTACK { get; set; }
        public Nullable<int> GETLOST { get; set; }
        public Nullable<int> ROAM { get; set; }
        public Nullable<int> SLEEPDISORDER { get; set; }
        public string REGNO { get; set; }
        public string ORGID { get; set; }
        public Nullable<bool> DELFLAG { get; set; }
        public Nullable<System.DateTime> DELDATE { get; set; }
        public Nullable<int> FORGETEAT { get; set; }
        public Nullable<int> REFUSALTOEAT { get; set; }
        public Nullable<int> EXPOSEDBODYPARTS { get; set; }
        public Nullable<int> NOTWEARCLOTHES { get; set; }
        public Nullable<int> INAPPROPRIATETOUCH { get; set; }
        public Nullable<int> COLLECTION { get; set; }
        public Nullable<int> IRRITABILITY { get; set; }
        public Nullable<int> COMPLAIN { get; set; }
        public Nullable<int> NOTCOOPERATE { get; set; }
        public Nullable<int> REFUSEHYGIENE { get; set; }
        public Nullable<int> NOINTEREST { get; set; }

    }
}
