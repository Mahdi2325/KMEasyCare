using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class VisitDocRecordsFilter
    {
        /// <summary>
        /// 住院序号
        /// </summary>
        public long? FeeNo { get; set; }
    }

    public class VisitHospitalFilter
    {
        public string OrgId { get; set; }
    }

    public class VisitDeptFilter
    {
        public string HospNo { get; set; }
    }

    public class VisitDoctorFilter
    {
        public string HospNo { get; set; }
        public string DeptNo { get; set; }
    }
    public class Icd9_DiseaseFilter
    {
        public string KeyWord { get; set; }
    }
    public class FreqFilter
    {
        public string KeyWord { get; set; }
    }
}

