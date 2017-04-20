using KMHC.SLTC.Business.Entity.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class DcNurseingPlanEval
    {
        public long Id { get; set; }
        public long FeeNo { get; set; }
        public string RegNo { get; set; }
        public string RegName { get; set; }
        public string ResidentNo { get; set; }
        public string Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? FirstevalDate { get; set; }
        public int EvalNumber { get; set; }
        public DateTime? EvalDate { get; set; }
        public DateTime? InDate { get; set; }
        //紧急送医医院
        public string HospitalName { get; set; }
        //家属签名
        public string FamilySignature { get; set; }
        //疾病史
        public string DiseaseInfo { get; set; }
        //手术情形
        public string OperationInfo { get; set; }
        //目前服药情形
        //public string MedicineInfo { get; set; }
        public bool? Chinesedrugflag { get; set; }
        public string Chinesedrug { get; set; }
        public bool? Westerndrugflag { get; set; }
        public string Westerndrug { get; set; }  
        //就医医院
        public string VisitHospitalName { get; set; }
        //就医方式
        public string VisitType { get; set; }
        //每月就医次数
        public int VisitNumber { get; set; }
        //心率
        public string HeartRate { get; set; }
        //呼吸形态
        public string BreathType { get; set; }
        //身高
        public int Height { get; set; }
        //体重
        public decimal Weight { get; set; }
        //腰围
        public decimal WaistLine { get; set; }
        //BMI
        public decimal BMI { get; set; }
        //理想体重
        public decimal IdealWeight { get; set; }
        //食欲
        public string Appetite { get; set; }
        //饮食习惯
        public string FoodHabit { get; set; }
        //饮食种类
        public string FoodType { get; set; }
        //进食的形式
        public string EatType { get; set; }
        //牙齿状况
        public string TeethState { get; set; }
        //牙龈状况
        public string GumsState { get; set; }
        //口腔黏膜
        public string Oralmucosa { get; set; }
        //吞咽能力
        public string SwallowingAbility { get; set; }
        //咀嚼能力
        public string MasticatoryAbility { get; set; }
        //主食
        public string StapleFood { get; set; }
        //肉类
        public string Meat { get; set; }
        //菜类
        public string Vegetables { get; set; }
        //点心
        public string Snack { get; set; }
        //汤品
        public string Soup { get; set; }
        //皮肤外观
        public string SkinState { get; set; }
        //皮肤颜色
        public string SkinColor { get; set; }
        //水肿
        public string Edema { get; set; }
        //皮肤完整性
        public string SkinIntegrity { get; set; }
        //不完整部分
        public string SkinPart { get; set; }
        //不完整-大小
        public string SkinSize { get; set; }
        //不完整-级数
        public int SkinLevel { get; set; }
        //排便
        public string Defecation { get; set; }
        //大便次数
        public int ShitNumber { get; set; }
        //大便量
        public string ShitAmOut { get; set; }
        //大便性状
        public string ShitNature { get; set; }
        //辅助排便方式
        public string AssistedDefecation { get; set; }
        //肠蠕动
        public string IntestinalPeristalsis { get; set; }
        //排尿情形
        public string Micturition { get; set; }
        //小便性状
        public string UrinationNature { get; set; }
        //小便失禁
        public bool AconuresisFlag { get; set; }
        //有小便失禁-- 描述
        public string AconuresisInfo { get; set; }
        //排尿处置
        public string UrinationDisposal { get; set; }
        //肌力 右上
        public string RightMuscle1 { get; set; }
        //肌力 右下
        public string RightMuscle2 { get; set; }
        //肌力 左上
        public string LeftMuscle1 { get; set; }
        //肌力 左下
        public string LeftMuscle2 { get; set; }
        //肌肉关节 右上
        public string RightJoint1 { get; set; }
        //肌肉关节 右下
        public string RightJoint2 { get; set; }
        //肌肉关节 左上
        public string LeftJoint1 { get; set; }
        //肌肉关节 左下
        public string LeftJoint2 { get; set; }
        //步态
        public string Gait { get; set; }
        //辅助工具
        public string AssistantTool { get; set; }
        //安全性
        public string AssistantSecurity { get; set; }
        //舒适性
        public string AssistantSuitability { get; set; }
        //日常休闲活动内容
        public string ActivityName { get; set; }
        //跌倒情形
        public string FallInfo { get; set; }
        //近一年内有否跌倒
        public bool? Fall1Year { get; set; }
        //是否受伤
        public bool? Injuredflag { get; set; }
        //受伤部位
        public string Injuredpart { get; set; } 
        //过去跌倒情况
        public string PastFallInfo { get; set; }
        //当时的受伤和处理情形
        public string InjuryDisposalInfo { get; set; }
        //频率
        public string PainFreq { get; set; }
        //疼痛性质
        public string PainLevel { get; set; }
        //疼痛部位
        public string PainPart { get; set; }
        //性质
        public string PainNature { get; set; }
        //持续时间
        public string DurationTime { get; set; }
        //减轻疼痛的方法
        public string EasePainMethod { get; set; }
        //认知感受-视力状况
        public string VisualAcuity { get; set; }
        //认知感受-辅助物
        public string AssistantTools { get; set; }
        //认知感受- 听力状况
        public string ListeningState { get; set; }
        //
        public int Interval { get; set; }
        //
        public DateTime? Nextevaldate { get; set; }
        //精神社会行为- 妄想
        public string Delusion { get; set; }
        //精神社会行为-外观
        public string PersonImage { get; set; }
        


        public string Attitude { get; set; }
        //精神社会行为-情绪
        public string EmotionState { get; set; }
        //精神社会行为-常引发不安的情景
        public string DisturbingEnv { get; set; }
        //精神社会行为-有效安抚情绪的方式
        public string SootheMotion { get; set; }
        //精神社会行为-行为
        public string Behavior { get; set; }
        //精神社会行为- 沟通方式
        public string CommunicationType { get; set; }
        //精神社会行为- 沟通能力
        public string CommunicationSkill { get; set; }
        //精神社会行为- 问题行为
        public string ProblemBehavior { get; set; }
        //机构Id
        public string OrgId { get; set; }
        //
        public DateTime? DelDate { get; set; }
        //
        public string DelFlag { get; set; }
    }

    public class DC_NursingPlanEval
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNO { get; set; }
        public string REGNAME { get; set; }
        public string RESIDENTNO { get; set; }
        public string SEX { get; set; }
        public Nullable<System.DateTime> BIRTHDATE { get; set; }
        public Nullable<System.DateTime> FIRSTEVALDATE { get; set; }
        public Nullable<int> EVALNUMBER { get; set; }
        public Nullable<System.DateTime> EVALDATE { get; set; }
        public Nullable<System.DateTime> INDATE { get; set; }
        public string HOSPITALNAME { get; set; }
        public string FAMILYSIGNATURE { get; set; }
        public string DISEASEINFO { get; set; }
        public string OPERATIONINFO { get; set; }
        public Nullable<bool> CHINESEDRUGFLAG { get; set; }
        public string CHINESEDRUG { get; set; }
        public Nullable<bool> WESTERNDRUGFLAG { get; set; }
        public string WESTERNDRUG { get; set; }
        public string VISITHOSPITALNAME { get; set; }
        public string VISITTYPE { get; set; }
        public Nullable<int> VISITNUMBER { get; set; }
        public string HEARTRATE { get; set; }
        public string BREATHTYPE { get; set; }
        public Nullable<int> HEIGHT { get; set; }
        public Nullable<decimal> WEIGHT { get; set; }
        public Nullable<decimal> WAISTLINE { get; set; }
        public Nullable<decimal> BMI { get; set; }
        public Nullable<decimal> IDEALWEIGHT { get; set; }
        public string APPETITE { get; set; }
        public string FOODHABIT { get; set; }
        public string FOODTYPE { get; set; }
        public string EATTYPE { get; set; }
        public string TEETHSTATE { get; set; }
        public string GUMSSTATE { get; set; }
        public string ORALMUCOSA { get; set; }
        public string SWALLOWINGABILITY { get; set; }
        public string MASTICATORYABILITY { get; set; }
        public string STAPLEFOOD { get; set; }
        public string MEAT { get; set; }
        public string VEGETABLES { get; set; }
        public string SNACK { get; set; }
        public string SOUP { get; set; }
        public string SKINSTATE { get; set; }
        public string SKINCOLOR { get; set; }
        public string EDEMA { get; set; }
        public string SKININTEGRITY { get; set; }
        public string SKINPART { get; set; }
        public string SKINSIZE { get; set; }
        public Nullable<int> SKINLEVEL { get; set; }
        public string DEFECATION { get; set; }
        public Nullable<int> SHITNUMBER { get; set; }
        public string SHITNATURE { get; set; }
        public string SHITAMOUT { get; set; }
        public string ASSISTEDDEFECATION { get; set; }
        public string INTESTINALPERISTALSIS { get; set; }
        public string MICTURITION { get; set; }
        public string URINATIONNATURE { get; set; }
        public Nullable<bool> ACONURESISFLAG { get; set; }
        public string ACONURESISINFO { get; set; }
        public string URINATIONDISPOSAL { get; set; }
        public string RIGHTMUSCLE1 { get; set; }
        public string RIGHTMUSCLE2 { get; set; }
        public string LEFTMUSCLE1 { get; set; }
        public string LEFTMUSCLE2 { get; set; }
        public string RIGHTJOINT1 { get; set; }
        public string RIGHTJOINT2 { get; set; }
        public string LEFTJOINT1 { get; set; }
        public string LEFTJOINT2 { get; set; }
        public string GAIT { get; set; }
        public string ASSISTANTTOOL { get; set; }
        public string ASSISTANTSECURITY { get; set; }
        public string ASSISTANTSUITABILITY { get; set; }
        public string ACTIVITYNAME { get; set; }
        public string FALLINFO { get; set; }
        public Nullable<bool> FALL1YEAR { get; set; }
        public Nullable<bool> INJUREDFLAG { get; set; }
        public string INJUREDPART { get; set; }
        public string PASTFALLINFO { get; set; }
        public string INJURYDISPOSALINFO { get; set; }
        public string PAINFREQ { get; set; }
        public string PAINLEVEL { get; set; }
        public string PAINPART { get; set; }
        public string PAINNATURE { get; set; }
        public string DURATIONTIME { get; set; }
        public string EASEPAINMETHOD { get; set; }
        public string VISUALACUITY { get; set; }
        public string ASSISTANTTOOLS { get; set; }
        public string LISTENINGSTATE { get; set; }
        public string DELUSION { get; set; }
        public string PERSONIMAGE { get; set; }
        public string ATTITUDE { get; set; }
        public string EMOTIONSTATE { get; set; }
        public string DISTURBINGENV { get; set; }
        public string SOOTHEMOTION { get; set; }
        public string BEHAVIOR { get; set; }
        public string COMMUNICATIONTYPE { get; set; }
        public string COMMUNICATIONSKILL { get; set; }
        public string PROBLEMBEHAVIOR { get; set; }
        public Nullable<System.DateTime> NEXTEVALDATE { get; set; }
        public Nullable<int> INTERVALDAY { get; set; }
        public Nullable<System.DateTime> DELDATE { get; set; }
        public Nullable<bool> DELFLAG { get; set; }
        public string ORGID { get; set; }

        public List<DC_EvalQuetionModel> evalQuetion { get; set; }
        public List<DC_RegCplModel> regCpl { get; set; }
    }

    public class DC_EvalQuetionModel
    {
        public long RECORDID { get; set; }
        public Nullable<long> ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNO { get; set; }
        public Nullable<int> QUESTIONID { get; set; }
        public string QUESTIONCODE { get; set; }
        public Nullable<int> EVALNUMBER { get; set; }
        public Nullable<decimal> SCORE { get; set; }
        public Nullable<System.DateTime> EVALDATE { get; set; }
        public Nullable<System.DateTime> NEXTEVALDATE { get; set; }
        public string EVALRESULT { get; set; }
        public string DESCRIPTION { get; set; }
        public string ORGID { get; set; }

    }
    public class DC_RegCplModel
    {
        public long SEQNO { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNO { get; set; }
        public string EMPNO { get; set; }
        public Nullable<System.DateTime> STARTDATE { get; set; }
        public Nullable<int> NEEDDAYS { get; set; }
        public Nullable<System.DateTime> TARGETDATE { get; set; }
        public string MAJORTYPE { get; set; }
        public string CPLEVEL { get; set; }
        public string CPDIA { get; set; }
        public Nullable<long> ID { get; set; }
        public string NSDESC { get; set; }
        public string CPREASON { get; set; }
        public Nullable<bool> FINISHFLAG { get; set; }
        public Nullable<System.DateTime> FINISHDATE { get; set; }
        public Nullable<int> TOTALDAYS { get; set; }
        public string CPRESULT { get; set; }
        public string UNFINISHREASON { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }
        public List<NSCPLACTIVITY> NsCplActivity { get; set; }
    }


}

