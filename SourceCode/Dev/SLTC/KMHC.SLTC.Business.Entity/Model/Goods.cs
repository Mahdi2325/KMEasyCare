using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Goods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string BarNo { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> LoanPrice { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public Nullable<int> InventoryQuantity { get; set; }
        public Nullable<int> InventoryBaseline { get; set; }
        public Nullable<int> TotalLoanAmount { get; set; }
        public Nullable<int> TotalSaleAmount { get; set; }
        public Nullable<bool> NotLess { get; set; }
        public Nullable<bool> IsPayItem { get; set; }
        public Nullable<bool> IsUsed { get; set; }
        public string OrgId { get; set; }
    }
}
