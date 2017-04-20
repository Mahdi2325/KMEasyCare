﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public  class LTC_NCIFinancialMonth
    {
        public string Month { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        /// <summary>
        /// 该结算月对应天数
        /// </summary>
        public int IntervalDays { get; set; }
        public string GovId { get; set; }
    }
}
