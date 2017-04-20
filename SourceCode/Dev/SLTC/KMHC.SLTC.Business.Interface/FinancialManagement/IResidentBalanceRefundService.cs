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
    public interface IResidentBalanceRefundService
    {

        #region LTC_RESIDENTBALANCE
        /// <summary>
        /// 保存LTC_RESIDENTBALANCE住民账户表
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<ResidentBalance> SaveResidentBalance(ResidentBalance request, string UpdateBy, DateTime UpdateDate);
        #endregion
    }
}
