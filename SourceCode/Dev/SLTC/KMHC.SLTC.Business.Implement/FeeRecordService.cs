using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class FeeRecordService : BaseService, IFeeRecordService
    {
        #region 获取使用记录数据

        /// <summary>
        /// 获取使用记录数据
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <returns>数据列表</returns>
        public BaseResponse<IList<FeeRecordBaseInfo>> QueryRecord(BaseRequest<FeeRecordFilter> request)
        {
            var response = new BaseResponse<IList<FeeRecordBaseInfo>>();
            List<FeeRecordBaseInfo> baseInfo = new List<FeeRecordBaseInfo>();
            var feeRecordListOfDrug = new List<ChargeRecord>();
            var feeRecordListOfMaterial = new List<ChargeRecord>();
            var feeRecordListOfService = new List<ChargeRecord>();

            try
            {
                #region 费用记录已有数据
                var q = from fee in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                        where fee.FEENO == request.Data.FeeNo && fee.ISDELETE == false && fee.ISREFUNDRECORD == false
                        join bill in unitOfWork.GetRepository<LTC_BILLV2>().dbSet on fee.BILLID equals bill.BILLID into bs
                        from b in bs.DefaultIfEmpty()
                        select new ChargeRecord
                        {
                            Status = b.STATUS,
                            IsDelete = b.ISDELETE,
                            ChargeRecordType = fee.CHARGERECORDTYPE,
                            ChargeRecordID = fee.CHARGERECORDID,
                        };

                var feeRecordList = q.Where(m => m.IsDelete == false && (m.Status == 0 || m.Status == 2)).ToList();
                #endregion

                #region 药品的相关数据
                if (request.Data.taskStatus.Contains("R0"))
                {
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        feeRecordListOfDrug = feeRecordList.Where(m => m.ChargeRecordType == Convert.ToInt32(FeeRecordEnum.drugrecord)).ToList();
                    }

                    var drugRecordList = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.FEENO == request.Data.FeeNo && m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.ISDELETE == false && m.TAKETIME >= request.Data.sDate && m.TAKETIME <= request.Data.eDate).ToList();
                    if (drugRecordList != null && drugRecordList.Count > 0)
                    {
                        foreach (var drug in drugRecordList)
                        {
                            if (feeRecordListOfDrug != null && feeRecordListOfDrug.Count > 0)
                            {
                                var count = feeRecordListOfDrug.Where(m => m.ChargeRecordID == drug.DRUGRECORDID).Count();
                                if (count <= 0)
                                {
                                    var fr = new FeeRecordBaseInfo();
                                    fr.ChargeRecordType = Convert.ToInt32(FeeRecordEnum.drugrecord);
                                    fr.ChargeRecordID = drug.DRUGRECORDID;
                                    fr.FeeNo = drug.FEENO;
                                    fr.UnitPrice = drug.UNITPRICE;
                                    fr.Count = Convert.ToInt32(drug.QTY);
                                    fr.Cost = drug.COST;
                                    fr.IsNCIItem = drug.ISNCIITEM;
                                    fr.NSID = drug.NSID;
                                    fr.ProjectID = drug.DRUGID;
                                    fr.ProjectName = drug.CNNAME;
                                    fr.TakeTime = drug.TAKETIME;
                                    fr.IsChecked = true;
                                    baseInfo.Add(fr);
                                }
                            }
                            else
                            {
                                var fr = new FeeRecordBaseInfo();
                                fr.ChargeRecordType = Convert.ToInt32(FeeRecordEnum.drugrecord);
                                fr.ChargeRecordID = drug.DRUGRECORDID;
                                fr.FeeNo = drug.FEENO;
                                fr.UnitPrice = drug.UNITPRICE;
                                fr.Count = Convert.ToInt32(drug.QTY);
                                fr.Cost = drug.COST;
                                fr.IsNCIItem = drug.ISNCIITEM;
                                fr.NSID = drug.NSID;
                                fr.TakeTime = drug.TAKETIME;
                                fr.ProjectID = drug.DRUGID;
                                fr.ProjectName = drug.CNNAME;
                                fr.IsChecked = true;
                                baseInfo.Add(fr);
                            }

                        }

                    }
                }
                #endregion

                #region 耗材信息数据
                if (request.Data.taskStatus.Contains("R1"))
                {
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        feeRecordListOfMaterial = feeRecordList.Where(m => m.ChargeRecordType == Convert.ToInt32(FeeRecordEnum.materialrecord)).ToList();
                    }

                    var materialRecordList = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.FEENO == request.Data.FeeNo && m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.ISDELETE == false && m.TAKETIME >= request.Data.sDate && m.TAKETIME <= request.Data.eDate).ToList();
                    if (materialRecordList != null && materialRecordList.Count > 0)
                    {
                        foreach (var material in materialRecordList)
                        {
                            if (feeRecordListOfMaterial != null && feeRecordListOfMaterial.Count > 0)
                            {
                                var count = feeRecordListOfMaterial.Where(m => m.ChargeRecordID == material.MATERIALRECORDID).Count();
                                if (count <= 0)
                                {
                                    var fr = new FeeRecordBaseInfo();
                                    fr.ChargeRecordType = Convert.ToInt32(FeeRecordEnum.materialrecord);
                                    fr.ChargeRecordID = material.MATERIALRECORDID;
                                    fr.FeeNo = material.FEENO;
                                    fr.UnitPrice = material.UNITPRICE;
                                    fr.Count = Convert.ToInt32(material.QTY);
                                    fr.Cost = material.COST;
                                    fr.IsNCIItem = material.ISNCIITEM;
                                    fr.NSID = material.NSID;
                                    fr.TakeTime = material.TAKETIME;
                                    fr.ProjectID = material.MATERIALID;
                                    fr.ProjectName = material.MATERIALNAME;
                                    fr.IsChecked = true;
                                    baseInfo.Add(fr);
                                }
                            }
                            else
                            {
                                var fr = new FeeRecordBaseInfo();
                                fr.ChargeRecordType = Convert.ToInt32(FeeRecordEnum.materialrecord);
                                fr.ChargeRecordID = material.MATERIALRECORDID;
                                fr.FeeNo = material.FEENO;
                                fr.UnitPrice = material.UNITPRICE;
                                fr.Count = Convert.ToInt32(material.QTY);
                                fr.Cost = material.COST;
                                fr.IsNCIItem = material.ISNCIITEM;
                                fr.NSID = material.NSID;
                                fr.TakeTime = material.TAKETIME;
                                fr.ProjectID = material.MATERIALID;
                                fr.ProjectName = material.MATERIALNAME;
                                fr.IsChecked = true;
                                baseInfo.Add(fr);
                            }

                        }

                    }
                }
                #endregion

                #region 服务记录数据

                if (request.Data.taskStatus.Contains("R2"))
                {
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        feeRecordListOfService = feeRecordList.Where(m => m.ChargeRecordType == Convert.ToInt32(FeeRecordEnum.servicerecord)).ToList();
                    }

                    var serviceRecordList = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.FEENO == request.Data.FeeNo && m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.ISDELETE == false && m.TAKETIME >= request.Data.sDate && m.TAKETIME <= request.Data.eDate).ToList();
                    if (serviceRecordList != null && serviceRecordList.Count > 0)
                    {
                        foreach (var service in serviceRecordList)
                        {
                            if (feeRecordListOfService != null && feeRecordListOfService.Count > 0)
                            {
                                var count = feeRecordListOfService.Where(m => m.ChargeRecordID == service.SERVICERECORDID).Count();
                                if (count <= 0)
                                {
                                    var fr = new FeeRecordBaseInfo();
                                    fr.ChargeRecordType = Convert.ToInt32(FeeRecordEnum.servicerecord);
                                    fr.ChargeRecordID = service.SERVICERECORDID;
                                    fr.FeeNo = service.FEENO;
                                    fr.UnitPrice = service.UNITPRICE;
                                    fr.Count = Convert.ToInt32(service.QTY);
                                    fr.Cost = service.COST;
                                    fr.IsNCIItem = service.ISNCIITEM;
                                    fr.NSID = service.NSID;
                                    fr.TakeTime = Convert.ToDateTime(service.TAKETIME);
                                    fr.ProjectID = service.SERVICEID;
                                    fr.ProjectName = service.SERVICENAME;
                                    fr.IsChecked = true;
                                    baseInfo.Add(fr);
                                }
                            }
                            else
                            {
                                var fr = new FeeRecordBaseInfo();
                                fr.ChargeRecordType = Convert.ToInt32(FeeRecordEnum.materialrecord);
                                fr.ChargeRecordID = service.SERVICERECORDID;
                                fr.FeeNo = service.FEENO;
                                fr.UnitPrice = service.UNITPRICE;
                                fr.Count = Convert.ToInt32(service.QTY);
                                fr.Cost = service.COST;
                                fr.IsNCIItem = service.ISNCIITEM;
                                fr.NSID = service.NSID;
                                fr.TakeTime = Convert.ToDateTime(service.TAKETIME); ;
                                fr.ProjectID = service.SERVICEID;
                                fr.ProjectName = service.SERVICENAME;
                                fr.IsChecked = true;
                                baseInfo.Add(fr);
                            }
                        }
                    }
                }
                #endregion

                if (baseInfo != null && baseInfo.Count > 0)
                {
                    response.Data = baseInfo;
                }
                else
                {
                    response.ResultCode = 1001;  //未查询到生成账单的记录数据
                    response.ResultMessage = "已生成最新的账单数据！";
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = ex.Message;
            }
            return response;
        }
        #endregion

        #region 保存费用录入信息
        /// <summary>
        /// 保存费用录入信息
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <returns>返回信息</returns>
        public BaseResponse<List<FeeRecordBaseInfo>> SaveFeeRecord(BillV2Info request)
        {
            BaseResponse<List<FeeRecordBaseInfo>> response = new BaseResponse<List<FeeRecordBaseInfo>>();
            if (request.feeRecordList != null && request.feeRecordList.Count > 0)
            {
                #region 住民账户信息
                Mapper.CreateMap<LTC_RESIDENTBALANCE, ResidentBalance>();
                var bal = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.FirstOrDefault(m => m.FEENO == request.Feeno && m.STATUS == 0);
                var balance = Mapper.Map<ResidentBalance>(bal);

                bool isHaveNCI = balance.ISHAVENCI == 0 ? true : false;
                #endregion

                #region 账单初始化数据
                BillV2 billv2 = new BillV2();
                billv2.BILLID = String.Format("{0}{1}", SecurityHelper.CurrentPrincipal.OrgId, DateTime.Now.ToString("yyyyMMddHHmmss"));  //账单ID
                billv2.BILLPAYID = null;
                billv2.REFUNDRECORDID = null;
                billv2.FEENO = Convert.ToInt32(request.Feeno);
                billv2.NCIITEMTOTALCOST = 0;
                billv2.SELFPAY = 0;
                billv2.NCIPAYLEVEL = balance.NCIPAYLEVEL;
                billv2.NCIPAYSCALE = balance.NCIPAYSCALE;
                billv2.NCIPAY = 0;
                billv2.NCIITEMSELFPAY = 0;
                billv2.BALANCESTARTTIME = request.STime;
                billv2.BALANCEENDTIME = request.ETime;
                billv2.HOSPDAY = 0;
                billv2.BILLCREATOR = SecurityHelper.CurrentPrincipal.UserId.ToString();
                billv2.BALANCEOPERATOR = string.Empty;
                billv2.REFUNDOPERATOR = string.Empty;
                billv2.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                billv2.CREATETIME = DateTime.Now;
                billv2.UPDATEBY = string.Empty;
                billv2.UPDATETIME = null;
                billv2.ISDELETE = false;
                #endregion

                var billv2Info = GetYearMonthFees(request, billv2, isHaveNCI);

                Mapper.CreateMap<BillV2, LTC_BILLV2>();
                var billModel = Mapper.Map<LTC_BILLV2>(billv2Info);
                unitOfWork.GetRepository<LTC_BILLV2>().Insert(billModel);
                unitOfWork.Save();

                #region 费用记录
                foreach (var item in request.feeRecordList)
                {
                    LTC_FEERECORD model = new LTC_FEERECORD();
                    model.FEERECORDID = String.Format("{0}{1}{2}{3}", SecurityHelper.CurrentPrincipal.OrgId, DateTime.Now.ToString("yyyyMMddHHmmss"), item.ChargeRecordType, item.ChargeRecordID);
                    model.BILLID = billv2.BILLID;
                    model.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    model.CREATETIME = DateTime.Now;
                    model.ISDELETE = false;
                    model.FEENO = item.FeeNo;
                    model.CHARGERECORDTYPE = item.ChargeRecordType;
                    model.CHARGERECORDID = item.ChargeRecordID;
                    model.UNITPRICE = item.UnitPrice;
                    model.COUNT = item.Count;
                    model.COST = Convert.ToDecimal(item.Cost);
                    model.ISNCIITEM = item.IsNCIItem;
                    model.ISREFUNDRECORD = false;
                    unitOfWork.GetRepository<LTC_FEERECORD>().Insert(model);
                }
                unitOfWork.Save();
                response.ResultCode = 1001;
                response.ResultMessage = "生成账单成功，请到费用查询中进行查看！";
                #endregion
            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "未查询到有效的使用记录数据！";
            }
            return response;
        }


        public BillV2 GetYearMonthFees(BillV2Info request, BillV2 billv2, bool isHaveNCI)
        {
            var keyDateList = GetDiffDateRange(request.STime, request.ETime.AddDays(1).AddSeconds(-1));

            //身份证号码
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    where ipd.FEENO == request.Feeno && ipd.ORGID == SecurityHelper.CurrentPrincipal.OrgId
                    join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO into bs
                    from b in bs.DefaultIfEmpty()
                    select new Person
                    {
                        IdNo = b.IDNO,
                    };

            var idNo = q.FirstOrDefault().IdNo;

            #region 年月分段
            foreach (var item in keyDateList)
            {
                var yearMonth = string.Format("{0}{1}", item.startDate.Year, item.startDate.Month);
                // 查询额度信息

                Mapper.CreateMap<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>();
                var lim = unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().dbSet.Where(m => m.FEENO == request.Feeno && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId && m.YEARMONTH == yearMonth).ToList();
                var limitList = Mapper.Map<List<MonthlyPayLimit>>(lim);

                if (limitList != null && limitList.Count > 0)
                {
                    #region 有额度数据
                    HasQuotaInfo(request, item, isHaveNCI, billv2, idNo);
                    #endregion
                }
                else
                {
                    #region 无额度数据
                    NotHasQuotaInfo(request, item, isHaveNCI, billv2, idNo);
                    #endregion
                }
            }
            return billv2;
            #endregion
        }

        public void HasQuotaInfo(BillV2Info billV2, KeyValueForDate item, bool isHaveNCI, BillV2 billInfo, string idNo)
        {
            var feeRecordYearMonth = billV2.feeRecordList.Where(m => m.TakeTime >= item.startDate && m.TakeTime <= item.endtDate.AddDays(1).AddSeconds(-1)).ToList();
            if (feeRecordYearMonth != null && feeRecordYearMonth.Count > 0)
            {
                #region 已有额度信息
                var yearMonth = string.Format("{0}{1}", item.startDate.Year, item.startDate.Month);
                Mapper.CreateMap<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>();
                var limit = unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().dbSet.FirstOrDefault(m => m.FEENO == billInfo.FEENO && m.YEARMONTH == yearMonth);
                var monthlyRecord = Mapper.Map<MonthlyPayLimit>(limit);
                #endregion
                #region 额度历史记录
                Mapper.CreateMap<LTC_MONTHLYPAYBILLRECORD, MonthlyPayBillRecord>();
                var record = unitOfWork.GetRepository<LTC_MONTHLYPAYBILLRECORD>().dbSet.Where(m => m.FEENO == billInfo.FEENO && m.YEARMONTH == yearMonth);
                var payLimitList = Mapper.Map<List<MonthlyPayBillRecord>>(record.ToList());
                #endregion
                #region 最新一笔费用信息

                decimal payedAmount = 0;  //累计额度

                var selfPay = feeRecordYearMonth.Where(m => m.IsNCIItem == false).Sum(m => m.Cost);  //自费项目总费用
                var nciItemTotalCost = feeRecordYearMonth.Where(m => m.IsNCIItem == true).Sum(m => m.Cost); // 护理险项目总费用
                
                var nciPayLevel = billInfo.NCIPAYLEVEL;
                var nciPaysCale = billInfo.NCIPAYSCALE;
                var hospDay = GetDiffDateDays(item.startDate, item.endtDate);
                int maxPayDays = GetRangeHospDay( Convert.ToDateTime(payLimitList.Min(m => m.COMPSTARTDATE)),Convert.ToDateTime(payLimitList.Max(m => m.COMPENDDATE)), item.startDate, item.endtDate);
                #endregion
                if (isHaveNCI)
                {
                    billInfo.SELFPAY += Convert.ToDecimal(selfPay);
                    billInfo.NCIITEMTOTALCOST += Convert.ToDecimal(nciItemTotalCost);
                    billInfo.HOSPDAY += hospDay;
                    var maxNCIPay = Convert.ToDecimal(maxPayDays * Convert.ToDecimal(nciPayLevel) * Convert.ToDecimal(nciPaysCale)); //最大可报销费用
                    var nciPayedCost = maxNCIPay - monthlyRecord.PAYEDAMOUNT; //  剩余的报销费用
                    if (Convert.ToDecimal(nciItemTotalCost) <= nciPayedCost)
                    {
                        billInfo.NCIPAY += Convert.ToDecimal(nciItemTotalCost);
                        payedAmount = Convert.ToDecimal(nciItemTotalCost);
                        billInfo.NCIITEMSELFPAY += Convert.ToDecimal(selfPay);
                    }
                    else
                    {
                        billInfo.NCIPAY += Convert.ToDecimal(nciPayedCost);
                        payedAmount = Convert.ToDecimal(nciPayedCost);
                        billInfo.NCIITEMSELFPAY += Convert.ToDecimal(selfPay + Convert.ToDecimal(nciItemTotalCost) - nciPayedCost);
                    }

                    #region 修改额度信息表
                    Mapper.CreateMap<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>();
                    Mapper.CreateMap<MonthlyPayLimit, LTC_MONTHLYPAYLIMIT>();
                    var recordModel = Mapper.Map<LTC_MONTHLYPAYLIMIT>(monthlyRecord);
                    recordModel.PAYEDAMOUNT += payedAmount;
                    unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().Update(recordModel);
                    unitOfWork.Save();
                    #endregion

                    #region 保存额度历史记录表
                    MonthlyPayBillRecord monthBillRecord = new MonthlyPayBillRecord();
                    monthBillRecord.BILLID = billInfo.BILLID;
                    monthBillRecord.YEARMONTH = string.Format("{0}{1}", item.startDate.Year, item.startDate.Month);
                    monthBillRecord.FEENO = billInfo.FEENO;
                    monthBillRecord.PAYEDAMOUNT = payedAmount;
                    monthBillRecord.COMPSTARTDATE = item.startDate;
                    monthBillRecord.COMPENDDATE = item.endtDate;
                    monthBillRecord.STATUS = 0;
                    monthBillRecord.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    monthBillRecord.CREATETIME = DateTime.Now;
                    monthBillRecord.ISDELETE = false;
                    SaveMonthlyPayBillRecord(monthBillRecord);
                    #endregion

                    #region 更新住民账户信息
                    var banmodel = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.Where(x => x.FEENO == billInfo.FEENO).FirstOrDefault();
                    if (banmodel != null)
                    {
                        banmodel.TOTALCOST = banmodel.TOTALCOST == null ? 0 : banmodel.TOTALCOST + selfPay + nciItemTotalCost;
                        banmodel.TOTALNCIPAY = banmodel.TOTALNCIPAY == null ? 0 : banmodel.TOTALNCIPAY + payedAmount;
                        unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().Update(banmodel);
                        unitOfWork.Save();
                    }
                    #endregion
                }
                else
                {
                    #region 无护理险资格
                    billInfo.SELFPAY += Convert.ToDecimal(selfPay);
                    billInfo.NCIITEMTOTALCOST += Convert.ToDecimal(nciItemTotalCost);
                    billInfo.NCIPAY += 0;
                    billInfo.HOSPDAY += hospDay;
                    billInfo.NCIITEMSELFPAY += Convert.ToDecimal(selfPay + nciItemTotalCost);
                    var banmodel = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.Where(x => x.FEENO == billInfo.FEENO).FirstOrDefault();
                    if (banmodel != null)
                    {
                        banmodel.TOTALCOST = banmodel.TOTALCOST == null ? 0 : banmodel.TOTALCOST + selfPay + nciItemTotalCost;
                        unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().Update(banmodel);
                        unitOfWork.Save();
                    }

                    #endregion
                }
              

            }
        }

        /// <summary>
        /// 无额度信息生成账单
        /// </summary>
        /// <param name="billV2">使用记录信息</param>
        /// <param name="item">时间集合</param>
        /// <param name="isHaveNCI">是否拥有护理险资格</param>
        /// <param name="billInfo">账单数据</param>
        public void NotHasQuotaInfo(BillV2Info billV2, KeyValueForDate item, bool isHaveNCI, BillV2 billInfo, string idNo)
        {
            var feeRecordYearMonth = billV2.feeRecordList.Where(m => m.TakeTime >= item.startDate && m.TakeTime <= item.endtDate.AddDays(1).AddSeconds(-1)).ToList();
            if (feeRecordYearMonth != null && feeRecordYearMonth.Count > 0)
            {
                decimal payedAmount = 0;  //累计额度

                var selfPay = feeRecordYearMonth.Where(m => m.IsNCIItem == false).Sum(m => m.Cost);  //自费项目总费用
                var nciItemTotalCost = feeRecordYearMonth.Where(m => m.IsNCIItem == true).Sum(m => m.Cost); // 护理险项目总费用

                int hospDay = GetDiffDateDays(item.startDate, item.endtDate);
                var nciPayLevel = billInfo.NCIPAYLEVEL;
                var nciPaysCale = billInfo.NCIPAYSCALE;

                // 是否拥有护理资格
                if (isHaveNCI)
                {
                    #region 有护理险资格
                    #region 账单数据更新
                    billInfo.SELFPAY += Convert.ToDecimal(selfPay);
                    billInfo.NCIITEMTOTALCOST += Convert.ToDecimal(nciItemTotalCost);
                    billInfo.HOSPDAY += hospDay;

                    var nciPayCost = Convert.ToDecimal(hospDay * Convert.ToDecimal(nciPayLevel) * Convert.ToDecimal(nciPaysCale)); // 护理险最多可报销的费用

                    if (Convert.ToDecimal(nciItemTotalCost) <= nciPayCost)
                    {
                        billInfo.NCIPAY += Convert.ToDecimal(nciItemTotalCost);
                        payedAmount = Convert.ToDecimal(nciItemTotalCost);
                        billInfo.NCIITEMSELFPAY += Convert.ToDecimal(selfPay);
                    }
                    else
                    {
                        billInfo.NCIPAY += Convert.ToDecimal(nciPayCost);
                        payedAmount = Convert.ToDecimal(nciPayCost);
                        billInfo.NCIITEMSELFPAY += Convert.ToDecimal(selfPay + Convert.ToDecimal(nciItemTotalCost) - nciPayCost);
                    }
                    #endregion

                    #region 保存额度信息表 和 额度历史记录表  和 住民账户表
                    #region 保存额度信息表
                    MonthlyPayLimit monthLimit = new MonthlyPayLimit();
                    monthLimit.YEARMONTH = string.Format("{0}{1}", item.startDate.Year, item.startDate.Month);
                    monthLimit.FEENO = billInfo.FEENO;
                    monthLimit.RESIDENTSSID = idNo;
                    monthLimit.PAYEDAMOUNT = payedAmount;
                    monthLimit.NCIPAYLEVEL = nciPayLevel;
                    monthLimit.NCIPAYSCALE = nciPaysCale;
                    monthLimit.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                    monthLimit.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    monthLimit.CREATETIME = DateTime.Now;
                    int payLimitId = SaveMonthlyPayLimit(monthLimit);
                    #endregion
                    #region 保存额度历史记录表
                    MonthlyPayBillRecord monthBillRecord = new MonthlyPayBillRecord();
                    monthBillRecord.BILLID = billInfo.BILLID;
                    monthBillRecord.YEARMONTH = string.Format("{0}{1}", item.startDate.Year, item.startDate.Month);
                    monthBillRecord.FEENO = billInfo.FEENO;
                    monthBillRecord.PAYEDAMOUNT = payedAmount;
                    monthBillRecord.COMPSTARTDATE = item.startDate;
                    monthBillRecord.COMPENDDATE = item.endtDate;
                    monthBillRecord.STATUS = 0;
                    monthBillRecord.CREATEBY = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    monthBillRecord.CREATETIME = DateTime.Now;
                    monthBillRecord.ISDELETE = false;
                    SaveMonthlyPayBillRecord(monthBillRecord);
                    #endregion
                    #region 更新住民账户信息
                    var banmodel = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.Where(x => x.FEENO == billInfo.FEENO).FirstOrDefault();
                    if (banmodel != null)
                    {
                        banmodel.TOTALCOST = banmodel.TOTALCOST == null ? 0 : banmodel.TOTALCOST + selfPay + nciItemTotalCost;
                        banmodel.TOTALNCIPAY = banmodel.TOTALNCIPAY == null ? 0 : banmodel.TOTALNCIPAY + payedAmount;
                        unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().Update(banmodel);
                        unitOfWork.Save();
                    }
                    #endregion
                    #endregion
                    #endregion
                }
                else
                {
                    #region 无护理险资格
                    billInfo.SELFPAY += Convert.ToDecimal(selfPay);
                    billInfo.NCIITEMTOTALCOST += Convert.ToDecimal(nciItemTotalCost);
                    billInfo.NCIPAY += 0;
                    billInfo.HOSPDAY += hospDay;
                    billInfo.NCIITEMSELFPAY += Convert.ToDecimal(selfPay + nciItemTotalCost);
                    var banmodel = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.Where(x => x.FEENO == billInfo.FEENO).FirstOrDefault();
                    if (banmodel != null)
                    {
                        banmodel.TOTALCOST = banmodel.TOTALCOST == null ? 0 : banmodel.TOTALCOST + selfPay + nciItemTotalCost;
                        unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().Update(banmodel);
                        unitOfWork.Save();
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 保存额度信息表
        /// </summary>
        /// <param name="monthLimit">年月额度信息数据</param>
        public int SaveMonthlyPayLimit(MonthlyPayLimit monthLimit)
        {
            Mapper.CreateMap<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>();
            Mapper.CreateMap<MonthlyPayLimit, LTC_MONTHLYPAYLIMIT>();
            var model = Mapper.Map<LTC_MONTHLYPAYLIMIT>(monthLimit);
            unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().Insert(model);
            return model.PAYLIMITID;
            unitOfWork.Save();
        }

        /// <summary>
        /// 保存额度历史记录表数据
        /// </summary>
        /// <param name="monthBillRecord">额度历史数据</param>
        public void SaveMonthlyPayBillRecord(MonthlyPayBillRecord monthBillRecord)
        {
            Mapper.CreateMap<LTC_MONTHLYPAYBILLRECORD, MonthlyPayBillRecord>();
            Mapper.CreateMap<MonthlyPayBillRecord, LTC_MONTHLYPAYBILLRECORD>();
            var model = Mapper.Map<LTC_MONTHLYPAYBILLRECORD>(monthBillRecord);
            unitOfWork.GetRepository<LTC_MONTHLYPAYBILLRECORD>().Insert(model);
            unitOfWork.Save();
        }

        /// <summary>
        /// 获取两个时间的时间差
        /// </summary>
        /// <param name="dt1">时间1</param>
        /// <param name="dt2">时间2</param>
        /// <returns>相差天数</returns>
        public int GetDiffDateDays(DateTime dt1, DateTime dt2)
        {
            var days = 0;
            TimeSpan ts = dt2 - dt1;
            days = ts.Days + 1;
            return days;
        }

        public int GetRangeHospDay(DateTime minDate, DateTime maxDate, DateTime startDate, DateTime endDate)
        {
            DateTime beginDate;
            DateTime finishDate;

            if (minDate > startDate)
            {
                beginDate = startDate;
            }
            else
            {
                beginDate = minDate;
            }

            if (maxDate > endDate)
            {
                finishDate = maxDate;
            }
            else
            {
                finishDate = endDate;
            }
            return GetDiffDateDays(beginDate, endDate);
        }

        /// <summary>
        ///  获取时间范围
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>时间范围集合</returns>
        public static List<KeyValueForDate> GetDiffDateRange(DateTime startTime, DateTime endTime)
        {
            List<KeyValueForDate> keyDateList = new List<KeyValueForDate>();
            DateTime dt1 = new DateTime(startTime.Year, startTime.Month, startTime.Day);
            DateTime dt2 = new DateTime(endTime.Year, endTime.Month, endTime.Day);
            while (dt1 <= dt2)
            {
                KeyValueForDate keyDate = new KeyValueForDate();
                DateTime dd = new DateTime(dt1.Year, dt1.Month, 1);
                int lastDay = dd.AddMonths(1).AddDays(-1).Day;
                DateTime monthEnd = new DateTime(dt1.Year, dt1.Month, lastDay);

                if (dt1.Year == dt2.Year && dt1.Month == dt2.Month)
                {
                    keyDate.startDate = dt1;
                    keyDate.endtDate = dt2;
                    keyDateList.Add(keyDate);
                    return keyDateList;
                }
                else
                {
                    keyDate.startDate = dt1;
                    keyDate.endtDate = monthEnd;
                    keyDateList.Add(keyDate);
                }
                dt1 = dt1.AddMonths(1);
                dt1 = new DateTime(dt1.Year, dt1.Month, 1);
            }
            return keyDateList;
        }

        public class KeyValueForDate
        {
            public DateTime startDate { get; set; }

            public DateTime endtDate { get; set; }
        }

        #endregion
    }
}
