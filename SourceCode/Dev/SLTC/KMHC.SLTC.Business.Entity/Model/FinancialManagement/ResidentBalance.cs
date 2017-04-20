using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ResidentBalance
    {
        public string BalanceID { get; set; }
        public string Name { get; set; }
        public Nullable<long> FeeNO { get; set; }
        public decimal Deposit { get; set; }
        public decimal Blance { get; set; }

        public decimal PreAmount { get; set; }

        public decimal RefundAmount { get; set; }
        public Nullable<decimal> TotalPayment { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<decimal> TotalNCIPay { get; set; }
        public Nullable<decimal> TotalNCIOverspend { get; set; }
        public bool IsHaveNCI { get; set; }
        public System.DateTime CertStartTime { get; set; }
        public System.DateTime CertExpiredTime { get; set; }
        public Nullable<System.DateTime> ApplyHosTime { get; set; }
        public string CertNo { get; set; }
        public Nullable<decimal> NCIPayLevel { get; set; }
        public Nullable<decimal> NCIPayScale { get; set; }
        public int CertStatus { get; set; }
        public int Status { get; set; }
        public string Createby { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
