using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
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
        private IDictManageService _service = IOCContainer.Instance.Resolve<IDictManageService>();
        #region 获取使用记录数据
        /// <summary>
        /// 获取使用记录数据
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <returns>数据列表</returns>
        public BaseResponse<IList<FeeRecordBaseInfo>> QueryNotGenerateBillRecord(BaseRequest<FeeRecordFilter> request)
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
                if (request.Data.TaskStatus.Contains("R0"))
                {
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        feeRecordListOfDrug = feeRecordList.Where(m => m.ChargeRecordType == Convert.ToInt32(ChargeItemType.Drug)).ToList();
                    }

                    var drugRecordList = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.FEENO == request.Data.FeeNo && m.TAKETIME >= request.Data.SDate && m.TAKETIME <= request.Data.EDate && m.ISDELETE == false && (m.STATUS == 0 || m.STATUS == 8)).ToList();
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
                                    fr.ChargeRecordType = Convert.ToInt32(ChargeItemType.Drug);
                                    fr.ChargeRecordID = drug.DRUGRECORDID;  //PID
                                    fr.FeeNo = drug.FEENO;
                                    fr.UnitPrice = drug.UNITPRICE;
                                    fr.Count = drug.QTY;  //  TotalQTY
                                    fr.Cost = drug.COST;
                                    fr.IsNCIItem = drug.ISNCIITEM;
                                    fr.NSID = drug.NSID;
                                    fr.ProjectID = drug.DRUGID;
                                    fr.ProjectName = drug.CNNAME;
                                    fr.TakeTime = drug.TAKETIME;
                                    fr.IsChecked = true;
                                    fr.ProjectID = drug.DRUGID;
                                    fr.ProjectName = drug.CNNAME;
                                    fr.Dosage = drug.DOSAGE;
                                    fr.Units = drug.UNITS;
                                    fr.Takeway = drug.TAKEWAY;
                                    fr.Form = drug.FORM;
                                    fr.Ferq = drug.FERQ;
                                    fr.Operator = drug.OPERATOR;
                                    fr.Comment = drug.COMMENT;
                                    fr.IsNCIItem = drug.ISNCIITEM;
                                    fr.IsChargeGroupItem = drug.ISCHARGEGROUPITEM;
                                    fr.Createby = drug.CREATEBY;
                                    fr.CreateTime = drug.CREATETIME;
                                    fr.IsDelete = drug.ISDELETE;
                                    baseInfo.Add(fr);
                                }
                            }
                            else
                            {
                                var fr = new FeeRecordBaseInfo();
                                fr.ChargeRecordType = Convert.ToInt32(ChargeItemType.Drug);
                                fr.ChargeRecordID = drug.DRUGRECORDID;
                                fr.FeeNo = drug.FEENO;
                                fr.UnitPrice = drug.UNITPRICE;
                                fr.Count = drug.QTY;
                                fr.Cost = drug.COST;
                                fr.IsNCIItem = drug.ISNCIITEM;
                                fr.NSID = drug.NSID;
                                fr.TakeTime = drug.TAKETIME;
                                fr.ProjectID = drug.DRUGID;
                                fr.ProjectName = drug.CNNAME;
                                fr.IsChecked = true;
                                fr.ProjectID = drug.DRUGID;
                                fr.ProjectName = drug.CNNAME;
                                fr.Dosage = drug.DOSAGE;
                                fr.Units = drug.UNITS;
                                fr.Takeway = drug.TAKEWAY;
                                fr.Form = drug.FORM;
                                fr.Ferq = drug.FERQ;
                                fr.Operator = drug.OPERATOR;
                                fr.Comment = drug.COMMENT;
                                fr.IsNCIItem = drug.ISNCIITEM;
                                fr.IsChargeGroupItem = drug.ISCHARGEGROUPITEM;
                                fr.Createby = drug.CREATEBY;
                                fr.CreateTime = drug.CREATETIME;
                                fr.IsDelete = drug.ISDELETE;
                                baseInfo.Add(fr);
                            }

                        }

                    }
                }
                #endregion

                #region 耗材信息数据
                if (request.Data.TaskStatus.Contains("R1"))
                {
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        feeRecordListOfMaterial = feeRecordList.Where(m => m.ChargeRecordType == Convert.ToInt32(ChargeItemType.Material)).ToList();
                    }

                    var materialRecordList = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.FEENO == request.Data.FeeNo && m.ISDELETE == false && m.TAKETIME >= request.Data.SDate && m.TAKETIME <= request.Data.EDate && m.ISDELETE == false && (m.STATUS == 0 || m.STATUS == 8)).ToList();
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
                                    fr.ChargeRecordType = Convert.ToInt32(ChargeItemType.Material);
                                    fr.ChargeRecordID = material.MATERIALRECORDID;
                                    fr.FeeNo = material.FEENO;
                                    fr.UnitPrice = material.UNITPRICE;
                                    fr.Count = material.QTY;
                                    fr.Cost = material.COST;
                                    fr.IsNCIItem = material.ISNCIITEM;
                                    fr.NSID = material.NSID;
                                    fr.TakeTime = material.TAKETIME;
                                    fr.ProjectID = material.MATERIALID;
                                    fr.ProjectName = material.MATERIALNAME;
                                    fr.IsChecked = true;
                                    fr.ProjectID = material.MATERIALID;
                                    fr.ProjectName = material.MATERIALNAME;
                                    fr.Dosage = material.QTY;
                                    fr.Units = material.UNITS;
                                    fr.Takeway = material.TAKEWAY;
                                    fr.Form = "";
                                    fr.Ferq = "";
                                    fr.Operator = material.OPERATOR;
                                    fr.Comment = material.COMMENT;
                                    fr.IsNCIItem = material.ISNCIITEM;
                                    fr.IsChargeGroupItem = material.ISCHARGEGROUPITEM;
                                    fr.Createby = material.CREATEBY;
                                    fr.CreateTime = material.CREATETIME;
                                    fr.IsDelete = material.ISDELETE;
                                    baseInfo.Add(fr);
                                }
                            }
                            else
                            {
                                var fr = new FeeRecordBaseInfo();
                                fr.ChargeRecordType = Convert.ToInt32(ChargeItemType.Material);
                                fr.ChargeRecordID = material.MATERIALRECORDID;
                                fr.FeeNo = material.FEENO;
                                fr.UnitPrice = material.UNITPRICE;
                                fr.Count = material.QTY;
                                fr.Cost = material.COST;
                                fr.IsNCIItem = material.ISNCIITEM;
                                fr.NSID = material.NSID;
                                fr.TakeTime = material.TAKETIME;
                                fr.ProjectID = material.MATERIALID;
                                fr.ProjectName = material.MATERIALNAME;
                                fr.IsChecked = true;
                                fr.ProjectID = material.MATERIALID;
                                fr.ProjectName = material.MATERIALNAME;
                                fr.Dosage = material.QTY;
                                fr.Units = material.UNITS;
                                fr.Takeway = material.TAKEWAY;
                                fr.Form = "";
                                fr.Ferq = "";
                                fr.Operator = material.OPERATOR;
                                fr.Comment = material.COMMENT;
                                fr.IsNCIItem = material.ISNCIITEM;
                                fr.IsChargeGroupItem = material.ISCHARGEGROUPITEM;
                                fr.Createby = material.CREATEBY;
                                fr.CreateTime = material.CREATETIME;
                                fr.IsDelete = material.ISDELETE;
                                baseInfo.Add(fr);
                            }

                        }

                    }
                }
                #endregion

                #region 服务记录数据

                if (request.Data.TaskStatus.Contains("R2"))
                {
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        feeRecordListOfService = feeRecordList.Where(m => m.ChargeRecordType == Convert.ToInt32(ChargeItemType.Service)).ToList();
                    }

                    var serviceRecordList = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.FEENO == request.Data.FeeNo && m.ISDELETE == false && m.TAKETIME >= request.Data.SDate && m.TAKETIME <= request.Data.EDate && m.ISDELETE == false && (m.STATUS == 0 || m.STATUS == 8)).ToList();
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
                                    fr.ChargeRecordType = Convert.ToInt32(ChargeItemType.Service);
                                    fr.ChargeRecordID = service.SERVICERECORDID;
                                    fr.FeeNo = service.FEENO;
                                    fr.UnitPrice = service.UNITPRICE;
                                    fr.Count = service.QTY;
                                    fr.Cost = service.COST;
                                    fr.IsNCIItem = service.ISNCIITEM;
                                    fr.NSID = service.NSID;
                                    fr.TakeTime = Convert.ToDateTime(service.TAKETIME);
                                    fr.ProjectID = service.SERVICEID;
                                    fr.ProjectName = service.SERVICENAME;
                                    fr.IsChecked = true;
                                    fr.ProjectID = service.SERVICEID;
                                    fr.ProjectName = service.SERVICENAME;
                                    fr.Dosage = service.QTY;
                                    fr.Units = service.UNITS;
                                    fr.Takeway = service.TAKEWAY;
                                    fr.Form = "";
                                    fr.Ferq = "";
                                    fr.Operator = service.OPERATOR;
                                    fr.Comment = service.COMMENT;
                                    fr.IsNCIItem = service.ISNCIITEM;
                                    fr.IsChargeGroupItem = service.ISCHARGEGROUPITEM;
                                    fr.Createby = service.CREATEBY;
                                    fr.CreateTime = service.CREATETIME;
                                    fr.IsDelete = service.ISDELETE;
                                    baseInfo.Add(fr);
                                }
                            }
                            else
                            {
                                var fr = new FeeRecordBaseInfo();
                                fr.ChargeRecordType = Convert.ToInt32(ChargeItemType.Service);
                                fr.ChargeRecordID = service.SERVICERECORDID;
                                fr.FeeNo = service.FEENO;
                                fr.UnitPrice = service.UNITPRICE;
                                fr.Count = service.QTY;
                                fr.Cost = service.COST;
                                fr.IsNCIItem = service.ISNCIITEM;
                                fr.NSID = service.NSID;
                                fr.TakeTime = Convert.ToDateTime(service.TAKETIME); ;
                                fr.ProjectID = service.SERVICEID;
                                fr.ProjectName = service.SERVICENAME;
                                fr.IsChecked = true;
                                fr.ProjectID = service.SERVICEID;
                                fr.ProjectName = service.SERVICENAME;
                                fr.Dosage = service.QTY;
                                fr.Units = service.UNITS;
                                fr.Takeway = service.TAKEWAY;
                                fr.Form = "";
                                fr.Ferq = "";
                                fr.Operator = service.OPERATOR;
                                fr.Comment = service.COMMENT;
                                fr.IsNCIItem = service.ISNCIITEM;
                                fr.IsChargeGroupItem = service.ISCHARGEGROUPITEM;
                                fr.Createby = service.CREATEBY;
                                fr.CreateTime = service.CREATETIME;
                                fr.IsDelete = service.ISDELETE;
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
        #region 生成账单新逻辑
        public BaseResponse<List<FeeRecordBaseInfo>> SaveFeeRecord(BillV2Info request)
        {
            var response = new BaseResponse<List<FeeRecordBaseInfo>>();
            if (request.feeRecordList != null && request.feeRecordList.Count > 0)
            {
                var ipdRegInfo = GetIpdRegInfo(request.Feeno);
                var nciInfo = GetNCIInfo(request.Feeno);

                var resEntryTime = ipdRegInfo.InDate.Value;  //住院日期
                var resDisChargeTime = ipdRegInfo.FinancialCloseTime;       //关账时间
                var odt = new DateTime(1, 1, 1);             //原始日期
                var dt = DateTime.Now;
                var maxTime = request.feeRecordList.Select(o => o.TakeTime).Max();    //账单最大日期
                var minTime = request.feeRecordList.Select(o => o.TakeTime).Min();    //账单结束日期
                var rsp = _service.GetFeeIntervalByDate(new DateTime(maxTime.Year, maxTime.Month, maxTime.Day));

                if (Convert.ToInt16(rsp.Month) > dt.Month)
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "当前选择的月份，超出正常的账单生成时间，请核实费用录入信息!";
                    return response;
                }
    
                decimal payLevel = 0;
                decimal payscale = 0;
                bool isHaveNCI = false;

                #region 护理险信息判断
                if (nciInfo != null)
                {
                    resEntryTime = nciInfo.ApplyHosTime.Value;
                    if (resDisChargeTime.HasValue && resDisChargeTime.Value != odt)
                    {
                        #region 有关账数据
                        if (minTime >= resEntryTime && maxTime <= resDisChargeTime.Value)
                        {
                            payLevel = nciInfo.NCIPaylevel;
                            payscale = nciInfo.NCIPayscale;
                            isHaveNCI = true;
                        }
                        else if (minTime > resDisChargeTime.Value || maxTime < resEntryTime)
                        {
                            payLevel = 0;
                            payscale = 0;
                            isHaveNCI = false;
                        }
                        else if (minTime < resEntryTime)
                        {
                            response.ResultCode = -1;
                            response.ResultMessage = "当前选择的时间内，有护理险资格（入住日期：" + resEntryTime.ToString("yyyy-MM-dd HH:mm:ss") + "）之外的数据，请核实费用录入信息";
                            return response;
                        }
                        else if (maxTime > resDisChargeTime.Value)
                        {
                            response.ResultCode = -1;
                            response.ResultMessage = "当前选择的时间内，有护理险资格（关账日期：" + resDisChargeTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + "）之外的数据，请核实费用录入信息";
                            return response;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 无关账数据
                        if (minTime >= resEntryTime)
                        {
                            payLevel = nciInfo.NCIPaylevel;
                            payscale = nciInfo.NCIPayscale;
                            isHaveNCI = true;
                        }
                        else if (maxTime < resEntryTime)
                        {
                            payLevel = 0;
                            payscale = 0;
                            isHaveNCI = false;
                        }
                        else if (minTime < resEntryTime)
                        {
                            response.ResultCode = -1;
                            response.ResultMessage = "当前选择的时间内，有护理险资格（入住日期：" + resEntryTime.ToString("yyyy-MM-dd HH:mm:ss") + "）之外的数据，请核实费用录入信息";
                            return response;
                        }
                        #endregion
                    }
                }
                #endregion

                #region 结算开始结束日期计算
                if (isHaveNCI)
                {
                    resEntryTime = resEntryTime >= request.STime ? resEntryTime : request.STime;
                    if (!resDisChargeTime.HasValue)
                    {
                        resDisChargeTime = request.ETime > dt && request.ETime.Month == dt.Month ? dt : request.ETime;
                    }
                    else if (resDisChargeTime.HasValue && request.STime < resDisChargeTime.Value && request.ETime < resDisChargeTime.Value)
                    {
                        resDisChargeTime = request.ETime;
                    }
                    else if (resDisChargeTime.HasValue && request.STime <= resDisChargeTime.Value && request.ETime >= resDisChargeTime.Value)
                    {
                        resDisChargeTime = resDisChargeTime.Value;
                    }
                }
                else
                {
                    resEntryTime = request.STime;
                    resDisChargeTime = resDisChargeTime.HasValue ? resDisChargeTime.Value : request.ETime;
                }
                #endregion

                #region 新一笔账单初始化数据
                Mapper.CreateMap<LTC_BILLV2, BillV2>();
                BillV2 billv2 = new BillV2();
                billv2.BillId = String.Format("{0}{1}{2}", "B", SecurityHelper.CurrentPrincipal.OrgId, DateTime.Now.ToString("yyyyMMddHHmmss"));  //账单ID
                billv2.BillPayId = null;
                billv2.ReFundRecordId = null;
                billv2.FeeNo = Convert.ToInt32(request.Feeno);
                billv2.NCIItemTotalCost = 0;
                billv2.SelfPay = 0;
                billv2.BillMonth = request.BillMonth;
                billv2.NCIPayLevel = payLevel;
                billv2.NCIPaysCale = payscale;
                billv2.NCIPay = 0;
                billv2.NCIItemSelfPay = 0;
                billv2.BalanceStartTime = resEntryTime;
                billv2.IsFinancialClose = resDisChargeTime.Value == request.ETime ? false : true;
                billv2.BalanceEndTime = resDisChargeTime.Value == request.ETime ? resDisChargeTime.Value.AddDays(1).AddSeconds(-1) : Convert.ToDateTime(resDisChargeTime.Value.AddDays(-1).ToString("yyyy-MM-dd") + "  23:59:59");
                billv2.HospDay = 0;
                billv2.BillCreator = SecurityHelper.CurrentPrincipal.EmpNo;
                billv2.BalanceOperator = string.Empty;
                billv2.RefundOperator = string.Empty;
                billv2.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                billv2.CreateTime = DateTime.Now;
                billv2.UpdateBy = string.Empty;
                billv2.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                billv2.UpdateTime = null;
                billv2.IsDelete = false;
                #endregion
                try
                {
                    CalculateAmount(request, billv2, isHaveNCI);
                    Mapper.CreateMap<BillV2, LTC_BILLV2>();
                    var model = Mapper.Map<LTC_BILLV2>(billv2);
                    unitOfWork.GetRepository<LTC_BILLV2>().Insert(model);
                    SaveDeduction(request);
                    SaveFeeRecordInfo(request,billv2);
                    unitOfWork.Save();
                    response.ResultCode = 1001;
                    response.ResultMessage = "生成账单成功，请到费用查询中进行查看！";
                }
                catch (Exception ex)
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "生成账单异常，请联系管理员，错误信息：" + ex.Message;
                }
            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "无生成账单的费用记录信息！";
            }
            return response;
        }

        #region 保存扣款记录数据
        public void SaveDeduction(BillV2Info request)
        {
            if (request.NCIDudeList != null && request.NCIDudeList.Count > 0)
            {
                foreach (var item in request.NCIDudeList)
                {
                    base.Save<LTC_NCIDEDUCTION, NCIDeductionModel>(item, (q) => q.ID == item.ID);
                }
            }
        }
        #endregion

        #region 费用记录
        public void SaveFeeRecordInfo(BillV2Info request,BillV2 billv2)
        {
            request.feeRecordList  = request.feeRecordList.Where(m => m.TakeTime >= billv2.BalanceStartTime.Value && m.TakeTime <= billv2.BalanceEndTime).ToList();

            if (request.feeRecordList != null && request.feeRecordList.Count > 0)
            {
                foreach (var item in request.feeRecordList)
                {
                    if (item.ChargeRecordType == Convert.ToInt32(ChargeItemType.Drug))
                    {
                        var drug = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.FirstOrDefault(m => m.DRUGRECORDID == item.ChargeRecordID && m.ISDELETE != true);
                        drug.STATUS = (int)RecordStatus.GenerateBill;
                        drug.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                        drug.UPDATETIME = DateTime.Now;
                        unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(drug);
                    }
                    else if (item.ChargeRecordType == Convert.ToInt32(ChargeItemType.Material))
                    {
                        var material = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.FirstOrDefault(m => m.MATERIALRECORDID == item.ChargeRecordID && m.ISDELETE != true);
                        material.STATUS = (int)RecordStatus.GenerateBill;
                        material.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                        material.UPDATETIME = DateTime.Now;
                        unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(material);
                    }
                    else if (item.ChargeRecordType == Convert.ToInt32(ChargeItemType.Service))
                    {
                        var service = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.FirstOrDefault(m => m.SERVICERECORDID == item.ChargeRecordID && m.ISDELETE != true);
                        service.STATUS = (int)RecordStatus.GenerateBill;
                        service.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                        service.UPDATETIME = DateTime.Now;
                        unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(service);
                    }

                    LTC_FEERECORD model = new LTC_FEERECORD();
                    model.FEERECORDID = String.Format("{0}{1}{2}{3}", SecurityHelper.CurrentPrincipal.OrgId, DateTime.Now.ToString("yyyyMMddHHmmss"), item.ChargeRecordType, item.ChargeRecordID);
                    model.BILLID = billv2.BillId;
                    model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
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
            }
        }
        #endregion

        /// <summary>
        /// 计算住院期间各项的金额统计
        /// </summary>
        /// <param name="request">费用记录相关信息</param>
        /// <param name="billv2">账单信息</param>
        /// <param name="isHaveNCI">是否拥有护理保险资格</param>
        public void CalculateAmount(BillV2Info request, BillV2 billv2, bool isHaveNCI)
        {
            // 查询额度信息
            Mapper.CreateMap<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>();
            var lim = unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().dbSet.Where(m => m.FEENO == request.Feeno && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId && m.YEARMONTH == request.BillMonth).ToList();
            var limitList = Mapper.Map<List<MonthlyPayLimit>>(lim);

            if (limitList != null && limitList.Count > 0)
            {
                #region 有额度数据
                HasQuotaInfo(request, isHaveNCI, billv2);
                #endregion
            }
            else
            {
                #region 无额度数据
                NotHasQuotaInfo(request, isHaveNCI, billv2);
                #endregion
            }

        }

        #region 额度数据解析
        public void HasQuotaInfo(BillV2Info billV2, bool isHaveNCI, BillV2 billInfo)
        {
            var feeRecordYearMonth = billV2.feeRecordList.Where(m => m.TakeTime >= billInfo.BalanceStartTime && m.TakeTime <= billInfo.BalanceEndTime).ToList();
            if (feeRecordYearMonth != null && feeRecordYearMonth.Count > 0)
            {
                #region 已有额度信息
                Mapper.CreateMap<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>();
                var limit = unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>().dbSet.FirstOrDefault(m => m.FEENO == billInfo.FeeNo && m.YEARMONTH == billInfo.BillMonth);
                var monthlyRecord = Mapper.Map<MonthlyPayLimit>(limit);
                #endregion
                #region 额度历史记录
                Mapper.CreateMap<LTC_MONTHLYPAYBILLRECORD, MonthlyPayBillRecord>();
                var record = unitOfWork.GetRepository<LTC_MONTHLYPAYBILLRECORD>().dbSet.Where(m => m.FEENO == billInfo.FeeNo && m.YEARMONTH == billInfo.BillMonth && m.STATUS == 0 && m.ISDELETE != true);
                var payLimitList = Mapper.Map<List<MonthlyPayBillRecord>>(record.ToList());
                #endregion
                #region 最新一笔费用信息

                decimal payedAmount = 0;  //累计额度

                var selfPay = feeRecordYearMonth.Where(m => m.IsNCIItem == false).Sum(m => m.Cost);  //自费项目总费用
                var nciItemTotalCost = feeRecordYearMonth.Where(m => m.IsNCIItem == true).Sum(m => m.Cost); // 护理险项目总费用

                var nciPayLevel = billInfo.NCIPayLevel;
                var nciPaysCale = billInfo.NCIPaysCale;
                var hospDay = GetDiffDateDays(billInfo.BalanceStartTime.Value, billInfo.BalanceEndTime);
                int maxPayDays = GetRangeHospDay(billInfo, Convert.ToDateTime(payLimitList.Min(m => m.CompStartDate)), Convert.ToDateTime(payLimitList.Max(m => m.CompEndDate)), billInfo.BalanceStartTime.Value, billInfo.BalanceEndTime);
                #endregion
                if (isHaveNCI)
                {

                    billInfo.SelfPay += Convert.ToDecimal(selfPay);
                    billInfo.NCIItemTotalCost += Convert.ToDecimal(nciItemTotalCost);
                    maxPayDays -= QueryLeaHospDays(billInfo.FeeNo, billInfo.BillMonth, hospDay, nciPayLevel, nciPaysCale, billInfo.BillId, billV2);
                    maxPayDays = maxPayDays <= 0 ? 0 : maxPayDays;
                    billInfo.HospDay += maxPayDays;
                    var maxNCIPay = Convert.ToDecimal(maxPayDays * Convert.ToDecimal(nciPayLevel) * Convert.ToDecimal(nciPaysCale)); //最大可报销费用
                    var nciPayedCost = maxNCIPay - monthlyRecord.PayedAmount; //  剩余的报销费用
                    if (Convert.ToDecimal(nciItemTotalCost) <= nciPayedCost)
                    {
                        billInfo.NCIPay += Convert.ToDecimal(nciItemTotalCost);
                        payedAmount = Convert.ToDecimal(nciItemTotalCost);
                        billInfo.NCIItemSelfPay += 0;
                    }
                    else
                    {
                        billInfo.NCIPay += Convert.ToDecimal(nciPayedCost);
                        payedAmount = Convert.ToDecimal(nciPayedCost);
                        billInfo.NCIItemSelfPay += Convert.ToDecimal(Convert.ToDecimal(nciItemTotalCost) - nciPayedCost);
                    }

                    #region 修改额度信息表
                    try
                    {
                        monthlyRecord.PayedAmount += payedAmount;
                        base.Save<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>(monthlyRecord, (q) => q.PAYLIMITID == monthlyRecord.PayLimitID);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    #endregion

                    #region 保存额度历史记录表
                    MonthlyPayBillRecord monthBillRecord = new MonthlyPayBillRecord();
                    monthBillRecord.BillID = billInfo.BillId;
                    monthBillRecord.YearMonth = billInfo.BillMonth;
                    monthBillRecord.FeeNO = billInfo.FeeNo;
                    monthBillRecord.PayedAmount = payedAmount;
                    monthBillRecord.CompStartDate = billInfo.BalanceStartTime;
                    monthBillRecord.CompEndDate = billInfo.BalanceEndTime;
                    monthBillRecord.Status = 0;
                    monthBillRecord.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                    monthBillRecord.CreateTime = DateTime.Now;
                    monthBillRecord.IsDelete = false;
                    SaveMonthlyPayBillRecord(monthBillRecord);
                    #endregion
                }
                else
                {
                    #region 无护理险资格
                    billInfo.SelfPay += Convert.ToDecimal(selfPay);
                    billInfo.NCIItemTotalCost += Convert.ToDecimal(nciItemTotalCost);
                    billInfo.NCIPay += 0;
                    hospDay -= QueryLeaHospDays(billInfo.FeeNo, billInfo.BillMonth, hospDay, nciPayLevel, nciPaysCale, billInfo.BillId, billV2);
                    hospDay = hospDay <= 0 ? 0 : hospDay;
                    billInfo.HospDay += hospDay;
                    billInfo.NCIItemSelfPay += Convert.ToDecimal(nciItemTotalCost);
                    #endregion
                }
            }
        }

        public void NotHasQuotaInfo(BillV2Info billV2, bool isHaveNCI, BillV2 billInfo)
        {
            var feeRecordYearMonth = billV2.feeRecordList.Where(m => m.TakeTime >= billInfo.BalanceStartTime.Value && m.TakeTime <= billInfo.BalanceEndTime).ToList();
            if (feeRecordYearMonth != null && feeRecordYearMonth.Count > 0)
            {
                decimal payedAmount = 0;  //累计额度

                var selfPay = feeRecordYearMonth.Where(m => m.IsNCIItem == false).Sum(m => m.Cost);  //自费项目总费用
                var nciItemTotalCost = feeRecordYearMonth.Where(m => m.IsNCIItem == true).Sum(m => m.Cost); // 护理险项目总费用
                int hospDay = GetDiffDateDays(billInfo.BalanceStartTime.Value, billInfo.BalanceEndTime);
                var nciPayLevel = billInfo.NCIPayLevel;
                var nciPaysCale = billInfo.NCIPaysCale;

                // 是否拥有护理资格
                if (isHaveNCI)
                {
                    #region 有护理险资格
                    #region 账单数据更新
                    billInfo.SelfPay += Convert.ToDecimal(selfPay);
                    billInfo.NCIItemTotalCost += Convert.ToDecimal(nciItemTotalCost);
                    hospDay -= QueryLeaHospDays(billInfo.FeeNo, billInfo.BillMonth, hospDay, nciPayLevel, nciPaysCale, billInfo.BillId, billV2);
                    hospDay = hospDay <= 0 ? 0 : hospDay;
                    billInfo.HospDay += hospDay;

                    var nciPayCost = Convert.ToDecimal(hospDay * Convert.ToDecimal(nciPayLevel) * Convert.ToDecimal(nciPaysCale)); // 护理险最多可报销的费用

                    if (Convert.ToDecimal(nciItemTotalCost) <= nciPayCost)
                    {
                        billInfo.NCIPay += Convert.ToDecimal(nciItemTotalCost);
                        payedAmount = Convert.ToDecimal(nciItemTotalCost);
                        billInfo.NCIItemSelfPay += 0;
                    }
                    else
                    {
                        billInfo.NCIPay += Convert.ToDecimal(nciPayCost);
                        payedAmount = Convert.ToDecimal(nciPayCost);
                        billInfo.NCIItemSelfPay += Convert.ToDecimal(Convert.ToDecimal(nciItemTotalCost) - nciPayCost);
                    }
                    #endregion

                    #region 保存额度信息表 和 额度历史记录表  和 住民账户表
                    #region 保存额度信息表
                    MonthlyPayLimit monthLimit = new MonthlyPayLimit();
                    monthLimit.FeeNO = billInfo.FeeNo;
                    monthLimit.ResidentssID = billV2.IdNo;
                    monthLimit.PayedAmount = payedAmount;
                    monthLimit.YearMonth = billInfo.BillMonth;
                    monthLimit.NCIPayLevel = nciPayLevel;
                    monthLimit.NCIPaysCale = nciPaysCale;
                    monthLimit.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    monthLimit.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                    monthLimit.CreateTime = DateTime.Now;
                    int payLimitId = SaveMonthlyPayLimit(monthLimit);
                    #endregion
                    #region 保存额度历史记录表
                    MonthlyPayBillRecord monthBillRecord = new MonthlyPayBillRecord();
                    monthBillRecord.BillID = billInfo.BillId;
                    monthBillRecord.YearMonth = billInfo.BillMonth;
                    monthBillRecord.FeeNO = billInfo.FeeNo;
                    monthBillRecord.PayedAmount = payedAmount;
                    monthBillRecord.CompStartDate = billInfo.BalanceStartTime;
                    monthBillRecord.CompEndDate = billInfo.BalanceEndTime;
                    monthBillRecord.Status = 0;
                    monthBillRecord.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                    monthBillRecord.CreateTime = DateTime.Now;
                    monthBillRecord.IsDelete = false;
                    SaveMonthlyPayBillRecord(monthBillRecord);
                    #endregion
                    #endregion
                    #endregion
                }
                else
                {
                    #region 无护理险资格
                    billInfo.SelfPay += Convert.ToDecimal(selfPay);
                    billInfo.NCIItemTotalCost += Convert.ToDecimal(nciItemTotalCost);
                    billInfo.NCIPay += 0;
                    hospDay -= QueryLeaHospDays(billInfo.FeeNo, billInfo.BillMonth, hospDay, nciPayLevel, nciPaysCale, billInfo.BillId, billV2);
                    hospDay = hospDay <= 0 ? 0 : hospDay;
                    billInfo.HospDay += hospDay;
                    billInfo.NCIItemSelfPay += Convert.ToDecimal(nciItemTotalCost);
                    #endregion
                }
            }
        }

        /// <summary>
        /// 获取住民住院信息
        /// </summary>
        /// <param name="feeNo">住民编号</param>
        /// <returns>住院信息</returns>
        public Resident GetIpdRegInfo(long feeNo)
        {
            Mapper.CreateMap<LTC_IPDREG, Resident>();
            var ipdReg = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.FirstOrDefault(m => m.FEENO == feeNo);
            return  Mapper.Map<Resident>(ipdReg);
        }

        public RegNCIInfo GetNCIInfo(long feeNo)
        {
            Mapper.CreateMap<LTC_REGNCIINFO, RegNCIInfo>();
            var bal = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.FirstOrDefault(m => m.FEENO == feeNo && m.STATUS == 0);
            return Mapper.Map<RegNCIInfo>(bal);
        }
        #endregion
        #endregion


        #region 旧保存费用录入信息
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


        public int QueryLeaHospDays(long feeNo, string yearMomthStr, int hospDay, decimal? nciPayLevel, decimal? nciPaysCale,string billId, BillV2Info billV2)
        {
            var totalDays = 0;
            DateTime stopTime = new DateTime(Convert.ToInt32(yearMomthStr.Substring(0, 4)), Convert.ToInt32(yearMomthStr.Substring(5, 2)), 1);
            var model = (from net in unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(m => m.ISDELETE != true)
                         join lh in unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet on net.LEAVEID equals lh.ID
                         select new NCIDeductionModel
                         {
                             ID = net.ID,
                             FeeNo = lh.FEENO,
                             Debitmonth = net.DEBITMONTH,
                             Debitdays = net.DEBITDAYS,
                             IsDelete = net.ISDELETE,
                             Orgid = net.ORGID,
                             BillID = net.BILLID,
                             DeductionType = net.DEDUCTIONTYPE,
                             DeductionStatus = net.DEDUCTIONSTATUS
                         }).ToList();
            model = model.Where(m => m.FeeNo == feeNo && m.IsDelete != true && m.Orgid == SecurityHelper.CurrentPrincipal.OrgId && m.DeductionStatus == (int)DeductionStatus.UnCharge && m.DeductionType == (int)DeductionType.LeaveHosp).ToList();
            Mapper.CreateMap<LTC_NCIDEDUCTION, NCIDeductionModel>();
            var edductionList = Mapper.Map<List<NCIDeductionModel>>(model);
            if (edductionList != null && edductionList.Count > 0)
            {
                billV2.NCIDudeList = new List<NCIDeductionModel>();
                foreach (var item in edductionList)
                {
                    DateTime itemTime = new DateTime(Convert.ToInt32(item.Debitmonth.Substring(0, 4)), Convert.ToInt32(item.Debitmonth.Substring(5, 2)), 1);
                    if (itemTime <= stopTime)  //获取比当前月份小 的所有请假的请假天数
                    {
                        totalDays += item.Debitdays.Value;
                        var ncidedu = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(m => m.ID == item.ID).FirstOrDefault();
                        Mapper.CreateMap<LTC_NCIDEDUCTION, NCIDeductionModel>();
                        var dedu = Mapper.Map<NCIDeductionModel>(ncidedu);
                        dedu.BillID = billId;
                        dedu.Amount = Convert.ToDouble(item.Debitdays.Value * nciPayLevel.Value * nciPaysCale.Value);
                        dedu.DeductionReason = "请假自动扣除补助金额";
                        dedu.DeductionStatus = (int)DeductionStatus.Charged;
                        dedu.Updateby = SecurityHelper.CurrentPrincipal.EmpNo;
                        dedu.UpdateTime = DateTime.Now;
                        billV2.NCIDudeList.Add(dedu);
                    }
                }
            }

            int? debitDays = 0;
            var billList = unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(m => m.BILLMONTH == yearMomthStr && m.FEENO == feeNo && m.ISDELETE != true && m.STATUS != (int)BillStatus.Refund).ToList();
            if (billList != null && billList.Count > 0)
            {
                foreach (var bill in billList)
                {
                    debitDays += (unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId && m.DEDUCTIONSTATUS == (int)DeductionStatus.Charged && m.DEDUCTIONTYPE == (int)DeductionType.LeaveHosp && m.BILLID == bill.BILLID).Sum(m => m.DEBITDAYS) ?? 0);
                }
            }

            return totalDays + (debitDays??0);
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


        public int GetRangeHospDay(BillV2 billv2, DateTime minDate, DateTime maxDate, DateTime startDate, DateTime endDate)
        {
            DateTime beginDate;
            DateTime finishDate;

            DateTime odate = new DateTime(0001, 1, 1);
            if (minDate == odate)
            {
                minDate = startDate;
            }
            if (maxDate == odate)
            {
                maxDate = endDate;
            }
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

            if (Convert.ToDateTime(billv2.BalanceStartTime) >= beginDate)
            {
                billv2.BalanceStartTime = beginDate;
            }
            if (Convert.ToDateTime(billv2.BalanceEndTime) <= finishDate)
            {
                billv2.BalanceEndTime = finishDate;
            }
            return GetDiffDateDays(beginDate, finishDate);
        }


        public int GetRangeHospDay(LTC_BILLV2 billv2, DateTime minDate, DateTime maxDate, DateTime startDate, DateTime endDate)
        {
            DateTime beginDate;
            DateTime finishDate;

            DateTime odate = new DateTime(0001, 1, 1);
            if (minDate == odate)
            {
                minDate = startDate;
            }
            if (maxDate == odate)
            {
                maxDate = endDate;
            }
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

            if (Convert.ToDateTime(billv2.BALANCESTARTTIME) >= beginDate)
            {
                billv2.BALANCESTARTTIME = beginDate;
            }
            if (Convert.ToDateTime(billv2.BALANCEENDTIME) <= finishDate)
            {
                billv2.BALANCEENDTIME = finishDate;
            }
            return GetDiffDateDays(beginDate, finishDate);
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
