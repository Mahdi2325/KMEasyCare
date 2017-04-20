using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Report
{
    public class ReportNurseingPlanEval
    {
        public string OrgName { get; set; } 
        public long Id { get; set; }
        //日字号
        public string ResidentNo { get; set; }
        //姓名
        public string RegName { get; set; }
        //性别
        public string Sex { get; set; }
        //出生
        public DateTime? BirthDate { get; set; }
        //年龄
        public string Age { get; set; }
        //初评日期
        public DateTime? FirstevalDate { get; set; }
        //第几次评估
        public int EvalNumber { get; set; }
        //评估日期
        public DateTime? EvalDate { get; set; }
        //评估间隔
        public int Interval { get; set; }
        //下次评估日期
        public DateTime? Nextevaldate { get; set; }
        //收托日期
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
        public string MedicineInfo { get; set; }
        //就医医院
        public string VisitHospitalN { get; set; }
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
        public decimal BMIValue { get; set; }
        //理想体重
        public decimal IW { get; set; }
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
        public string AssistedDef { get; set; }
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
        //近一年内跌倒情形
        public string Fall1YearInfo { get; set; }
        //过去跌倒情况
        public string PastFallIf { get; set; }
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
        public string AssistTools { get; set; }
        //认知感受- 听力状况
        public string ListeningState { get; set; }
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
        public string ProblemBeh { get; set; }

        //ADL评估
        //进食
        public string adlA1 { get; set; }
        public string adlA2 { get; set; }
        public string adlA3 { get; set; }
        public string adlA4 { get; set; }
        public string adlA5 { get; set; }
        public string adlA6 { get; set; }
        public string adlA7 { get; set; }
        public string adlA8 { get; set; }
        public string adlA9 { get; set; }
        public string lA10 { get; set; }
        //总分
        public string adlScore
        {
            get
            {
                return (Convert.ToInt32(string.IsNullOrEmpty(adlA1) ? "0" : adlA1) +
                    Convert.ToInt32(string.IsNullOrEmpty(adlA2) ? "0" : adlA2) +
                    Convert.ToInt32(string.IsNullOrEmpty(adlA3) ? "0" : adlA3) +
                    Convert.ToInt32(string.IsNullOrEmpty(adlA4) ? "0" : adlA4) +
                    Convert.ToInt32(string.IsNullOrEmpty(adlA5) ? "0" : adlA5) +
                    Convert.ToInt32(string.IsNullOrEmpty(adlA6) ? "0" : adlA6) +
                    Convert.ToInt32(string.IsNullOrEmpty(adlA7) ? "0" : adlA7) +
                    Convert.ToInt32(string.IsNullOrEmpty(adlA8) ? "0" : adlA8) +
                    Convert.ToInt32(string.IsNullOrEmpty(adlA9) ? "0" : adlA9) +
                    Convert.ToInt32(string.IsNullOrEmpty(lA10) ? "0" : lA10)).ToString();
            }
        }

        //iADL评估
        public string ilA1 { get; set; }
        public string ilA2 { get; set; }
        public string ilA3 { get; set; }
        public string ilA4 { get; set; }
        public string ilA5 { get; set; }
        public string ilA6 { get; set; }
        public string ilA7 { get; set; }
        public string ilA8 { get; set; }
        //总分
        public string ilScore
        {
            get
            {
                return (Convert.ToInt32(string.IsNullOrEmpty(ilA1) ? "0" : ilA1) +
                    Convert.ToInt32(string.IsNullOrEmpty(ilA2) ? "0" : ilA2) +
                    Convert.ToInt32(string.IsNullOrEmpty(ilA3) ? "0" : ilA3) +
                    Convert.ToInt32(string.IsNullOrEmpty(ilA4) ? "0" : ilA4) +
                    Convert.ToInt32(string.IsNullOrEmpty(ilA5) ? "0" : ilA5) +
                    Convert.ToInt32(string.IsNullOrEmpty(ilA6) ? "0" : ilA6) +
                    Convert.ToInt32(string.IsNullOrEmpty(ilA7) ? "0" : ilA7) +
                    Convert.ToInt32(string.IsNullOrEmpty(ilA8) ? "0" : ilA8)).ToString();
            }
        }
        //mmse评估
        public string mmseA1 { get; set; }
        public string mmseA2 { get; set; }
        public string mmseA3 { get; set; }
        public string mmseA4 { get; set; }
        public string mmseA5 { get; set; }
        public string mmseA6 { get; set; }
        public string mmseA7 { get; set; }
        //总分
        public string mmseScore
        {
            get
            {
                return (Convert.ToInt32(string.IsNullOrEmpty(mmseA1) ? "0" : mmseA1) +
                    Convert.ToInt32(string.IsNullOrEmpty(mmseA2) ? "0" : mmseA2) +
                    Convert.ToInt32(string.IsNullOrEmpty(mmseA3) ? "0" : mmseA3) +
                    Convert.ToInt32(string.IsNullOrEmpty(mmseA4) ? "0" : mmseA4) +
                    Convert.ToInt32(string.IsNullOrEmpty(mmseA5) ? "0" : mmseA5) +
                    Convert.ToInt32(string.IsNullOrEmpty(mmseA6) ? "0" : mmseA6) +
                    Convert.ToInt32(string.IsNullOrEmpty(mmseA7) ? "0" : mmseA7)).ToString();
            }
        }
        //上次计划
        public List<NursingPlan> lastNursingPlan { get; set; }
        //本次计划
        public List<NursingPlan> nursingPlan { get; set; }
    }

    public class ReportEvalQuestionResult
    {
        public long ID { get; set; }
        public Nullable<long> RECORDID { get; set; }
        public Nullable<int> QUESTIONID { get; set; }
        public Nullable<int> MAKERID { get; set; }
        public Nullable<decimal> MAKERVALUE { get; set; }
        public Nullable<int> LIMITEDVALUEID { get; set; }
        public Nullable<decimal> LIMITEDVALUE { get; set; }
        public string MAKENAME { get; set; }
        public string CODE { get; set; }
        public string ORGID { get; set; }
    }

    public class NursingPlan
    {
        public string Cpdia { get; set; }
        public string Activity { get; set; }
        public string Finished { get; set; }

    }

}

