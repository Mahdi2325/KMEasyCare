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
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;

namespace KMHC.SLTC.Business.Implement
{
    public class PaymentMgmtService : BaseService, IPaymentMgmtService
    {
        #region LTC_BILLV2
        public BaseResponse<IList<BillV2>> QueryBillV2(BaseRequest<PaymentMgmtFilter> request)
        {
            decimal TtlAmount = 0;
            decimal TtlZiFeiAmt = 0;
            decimal NCIAmt = 0;
            decimal GuDingAmt = 0;
            decimal GuDingZiFeiAmt = 0;
            decimal NoGuDingAmt = 0;
            decimal ZiFeiAmt = 0;
            decimal ZiFuAmt = 0;
            decimal ThisSelfAmt = 0;
            decimal LastPreAmt = 0;

            BaseResponse<IList<BillV2>> response = new BaseResponse<IList<BillV2>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                            join b in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.BILLCREATOR equals b.EMPNO
                            where a.FEENO == request.Data.FEENO && a.STATUS == request.Data.STATUS && a.ISDELETE == false
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
                                NCIPayLevel = a.NCIPAYLEVEL,
                                NCIPaysCale = a.NCIPAYSCALE,
                                NCIPay = a.NCIPAY,
                                NCIItemSelfPay = a.NCIITEMSELFPAY,
                                BalanceStartTime = a.BALANCESTARTTIME,
                                BalanceEndTime = a.BALANCEENDTIME,
                                HospDay = a.HOSPDAY,
                                Status = a.STATUS,
                                BillCreator = a.BILLCREATOR,
                                BilleName=b.EMPNAME,
                                BalanceOperator = a.BALANCEOPERATOR,
                                RefundOperator = a.REFUNDOPERATOR,
                                CreateBy = a.CREATEBY,
                                CreateTime = a.CREATETIME,
                                UpdateBy = a.UPDATEBY,
                                UpdateTime = a.UPDATETIME,
                                IsDelete = a.ISDELETE,
                            }).Distinct().ToList();

            foreach (var item in billData)
            {
                TtlAmount = TtlAmount + item.NCIItemTotalCost +item.SelfPay;
                NCIAmt = NCIAmt + item.NCIPay;
                ZiFuAmt = ZiFuAmt + item.NCIItemSelfPay;
                ZiFeiAmt = ZiFeiAmt+ item.SelfPay;

                var billDtlData = (from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                                join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                                join c in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on b.CHARGERECORDID equals c.DRUGRECORDID
                                join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on c.DRUGID equals d.DRUGID
                                where a.BILLID == item.BillId && b.CHARGERECORDTYPE == 1 && a.ISDELETE == false
                                orderby b.FEERECORDID descending

                                select new FeeRecord()
                                {
                                    BILLID = b.BILLID,
                                    FEERECORDID = b.FEERECORDID,
                                    CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                    CHARGEITEMID = c.DRUGID,
                                    MCDRUGCODE = d.MCDRUGCODE,
                                    INNERCODE = d.NSDRUGCODE,
                                    CNNAME = c.CNNAME,
                                    UNITS = c.UNITS,
                                    UNITPRICE = b.UNITPRICE,
                                    COUNT = b.COUNT,
                                    COST = b.COST,
                                    ISNCIITEM = b.ISNCIITEM,
                                    ISCHARGEGROUPITEM = c.ISCHARGEGROUPITEM,
                                    ISREFUNDRECORD = b.ISREFUNDRECORD,
                                    OPERATOR = c.OPERATOR,
                                    TAKETIME = c.TAKETIME,
                                    CREATETIME = b.CREATETIME
                                }).Union(from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                                         join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                                         join c in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on b.CHARGERECORDID equals c.MATERIALRECORDID
                                         join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.MATERIALID equals d.MATERIALID
                                         where a.BILLID == item.BillId && b.CHARGERECORDTYPE == 2 && a.ISDELETE == false
                                         orderby b.FEERECORDID descending

                                         select new FeeRecord()
                                         {
                                             BILLID = b.BILLID,
                                             FEERECORDID = b.FEERECORDID,
                                             CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                             CHARGEITEMID = c.MATERIALID,
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
                                             TAKETIME = c.TAKETIME,
                                             CREATETIME = b.CREATETIME
                                         }).Union(from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                                                  join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                                                  join c in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on b.CHARGERECORDID equals c.SERVICERECORDID
                                                  join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on c.SERVICEID equals d.SERVICEID
                                                  where a.BILLID == item.BillId && b.CHARGERECORDTYPE == 3 && a.ISDELETE == false
                                                  orderby b.FEERECORDID descending

                                                  select new FeeRecord()
                                                  {
                                                      BILLID = b.BILLID,
                                                      FEERECORDID = b.FEERECORDID,
                                                      CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                                      CHARGEITEMID = c.SERVICEID,
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
                                                      TAKETIME = c.TAKETIME,
                                                      CREATETIME = b.CREATETIME
                                                  }).ToList();


                foreach (var itemdtl in billDtlData)
                {
                    if (itemdtl.CHARGERECORDTYPE == 1)
                    {
                        var chargeGroupData = (from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                               join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                               join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                               join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on c.CHARGEITEMID equals d.DRUGID
                                               join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                               where a.FEENO == item.FeeNo && c.CHARGEITEMTYPE == itemdtl.CHARGERECORDTYPE && c.CHARGEITEMID == itemdtl.CHARGEITEMID
                                               && (a.ISDELETE == null || a.ISDELETE == false) && (b.ISDELETE == null || b.ISDELETE == false)
                                               && (c.ISDELETE == null || c.ISDELETE == false) && a.STATUS == 0 && b.STATUS == 0
                                               orderby c.CHARGEITEMID descending
                                               select new RsChargeGroup()
                                               {
                                                   CGRId = a.CGRID,
                                                   ChargeGroupId = b.CHARGEGROUPID,
                                                   ChargeGroupName = b.CHARGEGROUPNAME,
                                                   FeeNo = a.FEENO,
                                                   Status = d.STATUS,
                                                   ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                                   ChargeItemId = c.CHARGEITEMID,
                                                   ChargeItemType = c.CHARGEITEMTYPE,
                                                   ConversionRatio = d.CONVERSIONRATIO ?? 1,
                                                   FeeItemCount = c.FEEITEMCOUNT,
                                                   MCCode = d.MCDRUGCODE,
                                                   NSCode = d.NSDRUGCODE,
                                                   Name = d.CNNAME,
                                                   IsNciItem = d.ISNCIITEM,
                                                   Spec = d.SPEC,
                                                   Units = d.UNITS,
                                                   UnitPrice = d.UNITPRICE
                                               }).ToList();

                        if (chargeGroupData.Count > 0)
                        {
                            if (chargeGroupData[0].IsNciItem != true)
                            {
                                GuDingZiFeiAmt = GuDingZiFeiAmt + itemdtl.COST;
                            }
                            else
                            {
                                GuDingAmt = GuDingAmt + itemdtl.COST;
                            }
                        }else
                        {
                            NoGuDingAmt = NoGuDingAmt + itemdtl.COST;
                        }

                    }
                    else if (itemdtl.CHARGERECORDTYPE == 2)
                    {
                        var chargeGroupData = (from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                               join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                               join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                               join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.CHARGEITEMID equals d.MATERIALID
                                               join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                               where a.FEENO == item.FeeNo && c.CHARGEITEMTYPE == itemdtl.CHARGERECORDTYPE && c.CHARGEITEMID == itemdtl.CHARGEITEMID
                                                && (a.ISDELETE == null || a.ISDELETE == false) && (b.ISDELETE == null || b.ISDELETE == false)
                                                && (c.ISDELETE == null || c.ISDELETE == false) && a.STATUS == 0 && b.STATUS == 0
                                               orderby c.CHARGEITEMID descending

                                               select new RsChargeGroup()
                                               {
                                                   CGRId = a.CGRID,
                                                   ChargeGroupId = b.CHARGEGROUPID,
                                                   ChargeGroupName = b.CHARGEGROUPNAME,
                                                   FeeNo = a.FEENO,
                                                   Status = d.STATUS,
                                                   ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                                   ChargeItemId = c.CHARGEITEMID,
                                                   ChargeItemType = c.CHARGEITEMTYPE,
                                                   ConversionRatio = 1,
                                                   FeeItemCount = c.FEEITEMCOUNT,
                                                   MCCode = d.MCMATERIALCODE,
                                                   NSCode = d.NSMATERIALCODE,
                                                   Name = d.MATERIALNAME,
                                                   IsNciItem = d.ISNCIITEM,
                                                   Spec = d.SPEC,
                                                   Units = d.UNITS,
                                                   UnitPrice = d.UNITPRICE
                                               }).ToList();
                        if (chargeGroupData.Count > 0)
                        {
                            if (chargeGroupData[0].IsNciItem != true)
                            {
                                GuDingZiFeiAmt = GuDingZiFeiAmt + itemdtl.COST;
                            }
                            else
                            {
                                GuDingAmt = GuDingAmt + itemdtl.COST;
                            }
                        }
                        else
                        {
                            NoGuDingAmt = NoGuDingAmt + itemdtl.COST;
                        }
                    }
                    else if (itemdtl.CHARGERECORDTYPE == 3)
                    {
                        var chargeGroupData = (from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                               join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                               join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                               join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on c.CHARGEITEMID equals d.SERVICEID
                                               join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                               where a.FEENO == item.FeeNo && c.CHARGEITEMTYPE == itemdtl.CHARGERECORDTYPE && c.CHARGEITEMID == itemdtl.CHARGEITEMID
                                               && a.ISDELETE !=true && b.ISDELETE !=true
                                                && c.ISDELETE !=true && a.STATUS == 0 && b.STATUS == 0
                                               orderby c.CHARGEITEMID descending

                                               select new RsChargeGroup()
                                               {
                                                   CGRId = a.CGRID,
                                                   ChargeGroupId = b.CHARGEGROUPID,
                                                   ChargeGroupName = b.CHARGEGROUPNAME,
                                                   FeeNo = a.FEENO,
                                                   Status = d.STATUS,
                                                   ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                                   ChargeItemId = c.CHARGEITEMID,
                                                   ChargeItemType = c.CHARGEITEMTYPE,
                                                   ConversionRatio = 1,
                                                   FeeItemCount = c.FEEITEMCOUNT,
                                                   MCCode = d.MCSERVICECODE,
                                                   NSCode = d.NSSERVICECODE,
                                                   Name = d.SERVICENAME,
                                                   IsNciItem = d.ISNCIITEM,
                                                   Spec = "",
                                                   Units = d.UNITS,
                                                   UnitPrice = d.UNITPRICE
                                               }).ToList();
                        if (chargeGroupData.Count > 0)
                        {
                            if (chargeGroupData[0].IsNciItem != true)
                            {
                                GuDingZiFeiAmt = GuDingZiFeiAmt + itemdtl.COST;
                            }
                            else
                            {
                                GuDingAmt = GuDingAmt + itemdtl.COST;
                            }
                        }
                        else
                        {
                            NoGuDingAmt = NoGuDingAmt + itemdtl.COST;
                        }
                    }
                }
            }

            
            if (GuDingAmt - GuDingZiFeiAmt > 0)
            {
                //总自负费=护理险总费用-护理险固定费用
                TtlZiFeiAmt = TtlAmount - (GuDingAmt - GuDingZiFeiAmt);

                if (TtlZiFeiAmt >= 0)
                {
                    //固定费用大于报销金额
                    if ((GuDingAmt - GuDingZiFeiAmt) > NCIAmt)
                    {
                        LastPreAmt = GuDingAmt - GuDingZiFeiAmt - NCIAmt;
                        ThisSelfAmt = TtlZiFeiAmt;
                    }
                    else
                    {
                        LastPreAmt = 0;
                        ThisSelfAmt = TtlZiFeiAmt - (NCIAmt - GuDingAmt);
                    }
                }
                else
                {
                    ////固定费用大于报销金额
                    //if ((GuDingAmt - GuDingZiFeiAmt) > NCIAmt)
                    //{
                    //    LastPreAmt = GuDingAmt - GuDingZiFeiAmt - NCIAmt;
                    //    ThisSelfAmt = TtlZiFeiAmt;
                    //}
                    //else
                    //{
                    //    LastPreAmt = 0;
                    //    ThisSelfAmt = TtlZiFeiAmt - (NCIAmt - GuDingAmt);
                    //}
                    LastPreAmt = GuDingAmt - GuDingZiFeiAmt;
                    ThisSelfAmt = (GuDingAmt - GuDingZiFeiAmt) - TtlAmount;
                }
            }
            else
            {
                LastPreAmt = 0;
                ThisSelfAmt = ZiFeiAmt + ZiFuAmt;
            }

            foreach (var billdata in billData)
            {
                billdata.ThisSelfAmt = ThisSelfAmt;
                billdata.LastPreAmt = LastPreAmt;
            }

            response.Data = billData;
            return response;
        }
        #endregion

        #region LTC_PRECHARGE
        public BaseResponse<IList<PreCharge>> QueryPreCharge(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<PreCharge>> response = new BaseResponse<IList<PreCharge>>();
            var prechargedata = (from a in unitOfWork.GetRepository<LTC_PRECHARGE>().dbSet
                                 join b in unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet on a.BALANCEID equals b.BALANCEID
                                 where b.FEENO == request.Data.FEENO && a.ISDELETE== false
                                 orderby a.CREATETIME descending
                                    select new PreCharge()
                                    {
                                        PRECHARGEID = a.PRECHARGEID,
                                        BALANCEID = a.BALANCEID,
                                        PAYMENTTYPE = a.PAYMENTTYPE,
                                        PAYER = a.PAYER,
                                        RECEIPTNO = a.RECEIPTNO,
                                        COMMENT = a.COMMENT,
                                    }).ToList();
                response.Data = prechargedata;
                return response;
        }

        public BaseResponse<BillV2> SaveBillV2(BillV2 request, string UpdateBy, DateTime UpdateDate)
        {
            var newItem = new BillV2()
            {
                BillId=request.BillId,
                BillPayId = request.BillPayId,
                ReFundRecordId = request.ReFundRecordId,
                FeeNo = request.FeeNo,
                NCIItemTotalCost = request.NCIItemTotalCost,
                SelfPay = request.SelfPay,
                NCIPayLevel = request.NCIPayLevel,
                NCIPaysCale = request.NCIPaysCale,
                NCIPay = request.NCIPay,
                NCIItemSelfPay = request.NCIItemSelfPay,
                BalanceStartTime = request.BalanceStartTime,
                BalanceEndTime = request.BalanceEndTime,
                HospDay = request.HospDay,
                Status = 2,
                BillCreator = request.BillCreator,
                BalanceOperator = request.BalanceOperator,
                RefundOperator = request.RefundOperator,
                CreateBy = request.CreateBy,
                CreateTime = request.CreateTime,
                UpdateBy = UpdateBy,
                UpdateTime = UpdateDate,
                IsDelete = request.IsDelete,
            };
            if (newItem.BillId != null)
            {
                var billInfo = unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(m => m.BILLID == newItem.BillId).FirstOrDefault();
                if (billInfo != null)
                {
                    unitOfWork.GetRepository<LTC_BILLV2>().Update(billInfo);
                    unitOfWork.Save();
                }
            };
            return base.Save<LTC_BILLV2, BillV2>(newItem, (q) => q.BILLID == newItem.BillId);
        }
        #endregion

        #region LTC_FEERECORD
        public BaseResponse<IList<FeeRecord>> QueryFeeRecord(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<FeeRecord>> response = new BaseResponse<IList<FeeRecord>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                            join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                            join c in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on b.CHARGERECORDID equals c.DRUGRECORDID
                            join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on c.DRUGID equals d.DRUGID
                            where a.BILLID == request.Data.BILLID && b.CHARGERECORDTYPE == 1 && a.ISDELETE == false
                            orderby b.FEERECORDID descending

                            select new FeeRecord()
                            {
                                BILLID = b.BILLID,
                                FEERECORDID = b.FEERECORDID,
                                CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                CHARGEITEMID=c.DRUGID,
                                CHARGETYPEID=d.CHARGETYPEID,
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
                                TAKETIME=c.TAKETIME,
                                CREATETIME =b.CREATETIME
                            }).Union(from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                                     join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                                     join c in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on b.CHARGERECORDID equals c.MATERIALRECORDID
                                     join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.MATERIALID equals d.MATERIALID
                                     where a.BILLID == request.Data.BILLID && b.CHARGERECORDTYPE == 2 && a.ISDELETE == false
                                     orderby b.FEERECORDID descending

                                     select new FeeRecord()
                                     {
                                         BILLID = b.BILLID,
                                         FEERECORDID = b.FEERECORDID,
                                         CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                         CHARGEITEMID = c.MATERIALID,
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
                                         TAKETIME = c.TAKETIME,
                                         CREATETIME = b.CREATETIME
                                     }).Union(from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                                                     join b in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet on a.BILLID equals b.BILLID
                                                     join c in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on b.CHARGERECORDID equals c.SERVICERECORDID
                                              join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on c.SERVICEID equals d.SERVICEID
                                                     where a.BILLID == request.Data.BILLID && b.CHARGERECORDTYPE == 3  && a.ISDELETE == false
                                                     orderby b.FEERECORDID descending

                                                     select new FeeRecord()
                                                     {
                                                         BILLID = b.BILLID,
                                                         FEERECORDID = b.FEERECORDID,
                                                         CHARGERECORDTYPE = b.CHARGERECORDTYPE,
                                                         CHARGEITEMID=c.SERVICEID,
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
                                                         TAKETIME = c.TAKETIME,
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
                                Createby=a.CREATEBY,
                                CreateTime=a.CREATETIME,
                                Status = a.STATUS,
                                IsDelete=a.ISDELETE
                            }).ToList();
            response.Data = billData;
            return response;
        }

        public BaseResponse<List<BillV2>> SaveBillV2Info(BillV2List request)
        {
            var feeRecordListOfDrug = new List<ChargeRecord>();
            BaseResponse<List<BillV2>> response = new BaseResponse<List<BillV2>>();
            if (request.BillV2Lists != null && request.BillV2Lists.Count > 0)
            {
                #region 账单记录
                foreach (var item in request.BillV2Lists)
                {
                    LTC_BILLV2 model = new LTC_BILLV2();
                    model.BILLID = item.BillId;
                    model.BILLPAYID = item.BillPayId;
                    model.BILLMONTH = item.BillMonth;
                    model.REFUNDRECORDID = item.ReFundRecordId;
                    model.FEENO = item.FeeNo;
                    model.NCIITEMTOTALCOST = item.NCIItemTotalCost;
                    model.SELFPAY = item.SelfPay;
                    model.NCIPAYLEVEL = item.NCIPayLevel;
                    model.NCIPAYSCALE = item.NCIPaysCale;
                    model.NCIPAY = item.NCIPay;
                    model.NCIITEMSELFPAY = item.NCIItemSelfPay;
                    model.BALANCESTARTTIME = item.BalanceStartTime;
                    model.BALANCEENDTIME = item.BalanceEndTime;
                    model.HOSPDAY = item.HospDay;
                    model.STATUS = item.Status;
                    model.BILLCREATOR = item.BillCreator;
                    model.BALANCEOPERATOR = item.BalanceOperator;
                    model.CREATEBY = item.CreateBy;
                    model.CREATETIME = item.CreateTime;
                    model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.UPDATETIME = DateTime.Now;
                    model.ISDELETE = item.IsDelete;
                    model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                    unitOfWork.GetRepository<LTC_BILLV2>().Update(model);

                    var feeRecs = unitOfWork.GetRepository<LTC_FEERECORD>().dbSet.Where(m => m.BILLID == item.BillId).ToList();
                    if (feeRecs != null)
                    {
                        foreach (var feeRec in feeRecs)
                        {
                            if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Drug))
                            {
                                var drug = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.FirstOrDefault(m => m.DRUGRECORDID == feeRec.CHARGERECORDID);
                                drug.STATUS = Convert.ToInt32(RecordStatus.Charge);
                                drug.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                drug.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(drug);
                            }
                            else if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Material))
                            {
                                var material = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.FirstOrDefault(m => m.MATERIALRECORDID == feeRec.CHARGERECORDID);
                                material.STATUS = Convert.ToInt32(RecordStatus.Charge);
                                material.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                material.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(material);
                            }
                            else if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Service))
                            {
                                var service = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.FirstOrDefault(m => m.SERVICERECORDID == feeRec.CHARGERECORDID);
                                service.STATUS = Convert.ToInt32(RecordStatus.Charge);
                                service.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                service.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(service);
                            }
                        }
                    }

                }
                unitOfWork.Save();
                response.ResultCode = 1001;
                #endregion
            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "未查询到有效的账单数据！";
            }
            return response;
        }

        public BaseResponse<IList<RsChargeGroup>> GetRsChargeGroup(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<RsChargeGroup>> response = new BaseResponse<IList<RsChargeGroup>>();
            if (request.Data.CHARGERECORDTYPE == 1)
            {
                var chargeGroupData = (from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                       join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                       join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                       join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on c.CHARGEITEMID equals d.DRUGID
                                       join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                       where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == request.Data.CHARGERECORDTYPE && c.CHARGEITEMID==request.Data.FEERECORDID 
                                       && a.ISDELETE != true && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS == 0 && b.STATUS == 0
                                       orderby c.CHARGEITEMID descending
                                       select new RsChargeGroup()
                                       {
                                           CGRId = a.CGRID,
                                           ChargeGroupId = b.CHARGEGROUPID,
                                           ChargeGroupName = b.CHARGEGROUPNAME,
                                           FeeNo = a.FEENO,
                                           Status = d.STATUS,
                                           ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                           ChargeItemId = c.CHARGEITEMID,
                                           ChargeItemType = c.CHARGEITEMTYPE,
                                           ChargeTypeId = d.CHARGETYPEID,
                                           ConversionRatio = d.CONVERSIONRATIO ?? 1,
                                           FeeItemCount = c.FEEITEMCOUNT,
                                           MCCode = d.MCDRUGCODE,
                                           NSCode = d.NSDRUGCODE,
                                           Name = d.CNNAME,
                                           IsNciItem = d.ISNCIITEM,
                                           Spec = d.SPEC,
                                           Units = d.UNITS,
                                           UnitPrice = d.UNITPRICE
                                       }).ToList();
                response.RecordsCount = chargeGroupData.Count;
                response.Data = chargeGroupData;
                
            }
            else if(request.Data.CHARGERECORDTYPE == 2){
                var chargeGroupData = (from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                       join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                       join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                       join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.CHARGEITEMID equals d.MATERIALID
                                       join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                       where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == request.Data.CHARGERECORDTYPE && c.CHARGEITEMID == request.Data.FEERECORDID
                                        && a.ISDELETE != true && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS == 0 && b.STATUS == 0
                                       orderby c.CHARGEITEMID descending

                                       select new RsChargeGroup()
                                       {
                                           CGRId = a.CGRID,
                                           ChargeGroupId = b.CHARGEGROUPID,
                                           ChargeGroupName = b.CHARGEGROUPNAME,
                                           FeeNo = a.FEENO,
                                           Status = d.STATUS,
                                           ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                           ChargeItemId = c.CHARGEITEMID,
                                           ChargeItemType = c.CHARGEITEMTYPE,
                                           ChargeTypeId = d.CHARGETYPEID,
                                           ConversionRatio = 1,
                                           FeeItemCount = c.FEEITEMCOUNT,
                                           MCCode = d.MCMATERIALCODE,
                                           NSCode = d.NSMATERIALCODE,
                                           Name = d.MATERIALNAME,
                                           IsNciItem = d.ISNCIITEM,
                                           Spec = d.SPEC,
                                           Units = d.UNITS,
                                           UnitPrice = d.UNITPRICE
                                       }).ToList();
                response.RecordsCount = chargeGroupData.Count;
                response.Data = chargeGroupData;
            }
            else if (request.Data.CHARGERECORDTYPE == 3)
            {
                var chargeGroupData = (from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                       join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                       join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                       join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on c.CHARGEITEMID equals d.SERVICEID
                                       join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                       where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == request.Data.CHARGERECORDTYPE && c.CHARGEITEMID == request.Data.FEERECORDID
                                        && a.ISDELETE != true && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS == 0 && b.STATUS == 0
                                       orderby c.CHARGEITEMID descending

                                       select new RsChargeGroup()
                                       {
                                           CGRId = a.CGRID,
                                           ChargeGroupId = b.CHARGEGROUPID,
                                           ChargeGroupName = b.CHARGEGROUPNAME,
                                           FeeNo = a.FEENO,
                                           Status = d.STATUS,
                                           ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                           ChargeItemId = c.CHARGEITEMID,
                                           ChargeItemType = c.CHARGEITEMTYPE,
                                           ChargeTypeId = d.CHARGETYPEID,
                                           ConversionRatio = 1,
                                           FeeItemCount = c.FEEITEMCOUNT,
                                           MCCode = d.MCSERVICECODE,
                                           NSCode = d.NSSERVICECODE,
                                           Name = d.SERVICENAME,
                                           IsNciItem = d.ISNCIITEM,
                                           Spec = "",
                                           Units = d.UNITS,
                                           UnitPrice = d.UNITPRICE
                                       }).ToList();
                response.RecordsCount = chargeGroupData.Count;
                response.Data = chargeGroupData;
            }

            return response;
        }
        #endregion

        public BaseResponse<NCIFinancialMonth> GetNCIFinancialMonth(string FMonth)
        {
            return base.Get<LTC_NCIFINANCIALMONTH, NCIFinancialMonth>((q) => q.MONTH == FMonth && q.GOVID == SecurityHelper.CurrentPrincipal.GovId);
        }
    }
}
