using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using NPOI.SS.Formula.Functions;
using KM.Common;

namespace KMHC.SLTC.Business.Implement
{
    public class BillV2Service : BaseService, IBillV2Service
    {
        IDictManageService _service = IOCContainer.Instance.Resolve<IDictManageService>();
        #region 账单数据

        /// <summary>
        /// 查询账单列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>账单数据信息</returns>
        public BaseResponse<IList<BillV2>> QueryBillV2(BaseRequest<BillV2Filter> request)
        {
            var response = base.Query<LTC_BILLV2, BillV2>(request, (q) =>
            {
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }

                q = q.Where(m => m.CREATETIME >= request.Data.StarDate && m.CREATETIME <= request.Data.EndDate);

                q = q.Where(m => m.ISDELETE == false && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
                q = q.OrderByDescending(m => m.CREATETIME);
                return q;
            });
            return response;
        }

        #endregion

        #region 拉取使用记录数据

        /// <summary>
        /// 记录数据
        /// </summary>
        /// <param name="feeNo">住民No</param>
        /// <param name="billId">账单Id</param>
        /// <returns>费用记录信息</returns>
        public BaseResponse<BillV2FeeRecord> QueryBillV2FeeRecord(int feeNo, string billId)
        {
            var response = new BaseResponse<BillV2FeeRecord>();
            response.Data = new BillV2FeeRecord();
            response.Data.drugRecordList = new List<FeeRecordBaseInfo>();
            response.Data.materialRecordList = new List<FeeRecordBaseInfo>();
            response.Data.serviceRecordList = new List<FeeRecordBaseInfo>();
            response.Data.residentBalance = new RegNCIInfo();

            try
            {

                Mapper.CreateMap<LTC_REGNCIINFO, RegNCIInfo>();
                var bal = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.FirstOrDefault(m => m.FEENO == feeNo && m.STATUS == 0);
                var balance = Mapper.Map<RegNCIInfo>(bal);

                Mapper.CreateMap<LTC_RESIDENTBALANCE, ResidentBalance>();
                var regbal = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.FirstOrDefault(m => m.FEENO == feeNo && m.STATUS == 0 && m.ISDELETE != true);
                var regbalance = Mapper.Map<ResidentBalance>(regbal);

                if (regbalance.IsHaveNCI)
                {
                    if (balance != null)
                    {
                        response.Data.residentBalance = balance;
                        response.Data.residentBalance.IsHaveNCI = true;
                    }
                    else
                    {
                        response.Data.residentBalance.NCIPaylevel = 0;
                        response.Data.residentBalance.NCIPayscale = 0;
                        response.Data.residentBalance.IsHaveNCI = true;
                    }
                }
                else
                {
                    response.Data.residentBalance.NCIPaylevel = 0;
                    response.Data.residentBalance.NCIPayscale = 0;
                    response.Data.residentBalance.IsHaveNCI = false;
                }

                var feeRecordList =
                    unitOfWork.GetRepository<LTC_FEERECORD>()
                        .dbSet.Where(m => m.FEENO == feeNo && m.ISDELETE == false && m.BILLID == billId)
                        .ToList();
                if (feeRecordList != null && feeRecordList.Count > 0)
                {
                    foreach (var item in feeRecordList)
                    {
                        #region 药品数据

                        if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Drug))
                        {
                            var drugRecord =
                                unitOfWork.GetRepository<LTC_DRUGRECORD>()
                                    .dbSet.FirstOrDefault(m => m.DRUGRECORDID == item.CHARGERECORDID);
                            var drugModel = new FeeRecordBaseInfo();
                            drugModel.BillId = item.BILLID;
                            drugModel.FeeRecordID = item.FEERECORDID;
                            drugModel.ChargeRecordType = item.CHARGERECORDTYPE;
                            drugModel.ChargeRecordID = item.CHARGERECORDID;
                            drugModel.FeeNo = item.FEENO;
                            drugModel.UnitPrice = item.UNITPRICE;
                            drugModel.Count = item.COUNT;
                            drugModel.Cost = item.COST;
                            drugModel.IsNCIItem = item.ISNCIITEM;
                            drugModel.NSID = drugRecord.NSID;
                            drugModel.ProjectID = drugRecord.DRUGID;
                            drugModel.ProjectName = drugRecord.CNNAME;
                            drugModel.TakeTime = Convert.ToDateTime(drugRecord.TAKETIME);

                            response.Data.drugRecordList.Add(drugModel);
                        }

                        #endregion

                        #region 耗材数据

                        if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Material))
                        {
                            var materialRecord =
                                unitOfWork.GetRepository<LTC_MATERIALRECORD>()
                                    .dbSet.FirstOrDefault(m => m.MATERIALRECORDID == item.CHARGERECORDID);
                            var materialModel = new FeeRecordBaseInfo();

                            materialModel.BillId = item.BILLID;
                            materialModel.FeeRecordID = item.FEERECORDID;
                            materialModel.ChargeRecordType = item.CHARGERECORDTYPE;
                            materialModel.ChargeRecordID = item.CHARGERECORDID;
                            materialModel.FeeNo = item.FEENO;
                            materialModel.UnitPrice = item.UNITPRICE;
                            materialModel.Count = item.COUNT;
                            materialModel.Cost = item.COST;
                            materialModel.IsNCIItem = item.ISNCIITEM;
                            materialModel.NSID = materialRecord.NSID;
                            materialModel.ProjectID = materialRecord.MATERIALID;
                            materialModel.ProjectName = materialRecord.MATERIALNAME;
                            materialModel.TakeTime = Convert.ToDateTime(materialRecord.TAKETIME);
                            response.Data.materialRecordList.Add(materialModel);
                        }

                        #endregion

                        #region 服务数据

                        if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Service))
                        {
                            var serviceRecord =
                                unitOfWork.GetRepository<LTC_SERVICERECORD>()
                                    .dbSet.FirstOrDefault(m => m.SERVICERECORDID == item.CHARGERECORDID);
                            var serviceModel = new FeeRecordBaseInfo();

                            serviceModel.BillId = item.BILLID;
                            serviceModel.FeeRecordID = item.FEERECORDID;
                            serviceModel.ChargeRecordType = item.CHARGERECORDTYPE;
                            serviceModel.ChargeRecordID = item.CHARGERECORDID;
                            serviceModel.FeeNo = item.FEENO;
                            serviceModel.UnitPrice = item.UNITPRICE;
                            serviceModel.Count = item.COUNT;
                            serviceModel.Cost = item.COST;
                            serviceModel.IsNCIItem = item.ISNCIITEM;
                            serviceModel.NSID = serviceRecord.NSID;
                            serviceModel.ProjectID = serviceRecord.SERVICEID;
                            serviceModel.ProjectName = serviceRecord.SERVICENAME;
                            serviceModel.TakeTime = Convert.ToDateTime(serviceRecord.TAKETIME);

                            response.Data.serviceRecordList.Add(serviceModel);
                        }

                        #endregion
                    }
                }
                else
                {
                    response.ResultCode = 101; //未查询到有效的费用记录;
                    response.ResultMessage = "未查询到有效的费用记录数据";
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

        /// <summary>
        /// 费用数据
        /// </summary>
        /// <param name="feeNo">住民No</param>
        /// <param name="startDate">核算开始时间</param>
        /// <param name="endDate">核算结束时间</param>
        /// <returns>费用清单信息</returns>
        public BaseResponse<BillV2FeeList> QueryBillV2FeeList(long feeNo, DateTime startDate, DateTime endDate)
        {
            var response = new BaseResponse<BillV2FeeList>();
            response.Data = new BillV2FeeList();
            response.Data.regInformation = new List<RegInfo>();
            response.Data.feeRecordList = new List<FeeRecordBaseInfo>();

            try
            {
                double inHosDays = 0;
                int inHosCount = 0;
                double loshours = 0;

                var inHosregNo = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.FirstOrDefault(m => m.FEENO == feeNo);
                if (inHosregNo != null)
                {
                    var inHos = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.REGNO == inHosregNo.REGNO).ToList();
                    DateTime comparSDate = startDate;
                    DateTime comparEDate = endDate;

                    foreach (var hos in inHos)
                    {
                        if (hos.INDATE > comparEDate)
                        {
                            continue;
                        }
                        if (hos.OUTDATE != null && hos.OUTDATE < comparSDate)
                        {
                            continue;
                        }
                        ComputeInHosDay(hos, comparSDate, comparEDate, ref inHosDays, ref loshours);
                        inHosCount++;
                    }
                }
                //住民的基本信息
                var regInfo = (from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == feeNo)
                               join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO
                               join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { FLOOR = ipd.FLOOR, OrgID = ipd.ORGID } equals new { FLOOR = f.FLOORID, OrgID = f.ORGID } into fs
                               join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on new { ROOMNO = ipd.ROOMNO, OrgID = ipd.ORGID } equals new { ROOMNO = r.ROOMNO, OrgID = r.ORGID } into rs
                               from floor in fs.DefaultIfEmpty()
                               from room in rs.DefaultIfEmpty()
                               select new RegInfo
                               {
                                   FeeNO = ipd.FEENO,
                                   RegNO = reg.REGNO,
                                   Name = reg.NAME,
                                   Gender = reg.SEX,
                                   Birthday = reg.BRITHDATE,
                                   STime = startDate,
                                   ETime = endDate,
                                   InHosDays = inHosDays,
                                   ResidentNo = reg.RESIDENGNO,
                                   InHosCount = inHosCount,
                                   Floor = floor.FLOORNAME,
                                   RoomNo = room.ROOMNAME,
                                   BedNo = ipd.BEDNO
                               }).ToList();
                response.Data.regInformation = regInfo;



                string stringSDate = startDate.ToString("yyyy-MM");
                string stringEDate = endDate.ToString("yyyy-MM");
                var getregNo = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.FirstOrDefault(m => m.FEENO == feeNo);
                string regNofeeNO = "";
                if (getregNo != null)
                {
                    var getfeeNO = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.REGNO == getregNo.REGNO).ToList();
                    if (getfeeNO != null && getfeeNO.Count > 0)
                    {
                        foreach (var item in getfeeNO)
                        {
                            regNofeeNO += item.FEENO.ToString() + ",";
                        }
                        regNofeeNO = regNofeeNO.TrimEnd(',');
                    }
                }
                var allFeeNos = unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(q => q.BILLMONTH != null && q.BILLMONTH != "" && q.BILLMONTH.CompareTo(stringSDate) >= 0 && q.BILLMONTH.CompareTo(stringEDate) <= 0)
                    .Select(s => new { FEENO = s.FEENO }).Distinct().ToList().OrderByDescending(s => s.FEENO);

                bool flag = false;
                foreach (var fee in allFeeNos)
                {
                    string[] filterFeeNo = regNofeeNO.Split(',');
                    for (int i = 0; i < filterFeeNo.Length; i++)
                    {
                        if (filterFeeNo[i] == fee.FEENO.ToString())
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            continue;
                        }
                    }
                    if (!flag)
                    {
                        continue;
                    }
                    flag = false;

                    var feeRecordList = (from fr in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                            .Where(m => m.FEENO == fee.FEENO && m.ISDELETE != true && m.ISREFUNDRECORD != true).OrderByDescending(m => m.CREATETIME)
                                         join bv2 in unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(m => m.STATUS != (int)BillStatus.Refund && m.BILLMONTH.CompareTo(stringSDate) >= 0 && m.BILLMONTH.CompareTo(stringEDate) <= 0)
                                         on fr.BILLID equals bv2.BILLID
                                         select new
                                         {
                                             CHARGERECORDID = fr.CHARGERECORDID,
                                             CHARGERECORDTYPE = fr.CHARGERECORDTYPE,
                                             UNITPRICE = fr.UNITPRICE,
                                             COUNT = fr.COUNT,
                                             COST = fr.COST
                                         }).ToList();
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        foreach (var item in feeRecordList)
                        {
                            #region 药品数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Drug))
                            {
                                var drugRecord = from drrec in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.DRUGRECORDID == item.CHARGERECORDID)
                                                 join nsdr in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on drrec.DRUGID equals nsdr.DRUGID
                                                 select new FeeRecordBaseInfo
                                                 {
                                                     MCCode = nsdr.MCDRUGCODE,          //项目编码
                                                     ProjectName = nsdr.CNNAME,         //项目
                                                     UnitPrice = item.UNITPRICE,        //单价
                                                     Units = nsdr.UNITS,                //单位
                                                     Spec = nsdr.SPEC,                  //规格
                                                     Count = item.COUNT,                //数量
                                                     Cost = item.COST                   //金额
                                                 };
                                response.Data.feeRecordList.Add(drugRecord.ToList()[0]);
                            }

                            #endregion

                            #region 耗材数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Material))
                            {
                                var materialRecord = from mr in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.MATERIALRECORDID == item.CHARGERECORDID)
                                                     join nsm in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on mr.MATERIALID equals nsm.MATERIALID
                                                     select new FeeRecordBaseInfo
                                                     {
                                                         MCCode = nsm.MCMATERIALCODE,          //项目编码
                                                         ProjectName = nsm.MATERIALNAME,       //项目
                                                         UnitPrice = item.UNITPRICE,           //单价
                                                         Units = nsm.UNITS,                    //单位
                                                         Spec = nsm.SPEC,                      //规格
                                                         Count = item.COUNT,                   //数量
                                                         Cost = item.COST                      //金额
                                                     };
                                response.Data.feeRecordList.Add(materialRecord.ToList()[0]);
                            }

                            #endregion

                            #region 服务数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Service))
                            {
                                var serviceRecord = from ser in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.SERVICERECORDID == item.CHARGERECORDID)
                                                    join nss in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on ser.SERVICEID equals nss.SERVICEID
                                                    select new FeeRecordBaseInfo
                                                    {
                                                        MCCode = nss.MCSERVICECODE,          //项目编码
                                                        ProjectName = nss.SERVICENAME,       //项目
                                                        UnitPrice = item.UNITPRICE,           //单价
                                                        Units = nss.UNITS,                    //单位
                                                        Spec = "",                            //规格
                                                        Count = item.COUNT,                   //数量
                                                        Cost = item.COST                      //金额

                                                    };

                                response.Data.feeRecordList.Add(serviceRecord.ToList()[0]);
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 在院时间计算
        /// </summary>
        public void ComputeInHosDay(KMHC.SLTC.Persistence.LTC_IPDREG hos, DateTime comparSDate, DateTime comparEDate, ref double inHosDays, ref double loshours)
        {
            comparSDate = DictHelper.GetFeeIntervalStartDateByDate(comparSDate);
            comparEDate = DictHelper.GetFeeIntervalEndDateByDate(comparEDate);
            if (hos.OUTDATE != null)
            {
                if (hos.OUTDATE <= comparEDate)
                {
                    if (hos.INDATE >= comparSDate)
                    {
                        TimeSpan sp = (TimeSpan)(hos.OUTDATE - hos.INDATE);
                        inHosDays += sp.Days;
                    }
                    else
                    {
                        TimeSpan sp = (TimeSpan)(hos.OUTDATE - comparSDate);
                        inHosDays += sp.Days;
                    }

                }
                else
                {
                    if (comparEDate <= DateTime.Now.Date)
                    {
                        if (hos.INDATE >= comparSDate)
                        {
                            TimeSpan sp = (TimeSpan)(comparEDate - hos.INDATE);
                            inHosDays += sp.Days + 1; ;
                        }
                        else
                        {
                            TimeSpan sp = (TimeSpan)(comparEDate - comparSDate);
                            inHosDays += sp.Days + 1; ;
                        }
                    }
                    else
                    {
                        if (hos.INDATE >= comparSDate)
                        {
                            TimeSpan sp = (TimeSpan)(DateTime.Now.Date - hos.INDATE);
                            inHosDays += sp.Days + 1; ;
                        }
                        else
                        {
                            TimeSpan sp = (TimeSpan)(DateTime.Now.Date - comparSDate);
                            inHosDays += sp.Days + 1; ;
                        }
                    }

                }

            }
            else
            {
                if (comparEDate <= DateTime.Now.Date)
                {
                    if (hos.INDATE >= comparSDate)
                    {
                        TimeSpan sp = (TimeSpan)(comparEDate - hos.INDATE);
                        inHosDays += sp.Days + 1;
                    }
                    else
                    {
                        TimeSpan sp = (TimeSpan)(comparEDate - comparSDate);
                        inHosDays += sp.Days + 1;
                    }

                }
                else
                {
                    if (hos.INDATE >= comparSDate)
                    {
                        TimeSpan sp = (TimeSpan)(DateTime.Now.Date - hos.INDATE);
                        inHosDays += sp.Days + 1;
                    }
                    else
                    {
                        TimeSpan sp = (TimeSpan)(DateTime.Now.Date - comparSDate);
                        inHosDays += sp.Days + 1;
                    }
                }

            }
            //请假时间计算
            var leaveHos = unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet.Where(m => m.FEENO == hos.FEENO).ToList();
            DateTime leahosEDate = comparEDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            foreach (var los in leaveHos)
            {
                loshours = 0;
                if (los.STARTDATE > leahosEDate)
                {
                    continue;
                }
                if (los.RETURNDATE < comparSDate)
                {
                    continue;
                }
                if (los.RETURNDATE <= leahosEDate)
                {
                    if (los.STARTDATE >= comparSDate)
                    {
                        TimeSpan sp = (TimeSpan)(los.RETURNDATE - los.STARTDATE);
                        loshours += sp.TotalHours;
                    }
                    else
                    {
                        TimeSpan sp = (TimeSpan)(los.RETURNDATE - comparSDate);
                        loshours += sp.TotalHours;
                    }
                }
                else
                {
                    if (leahosEDate <= DateTime.Now.Date)
                    {
                        if (los.STARTDATE >= comparSDate)
                        {
                            TimeSpan sp = (TimeSpan)(leahosEDate - los.STARTDATE);
                            loshours += sp.TotalHours;
                        }
                        else
                        {
                            TimeSpan sp = (TimeSpan)(leahosEDate - comparSDate);
                            loshours += sp.TotalHours;
                        }
                    }
                    else
                    {
                        if (los.STARTDATE >= comparSDate)
                        {
                            TimeSpan sp = (TimeSpan)(DateTime.Now.Date - los.STARTDATE);
                            loshours += sp.TotalHours;
                        }
                        else
                        {
                            TimeSpan sp = (TimeSpan)(DateTime.Now.Date - comparSDate);
                            loshours += sp.TotalHours;
                        }
                    }
                }
                if (loshours % 24 >= 6)
                {
                    inHosDays -= Math.Floor(loshours / 24) + 1;
                }
                else
                {
                    inHosDays -= Math.Floor(loshours / 24);
                }
            }

        }

        /// <summary>
        /// 费用数据
        /// </summary>
        /// <param name="feeNo">住民No</param>
        /// <param name="startDate">核算开始时间</param>
        /// <param name="endDate">核算结束时间</param>
        /// <returns>费用清单信息</returns>
        public List<BillV2FeeList> QueryBillV2FeeListReport(int feeNo, DateTime startDate, DateTime endDate)
        {
            var response = new List<BillV2FeeList>();

            try
            {
                string stringSDate = startDate.ToString("yyyy-MM");
                string stringEDate = endDate.ToString("yyyy-MM");
                var getregNo = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.FirstOrDefault(m => m.FEENO == feeNo);//根据FeeNO去获取对应的RegNO
                string regNofeeNO = "";
                if (getregNo != null)
                {
                    //根据RegNO找出这个住民所有的FeeNO
                    var getfeeNO = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.REGNO == getregNo.REGNO).ToList();
                    if (getfeeNO != null && getfeeNO.Count > 0)
                    {
                        foreach (var item in getfeeNO)
                        {
                            regNofeeNO += item.FEENO.ToString() + ",";
                        }
                        regNofeeNO = regNofeeNO.TrimEnd(',');
                    }
                }
                //根据查询的区间筛选出在此区间内生成的账单对应的feeNo
                var allFeeNos = unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(q => q.BILLMONTH != null && q.BILLMONTH != "" && q.BILLMONTH.CompareTo(stringSDate) >= 0 && q.BILLMONTH.CompareTo(stringEDate) <= 0)
                    .Select(s => new { FEENO = s.FEENO }).Distinct().ToList().OrderByDescending(s => s.FEENO);
                bool flag = false;
                foreach (var fee in allFeeNos)
                {
                    var res = new BillV2FeeList();
                    res.regInformation = new List<RegInfo>();
                    res.feeRecordList = new List<FeeRecordBaseInfo>();
                    //筛选出符合这个住民在查询期间所有的feeNo
                    string[] filterFeeNo = regNofeeNO.Split(',');
                    for (int i = 0; i < filterFeeNo.Length; i++)
                    {
                        if (filterFeeNo[i] == fee.FEENO.ToString())
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            continue;
                        }
                    }
                    if (!flag)
                    {
                        continue;
                    }
                    flag = false;
                    //筛选出符合这个住民在查询期间所有的feeNo
                    double inHosDays = 0;
                    int inHosCount = 0;
                    double loshours = 0;

                    var inHosregNo = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.FirstOrDefault(m => m.FEENO == feeNo);
                    if (inHosregNo != null)
                    {
                        var inHos = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.REGNO == inHosregNo.REGNO).ToList();
                        DateTime comparSDate = startDate;
                        DateTime comparEDate = endDate;
                        foreach (var hos in inHos)
                        {
                            if (hos.INDATE > comparEDate)
                            {
                                continue;
                            }
                            if (hos.OUTDATE != null && hos.OUTDATE < comparSDate)
                            {
                                continue;
                            }
                            ComputeInHosDay(hos, comparSDate, comparEDate, ref inHosDays, ref loshours);
                            inHosCount++;
                        }
                    }

                    var regInfo = (from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == fee.FEENO)
                                   join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO
                                   join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { FLOOR = ipd.FLOOR, OrgID = ipd.ORGID } equals new { FLOOR = f.FLOORID, OrgID = f.ORGID } into fs
                                   join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on new { ROOMNO = ipd.ROOMNO, OrgID = ipd.ORGID } equals new { ROOMNO = r.ROOMNO, OrgID = r.ORGID } into rs
                                   from floor in fs.DefaultIfEmpty()
                                   from room in rs.DefaultIfEmpty()
                                   select new RegInfo
                                   {
                                       FeeNO = ipd.FEENO,
                                       RegNO = reg.REGNO,
                                       Name = reg.NAME,
                                       Gender = reg.SEX,
                                       Birthday = reg.BRITHDATE,
                                       STime = startDate,
                                       ETime = endDate,
                                       InHosDays = inHosDays,
                                       ResidentNo = reg.RESIDENGNO,
                                       InHosCount = inHosCount,
                                       Floor = floor.FLOORNAME,
                                       RoomNo = room.ROOMNAME,
                                       BedNo = ipd.BEDNO
                                   }).ToList();
                    res.regInformation = regInfo;

                    var feeRecordList = (from fr in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                            .Where(m => m.FEENO == fee.FEENO && m.ISDELETE != true && m.ISREFUNDRECORD != true).OrderByDescending(m => m.CREATETIME)
                                         join bv2 in unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(m => m.STATUS != (int)BillStatus.Refund && m.BILLMONTH.CompareTo(stringSDate) >= 0 && m.BILLMONTH.CompareTo(stringEDate) <= 0)
                                         on fr.BILLID equals bv2.BILLID
                                         select new
                                         {
                                             CHARGERECORDID = fr.CHARGERECORDID,
                                             CHARGERECORDTYPE = fr.CHARGERECORDTYPE,
                                             UNITPRICE = fr.UNITPRICE,
                                             COUNT = fr.COUNT,
                                             COST = fr.COST
                                         }).ToList();
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        foreach (var item in feeRecordList)
                        {
                            #region 药品数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Drug))
                            {
                                var drugRecord = from drrec in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.DRUGRECORDID == item.CHARGERECORDID)
                                                 join nsdr in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on drrec.DRUGID equals nsdr.DRUGID
                                                 select new FeeRecordBaseInfo
                                                 {
                                                     MCCode = nsdr.MCDRUGCODE,          //项目编码
                                                     ProjectName = nsdr.CNNAME,         //项目
                                                     UnitPrice = item.UNITPRICE,        //单价
                                                     Units = nsdr.UNITS,                //单位
                                                     Spec = nsdr.SPEC,                  //规格
                                                     Count = item.COUNT,                //数量
                                                     Cost = item.COST                   //金额
                                                 };
                                res.feeRecordList.Add(drugRecord.ToList()[0]);
                            }

                            #endregion

                            #region 耗材数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Material))
                            {
                                var materialRecord = from mr in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.MATERIALRECORDID == item.CHARGERECORDID)
                                                     join nsm in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on mr.MATERIALID equals nsm.MATERIALID
                                                     select new FeeRecordBaseInfo
                                                     {
                                                         MCCode = nsm.MCMATERIALCODE,          //项目编码
                                                         ProjectName = nsm.MATERIALNAME,       //项目
                                                         UnitPrice = item.UNITPRICE,           //单价
                                                         Units = nsm.UNITS,                    //单位
                                                         Spec = nsm.SPEC,                      //规格
                                                         Count = item.COUNT,                   //数量
                                                         Cost = item.COST                      //金额
                                                     };
                                res.feeRecordList.Add(materialRecord.ToList()[0]);
                            }

                            #endregion

                            #region 服务数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Service))
                            {
                                var serviceRecord = from ser in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.SERVICERECORDID == item.CHARGERECORDID)
                                                    join nss in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on ser.SERVICEID equals nss.SERVICEID
                                                    select new FeeRecordBaseInfo
                                                    {
                                                        MCCode = nss.MCSERVICECODE,          //项目编码
                                                        ProjectName = nss.SERVICENAME,       //项目
                                                        UnitPrice = item.UNITPRICE,           //单价
                                                        Units = nss.UNITS,                    //单位
                                                        Spec = "",                            //规格
                                                        Count = item.COUNT,                   //数量
                                                        Cost = item.COST                      //金额

                                                    };

                                res.feeRecordList.Add(serviceRecord.ToList()[0]);
                            }

                            #endregion
                        }
                    }
                    response.Add(res);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public bool IsHaveNCIMethod(long feeNo)
        {
            Mapper.CreateMap<LTC_RESIDENTBALANCE, ResidentBalance>();
            var regbal = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.FirstOrDefault(m => m.FEENO == feeNo && m.STATUS == 0 && m.ISDELETE != true);
            var regbalance = Mapper.Map<ResidentBalance>(regbal);
            if (regbalance.IsHaveNCI)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<BillV2FeeList> QueryBillV2AllFeeListReport(DateTime startDate, DateTime endDate)
        {
            var response = new List<BillV2FeeList>();

            try
            {
                string stringSDate = startDate.ToString("yyyy-MM");
                string stringEDate = endDate.ToString("yyyy-MM");
                var allFeeNos = unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(q => q.BILLMONTH != null && q.BILLMONTH != "" && q.BILLMONTH.CompareTo(stringSDate) >= 0 && q.BILLMONTH.CompareTo(stringEDate) <= 0 && q.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
                    .Select(s => new { FEENO = s.FEENO }).Distinct().ToList().OrderByDescending(s => s.FEENO);
                foreach (var fee in allFeeNos)
                {
                    var res = new BillV2FeeList();
                    res.regInformation = new List<RegInfo>();
                    res.feeRecordList = new List<FeeRecordBaseInfo>();
                    //判断是否有护理险资格
                    var regbalance = new ResidentBalance();
                    if (!IsHaveNCIMethod(fee.FEENO))
                    {
                        continue;
                    }

                    double inHosDays = 0;
                    int inHosCount = 0;
                    double loshours = 0;

                    var inHosregNo = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.FirstOrDefault(m => m.FEENO == fee.FEENO);
                    if (inHosregNo != null)
                    {
                        var inHos = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.REGNO == inHosregNo.REGNO).ToList();
                        DateTime comparSDate = startDate;
                        DateTime comparEDate = endDate;
                        foreach (var hos in inHos)
                        {
                            if (!IsHaveNCIMethod(hos.FEENO))
                            {
                                continue;
                            }
                            if (hos.INDATE > comparEDate)
                            {
                                continue;
                            }
                            if (hos.OUTDATE != null && hos.OUTDATE < comparSDate)
                            {
                                continue;
                            }
                            ComputeInHosDay(hos, comparSDate, comparEDate, ref inHosDays, ref loshours);
                            inHosCount++;
                        }
                    }

                    var regInfo = (from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == fee.FEENO)
                                   join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO
                                   join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { FLOOR = ipd.FLOOR, OrgID = ipd.ORGID } equals new { FLOOR = f.FLOORID, OrgID = f.ORGID } into fs
                                   join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on new { ROOMNO = ipd.ROOMNO, OrgID = ipd.ORGID } equals new { ROOMNO = r.ROOMNO, OrgID = r.ORGID } into rs
                                   from floor in fs.DefaultIfEmpty()
                                   from room in rs.DefaultIfEmpty()
                                   select new RegInfo
                                   {
                                       FeeNO = ipd.FEENO,
                                       RegNO = reg.REGNO,
                                       Name = reg.NAME,
                                       Gender = reg.SEX,
                                       Birthday = reg.BRITHDATE,
                                       STime = startDate,
                                       ETime = endDate,
                                       InHosDays = inHosDays,
                                       ResidentNo = reg.RESIDENGNO,
                                       InHosCount = inHosCount,
                                       Floor = floor.FLOORNAME,
                                       RoomNo = room.ROOMNAME,
                                       BedNo = ipd.BEDNO
                                   }).ToList();
                    res.regInformation = regInfo;

                    var feeRecordList = (from fr in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                            .Where(m => m.FEENO == fee.FEENO && m.ISDELETE != true && m.ISREFUNDRECORD != true).OrderByDescending(m => m.CREATETIME)
                                         join bv2 in unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(m => m.STATUS != (int)BillStatus.Refund && m.BILLMONTH.CompareTo(stringSDate) >= 0 && m.BILLMONTH.CompareTo(stringEDate) <= 0)
                                         on fr.BILLID equals bv2.BILLID
                                         select new
                                         {
                                             CHARGERECORDID = fr.CHARGERECORDID,
                                             CHARGERECORDTYPE = fr.CHARGERECORDTYPE,
                                             UNITPRICE = fr.UNITPRICE,
                                             COUNT = fr.COUNT,
                                             COST = fr.COST
                                         }).ToList();
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        foreach (var item in feeRecordList)
                        {
                            #region 药品数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Drug))
                            {
                                var drugRecord = from drrec in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.DRUGRECORDID == item.CHARGERECORDID)
                                                 join nsdr in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on drrec.DRUGID equals nsdr.DRUGID
                                                 select new FeeRecordBaseInfo
                                                 {
                                                     MCCode = nsdr.MCDRUGCODE,          //项目编码
                                                     ProjectName = nsdr.CNNAME,         //项目
                                                     UnitPrice = item.UNITPRICE,        //单价
                                                     Units = nsdr.UNITS,                //单位
                                                     Spec = nsdr.SPEC,                  //规格
                                                     Count = item.COUNT,                //数量
                                                     Cost = item.COST                   //金额
                                                 };
                                res.feeRecordList.Add(drugRecord.ToList()[0]);
                            }

                            #endregion

                            #region 耗材数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Material))
                            {
                                var materialRecord = from mr in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.MATERIALRECORDID == item.CHARGERECORDID)
                                                     join nsm in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on mr.MATERIALID equals nsm.MATERIALID
                                                     select new FeeRecordBaseInfo
                                                     {
                                                         MCCode = nsm.MCMATERIALCODE,          //项目编码
                                                         ProjectName = nsm.MATERIALNAME,       //项目
                                                         UnitPrice = item.UNITPRICE,           //单价
                                                         Units = nsm.UNITS,                    //单位
                                                         Spec = nsm.SPEC,                      //规格
                                                         Count = item.COUNT,                   //数量
                                                         Cost = item.COST                      //金额
                                                     };
                                res.feeRecordList.Add(materialRecord.ToList()[0]);
                            }

                            #endregion

                            #region 服务数据

                            if (item.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Service))
                            {
                                var serviceRecord = from ser in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.SERVICERECORDID == item.CHARGERECORDID)
                                                    join nss in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on ser.SERVICEID equals nss.SERVICEID
                                                    select new FeeRecordBaseInfo
                                                    {
                                                        MCCode = nss.MCSERVICECODE,          //项目编码
                                                        ProjectName = nss.SERVICENAME,       //项目
                                                        UnitPrice = item.UNITPRICE,           //单价
                                                        Units = nss.UNITS,                    //单位
                                                        Spec = "",                            //规格
                                                        Count = item.COUNT,                   //数量
                                                        Cost = item.COST                      //金额

                                                    };

                                res.feeRecordList.Add(serviceRecord.ToList()[0]);
                            }

                            #endregion
                        }
                    }
                    response.Add(res);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }



        #region 删除账单数据

        public BaseResponse<BillV2> DeleteBillInfo(string billNo)
        {
            var response = new BaseResponse<BillV2>();
            try
            {
                // 查询账单信息，删除相关联的数据
                Mapper.CreateMap<LTC_BILLV2, BillV2>();
                var model =
                    unitOfWork.GetRepository<LTC_BILLV2>()
                        .dbSet.Where(x => x.BILLID == billNo && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
                        .FirstOrDefault();
                var billInfo = Mapper.Map<BillV2>(model);
                if (billInfo != null)
                {
                    //更新账单表数据
                    billInfo.IsDelete = true;
                    base.Save<LTC_BILLV2, BillV2>(billInfo, (q) => q.BILLID == billNo);

                    //更新费用记录状态
                    Mapper.CreateMap<LTC_FEERECORD, FeeRecordModel>();
                    var feeList = unitOfWork.GetRepository<LTC_FEERECORD>().dbSet.Where(x => x.BILLID == billNo);
                    var feeRecordList = Mapper.Map<List<FeeRecordModel>>(feeList.ToList());
                    if (feeRecordList != null && feeRecordList.Count > 0)
                    {
                        foreach (var item in feeRecordList)
                        {
                            item.IsDelete = true;
                            base.Save<LTC_FEERECORD, FeeRecordModel>(item, (q) => q.FEERECORDID == item.FeeRecordID);
                        }

                    }

                    var ncideduModel = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(m => m.BILLID == billNo).ToList();
                    Mapper.CreateMap<LTC_NCIDEDUCTION, NCIDeductionModel>();
                    var edductionList = Mapper.Map<List<NCIDeductionModel>>(ncideduModel);
                    if (edductionList != null && edductionList.Count > 0)
                    {
                        foreach (var item in edductionList)
                        {
                            var dedu =
                                    unitOfWork.GetRepository<LTC_NCIDEDUCTION>()
                                        .dbSet.FirstOrDefault(m => m.ID == item.ID);
                            dedu.BILLID = null;
                            dedu.AMOUNT = 0;
                            dedu.DEDUCTIONREASON = "";
                            dedu.DEDUCTIONSTATUS = (int)DeductionStatus.UnCharge;
                            dedu.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                            dedu.UPDATETIME = DateTime.Now;
                            unitOfWork.GetRepository<LTC_NCIDEDUCTION>().Update(dedu);
                        }
                    }

                    //更新使用记录
                    var feeRecs =
                        unitOfWork.GetRepository<LTC_FEERECORD>().dbSet.Where(m => m.BILLID == billNo).ToList();
                    if (feeRecs != null)
                    {
                        foreach (var feeRec in feeRecs)
                        {
                            if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Drug))
                            {
                                var drug =
                                    unitOfWork.GetRepository<LTC_DRUGRECORD>()
                                        .dbSet.FirstOrDefault(m => m.DRUGRECORDID == feeRec.CHARGERECORDID);
                                drug.STATUS = Convert.ToInt32(RecordStatus.Create);
                                drug.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                drug.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(drug);
                            }
                            else if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Material))
                            {
                                var material =
                                    unitOfWork.GetRepository<LTC_MATERIALRECORD>()
                                        .dbSet.FirstOrDefault(m => m.MATERIALRECORDID == feeRec.CHARGERECORDID);
                                material.STATUS = Convert.ToInt32(RecordStatus.Create);
                                material.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                material.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(material);
                            }
                            else if (feeRec.CHARGERECORDTYPE == Convert.ToInt32(ChargeItemType.Service))
                            {
                                var service =
                                    unitOfWork.GetRepository<LTC_SERVICERECORD>()
                                        .dbSet.FirstOrDefault(m => m.SERVICERECORDID == feeRec.CHARGERECORDID);
                                service.STATUS = Convert.ToInt32(RecordStatus.Create);
                                service.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                service.UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(service);
                            }
                        }
                    }

                    #region 修改额度信息
                    var limitdate = new DateTime(billInfo.BalanceStartTime.Value.Year, billInfo.BalanceStartTime.Value.Month, billInfo.BalanceStartTime.Value.Day);
                    var nciFinancialMonth = _service.GetFeeIntervalByDate(limitdate);
                    var monthstr = Convert.ToInt16(nciFinancialMonth.Month) > 9 ? billInfo.BalanceStartTime.Value.Year + "-" + nciFinancialMonth.Month : billInfo.BalanceStartTime.Value.Year + "-0" + nciFinancialMonth.Month;
                    Mapper.CreateMap<LTC_MONTHLYPAYBILLRECORD, MonthlyPayBillRecord>();
                    var record =
                            unitOfWork.GetRepository<LTC_MONTHLYPAYBILLRECORD>()
                                .dbSet.Where(
                                    m =>
                                        m.FEENO == billInfo.FeeNo && m.BILLID == billNo &&
                                        m.ISDELETE == false);
                    var payLimitList = Mapper.Map<List<MonthlyPayBillRecord>>(record.ToList());
                    if (payLimitList != null && payLimitList.Count > 0)
                    {
                        foreach (var billRecord in payLimitList)
                        {
                            Mapper.CreateMap<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>();
                            var limit =
                                unitOfWork.GetRepository<LTC_MONTHLYPAYLIMIT>()
                                    .dbSet.FirstOrDefault(
                                        m =>
                                            m.FEENO == billInfo.FeeNo && m.YEARMONTH == monthstr &&
                                            m.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
                            var monthlyRecord = Mapper.Map<MonthlyPayLimit>(limit);
                            monthlyRecord.PayedAmount = monthlyRecord.PayedAmount -
                                                        Convert.ToDecimal(billRecord.PayedAmount);
                            base.Save<LTC_MONTHLYPAYLIMIT, MonthlyPayLimit>(monthlyRecord,
                                (q) => q.PAYLIMITID == monthlyRecord.PayLimitID);

                            billRecord.IsDelete = true;
                            base.Save<LTC_MONTHLYPAYBILLRECORD, MonthlyPayBillRecord>(billRecord,
                                (q) => q.ID == billRecord.ID);
                        }
                    }

                    #endregion
                }
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = ex.Message;
            }

            return response;
        }

        #endregion


        #region 获取时间范围

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

        public object GetYearMonth()
        {
            var dtEnd = _service.GetFeeIntervalEndDateByDate(DateTime.Now).ToString("yyyy-MM");
            //string dtEnd =DateTime.Now.Year+"-"+ _service.GetFeeIntervalByDate(DateTime.Now).Month.PadLeft(2,'0');
            return unitOfWork.GetRepository<LTC_BILLV2>()
                   .dbSet.Where(w => w.STATUS == (int)BillStatus.Charge && w.ORGID == SecurityHelper.CurrentPrincipal.OrgId
                   && w.NCIPAYSCALE > 0 && string.Compare(w.BILLMONTH, dtEnd) < 0).Select(s => s.BILLMONTH).Distinct();
        }

        public object GetMonthData(string month)
        {
            var billList = GetBillsByMonth(month);
            var billIdList = billList.Select(s => s.BILLID);
            var rsMonthFee = GetUploadDataAllResident(billList, month);
            //var deAmount = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
            //    string.Compare(w.DEBITMONTH, month) <= 0 && w.DEDUCTIONSTATUS == 0).Sum(s => (double?)s.AMOUNT);
            //var deDays = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
            //    string.Compare(w.DEBITMONTH, month) <= 0 && billIdList.Contains(w.BILLID)).Sum(s => (int?)s.DEBITDAYS);
            return new
            {
                resCount = rsMonthFee.Select(s => s.FeeNo).Distinct().Count(),
                hospDay = rsMonthFee.Select(s => s.HospDay).Sum(),
                nciItemTotalCost = rsMonthFee.Select(s => s.TotalAmount).Sum(),
                nciPay = rsMonthFee.Select(s => s.NCIPay).Sum(),
                //Amount = deAmount,
            };
        }

        public object CancelMonthData(string date)
        {
            var list = unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(w => w.STATUS == (int)BillStatus.Uploaded && w.ORGID == SecurityHelper.CurrentPrincipal.OrgId).ToList();
            list = list.Where(w => w.BALANCEENDTIME.ToString("yyyy-MM") == date).ToList();
            unitOfWork.BeginTransaction();
            list.ForEach((p) =>
            {
                Mapper.CreateMap<LTC_BILLV2, BillV2>();
                var bill = Mapper.Map<BillV2>(p);
                bill.Status = (int)BillStatus.Charge;
                base.Save<LTC_BILLV2, BillV2>(bill, (b) => b.BILLID == bill.BillId);
            });
            //var deList = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
            //    string.Compare(w.DEBITMONTH, date) <= 0 && w.DEDUCTIONSTATUS == 1).ToList();
            //deList.ForEach(p =>
            //{
            //    p.DEDUCTIONSTATUS = 0;
            //    unitOfWork.GetRepository<LTC_NCIDEDUCTION>().Update(p);
            //});
            unitOfWork.Commit();
            var org = unitOfWork.GetRepository<LTC_ORG>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            return org.NSNO;
        }
        public object GetMonthDataList(BaseRequest<MonthFeeFilter> request)
        {
            var response = new BaseResponse<IList<ResidentMonFee>>();
            if (string.IsNullOrEmpty(request.Data.date))
            {
                return response;
            }

            var billList = GetBillsByMonth(request.Data.date);

            var rsMonthFee = GetUploadDataAllResident(billList, request.Data.date);

            response.RecordsCount = rsMonthFee.Count();

            if (request != null && request.PageSize > 0)
            {
                var list = rsMonthFee.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.Data = list;
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = rsMonthFee;
            }
            return response;
        }

        public object GetOrgMonthDataList(string beginTime, string endTime)
        {
            var q = (from b in unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
                     select new
                     {
                         b.BALANCEENDTIME,
                         b.FEENO,
                         b.HOSPDAY,
                         b.NCIITEMTOTALCOST,
                         b.NCIPAY,
                         b.STATUS,
                     }).ToList();

            var q1 = q.GroupBy(g => new { BALANCEENDTIME = g.BALANCEENDTIME.ToString("yyyy-MM"), g.STATUS }).Select(s => new
            {
                YearMonth = s.Key.BALANCEENDTIME,
                FeeCount = s.Select(s1 => s1.FEENO).Distinct().Count(),
                TotalCost = s.Sum(m => m.NCIITEMTOTALCOST),
                HospDay = s.Sum(m => m.HOSPDAY),
                NCIPay = s.Sum(m => m.NCIPAY),
                STATUS = s.Key.STATUS
            });
            if (!string.IsNullOrEmpty(beginTime))
            {
                q1 = q1.Where(w => string.Compare(w.YearMonth, beginTime) >= 0).Select(s => s);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                q1 = q1.Where(w => string.Compare(w.YearMonth, endTime) <= 0).Select(s => s);
            }
            return q1;
        }

        public object UploadMonthData(string month)
        {
            var billList = GetBillsByMonth(month);

            var rsMonthFee = GetUploadDataAllResident(billList, month);

            var rsMonthFeeDtl = GetUploadDataResidentFeeDtl(billList);

            var nsMonthFee = GetUploadDataOrg(rsMonthFee, month, billList);

            var org = unitOfWork.GetRepository<LTC_ORG>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();

            //var nciDeduction = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
            //   string.Compare(w.DEBITMONTH, month) <= 0 && w.DEDUCTIONSTATUS == 0).Select(s => new
            //   {
            //       s.AMOUNT,
            //       s.DEBITDAYS,
            //       s.DEBITMONTH,
            //       s.DEDUCTIONREASON,
            //       s.DEDUCTIONTYPE,
            //       NSNO = org.NSNO,
            //       CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
            //   }).ToList();

            return new { nsNo = org.NSNO, nsMonthFee = nsMonthFee, rsMonthFee = rsMonthFee, rsMonthFeeDtl = rsMonthFeeDtl, /*nciDeduction*/ };
        }

        public BaseResponse<BillV2> UpdateBillStatusToUploaded(string month)
        {
            var billList = GetBillsByMonth(month);
            //更新相关账单状态
            billList.ForEach((p) =>
            {
                p.STATUS = (int)BillStatus.Uploaded;
            });
            //unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
            //    string.Compare(w.DEBITMONTH, month) <= 0 && w.DEDUCTIONSTATUS == 0).ToList().ForEach((p) =>
            //    {
            //        p.DEDUCTIONSTATUS = 1;
            //        unitOfWork.GetRepository<LTC_NCIDEDUCTION>().Update(p);
            //    });
            unitOfWork.Save();
            return new BaseResponse<BillV2>
            {
                ResultCode = 1,
                ResultMessage = "上传成功"
            };
        }

        //TODO 建一个Helper
        private void CalculateMonStartEndTime(string month, out DateTime startTime, out DateTime endTime)
        {
            startTime = DateTime.Parse(month + "-01 00:00:00");
            endTime = startTime.AddMonths(1).AddSeconds(-1);
        }
        public object GetRSMonFees(string date, long FeeNo)
        {
            DateTime startTime, endTime;
            CalculateMonStartEndTime(date, out startTime, out endTime);
            var q = from b in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                    where
                    b.FEENO == FeeNo && b.BALANCESTARTTIME >= startTime && b.BALANCEENDTIME <= endTime &&
                    b.STATUS == (int)BillStatus.Charge && b.ORGID == SecurityHelper.CurrentPrincipal.OrgId
                    join i in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on b.FEENO equals i.FEENO
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on i.REGNO equals r.REGNO
                    group new { b.NCIPAYSCALE, b.NCIITEMTOTALCOST, b.NCIPAY, r.NAME, r.IDNO, b.NCIPAYLEVEL, b.HOSPDAY, b.BALANCESTARTTIME, b.BALANCEENDTIME, b.SELFPAY } by
                    new { b.NCIPAYSCALE, r.NAME, r.IDNO, b.NCIPAYLEVEL, b.HOSPDAY, b.BALANCESTARTTIME, b.BALANCEENDTIME }
                        into g
                        select new
                        {
                            Name = g.Key.NAME,
                            IdNo = g.Key.IDNO,
                            NciPayScale = g.Key.NCIPAYSCALE,
                            TotalAmount = g.Sum(s => s.NCIITEMTOTALCOST) + g.Sum(s => s.SELFPAY),
                            NciPay = g.Sum(s => s.NCIPAY),
                            NciPayLevel = g.Key.NCIPAYLEVEL,
                            HospDay = g.Key.HOSPDAY,
                            BalanceStartTime = g.Key.BALANCESTARTTIME,
                            BalanceEndTime = g.Key.BALANCEENDTIME,

                        };


            return q.ToList().FirstOrDefault();
        }
        public object GetRSMonFeeDtl(int currentPage, int pageSize, string date, long FeeNo, string feeType)
        {
            BaseResponse<IList<RSMonFeeDtl>> response = new BaseResponse<IList<RSMonFeeDtl>>();
            DateTime startTime, endTime;
            CalculateMonStartEndTime(date, out startTime, out endTime);
            var billList = (from b in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                            where b.FEENO == FeeNo && b.BALANCESTARTTIME >= startTime && b.BALANCEENDTIME <= endTime &&
                            b.STATUS == (int)BillStatus.Charge && b.ORGID == SecurityHelper.CurrentPrincipal.OrgId
                            select b.BILLID).ToList();
            IQueryable<RSMonFeeDtl> q = null;
            if (feeType == "NCI")
            {
                q = ((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                      where o.CHARGERECORDTYPE == (int)ChargeItemType.Drug && billList.Contains(o.BILLID)
                      join d in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on o.CHARGERECORDID equals d.DRUGRECORDID
                      join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                      join dinfo in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on d.DRUGID equals dinfo.DRUGID
                      where dinfo.ISNCIITEM == true
                      select new RSMonFeeDtl
                      {
                          FEENAME = d.CNNAME,
                          FEETYPE = (int)ChargeItemType.Drug,
                          MCCODE = dinfo.MCDRUGCODE,
                          UNITPRICE = d.UNITPRICE,
                          QTY = d.QTY,
                          AMOUNT = d.COST,
                          TAKETIME = d.TAKETIME,
                          OPERATORNAME = e.EMPNAME,
                          FEENO = o.FEENO,

                      }).Concat((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                 where o.CHARGERECORDTYPE == (int)ChargeItemType.Material && billList.Contains(o.BILLID)
                                 join d in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on o.CHARGERECORDID equals d.MATERIALRECORDID
                                 join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                                 join minfo in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on d.MATERIALID equals minfo.MATERIALID
                                 where minfo.ISNCIITEM == true
                                 select new RSMonFeeDtl
                                 {
                                     FEENAME = d.MATERIALNAME,
                                     FEETYPE = (int)ChargeItemType.Material,
                                     MCCODE = minfo.MCMATERIALCODE,
                                     UNITPRICE = d.UNITPRICE,
                                     QTY = d.QTY,
                                     AMOUNT = d.COST,
                                     TAKETIME = d.TAKETIME,
                                     OPERATORNAME = e.EMPNAME,
                                     FEENO = o.FEENO,

                                 })).Concat((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                             where o.CHARGERECORDTYPE == (int)ChargeItemType.Service && billList.Contains(o.BILLID)
                                             join d in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on o.CHARGERECORDID equals d.SERVICERECORDID
                                             join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                                             join sinfo in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on d.SERVICEID equals sinfo.SERVICEID
                                             where sinfo.ISNCIITEM == true
                                             select new RSMonFeeDtl
                                             {
                                                 FEENAME = d.SERVICENAME,
                                                 FEETYPE = (int)ChargeItemType.Service,
                                                 MCCODE = sinfo.MCSERVICECODE,
                                                 UNITPRICE = d.UNITPRICE,
                                                 QTY = d.QTY,
                                                 AMOUNT = d.COST,
                                                 TAKETIME = d.TAKETIME,
                                                 OPERATORNAME = e.EMPNAME,
                                                 FEENO = o.FEENO,


                                             })));
            }
            else if (feeType == "SELF")
            {
                q = ((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                      where o.CHARGERECORDTYPE == (int)ChargeItemType.Drug && billList.Contains(o.BILLID)
                      join d in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on o.CHARGERECORDID equals d.DRUGRECORDID
                      join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                      join dinfo in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on d.DRUGID equals dinfo.DRUGID
                      where dinfo.ISNCIITEM == false
                      select new RSMonFeeDtl
                      {
                          FEENAME = d.CNNAME,
                          FEETYPE = (int)ChargeItemType.Drug,
                          MCCODE = dinfo.MCDRUGCODE,
                          UNITPRICE = d.UNITPRICE,
                          QTY = d.QTY,
                          AMOUNT = d.COST,
                          TAKETIME = d.TAKETIME,
                          OPERATORNAME = e.EMPNAME,
                          FEENO = o.FEENO,

                      }).Concat((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                 where o.CHARGERECORDTYPE == (int)ChargeItemType.Material && billList.Contains(o.BILLID)
                                 join d in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on o.CHARGERECORDID equals d.MATERIALRECORDID
                                 join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                                 join minfo in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on d.MATERIALID equals minfo.MATERIALID
                                 where minfo.ISNCIITEM == false
                                 select new RSMonFeeDtl
                                 {
                                     FEENAME = d.MATERIALNAME,
                                     FEETYPE = (int)ChargeItemType.Material,
                                     MCCODE = minfo.MCMATERIALCODE,
                                     UNITPRICE = d.UNITPRICE,
                                     QTY = d.QTY,
                                     AMOUNT = d.COST,
                                     TAKETIME = d.TAKETIME,
                                     OPERATORNAME = e.EMPNAME,
                                     FEENO = o.FEENO,


                                 })).Concat((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                             where o.CHARGERECORDTYPE == (int)ChargeItemType.Service && billList.Contains(o.BILLID)
                                             join d in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on o.CHARGERECORDID equals d.SERVICERECORDID
                                             join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                                             join sinfo in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on d.SERVICEID equals sinfo.SERVICEID
                                             where sinfo.ISNCIITEM == false
                                             select new RSMonFeeDtl
                                             {
                                                 FEENAME = d.SERVICENAME,
                                                 FEETYPE = (int)ChargeItemType.Service,
                                                 MCCODE = sinfo.MCSERVICECODE,
                                                 UNITPRICE = d.UNITPRICE,
                                                 QTY = d.QTY,
                                                 AMOUNT = d.COST,
                                                 TAKETIME = d.TAKETIME,
                                                 OPERATORNAME = e.EMPNAME,
                                                 FEENO = o.FEENO,


                                             })));
            }

            q = q.OrderByDescending(o => o.TAKETIME);
            response.RecordsCount = q.Count();
            var list = q.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            response.PagesCount = GetPagesCount(pageSize, response.RecordsCount);
            response.Data = list;
            return response;
        }
        //获取机构当月总有效账单.
        private List<LTC_BILLV2> GetBillsByMonth(string month)
        {
            //DateTime startTime, endTime;
            //CalculateMonStartEndTime(month, out startTime, out endTime);

            return unitOfWork.GetRepository<LTC_BILLV2>()
                .dbSet.Where(b => b.ORGID == SecurityHelper.CurrentPrincipal.OrgId
                && b.ISDELETE != true
                && b.STATUS == (int)BillStatus.Charge
                && b.NCIPAYSCALE > 0 //只上传有护理险报销资格的
                && b.BILLMONTH == month).ToList();
        }

        //获取住民月费用汇总
        private List<ResidentMonFee> GetUploadDataAllResident(IEnumerable<LTC_BILLV2> billList, string month)
        {
            DateTime startTime, endTime;
            //CalculateMonStartEndTime(month, out startTime, out endTime);
            DateTime dt = DateTime.Parse(month + "-01");
            startTime = DictHelper.GetFeeIntervalStartDateByDate(dt);
            endTime = DictHelper.GetFeeIntervalEndDateByDate(dt);
            //扣除请假天数
            var billIdList = billList.Select(s => s.BILLID);
            var deList = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
              string.Compare(w.DEBITMONTH, month) <= 0 && billIdList.Contains(w.BILLID)).Select(s => s).ToList();
            var resMonSum = (from b in billList
                             join d in deList on b.BILLID equals d.BILLID into db
                             from d in db.DefaultIfEmpty()
                             select new
                             {
                                 b.NCIITEMTOTALCOST,
                                 b.SELFPAY,
                                 b.NCIPAY,
                                 b.HOSPDAY,
                                 b.FEENO,
                                 b.NCIPAYLEVEL,
                                 b.NCIPAYSCALE,
                                 DEBITDAYS = d == null ? 0 : d.DEBITDAYS,
                             }).GroupBy(g => new { g.FEENO, g.NCIPAYLEVEL, g.NCIPAYSCALE }).Select(s => new
                {
                    TOTALAMOUNT = s.Sum(m => m.NCIITEMTOTALCOST) + s.Sum(m => m.SELFPAY),
                    NCIPAY = s.Sum(m => m.NCIPAY),
                    TOTALDAY = s.Sum(m => m.HOSPDAY),
                    FEENO = s.Key.FEENO,
                    NCIPAYLEVEL = s.Key.NCIPAYLEVEL,
                    NCIPAYSCALE = s.Key.NCIPAYSCALE,
                    DEBITDAYS = s.Sum(m => m.DEBITDAYS)
                });
            //var resMonSum = from b in billList
            //                join d in deList on b.BILLID equals d.BILLID into db
            //                from dbs in db.DefaultIfEmpty()
            //                group new { b.NCIITEMTOTALCOST, b.SELFPAY, b.NCIPAY, b.HOSPDAY, b.FEENO, dbs.DEBITDAYS } by b.FEENO into g
            //                select new
            //                {
            //                    TOTALAMOUNT = g.Sum(m => m.NCIITEMTOTALCOST) + g.Sum(m => m.SELFPAY),
            //                    NCIPAY = g.Sum(m => m.NCIPAY),
            //                    TOTALDAY = g.Sum(m => m.HOSPDAY) - g.Sum(m => m.DEBITDAYS),
            //                    FEENO = g.Key
            //                };

            //join获取住民额外信息
            var q = from s in resMonSum
                    join i in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on s.FEENO equals i.FEENO
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on i.REGNO equals r.REGNO
                    //join ba in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on s.FEENO equals ba.FEENO
                    //where ba.STATUS == 0
                    select new
                    {
                        r.NAME,
                        r.IDNO,
                        i.INDATE,
                        i.OUTDATE,
                        s.NCIPAYLEVEL,
                        s.NCIPAYSCALE,
                        //ba.APPLYHOSTIME,
                        s.TOTALAMOUNT,
                        s.NCIPAY,
                        s.FEENO,
                        //ba.CERTNO,
                        s.TOTALDAY,
                        s.DEBITDAYS
                    };
            var feeNoList = billList.Select(s => s.FEENO).Distinct();
            var regnciInfoList = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(w => feeNoList.Contains(w.FEENO)).GroupBy(g => new
            {
                g.FEENO,
                //g.NCIPAYLEVEL,
                //g.NCIPAYSCALE,
                g.CERTNO
            }).Select(s => new
            {
                FEENO = s.Key.FEENO,
                //NCIPAYLEVEL = s.Key.NCIPAYLEVEL,
                //NCIPAYSCALE = s.Key.NCIPAYSCALE,
                CERTNO = s.Key.CERTNO,
                APPLYHOSTIME = s.Max(m => m.APPLYHOSTIME),
            });
            var p = from b in q
                    join r in regnciInfoList on b.FEENO equals r.FEENO
                    select new
                    {
                        b.NAME,
                        b.IDNO,
                        b.INDATE,
                        b.OUTDATE,
                        b.NCIPAYLEVEL,
                        b.NCIPAYSCALE,
                        r.APPLYHOSTIME,
                        b.TOTALAMOUNT,
                        b.NCIPAY,
                        b.FEENO,
                        r.CERTNO,
                        b.TOTALDAY,
                        b.DEBITDAYS
                    };
            //生成住民月费用上传数据
            var rsMonthFee = p.Select(m =>
            {
                var entryDate = m.APPLYHOSTIME >= startTime ? m.APPLYHOSTIME : startTime;
                var dischargeDate = m.OUTDATE <= endTime ? m.OUTDATE : endTime;
                var totalDay = ((DateTime)dischargeDate - (DateTime)entryDate).Days + 1;
                return new ResidentMonFee
                {
                    YearMonth = month,
                    ResidentName = m.NAME,
                    ResidentSSId = m.IDNO,
                    HospEntryDate = (DateTime)entryDate,
                    HospDisChargeDate = (DateTime)dischargeDate,
                    HospDay = totalDay - m.DEBITDAYS.Value,
                    NCIPayLevel = (decimal)m.NCIPAYLEVEL,
                    NCIPayScale = (decimal)m.NCIPAYSCALE,
                    TotalAmount = m.TOTALAMOUNT,
                    NCIPay = m.NCIPAY,
                    CertNo = m.CERTNO,
                    FeeNo = m.FEENO,
                    CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,

                };
            }).ToList();
            return rsMonthFee;
        }

        //生成机构月费用上传数据
        private object GetUploadDataOrg(List<ResidentMonFee> rsMonthFee, string month, IEnumerable<LTC_BILLV2> billList)
        {
            var billIdList = billList.Select(s => s.BILLID);
            //var deAmount = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
            //   string.Compare(w.DEBITMONTH, month) <= 0 && w.DEDUCTIONSTATUS == 0).Sum(s => (double?)s.AMOUNT);
            //var deDays = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
            //   string.Compare(w.DEBITMONTH, month) <= 0 && billIdList.Contains(w.BILLID)).Sum(s => (int?)s.DEBITDAYS);
            return new
            {
                YEARMONTH = month,
                TOTALRESIDENT = rsMonthFee.Select(s => s.FeeNo).Distinct().Count(),
                TOTALHOSPDAY = rsMonthFee.GroupBy(g => g.ResidentSSId).Select(s => s.Sum(m => m.HospDay)).Sum(),
                TOTALAMOUNT = rsMonthFee.Select(s => s.TotalAmount).Sum(),
                TOTALNCIPAY = rsMonthFee.Select(s => s.NCIPay).Sum(),
                CREATORNAME = SecurityHelper.CurrentPrincipal.EmpName,
                CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo
            };
        }

        //获取住民费用明细
        private object GetUploadDataResidentFeeDtl(IEnumerable<LTC_BILLV2> billList)
        {
            var uploadRelatedBills = billList.Select(s => s.BILLID).ToList();
            var feeRecordList = ((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                  where o.CHARGERECORDTYPE == (int)ChargeItemType.Drug && uploadRelatedBills.Contains(o.BILLID)
                                  join d in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on o.CHARGERECORDID equals d.DRUGRECORDID
                                  join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                                  join dinfo in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on d.DRUGID equals dinfo.DRUGID
                                  select new
                                  {
                                      FEENAME = d.CNNAME,
                                      FEETYPE = ChargeItemType.Drug,
                                      MCCODE = dinfo.MCDRUGCODE,
                                      UNITPRICE = d.UNITPRICE,
                                      QTY = d.QTY,
                                      AMOUNT = d.COST,
                                      TAKETIME = d.TAKETIME,
                                      OPERATORNAME = e.EMPNAME,
                                      FEENO = o.FEENO,
                                      ISNCIITEM = dinfo.ISNCIITEM
                                  }).Concat((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                             where o.CHARGERECORDTYPE == (int)ChargeItemType.Material && uploadRelatedBills.Contains(o.BILLID)
                                             join d in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on o.CHARGERECORDID equals d.MATERIALRECORDID
                                             join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                                             join minfo in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on d.MATERIALID equals minfo.MATERIALID
                                             select new
                                             {
                                                 FEENAME = d.MATERIALNAME,
                                                 FEETYPE = ChargeItemType.Material,
                                                 MCCODE = minfo.MCMATERIALCODE,
                                                 UNITPRICE = d.UNITPRICE,
                                                 QTY = d.QTY,
                                                 AMOUNT = d.COST,
                                                 TAKETIME = d.TAKETIME,
                                                 OPERATORNAME = e.EMPNAME,
                                                 FEENO = o.FEENO,
                                                 ISNCIITEM = minfo.ISNCIITEM

                                             })).Concat((from o in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                                         where o.CHARGERECORDTYPE == (int)ChargeItemType.Service && uploadRelatedBills.Contains(o.BILLID)
                                                         join d in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on o.CHARGERECORDID equals d.SERVICERECORDID
                                                         join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.OPERATOR equals e.EMPNO
                                                         join sinfo in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on d.SERVICEID equals sinfo.SERVICEID
                                                         select new
                                                         {
                                                             FEENAME = d.SERVICENAME,
                                                             FEETYPE = ChargeItemType.Service,
                                                             MCCODE = sinfo.MCSERVICECODE,
                                                             UNITPRICE = d.UNITPRICE,
                                                             QTY = d.QTY,
                                                             AMOUNT = d.COST,
                                                             TAKETIME = d.TAKETIME,
                                                             OPERATORNAME = e.EMPNAME,
                                                             FEENO = o.FEENO,
                                                             ISNCIITEM = sinfo.ISNCIITEM

                                                         }))).ToList();

            //生成住民费用明细上传数据
            return feeRecordList.Select(s => new
            {
                FEENAME = s.FEENAME,
                FEETYPE = Util.GetEnumDescription<ChargeItemType>(s.FEETYPE),
                MCCODE = s.MCCODE ?? string.Empty,
                UNITPRICE = s.UNITPRICE,
                QTY = s.QTY,
                AMOUNT = s.AMOUNT,
                TAKETIME = s.TAKETIME,
                OPERATORNAME = s.OPERATORNAME,
                FEENO = s.FEENO,
                ISNCIITEM = s.ISNCIITEM,
                CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
            }).ToList();
        }

        public string GetNsno()
        {
            var org = unitOfWork.GetRepository<LTC_ORG>().dbSet.FirstOrDefault(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
            if (org == null)
            {
                return null;
            }
            return org.NSNO;
        }
        public List<PrintMonthFee> GetPrintData(BaseRequest<MonthFeeFilter> request)
        {
            var res = (BaseResponse<IList<ResidentMonFee>>)GetMonthDataList(request);
            var q = from s in res.Data
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on s.ResidentSSId equals r.IDNO
                    join i in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on r.REGNO equals i.REGNO
                    join ba in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on i.FEENO equals ba.FEENO
                    where ba.STATUS == 0
                    select new PrintMonthFee
                    {
                        InDate = i.INDATE,
                        OutDate = i.OUTDATE,
                        Name = r.NAME,
                        NCIPay = s.NCIPay,
                        NCIPayLevel = s.NCIPayLevel,
                        HospDay = s.HospDay,
                        TotalAmount = s.TotalAmount,
                        ResidentSSId = s.ResidentSSId,
                        Sex = r.SEX,
                        CareTypeId = ba.CARETYPEID,
                        CertStartTime = ba.CERTSTARTTIME,
                        BrithPlace = r.BRITHPLACE,
                        RsStatus = i.RSSTATUS,
                        DiseaseDiag = r.DISEASEDIAG,
                    };
            return q.ToList();
        }
        public List<PrintMonthFee> GetPrintData(BaseResponse<IList<ResidentMonFeeModel>> res)
        {
            var num = 1;
            var q = from s in res.Data
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on s.Residentssid equals r.IDNO
                    where r.ORGID == SecurityHelper.CurrentPrincipal.OrgId
                    join i in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on r.REGNO equals i.REGNO
                    //join ba in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on i.FEENO equals ba.FEENO
                    //where ba.STATUS == 0
                    select new PrintMonthFee
                    {
                        Index = num++,
                        InDate = i.INDATE,
                        OutDate = i.OUTDATE,
                        Name = r.NAME,
                        NCIPay = s.Ncipay,
                        NCIPayLevel = s.Ncipaylevel,
                        HospDay = s.Hospday,
                        TotalAmount = s.Totalamount,
                        ResidentSSId = s.Residentssid,
                        Sex = r.SEX,
                        FeeNo = i.FEENO,
                        //CareTypeId = ba.CARETYPEID,
                        //CertStartTime = ba.CERTSTARTTIME,
                        BrithPlace = r.BRITHPLACE,
                        RsStatus = i.RSSTATUS,
                        DiseaseDiag = r.DISEASEDIAG,
                        //ApplyHosTime = ba.APPLYHOSTIME,
                    };
            var p = q.ToList();
            var feeNoList = p.Select(s => s.FeeNo).Distinct();
            var rList = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(w => feeNoList.Contains(w.FEENO)).Select(s => s).ToList();
            var regnciInfoList = rList.GroupBy(g => new
            {
                g.FEENO,
                //g.CARETYPEID,
                //g.CERTSTARTTIME,
                //g.APPLYHOSTIME,
                //g.NCIPAYLEVEL,
                //g.NCIPAYSCALE,
                g.CERTNO
            }).Select(s => new
            {
                FEENO = s.Key.FEENO,
                //NCIPAYLEVEL = s.Key.NCIPAYLEVEL,
                //NCIPAYSCALE = s.Key.NCIPAYSCALE,
                CERTNO = s.Key.CERTNO,
                APPLYHOSTIME = s.Max(m => m.APPLYHOSTIME),
            });
            var rValue = from b in p
                         join r in regnciInfoList on b.FeeNo equals r.FEENO
                         select new PrintMonthFee
                    {
                        Index = b.Index,
                        InDate = b.InDate,
                        OutDate = b.OutDate,
                        Name = b.Name,
                        NCIPay = b.NCIPay,
                        NCIPayLevel = b.NCIPayLevel,
                        HospDay = b.HospDay,
                        TotalAmount = b.TotalAmount,
                        ResidentSSId = b.ResidentSSId,
                        Sex = b.Sex,
                        FeeNo = b.FeeNo,
                        CareTypeId = rList.Where(w => w.FEENO == b.FeeNo && w.APPLYHOSTIME == r.APPLYHOSTIME).FirstOrDefault() == null ?
                        "" : rList.Where(w => w.FEENO == b.FeeNo && w.APPLYHOSTIME == r.APPLYHOSTIME).FirstOrDefault().CARETYPEID,
                        CertStartTime = rList.Where(w => w.FEENO == b.FeeNo && w.APPLYHOSTIME == r.APPLYHOSTIME).FirstOrDefault() == null ?
                        DateTime.Now : rList.Where(w => w.FEENO == b.FeeNo && w.APPLYHOSTIME == r.APPLYHOSTIME).FirstOrDefault().CERTSTARTTIME,
                        BrithPlace = b.BrithPlace,
                        RsStatus = b.RsStatus,
                        DiseaseDiag = b.DiseaseDiag,
                        ApplyHosTime = r.APPLYHOSTIME,
                    };
            return rValue.ToList();

        }
        public List<PrintMonthFee> GetPrintData(BaseResponse<IList<TreatmentAccount>> res)
        {
            var num = 1;
            var q = from s in res.Data
                    join i in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on s.FeeNo equals i.FEENO
                    select new PrintMonthFee
                    {
                        Index = num++,
                        InDate = i.INDATE,
                        OutDate = i.OUTDATE,
                        Name = s.Name,
                        NCIPay = s.Ncipay,
                        NCIPayLevel = s.Ncipaylevel,
                        HospDay = s.Hospday,
                        TotalAmount = s.Totalamount,
                        ResidentSSId = s.Residentssid,
                        Sex = s.Gender,
                        CareTypeId = s.NsappcareTypeName,
                        EvaluationTime = s.EvaluationTime,
                        BrithPlace = s.Residence,
                        RsStatus = s.McTypeName,
                        DiseaseDiag = s.Disease,
                        ApplyHosTime = s.CertStartTime,
                        yearMonthArr = s.yearMonthArr,
                        FeeNo = s.FeeNo
                    };
            var list = q.ToList();
            var feeNoList = list.Select(s => s.FeeNo).Distinct();
            var regnciInfoList = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(w => feeNoList.Contains(w.FEENO)).GroupBy(g => new
            {
                g.FEENO,

            }).Select(s => new
            {
                FEENO = s.Key.FEENO,
                APPLYHOSTIME = s.Max(m => m.APPLYHOSTIME),
            }).ToList();
            var p = (from r1 in list
                    join r2 in regnciInfoList on r1.FeeNo equals r2.FEENO
                    select new PrintMonthFee
                    {
                        Index = r1.Index,
                        InDate = r2.APPLYHOSTIME >= r1.InDate ? r2.APPLYHOSTIME : r1.InDate,
                        OutDate = r1.OutDate,
                        Name = r1.Name,
                        NCIPay = r1.NCIPay,
                        NCIPayLevel = r1.NCIPayLevel,
                        HospDay = r1.HospDay,
                        TotalAmount = r1.TotalAmount,
                        ResidentSSId = r1.ResidentSSId,
                        Sex = r1.Sex,
                        CareTypeId = r1.CareTypeId,
                        EvaluationTime = r1.EvaluationTime,
                        BrithPlace = r1.BrithPlace,
                        RsStatus = r1.RsStatus,
                        DiseaseDiag = r1.DiseaseDiag,
                        ApplyHosTime = r1.ApplyHosTime,
                        yearMonthArr = r1.yearMonthArr,
                    }).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var yearList = p[i].yearMonthArr.Split(',');
                var yearMin = DateTime.Parse(yearList.Min() + "-01");
                var yearMax = DateTime.Parse(yearList.Max() + "-01");
                var startDate = DictHelper.GetFeeIntervalStartDateByDate(yearMin);
                var EndDate = DictHelper.GetFeeIntervalEndDateByDate(yearMax);
                p[i].InDate = DateTime.Compare(startDate, p[i].InDate.Value) >= 0 ? startDate : p[i].InDate.Value;
                p[i].OutDate = p[i].OutDate.HasValue ? (DateTime.Compare(EndDate, p[i].OutDate.Value) <= 0 ? EndDate : p[i].OutDate.Value) : EndDate;
            }
            return p;
        }

    }
}
