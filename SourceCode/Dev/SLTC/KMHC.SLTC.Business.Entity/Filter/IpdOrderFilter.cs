using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class IpdOrderFilter
    {
        public long FeeNo { get; set; }
        public long OrderNo { get; set; }
        public int OrderType { get; set; }
        public int ConfirmFlag { get; set; }
        public int CheckFlag { get; set; }
        public int StopFlag { get; set; }
        public int CancelFlag { get; set; }
        public int TimeFlag { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SortType { get; set; }
        public int LoadType { get; set; }

        public int FeeCode { get; set; }

        public int ItemType { get; set; }

        public string ChargeGroupId { get; set; }

        public string KeyWord { get; set; }
    }
}
