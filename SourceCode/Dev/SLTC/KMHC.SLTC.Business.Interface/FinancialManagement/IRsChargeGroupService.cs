using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IRsChargeGroupService
    {
        /// <summary>
        /// 获取LTC_RESIDENTBALANCE住民账户表
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<RsChargeGroup>> QueryRsChargeGroup(BaseRequest<PaymentMgmtFilter> request);

        /// <summary>
        /// 获取LTC_RESIDENTBALANCE住民账户表
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<RsChargeGroup>> GetRsChargeGroup(BaseRequest<PaymentMgmtFilter> request);
    }
}
