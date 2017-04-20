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
    public class ResidentBalanceRefundService : BaseService, IResidentBalanceRefundService
    {
        #region LTC_RESIDENTBALANCE
        public BaseResponse<ResidentBalance> SaveResidentBalance(ResidentBalance request, string UpdateBy, DateTime UpdateDate)
        {
            var newItem = new ResidentBalance()
            {
                BalanceID = request.BalanceID,
                Name = request.Name,
                FeeNO = request.FeeNO,
                Deposit = request.Deposit,
                Blance = request.Blance - request.RefundAmount,
                TotalPayment = request.TotalPayment,
                TotalCost = request.TotalCost,
                TotalNCIPay = request.TotalNCIPay,
                TotalNCIOverspend = request.TotalNCIOverspend,
                IsHaveNCI = request.IsHaveNCI,
                Status = request.Status,
                Createby = request.Createby,
                CreateTime = request.CreateTime,
                UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo,
                UpdateTime = UpdateDate,
                IsDelete = request.IsDelete
            };
            if (newItem.BalanceID != null)
            {
                var ResidentBalanceInfo = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.Where(m => m.BALANCEID == newItem.BalanceID).FirstOrDefault();
                if (ResidentBalanceInfo != null)
                {
                    unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().Update(ResidentBalanceInfo);
                    unitOfWork.Save();
                }
            }
            return base.Save<LTC_RESIDENTBALANCE, ResidentBalance>(newItem, (q) => q.BALANCEID == newItem.BalanceID);
        }

        #endregion
    }
}
