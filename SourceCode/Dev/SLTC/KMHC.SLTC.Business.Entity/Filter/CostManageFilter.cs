using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public partial class BillFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public partial class CostDtlFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
        public int BillId { get; set; }
    }

    public partial class CostGroupFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
        public string GroupName { get; set; }
    }

    public partial class CostGroupDtlFilter
    {
        public int GroupId { get; set; }
    }

    public partial class CostItemFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
        public string CostItemNo { get; set; }
        public string CostName { get; set; }
    }

    public partial class CostItemCostGroupFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
    }

    public partial class FixedCostFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public partial class PayBillFilter
    {
        public int Id { get; set; }
        public int BillId { get; set; }
    }


    public partial class PinMoneyFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public partial class ReceiptsFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }

}





