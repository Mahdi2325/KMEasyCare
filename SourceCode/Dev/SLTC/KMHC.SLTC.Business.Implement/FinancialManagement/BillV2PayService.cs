using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using KMHC.Infrastructure;

namespace KMHC.SLTC.Business.Implement
{
    public class BillV2PayService : BaseService, IBillV2PayService
    {

        #region LTC_BILLV2PAY
        public BaseResponse<BillV2PAY> SaveBillV2Pay(BillV2PAY request, string CreateBy, DateTime CreateDate)
        {
            var newItem = new BillV2PAY()
            {
                SELFPAY = request.SELFPAY,
                NCIITEMTOTALCOST = request.NCIITEMTOTALCOST,
                NCIPAY = request.NCIPAY,
                NCIITEMSELFPAY = request.NCIITEMSELFPAY,
                ACCOUNTBALANCEPAY=request.ACCOUNTBALANCEPAY,
                OPERATOR = request.OPERATOR,
                PAYER = request.PAYER,
                PAYMENTTYPE = request.PAYMENTTYPE,
                INVOICENO = request.INVOICENO,
                FEENO = request.FEENO,
                PAYTIME = CreateDate,
                CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                CREATETIME = CreateDate,
                UPDATEBY = request.CREATEBY,
                UPDATETIME = request.UPDATETIME,
                ISDELETE = request.ISDELETE
            };
            var billInfo = unitOfWork.GetRepository<LTC_BILLV2PAY>().dbSet.Where(m => m.BILLPAYID == newItem.BILLPAYID).FirstOrDefault();
            if (billInfo != null)
            {
                unitOfWork.GetRepository<LTC_BILLV2PAY>().Update(billInfo);
                unitOfWork.Save();
            }
            return base.Save<LTC_BILLV2PAY, BillV2PAY>(newItem, (q) => q.BILLPAYID == newItem.BILLPAYID);
        }
        #endregion
    }
}
