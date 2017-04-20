using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{

    public class MultiteamCarePlanRec
    {

      public  DC_MultiteamCarePlanRecModel MultiteamCarePlanRe { get; set; }
       

     public  List<DC_MultiteamCarePlanModel> MultiteamCar { get; set; }
    }

   public class DC_MultiteamCarePlanRecModel
    {
        public long FEENO { get; set; }
        public string REGNO { get; set; }
        public string NURSEAIDES { get; set; }
        public Nullable<System.DateTime> EVALDATE { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CHECKDATE { get; set; }
        public string CHECKEDBY { get; set; }
        public long SEQNO { get; set; }
        public Nullable<int> EVALNUMBER { get; set; }
        public string ECOLOGICALMAP { get; set; } //use for DC_MultiteaMcarePlanEvalModel 
        public string DISEASEINFO { get; set; } //use for DC_MultiteaMcarePlanEvalModel 
        public string SWEvalDate { get; set; }
        public int? SWEvalNum { get; set; }
        public string NurEvalDate { get; set; }
        public int? NurEvalNum { get; set; }
        public List<DC_MultiteamCarePlanModel> CarePlan { get; set; }
        public DC_MultiteaMcarePlanEvalModel PlanEval { get; set; }

    }
}
