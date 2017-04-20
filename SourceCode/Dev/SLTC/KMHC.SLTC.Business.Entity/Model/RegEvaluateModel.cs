using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RegEvaluateModel
    {
        public long? FeeNo { get; set; }
        public int? RegNo { get; set; }
        public DateTime? EvalDate { get; set; }
        public DateTime? NextEvalDate { get; set; }
        public int? Count { get; set; }
        public long Id { get; set; }
        public string EvaluateBy { get; set; }
        public string NextEvaluateBy { get; set; }
        /// <summary>
        /// 使用辅具
        /// </summary>
        public string AidTools { get; set; }
        /// <summary>
        /// 手册障碍类别
        /// </summary>
        public string BookType { get; set; }
        /// <summary>
        /// 手册障碍等级
        /// </summary>
        public string BookDegree { get; set; }
        /// <summary>
        /// 重大伤病卡名称
        /// </summary>
        public string IllCardName { get; set; }
        /// <summary>
        /// 曾接受相关服务
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 意识状态
        /// </summary>
        public string MindState { get; set; }
        /// <summary>
        /// 认知状况
        /// </summary>
        public string CognitiveState { get; set; }
        /// <summary>
        /// 口语表达能力
        /// </summary>
        public string ExpressionState { get; set; }
        /// <summary>
        /// 非口语表达能力
        /// </summary>
        public string NonexpressionState { get; set; }
        /// <summary>
        /// 语言理解能力
        /// </summary>
        public string LanguageState { get; set; }
        /// <summary>
        /// 情绪状况
        /// </summary>
        public string EmotionState { get; set; }
        /// <summary>
        /// 人格
        /// </summary>
        public string Personality { get; set; }
        /// <summary>
        /// 注意力
        /// </summary>
        public string Attention { get; set; }
        /// <summary>
        /// 现实感
        /// </summary>
        public string Realisticsense { get; set; }
        /// <summary>
        /// 社交参与度
        /// </summary>
        public string SocialParticipation { get; set; }
        /// <summary>
        /// 社交态度
        /// </summary>
        public string SocialAttitude { get; set; }
        /// <summary>
        /// 社交能力
        /// </summary>
        public string SocialSkills { get; set; }
        /// <summary>
        /// 沟通技巧
        /// </summary>
        public string CommSkills { get; set; }
        /// <summary>
        /// 应变能力
        /// </summary>
        public string ResponseSkills { get; set; }
        /// <summary>
        /// 解决问题能力
        /// </summary>
        public string FixissueSkills { get; set; }
        /// <summary>
        /// 个人兴趣嗜好
        /// </summary>
        public string Hobby { get; set; }
        /// <summary>
        /// 个人专长
        /// </summary>
        public string Expertise { get; set; }
        /// <summary>
        /// 特殊行为
        /// </summary>
        public string SpecialBehavior { get; set; }
        /// <summary>
        /// 入住原因摘要
        /// </summary>
        public string AdmissionReason { get; set; }
        /// <summary>
        /// 过去病史
        /// </summary>
        public string MedicalHistory { get; set; }
        /// <summary>
        /// 心理层面概述
        /// </summary>
        public string PsychologyDesc { get; set; }
        /// <summary>
        /// 家庭评估-家庭系统评估
        /// </summary>
        public string FamilySubSystem { get; set; }
        /// <summary>
        /// 家庭评估-个案对家庭的功能或角色
        /// </summary>
        public string FamilyFunRole { get; set; }
        /// <summary>
        /// 家庭评估-家属对个案之期待及支持度评估
        /// </summary>
        public string FamilySupport { get; set; }
        /// <summary>
        /// 家庭评估－家庭经济能力评估
        /// </summary>
        public string FamilyFinancial { get; set; }
        /// <summary>
        /// 社会资源评估-人际关系
        /// </summary>
        public string SocialRelationShip { get; set; }
        /// <summary>
        /// 社会资源评估－社区支持系统
        /// </summary>
        public string SocialSupport { get; set; }
        /// <summary>
        /// 社会资源评估－正式资源
        /// </summary>
        public string SocialFormalResource { get; set; }
        /// <summary>
        /// 主要问题
        /// </summary>
        public string KeyQuestion { get; set; }
        /// <summary>
        /// 社工处遇计划－短期目标　
        /// </summary>
        public string ProcessPlan_S { get; set; }
        /// <summary>
        /// 社工处遇计划－中期目标　
        /// </summary>
        public string ProcessPlan_M { get; set; }
        /// <summary>
        /// 社工处遇计划－长期目标　
        /// </summary>
        public string ProcessPlan_L { get; set; }
        /// <summary>
        /// 与家庭讨论服务计划
        /// </summary>
        public string RelativesDiscuss { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 记录创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 记录创建人
        /// </summary>
        public string CreateBy { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string VerifyBy { get; set; }
        /// <summary>
        /// 评估人员姓名
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgId { get; set; }
    }
}

