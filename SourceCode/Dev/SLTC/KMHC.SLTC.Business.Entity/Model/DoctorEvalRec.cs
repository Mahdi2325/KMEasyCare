using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class DoctorEvalRec
    {
        public long Id { get; set; }
        //住院序号
        public long FeeNo { get; set; }
        //病例号
        public int RegNo { get; set; }
        //评估日期
        public Nullable<System.DateTime> EvalDate { get; set; }
        //医生姓名
        public string DocName { get; set; }
        public string DocActName { get; set; }
        //吸烟史
        public string SmokingHistory { get; set; }
        //饮酒
        public string DrinkWine { get; set; }
        //咀嚼槟榔
        public string ChewingBetelnut { get; set; }
        //服食药物
        public string Drugflag { get; set; }
        //规律运动
        public string RegularExercise { get; set; }
        //其它描述
        public string OtherDesc { get; set; }
        //此次入住前疾病描述
        public string DiseaseDesc { get; set; }
        //健康史
        public string MedicalHistory { get; set; }
        //身高
        public decimal Height { get; set; }
        //体重
        public decimal Weight { get; set; }
        //腰围
        public decimal Waist { get; set; }
        //臀围
        public decimal Hipline { get; set; }
        //中臂环围
        public decimal ArmSaRound { get; set; }
        //生命徵象
        public int VitalSigns { get; set; }
        //意识状态
        public string Consciousness { get; set; }
        //情绪
        public string Emotion { get; set; }
        //沟通能力
        public string Communication { get; set; }
        //视力
        public string Eyesight { get; set; }
        //听力
        public string Hearing { get; set; }
        //触觉
        public string Tactilesensation { get; set; }
        //疼痛状态
        public string PainState { get; set; }
        //疼痛类型
        public string PainType { get; set; }
        //疼痛性质
        public string PainNature { get; set; }
        //饮食状态
        public string DietState { get; set; }
        //饮食方式
        public string DietWay { get; set; }
        //饮食形态
        public string DietPaTtern { get; set; }
        //药物使用状态
        public string DrugUseState { get; set; }
        //活动形态
        public string Movement { get; set; }
        //睡眠形态
        public string Sleep { get; set; }
        //是否使用安眠药
        public bool SleepPills { get; set; }
        //有无伤口
        public bool WoundFlag { get; set; }
        //伤口愈合状况
        public string WoundHeal { get; set; }
        //伤口部位
        public string WoundPart { get; set; }
        //伤口大小
        public string Woundsize { get; set; }
        //皮肤表面光滑度
        public string SkinDesc { get; set; }
        //口腔假牙装置
        public string FalseTeeth { get; set; }
        //颈部淋巴结
        public string Lymphnode { get; set; }
        //乳房(硬物或硬块)
        public string Breast { get; set; }
        //胸腔(罗音或嘈音)
        public string Pleural { get; set; }
        //胸腔是否均匀对称
        public string SymmetricalChest { get; set; }
        //心脏
        public string Heart { get; set; }
        //心脏_心杂音及S3、S4等
        public string HeartNoise { get; set; }
        //心率失常
        public string HeartArrhythmia { get; set; }
        //腹部
        public string Abdomen { get; set; }
        //四肢
        public string Limbs { get; set; }
        //神经系统
        public string Nervoussystem { get; set; }
        //重要之实验数据
        public string Checkdata { get; set; }
        //
        public Nullable<System.DateTime> CreateDate { get; set; }
        //
        public string CreateBy { get; set; }
        //
        public string OrgId { get; set; }        
    }
}

