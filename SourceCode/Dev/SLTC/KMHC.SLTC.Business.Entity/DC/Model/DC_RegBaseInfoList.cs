using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DC_RegBaseInfoList
    {
        public long Id { get; set; }
        public DateTime? RecordDate { get; set; }
        public int Cnt { get; set; }
        public long FeeNo { get; set; }
        public string RegName { get; set; }
        public string ResidentNo { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Language { get; set; }
        public string Vs { get; set; }
        public string Job { get; set; }
        public string Religion { get; set; }
        public string MerryState { get; set; }
        public string Education { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Bmi { get; set; }
        public string WaistLine { get; set; }
        public string DiseaseHistory { get; set; }
        public string AdlScore { get; set; }
        public string IadlScore { get; set; }
        public string MmseScore { get; set; }
        public string GdsScore { get; set; }
        public string UpperDisorder { get; set; }
        public string LowerDisorder { get; set; }
        public string Aphasia { get; set; }
        public string VisuallyImpaired { get; set; }
        public string HearingImpaired { get; set; }
        public string FalseTeethu { get; set; }
        public string FalseTeethl { get; set; }
        public string NoteatFood { get; set; }
        public string Likefood { get; set; }
        public string Questionbehavior { get; set; }
        public DateTime? Checkdate { get; set; }
        public string Xray { get; set; }
        public string Syphilis { get; set; }
        public string Aids { get; set; }
        public string Hbsag { get; set; }
        public string AmibaDysentery { get; set; }
        public string InsectEgg { get; set; }
        public string BacillusDysentery { get; set; }
        public DateTime? NextCheckdate { get; set; }
        public string Medicine { get; set; }
        public string Nurseno { get; set; }
        public string Supervisor { get; set; }
        public string Director { get; set; }
        public string RegNo { get; set; }
        public string OrgId { get; set; }        
    }
}
