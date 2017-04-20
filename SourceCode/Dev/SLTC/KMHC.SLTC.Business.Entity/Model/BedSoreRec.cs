using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class BedSoreRec
    {
        public long Seq { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string Occurchance { get; set; }
        public Nullable<System.DateTime> OccurDate { get; set; }
        public string Degree { get; set; }
        public string EvaluteBy { get; set; }
        public string InspectionResult { get; set; }
        public Nullable<DateTime> RevoceryDate { get; set; }
        public string EventreView { get; set; }
        public bool RevoceryFlag { get; set; }
        public string RevoceryDesc { get; set; }
        public string Pict1 { get; set; }
        public string Pict2 { get; set; }
        public string Pict3 { get; set; }
        public string Pict4 { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }

        /// <summary>
        /// 评估者姓名
        /// </summary>
        public string  EvaluteNameBy { get; set; }

        public List<BedSoreChgrec> Detail { get; set; }
    }
}






