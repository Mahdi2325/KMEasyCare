using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class DoctorCheckRecFilter
    {
        //住院序号
        public long FeeNo { get; set; }
        //病例号
        public int RegNo { get; set; }

        public long Id { get; set; }
    }
}

