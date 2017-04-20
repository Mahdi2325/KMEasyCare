using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_IpdRegModel
    {
        public long FeeNo { get; set; }
        public string RegNo { get; set; }
        public DateTime? InDate { get; set; }
        public string StationCode { get; set; }
        public string IpdFlag { get; set; }
        public string ResidentNo { get; set; }
        public string SocialWorker { get; set; }
        public string NurseAides { get; set; }
        public string NurseNo { get; set; }
        public bool? CloseFlag { get; set; }
        public string CloseReason { get; set; }
        public DateTime? OutDate { get; set; }
        public string ProvideService { get; set; }
        public string SvrContent { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CheckedBy { get; set; }
        public string OrgId { get; set; }

        public string Org { get; set; }
        public string isAdd { get; set; }

        public string RegName { get; set; }
        public string Phone { get; set; }
        public string IdNo { get; set; }
        public string Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string LivingAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string MerryState { get; set; }
        public DateTime? PrintDate { get; set; }
    }
}
