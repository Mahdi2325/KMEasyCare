using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Verify
    {
        public int Id { get; set; }
        public long FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public Nullable<int> SubsidyAmt { get; set; }
        public string SubsidyWay { get; set; }
        public string SubsidyUnit { get; set; }
        public Nullable<decimal> FeeRate { get; set; }
        public string ApplyDocNo { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public string ApproveDocNo { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string InsType { get; set; }
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string OtherAccountNo { get; set; }
        public Nullable<int> DepositBalance { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }

    }
}
