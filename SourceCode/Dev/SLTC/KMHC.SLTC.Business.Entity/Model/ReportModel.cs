namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;

    public class ReportModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MajorType { get; set; }
        public string ReportType { get; set; }
        public string SysType { get; set; }
        public bool? Status { get; set; }
        public string OrgId { get; set; }
        public string FilterType { get; set; }
    }

    public class ReportSetModel
    {
        public string MajorType { get; set; }
        public List<ReportModel> Items { get; set; }
    }
}
