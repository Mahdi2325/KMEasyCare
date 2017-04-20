using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
   public class DrugRecordinfoFilter
    {
        /// <summary>
        /// 住院序号
        /// </summary>
        public long? FeeNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SeqNo { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
