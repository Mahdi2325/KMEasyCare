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
    
    public partial class LTC_DOCTORCHECKREC
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<System.DateTime> CHECKDATE { get; set; }
        public string DEPTNO { get; set; }
        public string DOCNO { get; set; }
        public string CONSCIOUSNESS { get; set; }
        public string PHYSIOLOGY { get; set; }
        public Nullable<decimal> BODYTEMP { get; set; }
        public Nullable<int> PULSE { get; set; }
        public Nullable<int> BP { get; set; }
        public Nullable<int> BPH { get; set; }
        public Nullable<decimal> OXYGEN { get; set; }
        public Nullable<decimal> BS { get; set; }
        public string DISPOSITIONDESC { get; set; }
        public string OTHERDESC { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }
    }
}
