using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NutrtioncateRec
    {
        public long Id { get; set; }
        public long Feeno { get; set; }
        public int Regno { get; set; }
        public DateTime? Recorddate { get; set; }
        //饮食形态
        public string DietPattern { get; set; }
        //营养途径
        public string NutrtionPathway { get; set; }
        //进食方式
        public string DietWay { get; set; }
        //进食频率
        public string DinnerFreq { get; set; }
        //活动能力
        public string ActivityAbility { get; set; }
        //体重
        public decimal Weight { get; set; }
        //其他（疾病）
        public string OtherDisease { get; set; }
        //
        public decimal Bmi { get; set; }
        //体重评估
        public string WeightEval { get; set; }
        //目前饮食状况
        public string DietState { get; set; }
        //水分摄取
        public decimal WaterUptake { get; set; }
        //能量需求 kcal/天
        public int Kcal { get; set; }
        //能量需求类型
        public string KCALTYPE { get; set; }
        //主食
        public int KcalFood { get; set; }
        //肉鱼豆蛋
        public int KcalFish { get; set; }
        //蔬菜
        public int KcalVegetables { get; set; }
        //水果
        public int KcalFruit { get; set; }
        //油脂
        public int KcalGrease { get; set; }
        //蛋白质需求
        public int Protein { get; set; }
        //额外盐分摄取
        public string Salinity { get; set; }
        //管灌需求 kcal/天
        public int PipeKcal { get; set; }
        //蛋白质
        public int PipeProtein { get; set; }
        //冲管水量
        public string PipleWater { get; set; }
        //Vit补充
        public string PipleVit { get; set; }
        //其他水量
        public string PipleOtherWater { get; set; }
        //营养诊断
        public string NutrtionDiag { get; set; }
        //特殊疾病饮食
        public string SpecialDiet { get; set; }
        //其他计划与建议
        public string Suggestion { get; set; }
        //营养师
        public string Dietitian { get; set; }       
    }
}

