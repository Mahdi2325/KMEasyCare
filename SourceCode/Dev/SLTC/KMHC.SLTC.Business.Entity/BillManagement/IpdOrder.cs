using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.BillManagement
{
    public class IpdOrder
    {
        public string RsName { get; set; }
        public Nullable<bool> checkNoSendFlag { get; set; }
        public Nullable<long> OrderNo { get; set; }
        public Nullable<int> OrderType { get; set; }
        public string OrderName { get; set; }
        public Nullable<int> FeeCode { get; set; }
        public Nullable<int> ItemType { get; set; }
        public string AcRemark { get; set; }
        public decimal TakeQty { get; set; }
        public Nullable<int> TakeDay { get; set; }
        public string TakeFreq { get; set; }
        public decimal TakeFreqQty { get; set; }
        public string TakeWay { get; set; }
        public Nullable<int> ConversionRatio { get; set; }
        public string PrescribeUnits { get; set; }
        public string Units { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ChargeQty { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> ConfirmFlag { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
        public Nullable<int> CheckFlag { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public Nullable<int> StopFlag { get; set; }
        public Nullable<System.DateTime> StopDate { get; set; }
        public Nullable<int> StopCheckFlag { get; set; }
        public Nullable<System.DateTime> StopCheckDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> ExecTimes { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public Nullable<int> DeleteFlag { get; set; }
        public Nullable<int> FirstDayQuantity { get; set; }
        public string ChargeGroupId { get; set; }
        public string DoctorNo { get; set; }
        public string NurseNo { get; set; }
        public string DoctorName { get; set; }
        public string NurseName { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string OrgId { get; set; }
        public Nullable<long> SortNumber { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }


        //发送记录
        public long OrderPostRecNo { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public string PostNurseNo { get; set; }
        public string PostNurseName { get; set; }
        public Nullable<bool> PostIsDelete { get; set; }

        //使用记录状态
        public Nullable<int> Status { get; set; }

    }

    public class NewIpdOrder
    {
        public long NOrderNo { get; set; }
        public Nullable<int> NOrderType { get; set; }
        public string NOrderName { get; set; }
        public Nullable<int> NFeeCode { get; set; }
        public Nullable<int> NItemType { get; set; }
        public string NAcRemark { get; set; }
        public Nullable<decimal> NTakeQty { get; set; }
        public Nullable<int> NTakeDay { get; set; }
        public string NTakeFreq { get; set; }
        public string NTakeWay { get; set; }
        public Nullable<int> ConversionRatio { get; set; }
        public string NPrescribeUnits { get; set; }
        public string NUnits { get; set; }
        public decimal NUnitPrice { get; set; }
        public decimal NChargeQty { get; set; }
        public decimal NAmount { get; set; }
        public Nullable<int> ConfirmFlag { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
        public Nullable<int> CheckFlag { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public Nullable<int> StopFlag { get; set; }
        public Nullable<System.DateTime> StopDate { get; set; }
        public Nullable<int> StopCheckFlag { get; set; }
        public Nullable<System.DateTime> StopCheckDate { get; set; }
        public Nullable<System.DateTime> NStartDate { get; set; }
        public Nullable<System.DateTime> NEndDate { get; set; }
        public Nullable<int> ExecTimes { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public Nullable<int> DeleteFlag { get; set; }
        public Nullable<int> NFirstDayQuantity { get; set; }
        public string NChargeGroupId { get; set; }
        public string DoctorNo { get; set; }
        public string NurseNo { get; set; }
        public string DoctorName { get; set; }
        public string NurseName { get; set; }
        public Nullable<long> FeeNo { get; set; }
        public Nullable<int> RegNo { get; set; }
        public string OrgId { get; set; }
        public Nullable<long> NSortNumber { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }

    public class IpdOrderList
    {
        public IpdOrderList()
        {
            IpdOrderLists = new List<NewIpdOrder>();
        }
        public List<NewIpdOrder> IpdOrderLists { get; set; }
    }

    public class NoSendIpdOrderList
    {
        public NoSendIpdOrderList()
        {
            NoSendIpdOrderLists = new List<IpdOrder>();
        }
        public List<IpdOrder> NoSendIpdOrderLists { get; set; }
    }
}
