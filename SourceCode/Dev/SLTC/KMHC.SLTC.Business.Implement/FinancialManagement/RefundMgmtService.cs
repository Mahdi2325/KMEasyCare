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
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using KMHC.SLTC.Business.Entity;
using AutoMapper;

namespace KMHC.SLTC.Business.Implement
{
    public class RefundMgmtService : BaseService, IRefundMgmtService
    {
        #region LTC_BILLV2
        public BaseResponse<IList<BillV2>> QueryBillV2(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<BillV2>> response = new BaseResponse<IList<BillV2>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                            join b in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.BILLCREATOR equals b.EMPNO
                            join c in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on a.FEENO equals c.FEENO into reg_nciinfo
                            from reg_nci in reg_nciinfo.DefaultIfEmpty()
                            where a.FEENO == request.Data.FEENO && a.STATUS == request.Data.STATUS && a.ISDELETE == false
                            orderby a.CREATETIME descending
                            select new BillV2()
                            {
                                BillId = a.BILLID,
                                BillPayId = a.BILLPAYID,
                                BillMonth = a.BILLMONTH,
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
                                Status = a.STATUS,
                                BillCreator = a.BILLCREATOR,
                                BilleName = b.EMPNAME,
                                BalanceOperator = a.BALANCEOPERATOR,
                                RefundOperator = a.REFUNDOPERATOR,
                                CreateBy = a.CREATEBY,
                                CreateTime = a.CREATETIME,
                                UpdateBy = a.UPDATEBY,
                                UpdateTime = a.UPDATETIME,
                                IsDelete = a.ISDELETE,
                            }).ToList();
            response.Data = billData;
            return response;
        }


        public BaseResponse<BillV2> SaveBillV2(BillV2 request, string UpdateBy, DateTime UpdateDate)
        {
            var newItem = new BillV2()
            {
                BillId = request.BillId,
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
                Status = Convert.ToInt32(BillStatus.Refund),
                BillCreator = request.BillCreator,
                BalanceOperator = request.BalanceOperator,
                RefundOperator = SecurityHelper.CurrentPrincipal.EmpNo,
                CreateBy = request.CreateBy,
                CreateTime = request.CreateTime,
                UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo,
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
            }
            return base.Save<LTC_BILLV2, BillV2>(newItem, (q) => q.BILLID == newItem.BillId);
        }


        public BaseResponse<List<BillV2>> SaveBillV2Info(BillV2List request)
        {
            BaseResponse<List<BillV2>> response = new BaseResponse<List<BillV2>>();
            if (request.BillV2Lists != null && request.BillV2Lists.Count > 0)
            {
                #region 退款
                foreach (var item in request.BillV2Lists)
                {
                    //费用记录
                    var fc = unitOfWork.GetRepository<LTC_FEERECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ISDELETE == false && m.BILLID == item.BillId).ToList();
                    if (fc != null)
                    {
                        foreach (var itemfc in fc)
                        {
                            LTC_FEERECORD feemodel = new LTC_FEERECORD();
                            itemfc.ISREFUNDRECORD = true;
                            itemfc.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                            itemfc.UPDATETIME = DateTime.Now;
                            unitOfWork.GetRepository<LTC_FEERECORD>().Update(itemfc);

                            //创建一条负数据
                            feemodel.FEERECORDID = String.Format("{0}{1}{2}{3}", SecurityHelper.CurrentPrincipal.OrgId, DateTime.Now.ToString("yyyyMMddHHmmss"), itemfc.CHARGERECORDTYPE, itemfc.CHARGERECORDID);
                            feemodel.BILLID = itemfc.BILLID;
                            feemodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                            feemodel.CREATETIME = DateTime.Now;
                            feemodel.ISDELETE = false;
                            feemodel.FEENO = item.FeeNo;
                            feemodel.CHARGERECORDTYPE = itemfc.CHARGERECORDTYPE;
                            feemodel.CHARGERECORDID = itemfc.CHARGERECORDID;
                            feemodel.UNITPRICE = itemfc.UNITPRICE;
                            feemodel.COUNT = -itemfc.COUNT;
                            feemodel.COST = -itemfc.COST;
                            feemodel.ISNCIITEM = itemfc.ISNCIITEM;
                            feemodel.ISREFUNDRECORD = false;
                            unitOfWork.GetRepository<LTC_FEERECORD>().Insert(feemodel);
                        }
                    }

                    //账单记录
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
                    model.REFUNDOPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CREATEBY = item.CreateBy;
                    model.CREATETIME = item.CreateTime;
                    model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.UPDATETIME = DateTime.Now;
                    model.ISDELETE = item.IsDelete;
                    model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                    unitOfWork.GetRepository<LTC_BILLV2>().Update(model);

                    //使用记录
                    var feeRecs = unitOfWork.GetRepository<LTC_FEERECORD>().dbSet.Where(m => m.BILLID == item.BillId).ToList();
                    if (feeRecs != null)
                    {
                        foreach (var feeRec in feeRecs)
                        {
                            if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Drug))
                            {
                                var drug = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.FirstOrDefault(m => m.DRUGRECORDID == feeRec.CHARGERECORDID);
                                drug.STATUS = Convert.ToInt32(RecordStatus.Refund);
                                drug.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                drug.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(drug);
                            }
                            else if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Material))
                            {
                                var material = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.FirstOrDefault(m => m.MATERIALRECORDID == feeRec.CHARGERECORDID);
                                material.STATUS = Convert.ToInt32(RecordStatus.Refund);
                                material.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                material.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(material);
                            }
                            else if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Service))
                            {
                                var service = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.FirstOrDefault(m => m.SERVICERECORDID == feeRec.CHARGERECORDID);
                                service.STATUS = Convert.ToInt32(RecordStatus.Refund);
                                service.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                service.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(service);
                            }
                        }
                    }

                    //额度
                    var mpbr = unitOfWork.GetRepository<LTC_MONTHLYPAYBILLRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ISDELETE == false && m.BILLID == item.BillId).ToList();
                    if (mpbr != null)
                    {
                        foreach (var itemmpbr in mpbr)
                        {
                            itemmpbr.STATUS = Convert.ToInt32(BillStatus.Refund);
                            itemmpbr.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                            itemmpbr.UPDATETIME = DateTime.Now;
                            unitOfWork.GetRepository<LTC_MONTHLYPAYBILLRECORD>().Update(itemmpbr);

                            var mpl = unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().dbSet.FirstOrDefault(m => m.FEENO == item.FeeNo && m.YEARMONTH == itemmpbr.YEARMONTH && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
                            if (mpl != null)
                            {
                                mpl.PAYEDAMOUNT = mpl.PAYEDAMOUNT - itemmpbr.PAYEDAMOUNT;
                                mpl.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                mpl.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().Update(mpl);
                            }
                        }
                    }

                    //请假记录数据

                    var ncideduModel = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(m => m.BILLID == item.BillId).ToList();
                    Mapper.CreateMap<LTC_NCIDEDUCTION, NCIDeductionModel>();
                    var edductionList = Mapper.Map<List<NCIDeductionModel>>(ncideduModel);
                    if (edductionList != null && edductionList.Count > 0)
                    {
                        foreach (var deduitem in edductionList)
                        {
                            var dedu = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.FirstOrDefault(m => m.ID == deduitem.ID);
                            dedu.BILLID = null;
                            dedu.AMOUNT = 0;
                            dedu.DEDUCTIONREASON = "";
                            dedu.DEDUCTIONSTATUS = (int)DeductionStatus.UnCharge;
                            dedu.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                            dedu.UPDATETIME = DateTime.Now;
                            unitOfWork.GetRepository<LTC_NCIDEDUCTION>().Update(dedu);
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
        #endregion

        #region LTC_PRECHARGE
        public BaseResponse<IList<PreCharge>> QueryPreCharge(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<PreCharge>> response = new BaseResponse<IList<PreCharge>>();
            var prechargedata = (from a in unitOfWork.GetRepository<LTC_PRECHARGE>().dbSet
                                 join b in unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet on a.BALANCEID equals b.BALANCEID
                                 where b.FEENO == request.Data.FEENO && a.ISDELETE == false
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
                                CHARGETYPEID = d.CHARGETYPEID,
                                MCDRUGCODE = d.MCDRUGCODE,
                                INNERCODE = d.NSDRUGCODE,
                                CNNAME = c.CNNAME,
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
                                     join c in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on b.CHARGERECORDID equals c.MATERIALRECORDID
                                     join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.MATERIALID equals d.MATERIALID
                                     where a.BILLID == request.Data.BILLID && b.CHARGERECORDTYPE == 2 && a.ISDELETE == false
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
                                              where a.BILLID == request.Data.BILLID && b.CHARGERECORDTYPE == 3 && a.ISDELETE == false
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

        #endregion
    }
}
