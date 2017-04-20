/*
 * 描述:PainBodyPart
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/26/2016 7:35:07 PM    张正泉            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class PainBodyPartRec
    {
        public long Id { get; set; }
        public Nullable<long> SeqNo { get; set; }
        public string PainPart { get; set; }
        public string PainNature { get; set; }
        public string OccurChance { get; set; }
        public string OccurReason { get; set; }
        public string PainSeverity_Current { get; set; }
        public string PainSeverity_Most { get; set; }
        public string PainSeverity_Low { get; set; }
        public string PainSeverity_Bear { get; set; }
        public string PainDepth { get; set; }
        public string PainExtension { get; set; }
        public string StartPain { get; set; }
        public string PainFrequency { get; set; }
        public string PainDuration { get; set; }
        public string MostPain1Day { get; set; }
        public string EasePainWay { get; set; }
        public string PainSeriousFactor { get; set; }
        public string Symptom { get; set; }
        public string Affect_Sleep { get; set; }
        public string Affect_Activity { get; set; }
        public string Affect_Eating { get; set; }
        public string Affect_Attention { get; set; }
        public string Affect_Emotion { get; set; }
        public string Affect_Relations { get; set; }
        public string Affect_Others { get; set; }
        public Nullable<bool> CancelFlag { get; set; }
        public Nullable<DateTime> CancelDate { get; set; }
        public string CancelReason { get; set; }
        public string Picture { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
