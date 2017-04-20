/*****************************************************************************
 * Creator:	Lei Chen
 * Create Date: 2016-03-08
 * Modifier:
 * Modify Date:
 * Description:针剂
 ******************************************************************************/
using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Implement.Base;
using KMHC.SLTC.Business.Implement.Other;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class NursingManageService : BaseService, INursingManageService
    {

        IDictManageService dicSvc = IOCContainer.Instance.Resolve<IDictManageService>();
        #region 针剂
        public BaseResponse<InjectionView> SaveInjection(InjectionView request)
        {
            var response = new BaseResponse<InjectionView>();
            if (request != null)
            {
                Mapper.CreateMap<InjectionView, LTC_VACCINEINJECT>();
                var model = unitOfWork.GetRepository<LTC_VACCINEINJECT>().dbSet.FirstOrDefault(m => m.ID == request.ID);
                if (model == null)
                {
                    model = Mapper.Map<LTC_VACCINEINJECT>(request);
                    model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                    unitOfWork.GetRepository<LTC_VACCINEINJECT>().Insert(model);
                }
                else
                {
                    Mapper.Map(request, model);
                    unitOfWork.GetRepository<LTC_VACCINEINJECT>().Update(model);
                }
                unitOfWork.Save();
                response.ResultCode = (int)EnumResponseStatus.Success;
            }
            else
            {
                response.ResultCode = (int)EnumResponseStatus.Success;
                response.ResultMessage = "Injection ID不能为空";
            }
            return response;
        }

        public BaseResponse DeleteInjection(int id)
        {
            return base.Delete<LTC_VACCINEINJECT>(id);
        }

        public BaseResponse<InjectionView> GetInjection(object id)
        {
            Mapper.CreateMap<LTC_VACCINEINJECT, InjectionView>();
            long injectId = Convert.ToInt64(id);
            var response = new BaseResponse<InjectionView>();
            var findItem = unitOfWork.GetRepository<LTC_VACCINEINJECT>().dbSet.FirstOrDefault(m => m.ID == injectId);
            if (findItem != null)
            {
                response.Data = Mapper.Map<InjectionView>(unitOfWork.GetRepository<LTC_VACCINEINJECT>().dbSet.FirstOrDefault(m => m.ID == injectId));
            }
            return response;
        }

        public BaseResponse<IList<InjectionView>> GetInjecttionByRegNo(int regNo)
        {
            Mapper.CreateMap<LTC_VACCINEINJECT, InjectionView>();
            var response = new BaseResponse<IList<InjectionView>>();
            var q = from n in unitOfWork.GetRepository<LTC_VACCINEINJECT>().dbSet.Where(m => m.REGNO == regNo)
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.OPERATOR equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        OutValue = n,
                        EmpName = re.EMPNAME
                    };
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<InjectionView>();
                foreach (dynamic item in list)
                {
                    InjectionView newItem = Mapper.DynamicMap<InjectionView>(item.OutValue);
                    newItem.OPERATOR = item.EmpName;
                    response.Data.Add(newItem);
                }

            };

            var result = q.ToList();
            mapperResponse(result);
            return response;

            //if (findItem != null)
            //{
            //    response.Data = Mapper.Map<IList<InjectionView>>(unitOfWork.GetRepository<LTC_VACCINEINJECT>().dbSet.Where(m => m.REGNO == regNo));
            //}
            //return response;
        }

        //针剂 原始
        //        public BaseResponse<IList<InjectionView>> QueryInjection(NursingFilter request)
        //        {
        //            BaseResponse<IList<InjectionView>> response = new BaseResponse<IList<InjectionView>>();

        //            List<InjectionView> list = null;
        //            //
        //            if (request.RegNO == -1)
        //            {

        //            }
        //            else
        //            {
        //                StringBuilder sb = new StringBuilder();
        //                string sql = string.Format(@"SELECT * FROM(
        //                                SELECT REG.REGNO,IPD.FEENO,REG.NAME,INJECT.ITEMTYPE,INJECT.STATE,INJECT.INJECTDATE,
        //                                (SELECT EMPNAME FROM LTC_EMPFILE WHERE EMPNO=INJECT.OPERATOR) AS OPERATOR,INJECT.TRACESTATE, INJECT.ID,
        //                                 ROW_NUMBER() OVER (PARTITION BY REG.REGNO ORDER BY INJECTDATE) AS GROUP_IDX
        //                                ,(SELECT COUNT(*) FROM dbo.LTC_VACCINEINJECT WHERE REGNO=REG.REGNO) AS QUANTITY
        //                                 FROM dbo.LTC_REGFILE REG 
        //                                LEFT JOIN dbo.LTC_VACCINEINJECT INJECT
        //                                ON REG.REGNO=INJECT.REGNO
        //                                INNER JOIN LTC_IPDREG IPD
        //                                ON IPD.REGNO=REG.REGNO AND (IPD.IPDFLAG='I' OR IPD.IPDFLAG='N')
        //                                WHERE REG.ORGID='{0}'
        //                                ) T
        //                                WHERE T.GROUP_IDX=1 ", SecurityHelper.CurrentPrincipal.OrgId);
        //                sb.Append(sql);
        //                if (request != null)
        //                {
        //                    if (request.RegNO>1)
        //                    {
        //                        sb.Append(string.Format(" AND REGNO='{0}'", request.RegNO));
        //                    }
        //                    //if (!string.IsNullOrEmpty(request.Name))
        //                    //{
        //                    //    sb.Append(string.Format(" AND NAME LIKE '%{0}%'", request.Name));
        //                    //}
        //                    //if (!string.IsNullOrEmpty(request.Date))
        //                    //{
        //                    //    sb.Append(string.Format(" AND INJECTDATE= CONVERT(DATE,'{0}',120) ", request.Date));
        //                    //}
        //                }
        //                list = unitOfWork.GetRepository<InjectionView>().SqlQuery(sb.ToString()).ToList();



        //            }

        //            response.Data = list;
        //            return response;
        //        }



        // 针剂 姚丙慧
        public BaseResponse<IList<Vaccineinject>> QueryInjection(NursingFilter request)
        {
            var response = new BaseResponse<IList<Vaccineinject>>();
            //这边获取list的集合
            List<Vaccineinject> CheckReclist = new List<Vaccineinject>();
            List<LTC_VACCINEINJECT> regQuestion = unitOfWork.GetRepository<LTC_VACCINEINJECT>().dbSet.Where(m => m.REGNO == request.RegNO).ToList();
            Mapper.CreateMap<LTC_VACCINEINJECT, Vaccineinject>();
            Mapper.Map(regQuestion, CheckReclist);
            response.Data = CheckReclist;
            response.RecordsCount = regQuestion.Count;
            response.Data = CheckReclist.OrderByDescending(m => m.ID).Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
            response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            return response;

        }

        #endregion

        #region 评估量表
        public BaseResponse<IList<EvaluationRecord>> QueryEvaluationList(NursingFilter request)
        {
            BaseResponse<IList<EvaluationRecord>> response = new BaseResponse<IList<EvaluationRecord>>();
            LTC_QUESTION currentQuestion = unitOfWork.GetRepository<LTC_QUESTION>().dbSet.Where(x => x.CODE == request.Code && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            if (currentQuestion == null)
            {
                response.ResultCode = 2;
                response.ResultMessage = "请先维护" + request.Code + "评估表单数据!";
                return response;
            }
            List<EvaluationRecord> list = null;
            request.Id = currentQuestion.QUESTIONID;
            response.Id = currentQuestion.QUESTIONID;
            if (request != null && request.PageSize > 0)
            { }
            else
            {
                StringBuilder sb = new StringBuilder();
                string sql = string.Format(@" SELECT REG.REGNO,REG.NAME,REG.SEX,IPD.FEENO,Q.RECORDID, Q.EVALDATE,Q.NEXTEVALDATE,
                                (SELECT EMPNAME FROM LTC_EMPFILE WHERE Q.EVALUATEBY=EMPNO) AS EVALUATEBY,
                                (SELECT EMPNAME FROM LTC_EMPFILE WHERE Q.NEXTEVALUATEBY=EMPNO) AS NEXTEVALUATEBY,
                                (SELECT COUNT(*) FROM LTC_REGQUESTION WHERE REGNO=REG.REGNO AND QUESTIONID={0}) AS QUANTITY
                                 FROM LTC_REGFILE REG 
                                LEFT JOIN (SELECT * FROM (
								SELECT * FROM  LTC_REGQUESTION ORDER BY EVALDATE DESC) T GROUP BY FEENO) Q
                                ON REG.REGNO=Q.REGNO AND Q.QUESTIONID={0}
                                INNER JOIN LTC_IPDREG IPD
                                ON IPD.REGNO=REG.REGNO AND (IPD.IPDFLAG='I' OR IPD.IPDFLAG='N')
                                WHERE REG.ORGID='{1}'", request.Id, SecurityHelper.CurrentPrincipal.OrgId);
                sb.Append(sql);
                if (request != null)
                {
                    if (!string.IsNullOrEmpty(request.Name))
                    {
                        sb.Append(string.Format(" AND REG.NAME like '%{0}%'", request.Name));
                    }
                    if (request.FeeNo.HasValue && request.FeeNo.Value > 0)
                    {
                        sb.Append(string.Format(" AND IPD.FEENO {0}", request.FeeNo.Value));
                    }
                    if (!string.IsNullOrEmpty(request.Sex))
                    {
                        sb.Append(string.Format(" AND REG.SEX= '{0}' ", request.Sex));
                    }
                }
                list = unitOfWork.GetRepository<EvaluationRecord>().SqlQuery(sb.ToString()).ToList();
                // list.ForEach(p => p.SEX = CodeHelper.GetItemName(p.SEX, "A00.001"));
            }
            response.Data = list;
            return response;
        }
        public BaseResponse<QUESTION> GetQuetionByCode(string Code)
        {
            BaseResponse<QUESTION> response = new BaseResponse<QUESTION>();
            LTC_QUESTION currentQuestion = unitOfWork.GetRepository<LTC_QUESTION>().dbSet.Where(x => x.CODE == Code && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            if (currentQuestion == null)
            {
                response.ResultCode = 2;
                response.ResultMessage = "请先维护" + Code + "评估表单数据!";
                return response;
            }
            response= GetQuetion(currentQuestion.QUESTIONID,null,null);

            return response;
        }

        public BaseResponse<QUESTION> GetQuetion(int qId, long? regNo, long? recordId)
        {
            var response = new BaseResponse<QUESTION>();
            List<LTC_REGQUESTIONDATA> regQuetionData = new List<LTC_REGQUESTIONDATA>();
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
                regQuetionData = unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().dbSet.Where(m => m.RECORDID == recordId.Value).ToList();
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


        public BaseResponse SaveQuetion(REGQUESTION request)
        {
            var response = new BaseResponse();

            Mapper.CreateMap<REGQUESTION, LTC_REGQUESTION>();

            var model = unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(m => m.RECORDID == request.RECORDID).FirstOrDefault();
            if (model == null)
            {
                model = Mapper.Map<LTC_REGQUESTION>(request);
                //SecurityHelper.CurrentPrincipal.EmpNo;
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                if (!model.EVALDATE.HasValue)
                {
                    model.EVALDATE = DateTime.Now;
                }
                unitOfWork.GetRepository<LTC_REGQUESTION>().Insert(model);
                unitOfWork.Save();
                Mapper.CreateMap<REGQUESTIONDATA, LTC_REGQUESTIONDATA>();

                List<LTC_REGQUESTIONDATA> questionData = Mapper.Map<List<LTC_REGQUESTIONDATA>>(request.QuestionDataList);
                if (questionData!=null)
                {
                    questionData.ForEach(p => p.RECORDID = model.RECORDID);
                    unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().InsertRange(questionData);
                }
            }
            else
            {
                Mapper.Map(request, model);
                unitOfWork.GetRepository<LTC_REGQUESTION>().Update(model);

                List<LTC_REGQUESTIONDATA> regQueData = unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().dbSet.Where(m => m.RECORDID == request.RECORDID).ToList();
                regQueData.ForEach(p => unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().Delete(p));

                Mapper.CreateMap<REGQUESTIONDATA, LTC_REGQUESTIONDATA>();
                List<LTC_REGQUESTIONDATA> questionData = Mapper.Map<List<LTC_REGQUESTIONDATA>>(request.QuestionDataList);
                questionData.ForEach(p => p.RECORDID = model.RECORDID);
                unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().InsertRange(questionData);

            }
            unitOfWork.Save();

            return response;
        }

        public BaseResponse<REGQUESTION> GetREGQuetion(long recordId)
        {
            var response = new BaseResponse<REGQUESTION>();
            REGQUESTION RegQuestion = new REGQUESTION();
            Mapper.CreateMap<LTC_REGQUESTION, REGQUESTION>();
            LTC_REGQUESTION regQuestion = unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(m => m.RECORDID == recordId).FirstOrDefault();
            Mapper.Map(regQuestion, RegQuestion);
            response.Data = RegQuestion;
            return response;
        }

        public Calculation CalcEvaluation(QUESTION question)
        {
            Calculation cal = new Calculation();

            int qId = question.QUESTIONID;
            string QuestionName = question.QUESTIONNAME.Trim();


            switch (QuestionName)
            {
                case "失智量表":
                    cal = CalcEvalMMSE(qId, question.REGNO.Value, question.MakerItemList);
                    break;
                case "简易心智量表":
                    cal = CalcEvalSPMSQ(qId, question.REGNO.Value, question.MakerItemList);
                    break;
                default:
                    cal = CalcEvalCommon(qId, question.MakerItemList);
                    break;
            }
            return cal;
        }

        public BaseResponse<IList<REGQUESTION>> GetEvaluationHisOver(BaseRequest<NursingFilter> request)
        {
            var response = new BaseResponse<IList<REGQUESTION>>();

            var q = from n in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(m => m.FEENO == request.Data.FeeNo.Value && m.QUESTIONID == request.Data.QuestionId)
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.EVALUATEBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    orderby n.EVALDATE descending
                    select new
                    {
                        OutValue = n,
                        EmpName = re.EMPNAME
                    };
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<REGQUESTION>();
                foreach (dynamic item in list)
                {
                    REGQUESTION newItem = Mapper.DynamicMap<REGQUESTION>(item.OutValue);
                    newItem.EVALUATEBY = item.EmpName;
                    response.Data.Add(newItem);
                }
            };

            response.RecordsCount = q.Count();

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

        public BaseResponse<object> GetEvalRecsForExtApi(BaseRequest<EvalRecFilter> request)
        {
            var response = new BaseResponse<object>();
            var q = from r in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet
                join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on r.EVALUATEBY equals e.EMPNO
                join qs in unitOfWork.GetRepository<LTC_QUESTION>().dbSet.Where(m => m.ORGID == "tpl") on r.QUESTIONID
                equals qs.QUESTIONID
                select new
                {
                   FeeNo = r.FEENO,
                   EvalBy = e.EMPNAME,
                   Result = r.ENVRESULTS,
                   Score = r.SCORE,
                   EvalTime = r.EVALDATE,
                   EvalName = qs.QUESTIONNAME,
                   EvalQuestionId = qs.QUESTIONID
                };
            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            if (request.Data.SDate.HasValue)
            {
                q = q.Where(m => m.EvalTime >= request.Data.SDate);
            }
            if (request.Data.EDate.HasValue)
            {
                q = q.Where(m => m.EvalTime < request.Data.EDate);
            }
            q = q.OrderByDescending(m => m.EvalTime);
            response.RecordsCount = q.Count();

            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                response.Data = list;
            }
            else
            {
                var list = q.ToList();
                response.Data = list;
            }
            return response;
        }

        public BaseResponse DeleteEvaluation(long recordId)
        {
            var response = new BaseResponse();

            List<LTC_REGQUESTIONDATA> regQueData = unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().dbSet.Where(m => m.RECORDID == recordId).ToList();
            regQueData.ForEach(p => unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().Delete(p));

            LTC_REGQUESTION reqQue = unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(m => m.RECORDID == recordId).FirstOrDefault();
            unitOfWork.GetRepository<LTC_REGQUESTION>().Delete(reqQue);
            unitOfWork.Save();
            return response;
        }

        public BaseResponse<REGQUESTION> GetLatestRegQuetion(long feeNo, int quetionId)
        {
            var response = new BaseResponse<REGQUESTION>();
            REGQUESTION RegQuestion = new REGQUESTION();
            Mapper.CreateMap<LTC_REGQUESTION, REGQUESTION>();
            LTC_REGQUESTION regQuestion = unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(m => m.FEENO == feeNo && m.QUESTIONID == quetionId).OrderByDescending(x => x.EVALDATE).FirstOrDefault();
            Mapper.Map(regQuestion, RegQuestion);
            response.Data = RegQuestion;
            return response;
        }

        #endregion

        #region 团体活动批量评估

        public BaseResponse SaveBatchQuetion(List<REGQUESTION> request)
        {
            var response = new BaseResponse();
            foreach (REGQUESTION question in request)
            {
                SaveQuetion(question);
            }
            return response;
        }
        #endregion

        #region 护理需求
        public BaseResponse<IList<EvaluationRecord>> QueryNurDemandEvalList(NursingFilter request)
        {
            BaseResponse<IList<EvaluationRecord>> response = new BaseResponse<IList<EvaluationRecord>>();

            List<EvaluationRecord> list = null;
            if (request != null && request.PageSize > 0)
            { }
            else
            {
                StringBuilder sb = new StringBuilder();
                string sql = string.Format(@"SELECT REG.REGNO,REG.RESIDENGNO,REG.NAME,REG.SEX,IPD.FEENO,Q.ID, Q.EVALDATE,Q.NEXTEVALDATE,
                                (SELECT EMPNAME FROM LTC_EMPFILE WHERE Q.EVALUATEBY=EMPNO) AS EVALUATEBY,
                                (SELECT EMPNAME FROM LTC_EMPFILE WHERE Q.NEXTEVALUATEBY=EMPNO) AS NEXTEVALUATEBY
                                ,(SELECT COUNT(*) FROM LTC_CAREDEMANDEVAL WHERE FEENO=IPD.FEENO ) AS QUANTITY
                                 FROM LTC_IPDREG IPD 
                                LEFT JOIN (SELECT * FROM( select * from LTC_CAREDEMANDEVAL  ORDER BY EVALDATE ) T GROUP BY FEENO) Q
                                ON IPD.FEENO=Q.FEENO 
                                INNER JOIN LTC_REGFILE REG 
                                ON IPD.REGNO=REG.REGNO AND IPD.IPDFLAG='I' 
                                 WHERE REG.ORGID='{0}'", SecurityHelper.CurrentPrincipal.OrgId);

                sb.Append(sql);
                if (request != null)
                {
                    if (!string.IsNullOrEmpty(request.Name))
                    {
                        sb.Append(string.Format(" AND REG.NAME like '%{0}%'", request.Name));
                    }
                    if (request.FeeNo.HasValue && request.FeeNo.Value > 0)
                    {
                        sb.Append(string.Format(" AND IPD.FEENO = {0}", request.FeeNo.Value));
                    }
                    if (!string.IsNullOrEmpty(request.residengno))
                    {
                        sb.Append(string.Format(" AND REG.RESIDENGNO = {0}", request.residengno));
                    }
                    if (!string.IsNullOrEmpty(request.Sex))
                    {
                        sb.Append(string.Format(" AND REG.SEX= '{0}' ", request.Sex));
                    }
                }
                list = unitOfWork.GetRepository<EvaluationRecord>().SqlQuery(sb.ToString()).ToList();
                //list.ForEach(p => p.SEX = CodeHelper.GetItemName(p.SEX, "A00.001"));
            }
            response.Data = list;
            return response;
        }
        public BaseResponse<IList<EvaluationResult>> QueryEvaluationResult(long feeNo)
        {
            BaseResponse<IList<EvaluationResult>> response = new BaseResponse<IList<EvaluationResult>>();
            List<EvaluationResult> list = null;

            StringBuilder sb = new StringBuilder();

            List<string> questionList = new List<string>() { "GDS", "ADL", "MMSE", "IADL", "KARNOFSKY", "SORE", "BEHAVIOR", "FALL" };

            List<IdCode> questionIds = (from q in unitOfWork.GetRepository<LTC_QUESTION>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId && questionList.Contains(x.CODE))
                                        select new IdCode { Id = q.QUESTIONID, Code = q.CODE }).ToList();

            string sql = string.Format(@"SELECT * FROM(
                                     SELECT QUESTIONID,FEENO,SCORE,ENVRESULTS,EVALDATE   FROM LTC_REGQUESTION ORDER BY EVALDATE DESC) T 
                                            LEFT JOIN  LTC_QUESTION Q ON Q.QUESTIONID = T.QUESTIONID 
                                                WHERE  FEENO={0} 
						                        AND Q.ORGID='{1}'
            AND Q.`CODE` IN ('GDS','ADL','MMSE','IADL','KARNOFSKY','SORE','BEHAVIOR','FALL')
						GROUP BY  T.QUESTIONID ", feeNo, SecurityHelper.CurrentPrincipal.OrgId);
            sb.Append(sql);
            list = unitOfWork.GetRepository<EvaluationResult>().SqlQuery(sb.ToString()).ToList();

            foreach (IdCode q in questionIds)
            {
                if (list != null && list.Where(x => x.CODE == q.Code).ToList().Count > 0)
                {
                    EvaluationResult item = list.Where(x => x.CODE == q.Code).FirstOrDefault();
                    List<QuestionResult> QueResult = new List<QuestionResult>();
                    List<LTC_QUESTIONRESULTS> queResult = unitOfWork.GetRepository<LTC_QUESTIONRESULTS>().dbSet.Where(x => x.QUESTIONID == q.Id).ToList();
                    Mapper.CreateMap<LTC_QUESTIONRESULTS, QuestionResult>();
                    Mapper.Map(queResult, QueResult);
                    item.QueResult = QueResult;
                }
                else
                {
                    EvaluationResult item = new EvaluationResult();
                    item.CODE = q.Code;
                    List<LTC_QUESTIONRESULTS> queResult = unitOfWork.GetRepository<LTC_QUESTIONRESULTS>().dbSet.Where(x => x.QUESTIONID == q.Id).ToList();
                    List<QuestionResult> QueResult = new List<QuestionResult>();
                    Mapper.CreateMap<LTC_QUESTIONRESULTS, QuestionResult>();
                    Mapper.Map(queResult, QueResult);
                    item.QueResult = QueResult;
                    list.Add(item);
                }
            }
            response.Data = list;
            return response;
        }


        //根据item获取值的范围的
        public BaseResponse<IList<Person>> QueryPerson(int regno)
        {

            var response = new BaseResponse<IList<Person>>();

            //这边获取list的集合
            List<Person> CheckReclist = new List<Person>();

            List<LTC_REGFILE> regQuestion = unitOfWork.GetRepository<LTC_REGFILE>().dbSet.Where(m => m.REGNO == regno).ToList();


            Mapper.CreateMap<LTC_REGFILE, Person>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;

        }




        public BaseResponse<CareDemandEval> SaveCareDemand(CareDemandEval request)
        {
            request.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
            return base.Save<LTC_CAREDEMANDEVAL, CareDemandEval>(request, (q) => q.ID == request.ID);
        }
        public BaseResponse<CareDemandEval> GetCareDemand(long demandId)
        {
            return base.Get<LTC_CAREDEMANDEVAL, CareDemandEval>((q) => q.ID == demandId);
        }
        public BaseResponse<IList<CareDemandEval>> QueryCareDemandHis(NursingFilter requestData)
        {
            BaseRequest<NursingFilter> request = new BaseRequest<NursingFilter>();
            request.Data = requestData;
            request.PageSize = 0;

            BaseResponse<IList<CareDemandEval>> response = new BaseResponse<IList<CareDemandEval>>();
            var q = from n in unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.EVALUATEBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        CareDemandRec = n,
                        EVALUATEName = re.EMPNAME
                    };
            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.CareDemandRec.FEENO == request.Data.FeeNo);
            }

            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<CareDemandEval>();
                foreach (dynamic item in list)
                {
                    CareDemandEval newItem = Mapper.DynamicMap<CareDemandEval>(item.CareDemandRec);
                    newItem.EVALUATEName = item.EVALUATEName;
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

        public BaseResponse<CareDemandEval> QueryLatestCareDemand(long feeNo)
        {
            BaseResponse<CareDemandEval> response = new BaseResponse<CareDemandEval>();
            Mapper.CreateMap<LTC_CAREDEMANDEVAL, CareDemandEval>();
            LTC_CAREDEMANDEVAL demand = unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet.Where(m => m.FEENO == feeNo).OrderByDescending(x => x.EVALDATE).FirstOrDefault();

            response.Data = Mapper.Map<CareDemandEval>(demand);
            return response;
        }

        public BaseResponse DeleteNurDemandEval(long recordId)
        {
            var response = new BaseResponse();
            LTC_CAREDEMANDEVAL demand = unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet.Where(m => m.ID == recordId).FirstOrDefault();
            unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().Delete(demand);
            unitOfWork.Save();
            return response;
        }

        #endregion




        #region Helper

        private Calculation CalcEvalCommon(int qId, List<MakerItemCollection> makerItemList)
        {
            Calculation cal = new Calculation();
            decimal Score = 0;
            string Result = "";
            foreach (MakerItemCollection item in makerItemList)
            {
                if (item.LIMITEDVALUEID.HasValue && item.LIMITEDVALUEID.Value > 0)
                {
                    var answer = item.Answers.Where(x => x.LIMITEDVALUEID == item.LIMITEDVALUEID).FirstOrDefault();
                    if (answer != null)
                    {
                        if (answer.LIMITEDVALUE.HasValue)
                        {
                            if (answer.LIMITEDVALUE.Value == (int)EnumEvaluation.UnableToEval)
                            {
                                Score = -1;
                                Result = "无法评估";
                                break;
                            }
                            else if (answer.LIMITEDVALUE.Value == (int)EnumEvaluation.Undefined || answer.LIMITEDVALUE.Value == (int)EnumEvaluation.NotFill)
                            {
                            }
                            else
                            {
                                Score += answer.LIMITEDVALUE.Value;
                            }
                        }
                    }
                }
            }
            if (Score >= 0)
            {
                Result = GetQuestionResult(Score, qId);
            }
            cal.Score = Score;
            cal.Result = Result;
            return cal;
        }
        //失智量表
        private Calculation CalcEvalMMSE(int qId, int regNo, List<MakerItemCollection> makerItemList)
        {
            Calculation cal = new Calculation();
            LTC_REGFILE regFile = unitOfWork.GetRepository<LTC_REGFILE>().dbSet.Where(m => m.REGNO == regNo).FirstOrDefault();
            int currentAge = Util.GetAgeByDate(regFile.BRITHDATE);
            cal = CalcEvalCommon(qId, makerItemList);
            if (currentAge >= 60 && currentAge < 65)
            {
                if (cal.Score <= 20)
                {
                    cal.Result = "轻度认知功能障碍";
                }
                else
                {
                    cal.Result = "正常";
                }
            }
            else if (currentAge >= 65)
            {
                if (cal.Score <= 15)
                {
                    cal.Result = "认知功能障碍";
                }
                else
                {
                    cal.Result = "正常";
                }
            }
            else
            {
                cal.Result = "年龄层不符合计算标准";
            }
            return cal;
        }
        //简易心智量表
        private Calculation CalcEvalSPMSQ(int qId, int regNo, List<MakerItemCollection> makerItemList)
        {
            Calculation cal = new Calculation();
            LTC_REGFILE regFile = unitOfWork.GetRepository<LTC_REGFILE>().dbSet.Where(m => m.REGNO == regNo).FirstOrDefault();
            string eduCode = regFile.EDUCATION;
            cal = CalcEvalCommon(qId, makerItemList);
            if (eduCode == "004" || eduCode == "002")
            {
                if (cal.Score <= 3)
                {
                    cal.Result = "严重智力缺损";
                }
                else if (cal.Score <= 5 && cal.Score >= 4)
                {
                    cal.Result = "中度智力缺损";
                }
                else if (cal.Score <= 8 && cal.Score >= 6)
                {
                    cal.Result = "轻度智力缺损";
                }
                else if (cal.Score >= 9)
                {
                    cal.Result = "心智功能完好";
                }

            }
            else if (eduCode == "003")
            {
                if (cal.Score <= 2)
                {
                    cal.Result = "严重智力缺损";
                }
                else if (cal.Score <= 4 && cal.Score >= 3)
                {
                    cal.Result = "中度智力缺损";
                }
                else if (cal.Score <= 7 && cal.Score >= 5)
                {
                    cal.Result = "轻度智力缺损";
                }
                else if (cal.Score >= 8)
                {
                    cal.Result = "心智功能完好";
                }
            }
            else if (eduCode == "005" || eduCode == "006" || eduCode == "007" || eduCode == "008" || eduCode == "009" || eduCode == "010" || eduCode == "011")
            {
                if (cal.Score <= 1)
                {
                    cal.Result = "严重智力缺损";
                }
                else if (cal.Score <= 3 && cal.Score >= 2)
                {
                    cal.Result = "中度智力缺损";
                }
                else if (cal.Score <= 6 && cal.Score >= 4)
                {
                    cal.Result = "轻度智力缺损";
                }
                else if (cal.Score >= 7)
                {
                    cal.Result = "心智功能完好";
                }
            }
            else
            {
                cal.Result = "教育程度不符合计算标准";
            }
            return cal;
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

        public int? GetLimitedValueId(long? recordId, int makerId, List<LTC_REGQUESTIONDATA> regQuetionData)
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


        public string GetQuestionResult(decimal score, int qId)
        {
            List<LTC_QUESTIONRESULTS> questionResult = unitOfWork.GetRepository<LTC_QUESTIONRESULTS>().dbSet.Where(m => m.QUESTIONID == qId).ToList();
            LTC_QUESTIONRESULTS result = questionResult.Where(x => x.UPBOUND >= score && x.LOWBOUND <= score).FirstOrDefault();
            if (result != null)
            {
                return result.RESULTNAME;
            }
            return "";
        }

        #endregion

        #region 药品管理
        public BaseResponse<IList<Medicine>> QueryMedData(BaseRequest<MedicineFilter> request)
        {
            var response = new BaseResponse<IList<Medicine>>();
            if (request.Data.KeyWord != "$$$")
            {
                response = base.Query<LTC_MEDICINE, Medicine>(request, (q) =>
                {
                    if (!string.IsNullOrEmpty(request.Data.KeyWord))
                    {
                        q = q.Where(m => m.ORGID==SecurityHelper.CurrentPrincipal.OrgId);
                        q = q.Where(m => m.CHNNAME.Trim().ToLower().Contains(request.Data.KeyWord.ToLower()) || m.ENGNAME.Trim().ToLower().Contains(request.Data.KeyWord.ToLower()) || m.INSNO.Trim().ToLower().Contains(request.Data.KeyWord.ToLower()) || m.HOSPNO.Trim().Contains(request.Data.KeyWord) || m.MEDCODE.Trim().ToLower().Contains(request.Data.KeyWord.ToLower()));
                    }
                    q = q.OrderByDescending(m => m.MEDID);
                    return q;
                });
            }
            return response;
        }
        public BaseResponse<Medicine> GetMedData(int medId)
        {
            return base.Get<LTC_MEDICINE, Medicine>((q) => q.MEDID == medId);
        }

        public BaseResponse<Medicine> SaveMedData(Medicine request)
        {
            return base.Save<LTC_MEDICINE, Medicine>(request, (q) => q.MEDID == request.Medid);
        }

        public BaseResponse DeleteMedData(long MEDID)
        {

            var reqQueVisitPrescription = unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().dbSet.Where(m => m.MEDID == MEDID).FirstOrDefault();
            if (reqQueVisitPrescription == null)
            {
                return base.Delete<LTC_MEDICINE>(MEDID);
            }
            else
            {
                return new BaseResponse
                {
                    ResultCode = 12580,
                    ResultMessage = "该药品不能删除！"
                };
            }
        }
        #endregion

        #region 用医就药
        public BaseResponse<IList<VisitDocRecords>> QueryVisitDocRecData(BaseRequest<VisitDocRecordsFilter> request)
        {
            var response = new BaseResponse<IList<VisitDocRecords>>();
            var q = from vr in unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().dbSet
                    join e1 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on vr.RECORDBY equals e1.EMPNO into vr_e1
                    from vr_emp1 in vr_e1.DefaultIfEmpty()
                    join e2 in unitOfWork.GetRepository<LTC_VISITHOSPITAL>().dbSet on vr.VISITHOSP equals e2.HOSPNO into vr_e2
                    from vr_emp2 in vr_e2.DefaultIfEmpty()
                    join e3 in unitOfWork.GetRepository<LTC_VISITDEPT>().dbSet on vr.VISITDEPT equals e3.DEPTNO into vr_e3
                    from vr_emp3 in vr_e3.DefaultIfEmpty()
                    select new
                    {
                        visitDocRecords = vr,
                        RecordNameBy = vr_emp1.EMPNAME,
                        VisitHospName = vr_emp2.HOSPNAME,
                        VisitDeptName = vr_emp3.DEPTNAME,
                        VisitDoctorName = vr.VISITDOCTOR,
                        
                    };
            q = q.Where(m => m.visitDocRecords.FEENO == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.visitDocRecords.VISITDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<VisitDocRecords>();
                foreach (dynamic item in list)
                {
                    VisitDocRecords newItem = Mapper.DynamicMap<VisitDocRecords>(item.visitDocRecords);
                    newItem.RecordNameBy = item.RecordNameBy;
                    newItem.VisitHospName = item.VisitHospName;
                    newItem.VisitDeptName = item.VisitDeptName;
                    newItem.VisitDoctorName = item.VisitDoctorName;
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
            // var response = base.Query<LTC_VISITDOCRECORDS, VisitDocRecords>(request, (q) =>
            //{
            //    q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    q = q.OrderByDescending(m => m.SEQNO);
            //    return q;
            //});
            // return response;
        }

        public BaseResponse<VisitDocRecords> SaveVisitDocRecData(VisitDocRecords request)
        {
            foreach(var item in request.VisitPrescription)
            {
                item.Operator = SecurityHelper.CurrentPrincipal.EmpNo;
                item.TakeTime = DateTime.Now;
            }

            var response = new BaseResponse<VisitDocRecords>();
            //List<LTC_VISITPRESCRIPTION> regQueData = unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().dbSet.Where(m => m.SEQNO == request.SeqNo).ToList();
            //regQueData.ForEach(p => unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().Delete(p));
            Mapper.CreateMap<VisitPrescription, LTC_VISITPRESCRIPTION>();
            Mapper.CreateMap<LTC_VISITPRESCRIPTION, VisitPrescription>();
            var cm = Mapper.CreateMap<VisitDocRecords, LTC_VISITDOCRECORDS>();
            Mapper.CreateMap<LTC_VISITDOCRECORDS, VisitDocRecords>();
            var reqQue = unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().dbSet.Where(m => m.SEQNO == request.SeqNo).FirstOrDefault();
            if (reqQue == null)
            {
                reqQue = Mapper.Map<LTC_VISITDOCRECORDS>(request);
                unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().Insert(reqQue);
            }
            else
            {
                Mapper.Map(request, reqQue);
                unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().Update(reqQue);
            }
            unitOfWork.Save();
            Mapper.Map(reqQue, request);
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteVisitDocRecData(long SEQNO)
        {
            //return base.Delete<LTC_VISITDOCRECORDS>(SEQNO);

            var response = new BaseResponse();
            List<LTC_VISITPRESCRIPTION> regQueData = unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().dbSet.Where(m => m.SEQNO == SEQNO).ToList();
            regQueData.ForEach(p => unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().Delete(p));

            LTC_VISITDOCRECORDS reqQue = unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().dbSet.Where(m => m.SEQNO == SEQNO).FirstOrDefault();
            unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().Delete(reqQue);
            unitOfWork.Save();
            return response;
        }

        public BaseResponse<IList<VisitPrescription>> QueryVisitPreData(BaseRequest<VisitPrescriptionFilter> request)
        {
            var response = new BaseResponse<IList<VisitPrescription>>();
            var q = from vr in unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().dbSet
                    join m in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on vr.DRUGID equals m.DRUGID into vr_m
                    from vr_med in vr_m.DefaultIfEmpty()
                    join freq in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on vr.FREQ equals freq.FREQNO into vr_m2
                    from vr_freq in vr_m2.DefaultIfEmpty()
                    select new
                    {
                        visitPrescription = vr,
                        EngName = vr_med.ENNAME,
                        CnName = vr_med.CNNAME,
                        FreqName = vr_freq.FREQNAME,
                    };
            q = q.Where(m => m.visitPrescription.SEQNO == request.Data.SeqNo);
            q = q.OrderByDescending(m => m.visitPrescription.SEQNO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<VisitPrescription>();
                foreach (dynamic item in list)
                {
                    VisitPrescription newItem = Mapper.DynamicMap<VisitPrescription>(item.visitPrescription);
                    newItem.EngName = item.EngName;
                    newItem.CnName = item.CnName;
                    newItem.FreqName = item.FreqName;
                    response.Data.Add(newItem);
                }

            };
            //if (request != null && request.PageSize > 0)
            //{
            //    var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
            //    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            //    mapperResponse(list);
            //}
            //else
            //{
                //var list = q.ToList();
                mapperResponse(q.ToList());
            //}
            return response;
        }

        public BaseResponse<VisitPrescription> SaveVisitPreData(VisitPrescription request)
        {
            request.Operator = SecurityHelper.CurrentPrincipal.EmpNo;
            return base.Save<LTC_VISITPRESCRIPTION, VisitPrescription>(request, (q) => q.PID == request.PId);
        }
        public BaseResponse DeleteVisitPreData(long PID)
        {
            return base.Delete<LTC_VISITPRESCRIPTION>(PID);
        }
        #endregion

        #region 营养记录单
        public BaseResponse<IList<NutrtioncateRec>> QueryNutrtioncateRec(BaseRequest<NutrtioncateRecFilter> request)
        {
            var response = base.Query<LTC_NUTRTIONCAREREC, NutrtioncateRec>(request, (q) =>
            {
                q = q.Where(m => m.FEENO == request.Data.FeeNo);
                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        public BaseResponse<NutrtioncateRec> GetNutrtioncateRec(long id)
        {
            return base.Get<LTC_NUTRTIONCAREREC, NutrtioncateRec>((q) => q.ID == id);
        }

        public BaseResponse<NutrtioncateRec> SaveNutrtioncateRec(NutrtioncateRec request)
        {
            return base.Save<LTC_NUTRTIONCAREREC, NutrtioncateRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteNutrtioncateRec(long id)
        {
            return base.Delete<LTC_NUTRTIONCAREREC>(id);
        }
        #endregion
    }
}

public enum EnumEvaluation
{
    NotFill = 97,
    Undefined = 98,
    UnableToEval = 99
}

