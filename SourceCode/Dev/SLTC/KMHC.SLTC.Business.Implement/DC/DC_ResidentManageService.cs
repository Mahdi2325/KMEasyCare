using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.DC
{
    public class DC_ResidentManageService : BaseService, IDC_ResidentManageService
    {
        #region ResidentList

        public BaseResponse<IList<DC_Resident>> QueryDCResident(BaseRequest<DC_ResidentFilter> request)
        {
            string _orgId = SecurityHelper.CurrentPrincipal.OrgId;
            BaseResponse<IList<DC_Resident>> response = new BaseResponse<IList<DC_Resident>>();
            var q = from ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet
                    where ipd.ORGID == _orgId
                    join r in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                    join orgs in unitOfWork.GetRepository<LTC_ORG>().dbSet on ipd_r.ORGID equals orgs.ORGID into o
                    from org in o.DefaultIfEmpty()
                    select new DC_Resident
                    {
                        FeeNo = ipd.FEENO,
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.REGNAME,
                        NickName = ipd_r.NICKNAME,
                        Sex = ipd_r.SEX,
                        BirthDate = ipd_r.BIRTHDATE,
                        // Age = Util.GetAgeByDate(ipd_r.BIRTHDATE),
                        StationCode = ipd.STATIONCODE,
                        OrgId = ipd_r.ORGID,
                        ResidentNo = ipd.RESIDENTNO,
                        IpdFlag = ipd.IPDFLAG,
                        InDate = ipd.INDATE,
                        IdNo = ipd_r.IDNO,
                        BirthPlace = ipd_r.BIRTHPLACE,
                        OrgName = org.ORGNAME
                    };
            if (!string.IsNullOrEmpty(request.Data.IpdFlag))
            {
                if (request.Data.IpdFlag == "I")
                {
                    q = q.Where(m => m.IpdFlag == "I" || m.IpdFlag == "N");
                }
                else
                {
                    q = q.Where(m => m.IpdFlag == "O");
                }

            }
            if (!string.IsNullOrEmpty(request.Data.ResidentName))
            {
                q = q.Where(m => m.Name.Contains(request.Data.ResidentName));
            }
            if (!string.IsNullOrEmpty(request.Data.ResidentNo))
            {
                q = q.Where(m => m.ResidentNo.Contains(request.Data.ResidentNo));
            }
            if (!string.IsNullOrEmpty(request.Data.StationCode))
            {
                q = q.Where(m => m.StationCode == request.Data.StationCode);
            }
            q = q.OrderByDescending(m => m.FeeNo);
            response.RecordsCount = q.Count();
            response.Data = q.Distinct().ToList();
            return response;
        }

        public BaseResponse<DC_Resident> GetDCResident(string regNo)
        {
            string _orgId = SecurityHelper.CurrentPrincipal.OrgId;
            BaseResponse<DC_Resident> response = new BaseResponse<DC_Resident>();
            var q = from ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet
                    where ipd.ORGID == _orgId && ipd.REGNO == regNo
                    join r in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                    join orgs in unitOfWork.GetRepository<LTC_ORG>().dbSet on ipd_r.ORGID equals orgs.ORGID into o
                    from org in o.DefaultIfEmpty()
                    select new DC_Resident
                    {
                        FeeNo = ipd.FEENO,
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.REGNAME,
                        NickName = ipd_r.NICKNAME,
                        Sex = ipd_r.SEX,
                        BirthDate = ipd_r.BIRTHDATE,
                        StationCode = ipd.STATIONCODE,
                        OrgId = ipd_r.ORGID,
                        ResidentNo = ipd.RESIDENTNO,
                        IpdFlag = ipd.IPDFLAG,
                        InDate = ipd.INDATE,
                        IdNo = ipd_r.IDNO,
                        BirthPlace = ipd_r.BIRTHPLACE,
                        OrgName = org.ORGNAME
                    };
            response.Data = q.FirstOrDefault();
            return response;
        }
        #endregion

        #region 需求评估
        public BaseResponse<DcNurseingPlanEval> QueryCurrentDcNurseingPlanEval(BaseRequest<DcNurseingPlanEvalFilter> request)
        {
            BaseResponse<DcNurseingPlanEval> response = new BaseResponse<DcNurseingPlanEval>();
            Mapper.CreateMap<DC_NURSEINGPLANEVAL, DcNurseingPlanEval>();
            var q = from m in unitOfWork.GetRepository<DC_NURSEINGPLANEVAL>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.ID);
            if (q.Count() > 0)
            {

                response.Data = Mapper.Map<IList<DcNurseingPlanEval>>(q.ToList())[0];
            }
            return response;
        }
        public BaseResponse<IList<DcNurseingPlanEval>> QueryDcNurseingPlanEval(BaseRequest<DcNurseingPlanEvalFilter> request)
        {
            BaseResponse<IList<DcNurseingPlanEval>> response = new BaseResponse<IList<DcNurseingPlanEval>>();
            Mapper.CreateMap<DC_NURSEINGPLANEVAL, DcNurseingPlanEval>();
            var q = from m in unitOfWork.GetRepository<DC_NURSEINGPLANEVAL>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.EVALNUMBER);
            response.RecordsCount = q.Count();
            List<DC_NURSEINGPLANEVAL> list = null;
            if (response.RecordsCount > 0)
            {
                if (request != null && request.PageSize > 0)
                {
                    list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    list = q.ToList();
                }
                response.Data = Mapper.Map<IList<DcNurseingPlanEval>>(list);
            }
            return response;
        }
        public BaseResponse<DcNurseingPlanEval> saveNurseingPlanEval(DcNurseingPlanEval request)
        {
            return base.Save<DC_NURSEINGPLANEVAL, DcNurseingPlanEval>(request, (q) => q.ID == request.Id);
        }
        public BaseResponse<DcNurseingPlanEval> deleteNurseingPlanEval(DcNurseingPlanEval request)
        {
            request.DelDate = DateTime.Now;
            request.DelFlag = "1";
            return base.Save<DC_NURSEINGPLANEVAL, DcNurseingPlanEval>(request, (q) => q.ID == request.Id);
        }
        #endregion

        #region 量表评估
        public BaseResponse<IList<EVALQUESTION>> GetREGQuetionList(long Id)
        {
            BaseResponse<IList<EVALQUESTION>> response = new BaseResponse<IList<EVALQUESTION>>();
            Mapper.CreateMap<DC_EVALQUESTION, EVALQUESTION>();
            var q = from m in unitOfWork.GetRepository<DC_EVALQUESTION>().dbSet
                    select m;
            if (Id > 0)
                q = q.Where(m => m.ID == Id);
            q = q.OrderByDescending(m => m.EVALDATE);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<EVALQUESTION>>(q.ToList());
            }
            return response;
        }

        public BaseResponse<QUESTION> GetQuetionByCode(string Code, long? recordId)
        {
            BaseResponse<QUESTION> response = new BaseResponse<QUESTION>();
            LTC_QUESTION currentQuestion = unitOfWork.GetRepository<LTC_QUESTION>().dbSet.Where(x => x.CODE == Code && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            if (currentQuestion == null)
            {
                response.ResultCode = 2;
                response.ResultMessage = "请先维护" + Code + "评估表单数据!";
                return response;
            }
            response = GetQuetion(currentQuestion.QUESTIONID, recordId);

            return response;
        }

        public BaseResponse<QUESTION> GetQuetion(int qId, long? recordId)
        {
            var response = new BaseResponse<QUESTION>();
            List<DC_EVALQUESTIONRESULT> regQuetionData = new List<DC_EVALQUESTIONRESULT>();
            LTC_QUESTION question = unitOfWork.GetRepository<LTC_QUESTION>().dbSet.Where(m => m.QUESTIONID == qId).FirstOrDefault();
            List<LTC_MAKERITEM> makerItem = unitOfWork.GetRepository<LTC_MAKERITEM>().dbSet.Where(m => m.QUESTIONID == qId).ToList();
            List<LTC_QUESTIONRESULTS> questionResult = unitOfWork.GetRepository<LTC_QUESTIONRESULTS>().dbSet.Where(m => m.QUESTIONID == qId).ToList();
            List<QuestionResult> QuestionResult = (from x in questionResult.AsEnumerable()
                                                   select new QuestionResult
                                                   {
                                                       RESULTID = x.RESULTID,
                                                       QUESTIONID = x.QUESTIONID,
                                                       LOWBOUND = x.LOWBOUND,
                                                       UPBOUND = x.UPBOUND,
                                                       RESULTNAME = x.RESULTNAME
                                                   }).ToList();

            if (recordId.HasValue && recordId.Value > 0)
            {
                regQuetionData = unitOfWork.GetRepository<DC_EVALQUESTIONRESULT>().dbSet.Where(m => m.RECORDID == recordId.Value).ToList();
            }
            List<MakerItemCollection> MakerItemList = (from x in makerItem.AsEnumerable()
                                                       select new MakerItemCollection
                                                       {
                                                           MAKERID = x.MAKERID,
                                                           MAKENAME = x.MAKENAME,
                                                           SHOWNUMBER = x.SHOWNUMBER,
                                                           ISSHOW = x.ISSHOW,
                                                           QUESTIONID = x.QUESTIONID,
                                                           DATATYPE = x.DATATYPE,
                                                           LIMITEDID = x.LIMITEDID,
                                                           CATEGORY = x.CATEGORY,
                                                           Answers = GetMakerItemValue(x.LIMITEDID),
                                                           LIMITEDVALUEID = GetLimitedValueId(recordId, x.MAKERID, regQuetionData)
                                                       }).ToList();

            Mapper.CreateMap<LTC_QUESTION, QUESTION>();
            QUESTION result = Mapper.Map<QUESTION>(question);
            result.MakerItemList = MakerItemList;
            result.QuestionResult = QuestionResult;
            response.Data = result;
            return response;
        }
        public List<MakerItemValue> GetMakerItemValue(int? limitedId)
        {
            if (limitedId.HasValue)
            {
                List<LTC_MAKERITEMLIMITEDVALUE> list = unitOfWork.GetRepository<LTC_MAKERITEMLIMITEDVALUE>().dbSet.Where(m => m.LIMITEDID == limitedId.Value).ToList();
                List<MakerItemValue> result = (from x in list.AsEnumerable()
                                               select new MakerItemValue
                                               {
                                                   ISDEFAULT = x.ISDEFAULT,
                                                   LIMITEDID = x.LIMITEDID,
                                                   LIMITEDVALUE = x.LIMITEDVALUE,
                                                   LIMITEDVALUEID = x.LIMITEDVALUEID,
                                                   LIMITEDVALUENAME = x.LIMITEDVALUENAME,
                                                   SHOWNUMBER = x.SHOWNUMBER
                                               }).ToList();


                return result;
            }
            return null;
        }
        public int? GetLimitedValueId(long? recordId, int makerId, List<DC_EVALQUESTIONRESULT> regQuetionData)
        {
            if (recordId.HasValue && recordId.Value > 0)
            {
                var item = regQuetionData.AsEnumerable().Where(x => x.MAKERID == makerId).FirstOrDefault();
                if (item != null)
                {
                    return item.LIMITEDVALUEID;
                }
            }
            return null;
        }
        public BaseResponse SaveQuetion(EVALQUESTION request)
        {
            var response = new BaseResponse();

            Mapper.CreateMap<EVALQUESTION, DC_EVALQUESTION>();

            var model = unitOfWork.GetRepository<DC_EVALQUESTION>().dbSet.Where(m => m.RECORDID == request.RECORDID).FirstOrDefault();
            if (model == null)
            {
                model = Mapper.Map<DC_EVALQUESTION>(request);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                unitOfWork.GetRepository<DC_EVALQUESTION>().Insert(model);
                unitOfWork.Save();
                Mapper.CreateMap<EVALQUESTIONRESULT, DC_EVALQUESTIONRESULT>();

                List<DC_EVALQUESTIONRESULT> questionData = Mapper.Map<List<DC_EVALQUESTIONRESULT>>(request.QuestionDataList);
                questionData.ForEach(p => p.RECORDID = model.RECORDID);
                unitOfWork.GetRepository<DC_EVALQUESTIONRESULT>().InsertRange(questionData);
            }
            else
            {
                Mapper.Map(request, model);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                unitOfWork.GetRepository<DC_EVALQUESTION>().Update(model);

                List<DC_EVALQUESTIONRESULT> regQueData = unitOfWork.GetRepository<DC_EVALQUESTIONRESULT>().dbSet.Where(m => m.RECORDID == request.RECORDID).ToList();
                regQueData.ForEach(p => unitOfWork.GetRepository<DC_EVALQUESTIONRESULT>().Delete(p));

                Mapper.CreateMap<EVALQUESTIONRESULT, DC_EVALQUESTIONRESULT>();
                List<DC_EVALQUESTIONRESULT> questionData = Mapper.Map<List<DC_EVALQUESTIONRESULT>>(request.QuestionDataList);
                questionData.ForEach(p => p.RECORDID = model.RECORDID);
                unitOfWork.GetRepository<DC_EVALQUESTIONRESULT>().InsertRange(questionData);

            }
            unitOfWork.Save();

            return response;
        }
        public BaseResponse<EVALQUESTION> GetREGQuetion(long recordId)
        {
            var response = new BaseResponse<EVALQUESTION>();
            EVALQUESTION RegQuestion = new EVALQUESTION();
            Mapper.CreateMap<DC_EVALQUESTION, EVALQUESTION>();
            DC_EVALQUESTION regQuestion = unitOfWork.GetRepository<DC_EVALQUESTION>().dbSet.Where(m => m.RECORDID == recordId).FirstOrDefault();
            Mapper.Map(regQuestion, RegQuestion);
            response.Data = RegQuestion;
            return response;
        }
        public string GetREGQuetionScore(List<int> l)
        {
            try
            {
                var q = from m in unitOfWork.GetRepository<LTC_MAKERITEMLIMITEDVALUE>().dbSet
                        where l.Contains(m.LIMITEDID)
                        select m;
                var score = q.Sum(p => p.LIMITEDVALUE);
                return score.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region 照顾计画
        public string QueryDCCplAction(long Id, long feeNo)
        {
            var result = "0";
            BaseResponse<IList<DC_RegCpl>> response = new BaseResponse<IList<DC_RegCpl>>();
            BaseResponse<IList<DC_RegCpl>> response1 = new BaseResponse<IList<DC_RegCpl>>();
            response = QueryCurrentRegCpl(new BaseRequest<DC_RegCplFilter> { Data = new DC_RegCplFilter { FeeNo = feeNo, Id = Id } });
            response1 = QueryCurrentRegCpl1(new BaseRequest<DC_RegCplFilter> { Data = new DC_RegCplFilter { FeeNo = feeNo, Id = Id, FinishFlag = false } });
            if (response.RecordsCount > 0)
            {
                if (response1.RecordsCount > 0)
                {
                    result = "1";
                }
            }
            else
            {
                result = "2";
            }

            return result;
        }
        public BaseResponse<IList<DC_CarePlanProblem>> QueryCarePlanProblem(BaseRequest<DC_CarePlanProblemFilter> request)
        {
            BaseResponse<IList<DC_CarePlanProblem>> response = new BaseResponse<IList<DC_CarePlanProblem>>();
            Mapper.CreateMap<DC_CAREPLANPROBLEM, DC_CarePlanProblem>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANPROBLEM>().dbSet
                    select new DC_CarePlanProblem
                    {
                        CpNo = m.CPNO,
                        ProblemType = m.DIAPR,
                        MajorType = m.CATEGORYTYPE
                    };

            if (!string.IsNullOrEmpty(request.Data.MajorType))
            {
                q = q.Where(m => m.MajorType == request.Data.MajorType);
            }
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<DC_CarePlanProblem>>(q.ToList());
            }
            else
            {
                response.Data = new List<DC_CarePlanProblem>
                {

                };
            }
            return response;
        }
        public BaseResponse<IList<DC_CarePlanDia>> QueryCarePlanDia(BaseRequest<DC_CarePlanDiaFilter> request)
        {
            BaseResponse<IList<DC_CarePlanDia>> response = new BaseResponse<IList<DC_CarePlanDia>>();
            Mapper.CreateMap<DC_CAREPLANREASON, DC_CarePlanDia>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANREASON>().dbSet
                    select m;
            q = q.Where(m => m.CPNO == request.Data.CpNo);
            q = q.OrderBy(m => m.CRNO);
            response.RecordsCount = q.Count();

            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<DC_CarePlanDia>>(q.ToList());
            }
            return response;
        }
        public BaseResponse<IList<DC_CarePlanActivity>> QueryCarePlanActivity(BaseRequest<DC_CarePlanActivityFilter> request)
        {
            BaseResponse<IList<DC_CarePlanActivity>> response = new BaseResponse<IList<DC_CarePlanActivity>>();
            Mapper.CreateMap<DC_CAREPLANACTIVITY, DC_CarePlanActivity>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANACTIVITY>().dbSet
                    select m;
            q = q.Where(m => m.CPNO == request.Data.CpNo);
            q = q.OrderByDescending(m => m.CPNO);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<DC_CarePlanActivity>>(q.ToList());
            }
            return response;
        }
        public BaseResponse<IList<DC_RegCpl>> QueryCurrentRegCpl(BaseRequest<DC_RegCplFilter> request)
        {
            BaseResponse<IList<DC_RegCpl>> response = new BaseResponse<IList<DC_RegCpl>>();
            Mapper.CreateMap<DC_REGCPL, DC_RegCpl>();
            var q = from m in unitOfWork.GetRepository<DC_REGCPL>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.Where(m => m.ID == request.Data.Id);
            q = q.OrderByDescending(m => m.ID);
            if (q.Count() > 0)
            {
                response.RecordsCount = q.Count();
                response.Data = Mapper.Map<IList<DC_RegCpl>>(q.ToList());
            }
            else
            {
                response.RecordsCount = 0;
                response.Data = new List<DC_RegCpl>() { };
            }
            return response;
        }
        public BaseResponse<IList<DC_RegCpl>> QueryCurrentRegCpl1(BaseRequest<DC_RegCplFilter> request)
        {
            BaseResponse<IList<DC_RegCpl>> response = new BaseResponse<IList<DC_RegCpl>>();
            Mapper.CreateMap<DC_REGCPL, DC_RegCpl>();
            var q = from m in unitOfWork.GetRepository<DC_REGCPL>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.Where(m => m.ID == request.Data.Id);
            q = q.Where(m => m.FINISHFLAG == request.Data.FinishFlag);
            q = q.OrderByDescending(m => m.ID);
            if (q.Count() > 0)
            {
                response.RecordsCount = q.Count();
                response.Data = Mapper.Map<IList<DC_RegCpl>>(q.ToList());
            }
            else
            {
                response.RecordsCount = 0;
                response.Data = new List<DC_RegCpl>() { };
            }
            return response;
        }
        public BaseResponse<DC_RegCpl> saveRegCpl(DC_RegCpl request)
        {
            return base.Save<DC_REGCPL, DC_RegCpl>(request, (q) => q.SEQNO == request.SeqNo);
        }
        public BaseResponse<List<DC_RegCpl>> saveRegCplEval(List<DC_RegCpl> requestList)
        {
            BaseResponse<List<DC_RegCpl>> response = new BaseResponse<List<DC_RegCpl>>();
            Mapper.CreateMap<DC_RegCpl, DC_REGCPL>();
            requestList.ForEach(p =>
            {
                var model = Mapper.Map<DC_REGCPL>(p);
                unitOfWork.GetRepository<DC_REGCPL>().Update(model);

            });
            unitOfWork.Save();
            response.Data = requestList;
            return response;
        }
        #region 静态数据加载
        public BaseResponse<IList<CAREPLANPROBLEM>> QueryLevelPr(string categoryType)
        {
            BaseResponse<IList<CAREPLANPROBLEM>> response = new BaseResponse<IList<CAREPLANPROBLEM>>();
            Mapper.CreateMap<DC_CAREPLANPROBLEM, CAREPLANPROBLEM>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANPROBLEM>().dbSet
                    select new
                    {
                        LevelPr = m.LEVELPR,
                        OrgId = m.ORGID,
                        CategoryType = m.CATEGORYTYPE,
                    };
            //q = q.Where(m => m.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
            q = q.Where(m => m.CategoryType == categoryType);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<CAREPLANPROBLEM>();
                var oldItem = new CAREPLANPROBLEM();
                foreach (dynamic item in list)
                {

                    CAREPLANPROBLEM newItem = Mapper.DynamicMap<CAREPLANPROBLEM>(item);
                    if (oldItem.LevelPr == newItem.LevelPr)
                    {
                        continue;
                    }
                    response.Data.Add(newItem);
                    oldItem = newItem;
                }

            };
            mapperResponse(q.ToList());
            return response;
        }

        public BaseResponse<IList<CAREPLANPROBLEM>> QueryDiaPr(string levelPr, string categoryType)
        {
            BaseResponse<IList<CAREPLANPROBLEM>> response = new BaseResponse<IList<CAREPLANPROBLEM>>();
            Mapper.CreateMap<DC_CAREPLANPROBLEM, CAREPLANPROBLEM>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANPROBLEM>().dbSet
                    select new CAREPLANPROBLEM
                    {

                        CpNo = m.CPNO,
                        LevelPr = m.LEVELPR,
                        DiaPr = m.DIAPR,
                        CategoryType = m.CATEGORYTYPE,
                        OrgId=m.ORGID,
                    };
            q = q.Where(m => m.LevelPr == levelPr);
            //q = q.Where(m => m.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
            q = q.Where(m => m.CategoryType == categoryType);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<CAREPLANPROBLEM>>(q.ToList());
            }
            else
            {
                response.Data = new List<CAREPLANPROBLEM>
                {

                };
            }
            return response;
        }

        public BaseResponse<IList<CAREPLANREASON>> QueryCausep(int? cpNo)
        {
            BaseResponse<IList<CAREPLANREASON>> response = new BaseResponse<IList<CAREPLANREASON>>();
            Mapper.CreateMap<DC_CAREPLANREASON, CAREPLANREASON>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANREASON>().dbSet
                    select new CAREPLANREASON
                    {
                        CpNo = m.CPNO,
                        Causep = m.CAUSEP,
                    };
            q = q.Where(m => m.CpNo == cpNo);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<CAREPLANREASON>>(q.ToList());
            }
            else
            {
                response.Data = new List<CAREPLANREASON>
                {

                };
            }
            return response;
        }

        public BaseResponse<IList<CAREPLANDATA>> QueryPrData(int? cpNo)
        {
            BaseResponse<IList<CAREPLANDATA>> response = new BaseResponse<IList<CAREPLANDATA>>();
            Mapper.CreateMap<DC_CAREPLANDATA, CAREPLANDATA>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANDATA>().dbSet
                    select new CAREPLANDATA
                    {
                        CpNo = m.CPNO,
                        PrData = m.PRDATA,
                    };
            q = q.Where(m => m.CpNo == cpNo);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<CAREPLANDATA>>(q.ToList());
            }
            else
            {
                response.Data = new List<CAREPLANDATA>
                {

                };
            }
            return response;
        }

        public BaseResponse<IList<CAREPLANGOAL>> QueryGoalp(string dirPr)
        {
            BaseResponse<IList<CAREPLANGOAL>> response = new BaseResponse<IList<CAREPLANGOAL>>();
            Mapper.CreateMap<DC_CAREPLANGOAL, CAREPLANGOAL>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANGOAL>().dbSet
                    select new CAREPLANGOAL
                    {
                        CpNo = m.CPNO,
                        DiaPr = m.DIAPR,
                        Goalp = m.GOALP,
                    };
            q = q.Where(m => m.DiaPr == dirPr);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<CAREPLANGOAL>>(q.ToList());
            }
            else
            {
                response.Data = new List<CAREPLANGOAL>
                {

                };
            }
            return response;
        }

        public BaseResponse<IList<CAREPLANACTIVITY>> QueryActivity(string dirPr)
        {
            BaseResponse<IList<CAREPLANACTIVITY>> response = new BaseResponse<IList<CAREPLANACTIVITY>>();
            Mapper.CreateMap<DC_CAREPLANACTIVITY, CAREPLANACTIVITY>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANACTIVITY>().dbSet
                    select new CAREPLANACTIVITY
                    {
                        CpNo = m.CPNO,
                        DiaPr = m.DIAPR,
                        Activity = m.ACTIVITY,
                    };
            q = q.Where(m => m.DiaPr == dirPr);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<CAREPLANACTIVITY>>(q.ToList());
            }
            else
            {
                response.Data = new List<CAREPLANACTIVITY>
                {

                };
            }
            return response;
        }

        public BaseResponse<IList<CAREPLANEVAL>> QueryAssessvalue(string dirPr)
        {
            BaseResponse<IList<CAREPLANEVAL>> response = new BaseResponse<IList<CAREPLANEVAL>>();
            Mapper.CreateMap<DC_CAREPLANEVAL, CAREPLANEVAL>();
            var q = from m in unitOfWork.GetRepository<DC_CAREPLANEVAL>().dbSet
                    select new CAREPLANEVAL
                    {
                        CpNo = m.CPNO,
                        DiaPr = m.DIAPR,
                        Assessvalue = m.ASSESSVALUE,
                    };
            q = q.Where(m => m.DiaPr == dirPr);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<CAREPLANEVAL>>(q.ToList());
            }
            else
            {
                response.Data = new List<CAREPLANEVAL>
                {

                };
            }
            return response;
        }
        #endregion
        #region 目标
        public BaseResponse<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL>> QueryNsCplGoalp(long seqNo)
        {
            BaseResponse<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL>> response = new BaseResponse<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL>>();
            Mapper.CreateMap<DC_NSCPLGOAL, KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL>();
            var q = from m in unitOfWork.GetRepository<DC_NSCPLGOAL>().dbSet
                    select m;
            q = q.Where(m => m.SEQNO == seqNo);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL>>(q.ToList());
            }
            else
            {
                response.Data = new List<KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL>
                {

                };
            }
            return response;
        }
        public BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL> saveNsCplGoalp(KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL request)
        {
            return base.Save<DC_NSCPLGOAL, KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL>(request, (q) => q.ID == request.Id);
        }
        #endregion
        #region 措施
        public BaseResponse<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY>> QueryNsCplActivity(long seqNo)
        {
            BaseResponse<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY>> response = new BaseResponse<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY>>();
            Mapper.CreateMap<DC_NSCPLACTIVITY, KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY>();
            var q = from m in unitOfWork.GetRepository<DC_NSCPLACTIVITY>().dbSet
                    select m;
            q = q.Where(m => m.SEQNO == seqNo);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY>>(q.ToList());
            }
            else
            {
                response.Data = new List<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY>
                {

                };
            }
            return response;
        }
        public BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY> saveNsCplActivity(KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY request)
        {
            return base.Save<DC_NSCPLACTIVITY, KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY>(request, (q) => q.ID == request.Id);
        }
        #endregion
        #region 评值
        public BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE> QueryAssessValue(long seqNo)
        {
            BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE> response = new BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE>();
            Mapper.CreateMap<DC_ASSESSVALUE, KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE>();
            var q = from m in unitOfWork.GetRepository<DC_ASSESSVALUE>().dbSet
                    select m;
            q = q.Where(m => m.SEQNO == seqNo);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE>>(q.ToList())[0];
            }
            else
            {
                response.Data = new KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE
                {

                };
            }
            return response;
        }
        public BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE> saveAssessValue(KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE request)
        {
            return base.Save<DC_ASSESSVALUE, KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE>(request, (q) => q.ID == request.Id);
        }
        #endregion

        #endregion

        #region 个别化活动需求评估及计画
        public BaseResponse<DC_RegActivityRequestEval> QueryCurrentRegActivityRequestEval(BaseRequest<DC_RegActivityRequestEvalFilter> request)
        {
            BaseResponse<DC_RegActivityRequestEval> response = new BaseResponse<DC_RegActivityRequestEval>();
            Mapper.CreateMap<DC_REGACTIVITYREQUESTEVAL, DC_RegActivityRequestEval>();
            var q = from m in unitOfWork.GetRepository<DC_REGACTIVITYREQUESTEVAL>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.Where(m => m.DELFLAG == false);
            q = q.OrderByDescending(m => m.EVALDATE);
            if (q.Count() > 0)
            {
                response.Data = Mapper.Map<IList<DC_RegActivityRequestEval>>(q.ToList())[0];
            }
            else
            {
                var regFmodel = base.Get<DC_REGFILE, DC_RegFileModel>((m) => m.REGNO == request.Data.RegNo).Data ?? new DC_RegFileModel { };
                var npEvalmodel = QueryCurrentDcNurseingPlanEval(new BaseRequest<DcNurseingPlanEvalFilter> { Data = new DcNurseingPlanEvalFilter { FeeNo = request.Data.FeeNo } }).Data ?? new DcNurseingPlanEval { };
                var AdlEvalModel = QuerySheetEval(request.Data.FeeNo, "ADL", SecurityHelper.CurrentPrincipal.OrgId).Data;
                var _adlItemResult = "";
                if (AdlEvalModel.QUESTIONID != null)
                {
                    var AdlqAnsw = GetQuetion(Convert.ToInt32(AdlEvalModel.QUESTIONID), AdlEvalModel.RECORDID).Data;
                    if (AdlqAnsw != null)
                    {
                        for (var i = 0; i < AdlqAnsw.MakerItemList.Count; i++)
                        {
                            decimal? maxLimitedValue = 0;
                            decimal? tempLimitedValue = 0;
                            decimal? currentLimitedValue = 0;
                            for (var j = 0; j < AdlqAnsw.MakerItemList[i].Answers.Count; j++)
                            {
                                tempLimitedValue = AdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                                if (maxLimitedValue < tempLimitedValue)
                                {
                                    maxLimitedValue = tempLimitedValue;
                                }
                                if (AdlqAnsw.MakerItemList[i].LIMITEDVALUEID == AdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUEID)
                                {
                                    currentLimitedValue = AdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                                }
                            }
                            if (maxLimitedValue != currentLimitedValue)
                            {
                                _adlItemResult = _adlItemResult + " " + AdlqAnsw.MakerItemList[i].MAKENAME;
                            }
                        }
                    }

                    if (AdlEvalModel.SCORE >= 80)
                    {
                        AdlEvalModel.EVALRESULT = "轻";
                    }
                    else if (AdlEvalModel.SCORE >= 60 && AdlEvalModel.SCORE < 80)
                    {
                        AdlEvalModel.EVALRESULT = "中";
                    }
                    else if (AdlEvalModel.SCORE < 60)
                    {
                        AdlEvalModel.EVALRESULT = "重";
                    }
                }

                var iAdlEvalModel = QuerySheetEval(request.Data.FeeNo, "IADL", SecurityHelper.CurrentPrincipal.OrgId).Data;

                var _iadlItemResult = "";
                if (iAdlEvalModel.QUESTIONID != null)
                {
                    var iAdlqAnsw = GetQuetion(Convert.ToInt32(iAdlEvalModel.QUESTIONID), iAdlEvalModel.RECORDID).Data;
                    if (iAdlqAnsw != null)
                    {
                        for (var i = 0; i < iAdlqAnsw.MakerItemList.Count; i++)
                        {
                            decimal? maxLimitedValue = 0;
                            decimal? tempLimitedValue = 0;
                            decimal? currentLimitedValue = 0;
                            for (var j = 0; j < iAdlqAnsw.MakerItemList[i].Answers.Count; j++)
                            {
                                tempLimitedValue = iAdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                                if (maxLimitedValue < tempLimitedValue)
                                {
                                    maxLimitedValue = tempLimitedValue;
                                }
                                if (iAdlqAnsw.MakerItemList[i].LIMITEDVALUEID == iAdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUEID)
                                {
                                    currentLimitedValue = iAdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                                }
                            }
                            if (maxLimitedValue != currentLimitedValue)
                            {
                                _iadlItemResult = _iadlItemResult + " " + iAdlqAnsw.MakerItemList[i].MAKENAME;
                            }
                        }
                    }

                    //if (iAdlEvalModel.SCORE >= 80)
                    //{
                    //    iAdlEvalModel.EVALRESULT = "轻";
                    //}
                    //else if (iAdlEvalModel.SCORE >= 60 && iAdlEvalModel.SCORE < 80)
                    //{
                    //    iAdlEvalModel.EVALRESULT = "中";
                    //}
                    //else if (iAdlEvalModel.SCORE < 60)
                    //{
                    //    iAdlEvalModel.EVALRESULT = "重";
                    //}
                }


                var MmseEvalModel = QuerySheetEval(request.Data.FeeNo, "MMSE", SecurityHelper.CurrentPrincipal.OrgId).Data;
                var _mmseItemResult = "";
                if (MmseEvalModel.QUESTIONID != null)
                {
                    var MmseqAnsw = GetQuetion(Convert.ToInt32(MmseEvalModel.QUESTIONID), MmseEvalModel.RECORDID).Data;
                    if (MmseqAnsw != null)
                    {
                        for (var i = 0; i < MmseqAnsw.MakerItemList.Count; i++)
                        {
                            decimal? maxLimitedValue = 0;
                            decimal? tempLimitedValue = 0;
                            decimal? currentLimitedValue = 0;
                            for (var j = 0; j < MmseqAnsw.MakerItemList[i].Answers.Count; j++)
                            {
                                tempLimitedValue = MmseqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                                if (maxLimitedValue < tempLimitedValue)
                                {
                                    maxLimitedValue = tempLimitedValue;
                                }
                                if (MmseqAnsw.MakerItemList[i].LIMITEDVALUEID == MmseqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUEID)
                                {
                                    currentLimitedValue = MmseqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                                }
                            }
                            if (maxLimitedValue != currentLimitedValue)
                            {
                                _mmseItemResult = _mmseItemResult + " " + MmseqAnsw.MakerItemList[i].CATEGORY;
                            }
                        }
                    }

                    if (MmseEvalModel.SCORE >= 20)
                    {
                        MmseEvalModel.EVALRESULT = "轻";
                    }
                    else if (MmseEvalModel.SCORE >= 6 && MmseEvalModel.SCORE < 20)
                    {
                        MmseEvalModel.EVALRESULT = "中";
                    }
                    else if (MmseEvalModel.SCORE < 6)
                    {
                        MmseEvalModel.EVALRESULT = "重";
                    }
                }

                var GdsEvalModel = QuerySheetEval(request.Data.FeeNo, "GDS", SecurityHelper.CurrentPrincipal.OrgId).Data;
                var _gdsItemResult = "";
                if (GdsEvalModel.QUESTIONID != null)
                {
                    var GdsqAnsw = GetQuetion(Convert.ToInt32(GdsEvalModel.QUESTIONID), GdsEvalModel.RECORDID).Data;
                    if (GdsqAnsw != null)
                    {
                        for (var i = 0; i < GdsqAnsw.MakerItemList.Count; i++)
                        {
                            decimal? maxLimitedValue = 0;
                            decimal? tempLimitedValue = 0;
                            decimal? currentLimitedValue = 0;
                            for (var j = 0; j < GdsqAnsw.MakerItemList[i].Answers.Count; j++)
                            {
                                if (GdsqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE >= 98)
                                {
                                    continue;
                                }
                                tempLimitedValue = GdsqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                                if (maxLimitedValue < tempLimitedValue)
                                {
                                    maxLimitedValue = tempLimitedValue;
                                }
                                if (GdsqAnsw.MakerItemList[i].LIMITEDVALUEID == GdsqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUEID)
                                {
                                    currentLimitedValue = GdsqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                                }
                            }
                            if (maxLimitedValue != currentLimitedValue)
                            {
                                _gdsItemResult = _gdsItemResult + " " + GdsqAnsw.MakerItemList[i].MAKENAME;
                            }
                        }
                    }

                    //if (MmseEvalModel.SCORE >= 20)
                    //{
                    //    MmseEvalModel.EVALRESULT = "轻";
                    //}
                    //else if (MmseEvalModel.SCORE >= 6 && MmseEvalModel.SCORE < 20)
                    //{
                    //    MmseEvalModel.EVALRESULT = "中";
                    //}
                    //else if (MmseEvalModel.SCORE < 6)
                    //{
                    //    MmseEvalModel.EVALRESULT = "重";
                    //}
                }
                response.Data = new DC_RegActivityRequestEval
                {
                    Education = regFmodel.Education ?? "",
                    MerryState = regFmodel.MerryState ?? "",
                    Profession = regFmodel.Profession ?? "",
                    Language = regFmodel.Language ?? "",
                    Visualacuity = npEvalmodel.VisualAcuity ?? "",
                    ListeningState = npEvalmodel.ListeningState ?? "",
                    Delusion = npEvalmodel.Delusion ?? "",
                    Visualillusion = "",
                    EmotionState = npEvalmodel.EmotionState ?? "",
                    ProblemBehavior = npEvalmodel.ProblemBehavior ?? "",

                    AdlScore = Math.Round(AdlEvalModel.SCORE??0).ToString() ?? "",
                    AdlItemResult = _adlItemResult,
                    AdlResult = AdlEvalModel.EVALRESULT ?? "",
                    IadlScore =  Math.Round(iAdlEvalModel.SCORE??0).ToString() ?? "",
                    IadlItemResult = _iadlItemResult,
                    IadlResult = iAdlEvalModel.EVALRESULT ?? "",
                    MmseScore = Math.Round(MmseEvalModel.SCORE??0).ToString() ?? "",
                    MmseItemResult = _mmseItemResult,
                    MmseResult =MmseEvalModel.EVALRESULT ?? "",
                    GdsScore = Math.Round(GdsEvalModel.SCORE??0).ToString() ?? "",
                    GdsItemResult = _gdsItemResult,
                    GdsResult = GdsEvalModel.EVALRESULT ?? "",

                };
            }
            return response;
        }
        public BaseResponse<DC_RegActivityRequestEval> QueryPartRegActivityRequestEval(BaseRequest<DC_RegActivityRequestEvalFilter> request)
        {
            BaseResponse<DC_RegActivityRequestEval> response = new BaseResponse<DC_RegActivityRequestEval>();
            var regFmodel = base.Get<DC_REGFILE, DC_RegFileModel>((m) => m.REGNO == request.Data.RegNo).Data ?? new DC_RegFileModel { };
            var npEvalmodel = QueryCurrentDcNurseingPlanEval(new BaseRequest<DcNurseingPlanEvalFilter> { Data = new DcNurseingPlanEvalFilter { FeeNo = request.Data.FeeNo } }).Data ?? new DcNurseingPlanEval { };
            var AdlEvalModel = QuerySheetEval(request.Data.FeeNo, "ADL", SecurityHelper.CurrentPrincipal.OrgId).Data;
            var _adlItemResult = "";
            if (AdlEvalModel.QUESTIONID != null)
            {
                var AdlqAnsw = GetQuetion(Convert.ToInt32(AdlEvalModel.QUESTIONID), AdlEvalModel.RECORDID).Data;
                if (AdlqAnsw != null)
                {
                    for (var i = 0; i < AdlqAnsw.MakerItemList.Count; i++)
                    {
                        decimal? maxLimitedValue = 0;
                        decimal? tempLimitedValue = 0;
                        decimal? currentLimitedValue = 0;
                        for (var j = 0; j < AdlqAnsw.MakerItemList[i].Answers.Count; j++)
                        {
                            tempLimitedValue = AdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                            if (maxLimitedValue < tempLimitedValue)
                            {
                                maxLimitedValue = tempLimitedValue;
                            }
                            if (AdlqAnsw.MakerItemList[i].LIMITEDVALUEID == AdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUEID)
                            {
                                currentLimitedValue = AdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                            }
                        }
                        if (maxLimitedValue != currentLimitedValue)
                        {
                            _adlItemResult = _adlItemResult + " " + AdlqAnsw.MakerItemList[i].MAKENAME;
                        }
                    }
                }

                if (AdlEvalModel.SCORE >= 80)
                {
                    AdlEvalModel.EVALRESULT = "轻";
                }
                else if (AdlEvalModel.SCORE >= 60 && AdlEvalModel.SCORE < 80)
                {
                    AdlEvalModel.EVALRESULT = "中";
                }
                else if (AdlEvalModel.SCORE < 60)
                {
                    AdlEvalModel.EVALRESULT = "重";
                }
            }

            var iAdlEvalModel = QuerySheetEval(request.Data.FeeNo, "IADL", SecurityHelper.CurrentPrincipal.OrgId).Data;

            var _iadlItemResult = "";
            if (iAdlEvalModel.QUESTIONID != null)
            {
                var iAdlqAnsw = GetQuetion(Convert.ToInt32(iAdlEvalModel.QUESTIONID), iAdlEvalModel.RECORDID).Data;
                if (iAdlqAnsw != null)
                {
                    for (var i = 0; i < iAdlqAnsw.MakerItemList.Count; i++)
                    {
                        decimal? maxLimitedValue = 0;
                        decimal? tempLimitedValue = 0;
                        decimal? currentLimitedValue = 0;
                        for (var j = 0; j < iAdlqAnsw.MakerItemList[i].Answers.Count; j++)
                        {
                            tempLimitedValue = iAdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                            if (maxLimitedValue < tempLimitedValue)
                            {
                                maxLimitedValue = tempLimitedValue;
                            }
                            if (iAdlqAnsw.MakerItemList[i].LIMITEDVALUEID == iAdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUEID)
                            {
                                currentLimitedValue = iAdlqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                            }
                        }
                        if (maxLimitedValue != currentLimitedValue)
                        {
                            _iadlItemResult = _iadlItemResult + " " + iAdlqAnsw.MakerItemList[i].MAKENAME;
                        }
                    }
                }

                //if (iAdlEvalModel.SCORE >= 80)
                //{
                //    iAdlEvalModel.EVALRESULT = "轻";
                //}
                //else if (iAdlEvalModel.SCORE >= 60 && iAdlEvalModel.SCORE < 80)
                //{
                //    iAdlEvalModel.EVALRESULT = "中";
                //}
                //else if (iAdlEvalModel.SCORE < 60)
                //{
                //    iAdlEvalModel.EVALRESULT = "重";
                //}
            }


            var MmseEvalModel = QuerySheetEval(request.Data.FeeNo, "MMSE", SecurityHelper.CurrentPrincipal.OrgId).Data;
            var _mmseItemResult = "";
            if (MmseEvalModel.QUESTIONID != null)
            {
                var MmseqAnsw = GetQuetion(Convert.ToInt32(MmseEvalModel.QUESTIONID), MmseEvalModel.RECORDID).Data;
                if (MmseqAnsw != null)
                {
                    for (var i = 0; i < MmseqAnsw.MakerItemList.Count; i++)
                    {
                        decimal? maxLimitedValue = 0;
                        decimal? tempLimitedValue = 0;
                        decimal? currentLimitedValue = 0;
                        for (var j = 0; j < MmseqAnsw.MakerItemList[i].Answers.Count; j++)
                        {
                            tempLimitedValue = MmseqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                            if (maxLimitedValue < tempLimitedValue)
                            {
                                maxLimitedValue = tempLimitedValue;
                            }
                            if (MmseqAnsw.MakerItemList[i].LIMITEDVALUEID == MmseqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUEID)
                            {
                                currentLimitedValue = MmseqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                            }
                        }
                        if (maxLimitedValue != currentLimitedValue)
                        {
                            _mmseItemResult = _mmseItemResult + " " + MmseqAnsw.MakerItemList[i].MAKENAME;
                        }
                    }
                }

                if (MmseEvalModel.SCORE >= 20)
                {
                    MmseEvalModel.EVALRESULT = "轻";
                }
                else if (MmseEvalModel.SCORE >= 6 && MmseEvalModel.SCORE < 20)
                {
                    MmseEvalModel.EVALRESULT = "中";
                }
                else if (MmseEvalModel.SCORE < 6)
                {
                    MmseEvalModel.EVALRESULT = "重";
                }
            }

            var GdsEvalModel = QuerySheetEval(request.Data.FeeNo, "GDS", SecurityHelper.CurrentPrincipal.OrgId).Data;
            var _gdsItemResult = "";
            if (GdsEvalModel.QUESTIONID != null)
            {
                var GdsqAnsw = GetQuetion(Convert.ToInt32(GdsEvalModel.QUESTIONID), GdsEvalModel.RECORDID).Data;
                if (GdsqAnsw != null)
                {
                    for (var i = 0; i < GdsqAnsw.MakerItemList.Count; i++)
                    {
                        decimal? maxLimitedValue = 0;
                        decimal? tempLimitedValue = 0;
                        decimal? currentLimitedValue = 0;
                        for (var j = 0; j < GdsqAnsw.MakerItemList[i].Answers.Count; j++)
                        {
                            if (GdsqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE >= 98)
                            {
                                continue;
                            }
                            tempLimitedValue = GdsqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                            if (maxLimitedValue < tempLimitedValue)
                            {
                                maxLimitedValue = tempLimitedValue;
                            }
                            if (GdsqAnsw.MakerItemList[i].LIMITEDVALUEID == GdsqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUEID)
                            {
                                currentLimitedValue = GdsqAnsw.MakerItemList[i].Answers[j].LIMITEDVALUE;
                            }
                        }
                        if (maxLimitedValue != currentLimitedValue)
                        {
                            _gdsItemResult = _gdsItemResult + " " + GdsqAnsw.MakerItemList[i].MAKENAME;
                        }
                    }
                }

                //if (MmseEvalModel.SCORE >= 20)
                //{
                //    MmseEvalModel.EVALRESULT = "轻";
                //}
                //else if (MmseEvalModel.SCORE >= 6 && MmseEvalModel.SCORE < 20)
                //{
                //    MmseEvalModel.EVALRESULT = "中";
                //}
                //else if (MmseEvalModel.SCORE < 6)
                //{
                //    MmseEvalModel.EVALRESULT = "重";
                //}
            }
            response.Data = new DC_RegActivityRequestEval
            {
                Education = regFmodel.Education ?? "",
                MerryState = regFmodel.MerryState ?? "",
                Profession = regFmodel.Profession ?? "",
                Language = regFmodel.Language ?? "",
                Visualacuity = npEvalmodel.VisualAcuity ?? "",
                ListeningState = npEvalmodel.ListeningState ?? "",
                Delusion = npEvalmodel.Delusion ?? "",
                Visualillusion = "",
                EmotionState = npEvalmodel.EmotionState ?? "",
                ProblemBehavior = npEvalmodel.ProblemBehavior ?? "",

                AdlScore = AdlEvalModel.SCORE.ToString() ?? "",
                AdlItemResult = _adlItemResult,
                AdlResult = AdlEvalModel.EVALRESULT ?? "",
                IadlScore = iAdlEvalModel.SCORE.ToString() ?? "",
                IadlItemResult = _iadlItemResult,
                IadlResult = iAdlEvalModel.EVALRESULT ?? "",
                MmseScore = MmseEvalModel.SCORE.ToString() ?? "",
                MmseItemResult = _mmseItemResult,
                MmseResult = MmseEvalModel.EVALRESULT ?? "",
                GdsScore = GdsEvalModel.SCORE.ToString() ?? "",
                GdsItemResult = _gdsItemResult,
                GdsResult = GdsEvalModel.EVALRESULT ?? "",

            };
            return response;
        }
        public BaseResponse<EVALQUESTION2> QuerySheetEval(long feeNo, string code, string orgId)
        {
            var response = new BaseResponse<IList<EVALQUESTION2>>();
            Mapper.CreateMap<DC_EVALQUESTION, EVALQUESTION2>();
            var q = from eq in unitOfWork.GetRepository<DC_EVALQUESTION>().dbSet
                    join que in unitOfWork.GetRepository<LTC_QUESTION>().dbSet on eq.QUESTIONCODE equals que.CODE into qu
                    from qe in qu.DefaultIfEmpty()
                    select new
                    {
                        QUESTIONID = qe.QUESTIONID,
                        REGNO = eq.REGNO,
                        RECORDID = eq.RECORDID,
                        EVALRESULT = eq.EVALRESULT,
                        SCORE = eq.SCORE,
                        FEENO = eq.FEENO,
                        CODE = qe.CODE,
                        ORGID = qe.ORGID,
                    };
            q = q.Where(m => m.FEENO == feeNo);
            q = q.Where(m => m.CODE == code);
            q = q.Where(m => m.ORGID == orgId);
            q = q.OrderByDescending(m => m.RECORDID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<EVALQUESTION2>();
                foreach (dynamic item in list)
                {
                    EVALQUESTION2 newItem = Mapper.DynamicMap<EVALQUESTION2>(item);
                    response.Data.Add(newItem);
                }

            };
            mapperResponse(q.ToList());
            return new BaseResponse<EVALQUESTION2>()
            {
                Data = response.RecordsCount > 0 ? response.Data[0] : new EVALQUESTION2 { }
            };
        }
        public BaseResponse<IList<DC_RegActivityRequestEval>> QueryRegActivityRequestEval(BaseRequest<DC_RegActivityRequestEvalFilter> request)
        {
            BaseResponse<IList<DC_RegActivityRequestEval>> response = new BaseResponse<IList<DC_RegActivityRequestEval>>();
            Mapper.CreateMap<DC_REGACTIVITYREQUESTEVAL, DC_RegActivityRequestEval>();
            var q = from m in unitOfWork.GetRepository<DC_REGACTIVITYREQUESTEVAL>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.Where(m => m.DELFLAG == false);
            q = q.OrderByDescending(m => m.EVALDATE);
            response.RecordsCount = q.Count();
            List<DC_REGACTIVITYREQUESTEVAL> list = null;
            if (response.RecordsCount > 0)
            {
                if (request != null && request.PageSize > 0)
                {
                    list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    list = q.ToList();
                }
                response.Data = Mapper.Map<IList<DC_RegActivityRequestEval>>(list);
            }
            return response;
        }
        public BaseResponse<DC_RegActivityRequestEval> saveRegActivityRequestEval(DC_RegActivityRequestEval request)
        {
            //标记为未删除
            request.DelFlag = false;
            return base.Save<DC_REGACTIVITYREQUESTEVAL, DC_RegActivityRequestEval>(request, (q) => q.ID == request.Id);
        }
        public BaseResponse<DC_RegActivityRequestEval> deleteRegActivityRequestEval(long Id)
        {
            var request = new DC_RegActivityRequestEval
            {
                DelDate = DateTime.Now,
                DelFlag = true
            };
            return base.Save<DC_REGACTIVITYREQUESTEVAL, DC_RegActivityRequestEval>(request, (q) => q.ID == Id);
        }
        #endregion

        #region 护理诊断一览表
        public BaseResponse<IList<DC_RegCpl>> QueryRegCpl(BaseRequest<DC_RegCplFilter> request)
        {
            BaseResponse<IList<DC_RegCpl>> response = new BaseResponse<IList<DC_RegCpl>>();
            Mapper.CreateMap<DC_REGCPL, DC_RegCpl>();
            var q = from m in unitOfWork.GetRepository<DC_REGCPL>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.CREATEDATE);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<DC_RegCpl>>(q.ToList());
            }
            return response;
        }
        #endregion

        #region 个案基本资料汇整表
        public BaseResponse<IList<DC_RegBaseInfoList>> QueryAllRegBaseInfoList(BaseRequest<DC_RegBaseInfoListFilter> request)
        {
            return base.Query<DC_REGBASEINFOLIST, DC_RegBaseInfoList>(request, (q) =>
            {
                q = q.Where(m => m.FEENO == request.Data.FeeNo);
                q = q.OrderByDescending(m => m.CNT);
                return q;
            });
        }
        public BaseResponse<DC_RegBaseInfoList> QueryRegBaseInfoList(BaseRequest<DC_RegBaseInfoListFilter> request)
        {
            var response = new BaseResponse<DC_RegBaseInfoList>();
            Mapper.CreateMap<DC_REGBASEINFOLIST, DC_RegBaseInfoList>();
            var q = from m in unitOfWork.GetRepository<DC_REGBASEINFOLIST>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.ID);
            if (q.Count() > 0)
            {
                response.Data = Mapper.Map<IList<DC_RegBaseInfoList>>(q.ToList())[0];
            }
            else
            {
                response.Data = null;
            }

            return response;
        }
        public BaseResponse<DC_RegBaseInfoList> QueryRegBaseInfo(BaseRequest<DC_RegBaseInfoListFilter> request)
        {
            var response = new BaseResponse<IList<DC_RegBaseInfoList>>();
            var q = from reg in unitOfWork.GetRepository<DC_REGFILE>().dbSet
                    join npe in unitOfWork.GetRepository<DC_NURSEINGPLANEVAL>().dbSet on reg.REGNO equals npe.REGNO into r_npe
                    from npe_part in r_npe.DefaultIfEmpty()
                    select new
                    {
                        Cnt = npe_part.EVALNUMBER,
                        RegNo = reg.REGNO,
                        ContactName = reg.CONTACTNAME1,
                        ContactPhone = reg.CONTACTMOBILE1,
                        Address = reg.LIVINGADDRESS,
                        Language = reg.LANGUAGE,
                        Job = reg.PROFESSION,
                        Religion = reg.RELIGION,
                        MerryState = reg.MERRYSTATE,
                        Education = reg.EDUCATION,
                        Height = npe_part.HEIGHT,
                        Weight = npe_part.WEIGHT,
                        Bmi = npe_part.BMI,
                        WaistLine = npe_part.WAISTLINE,
                        DiseaseHistory = npe_part.DISEASEINFO,
                        Questionbehavior = npe_part.PROBLEMBEHAVIOR,
                        Mark = npe_part.ID,
                    };
            q = q.Where(m => m.RegNo == request.Data.RegNo);
            q = q.Where(m => m.Cnt == request.Data.Cnt);
            q = q.OrderByDescending(m => m.Mark);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<DC_RegBaseInfoList>();
                foreach (dynamic item in list)
                {
                    DC_RegBaseInfoList newItem = Mapper.DynamicMap<DC_RegBaseInfoList>(item);
                    response.Data.Add(newItem);
                }

            };
            mapperResponse(q.ToList());
            if (response.RecordsCount > 0)
            {
                response.Data[0].AdlScore = Math.Round(QueryBaseInfoSheetEval(request.Data.FeeNo, request.Data.Cnt, "ADL", SecurityHelper.CurrentPrincipal.OrgId).Data.SCORE ?? 0, 0).ToString() ?? "";
                response.Data[0].IadlScore = Math.Round(QueryBaseInfoSheetEval(request.Data.FeeNo, request.Data.Cnt, "IADL", SecurityHelper.CurrentPrincipal.OrgId).Data.SCORE ?? 0, 0).ToString() ?? "";
                response.Data[0].MmseScore = Math.Round(QueryBaseInfoSheetEval(request.Data.FeeNo, request.Data.Cnt, "MMSE", SecurityHelper.CurrentPrincipal.OrgId).Data.SCORE ?? 0, 0).ToString() ?? "";
                response.Data[0].GdsScore = Math.Round(QueryBaseInfoSheetEval(request.Data.FeeNo, request.Data.Cnt, "GDS", SecurityHelper.CurrentPrincipal.OrgId).Data.SCORE ?? 0, 0).ToString() ?? "";
            }
            return new BaseResponse<DC_RegBaseInfoList>()
            {
                Data = response.RecordsCount > 0 ? response.Data[0] : null
            };

        }
        public BaseResponse<EVALQUESTION2> QueryBaseInfoSheetEval(long feeNo, int cnt, string code, string orgId)
        {
            var response = new BaseResponse<IList<EVALQUESTION2>>();
            Mapper.CreateMap<DC_EVALQUESTION, EVALQUESTION2>();
            var q = from eq in unitOfWork.GetRepository<DC_EVALQUESTION>().dbSet
                    join que in unitOfWork.GetRepository<LTC_QUESTION>().dbSet on eq.QUESTIONID equals que.QUESTIONID into qu
                    from qe in qu.DefaultIfEmpty()
                    select new
                    {
                        RECORDID = eq.RECORDID,
                        EVALRESULT = eq.EVALRESULT,
                        SCORE = eq.SCORE,
                        FEENO = eq.FEENO,
                        EVALNUMBER = eq.EVALNUMBER,
                        CODE = qe.CODE,
                        ORGID = qe.ORGID,
                    };
            q = q.Where(m => m.FEENO == feeNo);
            q = q.Where(m => m.EVALNUMBER == cnt);
            q = q.Where(m => m.CODE == code);
            q = q.Where(m => m.ORGID == orgId);
            q = q.OrderByDescending(m => m.RECORDID);
            response.RecordsCount = q.Count();
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<EVALQUESTION2>();
                foreach (dynamic item in list)
                {
                    EVALQUESTION2 newItem = Mapper.DynamicMap<EVALQUESTION2>(item);
                    response.Data.Add(newItem);
                }

            };
            mapperResponse(q.ToList());
            return new BaseResponse<EVALQUESTION2>()
            {
                Data = response.RecordsCount > 0 ? response.Data[0] : new EVALQUESTION2 { }
            };
        }
        public BaseResponse<IList<EVALQUESTION2>> QuerySheetEval(long feeNo,int cnt)
        {
            var response = new BaseResponse<IList<EVALQUESTION2>>();
            Mapper.CreateMap<DC_EVALQUESTION, EVALQUESTION2>();
            var q = from eq in unitOfWork.GetRepository<DC_EVALQUESTION>().dbSet
                    join que in unitOfWork.GetRepository<LTC_QUESTION>().dbSet on eq.QUESTIONID equals que.QUESTIONID into qu
                    from qe in qu.DefaultIfEmpty()
                    select new
                    {
                        QUESTIONID = eq.QUESTIONID,
                        REGNO = eq.REGNO,
                        RECORDID = eq.RECORDID,
                        EVALRESULT = eq.EVALRESULT,
                        SCORE = eq.SCORE,
                        FEENO = eq.FEENO,
                        EVALNUMBER = eq.EVALNUMBER,
                        CODE = qe.CODE,
                        ORGID = qe.ORGID,
                    };
            q = q.Where(m => m.FEENO == feeNo);
            q = q.Where(m => m.EVALNUMBER == cnt);
            q = q.OrderByDescending(m => m.RECORDID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<EVALQUESTION2>();
                foreach (dynamic item in list)
                {
                    EVALQUESTION2 newItem = Mapper.DynamicMap<EVALQUESTION2>(item);
                    response.Data.Add(newItem);
                }

            };
            mapperResponse(q.ToList());
            return response;
        }
        private string GetScore(List<EVALQUESTION2> evalQuestion, string code, string orgId)
        {
            if (evalQuestion.Where(m => m.CODE == code && m.ORGID == orgId).ToList().Count > 0)
            {
                decimal score = (evalQuestion.Where(m => m.CODE == code && m.ORGID == orgId).ToList())[0].SCORE ?? 0;
                return Math.Round(score, 0).ToString();
            }
            else
            {
                return "";
            }

        }
        public BaseResponse<DC_RegBaseInfoList> saveRegBaseInfoList(DC_RegBaseInfoList request)
        {
            return base.Save<DC_REGBASEINFOLIST, DC_RegBaseInfoList>(request, (q) => q.ID == request.Id);
        }
        #endregion

        #region 个案药品管理
        public BaseResponse<IList<DcRegMedicine>> QueryMedicine(BaseRequest<DcRegMedicine> request)
        {
            BaseResponse<IList<DcRegMedicine>> response = new BaseResponse<IList<DcRegMedicine>>();
            Mapper.CreateMap<DC_REGMEDICINE, DcRegMedicine>();
            var q = from m in unitOfWork.GetRepository<DC_REGMEDICINE>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.OrderBy(m => m.CREATEDATE);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<DcRegMedicine>>(q.ToList());
            }
            else
            {
                response.Data = new List<DcRegMedicine> { };
            }
            return response;
        }
        public BaseResponse<DcRegMedicine> saveMedicine(BaseResponse<DcRegMedicine> request)
        {
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            return base.Save<DC_REGMEDICINE, DcRegMedicine>(request.Data, (q) => q.ID == request.Data.Id);
        }

        public BaseResponse DeleteMedicine(long id)
        {
            return base.Delete<DC_REGMEDICINE>(id);
        }
        public BaseResponse<List<DcRegMedicine>> saveMedicineList(BaseResponse<List<DcRegMedicine>> request)
        {
            BaseResponse<List<DcRegMedicine>> response = new BaseResponse<List<DcRegMedicine>>();
            Mapper.CreateMap<DcRegMedicine, DC_REGMEDICINE>();
            request.Data.ForEach(m =>
            {
                var model = Mapper.Map<DC_REGMEDICINE>(m);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                if (model.ID > 0)
                {
                    unitOfWork.GetRepository<DC_REGMEDICINE>().Update(model);
                }
                else
                {
                    unitOfWork.GetRepository<DC_REGMEDICINE>().Insert(model);
                }

            });
            unitOfWork.Save();
            response.Data = request.Data;
            return response;
        }
        #endregion



    }
}

