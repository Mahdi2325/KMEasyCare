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
    
    public partial class LTC_NCIEVALUATE
    {
        public LTC_NCIEVALUATE()
        {
            this.LTC_NCIEVALUATEDTL = new HashSet<LTC_NCIEVALUATEDTL>();
        }
    
        public int NCIEVALUATEID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string NAME { get; set; }
        public string SSNO { get; set; }
        public Nullable<System.DateTime> STARTTIME { get; set; }
        public string RESIDENTNO { get; set; }
        public string BEDNO { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public string UPDATEBY { get; set; }
    
        public virtual ICollection<LTC_NCIEVALUATEDTL> LTC_NCIEVALUATEDTL { get; set; }
    }
}
