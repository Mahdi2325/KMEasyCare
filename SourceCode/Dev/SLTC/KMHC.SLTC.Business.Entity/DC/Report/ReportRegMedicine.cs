using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Report
{
    public class ReportRegMedicine
    {
        public string OrgName { get; set; } 
        public long Id { get; set; }
        public long FeeNo { get; set; }
        public string RegNo { get; set; }
        public string RegName { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ResidentNo { get; set; }
        //药品名称
        public string MedicineName { get; set; }
        //开药医院
        public string HospitalName { get; set; }
        //科别
        public string DeptName { get; set; }
        //剂量
        public string TakeQty { get; set; }
        //服药途径
        public string TakeProc { get; set; }
        //用药时间
        public string TakeDateTime { get; set; }
        //使用期限-开始时间
        public DateTime? StartDate { get; set; }
        //使用期限-结束时间
        public DateTime? EndDate { get; set; }
        //停用时间
        public DateTime? BreakDate { get; set; }
        //停用原因
        public string BreakReason { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string OrgId { get; set; }
    }
}

