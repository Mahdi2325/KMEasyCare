using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class CheckItem
    {
        public string ITEMCODE { get; set; }
        public string ITEMNAME { get; set; }
        public string NORMALVALUE { get; set; }
        public string LOWBOUND { get; set; }
        public string UPBOUND { get; set; }
        public string DESCRIPTION { get; set; }
        public string TYPECODE { get; set; }
        public string GROUPCODE { get; set; }

      
    }
}
