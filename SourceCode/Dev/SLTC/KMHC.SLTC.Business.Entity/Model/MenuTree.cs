using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class MenuTree
    {
        public string name { get; set; }

        public string url { get; set; }

        public bool status { get; set; }

        public IEnumerable<MenuTree> nodes { get; set; }

    }
}
