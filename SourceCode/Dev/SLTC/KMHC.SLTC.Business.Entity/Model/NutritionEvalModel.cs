using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KMHC.SLTC.Business.Entity.Model 
{
	public class NutritionEvalModel 
	{
		public long ID { get; set; }
		public Nullable<long> FEENO { get; set; }
		public Nullable<int> REGNO { get; set; }
		public string NAME { get; set; }
		public Nullable<System.DateTime> BIRTHDATE { get; set; }
		public Nullable<System.DateTime> EVALDATE { get; set; }
		public string SEX { get; set; }
		public string RESIDENGNO { get; set; }
		public string BEDNO { get; set; }
		public Nullable<System.DateTime> INDATE { get; set; }
		public string DISEASEDIAG { get; set; }
		public string CHEWDIFFCULT { get; set; }
		public string SWALLOWABILITY { get; set; }
		public string EATPATTERN { get; set; }
		public string DIGESTIONPROBLEM { get; set; }
		public string FOODTABOO { get; set; }
		public string ACTIVITYABILITY { get; set; }
		public string PRESSURESORE { get; set; }
		public string EDEMA { get; set; }
		public string CURRENTDIET { get; set; }
		public string EATAMOUNT { get; set; }
		public string WATER { get; set; }
		public string SUPPLEMENTS { get; set; }
		public string SNACK { get; set; }
	}

	public class BiochemistryModel {
		public string ItemName { get; set; }
		public string ItemCode { get; set; }
		public DateTime? EvalDate { get; set; }
		public long? FeeNo { get; set; }
		public string CheckItem { get; set; }
		public DateTime? CheckDate { get; set; }
	}
}
