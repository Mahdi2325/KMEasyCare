using System;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ConsultRec
    {
        public long Id { get; set; }
        public string ConsultantName { get; set; }
        public DateTime? ConsultTime { get; set; }
        public string ConsultantPhone { get; set; }
        public string Appellation { get; set; }
        public string OldManName { get; set; }
        public string OldManSex { get; set; }
        public int? OldManAge { get; set; }
        public string Channel { get; set; }
        public string ReservationBed { get; set; }
        public string EarnestStatus { get; set; }
        public float? EarnestAmount { get; set; }
        public string RecordBy { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? CallBackTime { get; set; }
    }
}
