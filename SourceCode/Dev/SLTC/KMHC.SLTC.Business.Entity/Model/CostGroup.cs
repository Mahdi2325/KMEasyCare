namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CostGroup
    {
        public int Id { get; set; }
        public string GroupNo { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; }
        public string OrgId { get; set; }

        public List<CostGroupDtl> GroupItems { get; set; }
    }
}
