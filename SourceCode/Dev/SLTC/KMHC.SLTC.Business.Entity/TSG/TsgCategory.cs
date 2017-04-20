using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.TSG
{
    public class TsgCategory
    {
        public TsgCategory()
        {
            this.TsgQuestion = new HashSet<TsgQuestion>();
        }
        public int Id { get; set; }
        public int Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Creater { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public Nullable<bool> Status { get; set; }
        public virtual ICollection<TsgQuestion> TsgQuestion { get; set; }

        public string Class{ get; set; }
    }

    public class TsgCategoryFilter
    {
        public int Type { get; set; }
    }
}
