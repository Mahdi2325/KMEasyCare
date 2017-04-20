using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Rehabilitrec
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<System.DateTime> RECDATE { get; set; }
        public Nullable<int> INTERVALDAY { get; set; }
        public string RECORDBY { get; set; }
        public string NEXTRECORDBY { get; set; }
        public string HOSPNAME { get; set; }
        public string ITEMNAME { get; set; }
        public string ASSESSMENT { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> NEXTRECDATE { get; set; }
        public string ORGID { get; set; }

    }
}
