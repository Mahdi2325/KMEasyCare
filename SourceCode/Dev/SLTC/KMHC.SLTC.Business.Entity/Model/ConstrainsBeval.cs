/*
 * 描述:ConstrainsBeval
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/28/2016 2:12:35 PM    张正泉            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ConstrainsBeval
    {
        public long Id { get; set; }
        public Nullable<long> SeqNo { get; set; }
        public Nullable<DateTime> EvalDate { get; set; }
        public string Reason { get; set; }
        public string ConstraintWay { get; set; }
        public string BodyPart { get; set; }
        public string Duration { get; set; }
        public string TempCancelDesc { get; set; }
        public string SkinDesc { get; set; }
        public string BloodCircleDesc { get; set; }
        public string EvaluateBy { get; set; }

        /// <summary>
        /// 评估人员姓名
        /// </summary>
        public string EvaluateByName { get; set; }
    }
}
