using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class CodeFile
    {
        public string ITEMTYPE { get; set; }
        public string TYPENAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string MODIFYFLAG { get; set; }
        public Nullable<System.DateTime> UPDATEDATE { get; set; }
        public string UPDATEBY { get; set; }
    }
}
