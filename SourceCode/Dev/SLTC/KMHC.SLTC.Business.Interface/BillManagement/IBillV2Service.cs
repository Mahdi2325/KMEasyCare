using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IBillV2Service : IBaseService
    {
        /// <summary>
        /// 查询账单列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>账单数据信息</returns>
        BaseResponse<IList<BillV2>> QueryBillV2(BaseRequest<BillV2Filter> request);

        /// <summary>
        /// 记录数据
        /// </summary>
        /// <param name="feeNo">住民No</param>
        /// <param name="billId">账单Id</param>
        /// <returns>费用记录信息</returns>
        BaseResponse<BillV2FeeRecord> QueryBillV2FeeRecord(int feeNo, string billId);

        /// <summary>
        /// 费用清单查询
        /// </summary>
        /// <param name="feeNo">住民feeNO</param>
        /// <param name="startDate">开始核算时间</param>
        /// <param name="endDate">核算结束时间</param>
        /// <returns>费用记录信息</returns>
        BaseResponse<BillV2FeeList> QueryBillV2FeeList(long feeNo, DateTime startDate, DateTime endDate);

        List<BillV2FeeList> QueryBillV2FeeListReport(int feeNo, DateTime startDate, DateTime endDate);

        List<BillV2FeeList> QueryBillV2AllFeeListReport(DateTime startDate, DateTime endDate);

        BaseResponse<BillV2> DeleteBillInfo(string billNo);
        object GetYearMonth();
        object GetMonthData(string date);
        object GetRSMonFees(string date,long FeeNo);
        object GetRSMonFeeDtl(int currentPage, int pageSize, string date, long FeeNo, string feeType);
        object CancelMonthData(string date);
        object GetMonthDataList(BaseRequest<MonthFeeFilter> request);
        object GetOrgMonthDataList(string beginTime, string endTime);
        object UploadMonthData(string date);
        BaseResponse<BillV2> UpdateBillStatusToUploaded(string month);
        string GetNsno();
        List<PrintMonthFee> GetPrintData(BaseRequest<MonthFeeFilter> request);
        List<PrintMonthFee> GetPrintData(BaseResponse<IList<ResidentMonFeeModel>> res);
        List<PrintMonthFee> GetPrintData(BaseResponse<IList<TreatmentAccount>> res);
    }
}
