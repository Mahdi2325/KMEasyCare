using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Report
{
   public class ActivityRequEval
    {
        public string InDate     { get; set; }
        public string EvalDate { get; set; }
        public string FeeName { get; set; }
        public string Carer { get; set; }
        public string Vision { get; set; }
        public string Attention { get; set; }
        public string Smell { get; set; }

        public string Nottalked { get; set; }
        public string Sensation { get; set; }
        public string Taste { get; set; }
        public string Hearing { get; set; }
        public string Upperlimb { get; set; }
        public string Lowerlimb { get; set; }
        public string Hallucination { get; set; }
        public string Delusion { get; set; }


        public string Directionsense { get; set; }
        public string Comprehension { get; set; }
        public string Memory { get; set; }
        public string Expression { get; set; }
        public string Othernarrative { get; set; }


        public string Emotion { get; set; }
        public string Self { get; set; }
        public string Behaviorcontent { get; set; }
        public string Behaviorfreq { get; set; }
        public string Activity { get; set; }
        public string Talkedwilling { get; set; }


        public string Artactivity { get; set; }
        public string Aidsactivity { get; set; }
        public string Severeactivity { get; set; }
    }
}
