#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :
** 说明     :
******************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class TreatmentAccount : ResidentMonFeeModel
    {
        public int McType { get; set; }
        public string McTypeName { get; set; }
        public string Gender { get; set; }
        public string Residence { get; set; }
        public string Disease { get; set; }
        public int NsappcareType { get; set; }
        public string NsappcareTypeName { get; set; }
        public DateTime? CertStartTime { get; set; }
        public DateTime? EvaluationTime { get; set; }
        public string InDate { get; set; }
        public string OutDate { get; set; }
        public long? FeeNo { get; set; }
        public string yearMonthArr { get; set; } 
    }
}
