using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report
{
    public class Answer
    {
        public int? Id { get; set; }

        public long SurveyId { get; set; }

        public decimal? Score { get; set; }

        public string Value { get; set; }
    }

    public class Survey
    {
        public long Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string NursingDesc { get; set; }
        public string NursingBy { get; set; }
        public string Result { get; set; }
        public decimal? Score { get; set; }
    }

    public class ExportResidentInfo
    {
        public int RegNo { get; set; }
        public string ResidentNo { get; set; }
        public string Name { get; set; }
        public string BedNo { get; set; }
        public string Sex { get; set; }
        public string Org { get; set; }
        public string Floor { get; set; }
        public string RoomNo { get; set; }
        public int? Age { get; set; }
        public decimal? Weight { get; set; }
        public Nullable<DateTime> BirthDate { get; set; }
        public Nullable<DateTime> InDate { get; set; }
    }

    public class ResidentInfo
    {
        public int RegNo { get; set; }
        public long FeeNo { get; set; }
        public string Name { get; set; }
        public string BedNo { get; set; }
        public string Sex { get; set; }
        public string Org { get; set; }
        public string Floor { get; set; }
        public string RoomNo { get; set; }
        public int? Age { get; set; }
        public decimal? Weight { get; set; }
        public string ResidengNo { get; set; }
        public string DiseaseDiag { get; set; }
    }
}
