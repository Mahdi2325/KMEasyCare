using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model.FinancialManagement
{
    public partial class MonthlyPayBillRecords
    {
        public int ID { get; set; }
        public string BILLID { get; set; }
        public string YEARMONTH { get; set; }
        public long FEENO { get; set; }
        public decimal PAYEDAMOUNT { get; set; }
        public System.DateTime COMPSTARTDATE { get; set; }
        public System.DateTime COMPENDDATE { get; set; }
        public int STATUS { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public string UPDATEBY { get; set; }
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        public Nullable<bool> ISDELETE { get; set; }

        public class MonthlyPayBillRecordsList
        {
            public MonthlyPayBillRecordsList()
            {
                MonthlyPayBillRecordsLists = new List<MonthlyPayBillRecords>();
            }
            public List<MonthlyPayBillRecords> MonthlyPayBillRecordsLists { get; set; }
        }
    }
}
