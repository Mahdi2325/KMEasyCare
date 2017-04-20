using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
   public class DC_MultiteaMcarePlanEvalModel
    {
        public long SEQNO { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNO { get; set; }
        public string NURSEAIDES { get; set; }
        public Nullable<System.DateTime> EVALDATE { get; set; }
        public Nullable<int> EVALNUMBER { get; set; }
        public string ECOLOGICALMAP { get; set; }
        public string PERSONALHISTORY { get; set; }
        public string PHYSIOLOGY { get; set; }
        public string FAMILYSUPPORT { get; set; }
        public string SOCIALRESOURCES { get; set; }
        public string DISEASEINFO { get; set; }
        public string MMSESCORE { get; set; }
        public string IADLSCORE { get; set; }
        public string ADLSCORE { get; set; }
        public string GODSSCORE { get; set; }
        public string MOOD { get; set; }
        public string PROBLEMBEHAVIOR { get; set; }
        public string PSYCHOLOGY { get; set; }
        public string ECONOMICCAPACITY { get; set; }
        public string INTERPERSONAL { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CHECKDATE { get; set; }
        public string CHECKEDBY { get; set; }
    }
}
