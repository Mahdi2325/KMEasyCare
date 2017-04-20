using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class HomeCareSuperviseModel
    {
        public long Id;
        public long? FeeNo;
        public int? RegNo;
        public DateTime? SuperviseDate;
        public string Operator;
        public string Supervisor;
        public string Contacttype;
        public int? Minutes;
        public string Supervisedesc;
        public string Assessment;
        public string Processdesc;
        public DateTime? CreateDate;
        public string CreateBy;
        public DateTime? ContactDate;
        public string OrgId;
    }
}
