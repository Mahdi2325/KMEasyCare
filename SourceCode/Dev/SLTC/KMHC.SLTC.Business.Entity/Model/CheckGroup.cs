using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class CheckGroup
    {
        public string GROUPCODE { get; set; }
        public string GROUPNAME { get; set; }


        //public virtual ICollection<LTC_CHECKITEM> LTC_CHECKITEM { get; set; }
    }
}
