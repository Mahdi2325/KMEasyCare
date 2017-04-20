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

namespace KMHC.SLTC.Business.Implement
{
    public class AdvanceChargeRefundService : BaseService, IAdvanceChargeRefundService
    {

        #region LTC_PRECHARGE
        public BaseResponse<IList<ResidentBalanceRefund>> QueryResidentBalanceRefund(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<ResidentBalanceRefund>> response = new BaseResponse<IList<ResidentBalanceRefund>>();
            var prechargedata = (from a in unitOfWork.GetRepository<LTC_RESIDENTBALANCEREFUND>().dbSet
                                 join b in unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet on a.BALANCEID equals b.BALANCEID
                                 join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.OPERATOR equals c.EMPNO
                                 where b.FEENO == request.Data.FEENO && a.ISDELETE== false

                                 orderby a.CREATETIME descending
                                    select new ResidentBalanceRefund()
                                    {
                                        BALANCEREFUNDID = a.BALANCEREFUNDID,
                                        BALANCEID = a.BALANCEID,
                                        REFUNDAMOUNT = a.REFUNDAMOUNT,
                                        REASON = a.REASON,
                                        OPERATOR = a.OPERATOR,
                                        EMPNAME=c.EMPNAME,
                                        RECEIVER =a.RECEIVER,
                                        PAYMENTTYPE =a.PAYMENTTYPE,
                                        REFUNDTIME=a.REFUNDTIME,
                                        CREATEBY = a.CREATEBY,
                                        CREATETIME = a.CREATETIME,
                                        UPDATEBY=a.UPDATEBY,
                                        UPDATETIME= a.UPDATETIME,
                                        ISDELETE= a.ISDELETE

                                    }).ToList();

            response.RecordsCount = prechargedata.Count;
            List<ResidentBalanceRefund> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = prechargedata.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = prechargedata.ToList();
            }

            response.Data = list;
            return response;
        }

        public BaseResponse<List<ResidentBalanceRefund>> SaveResidentBalanceRefund(ResidentBalanceRefund request, string CreateBy, DateTime CreateDate)
        {
            BaseResponse<List<ResidentBalanceRefund>> response = new BaseResponse<List<ResidentBalanceRefund>>();
            response.Data = new List<ResidentBalanceRefund>();
            unitOfWork.BeginTransaction();
            var preItem = new ResidentBalanceRefund()
            {
                BALANCEREFUNDID = request.BALANCEREFUNDID,
                BALANCEID = request.BALANCEID,
                REFUNDAMOUNT = request.REFUNDAMOUNT,
                REASON = request.REASON,
                RECEIVER = request.RECEIVER,
                OPERATOR = request.OPERATOR,
                PAYMENTTYPE = request.PAYMENTTYPE,
                REFUNDTIME = CreateDate,
                CREATEBY = CreateBy,
                CREATETIME = CreateDate,
                ISDELETE = false
            };

            base.Save<LTC_RESIDENTBALANCEREFUND, ResidentBalanceRefund>(preItem, (q) => q.BALANCEREFUNDID == preItem.BALANCEREFUNDID);

            unitOfWork.Commit();
            response.Data.Add(preItem);
            return response;
        }
        #endregion

        #region LTC_RESIDENTBALANCE
        public BaseResponse<IList<ResidentBalance>> QueryRESIDENTBALANCE(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<ResidentBalance>> response = new BaseResponse<IList<ResidentBalance>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet
                            join b in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals b.FEENO
                            join c in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on b.REGNO equals c.REGNO
                            join d in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on a.FEENO equals d.FEENO
                            where a.FEENO == request.Data.FEENO
                            orderby a.CREATETIME descending
                            select new ResidentBalance()
                            {
                                BalanceID = a.BALANCEID,
                                Name = c.NAME,
                                FeeNO = a.FEENO,
                                Blance = a.BLANCE,
                                Deposit = a.DEPOSIT,
                                TotalPayment = a.TOTALPAYMENT,
                                TotalCost = a.TOTALCOST,
                                TotalNCIPay = a.TOTALNCIPAY,
                                TotalNCIOverspend = a.TOTALNCIOVERSPEND,
                                IsHaveNCI = a.ISHAVENCI,
                                NCIPayLevel = d.NCIPAYLEVEL,
                                NCIPayScale = d.NCIPAYSCALE,
                                Status = a.STATUS
                            }).ToList();
            response.Data = billData;
            return response;
        }


        public BaseResponse<ResidentBalance> SaveResidentBalance(ResidentBalance request,decimal Balance)
        {
            var newItem = new ResidentBalance()
            {
                BalanceID = request.BalanceID,
                Name = request.Name,
                FeeNO = request.FeeNO,
                Deposit = request.Deposit,
                Blance = request.Blance + Balance,
                TotalPayment = request.TotalPayment,
                TotalCost = request.TotalPayment,
                TotalNCIPay = request.TotalPayment,
                TotalNCIOverspend = request.TotalPayment,
                IsHaveNCI = request.IsHaveNCI,
                NCIPayLevel = request.NCIPayLevel,
                NCIPayScale = request.NCIPayScale,
                Status = request.Status,
                Createby = request.Createby,
                CreateTime = request.CreateTime,
                UpdateBy = request.UpdateBy,
                UpdateTime = request.UpdateTime,
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
