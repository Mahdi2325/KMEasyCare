using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface ISettleAccountService
    {
        /// <summary>
        /// 结算信息
        /// </summary>
        /// <param name="feeNo">住民No</param>
        /// <returns>费用记录信息</returns>
        BaseResponse<SettleAccountModel> QuerySettleAccountInfo(long feeNo);
        /// <summary>
        /// 护理险费用详细信息
        /// </summary>
        /// <param name="feeNo">住民No</param>
        /// <param name="beginTime">结算开始日</param>
        /// <param name="endTime">结算结束日</param>
        /// <returns>费用记录信息</returns>
        BaseResponse<IList<FeeRecordBaseInfo>> QueryRecord(long feeNo, string beginTime, string endTime);
        /// <summary>
        /// 获取已缴费账单
        /// </summary>
        /// <param name="feeNo">住民No</param>
        /// <param name="beginTime">结算开始日</param>
        /// <param name="endTime">结算结束日</param>
        /// <returns></returns>
        BaseResponse<IList<BillV2>> QueryBillV2(long feeNo, string beginTime, string endTime);

    }
}
