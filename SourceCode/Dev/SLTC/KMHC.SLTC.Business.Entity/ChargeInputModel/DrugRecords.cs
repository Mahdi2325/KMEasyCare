using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.PackageRelated
{
   public class DrugRecords
    {
        public int DRUGRECORDID { get; set; }
        public int DRUGID { get; set; }
        public string NSID { get; set; }
        public long FEENO { get; set; }
        public string CNNAME { get; set; }
        public int CONVERSIONRATIO { get; set; }
        public string FORM { get; set; }
        public decimal DRUGQTY { get; set; }
        public string UNITS { get; set; }
        public decimal QTY { get; set; }
        public decimal UNITPRICE { get; set; }
        public decimal COST { get; set; }
        public decimal DOSAGE { get; set; }
        public string TAKEWAY { get; set; }
        public string FERQ { get; set; }
        public System.DateTime TAKETIME { get; set; }
        public string OPERATOR { get; set; }
        public string COMMENT { get; set; }
        public bool ISNCIITEM { get; set; }
        public bool ISCHARGEGROUPITEM { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
        public Nullable<int> CGCRID { get; set; }
        public string PRESCRIBEUNITS { get; set; }
    }
}
