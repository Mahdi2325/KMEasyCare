using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class NewResideEntenvRecFilter
    {
        public long ID { get; set; }
        public string RESIDENGNO { get; set; } 
        public Nullable<System.DateTime> INDATE { get; set; } 
        public Nullable<bool> BELLFLAG { get; set; } 
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public string ORGID { get; set; }

    }
    
}





