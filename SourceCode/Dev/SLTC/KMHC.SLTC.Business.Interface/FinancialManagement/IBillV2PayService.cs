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
    public interface IBillV2PayService
    {

        #region LTC_BillV2PAY
        /// <summary>
        /// 保存LTC_BillV2PAY缴费记录表
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<BillV2PAY> SaveBillV2Pay(BillV2PAY request, string CreateBy, DateTime CreateDate);
        #endregion
    }
}
