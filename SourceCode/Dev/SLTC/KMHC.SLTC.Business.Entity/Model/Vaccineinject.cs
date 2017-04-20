using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
  public  class Vaccineinject
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public string ITEMTYPE { get; set; }
        public string STATE { get; set; }
        public string TRACESTATE { get; set; }
        public Nullable<System.DateTime> INJECTDATE { get; set; }
        public Nullable<int> INTERVAL { get; set; }
        public Nullable<System.DateTime> NEXTINJECTDATE { get; set; }
        public string DESCRIPTION { get; set; }
        public string OPERATOR { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public string NEXTOPERATEBY { get; set; }
        public string ORGID { get; set; }
    }
}
