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
    public interface IRefundRecService
    {
        #region LTC_BILLV2
        /// <summary>
        /// 获取LTC_BILLV2 退费账单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BillV2>> QueryBillV2(BaseRequest<RefundMgmtFilter> request);


        #endregion

        #region LTC_FEERECORD
        /// <summary>
        /// 获取LTC_FEERECORD费用记录
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<FeeRecord>> QueryFeeRecord(BaseRequest<RefundMgmtFilter> request);


        /// <summary>
        /// 获取LTC_Refund 退费记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Refund>> QueryRefund(BaseRequest<RefundMgmtFilter> request);
        #endregion
    }
}
