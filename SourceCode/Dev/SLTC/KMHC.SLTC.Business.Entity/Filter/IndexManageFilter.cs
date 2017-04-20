using System;
namespace KMHC.SLTC.Business.Entity.Filter
{
    public class BedSoreRecFilter
    {
        public Nullable<long> FeeNo { get; set; }
        public string OrgId { get; set; }
        public string Degree { get; set; }
    }

    public class BedSoreChgrecFilter
    {
        public Nullable<long> Seq { get; set; }
    }

    public class ConstraintRecFilter
    {
        public long FeeNo { get; set; }
        public string OrgId { get; set; }
    }

    public class InfectionIndFilter
    {

        public string OrgId { get; set; }

        public long RegNo { get; set; }

        public long FeeNo { get; set; }
    }

    public partial class InfectionSympotmFilter
    {
        public long? SeqNo { get; set; }
        public string OrgId { get; set; }
    }

    public partial class InfectionItemFilter
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }

    public partial class SymptomItemFilter
    {
        public string Id { get; set; }
        public string SympotmCode { get; set; }
        public string SympotmName { get; set; }
        public string ItemCode { get; set; }
    }

    public partial class LabExamRecFilter
    {
        public long? SeqNo { get; set; }
        public string OrgId { get; set; }
    }
    public class PainEvalRecFilter
    {
        public long FeeNo { get; set; }
        public string OrgId { get; set; }
    }


    public class PainBodyPartRecFilter
    {
        public Nullable<long> SeqNo { get; set; }
    }

    public class ConstrainsBevalFilter
    {
        public Nullable<long> SeqNo { get; set; }
    }

    public class UnPlanWeightIndFilter
    {

        public string OrgId { get; set; }

        public long RegNo { get; set; }

        public long FeeNo { get; set; }
        public string FloorName { get; set; }
        public string RoomName { get; set; }

		public DateTime? ThisRecDate1 { get; set; }
		public DateTime? ThisRecDate2 { get; set; }
    }

    public class UnPlanEdipdFilter
    {

        public string OrgId { get; set; }

        public long RegNo { get; set; }

        public long FeeNo { get; set; }
    }
}





