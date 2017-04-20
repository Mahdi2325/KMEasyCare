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
    
    public partial class NCI_USER
    {
        public NCI_USER()
        {
            this.NCI_AGENCYSTAFF = new HashSet<NCI_AGENCYSTAFF>();
            this.NCI_GOVERNMENTSTAFF = new HashSet<NCI_GOVERNMENTSTAFF>();
            this.NCI_INSURANCECOMPANYSTAFF = new HashSet<NCI_INSURANCECOMPANYSTAFF>();
            this.NCI_NURSINGHOMESTAFF = new HashSet<NCI_NURSINGHOMESTAFF>();
        }
    
        public int USERID { get; set; }
        public Nullable<int> PARENTUSERID { get; set; }
        public string ROLEID { get; set; }
        public int ORGTYPE { get; set; }
        public string ORGID { get; set; }
        public string ACCOUNT { get; set; }
        public string PASSWORD { get; set; }
        public string USERNAME { get; set; }
        public string EMAIL { get; set; }
        public string IDNO { get; set; }
        public int STATUS { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    
        public virtual ICollection<NCI_AGENCYSTAFF> NCI_AGENCYSTAFF { get; set; }
        public virtual ICollection<NCI_GOVERNMENTSTAFF> NCI_GOVERNMENTSTAFF { get; set; }
        public virtual ICollection<NCI_INSURANCECOMPANYSTAFF> NCI_INSURANCECOMPANYSTAFF { get; set; }
        public virtual ICollection<NCI_NURSINGHOMESTAFF> NCI_NURSINGHOMESTAFF { get; set; }
        public virtual NCI_ROLE NCI_ROLE { get; set; }
    }
}