using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.PackageRelated
{
    public class RESCHARGEGRO
    {
        public int CGRID { get; set; }
        public string CHARGEGROUPID { get; set; }
        public string CHARGEGROUPNAME { get; set; }
        public long FEENO { get; set; }
        public Nullable<System.DateTime> CURBEGINDATE { get; set; }
        public Nullable<System.DateTime> CURENDDATE { get; set; }
        public Nullable<System.DateTime> OVERALLBEGINDATE { get; set; }
        public Nullable<System.DateTime> OVERALLENDDATE { get; set; }
        public bool CANAUTORENEW { get; set; }
        public int STATUS { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
        //扩展属性
        public string CHARGEGROUPPERIOD { get; set; }
    }
}
