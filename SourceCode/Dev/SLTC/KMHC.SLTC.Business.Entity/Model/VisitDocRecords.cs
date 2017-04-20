using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class VisitDocRecords
    {
        public  VisitDocRecords()
        {
            this.VisitPrescription = new HashSet<VisitPrescription>();
        }

        #region 基本属性
        public int SeqNo { get; set; }
        public Nullable<System.DateTime> VisitDate { get; set; }
        public string VisitType { get; set; }
        public string RecordBy { get; set; }
        public string VisitHosp { get; set; }
        public string VisitDept { get; set; }
        public string VisitDoctor { get; set; }
        public string VisitReason { get; set; }
        public Nullable<bool> InfectFlag { get; set; }
        public Nullable<bool> PlanvisitFlag { get; set; }
        public int TakeDays { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string DiseaseType { get; set; }
        public string DiseaseName { get; set; }
        public string Symptoms { get; set; }
        public string Objectivedescrip { get; set; }
        public string Diagnosticeval { get; set; }
        public string Treatment { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Nextvisitflag { get; set; }
        public string Nextvisithint { get; set; }
        public string Prereginfo { get; set; }
        public int Intervalday { get; set; }
        public Nullable<System.DateTime> Nextvisitdate { get; set; }
        public string Nextvisittype { get; set; }
        public string OrgId { get; set; }
        public string Infectvisitreason { get; set; }
        public long FeeNo { get; set; }
        public int RegNo { get; set; }
        #endregion

        #region 扩展属性
        public string RecordNameBy { get; set; }
        public string VisitHospName { get; set; }
        public string VisitDeptName { get; set; }
        public string VisitDoctorName { get; set; }
        #endregion

        public virtual ICollection<VisitPrescription> VisitPrescription { get; set; }
    }

    public class VisitHospital
    {
        public VisitHospital()
        {
            this.VisitDept = new HashSet<VisitDept>();
            this.VisitDoctor = new HashSet<VisitDoctor>();
        }
        public string HospNo { get; set; }
        public string HospName { get; set; }
        public string OrgId { get; set; }
        public virtual ICollection<VisitDept> VisitDept { get; set; }
        public virtual ICollection<VisitDoctor> VisitDoctor { get; set; }

    }

    public class VisitDept
    {
        public string DeptNo { get; set; }
        public string DeptName { get; set; }
        public string HospNo { get; set; }
    }

    public class VisitDoctor
    {
        public string DocNo { get; set; }
        public string DocName { get; set; }
        public string HospNo { get; set; }
        public string DeptNo { get; set; }
    }

    public class Icd9_Disease
    {
        public string IcdCode { get; set; }
        public bool Status { get; set; }
        public string EngName { get; set; }
    }

}

