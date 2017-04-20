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
    public interface IRefundMgmtService
    {
        #region LTC_BILLV2
        /// <summary>
        /// 获取LTC_BILLV2账单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BillV2>> QueryBillV2(BaseRequest<PaymentMgmtFilter> request);
        #endregion
        #region LTC_RESIDENTBALANCE
        /// <summary>
        /// 获取LTC_RESIDENTBALANCE住民账户表
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<ResidentBalance>> QueryRESIDENTBALANCE(BaseRequest<PaymentMgmtFilter> request);
        #endregion
        #region LTC_BILLV2
        /// <summary>
        /// 保存账单信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<BillV2> SaveBillV2(BillV2 request, string UpdateBy, DateTime UpdateDate);

        BaseResponse<List<BillV2>> SaveBillV2Info(BillV2List request);
        #endregion

        #region LTC_FEERECORD
        /// <summary>
        /// 获取LTC_FEERECORD费用记录
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<FeeRecord>> QueryFeeRecord(BaseRequest<PaymentMgmtFilter> request);
        #endregion
    }
}
