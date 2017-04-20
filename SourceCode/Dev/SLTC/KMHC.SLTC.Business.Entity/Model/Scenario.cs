using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class ScenarioMain
    {
        public ScenarioMain()
        {
            this.ScenarioItem = new HashSet<ScenarioItem>();
        }
        public int Id { get; set; }
        public int? Scenario { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<ScenarioItem> ScenarioItem { get; set; }
    }
    public class ScenarioItem
    {
        public ScenarioItem()
        {
            this.ScenarioItemOption = new HashSet<ScenarioItemOption>();
        }
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? GroupId { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<bool> IsShowInput { get; set; }
        public int RegdiseasehisDtlId { get; set; }
        public Nullable<DateTime> SickTime { get; set; }
        public string OrgiTreatmentHos { get; set; }
        public string ExpectTransferTo { get; set; }
        public Nullable<bool> HaveCure { get; set; }
        public Nullable<bool> IsCheck { get; set; }
        public Nullable<bool> IsShowOption { get; set; }
        public virtual ICollection<ScenarioItemOption> ScenarioItemOption { get; set; }
    }

    public class ScenarioItemOption
    {
        public int Id { get; set; }
        public int? ScenarioitemId { get; set; }
        public int? OptionId { get; set; }
        public string OptionName { get; set; }
        public Nullable<bool> IsCheck { get; set; }
    }
}
