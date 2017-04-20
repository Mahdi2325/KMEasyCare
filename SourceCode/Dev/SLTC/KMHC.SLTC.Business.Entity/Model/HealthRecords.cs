using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class HealthRecords
    {
        public HealthRecords()
        {
            GeneralDescriptionInfo = string.Empty;
            MeasurementList = new List<Measurement>();
            ADLEvaluation = new EvaluationRecord();
            MMSEEvaluation = new EvaluationRecord();
            IADLEvaluation = new EvaluationRecord();
            SoreEvaluation = new EvaluationRecord();
            FallEvaluation = new EvaluationRecord();
        }
        /// <summary>
        /// 总记录条数
        /// </summary>
        public int TotalRecordsNum { get; set; }
        /// <summary>
        /// 总的描述信息
        /// </summary>
        public string GeneralDescriptionInfo { get; set; }
        public List<Measurement> MeasurementList { get; set; }
        public EvaluationRecord ADLEvaluation { get; set; }
        public EvaluationRecord MMSEEvaluation { get; set; }
        public EvaluationRecord IADLEvaluation { get; set; }
        public EvaluationRecord SoreEvaluation { get; set; }
        public EvaluationRecord FallEvaluation { get; set; }
    }

    public class EvaluationRecord : REGQUESTION
    {
        public int TotalRecordNum { get; set; }
    }

    public class Measurement : MeaSuredRecord
    {
        /// <summary>
        /// 单项总记录数
        /// </summary>
        public int TotalRecordNum { get; set; }

        /// <summary>
        /// 正常记录数
        /// </summary>
        public int NormalRecordNum { get; set; }
        /// <summary>
        /// 超标记录数
        /// </summary>
        public int ExceededRecordNum { get; set; }

        /// <summary>
        /// 记录名称
        /// </summary>
        public string RecordName { get; set; }
        /// <summary>
        /// 记录人
        /// </summary>
        public string Empname { get; set; }
    }
}
