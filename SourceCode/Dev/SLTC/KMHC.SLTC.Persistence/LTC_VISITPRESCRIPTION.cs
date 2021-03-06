//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KMHC.SLTC.Persistence
{
    using System;
    using System.Collections.Generic;
    
    public partial class LTC_VISITPRESCRIPTION
    {
        public int PID { get; set; }
        public Nullable<int> SEQNO { get; set; }
        public Nullable<int> MEDID { get; set; }
        public string DOSAGE { get; set; }
        public Nullable<decimal> QTY { get; set; }
        public string TAKEQTY { get; set; }
        public string TAKEWAY { get; set; }
        public string FREQ { get; set; }
        public string FREQTIME { get; set; }
        public Nullable<int> FREQDAY { get; set; }
        public Nullable<int> FREQQTY { get; set; }
        public Nullable<bool> LONGFLAG { get; set; }
        public Nullable<bool> USEFLAG { get; set; }
        public Nullable<System.DateTime> STARTDATE { get; set; }
        public Nullable<System.DateTime> ENDDATE { get; set; }
        public string DESCRIPTION { get; set; }
        public string ORGID { get; set; }
        public string NSID { get; set; }
        public string UNITS { get; set; }
        public decimal UNITPRICE { get; set; }
        public decimal TOTALQTY { get; set; }
        public decimal COST { get; set; }
        public string OPERATOR { get; set; }
        public string COMMENT { get; set; }
        public bool ISNCIITEM { get; set; }
        public bool ISCHARGEGROUPITEM { get; set; }
        public System.DateTime TAKETIME { get; set; }
        public string CNNAME { get; set; }
        public Nullable<int> DRUGID { get; set; }
        public long FEENO { get; set; }
    
        public virtual LTC_MEDICINE LTC_MEDICINE { get; set; }
        public virtual LTC_VISITDOCRECORDS LTC_VISITDOCRECORDS { get; set; }
    }
}
