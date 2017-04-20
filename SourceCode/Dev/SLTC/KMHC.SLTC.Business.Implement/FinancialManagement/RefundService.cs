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
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using KMHC.Infrastructure;

namespace KMHC.SLTC.Business.Implement
{
    public class RefundService : BaseService, IRefundService
    {

        #region LTC_BILLV2PAY
        public BaseResponse<Refund> SaveRefund(Refund request, string CreateBy, DateTime CreateDate)
        {
            var newItem = new Refund()
            {
                FEENO = request.FEENO,
                SELFPAY = request.SELFPAY,
                NCIITEMTOTALCOST = request.NCIITEMTOTALCOST,
                NCIPAY = request.NCIPAY,
                NCIITEMSELFPAY = request.NCIITEMSELFPAY,
                REFUNDREASON=request.REFUNDREASON,
                REFUNDAMOUNT = request.REFUNDAMOUNT,
                COMMENT=request.COMMENT,
                OPERATOR = request.OPERATOR,
                RECEIVER = request.RECEIVER,
                PAYMENTTYPE = request.PAYMENTTYPE,
                REFUNDTIME = CreateDate,
                CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                CREATETIME = CreateDate,
                UPDATEBY = request.CREATEBY,
                UPDATETIME = request.UPDATETIME,
                ISDELETE = false
            };

            var billInfo = unitOfWork.GetRepository<LTC_REFUND>().dbSet.Where(m => m.REFUNDRECORDID == newItem.REFUNDRECORDID).FirstOrDefault();
            if (billInfo != null)
            {
                unitOfWork.GetRepository<LTC_REFUND>().Update(billInfo);
                unitOfWork.Save();
            }
            return base.Save<LTC_REFUND, Refund>(newItem, (q) => q.REFUNDRECORDID == newItem.REFUNDRECORDID);
        }

        #endregion
    }
}
