using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.TSG
{
    public class TsgQuestion
    {
        public TsgQuestion()
        {
            this.TsgAnswer = new HashSet<TsgAnswer>();
        }
        public int Id { get; set; }
        public string CategoryCode { get; set; }
        public string Name { get; set; }
        public string QueImageUrl { get; set; }
        public string Description { get; set; }
        public int? OrderSeq { get; set; }
        public string Creater { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public string Updater { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public Nullable<bool> Status { get; set; }
        public virtual ICollection<TsgAnswer> TsgAnswer { get; set; }

        public string CategoryName { get; set; }
    }

    public class TsgQuestionFilter
    {
        public string KeyWord { get; set; }
    }
    public class TsgQuestionData
    {
        public TsgQuestion TsgQuestion { get; set; }
        public TsgAnswer TsgAnswer { get; set; }
    }

}
