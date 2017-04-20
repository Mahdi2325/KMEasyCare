/*****************************************************************************
 * Creator:	Lei Chen
 * Create Date: 2016-03-14
 * Modifier:
 * Modify Date:
 * Description:照护计划
 ******************************************************************************/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace KMHC.SLTC.Business.Implement
{
    public class CarePlansManageService : BaseService, ICarePlansManageService
    {
        IDictManageService _dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
        #region 照护计划制定
        private static IEnumerable<CARE_PLANPROBLEM> cp;
        private static IEnumerable<CARE_PLANREASON> cp_reason;
        private static IEnumerable<CARE_PLANDATA> cp_data;

        public BaseResponse<List<CodeValue>> GetCategory()
        {
            BaseResponse<List<CodeValue>> response = new BaseResponse<List<CodeValue>>();
            response.Data = (from p in AllCP
                             select new CodeValue
                             {
                                 ItemCode = p.CATEGORY,
                                 ItemName = p.CATEGORY
                             }).Distinct(new CodeValueComparer()).ToList();

            return response;
        }

        public BaseResponse<List<CodeValue>> GetLevelPRCategory(string category)
        {
            BaseResponse<List<CodeValue>> response = new BaseResponse<List<CodeValue>>();

            response.Data = (from p in AllCP
                             select new CodeValue
                                             {
                                                 ItemCode = p.LEVELPR,
                                                 ItemName = p.LEVELPR
                                             }).Distinct(new CodeValueComparer()).ToList();

            return response;
        }

        public BaseResponse<List<CodeValue>> GetDiaPR(string category, string levelPR)
        {
            BaseResponse<List<CodeValue>> response = new BaseResponse<List<CodeValue>>();
            if (!string.IsNullOrEmpty(levelPR))
            {
                response.Data = (from p in AllCP.Where(x => x.LEVELPR == levelPR)
                                 select new CodeValue
                                 {
                                     ItemCode = p.CP_NO.ToString(),
                                     ItemName = p.DIAPR
                                 }).Distinct().ToList();
            }
            return response;
        }

        public BaseResponse<List<string>> GetCP_Reason(int cp_no)
        {
            BaseResponse<List<string>> response = new BaseResponse<List<string>>();

            List<string> result = (from p in AllCP_Reason.Where(x => x.CP_NO == cp_no)
                                   select p.CAUSEP
                            ).ToList();
            response.Data = result;
            // response.ResultMessage = string.Join("\n", result.ToArray());
            return response;
        }

        public BaseResponse<List<string>> GetCP_Data(int cp_no)
        {
            BaseResponse<List<string>> response = new BaseResponse<List<string>>();

            List<string> result = (from p in AllCP_Data.Where(x => x.CP_NO == cp_no && x.PR == "P")
                                   select p.PRDATA
                            ).ToList();
            // response.ResultMessage = string.Join("\n", result.ToArray());
            response.Data = result;
            return response;
        }

        public BaseResponse<NSCPL> SaveNSCPL(NSCPL request)
        {
            BaseResponse<NSCPL> response = new BaseResponse<NSCPL>();
            Mapper.CreateMap<NSCPL, LTC_NSCPL>();
            var model = unitOfWork.GetRepository<LTC_NSCPL>().dbSet.Where(x => x.SEQNO == request.SEQNO).FirstOrDefault();
            if (model == null)
            {
                request.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CREATEDATE = DateTime.Now;
                model = Mapper.Map<LTC_NSCPL>(request);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                unitOfWork.GetRepository<LTC_NSCPL>().Insert(model);
            }
            else
            {
                Mapper.Map(request, model);
                unitOfWork.GetRepository<LTC_NSCPL>().Update(model);
            }
            unitOfWork.Save();
            request.SEQNO = model.SEQNO;
            response.Data = request;
            return response;
        }

        #endregion



        #region 照护目标
        public BaseResponse<IList<NSCPLGOAL>> GetCareGoalList(long seqNo)
        {
            var response = base.Query<LTC_NSCPLGOAL, NSCPLGOAL>(null, (q) =>
            {
                q = q.Where(m => m.SEQNO == seqNo);
                return q;
            });
            return response;
        }
        public BaseResponse<NSCPLGOAL> SaveGoal(NSCPLGOAL request)
        {
            BaseResponse<NSCPLGOAL> response = new BaseResponse<NSCPLGOAL>();
            Mapper.CreateMap<NSCPLGOAL, LTC_NSCPLGOAL>();
            var model = unitOfWork.GetRepository<LTC_NSCPLGOAL>().dbSet.Where(x => x.ID == request.ID).FirstOrDefault();
            if (model == null)
            {
                model = Mapper.Map<LTC_NSCPLGOAL>(request);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                unitOfWork.GetRepository<LTC_NSCPLGOAL>().Insert(model);

            }
            else
            {
                Mapper.Map(request, model);
                unitOfWork.GetRepository<LTC_NSCPLGOAL>().Update(model);
            }
            unitOfWork.Save();
            request.ID = model.ID;
            response.Data = request;
            return response;
        }
        public BaseResponse DeleteGoal(long id)
        {
            return base.Delete<LTC_NSCPLGOAL>(id);
        }
        public BaseResponse<List<CodeValue>> GetPlanGoalsLP(int cp_no)
        {
            BaseResponse<List<CodeValue>> response = new BaseResponse<List<CodeValue>>();
            response.Data = (from p in unitOfWork.GetRepository<CARE_PLANGOAL>().dbSet.Where(x => x.CP_NO == cp_no)
                             select new CodeValue
                             {
                                 ItemCode = p.GOALP,
                                 ItemName = p.GOALP
                             }).ToList();

            return response;
        }

        #endregion

        #region 照护措施
        public BaseResponse<IList<LTC_NSCPLActivity>> GetCareActivityList(long seqNo)
        {
            var response = base.Query<LTC_NSCPLACTIVITY, LTC_NSCPLActivity>(null, (q) =>
            {
                q = q.Where(m => m.SEQNO == seqNo);
                return q;
            });
            return response;
        }
        public BaseResponse<LTC_NSCPLActivity> SaveActivity(LTC_NSCPLActivity request)
        {
            BaseResponse<LTC_NSCPLActivity> response = new BaseResponse<LTC_NSCPLActivity>();
            Mapper.CreateMap<LTC_NSCPLActivity, LTC_NSCPLACTIVITY>();
            var model = unitOfWork.GetRepository<LTC_NSCPLACTIVITY>().dbSet.Where(x => x.ID == request.ID).FirstOrDefault();
            if (model == null)
            {
                model = Mapper.Map<LTC_NSCPLACTIVITY>(request);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                unitOfWork.GetRepository<LTC_NSCPLACTIVITY>().Insert(model);
            }
            else
            {
                Mapper.Map(request, model);
                unitOfWork.GetRepository<LTC_NSCPLACTIVITY>().Update(model);
            }
            unitOfWork.Save();
            request.ID = model.ID;
            response.Data = request;
            return response;
            //return base.Save<LTC_NSCPLACTIVITY, NSCPLACTIVITY>(request, (q) => q.ID == request.ID);
        }
        public BaseResponse DeleteActivity(long id)
        {
            return base.Delete<LTC_NSCPLACTIVITY>(id);
        }
        public BaseResponse<List<CodeValue>> GetPlanActivityLP(int cp_no)
        {
            BaseResponse<List<CodeValue>> response = new BaseResponse<List<CodeValue>>();
            response.Data = (from p in unitOfWork.GetRepository<CARE_PLANACTIVITY>().dbSet.Where(x => x.CP_NO == cp_no)
                             select new CodeValue
                             {
                                 ItemCode = p.ACTIVITY,
                                 ItemName = p.ACTIVITY
                             }).ToList();

            return response;
        }
        public LTC_NSCPLActivity GetActivity(int id)
        {

            var q = unitOfWork.GetRepository<LTC_NSCPLACTIVITY>().dbSet.Where(x => x.ID == id).Select(s => new LTC_NSCPLActivity
            {
                CPLACTIVITY = s.CPLACTIVITY
            }).ToList().FirstOrDefault();
            return q;

        }

        #endregion

        #region 照护列表
        public BaseResponse<IList<NSCPLView>> QueryCarePlanList(CarePlansFilter request)
        {
            BaseResponse<IList<NSCPLView>> response = new BaseResponse<IList<NSCPLView>>();

            List<NSCPLView> list = null;
            if (request != null && request.PageSize > 0)
            { }
            else
            {
                StringBuilder sb = new StringBuilder();
                string sql = string.Format(@" SELECT REG.REGNO,REG.NAME,IPD.FEENO, PL.SEQNO, PL.STARTDATE,PL.FINISHDATE,PL.CPTYPE,PL.CPLEVEL,PL.CPDIAG, PL.EMPNO,PL.FINISHFLAG,
                                    (SELECT EMPNAME FROM LTC_EMPFILE WHERE PL.EMPNO=EMPNO) AS EMPNAME
                                    ,(SELECT COUNT(*) FROM LTC_NSCPL WHERE REGNO=REG.REGNO) AS QUANTITY
                                    ,(SELECT COUNT(*) FROM LTC_NSCPL WHERE REGNO=REG.REGNO AND FINISHFLAG=1) AS QUANFINISH
                                        FROM LTC_REGFILE REG 
                                    LEFT JOIN ( SELECT * FROM ( SELECT * FROM LTC_NSCPL	ORDER BY STARTDATE DESC ) T GROUP BY REGNO ) PL
                                    ON REG.REGNO=PL.REGNO
                                    INNER JOIN LTC_IPDREG IPD
                                    ON IPD.REGNO=REG.REGNO AND IPD.IPDFLAG='I'
									WHERE REG.ORGID='{0}'", SecurityHelper.CurrentPrincipal.OrgId);
                sb.Append(sql);
                if (request != null)
                {
                    if (!string.IsNullOrEmpty(request.ItemType))
                    {
                        sb.Append(string.Format(" AND ITEMTYPE='{0}'", request.ItemType));
                    }
                    if (!string.IsNullOrEmpty(request.Name))
                    {
                        sb.Append(string.Format(" AND NAME LIKE '%{0}%'", request.Name));
                    }
                    if (!string.IsNullOrEmpty(request.Date))
                    {
                        sb.Append(string.Format(" AND INJECTDATE= CONVERT(DATE,'{0}',120) ", request.Date));
                    }
                }
                list = unitOfWork.GetRepository<NSCPLView>().SqlQuery(sb.ToString()).ToList();
                foreach (NSCPLView item in list)
                {
                    item.CPDIAG = GetDIAPR(item.CPDIAG);
                    item.PERCENTAGE = GetFinishPercentage(item.QUANFINISH, item.QUANTITY);
                }
            }
            response.Data = list;
            return response;
        }

        public BaseResponse<NSCPL> GetCarePlan(long seqNo)
        {
            return base.Get<LTC_NSCPL, NSCPL>((q) => q.SEQNO == seqNo);
        }

        #endregion

        #region 照护计划明细
        public BaseResponse<IList<NSCPL>> QueryCarePlanDetail(BaseRequest<CarePlanDetailFilter> request)
        {
            BaseResponse<IList<NSCPL>> response = new BaseResponse<IList<NSCPL>>();
            var q = (from ns in unitOfWork.GetRepository<LTC_NSCPL>().dbSet.Where(x => x.FEENO == request.Data.FeeNo)
                     join em_f in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ns.EMPNO equals em_f.EMPNO into ns_emp
                     from ns_d in ns_emp.DefaultIfEmpty()
                     select new NSCPL
                     {
                         SEQNO = ns.SEQNO,
                         EMPNO = ns_d.EMPNAME,
                         CPSOURCE = ns.CPSOURCE,
                         CPTYPE = ns.CPTYPE,
                         CPLEVEL = ns.CPLEVEL,
                         CPDIAG = ns.CPDIAG,
                         CPNO = ns.CPDIAG,
                         NSDESC = ns.NSDESC,
                         CPCAUSE = ns.CPCAUSE,
                         CPREASON = ns.CPREASON,
                         FINISHFLAG = ns.FINISHFLAG,
                         CPRESULT = ns.CPRESULT,
                         DESCRIPTION = ns.DESCRIPTION,
                         CREATEBY = ns.CREATEBY,
                         ORGID = ns.ORGID,
                         FEENO = ns.FEENO,
                         REGNO = ns.REGNO,
                         NEEDDAYS = ns.NEEDDAYS,
                         TOTALDAYS = ns.TOTALDAYS,
                         STARTDATE = ns.STARTDATE,
                         TARGETDATE = ns.TARGETDATE,
                         FINISHDATE = ns.FINISHDATE,
                         CREATEDATE = ns.CREATEDATE

                     }).ToList();

            q = q.ToList().OrderByDescending(m => m.SEQNO).ToList();

            response.RecordsCount = q.Count();

            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);

                foreach (NSCPL item in response.Data)
                {
                    item.CPDIAG = GetDIAPR(item.CPDIAG);
                }
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }
        public object QueryAssTask(BaseRequest<AssignTaskFilterByBobDu> request)
        {

            var q = from ns in unitOfWork.GetRepository<LTC_NSCPL>().dbSet.Where(x => x.FEENO == request.Data.feeno)

                    join ipd in unitOfWork.GetRepository<LTC_NSCPLACTIVITY>().dbSet on ns.SEQNO equals ipd.SEQNO into ipd_Reg
                    from ipdReg in ipd_Reg.DefaultIfEmpty()


                    select new
                    {
                        id = ipdReg.ID,
                        title = ipdReg.CPLACTIVITY,
                        start = ipdReg.RECDATE,
                        end = ipdReg.FINISHDATE

                    };

            q = q.Where(m => m.start >= request.Data.start && m.start <= request.Data.end);
            return q;

        }
        public BaseResponse DeleteCarePlan(long id)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                using (TWSLTCEntities context = new TWSLTCEntities())
                {
                    List<LTC_ASSESSVALUE> assess = context.LTC_ASSESSVALUE.Where(x => x.SEQNO == id).ToList();
                    if (assess.Count > 0)
                    {
                        assess.ForEach(p => context.LTC_ASSESSVALUE.Attach(p));
                        assess.ForEach(p => context.LTC_ASSESSVALUE.Remove(p));
                    }

                    List<LTC_NSCPLGOAL> goal = context.LTC_NSCPLGOAL.Where(x => x.SEQNO == id).ToList();
                    if (goal.Count > 0)
                    {
                        goal.ForEach(p => context.LTC_NSCPLGOAL.Attach(p));
                        goal.ForEach(p => context.LTC_NSCPLGOAL.Remove(p));
                    }
                    List<LTC_NSCPLACTIVITY> activity = context.LTC_NSCPLACTIVITY.Where(x => x.SEQNO == id).ToList();
                    if (activity.Count > 0)
                    {
                        activity.ForEach(p => context.LTC_NSCPLACTIVITY.Attach(p));
                        activity.ForEach(p => context.LTC_NSCPLACTIVITY.Remove(p));
                    }

                    LTC_NSCPL nscpl = context.LTC_NSCPL.Where(x => x.SEQNO == id).FirstOrDefault();
                    context.LTC_NSCPL.Attach(nscpl);
                    context.LTC_NSCPL.Remove(nscpl);
                    context.SaveChanges();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region 照护评值
        public BaseResponse<ASSESSVALUE> SaveAssess(ASSESSVALUE request)
        {
            BaseResponse<ASSESSVALUE> response = new BaseResponse<ASSESSVALUE>();
            Mapper.CreateMap<ASSESSVALUE, LTC_ASSESSVALUE>();
            var model = unitOfWork.GetRepository<LTC_ASSESSVALUE>().dbSet.Where(x => x.ID == request.ID).FirstOrDefault();
            if (model == null)
            {
                model = Mapper.Map<LTC_ASSESSVALUE>(request);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                model.RECORDBY = SecurityHelper.CurrentPrincipal.EmpNo;
                unitOfWork.GetRepository<LTC_ASSESSVALUE>().Insert(model);
                request.RECORDBY = SecurityHelper.CurrentPrincipal.EmpNo;
            }
            else
            {
                Mapper.Map(request, model);
                unitOfWork.GetRepository<LTC_ASSESSVALUE>().Update(model);
            }
            //if (!string.IsNullOrEmpty(request.EXECUTEBY))
            //{
            //    request.EXECUTEBY = unitOfWork.GetRepository<LTC_EMPFILE>().dbSet.Where(x => x.EMPNO == request.EXECUTEBY).FirstOrDefault().EMPNAME;
            //}
            unitOfWork.Save();
            request.ID = model.ID;
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteAssess(long id)
        {
            return base.Delete<LTC_ASSESSVALUE>(id);
        }

        public BaseResponse<IList<ASSESSVALUE>> GetCareAssessList(long seqNo)
        {
            var response = new BaseResponse<IList<ASSESSVALUE>>();
            var q = from n in unitOfWork.GetRepository<LTC_ASSESSVALUE>().dbSet.Where(m => m.SEQNO == seqNo)
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    join m in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.EXECUTEBY equals m.EMPNO into mes
                    from me in mes.DefaultIfEmpty()
                    select new
                    {
                        OutValue = n,
                        EmpName = re.EMPNAME,
                        RECORDBY = n.RECORDBY,
                        ExecuteName = me.EMPNAME,
                        ExecuteBy = n.EXECUTEBY
                    };
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ASSESSVALUE>();
                foreach (dynamic item in list)
                {
                    ASSESSVALUE newItem = Mapper.DynamicMap<ASSESSVALUE>(item.OutValue);
                    newItem.RECORDBY = item.RECORDBY;
                    newItem.EXECUTEBYNAME = item.ExecuteName;
                    newItem.EXECUTEBY = item.ExecuteBy;
                    response.Data.Add(newItem);
                }

            };
            var result = q.ToList();
            mapperResponse(result);
            return response;
        }

        public BaseResponse<List<CodeValue>> GetPlanAssessLP(int cp_no)
        {
            BaseResponse<List<CodeValue>> response = new BaseResponse<List<CodeValue>>();
            response.Data = (from p in unitOfWork.GetRepository<CARE_PLANEVAL>().dbSet.Where(x => x.CP_NO == cp_no)
                             select new CodeValue
                             {
                                 ItemCode = p.ASSESSVALUER,
                                 ItemName = p.ASSESSVALUER
                             }).ToList();

            return response;
        }

        public BaseResponse<object> GetCarePlanForExtApi(BaseRequest<CarePlanRecFilter> request)
        {
            var response = new BaseResponse<object>();
            var q = from c in unitOfWork.GetRepository<LTC_NSCPL>().dbSet
                    join d in unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Where(m=>m.ITEMTYPE=="G00.001") on c.FINISHFLAG equals  d.ITEMCODE
                    select new
                    {
                        FeeNo = c.FEENO,
                        CplLevel = c.CPLEVEL,
                        CplReason = c.CPREASON,
                        CplResult = c.CPRESULT,
                        StartDate = c.STARTDATE,
                        EndDate = c.FINISHDATE,
                        FinishFlag = d.ITEMNAME,
                        SeqNo = c.SEQNO
                    };
            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            if (request.Data.SDate.HasValue)
            {
                q = q.Where(m => m.StartDate >= request.Data.SDate);
            }
            if (request.Data.EDate.HasValue)
            {
                q = q.Where(m => m.StartDate < request.Data.EDate);
            }
            q = q.OrderByDescending(m => m.StartDate);
            response.RecordsCount = q.Count();
            var list = q.ToList();
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            var data = list.Select(p =>
            {
                var activity = unitOfWork.GetRepository<LTC_NSCPLACTIVITY>().dbSet.Where(m => m.SEQNO == p.SeqNo)
                    .Select(x => new
                    {
                        Activity = x.CPLACTIVITY
                    });
                var goal = unitOfWork.GetRepository<LTC_NSCPLGOAL>().dbSet.Where(m => m.SEQNO == p.SeqNo)
                     .Select(x => new
                     {
                         Activity = x.CPLGOAL
                     });
                return new
                {
                    CplLevel = p.CplLevel,
                    CplReason = p.CplReason,
                    CplResult = p.CplResult,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    FinishFlag = p.FinishFlag,
                    Activity = activity,
                    Goal = goal
                };
            });
            response.Data = data;
            return response;
        }

        #endregion



        #region Helper

        private static void EnsureLTC_CPRefresh()
        {
            using (TWSLTCContext context = new TWSLTCContext())
            {
                cp = context.CARE_PLANPROBLEM.ToList();
            }
        }
        private static void EnsureLTC_CPReasonRefresh()
        {
            using (TWSLTCContext context = new TWSLTCContext())
            {
                cp_reason = context.CARE_PLANREASON.ToList();
            }
        }
        private static void EnsureLTC_CPDataRefresh()
        {
            using (TWSLTCContext context = new TWSLTCContext())
            {
                cp_data = context.CARE_PLANDATA.ToList();
            }
        }
        private static IEnumerable<CARE_PLANPROBLEM> AllCP
        {
            get
            {
                if (cp == null)
                {
                    EnsureLTC_CPRefresh();
                }
                return cp;
            }
        }

        private static IEnumerable<CARE_PLANREASON> AllCP_Reason
        {
            get
            {
                if (cp_reason == null)
                {
                    EnsureLTC_CPReasonRefresh();
                }
                return cp_reason;
            }
        }

        private static IEnumerable<CARE_PLANDATA> AllCP_Data
        {
            get
            {
                if (cp_data == null)
                {
                    EnsureLTC_CPDataRefresh();
                }
                return cp_data;
            }
        }

        private string GetDIAPR(string CP_NO)
        {
            if (!string.IsNullOrEmpty(CP_NO))
            {
                int CP_ID = int.Parse(CP_NO);
                if (AllCP.Where(x => x.CP_NO == CP_ID).Count() > 0)
                {
                    return AllCP.Where(x => x.CP_NO == CP_ID).FirstOrDefault().DIAPR;
                }
            }
            return "";
        }

        private string GetFinishPercentage(int finishCount, int totalCount)
        {
            if (totalCount == 0 || finishCount == 0)
            {
                return "0.00%";
            }
            double percent = (double)finishCount / (double)totalCount;
            return percent.ToString("P");
        }


        #endregion


    }
}

