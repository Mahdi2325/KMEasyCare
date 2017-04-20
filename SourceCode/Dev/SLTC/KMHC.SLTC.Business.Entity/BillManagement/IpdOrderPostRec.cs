using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.BillManagement
{
    public class IpdOrderPostRec
    {
        public long OrderPostRecNo { get; set; }
        public Nullable<long> OrderNo { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string NurseNo { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
