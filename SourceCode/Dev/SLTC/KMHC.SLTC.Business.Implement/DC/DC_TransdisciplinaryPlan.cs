using AutoMapper;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.Infrastructure;

namespace KMHC.SLTC.Business.Implement.DC
{
    public class DC_TransdisciplinaryPlan : BaseService, IDC_TransdisciplinaryPlan
    {

        //<summary>
        //查询单笔夸专业照顾计划
        //</summary>
        public BaseResponse<DC_MultiteamCarePlanRecModel> QueryMultiCarePlanRec(long seqNo)
        {
            //加载子项目
            var response = new BaseResponse<DC_MultiteamCarePlanRecModel>();
            DC_MultiteamCarePlanRecModel multiPlan = new DC_MultiteamCarePlanRecModel();
            List<DC_MULTITEAMCAREPLAN> carePlanList = new List<DC_MULTITEAMCAREPLAN>();
            List<DC_MultiteamCarePlanModel> carePlanListModel = new List<DC_MultiteamCarePlanModel>();
            DC_MULTITEAMCAREPLANEVAL careEval = new DC_MULTITEAMCAREPLANEVAL();
            DC_MultiteaMcarePlanEvalModel careEvalModel = new DC_MultiteaMcarePlanEvalModel();


            DC_MULTITEAMCAREPLANREC model = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x => x.SEQNO == seqNo).FirstOrDefault();

            if (model != null)
            {
                carePlanList.AddRange(model.DC_MULTITEAMCAREPLAN);
                careEval = model.DC_MULTITEAMCAREPLANEVAL;

                Mapper.CreateMap<DC_MULTITEAMCAREPLANREC, DC_MultiteamCarePlanRecModel>();
                Mapper.Map(model, multiPlan);

                Mapper.CreateMap<DC_MULTITEAMCAREPLAN, DC_MultiteamCarePlanModel>();
                Mapper.Map(carePlanList, carePlanListModel);

                Mapper.CreateMap<DC_MULTITEAMCAREPLANEVAL, DC_MultiteaMcarePlanEvalModel>();
                Mapper.Map(careEval, careEvalModel);

                multiPlan.CarePlan = carePlanListModel;
                multiPlan.PlanEval = careEvalModel;
            }

            response.Data = multiPlan;

            return response;
        }

        public BaseResponse<DC_MultiteamCarePlanRecModel> QueryLatestMultiCarePlanRec(long feeNo)
        {
            BaseResponse<DC_MultiteamCarePlanRecModel> response = new BaseResponse<DC_MultiteamCarePlanRecModel>();
            var maxEval = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x=>x.FEENO == feeNo).OrderByDescending(x => x.EVALNUMBER).FirstOrDefault();

            if (maxEval!=null)
            {
                long seqNo = maxEval.SEQNO;
                response = QueryMultiCarePlanRec(seqNo);
            }
            return response;
        }

        public  BaseResponse<DC_MultiteamCarePlanRecModel>  QueryTransdisciplinaryRef(long feeNo)
        {
            BaseResponse<DC_MultiteamCarePlanRecModel> response = new BaseResponse<DC_MultiteamCarePlanRecModel>();
            DC_MultiteamCarePlanRecModel data = new DC_MultiteamCarePlanRecModel();
            List<DC_MultiteamCarePlanModel> carePlan = new List<DC_MultiteamCarePlanModel>();
            int currentEvalNum = 1;
            var maxPlanRec = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x => x.FEENO == feeNo).OrderByDescending(x => x.EVALNUMBER).FirstOrDefault();
            if(maxPlanRec!=null)
            {
                currentEvalNum = maxPlanRec.EVALNUMBER.HasValue ? maxPlanRec.EVALNUMBER.Value + 1 : 1;
            }
           
            DC_MultiteamCarePlanRecModel basicInfo = (from ip in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(x => x.FEENO == feeNo && x.IPDFLAG == "I")
                   join re in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ip.REGNO equals re.REGNO
                   select new DC_MultiteamCarePlanRecModel { 
                        FEENO=ip.FEENO,
                        REGNO=re.REGNO,
                        NURSEAIDES=ip.NURSEAIDES,
                        DISEASEINFO=re.DISEASEINFO,
                        ECOLOGICALMAP=re.ECOLOGICALMAP
                   }).FirstOrDefault();
            data = basicInfo;
            data.EVALNUMBER = currentEvalNum;
            SwRegEvalPlan swPlan = GetSocialWokerPlan(feeNo);
            DC_NursingPlanEval nursingPlan = GetNursingPlan(feeNo);

            DC_MultiteaMcarePlanEvalModel evalModel = new DC_MultiteaMcarePlanEvalModel();
            Mapper.CreateMap<SwRegEvalPlan, DC_MultiteaMcarePlanEvalModel>();
            Mapper.Map(swPlan, evalModel);
            DateTime now=DateTime.Now;
            evalModel.DISEASEINFO = data.DISEASEINFO;
            evalModel.ECOLOGICALMAP = data.ECOLOGICALMAP;
            evalModel.CHECKDATE = now;
            evalModel.CHECKEDBY = SecurityHelper.CurrentPrincipal.EmpNo;
            data.CHECKDATE = now;
            data.CHECKEDBY = SecurityHelper.CurrentPrincipal.EmpNo;
            if (nursingPlan.evalQuetion!=null&&nursingPlan.evalQuetion.Count > 0)
            {
                data.NurEvalDate = nursingPlan.EVALDATE.HasValue ? nursingPlan.EVALDATE.Value.ToShortDateString() : "";
                data.NurEvalNum = nursingPlan.EVALNUMBER;
                for(int i=0;i<nursingPlan.evalQuetion.Count;i++)
                {
                    if(nursingPlan.evalQuetion[i].QUESTIONCODE=="ADL")
                    {
                        evalModel.ADLSCORE = nursingPlan.evalQuetion[i].SCORE == null ? "" : nursingPlan.evalQuetion[i].SCORE.ToString();
                    }else if(nursingPlan.evalQuetion[i].QUESTIONCODE=="GDS")
                    {
                        evalModel.GODSSCORE = nursingPlan.evalQuetion[i].SCORE == null ? "" : nursingPlan.evalQuetion[i].SCORE.ToString();
                    }
                    else if (nursingPlan.evalQuetion[i].QUESTIONCODE == "IADL")
                    {
                        evalModel.IADLSCORE = nursingPlan.evalQuetion[i].SCORE == null ? "" : nursingPlan.evalQuetion[i].SCORE.ToString();
                    }
                    else if (nursingPlan.evalQuetion[i].QUESTIONCODE == "MMSE")
                    {
                        evalModel.MMSESCORE = nursingPlan.evalQuetion[i].SCORE == null ? "" : nursingPlan.evalQuetion[i].SCORE.ToString();
                    }
                }
            }
            if (swPlan.TaskGoalsStrategyModel!=null&&swPlan.TaskGoalsStrategyModel.Count > 0)
            {
                data.SWEvalDate = swPlan.EVALDATE.HasValue ? swPlan.EVALDATE.Value.ToShortDateString() : "";
                data.SWEvalNum = swPlan.EVALNUMBER;
                List<DC_MultiteamCarePlanModel> swCarePlan = (from task in swPlan.TaskGoalsStrategyModel.AsEnumerable()
                                                              select new DC_MultiteamCarePlanModel
                                                              {
                                                                  MAJORTYPE = "社工",
                                                                  QUESTIONTYPE = task.QUESTIONTYPE,
                                                                  ACTIVITY = task.PLANACTIVITY
                                                              }).ToList();
                carePlan.AddRange(swCarePlan);
            }
            if (nursingPlan.regCpl!=null&&nursingPlan.regCpl.Count > 0)
            {
                List<DC_MultiteamCarePlanModel> nursingCarePlan = (from task in nursingPlan.regCpl.AsEnumerable()
                                                              select new DC_MultiteamCarePlanModel
                                                              {
                                                                  MAJORTYPE = "护理",
                                                                  QUESTIONTYPE = task.CPDIA,
                                                                  ACTIVITY = task.NsCplActivity.Select(x => x.CplActivity).ToList().Join()
                                                              }).ToList();
                carePlan.AddRange(nursingCarePlan);
            }



            data.PlanEval = evalModel;
            data.CarePlan = carePlan;
            response.Data=data;
            return response;

        }

        public  BaseResponse<DC_MultiteamCarePlanRecModel>  SaveTransdisciplinary(DC_MultiteamCarePlanRecModel carePlanRec)
        {
            BaseResponse<DC_MultiteamCarePlanRecModel> response = new BaseResponse<DC_MultiteamCarePlanRecModel>();
            DC_MULTITEAMCAREPLANREC planRec = new DC_MULTITEAMCAREPLANREC();
            List<DC_MULTITEAMCAREPLAN> carePlan = new List<DC_MULTITEAMCAREPLAN>();
            List<DC_MultiteamCarePlanModel> carePlanModel = new List<DC_MultiteamCarePlanModel>();
            carePlanModel.AddRange(carePlanRec.CarePlan);
            DC_MULTITEAMCAREPLANEVAL planEval = new DC_MULTITEAMCAREPLANEVAL();
            DC_MultiteaMcarePlanEvalModel planEvalModel = new DC_MultiteaMcarePlanEvalModel();
            planEvalModel = carePlanRec.PlanEval;
            if (carePlanModel.Count>0)
            {
                Mapper.CreateMap<DC_MultiteamCarePlanModel, DC_MULTITEAMCAREPLAN>();
                Mapper.Map(carePlanModel, carePlan);
            }
            if (planEvalModel!=null)
            {
                planEvalModel.FEENO = carePlanRec.FEENO;
                planEvalModel.REGNO = carePlanRec.REGNO;
                planEvalModel.SEQNO = carePlanRec.SEQNO;
                planEvalModel.EVALNUMBER = carePlanRec.EVALNUMBER;
                Mapper.CreateMap<DC_MultiteaMcarePlanEvalModel, DC_MULTITEAMCAREPLANEVAL>();
                Mapper.Map(planEvalModel, planEval);
            }

            Mapper.CreateMap<DC_MultiteamCarePlanRecModel, DC_MULTITEAMCAREPLANREC>();
            Mapper.Map(carePlanRec, planRec);
            planRec.DC_MULTITEAMCAREPLAN = carePlan;
            planRec.DC_MULTITEAMCAREPLANEVAL = planEval;
            if (planRec.SEQNO>0)
            {
                unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().Update(planRec);
                unitOfWork.GetRepository<DC_MULTITEAMCAREPLANEVAL>().Update(planRec.DC_MULTITEAMCAREPLANEVAL);
                if( planRec.DC_MULTITEAMCAREPLAN.Count>0)
                {
                    planRec.DC_MULTITEAMCAREPLAN.ToList().ForEach(x => unitOfWork.GetRepository<DC_MULTITEAMCAREPLAN>().Update(x));
                }
            }
            else
            {
                planRec.CREATEDATE = DateTime.Now;
                planRec.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().Insert(planRec);
            }
           unitOfWork.Save();
           carePlanRec.SEQNO = planRec.SEQNO;
           response.Data = carePlanRec;
            return response;
        }

          #region 夸专业 历史记录
        public BaseResponse<List<DC_MultiteaMcarePlanEvalModel>> QueryTransdisciplinaryHistory(long feeNo)
        {
            BaseResponse<List<DC_MultiteaMcarePlanEvalModel>> response = new BaseResponse<List<DC_MultiteaMcarePlanEvalModel>>();
            List<DC_MultiteaMcarePlanEvalModel> data = new List<DC_MultiteaMcarePlanEvalModel>();
            List<DC_MULTITEAMCAREPLANEVAL> planEval = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANEVAL>().dbSet.Where(x => x.FEENO == feeNo).OrderBy(x => x.EVALNUMBER).ToList();
            Mapper.CreateMap<DC_MULTITEAMCAREPLANEVAL, DC_MultiteaMcarePlanEvalModel>();
            Mapper.Map(planEval, data);
            response.Data = data;
            return response;
        }


          #endregion


          #region 拉取社工参考项



        public SwRegEvalPlan GetSocialWokerPlan(long feeNo)
        {
            SwRegEvalPlan swPlan = new SwRegEvalPlan();
            var maxEvalPlan = unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet.Where(x => x.FEENO == feeNo).OrderByDescending(x => x.EVALNUMBER).FirstOrDefault();
            if (maxEvalPlan != null)
            {
                Mapper.CreateMap<DC_SWREGEVALPLAN, SwRegEvalPlan>();
                Mapper.Map(maxEvalPlan, swPlan);

                List<DC_TASKGOALSSTRATEGY> tasks = new List<DC_TASKGOALSSTRATEGY>();
                List<DC_TaskGoalsStrategyModel> tasksModel = new List<DC_TaskGoalsStrategyModel>();
                tasks.AddRange(maxEvalPlan.DC_TASKGOALSSTRATEGY);
                Mapper.CreateMap<DC_TASKGOALSSTRATEGY, DC_TaskGoalsStrategyModel>();
                Mapper.Map(tasks, tasksModel);

                swPlan.TaskGoalsStrategyModel = tasksModel;
            }
            return swPlan;
        }

        public BaseResponse<DC_MultiteamCarePlanRecModel> GetTransdisciplinaryPlanRec(long seqNo)
        {
            //加载子项目
            var response = new BaseResponse<DC_MultiteamCarePlanRecModel>();
            DC_MultiteamCarePlanRecModel multiPlan = new DC_MultiteamCarePlanRecModel();
            List<DC_MULTITEAMCAREPLAN> carePlanList = new List<DC_MULTITEAMCAREPLAN>();
            List<DC_MultiteamCarePlanModel> carePlanListModel = new List<DC_MultiteamCarePlanModel>();
            DC_MULTITEAMCAREPLANEVAL careEval = new DC_MULTITEAMCAREPLANEVAL();
            DC_MultiteaMcarePlanEvalModel careEvalModel = new DC_MultiteaMcarePlanEvalModel();


            DC_MULTITEAMCAREPLANREC model = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x => x.SEQNO == seqNo).FirstOrDefault();

            if (model != null)
            {
                carePlanList.AddRange(model.DC_MULTITEAMCAREPLAN);
                careEval = model.DC_MULTITEAMCAREPLANEVAL;

                Mapper.CreateMap<DC_MULTITEAMCAREPLANREC, DC_MultiteamCarePlanRecModel>();
                Mapper.Map(model, multiPlan);

                Mapper.CreateMap<DC_MULTITEAMCAREPLAN, DC_MultiteamCarePlanModel>();
                Mapper.Map(carePlanList, carePlanListModel);

                Mapper.CreateMap<DC_MULTITEAMCAREPLANEVAL, DC_MultiteaMcarePlanEvalModel>();
                Mapper.Map(careEval, careEvalModel);

                multiPlan.CarePlan = carePlanListModel;
                multiPlan.PlanEval = careEvalModel;
            }

            response.Data = multiPlan;

            return response;
        }


        public BaseResponse<DC_MultiteamCarePlanRecModel> GetLatestTransdisciplinaryPlanRec(long feeNo)
        {
            BaseResponse<DC_MultiteamCarePlanRecModel> response = new BaseResponse<DC_MultiteamCarePlanRecModel>();
            var maxEval = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x=>x.FEENO==feeNo).OrderByDescending(x => x.EVALNUMBER).FirstOrDefault();

            if (maxEval != null)
            {
                long seqNo = maxEval.SEQNO;
                response = QueryMultiCarePlanRec(seqNo);
            }
            return response;
        }
        
        #endregion

        #region 拉取护理参考项
        public DC_NursingPlanEval GetNursingPlan(long feeNo)
        {
            DC_NursingPlanEval nursingPlan = new DC_NursingPlanEval();
            var maxEvalPlan = unitOfWork.GetRepository<DC_NURSEINGPLANEVAL>().dbSet.Where(x => x.FEENO == feeNo).OrderByDescending(x => x.EVALNUMBER).FirstOrDefault();
            if (maxEvalPlan != null)
            {
                Mapper.CreateMap<DC_NURSEINGPLANEVAL, DC_NursingPlanEval>();
                Mapper.Map(maxEvalPlan, nursingPlan);

                List<DC_EvalQuetionModel> questionModel = new List<DC_EvalQuetionModel>();
                List<DC_EVALQUESTION> question = new List<DC_EVALQUESTION>();
                question.AddRange(maxEvalPlan.DC_EVALQUESTION);
                Mapper.CreateMap<DC_EVALQUESTION, DC_EvalQuetionModel>();
                Mapper.Map(question, questionModel);

                List<DC_RegCplModel> regCplModel = new List<DC_RegCplModel>();
                List<DC_REGCPL> regCpl = new List<DC_REGCPL>();
                regCpl.AddRange(maxEvalPlan.DC_REGCPL);

                regCpl.ForEach(x =>
                {
                    DC_RegCplModel cpl = new DC_RegCplModel();
                    List<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY> activityModel = new List<Entity.DC.Model.NSCPLACTIVITY>();
                    List<DC_NSCPLACTIVITY> activity = new List<DC_NSCPLACTIVITY>();
                    activity.AddRange(x.DC_NSCPLACTIVITY);
                    Mapper.CreateMap<DC_REGCPL, DC_RegCplModel>();
                    Mapper.Map(x, cpl);
                    Mapper.CreateMap<DC_NSCPLACTIVITY, NSCPLACTIVITY>();
                    Mapper.Map(activity, activityModel);
                    cpl.NsCplActivity = activityModel;
                    regCplModel.Add(cpl);
                });
           
                nursingPlan.evalQuetion = questionModel;
                nursingPlan.regCpl = regCplModel;

            }
            return nursingPlan;
        }

        #endregion

        #region 删除历史记录

        public BaseResponse DeleteTransdisciplinaryHis(long seqNo)
        {
            BaseResponse response=new BaseResponse();
            DC_MULTITEAMCAREPLANREC rec = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x => x.SEQNO == seqNo).FirstOrDefault();
            unitOfWork.GetRepository<DC_MULTITEAMCAREPLANEVAL>().Delete(rec.DC_MULTITEAMCAREPLANEVAL);
            if (rec.DC_MULTITEAMCAREPLAN.Count>0)
            {
                rec.DC_MULTITEAMCAREPLAN.ToList().ForEach(x => unitOfWork.GetRepository<DC_MULTITEAMCAREPLAN>().Delete(x));
            }
            unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().Delete(rec);
            unitOfWork.Save();

            return response;

        }


        #endregion

        #region TimeLine

        public BaseResponse<CasesTimeline> QueryCasesTimeline(long feeNo, string startDate, string endDate, int tag)
        {
            BaseResponse<CasesTimeline> response = new BaseResponse<CasesTimeline>();
            CasesTimeline data = new CasesTimeline();
            data.TimelineContainer = new List<TimelineContainer>();
            data.FEENO = feeNo;
            data.StartDate = startDate;
            data.EndDate = endDate;
            List<SwRegEvalPlan> swPlan = new List<SwRegEvalPlan>();
            List<DC_NursingPlanEval> nurPlan = new List<DC_NursingPlanEval>();

            if ((int)MajorType.护理照顾计划 == tag)
            {
                nurPlan = GetHisNursingPlanByDate(feeNo, startDate, endDate).OrderBy(x=>x.EVALDATE.HasValue?x.EVALDATE.Value:DateTime.Now).ToList();
            }
            else if ((int)MajorType.社工照顾计划 == tag)
            {
                swPlan = GetHisSocialWokerPlanByDate(feeNo, startDate, endDate).OrderBy(x => x.EVALDATE.HasValue ? x.EVALDATE.Value : DateTime.Now).ToList();
            }
            else
            {
                nurPlan = GetHisNursingPlanByDate(feeNo, startDate, endDate).OrderBy(x => x.EVALDATE.HasValue ? x.EVALDATE.Value : DateTime.Now).ToList();
                swPlan = GetHisSocialWokerPlanByDate(feeNo, startDate, endDate).OrderBy(x => x.EVALDATE.HasValue ? x.EVALDATE.Value : DateTime.Now).ToList();
            }
            TimelineContainer swContainer = new TimelineContainer();
            swContainer.MajorType = MajorType.社工照顾计划.ToString();
            swContainer.Timeline = new List<Timeline>();
            swContainer.Tag = (int)MajorType.社工照顾计划;

            TimelineContainer nurContainer = new TimelineContainer();
            nurContainer.MajorType = MajorType.护理照顾计划.ToString();
            nurContainer.Timeline = new List<Timeline>();
            nurContainer.Tag = (int)MajorType.护理照顾计划;
          
            foreach(SwRegEvalPlan plan in swPlan)
            {
                Timeline timeline = new Timeline();
                timeline.Title = "需求类别";
                timeline.Title1 = "项目";
                timeline.Header1 = "问题描述";
                timeline.Header2 = "介入方式或资源连接";
                timeline.StartDate = plan.EVALDATE.HasValue ? FormattingDate(plan.EVALDATE.Value) : "";
                if (!string.IsNullOrEmpty(timeline.StartDate))
                {
                    timeline.Month = plan.EVALDATE.Value.Month.ToString();
                    timeline.Day = plan.EVALDATE.Value.Day.ToString();
                }
                timeline.Plan = new List<TimelineText>();
                timeline.Plan = (from task in plan.TaskGoalsStrategyModel.AsEnumerable()
                                 select new TimelineText
                                {
                                    MAJORTYPE = "社工",
                                    CPDIA=task.CPDIA,
                                    QUESTIONDESC=task.QUESTIONDESC,
                                    QUESTIONTYPE = task.QUESTIONTYPE,
                                    ACTIVITY = new List<string>(){task.PLANACTIVITY},
                                }).ToList();

                swContainer.Timeline.Add(timeline);
            }

            foreach (DC_NursingPlanEval plan in nurPlan)
            {
                Timeline timeline = new Timeline();
                timeline.Title = "评估结果";
                timeline.Header1 = "护理诊断";
                timeline.Header2 = "措施";
                timeline.StartDate = plan.EVALDATE.HasValue ? FormattingDate(plan.EVALDATE.Value) : "";
                if (!string.IsNullOrEmpty(timeline.StartDate))
                {
                    timeline.Month = plan.EVALDATE.Value.Month.ToString();
                    timeline.Day = plan.EVALDATE.Value.Day.ToString();
                }
                timeline.Plan = new List<TimelineText>();
                if (plan.evalQuetion != null && plan.evalQuetion.Count > 0)
                {
                    for (int i = 0; i < plan.evalQuetion.Count; i++)
                    {
                        if (plan.evalQuetion[i].QUESTIONCODE == "ADL")
                        {
                            timeline.ADLSCORE = plan.evalQuetion[i].SCORE == null ? "尚未评估" : plan.evalQuetion[i].SCORE.ToString()+"分";
                        }
                        else if (plan.evalQuetion[i].QUESTIONCODE == "GDS")
                        {
                            timeline.GODSSCORE = plan.evalQuetion[i].SCORE == null ? "尚未评估" : plan.evalQuetion[i].SCORE.ToString() + "分";
                        }
                        else if (plan.evalQuetion[i].QUESTIONCODE == "IADL")
                        {
                            timeline.IADLSCORE = plan.evalQuetion[i].SCORE == null ? "尚未评估" : plan.evalQuetion[i].SCORE.ToString() + "分";
                        }
                        else if (plan.evalQuetion[i].QUESTIONCODE == "MMSE")
                        {
                            timeline.MMSESCORE = plan.evalQuetion[i].SCORE == null ? "尚未评估" : plan.evalQuetion[i].SCORE.ToString() + "分";
                        }
                    }
                }


                if (plan.regCpl != null && plan.regCpl.Count > 0)
                {
                    timeline.Plan = (from task in plan.regCpl.AsEnumerable()
                                                          select new TimelineText
                                                                       {
                                                                           MAJORTYPE = "护理",
                                                                           QUESTIONTYPE = task.CPDIA,
                                                                           ACTIVITY = task.NsCplActivity.Select(x => x.CplActivity).ToList()
                                                                       }).ToList();
                }

                nurContainer.Timeline.Add(timeline);
            }
            string StartDate="";
            if(swContainer.Timeline.OrderBy(x => x.StartDate).FirstOrDefault()!=null)
            {
                int count = swContainer.Timeline.Count();
                int midDateCount = count / 2;
                if (midDateCount > 0)
                {
                    StartDate = swContainer.Timeline[midDateCount].StartDate;
                }
                else
                {
                    StartDate = swContainer.Timeline[0].StartDate;
                }
          
            }
            if(nurContainer.Timeline.OrderBy(x => x.StartDate).FirstOrDefault()!=null)
            {
                int count = nurContainer.Timeline.Count();
                int midDateCount = count / 2;
                if (midDateCount > 0)
                {
                    StartDate = nurContainer.Timeline[midDateCount].StartDate;
                }
                else
                {
                    StartDate = nurContainer.Timeline[0].StartDate;
                }
            }

            data.StartDate = StartDate;
            if((int)MajorType.社工照顾计划==tag)
            {
                data.TimelineContainer.Add(swContainer);
            }
            else if ((int)MajorType.护理照顾计划 == tag)
            {
                data.TimelineContainer.Add(nurContainer);
            }
            else
            {
                data.TimelineContainer.Add(swContainer);
                data.TimelineContainer.Add(nurContainer);
            }
            response.Data = data;
            return response;
        }

        private string FormattingDate(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");

        }


        public List<SwRegEvalPlan> GetHisSocialWokerPlanByDate(long feeNo,string starDate,string endDate)
        {
            List<SwRegEvalPlan> result = new List<SwRegEvalPlan>();
            var PlanList = unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet.Where(x => x.FEENO == feeNo);
            if (!string.IsNullOrEmpty(starDate))
            {
                DateTime beginDate=DateTime.Parse(starDate);
               PlanList= PlanList.Where(x=>x.EVALDATE>=beginDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime finishDate = DateTime.Parse(starDate);
               PlanList= PlanList.Where(x => x.EVALDATE <= finishDate);
            }
            List<DC_SWREGEVALPLAN> list = PlanList.ToList();
             foreach(DC_SWREGEVALPLAN plan in list)
             {
                 SwRegEvalPlan swPlan = new SwRegEvalPlan();
                 Mapper.CreateMap<DC_SWREGEVALPLAN, SwRegEvalPlan>();
                 Mapper.Map(plan, swPlan);

                 List<DC_TASKGOALSSTRATEGY> tasks = new List<DC_TASKGOALSSTRATEGY>();
                 List<DC_TaskGoalsStrategyModel> tasksModel = new List<DC_TaskGoalsStrategyModel>();
                 tasks.AddRange(plan.DC_TASKGOALSSTRATEGY);
                 Mapper.CreateMap<DC_TASKGOALSSTRATEGY, DC_TaskGoalsStrategyModel>();
                 Mapper.Map(tasks, tasksModel);
                 swPlan.TaskGoalsStrategyModel = tasksModel;
                 result.Add(swPlan);
             }

             return result;
        }

        public List<DC_NursingPlanEval> GetHisNursingPlanByDate(long feeNo, string starDate, string endDate)
        {
            List<DC_NursingPlanEval> result = new List<DC_NursingPlanEval>();
            var PlanList = unitOfWork.GetRepository<DC_NURSEINGPLANEVAL>().dbSet.Where(x => x.FEENO == feeNo);
            if (!string.IsNullOrEmpty(starDate))
            {
                DateTime beginDate = DateTime.Parse(starDate);
              PlanList= PlanList.Where(x => x.EVALDATE >= beginDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime finishDate = DateTime.Parse(starDate);
               PlanList= PlanList.Where(x => x.EVALDATE <= finishDate);
            }

            List<DC_NURSEINGPLANEVAL> list = PlanList.ToList();
            foreach (DC_NURSEINGPLANEVAL plan in list)
            {
                DC_NursingPlanEval nursingPlan = new DC_NursingPlanEval();
                Mapper.CreateMap<DC_NURSEINGPLANEVAL, DC_NursingPlanEval>();
                Mapper.Map(plan, nursingPlan);

                List<DC_EvalQuetionModel> questionModel = new List<DC_EvalQuetionModel>();
                List<DC_EVALQUESTION> question = new List<DC_EVALQUESTION>();
                question.AddRange(plan.DC_EVALQUESTION);
                Mapper.CreateMap<DC_EVALQUESTION, DC_EvalQuetionModel>();
                Mapper.Map(question, questionModel);

                List<DC_RegCplModel> regCplModel = new List<DC_RegCplModel>();
                List<DC_REGCPL> regCpl = new List<DC_REGCPL>();
                regCpl.AddRange(plan.DC_REGCPL);

                regCpl.ForEach(x =>
                {
                    DC_RegCplModel cpl = new DC_RegCplModel();
                    List<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY> activityModel = new List<Entity.DC.Model.NSCPLACTIVITY>();
                    List<DC_NSCPLACTIVITY> activity = new List<DC_NSCPLACTIVITY>();
                    activity.AddRange(x.DC_NSCPLACTIVITY);
                    Mapper.CreateMap<DC_REGCPL, DC_RegCplModel>();
                    Mapper.Map(x, cpl);
                    Mapper.CreateMap<DC_NSCPLACTIVITY, NSCPLACTIVITY>();
                    Mapper.Map(activity, activityModel);
                    cpl.NsCplActivity = activityModel;
                    regCplModel.Add(cpl);
                });
                nursingPlan.evalQuetion = questionModel;
                nursingPlan.regCpl = regCplModel;

                result.Add(nursingPlan);

            }
            return result;
        }


        #endregion

    }
}

