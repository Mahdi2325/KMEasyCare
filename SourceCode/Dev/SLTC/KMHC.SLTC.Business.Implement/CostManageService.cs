using System;
using AutoMapper;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KMHC.Infrastructure;
using System.Collections;
using System.Reflection;
using System.ComponentModel;


namespace KMHC.SLTC.Business.Implement
{
    public class CostManageService : BaseService, ICostManageService
    {
        #region LTC_PAYBILL
        public BaseResponse<IList<PayBill>> QueryPayBill(BaseRequest<PayBillFilter> request)
        {
            var response = base.Query<LTC_PAYBILL, PayBill>(request, (q) =>
            {
                if (request != null && request.Data.BillId > 0)
                {
                    q = q.Where(m => m.BILLID == request.Data.BillId);
                }
                q = q.OrderBy(m => m.PAYBILLNO);
                return q;
            });
            return response;
        }


        public BaseResponse<decimal?> getSum(BaseRequest<PayBillFilter> request)
        {
            BaseResponse<decimal?> response = new BaseResponse<decimal?>();
            response.Data = 0;
            try
            {
                if (request.Data.BillId > 0)
                {
                    var ss = (from o in unitOfWork.GetRepository<LTC_PAYBILL>().dbSet
                              where o.BILLID == request.Data.BillId
                              select 0).Count();
                    if (ss > 0)
                    {
                        response.Data = base.unitOfWork.GetRepository<LTC_PAYBILL>().dbSet.Where(t => (t.BILLID == request.Data.BillId)).GroupBy(e => e.BILLID).Select(g => new { sumCost = g.Sum(e => e.COST) }).FirstOrDefault().sumCost;
                    }
                    else
                    {
                        response.Data = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public BaseResponse<PayBill> GetPayBill(long id)
        {
            return base.Get<LTC_PAYBILL, PayBill>((q) => q.ID == id);
        }

        public BaseResponse<PayBill> SavePayBill(PayBill request)
        {
            if (request.Id == 0)
            {
                request.PayBillNo = base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.PayBillNo);
            }

            #region 当缴费已完成时，更新账单状态
            if (request.BillStatus == "003")
            {
                var billInfo = unitOfWork.GetRepository<LTC_BILL>().dbSet.Where(m => m.ID == request.BillId).FirstOrDefault();
                if (billInfo != null)
                {
                    billInfo.BILLSTATE = GetEnumDescription(EnumBillState.Close).ToString();
                    unitOfWork.GetRepository<LTC_BILL>().Update(billInfo);
                    unitOfWork.Save();
                }
            }
            #endregion
            return base.Save<LTC_PAYBILL, PayBill>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeletePaybill(long id)
        {
            return base.Delete<LTC_PAYBILL>(id);
        }



        #endregion

        #region LTC_FIXEDCOST
        public BaseResponse<IList<FixedCost>> QueryFixedCost(BaseRequest<FixedCostFilter> request)
        {

            BaseResponse<IList<FixedCost>> response = new BaseResponse<IList<FixedCost>>();
            var q = from n in unitOfWork.GetRepository<LTC_FIXEDCOST>().dbSet select new { FixedCost = n };
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.FixedCost.ORGID == request.Data.OrgId);
            }
            if (request.Data.FeeNo >= 0)
            {
                q = q.Where(m => m.FixedCost.FEENO == request.Data.FeeNo);
            }
            q = q.OrderBy(m => m.FixedCost.COSTITEMNO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<FixedCost>();
                foreach (dynamic item in list)
                {
                    FixedCost newItem = Mapper.DynamicMap<FixedCost>(item.FixedCost);
                    response.Data.Add(newItem);
                }
            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;

        }

        public BaseResponse<FixedCost> GetFixedCost(int id)
        {
            return base.Get<LTC_FIXEDCOST, FixedCost>((q) => q.ID == id);
        }

        public BaseResponse<FixedCost> SaveFixedCost(FixedCost request)
        {
            return base.Save<LTC_FIXEDCOST, FixedCost>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse<List<FixedCost>> SaveFixedCost(int groupId, int regNo, long feeNo, DateTime? StartDate, DateTime? EndDate)
        {
            BaseResponse<List<FixedCost>> response = new BaseResponse<List<FixedCost>>();
            response.Data = new List<FixedCost>();
            var costGroup = this.GetCostGroupExtend(groupId);
            if (costGroup.ResultCode == 0)
            {
                string orgId = SecurityHelper.CurrentPrincipal.OrgId;
                unitOfWork.BeginTransaction();
                costGroup.Data.GroupItems.ForEach(it =>
                {
                    var newItem = new FixedCost()
                    {
                        OrgId = orgId,
                        RegNo = regNo,
                        FeeNo = feeNo,
                        CostItemId = it.CostItemId,
                        ItemUnit = it.ItemUnit,
                        CostItemNo = it.CostItemNo,
                        CostName = it.CostName,
                        Price = it.Price,
                        Period = it.Period,
                        RepeatCount = it.RepeatCount,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        IsEndCharGes = false
                    };
                    base.Save<LTC_FIXEDCOST, FixedCost>(newItem, (q) => q.ID == newItem.Id);
                    response.Data.Add(newItem);
                });
                unitOfWork.Commit();
            }

            if (response.Data != null && response.Data.Count > 0)
            {
                response.ResultCode = 1002;
            }
            else
            {
                response.ResultCode = 1001;
            }
            return response;
        }

        public BaseResponse DeleteFixedCost(int id)
        {
            return base.Delete<LTC_FIXEDCOST>(id);
        }
        #endregion

        #region LTC_COSTITEM
        public BaseResponse<IList<CostItem>> QueryCostItem(BaseRequest<CostItemFilter> request)
        {
            var response = base.Query<LTC_COSTITEM, CostItem>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (!string.IsNullOrEmpty(request.Data.CostItemNo))
                {
                    q = q.Where(m => m.COSTITEMNO.Contains(request.Data.CostItemNo));
                }
                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        public BaseResponse<CostItem> GetCostItem(int id)
        {
            return base.Get<LTC_COSTITEM, CostItem>((q) => q.ID == id);
        }

        public BaseResponse<CostItem> SaveCostItem(CostItem request)
        {
            return base.Save<LTC_COSTITEM, CostItem>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteCostItem(int id)
        {
            unitOfWork.GetRepository<LTC_FIXEDCOST>().Delete(p => p.COSTITEMID == id);
            return base.Delete<LTC_COSTITEM>(id);
        }
        #endregion

        #region LTC_COSTGROUP
        public BaseResponse<IList<CostGroup>> QueryCostGroup(BaseRequest<CostGroupFilter> request)
        {
            var response = base.Query<LTC_COSTGROUP, CostGroup>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }

                if (!string.IsNullOrWhiteSpace(request.Data.GroupName))
                {
                    q = q.Where(m => m.GROUPNAME.Contains(request.Data.GroupName) || m.GROUPNO.Contains(request.Data.GroupName));
                }

                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        public BaseResponse<CostGroup> GetCostGroupExtend(int id)
        {
            var response = base.Get<LTC_COSTGROUP, CostGroup>((q) => q.ID == id);
            if (response != null && response.Data != null)
            {
                response.Data.GroupItems = (from it in unitOfWork.GetRepository<LTC_COSTGROUPDTL>().dbSet
                                            where it.GROUPID == response.Data.Id
                                            select new CostGroupDtl
                                            {
                                                Id = it.ID,
                                                GroupId = it.GROUPID,
                                                CostItemId = it.COSTITEMID ?? 0,
                                                ItemUnit = it.ITEMUNIT,
                                                CostItemNo = it.COSTITEMNO,
                                                CostName = it.COSTNAME,
                                                Price = it.PRICE,
                                                Period = it.PERIOD,
                                                RepeatCount = it.REPEATCOUNT
                                            }).ToList();
            }
            return response;
        }
        public BaseResponse<CostGroup> GetCostGroup(int id)
        {
            return base.Get<LTC_COSTGROUP, CostGroup>((q) => q.ID == id);
        }

        public BaseResponse<CostGroup> SaveCostGroup(CostGroup request)
        {
            var response = new BaseResponse<CostGroup>();

            var count = unitOfWork.GetRepository<LTC_COSTGROUP>().dbSet.Count(o => o.ORGID == request.OrgId && o.ID != request.Id && (o.GROUPNAME == request.GroupName || o.GROUPNO == request.GroupNo));
            if (count > 0)
            {
                response.ResultCode = 1001;  //添加失败
            }
            else
            {
                response = base.Save<LTC_COSTGROUP, CostGroup>(request, (q) => q.ID == request.Id);
                if (response.Data.Id > 0 && request.GroupItems != null && request.GroupItems.Count > 0)
                {
                    unitOfWork.BeginTransaction();
                    request.GroupItems.ForEach((p) =>
                    {
                        p.GroupId = response.Data.Id;
                        base.Save<LTC_COSTGROUPDTL, CostGroupDtl>(p, (q) => p.Id == q.ID);
                    });

                    unitOfWork.Commit();
                    response.ResultCode = 1002; // 保存成功
                }
            }
            return response;
        }

        public BaseResponse DeleteCostGroup(int id)
        {
            return base.Delete<LTC_COSTGROUP>(id);
        }
        #endregion

        #region LTC_COSTGROUPDTL
        public BaseResponse<IList<CostGroupDtl>> QueryCostGroupDtl(BaseRequest<CostGroupDtlFilter> request)
        {
            var response = base.Query<LTC_COSTGROUPDTL, CostGroupDtl>(request, (q) =>
            {
                if (request.Data.GroupId > 0)
                {
                    q = q.Where(m => m.GROUPID == request.Data.GroupId);
                }
                q = q.OrderBy(m => m.ID);
                return q;
            });
            return response;
        }

        public BaseResponse<CostGroupDtl> GetCostGroupDtl(int id)
        {
            return base.Get<LTC_COSTGROUPDTL, CostGroupDtl>((q) => q.ID == id);
        }

        public BaseResponse<CostGroupDtl> SaveCostGroupDtl(CostGroupDtl request)
        {
            return base.Save<LTC_COSTGROUPDTL, CostGroupDtl>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteCostGroupDtl(int id)
        {
            return base.Delete<LTC_COSTGROUPDTL>(id);
        }

        public BaseResponse DeleteCostGroupDtl(List<string> ids)
        {
            var response = new BaseResponse();
            unitOfWork.BeginTransaction();
            ids.ForEach((p) =>
            {
                base.Delete<LTC_COSTGROUPDTL>(int.Parse(p));
            });
            unitOfWork.Commit();
            return response;
        }

        public int DeleteCostGroupDtlByGroupId(int groupId)
        {
            return base.Delete<LTC_COSTGROUPDTL>((p) => p.GROUPID == groupId);
        }
        #endregion

        #region LTC_COSTDTL
        public BaseResponse<IList<CostDtl>> QueryCostDtl(BaseRequest<CostDtlFilter> request)
        {
            var response = base.Query<LTC_COSTDTL, CostDtl>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }

                if (request.Data.BillId > 0)
                {
                    q = q.Where(m => m.BILLID == request.Data.BillId);
                    q = q.OrderBy(m => m.ID);
                }
                if (request.Data.BillId == 0)
                {
                    if (request.Data.FeeNo >= 0)
                    {
                        q = q.Where(m => m.FEENO == request.Data.FeeNo);
                    }
                    q = q.Where(m => m.COSTSOURCE == null);
                    q = q.OrderByDescending(m => m.ID);
                }


                return q;
            });
            return response;
        }

        public BaseResponse<CostDtl> GetCostDtl(long id)
        {
            return base.Get<LTC_COSTDTL, CostDtl>((q) => q.ID == id);
        }

        public BaseResponse<CostDtl> SaveCostDtl(CostDtl request)
        {
            return base.Save<LTC_COSTDTL, CostDtl>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteCostDtl(long id)
        {
            return base.Delete<LTC_COSTDTL>(id);
        }
        #endregion

        #region LTC_BILL
        public BaseResponse<IList<Bill>> QueryBill(BaseRequest<BillFilter> request)
        {
            var response = base.Query<LTC_BILL, Bill>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.BILLNO);
                return q;
            });
            return response;
        }

        public BaseResponse<Bill> GetBill(long id)
        {
            return base.Get<LTC_BILL, Bill>((q) => q.ID == id);
        }


        #region 生成账单

        public BaseResponse GenerateBill(int FeeNo, string OrgId)
        {
            BaseResponse response = new BaseResponse();

            var nowTime = DateTime.Now.Date;
            var minByMonth = new DateTime(nowTime.Year, nowTime.Month, 1);
            var days = DateTime.DaysInMonth(nowTime.Year, nowTime.Month);
            var maxByMonth = new DateTime(nowTime.Year, nowTime.Month, days);

            // 周期为天的固定收费
            var fixedCostByDay = unitOfWork.GetRepository<LTC_FIXEDCOST>().dbSet
                .Where(m => m.PERIOD == "Day");
            var fixedCostByDayList = fixedCostByDay.ToList();
            // 周期为月的固定收费
            var fixedCostByMonth = unitOfWork.GetRepository<LTC_FIXEDCOST>().dbSet.Where(m => m.PERIOD == "Month");
            var fixedCostByMonthList = fixedCostByMonth.ToList();
            //.ToList();
            // 查找所有没有账单的收费明细
            var costDtl = unitOfWork.GetRepository<LTC_COSTDTL>().dbSet.Where(m => !m.BILLID.HasValue || (m.BILLID.HasValue && m.BILLID.Value == 0));
            var costDtlList = costDtl.ToList();

            // 查找有固定收费或有示出账单的收费明细的住民
            var q = from reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on reg.REGNO equals ipd.REGNO
                    where costDtl.Any(m => m.FEENO == ipd.FEENO) || fixedCostByDay.Any(m => m.FEENO == ipd.FEENO) || fixedCostByMonth.Any(m => m.FEENO == ipd.FEENO)
                    select new BillRelated { REGNO = reg.REGNO, ORGID = reg.ORGID, FEENO = ipd.FEENO };

            q = q.Distinct();

            q = q.Where(m => m.FEENO == FeeNo && m.ORGID == OrgId);

            var ipdList = q.ToList();

            var feeNoList = ipdList.Select(m => m.FEENO).ToList();

            // 需要出账单且在这个月有请假的请假数据
            var leavehospList = unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet.Where(m => feeNoList.Contains(m.FEENO ?? 0)
                && ((m.STARTDATE.HasValue && m.STARTDATE.Value.CompareTo(minByMonth) <= 0 && m.ENDDATE.HasValue && m.ENDDATE.Value.CompareTo(minByMonth) >= 0)
                || (m.STARTDATE.HasValue && m.STARTDATE.Value.CompareTo(maxByMonth) <= 0 && m.ENDDATE.HasValue && m.ENDDATE.Value.CompareTo(maxByMonth) >= 0))
                ).ToList();
            var calculateCostList = new CalculateList();
            // 生成账单
            ipdList.ForEach(ipd =>
            {
                // 计算出账单费用
                calculateCostList = CalculateCost(ipd, nowTime, minByMonth, maxByMonth, days, fixedCostByDayList, fixedCostByMonthList, costDtlList, leavehospList);

                if (calculateCostList.Cost != 0)
                {
                    LTC_BILL model = new LTC_BILL();
                    model.BILLNO = base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.BillNo); //???
                    model.BILLSTATE = GetEnumDescription(EnumBillState.Open).ToString();

                    model.COST = calculateCostList.Cost;
                    model.DESCRIPTION = DateTime.Now.ToString("yyyy-MM-dd");

                    if (calculateCostList.FixedMonthCost != 0)
                    {
                        var minDate = calculateCostList.ReturnMonthFixedCost.Where(m => m.RepeatCount != 0).Select(m => m.StartDate).Min();
                        var maxCount = calculateCostList.ReturnMonthFixedCost.Where(m => m.RepeatCount != 0).Select(m => m.RepeatCount).Max();

                        model.BILLDATE = minDate;
                        model.BILLENDDATE = Convert.ToDateTime(minDate).AddMonths(Convert.ToInt32(maxCount));
                    }
                    else if (calculateCostList.FixedMonthCost == 0 && calculateCostList.FixedDayCost != 0)
                    {
                        var minDate = calculateCostList.ReturnDayFixedCost.Where(m => m.RepeatCount != 0).Select(m => m.StartDate).Min();
                        var maxCount = calculateCostList.ReturnDayFixedCost.Where(m => m.RepeatCount != 0).Select(m => m.RepeatCount).Max();

                        model.BILLDATE = minDate;
                        model.BILLENDDATE = Convert.ToDateTime(minDate).AddDays(Convert.ToInt32(maxCount));
                    }
                    else
                    {
                        model.BILLDATE = DateTime.Now;
                        model.BILLENDDATE = DateTime.Now;
                    }

                    model.FEENO = ipd.FEENO;
                    model.REGNO = ipd.REGNO;
                    model.ORGID = ipd.ORGID;
                    model.CREATEDATE = nowTime;
                    unitOfWork.GetRepository<LTC_BILL>().Insert(model);
                    ipd.Bill = model;
                    response.ResultCode = 1001;
                }
                else
                {
                    response.ResultCode = 1002;
                }
            });

            unitOfWork.Save();
            // 反写收费明细表的账单ID
            ipdList.ForEach(ipd =>
            {
                ipd.CostDtlList.ForEach(cost =>
                {
                    cost.BILLID = ipd.Bill.ID;
                    unitOfWork.GetRepository<LTC_COSTDTL>().Update(cost);
                });

                if (calculateCostList.FixedDayCost != 0)
                {
                    calculateCostList.ReturnDayFixedCost.ForEach(fixInfo =>
                    {
                        if (fixInfo.RepeatCount != 0)
                        {
                            LTC_COSTDTL costdtl = new LTC_COSTDTL
                            {
                                COSTITEMNO = fixInfo.CostItemNo,
                                COSTNAME = fixInfo.CostName,
                                ITEMTYPE = fixInfo.Period,
                                OCCURTIME = fixInfo.GenerateDate,
                                QUANTITY = fixInfo.RepeatCount,
                                PRICE = fixInfo.Price,
                                TOTALPRICE = (fixInfo.RepeatCount) * (fixInfo.Price),
                                SELFFLAG = true,
                                DESCRIPTION = Convert.ToDateTime(fixInfo.StartDate).ToString("yyyy-MM-dd") + " 至  " + Convert.ToDateTime(fixInfo.StartDate).AddDays(Convert.ToDouble(fixInfo.RepeatCount)).ToString("yyyy-MM-dd"),
                                REGNO = fixInfo.RegNo,
                                FEENO = fixInfo.FeeNo,
                                ORGID = fixInfo.OrgId,
                                COSTSOURCE = fixInfo.Id,
                                BILLID = Convert.ToInt32(ipd.Bill.ID)
                            };
                            unitOfWork.GetRepository<LTC_COSTDTL>().Insert(costdtl);
                        }
                    });
                }

                if (calculateCostList.FixedMonthCost != 0)
                {
                    calculateCostList.ReturnMonthFixedCost.ForEach(fixInfo =>
                    {
                        if (fixInfo.RepeatCount != 0)
                        {
                            for (int i = 1; i <= fixInfo.RepeatCount; i++)
                            {
                                if (i == 1)
                                {
                                    LTC_COSTDTL costdtl = new LTC_COSTDTL
                                    {
                                        COSTITEMNO = fixInfo.CostItemNo,
                                        COSTNAME = fixInfo.CostName,
                                        ITEMTYPE = fixInfo.Period,
                                        OCCURTIME = fixInfo.GenerateDate,
                                        QUANTITY = 1,
                                        PRICE = fixInfo.Price,
                                        TOTALPRICE = 1 * (fixInfo.Price),
                                        SELFFLAG = true,
                                        DESCRIPTION = Convert.ToDateTime(fixInfo.StartDate).ToString("yyyy-MM-dd") + " 至  " + Convert.ToDateTime(fixInfo.StartDate).AddMonths(1).ToString("yyyy-MM-dd"),
                                        REGNO = fixInfo.RegNo,
                                        FEENO = fixInfo.FeeNo,
                                        ORGID = fixInfo.OrgId,
                                        COSTSOURCE = fixInfo.Id,
                                        BILLID = Convert.ToInt32(ipd.Bill.ID)
                                    };
                                    unitOfWork.GetRepository<LTC_COSTDTL>().Insert(costdtl);
                                }
                                else
                                {
                                    LTC_COSTDTL costdtl = new LTC_COSTDTL
                                    {
                                        COSTITEMNO = fixInfo.CostItemNo,
                                        COSTNAME = fixInfo.CostName,
                                        ITEMTYPE = fixInfo.Period,
                                        OCCURTIME = fixInfo.GenerateDate,
                                        QUANTITY = 1,
                                        PRICE = fixInfo.Price,
                                        TOTALPRICE = 1 * (fixInfo.Price),
                                        SELFFLAG = true,
                                        DESCRIPTION = Convert.ToDateTime(fixInfo.StartDate).AddMonths(i - 1).AddDays(1).ToString("yyyy-MM-dd") + " 至  " + Convert.ToDateTime(fixInfo.StartDate).AddMonths(i).ToString("yyyy-MM-dd"),
                                        REGNO = fixInfo.RegNo,
                                        FEENO = fixInfo.FeeNo,
                                        ORGID = fixInfo.OrgId,
                                        COSTSOURCE = fixInfo.Id,
                                        BILLID = Convert.ToInt32(ipd.Bill.ID)
                                    };
                                    unitOfWork.GetRepository<LTC_COSTDTL>().Insert(costdtl);
                                }
                            }

                        }
                    });
                }

            });

            unitOfWork.Save();

            if (ipdList.Count == 0)
            {
                response.ResultCode = 1002;
            }
            return response;
        }

        #region 账单价格

        /// <summary>
        /// 生成账单价格
        /// </summary>
        /// <param name="billRelated">生成账单的住民</param>
        /// <param name="nowDate">当前时间</param>
        /// <param name="minByMonth">最小月份时间</param>
        /// <param name="maxByMonth">最大月份时间</param>
        /// <param name="days">时间</param>
        /// <param name="fixedCostByDay">固定费用录入（日）</param>
        /// <param name="fixedCostByMonth">固定费用录入（月）</param>
        /// <param name="costDtl">费用录入</param>
        /// <param name="leavehospList">离院记录</param>
        /// <returns>价格</returns>
        private CalculateList CalculateCost(BillRelated billRelated, DateTime nowDate, DateTime minByMonth, DateTime maxByMonth, int days, List<LTC_FIXEDCOST> fixedCostByDay, List<LTC_FIXEDCOST> fixedCostByMonth, List<LTC_COSTDTL> costDtl, List<LTC_LEAVEHOSP> leavehospList)
        {
            long feeNo = billRelated.FEENO;

            decimal dtlCost = 0;
            decimal fixedDayCost = 0;
            decimal fixedMonthCost = 0;
            decimal totalcost = 0;
            DateTime repeatStartDate;
            DateTime repeatDayDate;

            CalculateList calculateList = new CalculateList();
            List<FixedCost> returnDayFixedCost = new List<FixedCost>();
            List<FixedCost> returnMonthFixedCost = new List<FixedCost>();

            #region 费用录入价格
            billRelated.CostDtlList = costDtl.FindAll(m => m.FEENO == feeNo && m.BILLID == 0);
            dtlCost += billRelated.CostDtlList.Sum(m => m.TOTALPRICE ?? 0);
            #endregion


            #region 累加按天的固定收费

            var fixedCostByDayFeeNo = fixedCostByDay.FindAll(m => m.FEENO == feeNo);

            fixedCostByDayFeeNo.ForEach(m =>
            {
                int leaveDay = 0;
                int repeatCount = 0;
                repeatDayDate = nowDate;


                if (!Convert.ToBoolean(m.ISENDCHARGES))
                {
                    if (m.ENDDATE.HasValue && m.ENDDATE != null)
                    {
                        if (m.REPEATCOUNT == null || m.REPEATCOUNT == 0)
                        {
                            repeatCount = DateDiff(Convert.ToDateTime(m.STARTDATE), Convert.ToDateTime(m.ENDDATE));
                            repeatDayDate = Convert.ToDateTime(m.STARTDATE);
                            //leaveDay = LeaveDays(leavehospList, Convert.ToDateTime(m.STARTDATE), repeatCount);
                        }
                        else if (m.REPEATCOUNT.HasValue && m.REPEATCOUNT != 0)
                        {
                            DateTime RePeatDay = m.STARTDATE.AddDays(Convert.ToDouble(m.REPEATCOUNT));
                            if (RePeatDay >= Convert.ToDateTime(m.ENDDATE))
                            {
                                repeatCount = 0;
                                //leaveDay = 0;
                            }
                            else
                            {
                                repeatCount = DateDiff(Convert.ToDateTime(RePeatDay), Convert.ToDateTime(m.ENDDATE));
                                repeatDayDate = Convert.ToDateTime(RePeatDay);
                                //leaveDay = LeaveDays(leavehospList, RePeatDay, repeatCount);
                            }
                        }
                    }
                    else
                    {
                        if (m.REPEATCOUNT == null || m.REPEATCOUNT == 0)
                        {
                            repeatCount = DateDiff(Convert.ToDateTime(m.STARTDATE), nowDate);
                            repeatDayDate = Convert.ToDateTime(m.STARTDATE);
                            //leaveDay = LeaveDays(leavehospList, Convert.ToDateTime(m.STARTDATE), repeatCount);
                        }
                        else if (m.REPEATCOUNT.HasValue && m.REPEATCOUNT != 0)
                        {
                            DateTime RePeatDay = m.STARTDATE.AddDays(Convert.ToDouble(m.REPEATCOUNT));
                            if (RePeatDay >= nowDate)
                            {
                                repeatCount = 0;
                                //leaveDay = 0;
                            }
                            else
                            {
                                repeatCount = DateDiff(RePeatDay, nowDate);
                                repeatDayDate = Convert.ToDateTime(RePeatDay);
                                // leaveDay = LeaveDays(leavehospList, RePeatDay, repeatCount);
                            }
                        }
                    }
                }
                if (m.PRICE.HasValue && m.PRICE != null)
                {
                    var total = Convert.ToDecimal(m.PRICE) * repeatCount;
                    fixedDayCost += total;
                }
                else
                {
                    var total = 0 * repeatCount;
                    fixedDayCost += total;
                }

                m.GENERATEDATE = nowDate;
                m.REPEATCOUNT += repeatCount;
                unitOfWork.GetRepository<LTC_FIXEDCOST>().Update(m);

                FixedCost fixDay = new FixedCost();
                fixDay.Id = m.ID;
                fixDay.CostItemNo = m.COSTITEMNO;
                fixDay.CostName = m.COSTNAME;
                fixDay.Period = m.PERIOD;
                fixDay.GenerateDate = nowDate;
                fixDay.RegNo = m.REGNO;
                fixDay.FeeNo = m.FEENO;
                fixDay.OrgId = m.ORGID;
                fixDay.Price = m.PRICE;
                fixDay.StartDate = repeatDayDate;
                fixDay.RepeatCount = repeatCount;
                returnDayFixedCost.Add(fixDay);

            });
            #endregion

            #region 累加按月固定收费

            var fixedCostByMonthFeeNo = fixedCostByMonth.FindAll(m => m.FEENO == feeNo);
            fixedCostByMonthFeeNo.ForEach(m =>
            {
                int leaveDay = 0;
                int repeatCount = 0;
                repeatStartDate = new DateTime(1900, 1, 1);
                if (!Convert.ToBoolean(m.ISENDCHARGES))
                {
                    if (m.ENDDATE.HasValue && m.ENDDATE != null)
                    {
                        if (m.REPEATCOUNT == null || m.REPEATCOUNT == 0)
                        {
                            repeatCount = DiffDateMonth(Convert.ToDateTime(m.STARTDATE), Convert.ToDateTime(m.ENDDATE));
                            repeatStartDate = Convert.ToDateTime(m.STARTDATE);
                            //repeatCount = DateDiffMonth(ReturnStartDate(Convert.ToDateTime(m.STARTDATE)), ReturnEndDate(Convert.ToDateTime(m.ENDDATE)));
                            //leaveDay = LeaveDays(leavehospList, Convert.ToDateTime(m.STARTDATE), repeatCount);
                        }
                        else if (m.REPEATCOUNT.HasValue && m.REPEATCOUNT != 0)
                        {
                            //DateTime RePeatDay = ReturnStartDate(Convert.ToDateTime(m.STARTDATE)).AddMonths(Convert.ToInt32(m.REPEATCOUNT));
                            DateTime RePeatDay = Convert.ToDateTime(m.STARTDATE).AddMonths(Convert.ToInt32(m.REPEATCOUNT));
                            if (RePeatDay > Convert.ToDateTime(m.ENDDATE))
                            {
                                repeatCount = 0;
                                //leaveDay = 0;
                            }
                            else
                            {
                                repeatCount = DiffDateMonth(Convert.ToDateTime(RePeatDay), Convert.ToDateTime(m.ENDDATE));
                                repeatStartDate = Convert.ToDateTime(RePeatDay);
                                // repeatCount = DateDiffMonth(Convert.ToDateTime(RePeatDay), ReturnEndDate(Convert.ToDateTime(m.ENDDATE)));
                                //leaveDay = LeaveDays(leavehospList, RePeatDay, repeatCount);
                            }
                        }
                    }
                    else
                    {
                        if (m.REPEATCOUNT == null || m.REPEATCOUNT == 0)
                        {
                            repeatCount = DiffDateMonth(Convert.ToDateTime(m.STARTDATE), nowDate);
                            repeatStartDate = Convert.ToDateTime(Convert.ToDateTime(m.STARTDATE));
                            //repeatCount = DateDiffMonth(ReturnStartDate(Convert.ToDateTime(m.STARTDATE)), ReturnEndDate(nowDate));
                            //leaveDay = LeaveDays(leavehospList, Convert.ToDateTime(m.STARTDATE), repeatCount);
                        }
                        else if (m.REPEATCOUNT.HasValue && m.REPEATCOUNT != 0)
                        {
                            DateTime RePeatDay = Convert.ToDateTime(m.STARTDATE).AddMonths(Convert.ToInt32(m.REPEATCOUNT));
                            //DateTime RePeatDay = ReturnStartDate(Convert.ToDateTime(m.STARTDATE)).AddMonths(Convert.ToInt32(m.REPEATCOUNT));
                            if (RePeatDay > nowDate)
                            {
                                repeatCount = 0;
                                //leaveDay = 0;
                            }
                            else
                            {
                                repeatCount = DiffDateMonth(RePeatDay, nowDate);
                                repeatStartDate = RePeatDay;
                                //repeatCount = DateDiffMonth(RePeatDay,  ReturnEndDate(nowDate));
                                // leaveDay = LeaveDays(leavehospList, RePeatDay, repeatCount);
                            }
                        }
                    }
                }
                if (m.PRICE.HasValue && m.PRICE != null)
                {
                    var total = Convert.ToDecimal(m.PRICE) * repeatCount;
                    fixedMonthCost += total;
                }
                else
                {
                    var total = 0 * repeatCount;
                    fixedMonthCost += total;
                }

                m.GENERATEDATE = nowDate;
                m.REPEATCOUNT += repeatCount;
                unitOfWork.GetRepository<LTC_FIXEDCOST>().Update(m);

                FixedCost fixMonth = new FixedCost();
                fixMonth.Id = m.ID;
                fixMonth.CostItemNo = m.COSTITEMNO;
                fixMonth.CostName = m.COSTNAME;
                fixMonth.Period = m.PERIOD;
                fixMonth.GenerateDate = nowDate;
                fixMonth.RegNo = m.REGNO;
                fixMonth.FeeNo = m.FEENO;
                fixMonth.OrgId = m.ORGID;
                fixMonth.Price = m.PRICE;
                fixMonth.RepeatCount = repeatCount;
                fixMonth.StartDate = repeatStartDate;
                returnMonthFixedCost.Add(fixMonth);
            });
            #endregion

            calculateList.Cost = dtlCost + fixedDayCost + fixedMonthCost;  //账单总价

            calculateList.FixedDayCost = fixedDayCost;
            calculateList.FixedMonthCost = fixedMonthCost;

            calculateList.ReturnDayFixedCost = returnDayFixedCost;
            calculateList.ReturnMonthFixedCost = returnMonthFixedCost;
            return calculateList;
        }

        #endregion

        #region 天数计算
        /// <summary>
        /// 账单应付的天数
        /// </summary>
        /// <param name="leavehospList">离院记录</param>
        /// <param name="closeDate">关闭时间</param>
        /// <param name="nowDate">当前时间</param>
        /// <param name="days">天数</param>
        /// <returns>天数</returns>
        private int DaysByIPD(List<LTC_LEAVEHOSP> leavehospList, DateTime? closeDate, DateTime nowDate, int days)
        {
            int dayByIPD = 0;
            for (int i = 1; i <= days; i++)
            {
                var tmpDate = new DateTime(nowDate.Year, nowDate.Month, i);
                if (!closeDate.HasValue || (closeDate.HasValue && tmpDate.CompareTo(closeDate.Value) > 0))
                {
                    var leavehospFlag = leavehospList.Any(l => l.STARTDATE.HasValue && l.STARTDATE.Value.CompareTo(tmpDate) <= 0 && l.ENDDATE.HasValue && l.ENDDATE.Value.CompareTo(tmpDate) >= 0);
                    if (!leavehospFlag)
                    {
                        dayByIPD++;
                    }
                }
                if (nowDate.Date.Day == i)
                {
                    break;
                }
            }
            return dayByIPD;
        }

        /// <summary>
        /// 计算请假天数
        /// </summary>
        /// <param name="leavehospList">请假记录List</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="repeatCount">天数</param>
        /// <returns>请假天数</returns>
        private int LeaveDays(List<LTC_LEAVEHOSP> leavehospList, DateTime startDate, int repeatCount)
        {
            int leaveDay = 0;
            for (int i = 1; i <= repeatCount; i++)
            {
                var tmpDate = startDate.AddDays(i);
                var leavehospFlag = leavehospList.Any(l => l.STARTDATE.HasValue && l.STARTDATE.Value.CompareTo(tmpDate) <= 0 && l.ENDDATE.HasValue && l.ENDDATE.Value.CompareTo(tmpDate) >= 0);
                if (!leavehospFlag)
                {
                    leaveDay++;
                }
            }
            return leaveDay;
        }

        #endregion

        #region 附加方法

        /// <summary>
        /// 计算两个时间的时间差
        /// </summary>
        /// <param name="DateTime1">时间1</param>
        /// <param name="DateTime2">时间2</param>
        /// <returns>相差天数</returns>
        private int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts = DateTime1.Subtract(DateTime2).Duration();
            return ts.Days;
        }
        #region 月账单生成方法  (New)
        /// <summary>
        /// 日期相差的月数
        /// </summary>
        /// <param name="datetime1">开始日期</param>
        /// <param name="datetime2">结束日期</param>
        /// <returns>相差的月数</returns>
        private int DiffDateMonth(DateTime datetime1, DateTime datetime2)
        {
            int y = datetime2.Year - datetime1.Year; //年差
            int m = y * 12 + datetime2.Month - datetime1.Month;  // 月差
            int d = datetime2.Day - datetime1.Day;
            if (d > 0)
            {
                m = m + 1;
            }
            return m;
        }
        #endregion

        #region 月账单生成方法   (Old)
        /// <summary>
        /// 计算两个时间相差的月数
        /// </summary>
        /// <param name="DateTime1">时间1</param>
        /// <param name="DateTime2">时间2</param>
        /// <returns>相差的月数</returns>
        private int DateDiffMonth(DateTime DateTime1, DateTime DateTime2)
        {
            int y = DateTime2.Year - DateTime1.Year; //年差
            int m = y * 12 + DateTime2.Month - DateTime1.Month + 1;
            return m;
        }

        /// <summary>
        /// 返回开始时间
        /// </summary>
        /// <param name="dateTime">时间参数</param>
        /// <returns>开始时间</returns>
        private DateTime ReturnStartDate(DateTime dateTime)
        {
            var minByMonth = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            return minByMonth;
        }

        /// <summary>
        /// 返回结束时间
        /// </summary>
        /// <param name="dateTime">时间参数</param>
        /// <returns>时间</returns>
        private DateTime ReturnEndDate(DateTime dateTime)
        {
            var days = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            var maxByMonth = new DateTime(dateTime.Year, dateTime.Month, days);
            return maxByMonth;
        }
        #endregion
        #endregion

        #endregion

        public class CalculateList
        {
            public CalculateList()
            {
                ReturnDayFixedCost = new List<FixedCost>();
                ReturnMonthFixedCost = new List<FixedCost>();
                Cost = 0;
                FixedDayCost = 0;
                FixedMonthCost = 0;
            }


            /// <summary>
            /// 固定费用价格（日）
            /// </summary>
            public decimal FixedDayCost { get; set; }

            /// <summary>
            ///固定费用价格（月）
            /// </summary>
            public decimal FixedMonthCost { get; set; }

            /// <summary>
            /// 总共价格
            /// </summary>
            public decimal Cost { get; set; }

            public List<FixedCost> ReturnDayFixedCost { get; set; }

            public List<FixedCost> ReturnMonthFixedCost { get; set; }
        }

        public class BillRelated
        {
            public int REGNO { get; set; }
            public long FEENO { get; set; }
            public string ORGID { get; set; }
            public LTC_BILL Bill { get; set; }
            public List<LTC_COSTDTL> CostDtlList { get; set; }
        }

        public BaseResponse<Bill> SaveBill(Bill request)
        {
            return base.Save<LTC_BILL, Bill>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteBill(long id)
        {
            BaseResponse response = new BaseResponse();
            response.ResultCode = 101; //默认状态为失败状态
            response.ResultMessage = "数据库不存在当前数据！";
            try
            {
                if (id < 0)
                {
                    response.ResultCode = 101;
                    response.ResultMessage = "数据库不存在当前数据！";
                }
                else
                {
                    var ss = (from o in unitOfWork.GetRepository<LTC_PAYBILL>().dbSet
                              where o.BILLID == id
                              select 0).Count();
                    if (ss > 0)
                    {
                        response.ResultCode = 102;  //  状态为存在缴费数据
                        response.ResultMessage = "该条数据已存在缴费记录请勿删除！";
                    }
                    else
                    {
                        var bill = unitOfWork.GetRepository<LTC_BILL>().dbSet.OrderByDescending(o => o.ID).FirstOrDefault();

                        if (bill != null)
                        {
                            if (bill.ID == id)
                            {
                                // 1. 更新费用录入的数据
                                UpdateCostDtlData(id);
                                // 2. 更新固定费用录入的信息
                                UpdateFixedInfo(id);
                                //删除账单数据
                                unitOfWork.BeginTransaction();
                                string strSql = String.Format("delete from LTC_BILL where ID='{0}'", id);
                                unitOfWork.GetRepository<LTC_BILL>().ExecuteSqlCommand(strSql);
                                unitOfWork.Save();
                                response.ResultCode = 100;  //删除成功
                                response.ResultMessage = "删除成功！";
                            }
                            else
                            {
                                response.ResultCode = 105;  //删除顺序存在错误
                                response.ResultMessage = "请删除最新一笔的账单数据！";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = 0;
                response.ResultMessage = "删除异常,请联系管理员！";
                throw ex;
            }

            return response;
        }
        #endregion

        #region 删除账单
        /// <summary>
        /// 修改费用录入信息数据
        /// </summary>
        /// <param name="id">账单ID</param>
        public void UpdateCostDtlData(long id)
        {
            var costDtl = unitOfWork.GetRepository<LTC_COSTDTL>().dbSet.Where(m => m.BILLID == id && m.COSTSOURCE == null).ToList();
            if (costDtl != null && costDtl.Count > 0)
            {
                costDtl.ForEach(cost =>
                    {
                        cost.BILLID = 0;
                        unitOfWork.GetRepository<LTC_COSTDTL>().Update(cost);
                        unitOfWork.Save();
                    });
            }
        }

        /// <summary>
        /// 修改固定费用录入信息
        /// </summary>
        /// <param name="id"></param>
        public void UpdateFixedInfo(long id)
        {
            var costDtl = unitOfWork.GetRepository<LTC_COSTDTL>().dbSet.Where(m => m.BILLID == id && m.COSTSOURCE > 0).ToList();
            if (costDtl != null && costDtl.Count > 0)
            {
                var list = GoHeavy(costDtl);
                if (list != null && list.Count > 0)
                {
                    UpdateFixedDtl(list, costDtl);
                    foreach (var item in costDtl)
                    {
                        unitOfWork.BeginTransaction();
                        string strSql = String.Format("delete from LTC_COSTDTL where ID='{0}'", item.ID);
                        unitOfWork.GetRepository<LTC_BILL>().ExecuteSqlCommand(strSql);
                        unitOfWork.Save();
                    }
                }

            }
        }

        public void UpdateFixedDtl(List<int> list, List<LTC_COSTDTL> costDtl)
        {
            foreach (var item in list)
            {
                var fixedInfo = unitOfWork.GetRepository<LTC_FIXEDCOST>().dbSet.Where(m => m.ID == item).FirstOrDefault();
                var costlist = costDtl.Where(o => o.COSTSOURCE == item && o.ITEMTYPE == fixedInfo.PERIOD).ToList();
                var repCount = 0;
                if (costlist != null && costlist.Count > 0)
                {
                    repCount = Convert.ToInt32(costlist.Sum(it => it.QUANTITY));
                }

                fixedInfo.REPEATCOUNT = fixedInfo.REPEATCOUNT - repCount;
                unitOfWork.GetRepository<LTC_FIXEDCOST>().Update(fixedInfo);
                unitOfWork.Save();
            }
        }


        /// <summary>
        /// 费用录入去重
        /// </summary>
        /// <param name="costList"></param>
        /// <returns></returns>
        public List<int> GoHeavy(List<LTC_COSTDTL> costList)
        {
            List<int> list = new List<int>();
            foreach (LTC_COSTDTL item in costList)
            {
                if (item.COSTSOURCE > 0)
                {
                    if (!list.Contains(Convert.ToInt32(item.COSTSOURCE)))
                    {
                        list.Add(Convert.ToInt32(item.COSTSOURCE));
                    }
                }
            }
            return list;
        }


        #endregion

        #region LTC_PINMONEY
        public BaseResponse<IList<PinMoney>> QueryPinMoney(BaseRequest<PinMoneyFilter> request)
        {

            var response = base.Query<LTC_PINMONEY, PinMoney>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        public BaseResponse<PinMoney> GetPinMoney(long Id)
        {
            return base.Get<LTC_PINMONEY, PinMoney>((q) => q.ID == Id);
        }

        public BaseResponse<PinMoney> SavePinMoney(PinMoney request)
        {
            //if (request.Id == 0)
            //{
            //    request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            //}
            return base.Save<LTC_PINMONEY, PinMoney>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeletePinMoney(long Id)
        {
            return base.Delete<LTC_PINMONEY>(Id);
        }
        #endregion

        #region LTC_RECEIPTS
        public BaseResponse<IList<Receipts>> QueryReceipts(BaseRequest<ReceiptsFilter> request)
        {

            var response = base.Query<LTC_RECEIPTS, Receipts>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        public BaseResponse<Receipts> GetReceipts(long Id)
        {
            return base.Get<LTC_RECEIPTS, Receipts>((q) => q.ID == Id);
        }

        public BaseResponse<Receipts> SaveReceipts(Receipts request)
        {
            //if (request.Id == 0)
            //{
            //    request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            //}
            return base.Save<LTC_RECEIPTS, Receipts>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteReceipts(long Id)
        {
            return base.Delete<LTC_RECEIPTS>(Id);
        }
        #endregion

        public BaseResponse<IList<CostItemCostGroup>> QueryCostItemAndCostGroup(BaseRequest<CostItemCostGroupFilter> request)
        {
            var responseItem = base.Query<LTC_COSTITEM, CostItem>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (!string.IsNullOrEmpty(request.Data.No) && !string.IsNullOrEmpty(request.Data.Name))
                {
                    q = q.Where(m => m.COSTITEMNO.Contains(request.Data.No) || m.COSTNAME.Contains(request.Data.Name));
                }
                q = q.OrderBy(m => m.ORGID);
                return q;
            });
            var responseGroup = base.Query<LTC_COSTGROUP, CostGroup>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (!string.IsNullOrEmpty(request.Data.No) && !string.IsNullOrEmpty(request.Data.Name))
                {
                    q = q.Where(m => m.GROUPNO.Contains(request.Data.No) || m.GROUPNAME.Contains(request.Data.Name));
                }
                q = q.OrderBy(m => m.ORGID);
                return q;
            });
            var response = new BaseResponse<IList<CostItemCostGroup>>();
            response.Data = new List<CostItemCostGroup>();
            if (responseGroup.ResultCode == 0)
            {
                foreach (var item in responseGroup.Data)
                {
                    var newItem = new CostItemCostGroup()
                    {
                        Id = item.Id,
                        No = item.GroupNo,
                        Name = item.GroupName,
                        DataType = "Group"
                    };
                    response.Data.Add(newItem);
                }
            }
            if (responseItem.ResultCode == 0)
            {
                foreach (var item in responseItem.Data)
                {
                    var newItem = new CostItemCostGroup()
                    {
                        Id = item.Id,
                        No = item.CostItemNo,
                        Name = item.CostName,
                        DataType = "Item"
                    };
                    response.Data.Add(newItem);
                }
            }
            return response;
        }


        public static string GetEnumDescription(Enum enumSubitem)
        {
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);
            Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                return strValue;
            }
            else
            {
                DescriptionAttribute da = (DescriptionAttribute)objs[0];
                return da.Description;
            }

        }
    }
}
