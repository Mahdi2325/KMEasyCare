using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Regdiseasehis
    { 
        public int RegNo { get; set; }
        public Nullable<bool> HaveOperation { get; set; }
        public string Operation { get; set; }
        public Nullable<bool> HaveDrugAllergy { get; set; }
        public string DrugAllergy { get; set; }
        public Nullable<bool> HaveFoodAllergy { get; set; }
        public string FoodAllergy { get; set; }
        public Nullable<bool> HaveTransfusion { get; set; }
        public string OrigMedicalHos { get; set; }
        public string EmergencyTransTo { get; set; }
        public Nullable<bool> IsAgreeTransfer { get; set; }
        public string MedicalHis { get; set; }
        public string Others { get; set; }
    }
    public class RegdiseasehisDtl
    {
        public int Id { get; set; }
        public int RegNo { get; set; }
        public int CategoryId { get; set; }
        public int ItemId { get; set; }
        public string OtherItemName { get; set; }
        public string ScenarioOptionIds { get; set; }
        public Nullable<DateTime> SickTime { get; set; }
        public string OrgiTreatmentHos { get; set; }
        public string ExpectTransferTo { get; set; }
        public Nullable<bool> HaveCure { get; set; }     
    }



}
