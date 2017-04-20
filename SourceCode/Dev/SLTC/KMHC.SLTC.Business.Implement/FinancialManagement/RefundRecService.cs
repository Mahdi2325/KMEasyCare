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
    public class RefundRecService : BaseService, IRefundRecService
    {
        #region LTC_BILLV2
        public BaseResponse<IList<BillV2>> QueryBillV2(BaseRequest<RefundMgmtFilter> request)
        {
            BaseResponse<IList<BillV2>> response = new BaseResponse<IList<BillV2>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                            join b in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.BALANCEOPERATOR equals b.EMPNO
                            join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.REFUNDOPERATOR equals c.EMPNO
                            join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.BILLCREATOR equals d.EMPNO
                            join e in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on a.FEENO equals e.FEENO into reg_nciinfo
                            from reg_nci in reg_nciinfo.DefaultIfEmpty()
                            where a.FEENO == request.Data.FEENO && a.STATUS== request.Data.STATUS && a.ISDELETE == false
                            orderby a.CREATETIME descending
                            select new BillV2()
                            {
                                BillId = a.BILLID,
                                BillPayId = a.BILLPAYID,
                                BillMonth=a.BILLMONTH,
                                ReFundRecordId = a.REFUNDRECORDID,
                                FeeNo = a.FEENO,
                                NCIItemTotalCost = a.NCIITEMTOTALCOST,
                                SelfPay = a.SELFPAY,
                                NCIPayLevel = reg_nci.NCIPAYLEVEL,
                                NCIPaysCale = reg_nci.NCIPAYSCALE,
                                NCIPay = a.NCIPAY,
                                NCIItemSelfPay = a.NCIITEMSELFPAY,
                                BalanceStartTime = a.BALANCESTARTTIME,
                                BalanceEndTime = a.BALANCEENDTIME,
                                HospDay = a.HOSPDAY,
                                Status = 8,
                                BillCreator = a.BILLCREATOR,
                                BilleName = d.EMPNAME,
                                BalanceOperator = a.BALANCEOPERATOR,
                                BalanceeName=b.EMPNAME,
                                RefundOperator = a.REFUNDOPERATOR,
                                RefundeName = c.EMPNAME,
                                CreateBy = a.CREATEBY,
                                CreateTime = a.CREATETIME,
                                UpdateBy = a.UPDATEBY,
                                UpdateTime = a.UPDATETIME,
                                IsDelete = a.ISDELETE,
                            }).Distinct().ToList();
            response.RecordsCount = billData.Count;
            List<BillV2> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = billData.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = billData.ToList();
            }

            response.Data = list;
            return response;
        }
        #endregion

        #region LTC_REFUND
        public BaseResponse<IList<Refund>> QueryRefund(BaseRequest<RefundMgmtFilter> request)
        {
            BaseResponse<IList<Refund>> response = new BaseResponse<IList<Refund>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_REFUND>().dbSet
                            join b in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.OPERATOR equals b.EMPNO
                            where a.REFUNDRECORDID== request.Data.REFUNDRECORDID && a.ISDELETE == false
                            orderby a.CREATETIME descending
                            select new Refund()
                            {
                                REFUNDRECORDID = a.REFUNDRECORDID,
                                FEENO=a.FEENO,
                                NEWBILLID=a.NEWBILLID,
                                REFUNDREASON=a.REFUNDREASON,
                                SELFPAY = a.SELFPAY,
                                NCIITEMTOTALCOST = a.NCIITEMTOTALCOST,
                                NCIPAY = a.NCIPAY,
                                NCIITEMSELFPAY = a.NCIITEMSELFPAY,
                                REFUNDAMOUNT=a.REFUNDAMOUNT,
                                COMMENT=a.COMMENT,
                                OPERATOR = a.OPERATOR,
                                EMPNAME = b.EMPNAME,
                                RECEIVER = a.RECEIVER,
                                PAYMENTTYPE = a.PAYMENTTYPE,
                                REFUNDTIME=a.REFUNDTIME,
                                CREATEBY = a.CREATEBY,
                                CREATETIME = a.CREATETIME,
                                UPDATEBY = a.UPDATEBY,
                                UPDATETIME= a.UPDATETIME,
                                ISDELETE = a.ISDELETE
                            }).ToList();
            response.Data = billData;
            return response;
        }
        #endregion

        #region LTC_FEERECORD
        public BaseResponse<IList<FeeRecord>> QueryFeeRecord(BaseRequest<RefundMgmtFilter> request)
        {
            BaseResponse<IList<FeeRecord>> response = new BaseResponse<IList<FeeRecord>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                            join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                            join c in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on b.CHARGERECORDID equals c.DRUGRECORDID
                            join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on c.DRUGID equals d.DRUGID
                            where a.BILLID == request.Data.BILLID && b.CHARGERECORDTYPE==1 && a.ISDELETE == false && b.ISREFUNDRECORD== true
                            orderby b.FEERECORDID descending

                            select new FeeRecord()
                            {
                                BILLID = b.BILLID,
                                FEERECORDID = b.FEERECORDID,
                                CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                CHARGETYPEID = d.CHARGETYPEID,
                                MCDRUGCODE = d.MCDRUGCODE,
                                INNERCODE = d.NSDRUGCODE,
                                CNNAME = c.CNNAME,
                                UNITS = c.UNITS,
                                UNITPRICE = b.UNITPRICE,
                                COUNT = b.COUNT,
                                COST = b.COST,
                                ISNCIITEM = b.ISNCIITEM,
                                ISCHARGEGROUPITEM = c.ISCHARGEGROUPITEM,
                                ISREFUNDRECORD=b.ISREFUNDRECORD,
                                OPERATOR=c.OPERATOR,
                                CREATETIME =b.CREATETIME
                            }).Union(from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                                     join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                                     join c in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on b.CHARGERECORDID equals c.MATERIALRECORDID
                                     join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.MATERIALID equals d.MATERIALID
                                     where a.BILLID == request.Data.BILLID && b.CHARGERECORDTYPE == 2 && a.ISDELETE == false && b.ISREFUNDRECORD == true
                                     orderby b.FEERECORDID descending

                                     select new FeeRecord()
                                     {
                                         BILLID = b.BILLID,
                                         FEERECORDID = b.FEERECORDID,
                                         CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                         CHARGETYPEID = d.CHARGETYPEID,
                                         MCDRUGCODE = d.MCMATERIALCODE,
                                         INNERCODE = d.NSMATERIALCODE,
                                         CNNAME = c.MATERIALNAME,
                                         UNITS = c.UNITS,
                                         UNITPRICE = b.UNITPRICE,
                                         COUNT = b.COUNT,
                                         COST = b.COST,
                                         ISNCIITEM = b.ISNCIITEM,
                                         ISCHARGEGROUPITEM = c.ISCHARGEGROUPITEM,
                                         ISREFUNDRECORD = b.ISREFUNDRECORD,
                                         OPERATOR = c.OPERATOR,
                                         CREATETIME = b.CREATETIME
                                     }).Union(from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                                                     join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                                                     join c in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on b.CHARGERECORDID equals c.SERVICERECORDID
                                              join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on c.SERVICEID equals d.SERVICEID
                                                     where a.BILLID == request.Data.BILLID && b.CHARGERECORDTYPE == 3  && a.ISDELETE == false && b.ISREFUNDRECORD == true
                                              orderby b.FEERECORDID descending

                                                     select new FeeRecord()
                                                     {
                                                         BILLID = b.BILLID,
                                                         FEERECORDID = b.FEERECORDID,
                                                         CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                                         CHARGETYPEID = d.CHARGETYPEID,
                                                         MCDRUGCODE = d.MCSERVICECODE,
                                                         INNERCODE = d.NSSERVICECODE,
                                                         CNNAME = c.SERVICENAME,
                                                         UNITS = c.UNITS,
                                                         UNITPRICE = b.UNITPRICE,
                                                         COUNT = b.COUNT,
                                                         COST = b.COST,
                                                         ISNCIITEM = b.ISNCIITEM,
                                                         ISCHARGEGROUPITEM = c.ISCHARGEGROUPITEM,
                                                         ISREFUNDRECORD = b.ISREFUNDRECORD,
                                                         OPERATOR = c.OPERATOR,
                                                         CREATETIME = b.CREATETIME
                                                     }).ToList();

            response.RecordsCount = billData.Count;
            List<FeeRecord> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = billData.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = billData.ToList();
            }

            response.Data = list;
            return response;
        }
        #endregion
    }
}
