using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Report
{
    public class ReportRequest
    {
        public long recId { get; set; }
        public long feeNo { get; set; }
        public long seqNo { get; set; }
        public int id { get; set; }

        public int year { get; set; }

        public int month { get; set; }

        public string feeName { get; set; }
        public int questionId { get; set; }
        public string type { get; set; }

        public string beginTime { get; set; }

        public string endTime { get; set; }
    }
}
