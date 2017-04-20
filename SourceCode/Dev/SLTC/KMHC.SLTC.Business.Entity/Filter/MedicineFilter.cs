using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class MedicineFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public int Medid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EngName { get; set; }

        /// <summary>
        /// 健保药码
        /// </summary>
        public string InsNo { get; set; }

        /// <summary>
        /// 开药医院
        /// </summary>
        public string HospNo { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}

