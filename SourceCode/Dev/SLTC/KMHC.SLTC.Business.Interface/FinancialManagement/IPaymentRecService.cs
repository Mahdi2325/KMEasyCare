using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IPaymentRecService
    {
        #region LTC_BILLV2
        /// <summary>
        /// 获取LTC_BILLV2账单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BillV2>> QueryBillV2(BaseRequest<PaymentMgmtFilter> request);


        #endregion

        #region LTC_FEERECORD
        /// <summary>
        /// 获取LTC_FEERECORD费用记录
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<FeeRecord>> QueryFeeRecord(BaseRequest<PaymentMgmtFilter> request);


        /// <summary>
        /// 获取LTC_BillV2PAY账单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BillV2PAY>> QueryBillV2PAY(BaseRequest<PaymentMgmtFilter> request);
        #endregion
    }
}
