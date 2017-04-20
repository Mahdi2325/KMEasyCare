using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class CaresvrRecModel
    {
        public long Id { get; set; }
        public long? FeeNo { get; set; }
        public int? RegNo { get; set; }
        public string Carer { get; set; }
        public string CarerName { get; set; }
        public DateTime? RecDate { get; set; }
        public string SvrAddress { get; set; }
        public string SvrType { get; set; }
        public string RelationType { get; set; }
        public string SvrFocus { get; set; }
        public string QuestionDesc { get; set; }
        public string TreatDesc { get; set; }
        public string EvalStatus { get; set; }
        public int? EvalMinutes { get; set; }
        public string EvalDesc { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
        public string SvrPeople { get; set; }
        public string QuestionLevel { get; set; }
        public string QuestionFocus { get; set; }
        public string ProcessActivity { get; set; }
    }
}
