using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class FallIncidentEvent
    {
        public long? ID { get; set; }
        //
        public long FeeNo { get; set; }
        //住民NO
        public int RegNo { get; set; }
        //在场人员
        public string RecordBy { get; set; }
        //发生地点
        public string EventAddress { get; set; }
        //发生时间
        public Nullable<System.DateTime> EventDate { get; set; }
        //事件类别
        public string EventType { get; set; }
        //事件伤害分级
        public string EventDegree { get; set; }

        //事件原因
        public string EventCause { get; set; }
        //发生前意识状态
        public string ConsciousState { get; set; }
        //发生前情绪状态
        public string EmotionalState { get; set; }
        //出现征兆
        public string Signs { get; set; }
        //事件说明
        public string EventDesc { get; set; }

        //处理人员
        public string SettleBy { get; set; }
        //送医时间
        public Nullable<System.DateTime> VisitDocDate { get; set; }
        //医院名称
        public string HospName { get; set; }
        //通知对象
        public string NotifyPeople { get; set; }
        //需通报主管机构
        public bool NotifyGovFlag { get; set; }
        //通报日期
        public Nullable<System.DateTime> NotifyDate { get; set; }
        //发生后意识状态
        public string ConsciousState_a { get; set; }
        //发生后情绪状态
        public string EmotionState_a { get; set; }
        //处理结果
        public string SettleResult { get; set; }
        //後续处理说明
        public string FollowUpInstructions { get; set; }
        //医疗争议
        public string MedicalDispute { get; set; }
        //导致影响
        public string Affects { get; set; }
        //影响说明
        public string AffectsDesc { get; set; }
        //检讨改善
        public string Improvement { get; set; }
        //备注
        public string Description { get; set; }

        //
        public string OrgId { get; set; }

        public string Pict1 { get; set; }

        public string Pict2 { get; set; }

        //在场人员姓名
        public string RecordNameBy { get; set; }

        //处理人员姓名
        public string SettleNameBy { get; set; }

    }
}

