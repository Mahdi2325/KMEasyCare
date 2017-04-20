using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.PackageRelated
{
    public class CHARGEGROUP
    {
        //public CHARGEGROUP()
        //{
        //    this.CHARGEITEM = new HashSet<CHARGEITEM>();
           
        //}
    
        public string CHARGEGROUPID { get; set; }
        public string CHARGEGROUPNAME { get; set; }
        public string CHARGEGROUPDESC { get; set; }
        public string CHARGEGROUPPERIOD { get; set; }
        public string CHARGEGROUPPERIODNAME { get; set; }
        public bool CANAUTORENEW { get; set; }
        public int STATUS { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
        public string NSID { get; set; }
        //public virtual ICollection<CHARGEITEM> CHARGEITEM { get; set; }
        public  List<CHARGEITEM> CHARGEITEM { get; set; }
      
    }
}
