using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class GoodsLoan
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string No { get; set; }
        public Nullable<int> ManufactureId { get; set; }
        public Nullable<System.DateTime> LoanDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<decimal> Sum { get; set; }
        public Nullable<int> IntervalDay { get; set; }
        public Nullable<System.DateTime> NextDate { get; set; }
        public string Purchaser { get; set; }
        public string Remark { get; set; }
        public string OrgId { get; set; }
    }
}
