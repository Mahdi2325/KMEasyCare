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
    public interface IRefundService
    {

        #region LTC_REFUND
        /// <summary>
        /// 保存LTC_REFUND退费记录表
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Refund> SaveRefund(Refund request, string CreateBy, DateTime CreateDate);
        #endregion
    }
}
