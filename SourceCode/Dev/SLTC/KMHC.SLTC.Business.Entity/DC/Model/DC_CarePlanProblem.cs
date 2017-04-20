using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class DC_CarePlanProblem
    {
        public int CpNo { get; set; }
        public string LevelPr { get; set; }
        public string ProblemType { get; set; }
        public string MajorType { get; set; }
        public string OrgId { get; set; }
    }

    public class DC_CarePlanActivity
    {
        public int CaNo { get; set; }
        public int CpNo { get; set; }
        public string Activity { get; set; }
        public string Action { get; set; }
    }

    public class DC_CarePlanDia
    {
        public int Id { get; set; }
        public int CpNo { get; set; }
        public string CpDia { get; set; }

    }

    /// <summary>
    /// 问题层面
    /// </summary>
    public class CAREPLANPROBLEM
    {
        public int CpNo { get; set; }
        public string LevelPr { get; set; }
        public string CategoryType { get; set; }
        public string DiaPr { get; set; }
        public string OrgId { get; set; }
    }

    /// <summary>
    /// 导因
    /// </summary>
    public class CAREPLANREASON
    {
        public int CrNo { get; set; }
        public int? CpNo { get; set; }
        public string DiaPr { get; set; }
        public string Causep { get; set; }
    }

    /// <summary>
    /// 特征
    /// </summary>
    public class CAREPLANDATA
    {
        public int CdNo { get; set; }
        public int? CpNo { get; set; }
        public string DiaPr { get; set; }
        public string Pr { get; set; }
        public string PrData { get; set; }
    }

    /// <summary>
    /// 目标
    /// </summary>
    public class CAREPLANGOAL
    {
        public int CgNo { get; set; }
        public int? CpNo { get; set; }
        public string DiaPr { get; set; }
        public string Goalp { get; set; }
    }

    /// <summary>
    /// 措施
    /// </summary>
    public class CAREPLANACTIVITY
    {
        public int CaNo { get; set; }
        public int? CpNo { get; set; }
        public string DiaPr { get; set; }
        public string Pr { get; set; }
        public string Activity { get; set; }
        public string Sysaction { get; set; }
    }

    /// <summary>
    /// 评值
    /// </summary>
    public class CAREPLANEVAL
    {
        public int CeNo { get; set; }
        public int? CpNo { get; set; }
        public string DiaPr { get; set; }
        public string Assessvalue { get; set; }

    }
}
