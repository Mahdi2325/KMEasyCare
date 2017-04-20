using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class GoodsSale
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string No { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<decimal> Sum { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> SaleTime { get; set; }
        public string Remark { get; set; }
        public string OrgId { get; set; }
    }
}
