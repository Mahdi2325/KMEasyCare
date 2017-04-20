using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.PackageRelated
{
    public class CHARGERECORD
    {
        public int CGCRID { get; set; }
        public string CHARGEGROUPID { get; set; }
        public long RESIDENTID { get; set; }
        public Nullable<int> CHARGERECORDTYPE { get; set; }
        public Nullable<int> CHARGERECORDID { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
    }
}
