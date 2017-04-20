using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//字典项目表zhongyh
namespace KMHC.SLTC.Business.Entity
{
    public class DC_COMMDTLModel
    {
        public string ITEMCODE { get; set; }
        public string ITEMTYPE { get; set; }
        public string ITEMNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<int> ORDERSEQ { get; set; }
        public Nullable<System.DateTime> UPDATEDATE { get; set; }
        public string UPDATEBY { get; set; }
    }
}
