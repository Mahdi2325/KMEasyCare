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

namespace KMHC.SLTC.Business.Implement
{
    public class AdvanceChargeService : BaseService, IAdvanceChargeService
    {
        #region LTC_PRECHARGE
        public BaseResponse<IList<PreCharge>> QueryPreCharge(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<PreCharge>> response = new BaseResponse<IList<PreCharge>>();
            var prechargedata = (from a in unitOfWork.GetRepository<LTC_PRECHARGE>().dbSet
                                 join b in unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet on a.BALANCEID equals b.BALANCEID
                                 join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.OPERATOR equals c.EMPNO
                                 where b.FEENO == request.Data.FEENO && a.ISDELETE == false

                                 orderby a.CREATETIME descending
                                 select new PreCharge()
                                 {
                                     PRECHARGEID = a.PRECHARGEID,
                                     BALANCEID = a.BALANCEID,
                                     AMOUNT = a.AMOUNT,
                                     PAYMENTTYPE = a.PAYMENTTYPE,
                                     PAYER = a.PAYER,
                                     PRECHARGETIME = a.PRECHARGETIME,
                                     OPERATOR = a.OPERATOR,
                                     EMPNAME = c.EMPNAME,
                                     RECEIPTNO = a.RECEIPTNO,
                                     COMMENT = a.COMMENT,
                                 }).ToList();

            response.RecordsCount = prechargedata.Count;
            List<PreCharge> list = null;
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

        public BaseResponse<List<PreCharge>> SavePreCharge(PreCharge request, string CreateBy, DateTime CreateDate)
        {
            BaseResponse<List<PreCharge>> response = new BaseResponse<List<PreCharge>>();
            response.Data = new List<PreCharge>();
            unitOfWork.BeginTransaction();
            var preItem = new PreCharge()
            {
                BALANCEID = request.BALANCEID,
                AMOUNT = request.AMOUNT,
                PAYMENTTYPE = request.PAYMENTTYPE,
                PAYER = request.PAYER,
                RECEIPTNO = request.RECEIPTNO,
                OPERATOR = request.OPERATOR,
                PRECHARGETIME = CreateDate,
                COMMENT = request.COMMENT,
                CREATEBY = CreateBy,
                CREATETIME = CreateDate,
                ISDELETE = false
            };
            base.Save<LTC_PRECHARGE, PreCharge>(preItem, (q) => q.PRECHARGEID == preItem.PRECHARGEID);

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
                            join d in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on a.FEENO equals d.FEENO into reg_nciinfo
                            from reg_nci in reg_nciinfo.DefaultIfEmpty()
                            where a.FEENO == request.Data.FEENO
                            orderby reg_nci.CREATETIME descending
                            select new ResidentBalance()
                            {
                                BalanceID = a.BALANCEID,
                                Name = c.NAME,
                                FeeNO = a.FEENO,
                                Blance = a.BLANCE,
                                Deposit = a.DEPOSIT,
                                TotalPayment = a.TOTALPAYMENT,
                                TotalCost = a.TOTALCOST,
                                CertStartTime = reg_nci.CERTSTARTTIME,
                                CertExpiredTime = reg_nci.CERTEXPIREDTIME,
                                ApplyHosTime = reg_nci.APPLYHOSTIME,
                                CertNo = reg_nci.CERTNO,
                                CertStatus = reg_nci.STATUS,
                                TotalNCIPay = a.TOTALNCIPAY,
                                TotalNCIOverspend = a.TOTALNCIOVERSPEND,
                                IsHaveNCI = a.ISHAVENCI,
                                NCIPayLevel = reg_nci.NCIPAYLEVEL,
                                NCIPayScale = reg_nci.NCIPAYSCALE,
                                Status = a.STATUS
                            }).ToList();
            response.Data = billData;
            return response;
        }


        public BaseResponse<ResidentBalance> SaveResidentBalance(ResidentBalance request, decimal Balance)
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
