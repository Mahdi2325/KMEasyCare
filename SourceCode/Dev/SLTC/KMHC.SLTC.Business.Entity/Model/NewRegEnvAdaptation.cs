using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NewRegEnvAdaptation
    {
        public long ID { get; set; }
        public Nullable<System.DateTime> INDATE { get; set; }
        public Nullable<System.DateTime> W1EVALDATE { get; set; }
        public Nullable<long> INFORMFLAG { get; set; }
        public Nullable<long> COMMFLAG { get; set; }
        public string INTERPERSONAL { get; set; }
        public string PARTICIPATION { get; set; }
        public string COORDINATION { get; set; }
        public string EMOTION { get; set; }
        public string RESISTANCE { get; set; }
        public string HELP { get; set; }
        public string PROCESSACTIVITY { get; set; }
        public string TRACEREC { get; set; }
       
        public Nullable<System.DateTime> W2EVALDATE { get; set; }
        public Nullable<System.DateTime> W3EVALDATE { get; set; }
        public Nullable<System.DateTime> W4EVALDATE { get; set; }
        public string EVALUATION { get; set; }
        public int? WEEK { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public string ORGID { get; set; }
    }
}





