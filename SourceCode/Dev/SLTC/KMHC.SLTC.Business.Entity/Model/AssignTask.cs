using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class AssignTask
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> AssignDate { get; set; }
        public string AssignedBy { get; set; }
        public string EMPNAME { get; set; }
        public string AssignedName { get; set; }
        public string Assignee { get; set; }
        public string AssignName { get; set; }
        public string Content { get; set; }
        public Nullable<bool> RecStatus { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string UnFinishReason { get; set; }
        public Nullable<System.DateTime> PerformDate { get; set; }
        public string ClassType { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public string Url { get; set; }
        public Nullable<bool> AutoFlag { get; set; }
        public Nullable<bool> NewrecFlag { get; set; }
        public string OrgId { get; set; }

        #region 扩展属性 Add By Duke
        public string ResidentName { get; set; }
        #endregion
    }
    public class AssignTask2
    {
        public string KEY { get; set; }
        public DateTime? NEXTEVALDATE { get; set; }
        public string NEXTEVALUATEBY { get; set; }
        public long FEENO { get; set; }

    }

    public class TaskInfoTmp
    {
        /// <summary>
        /// 触发照会模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 触发照会内容
        /// </summary>
        public string Content { get; set; }
    }
    public enum AutoTaskType
    {
        ADL,//巴氏量表
        MMSE,
        SPMSQ,
        IADL,
        SORE,
        FALL,
        KARNOFSKY,
        BEHAVIOR,
        NursingDemand,
        GDS,
        NOS, //社会网络评估
        EvaluateAdd //社工评估
    };
    /// <summary>
    /// 系统生成照会提醒模板 
    /// </summary>
    public class AutoTaskTmp
    {
        public static Dictionary<string, TaskInfoTmp> AutoTask = new Dictionary<string, TaskInfoTmp>{
          {AutoTaskType.ADL.ToString(),new TaskInfoTmp(){ModuleName="ADL评估",Content="ADL评估(此提醒由系统生成)"}},
          {AutoTaskType.MMSE.ToString(),new TaskInfoTmp(){ModuleName="MMSE评估",Content="MMSE评估(此提醒由系统生成)"}},
          {AutoTaskType.SPMSQ.ToString(),new TaskInfoTmp(){ModuleName="SPMSQ评估",Content="简易心智量表评估(此提醒由系统生成)"}},
          {AutoTaskType.IADL.ToString(),new TaskInfoTmp(){ModuleName="IADL评估",Content="IADL评估(此提醒由系统生成)"}},
          {AutoTaskType.SORE.ToString(),new TaskInfoTmp(){ModuleName="SORE评估",Content="压疮风险评估(此提醒由系统生成)"}},
          {AutoTaskType.FALL.ToString(),new TaskInfoTmp(){ModuleName="FALL评估",Content="跌倒危险因数评估(此提醒由系统生成)"}},
          {AutoTaskType.KARNOFSKY.ToString(),new TaskInfoTmp(){ModuleName="KARNOFSKY评估",Content="柯氏量表评估(此提醒由系统生成)"}},
          {AutoTaskType.BEHAVIOR.ToString(),new TaskInfoTmp(){ModuleName="BEHAVIOR评估",Content="行为认知评估(此提醒由系统生成)"}},
          {AutoTaskType.NursingDemand.ToString(),new TaskInfoTmp(){ModuleName="NursingDemand评估",Content="护理需求评估(此提醒由系统生成)"}},
           {AutoTaskType.GDS.ToString(),new TaskInfoTmp(){ModuleName="忧郁量表评估",Content="忧郁量表评估(此提醒由系统生成)"}},
           {AutoTaskType.NOS.ToString(),new TaskInfoTmp(){ModuleName="社会网络评估",Content="社会网络评估(此提醒由系统生成)"}},
            {AutoTaskType.EvaluateAdd.ToString(),new TaskInfoTmp(){ModuleName="社工评估",Content="社工评估(此提醒由系统生成)"}}
      };

    }

}






