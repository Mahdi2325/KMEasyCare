using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class EvalPlan
    {
        public SwRegEvalPlan swRegEvalPlanModel { get; set; }
        public List<DC_TaskGoalsStrategyModel> TaskGoalsStrategyModel { get; set; }
    }
    public class DC_SwRegEvalPlanModel
    {
        public long EVALPLANID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string RESIDENTNO { get; set; }
        public Nullable<System.DateTime> EVALDATE { get; set; }
        public Nullable<int> EVALNUMBER { get; set; }
        public Nullable<System.DateTime> NEXTEVALDATE { get; set; }
        public Nullable<System.DateTime> INDATE { get; set; }
        public string REGNAME { get; set; }
        public string SEX { get; set; }
        public Nullable<System.DateTime> BIRTHDATE { get; set; }
        public string IDNO { get; set; }
        public string CONTACTNAME { get; set; }
        public string CONTACTPHONE { get; set; }
        public string CONTACTMOBILE { get; set; }
        public string APPELLATION { get; set; }
        public string LIVINGADDRESS { get; set; }
        public string PTYPE { get; set; }
        public string OBSTACLEMANUAL { get; set; }
        public string SOURCETYPE { get; set; }
        public string TAKECAREREASON { get; set; }
        public string TAKECARETYPE { get; set; }
        public string SERVICETYPE { get; set; }
        public string DISEASEINFO { get; set; }
        public string ECOLOGICALMAP { get; set; }
        public string PERSONALHISTORY { get; set; }
        public string PHYSIOLOGY { get; set; }
        public string PSYCHOLOGY { get; set; }
        public string FAMILYSUPPORT { get; set; }
        public string ECONOMICCAPACITY { get; set; }
        public string SOCIALRESOURCES { get; set; }
        public string SOCIALRESOURCE { get; set; }
        public string CURRENTSUBSIDY { get; set; }
        public string ASSISTAPPLICATION { get; set; }
        public string MMSE { get; set; }
        public string ADL { get; set; }
        public string IADL { get; set; }
        public string GDS { get; set; }
        public string EMOTIONSTATE { get; set; }
        public string BEHAVIOR { get; set; }
        public string ATTITUDE { get; set; }
        public string PAYATTENTION { get; set; }
        public string THOUGHT { get; set; }
        public string UNDERSTANDABILITY { get; set; }
        public string SOCIALABILITY { get; set; }
        public string EYESIGHT { get; set; }
        public string HEARING { get; set; }
        public string EXPRESSION { get; set; }
        public string UNDERSTANDING { get; set; }
        public string FAMILYINTERACTION { get; set; }
        public string RELATIVEINTERACTION { get; set; }
        public string FRIENDINTERACTION { get; set; }
        public string ELDERINTERACTION { get; set; }
        public string ADAPTIVESTATE { get; set; }
        public string LIVINGCONDITION { get; set; }
        public string JOBINFO { get; set; }
        public string DAYTAKECAREHOUR { get; set; }
        public Nullable<bool> RELATIVESNEEDCARE { get; set; }
        public Nullable<bool> REPLACEMENT { get; set; }
        public Nullable<bool> EASEPRESSURE { get; set; }
        public string LIFEQUALITY { get; set; }
        public string FAMILYEXPECT { get; set; }
        public string ORGID { get; set; }
        public string REGNO { get; set; }
        public Nullable<bool> DELFLAG { get; set; }
        public Nullable<System.DateTime> DELDATE { get; set; }


        public string MAJORTYPE { get; set; }
        public string CPDIA { get; set; }
        public string TREATMENTGOAL { get; set; }

        public string ORGNAME { get; set; }
    }

    public class SwRegEvalPlan
    {
        #region 字段
        
        public long EVALPLANID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string RESIDENTNO { get; set; }
        public Nullable<System.DateTime> EVALDATE { get; set; }
        public Nullable<int> EVALNUMBER { get; set; }
        public Nullable<System.DateTime> NEXTEVALDATE { get; set; }
        public Nullable<System.DateTime> INDATE { get; set; }
        public string REGNAME { get; set; }
        public string SEX { get; set; }
        public Nullable<System.DateTime> BIRTHDATE { get; set; }
        public string IDNO { get; set; }
        public string CONTACTNAME { get; set; }
        public string CONTACTPHONE { get; set; }
        public string CONTACTMOBILE { get; set; }
        public string APPELLATION { get; set; }
        public string LIVINGADDRESS { get; set; }
        public string PTYPE { get; set; }
        public string OBSTACLEMANUAL { get; set; }
        public string SOURCETYPE { get; set; }
        public string TAKECAREREASON { get; set; }
        public string TAKECARETYPE { get; set; }
        public string SERVICETYPE { get; set; }
        public string DISEASEINFO { get; set; }
        public string ECOLOGICALMAP { get; set; }
        public string PERSONALHISTORY { get; set; }
        public string PHYSIOLOGY { get; set; }
        public string PSYCHOLOGY { get; set; }
        public string FAMILYSUPPORT { get; set; }
        public string ECONOMICCAPACITY { get; set; }
        public string SOCIALRESOURCES { get; set; }
        public string SOCIALRESOURCE { get; set; }
        public string CURRENTSUBSIDY { get; set; }
        public string ASSISTAPPLICATION { get; set; }
        public string MMSE { get; set; }
        public string ADL { get; set; }
        public string IADL { get; set; }
        public string GDS { get; set; }
        public string EMOTIONSTATE { get; set; }
        public string BEHAVIOR { get; set; }
        public string ATTITUDE { get; set; }
        public string PAYATTENTION { get; set; }
        public string THOUGHT { get; set; }
        public string UNDERSTANDABILITY { get; set; }
        public string SOCIALABILITY { get; set; }
        public string EYESIGHT { get; set; }
        public string HEARING { get; set; }
        public string EXPRESSION { get; set; }
        public string UNDERSTANDING { get; set; }
        public string FAMILYINTERACTION { get; set; }
        public string RELATIVEINTERACTION { get; set; }
        public string FRIENDINTERACTION { get; set; }
        public string ELDERINTERACTION { get; set; }
        public string ADAPTIVESTATE { get; set; }
        public string LIVINGCONDITION { get; set; }
        public string JOBINFO { get; set; }
        public string DAYTAKECAREHOUR { get; set; }
        public Nullable<bool> RELATIVESNEEDCARE { get; set; }
        public Nullable<bool> REPLACEMENT { get; set; }
        public Nullable<bool> EASEPRESSURE { get; set; }
        public string LIFEQUALITY { get; set; }
        public string FAMILYEXPECT { get; set; }
        public string ORGID { get; set; }
        public string REGNO { get; set; }
        public Nullable<bool> DELFLAG { get; set; }
        public Nullable<System.DateTime> DELDATE { get; set; }
        #endregion
        public List<DC_TaskGoalsStrategyModel> TaskGoalsStrategyModel { get; set; }
    }
}
