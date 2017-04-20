using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class VitalsignFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }
    public class OutValueFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }
    public class InValueFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }
    }
    public class NursingRecFilter
    {
        public long Id { get; set; }
        public long? FeeNo { get; set; }
        public long? RegNo { get; set; }
        public Nullable<bool> PrintFlag { get; set; }
        public string Order { get; set; }
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
    }
    public class NursingHandoverFilter
    {
        public long? FeeNo { get; set; }
        public long? RegNo { get; set; }
    }
    public class AffairsHandoverFilter
    {
        public int Id { get; set; }
        public string RecorderName { get; set; }
    }
    public class AssignTaskFilter
    {
        public long? FeeNo { get; set; }
        public string OrgId { get; set; }

        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public string Assignee { get; set; }
        /// <summary>
        /// 是否已完成  
        /// </summary>
        public bool? RecStatus { get; set; }

        /// <summary>
        /// 查询状态
        /// </summary>
        public string TaskStatus { get; set; }

        /// <summary>
        /// 是否未读
        /// </summary>
        public bool? NewRecFlag { get; set; }

    }

    public class GroupActivityRecFilter
    {
        public string OrgId { get; set; }
        public string ActivityName { get; set; }
    }
}






