using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.PackageRelated
{
    public class CHARGEITEM
    {
        public int CGCRID { get; set; }
        public string MCDRUGCODE { get; set; }
        public string NSDRUGCODE { get; set; }
        public int DRUGRECORDID { get; set; }
        public Nullable<int> CGCIID { get; set; }
        public string CHARGEGROUPID { get; set; }
        public int CHARGEID { get; set; }
        public long OrderNo { get; set; }
        public string NAME { get; set; }
        public string ENNAME { get; set; }
        public string PINYIN { get; set; }
        public Nullable<int> FeeCode { get; set; }
        public Nullable<int> ITEMTYPE { get; set; }
        public int SERVICEID { get; set; }
        public string SERVICENAME { get; set; }

        public int MATERITALID { get; set; }
        public string MATERITALNAME { get; set; }

        public int DRUGID { get; set; }
        public string CNNAME { get; set; }
        public string MCCODE { get; set; }
        public string NSCODE { get; set; }
        public string NSID { get; set; }
        public string SPEC { get; set; }
        public string UNITS { get; set; }
        //转换比
        public Nullable<int> CONVERSIONRATIO { get; set; }
        //开药单位
        public string PRESCRIBEUNITS { get; set; }
        public decimal QTY { get; set; }
        public decimal ChargeQty { get; set; }
        public string FORM { get; set; }
        public string FERQ { get; set; }
        public decimal DOSAGE { get; set; }
        public string TAKEWAY { get; set; }
        public System.DateTime TAKETIME { get; set; }
        public decimal UNITPRICE { get; set; }
        public Nullable<decimal> COST { get; set; }
        public int CHARGEITEMID { get; set; }
        public string DRUGUSAGEMODE { get; set; }
        public Nullable<int> CHARGERECORDID { get; set; }
        public int CHARGEITEMTYPE { get; set; }
        public string CHARGEGROUPNAME { get; set; }
        public Nullable<int> CHARGERECORDTYPE { get; set; }
        public decimal FEEITEMCOUNT { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
        public bool ISCHARGEGROUPITEM { get; set; }
        public bool ISNCIITEM { get; set; }
        public string OPERATOR { get; set; }
        public Nullable<int> STATUS { get; set; }

        public virtual CHARGEGROUP CHARGEGROUP { get; set; }

        //住民ID
        public long FEENO { get; set; }
    }

    public class MainChargeItem
    {
        public string NSID { get; set; }
        public long FEENO { get; set; }
        public int CGCRID { get; set; }
        public string CHARGEGROUPID { get; set; }
        public string CHARGEGROUPNAME { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
        public IList<CHARGEITEM> ChargeItemList { get; set; }
    }
}
