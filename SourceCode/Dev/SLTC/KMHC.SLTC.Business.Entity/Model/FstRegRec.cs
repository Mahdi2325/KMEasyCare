using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class FstRegRec
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 住民系统编号
        /// </summary>
        public Nullable<int> RegNo { get; set; }
        /// <summary>
        /// 入住时间
        /// </summary>
        public Nullable<System.DateTime> RegTime { get; set; }
        /// <summary>
        /// 主诉
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 现病史
        /// </summary>
        public string PresentIllness { get; set; }
        /// <summary>
        /// 既往史否
        /// </summary>
        public Nullable<bool> PrevhxFlag { get; set; }
        /// <summary>
        /// 既往史内容
        /// </summary>
        public string Prevhx { get; set; }
        /// <summary>
        /// 结婚否
        /// </summary>
        public Nullable<bool> MarryFlag { get; set; }
        /// <summary>
        /// 有子女否
        /// </summary>
        public Nullable<bool> OffspringFlag { get; set; }
        /// <summary>
        /// 子女信息
        /// </summary>
        public string Offspring { get; set; }
        /// <summary>
        /// 停经否
        /// </summary>
        public string Menelipsis { get; set; }
        /// <summary>
        /// 末次月经
        /// </summary>
        public Nullable<System.DateTime> LMP { get; set; }
        /// <summary>
        /// 用药史否
        /// </summary>
        public Nullable<bool> MedhFlag { get; set; }
        /// <summary>
        /// 用药史内容
        /// </summary>
        public string Medh { get; set; }
        /// <summary>
        /// 药食物过敏史否
        /// </summary>
        public Nullable<bool> AllergichFlag { get; set; }
        /// <summary>
        /// 药食物过敏史
        /// </summary>
        public string Allergich { get; set; }
        /// <summary>
        /// 手术史否
        /// </summary>
        public Nullable<bool> OperationhxFlag { get; set; }
        /// <summary>
        /// 手术史内容
        /// </summary>
        public string Operationhx { get; set; }
        /// <summary>
        /// 个人史否
        /// </summary>
        public Nullable<bool> PersonalhxFlag { get; set; }
        /// <summary>
        /// 个人史内容
        /// </summary>
        public string Personalhx { get; set; }
        /// <summary>
        /// 家族史否
        /// </summary>
        public Nullable<bool> FhxFlag { get; set; }
        /// <summary>
        /// 家族史
        /// </summary>
        public string Fhx { get; set; }
        /// <summary>
        /// 体温
        /// </summary>
        public string Temp { get; set; }
        /// <summary>
        /// 脉搏
        /// </summary>
        public Nullable<decimal> Pulse { get; set; }
        /// <summary>
        /// 呼吸
        /// </summary>
        public Nullable<decimal> Breath { get; set; }
        /// <summary>
        /// 舒张压
        /// </summary>
        public Nullable<decimal> DP { get; set; }
        /// <summary>
        /// 收缩压
        /// </summary>
        public Nullable<decimal> SP { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public Nullable<decimal> Height { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public Nullable<decimal> Weight { get; set; }
        /// <summary>
        /// 皮肤黏膜红润否
        /// </summary>
        public Nullable<bool> SkinmucosaHRunFlag { get; set; }
        /// <summary>
        /// 皮肤黏膜黄染否
        /// </summary>
        public Nullable<bool> SkinmucosaHRanFlag { get; set; }
        /// <summary>
        /// 皮肤黏膜苍白否
        /// </summary>
        public Nullable<bool> SkinmucosaCBFlag { get; set; }
        /// <summary>
        /// 皮肤黏膜千瘪否
        /// </summary>
        public Nullable<bool> SkinmucosaQBFlag { get; set; }
        /// <summary>
        /// 皮肤黏膜皮疹否
        /// </summary>
        public Nullable<bool> SkinmucosaPZFlag { get; set; }
        /// <summary>
        /// 皮肤黏膜信息
        /// </summary>
        public string Skinmucosa { get; set; }
        /// <summary>
        /// 其他皮肤黏膜否
        /// </summary>
        public Nullable<bool> OtherSkinmucosaFlag { get; set; }
        /// <summary>
        /// 其他皮肤黏膜信息
        /// </summary>
        public string OtherSkinmucosa { get; set; }
        /// <summary>
        /// 淋巴结否
        /// </summary>
        public Nullable<bool> LymphGlandFlag { get; set; }
        /// <summary>
        /// 淋巴结
        /// </summary>
        public string LymphGland { get; set; }
        /// <summary>
        /// 头颅正常否
        /// </summary>
        public Nullable<bool> HeadFlag { get; set; }
        /// <summary>
        /// 眼：巩膜正常否
        /// </summary>
        public Nullable<bool> ScleraFlag { get; set; }
        /// <summary>
        /// 眼：空桶正常否
        /// </summary>
        public Nullable<bool> EBFlag { get; set; }
        /// <summary>
        /// 眼：空桶信息
        /// </summary>
        public string EB { get; set; }
        /// <summary>
        /// 眼：结膜  充血否
        /// </summary>
        public Nullable<bool> ConjunctivaFlag { get; set; }
        /// <summary>
        /// 眼：结膜  其他信息
        /// </summary>
        public string OtherConjunctiva { get; set; }
        /// <summary>
        /// 耳：听力 耳廊异常否
        /// </summary>
        public Nullable<bool> EARFlag { get; set; }
        /// <summary>
        /// 耳：听力 耳廊 异常信息
        /// </summary>
        public string EAR { get; set; }
        /// <summary>
        /// 耳：听力： 外耳道选项
        /// </summary>
        public string AcousticDuctPro { get; set; }
        /// <summary>
        /// 耳：听力：外耳道信息
        /// </summary>
        public string AcousticDuct { get; set; }
        /// <summary>
        /// 鼻中隔选项
        /// </summary>
        public string NasalseptumPro { get; set; }
        /// <summary>
        /// 鼻中隔信息
        /// </summary>
        public string Nasalseptum { get; set; }
        /// <summary>
        /// 鼻腔黏膜
        /// </summary>
        public string Nasalmucosa { get; set; }
        /// <summary>
        /// 鼻胛
        /// </summary>
        public string Nasalshoulder { get; set; }
        /// <summary>
        /// 唇
        /// </summary>
        public string LIP { get; set; }
        /// <summary>
        /// 舌
        /// </summary>
        public string Tongue { get; set; }
        /// <summary>
        /// 口腔黏膜项选
        /// </summary>
        public string OralmucosaPro { get; set; }
        /// <summary>
        /// 口腔黏膜
        /// </summary>
        public string Oralmucosa { get; set; }
        /// <summary>
        /// 牙齿选项
        /// </summary>
        public string Tooth { get; set; }
        /// <summary>
        /// 扁桃体肿大否
        /// </summary>
        public string Tonsil { get; set; }
        /// <summary>
        /// 扁桃体肿大  __侧
        /// </summary>
        public string TonsilC { get; set; }
        /// <summary>
        /// 扁桃体肿大  __度
        /// </summary>
        public string TonsilD { get; set; }
        /// <summary>
        /// 扁桃体化脓
        /// </summary>
        public string TonsilHN { get; set; }
        /// <summary>
        /// 气管
        /// </summary>
        public string Organ { get; set; }
        /// <summary>
        /// 甲状腺
        /// </summary>
        public string ThyroidGland { get; set; }
        /// <summary>
        /// 甲状腺肿大  __侧
        /// </summary>
        public string ThyroidGlandC { get; set; }
        /// <summary>
        /// 甲状腺肿大  __度
        /// </summary>
        public string ThyroidGlandD { get; set; }
        /// <summary>
        /// 胸廓正常否
        /// </summary>
        public Nullable<bool> ThoraxFlag { get; set; }
        /// <summary>
        /// 胸廓
        /// </summary>
        public string Thorax { get; set; }
        /// <summary>
        /// 肺选项
        /// </summary>
        public string LungPro { get; set; }
        /// <summary>
        /// 肺：干罗音 否
        /// </summary>
        public Nullable<bool> LungGLYFlag { get; set; }
        /// <summary>
        /// 干啰音 __侧
        /// </summary>
        public string LungG { get; set; }
        /// <summary>
        /// 肺：湿罗音 否
        /// </summary>
        public Nullable<bool> LungSLYFlag { get; set; }
        /// <summary>
        /// 湿啰音 __侧
        /// </summary>
        public string LungS { get; set; }
        /// <summary>
        /// 心率
        /// </summary>
        public Nullable<int> HeartRate { get; set; }
        /// <summary>
        /// 心律齐否
        /// </summary>
        public Nullable<bool> HeartrhythmFlag { get; set; }
        /// <summary>
        /// 心律信息
        /// </summary>
        public string HeartRhythm { get; set; }
        /// <summary>
        /// 心音
        /// </summary>
        public string HeartSound { get; set; }
        /// <summary>
        /// 乳房对称否
        /// </summary>
        public Nullable<bool> BreastDCFlag { get; set; }
        /// <summary>
        /// 乳房包块否
        /// </summary>
        public Nullable<bool> BreastBKFlag { get; set; }

        /// <summary>
        /// 乳房包块描述
        /// </summary>
        public string Breast { get; set; }
        /// <summary>
        /// 其他乳房
        /// </summary>
        public string OtherBreast { get; set; }
        /// <summary>
        /// 乳头选项
        /// </summary>
        public string NipplePro { get; set; }
        /// <summary>
        /// 乳头分泌物选项
        /// </summary>
        public string NippleFMWPro { get; set; }
        /// <summary>
        /// 乳头分泌物信息
        /// </summary>
        public string Nipple { get; set; }
        /// <summary>
        /// 其他乳头信息
        /// </summary>
        public string OtherNipple { get; set; }
        /// <summary>
        /// 腹部：板状腹否
        /// </summary>
        public Nullable<bool> AbdomenBZFFlag { get; set; }
        /// <summary>
        /// 腹部选项
        /// </summary>
        public string AbdomenPro { get; set; }
        /// <summary>
        /// 腹部压痛信息
        /// </summary>
        public string Abdomen { get; set; }
        /// <summary>
        /// 肠鸣音选项
        /// </summary>
        public string BowelSounDPro { get; set; }
        /// <summary>
        /// 肠鸣音亢进
        /// </summary>
        public string BowelSoundKJ { get; set; }
        /// <summary>
        /// 肠鸣音减弱
        /// </summary>
        public string BowelSoundJR { get; set; }
        /// <summary>
        /// 肠鸣音其他
        /// </summary>
        public string OtherBowelSound { get; set; }
        /// <summary>
        /// 脊柱四肢 选项
        /// </summary>
        public string SpinelimbsPro { get; set; }
        /// <summary>
        /// 脊柱四肢 畸形选项
        /// </summary>
        public string SpinelimbsJXPro { get; set; }
        /// <summary>
        /// 脊柱四肢 活动受限部位
        /// </summary>
        public string Spinelimbsbw { get; set; }
        /// <summary>
        /// 柱脊四肢畸形信息
        /// </summary>
        public string Spinelimbsjx { get; set; }
        /// <summary>
        /// 肛门及外生殖器选项
        /// </summary>
        public string AnusgenitaliaPro { get; set; }
        /// <summary>
        /// 肛门及外生殖器 其他信息
        /// </summary>
        public string OtherAnusgenitaliaPro { get; set; }
        /// <summary>
        /// 神经系统选项
        /// </summary>
        public string Nervous { get; set; }
        /// <summary>
        /// 神经系统 肌力减弱  __侧
        /// </summary>
        public string Nervousc { get; set; }
        /// <summary>
        /// 神经系统 肌力减弱  __级
        /// </summary>
        public string Nervousl { get; set; }
        /// <summary>
        /// 其他神经系统
        /// </summary>
        public string OtherNervous { get; set; }
        /// <summary>
        /// 其他专科体检
        /// </summary>
        public string OtheRdeptexam { get; set; }
        /// <summary>
        /// 痛疼否
        /// </summary>
        public Nullable<bool> PainFlag { get; set; }
        /// <summary>
        /// 疼痛分数
        /// </summary>
        public string PainScore { get; set; }
        /// <summary>
        /// 疼痛部位
        /// </summary>
        public string PainPosition { get; set; }
        /// <summary>
        /// 疼痛性质
        /// </summary>
        public string PainProperty { get; set; }
        /// <summary>
        /// 疼痛频率
        /// </summary>
        public string PainFreq { get; set; }
        /// <summary>
        /// 营养需求否
        /// </summary>
        public Nullable<bool> NutritionalFlag { get; set; }
        /// <summary>
        /// 营养需求
        /// </summary>
        public string Nutritional { get; set; }
        /// <summary>
        /// 康复需求
        /// </summary>
        public Nullable<bool> RehagNeeds { get; set; }
        /// <summary>
        /// 特殊需求 牙齿
        /// </summary>
        public string SpecialNeedsTooth { get; set; }
        /// <summary>
        /// 特殊需求 听力
        /// </summary>
        public string SpecialNeedHear { get; set; }
        /// <summary>
        /// 特殊需求 听力 左右
        /// </summary>
        public string SpecialNeedHearpos { get; set; }
        /// <summary>
        /// 特殊需求 视力
        /// </summary>
        public string SpecialNeedVision { get; set; }
        /// <summary>
        /// 特殊需求 视力 左右
        /// </summary>
        public string SpecialNeedVisionPos { get; set; }
        /// <summary>
        /// 教宣对象
        /// </summary>
        public string Missionobj { get; set; }
        /// <summary>
        /// 学习能力
        /// </summary>
        public Nullable<bool> LearnAbility { get; set; }
        /// <summary>
        /// 学习意愿
        /// </summary>
        public string LearnWish { get; set; }
        /// <summary>
        /// 学习需求否
        /// </summary>
        public Nullable<bool> LearnNeedFlag { get; set; }
        /// <summary>
        /// 学习需求
        /// </summary>
        public string LearnNeed { get; set; }
        /// <summary>
        /// 初步诊断
        /// </summary>
        public string TentativeDiag { get; set; }
        /// <summary>
        /// 诊疗计划
        /// </summary>
        public string Plan { get; set; }
        /// <summary>
        /// 处理
        /// </summary>
        public string Handle { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
