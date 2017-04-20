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
    public interface IAdvanceChargeService
    {
        #region LTC_PRECHARGE
        /// <summary>
        /// 获取LTC_PRECHARGE预收款列表
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<PreCharge>> QueryPreCharge(BaseRequest<PaymentMgmtFilter> request);
        /// <summary>
        /// 保存LTC_PRECHARGE预收款
        /// </summary>
        /// <returns></returns>
        BaseResponse<List<PreCharge>> SavePreCharge(PreCharge request, string CreateBy, DateTime CreateDate);
        #endregion

        #region LTC_RESIDENTBALANCE
        /// <summary>
        /// 获取LTC_RESIDENTBALANCE住民账户表
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<ResidentBalance>> QueryRESIDENTBALANCE(BaseRequest<PaymentMgmtFilter> request);
        /// <summary>
        /// 保存LTC_RESIDENTBALANCE住民账户表
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<ResidentBalance> SaveResidentBalance(ResidentBalance request,decimal Balance);
        #endregion
    }
}
