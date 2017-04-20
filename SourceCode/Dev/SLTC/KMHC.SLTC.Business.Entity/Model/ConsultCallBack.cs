using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ConsultCallBack
    {
        public long Id { get; set; }
        public long? ConsultRecId { get; set; }
        public DateTime? CallBackTime { get; set; }
        public string Interviewee { get; set; }
        public string InterviewRec { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
