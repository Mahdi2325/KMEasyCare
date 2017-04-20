using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class FeeRecord
    {
        public string FEERECORDID { get; set; }
        public string BILLID { get; set; }
        public Nullable<int> CHARGERECORDTYPE { get; set; }
        public Nullable<int> CHARGERECORDID { get; set; }

        public int CHARGEITEMID { get; set; }
        public long FEENO { get; set; }
        public string MCDRUGCODE { get; set; }
        public string INNERCODE { get; set; }
        public string VALUETYPE { get; set; }
        public string CHARGETYPEID { get; set; }
        public string CNNAME { get; set; }
        public string UNITS { get; set; }
        public decimal UNITPRICE { get; set; }
        public decimal COUNT { get; set; }
        public decimal COST { get; set; }
        public bool ISNCIITEM { get; set; }
        public Nullable<bool> ISCHARGEGROUPITEM { get; set; }
        public Nullable<bool> ISREFUNDRECORD { get; set; }
        public System.DateTime TAKETIME { get; set; }
        public string OPERATOR { get; set; }
        public string CREATEBY { get; set; }

        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    }
}
