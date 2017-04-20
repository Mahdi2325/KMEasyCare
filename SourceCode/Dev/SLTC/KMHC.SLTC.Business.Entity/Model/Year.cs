namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Year
    {
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int August { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }
    }

    public partial class YearJoinCategory : Year
    {
        public string Category { get; set; }

        public void SetYearJoinCategory(List<StatisticItem> sourceList)
        {
            sourceList.ForEach(c =>
            {
                switch (c.Month)
                {
                    case 1: this.January = c.Total; break;
                    case 2: this.February = c.Total; break;
                    case 3: this.March = c.Total; break;
                    case 4: this.April = c.Total; break;
                    case 5: this.May = c.Total; break;
                    case 6: this.June = c.Total; break;
                    case 7: this.July = c.Total; break;
                    case 8: this.August = c.Total; break;
                    case 9: this.September = c.Total; break;
                    case 10: this.October = c.Total; break;
                    case 11: this.November = c.Total; break;
                    case 12: this.December = c.Total; break;
                }
            });
        }
    }
}
