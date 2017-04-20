using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class GoodsFilter
    {
        //物品名称
        public string Name { get; set; }
        //物品类型
        public string Type { get; set; }
        ///物品编码
        public string No { get; set; }
    }

    public class GoodsRecordFilter
    {
        //物品名称Id
        public int GoodsId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }


    }
    
}





