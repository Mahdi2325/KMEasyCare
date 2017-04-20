using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ResBasicInfo
    {
        public int NCIEvaluateid { get; set; }
        public Nullable<long> Feeno { get; set; }
        public string Name { get; set; }
        public string Ssno { get; set; }
        public Nullable<System.DateTime> Starttime { get; set; }
        public string Residentno { get; set; }
        public string Bedno { get; set; }
        public List<NursingCareEval> nursingEval { get; set; }
    }

   public class NursingCareEval
    {
        public string Itemcode { get; set; }
        public string Itemname { get; set; }
        public string Itemtype { get; set; }
        public Nullable<int> Order { get; set; }
        public string Description { get; set; }
        public int MaxTotalnum { get; set; }
        public List<WorkSubitem> QuesItem { get; set; }
    }


   public class WorkSubitem
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public string Itemcode { get; set; }
       public Nullable<int> Mininum { get; set; }
       public Nullable<int> Maxinum { get; set; }
       public Nullable<int> Order { get; set; }
       public string Remarks { get; set; }
       public int? Score { get; set; }

       public NCIEvaluatedtl ncievaluatedtl { get; set; }
       public List<ScoreOfValue> scoreOfValue { get; set; }
   }

   public class ScoreOfValue
   {
       public int value { get; set; }
       public string Score { get; set; }
   }

   public class NCIEvaluatedtl
   {
       public int ID { get; set; }
       public Nullable<int> NCIEvaluateid { get; set; }
       public string Itemcode { get; set; }
       public Nullable<int> Subitemid { get; set; }
       public Nullable<int> Score { get; set; }
   }

   public class NCIEvaluate
   {
       public int NCIEvaluateid { get; set; }
       public Nullable<long> Feeno { get; set; }
       public string Name { get; set; }
       public string Ssno { get; set; }
       public Nullable<System.DateTime> Starttime { get; set; }
       public string Residentno { get; set; }
       public string Bedno { get; set; }
       public Nullable<System.DateTime> Createtime { get; set; }
       public string Createby { get; set; }
       public Nullable<System.DateTime> Updatetime { get; set; }
       public string Updateby { get; set; }
       public int? TotalScore { get; set; }
   }
}
