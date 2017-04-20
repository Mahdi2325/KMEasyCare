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
    
    public partial class NCI_NURSINGHOME
    {
        public NCI_NURSINGHOME()
        {
            this.NCI_NURSINGHOMESTAFF = new HashSet<NCI_NURSINGHOMESTAFF>();
            this.NCIA_APPCERT = new HashSet<NCIA_APPCERT>();
            this.NCIA_CARETYPECONDITION = new HashSet<NCIA_CARETYPECONDITION>();
            this.NCIA_NSAPPCERTRATE = new HashSet<NCIA_NSAPPCERTRATE>();
            this.NCIA_NSAPPHOSPRATE = new HashSet<NCIA_NSAPPHOSPRATE>();
            this.NCI_CARETYPE = new HashSet<NCI_CARETYPE>();
        }
    
        public string NSID { get; set; }
        public string NSNO { get; set; }
        public string GOVID { get; set; }
        public string NSNAME { get; set; }
        public string NSTYPE { get; set; }
        public string ADDRESS { get; set; }
        public string CONTACTOR { get; set; }
        public string PHONE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    
        public virtual NCI_GOVERNMENT NCI_GOVERNMENT { get; set; }
        public virtual ICollection<NCI_NURSINGHOMESTAFF> NCI_NURSINGHOMESTAFF { get; set; }
        public virtual ICollection<NCIA_APPCERT> NCIA_APPCERT { get; set; }
        public virtual ICollection<NCIA_CARETYPECONDITION> NCIA_CARETYPECONDITION { get; set; }
        public virtual ICollection<NCIA_NSAPPCERTRATE> NCIA_NSAPPCERTRATE { get; set; }
        public virtual ICollection<NCIA_NSAPPHOSPRATE> NCIA_NSAPPHOSPRATE { get; set; }
        public virtual ICollection<NCI_CARETYPE> NCI_CARETYPE { get; set; }
    }
}