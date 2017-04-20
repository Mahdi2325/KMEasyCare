using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
   public class DeductionFilter
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public string NsNo { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
}
