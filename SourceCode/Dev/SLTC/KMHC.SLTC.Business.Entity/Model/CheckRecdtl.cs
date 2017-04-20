using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
  public  class CheckRecdtl
    {

        public long ID { get; set; }
        public string CHECKTYPE { get; set; }
        public string CHECKITEM { get; set; }
        public string CHECKRESULTS { get; set; }
        public string DESCRIPTION { get; set; }
        public string EVALCOMBO { get; set; }
        public Nullable<long> RECORDID { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }

        //public virtual LTC_CHECKREC LTC_CHECKREC { get; set; }
    }
}
