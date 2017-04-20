using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class LTC_UserLoginLog
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public string Ip { get; set; }
        public Nullable<System.DateTime> LoginTime { get; set; }
    }
}
