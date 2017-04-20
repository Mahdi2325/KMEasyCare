using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class HealthRecordsService : BaseService, IHealthRecordsService
    {
        #region 量测数据

        public HealthRecords GetMeasurementInfo(int feeNo)
        {
            var response = new HealthRecords();
            response.MeasurementList = new List<Measurement>();
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            response.TotalRecordsNum = (from o in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet
                                        where o.MEASURETIME.HasValue
                                        && ((DateTime)o.MEASURETIME).Year == year && ((DateTime)o.MEASURETIME).Month == month && o.FEENO == feeNo
                                        select 0).Count();
            if (response.TotalRecordsNum > 0)
            {
                response.MeasurementList.Add(GetMeasurement("001", "体温", feeNo));
                response.MeasurementList.Add(GetMeasurement("002", "脉搏", feeNo));
                response.MeasurementList.Add(GetMeasurement("003", "呼吸", feeNo));
                response.MeasurementList.Add(GetMeasurement("004", "收缩压", feeNo));
                response.MeasurementList.Add(GetMeasurement("005", "舒张压", feeNo));
            }

            var weight = GetWeight("体重", feeNo);
            if (weight.ID != 0)
            {
                response.TotalRecordsNum += weight.TotalRecordNum;
                response.MeasurementList.Add(weight);
            }
            else
            {

                response.MeasurementList.Add(null);
            }

            response.MeasurementList.Add(GetBloodSugar("血糖", feeNo));

            if (response.MeasurementList != null && response.MeasurementList.Count > 0)
            {
                foreach (var item in response.MeasurementList)
                {
                    if (item != null)
                    {
                        if (item.Description != "正常")
                        {
                            response.GeneralDescriptionInfo = item.RecordName + item.Description;
                        }
                    }
                }
                if (string.IsNullOrEmpty(response.GeneralDescriptionInfo))
                {
                    response.GeneralDescriptionInfo = "各项量测数据均正常";
                }
            }
            else
            {
                response.GeneralDescriptionInfo = "未查询到有效数据";
            }

            return response;
        }
        public Measurement GetBloodSugar(string name, int feeno)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;

            Mapper.CreateMap<LTC_MEASUREDRECORD, Measurement>();
            var mea = unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet.OrderByDescending(m => m.MEASURETIME).FirstOrDefault(m => m.MEASURETIME.HasValue && ((DateTime)m.MEASURETIME).Year == year && ((DateTime)m.MEASURETIME).Month == month && (m.MEASUREITEMCODE == "S006001" || m.MEASUREITEMCODE == "S006002" || m.MEASUREITEMCODE == "S006003" || m.MEASUREITEMCODE == "S006004" || m.MEASUREITEMCODE == "S006005" || m.MEASUREITEMCODE == "S006006" || m.MEASUREITEMCODE == "S006007" || m.MEASUREITEMCODE == "S006008" || m.MEASUREITEMCODE == "S006009") && m.FEENO == feeno && m.ISDELETE != true);
            var measurement = Mapper.Map<Measurement>(mea);

            if (measurement != null)
            {
                measurement.TotalRecordNum = (from o in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet
                                              where o.MEASURETIME.HasValue
                                              && ((DateTime)o.MEASURETIME).Year == year && ((DateTime)o.MEASURETIME).Month == month && (o.MEASUREITEMCODE == "S006001" || o.MEASUREITEMCODE == "S006002" || o.MEASUREITEMCODE == "S006003" || o.MEASUREITEMCODE == "S006004" || o.MEASUREITEMCODE == "S006005" || o.MEASUREITEMCODE == "S006006" || o.MEASUREITEMCODE == "S006007" || o.MEASUREITEMCODE == "S006008" || o.MEASUREITEMCODE == "S006009") && o.FEENO == feeno && o.ISDELETE != true
                                              select 0).Count();

                measurement.NormalRecordNum = (from o in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet
                                               where o.MEASURETIME.HasValue
                                               && ((DateTime)o.MEASURETIME).Year == year && ((DateTime)o.MEASURETIME).Month == month && (o.MEASUREITEMCODE == "S006001" || o.MEASUREITEMCODE == "S006002" || o.MEASUREITEMCODE == "S006003" || o.MEASUREITEMCODE == "S006004" || o.MEASUREITEMCODE == "S006005" || o.MEASUREITEMCODE == "S006006" || o.MEASUREITEMCODE == "S006007" || o.MEASUREITEMCODE == "S006008" || o.MEASUREITEMCODE == "S006009") && o.DESCRIPTION == "正常" && o.FEENO == feeno && o.ISDELETE != true
                                               select 0).Count();

                measurement.ExceededRecordNum = (from o in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet
                                                 where o.MEASURETIME.HasValue
                                                 && ((DateTime)o.MEASURETIME).Year == year && ((DateTime)o.MEASURETIME).Month == month && (o.MEASUREITEMCODE == "S006001" || o.MEASUREITEMCODE == "S006002" || o.MEASUREITEMCODE == "S006003" || o.MEASUREITEMCODE == "S006004" || o.MEASUREITEMCODE == "S006005" || o.MEASUREITEMCODE == "S006006" || o.MEASUREITEMCODE == "S006007" || o.MEASUREITEMCODE == "S006008" || o.MEASUREITEMCODE == "S006009") && o.DESCRIPTION != "正常" && o.FEENO == feeno && o.ISDELETE != true
                                                 select 0).Count();
                measurement.RecordName = name;
            }
            return measurement;
        }
        public Measurement GetWeight(string name, int feeno)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            Measurement measurement = new Measurement();

            Mapper.CreateMap<LTC_UNPLANWEIGHTIND, UnPlanWeightInd>();
            var unweight = unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().dbSet.OrderByDescending(m => m.THISRECDATE).FirstOrDefault(m => m.THISRECDATE.HasValue && ((DateTime)m.THISRECDATE).Year == year && ((DateTime)m.THISRECDATE).Month == month && m.FEENO == feeno);
            var unplanweightind = Mapper.Map<UnPlanWeightInd>(unweight);
            if (unplanweightind != null)
            {
                measurement.RecordName = name;
                measurement.TotalRecordNum = (from o in unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().dbSet
                                              where o.THISRECDATE.HasValue
                                              && ((DateTime)o.THISRECDATE).Year == year && ((DateTime)o.THISRECDATE).Month == month && o.FEENO == feeno
                                              select 0).Count();

                measurement.NormalRecordNum = (from o in unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().dbSet
                                               where o.THISRECDATE.HasValue
                                               && ((DateTime)o.THISRECDATE).Year == year && ((DateTime)o.THISRECDATE).Month == month && o.BMIRESULTS == "正常" && o.FEENO == feeno
                                               select 0).Count();

                measurement.ExceededRecordNum = (from o in unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().dbSet
                                                 where o.THISRECDATE.HasValue
                                                 && ((DateTime)o.THISRECDATE).Year == year && ((DateTime)o.THISRECDATE).Month == month && o.BMIRESULTS != "正常" && o.FEENO == feeno
                                                 select 0).Count();
                measurement.ID = (int)unplanweightind.Id;
                measurement.Createby = unplanweightind.RecordBy;
                measurement.MeaSureTime = (DateTime)unplanweightind.ThisRecDate;
                measurement.FeeNo = unplanweightind.FeeNo;
                measurement.Description = unplanweightind.BMIResults;
                measurement.MeaSuredPerson = unplanweightind.RecordBy;
                measurement.MeaSuredValue = (float)unplanweightind.ThisWeight;
                measurement.MeasureItemCode = "011";
                measurement.MeaSureTime = unplanweightind.ThisRecDate;
            }
            return measurement;
        }
        public Measurement GetMeasurement(string code, string name, int feeno)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;

            Mapper.CreateMap<LTC_MEASUREDRECORD, Measurement>();
            var mea = unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet.OrderByDescending(m => m.MEASURETIME).FirstOrDefault(m => m.MEASURETIME.HasValue && ((DateTime)m.MEASURETIME).Year == year && ((DateTime)m.MEASURETIME).Month == month && m.MEASUREITEMCODE == code && m.FEENO == feeno && m.ISDELETE != true);
            var measurement = Mapper.Map<Measurement>(mea);

            if (measurement != null)
            {
                measurement.TotalRecordNum = (from o in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet
                                              where o.MEASURETIME.HasValue
                                              && ((DateTime)o.MEASURETIME).Year == year && ((DateTime)o.MEASURETIME).Month == month && o.MEASUREITEMCODE == code && o.FEENO == feeno && o.ISDELETE != true
                                              select 0).Count();

                measurement.NormalRecordNum = (from o in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet
                                               where o.MEASURETIME.HasValue
                                               && ((DateTime)o.MEASURETIME).Year == year && ((DateTime)o.MEASURETIME).Month == month && o.MEASUREITEMCODE == code && o.DESCRIPTION == "正常" && o.FEENO == feeno && o.ISDELETE != true
                                               select 0).Count();

                measurement.ExceededRecordNum = (from o in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet
                                                 where o.MEASURETIME.HasValue
                                                 && ((DateTime)o.MEASURETIME).Year == year && ((DateTime)o.MEASURETIME).Month == month && o.MEASUREITEMCODE == code && o.DESCRIPTION != "正常" && o.FEENO == feeno && o.ISDELETE != true
                                                 select 0).Count();
                measurement.RecordName = name;
            }
            return measurement;
        }
        public BaseResponse<IList<Measurement>> QueryHealthRecordList(BaseRequest<HeathRecordFilter> request)
        {
            var response = new BaseResponse<IList<Measurement>>();
            var q = from mr in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet
                    join mi in unitOfWork.GetRepository<LTC_MEASUREITEM>().dbSet on mr.MEASUREITEMCODE equals mi.ITEMCODE into mi_m
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on mr.MEASUREDPERSON equals emp.EMPNO into emp_e
                    from mi_mea in mi_m.DefaultIfEmpty()
                    from emp_emp in emp_e.DefaultIfEmpty()
                    select new
                    {
                        meau = mr,
                        ItemName = mi_mea.ITEMNAME,
                        EmpName = emp_emp.EMPNAME
                    };

            q = q.Where(m => m.meau.ORGID == SecurityHelper.CurrentPrincipal.OrgId && m.meau.FEENO == request.Data.FeeNo && m.meau.ISDELETE == false);

            //001 体温  002 脉搏  003  呼吸
            if (request.Data.ItemCode == "001" || request.Data.ItemCode == "002" | request.Data.ItemCode == "003")
            {
                q = q.Where(m => m.meau.MEASUREITEMCODE == request.Data.ItemCode);
            }

            if (request.Data.ItemCode == "004")
            {
                q = q.Where(m => m.meau.MEASUREITEMCODE == "004" || m.meau.MEASUREITEMCODE == "005");
            }
            if (request.Data.ItemCode == "006")
            {
                q = q.Where(m => m.meau.MEASUREITEMCODE == "S006001" || m.meau.MEASUREITEMCODE == "S006002" || m.meau.MEASUREITEMCODE == "S006003" || m.meau.MEASUREITEMCODE == "S006004" || m.meau.MEASUREITEMCODE == "S006005" || m.meau.MEASUREITEMCODE == "S006006" || m.meau.MEASUREITEMCODE == "S006007" || m.meau.MEASUREITEMCODE == "S006008" || m.meau.MEASUREITEMCODE == "S006009");
            }

            q = q.OrderByDescending(m => m.meau.MEASURETIME);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<Measurement>();
                foreach (dynamic item in list)
                {
                    Measurement newItem = Mapper.DynamicMap<Measurement>(item.meau);
                    newItem.RecordName = item.ItemName;
                    newItem.Empname = item.EmpName;
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

        public BaseResponse<object> GetMeasureDataForExtApi(BaseRequest<MeasureDataFilter> request)
        {
            var response = new BaseResponse<object>();
            var q = from mr in unitOfWork.GetRepository<LTC_MEASUREDRECORD>().dbSet.Where(m => m.ISDELETE != true)
                    join mi in unitOfWork.GetRepository<LTC_MEASUREITEM>().dbSet on mr.MEASUREITEMCODE equals mi.ITEMCODE
                    select new
                    {
                        FeeNo = mr.FEENO,
                        MeasureTime = mr.MEASURETIME,
                        ItemCode = mr.MEASUREITEMCODE,
                        ItemName = mi.ITEMNAME,
                        Value = mr.MEASUREDVALUE,
                        Desc = mr.DESCRIPTION
                    };
            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            if (request.Data.SDate.HasValue)
            {
                q = q.Where(m => m.MeasureTime >= request.Data.SDate);
            }
            if (request.Data.EDate.HasValue)
            {
                q = q.Where(m => m.MeasureTime < request.Data.EDate);
            }
            q = q.OrderByDescending(m => m.MeasureTime);
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

        public BaseResponse SaveMeaSuredRecord(Measurement baseRequest)
        {
            baseRequest.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
            baseRequest.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateTime = DateTime.Now;
            baseRequest.IsDelete = false;
            return base.Save<LTC_MEASUREDRECORD, Measurement>(baseRequest, (q) => q.ID == baseRequest.ID);
        }

        #endregion
        public HealthRecords GetDrugInfo(int feeNo)
        {
            var response = new HealthRecords();
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var list = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(w => w.STATUS != 8 && w.NSID == SecurityHelper.CurrentPrincipal.OrgId && w.FEENO == feeNo && w.TAKETIME.Year == year && w.TAKETIME.Month == month && w.ISDELETE != true).ToList();
            list = list.OrderByDescending(m => m.TAKETIME).ToList();
            response.TotalRecordsNum = list.Count();
            if (list != null && list.Count > 0)
            {
                response.GeneralDescriptionInfo = "服用" + list.FirstOrDefault().CNNAME;
            }
            else
            {
                response.GeneralDescriptionInfo = "无用药记录";
            }
            return response;
        }


        #region 用药记录
        public HealthRecords GetMedicationInfo(int feeNo)
        {
            var response = new HealthRecords();
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;

            var q = from it in unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().dbSet
                    join n in unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().dbSet on it.SEQNO equals n.SEQNO into nns
                    from nn in nns.DefaultIfEmpty()
                    join m in unitOfWork.GetRepository<LTC_MEDICINE>().dbSet on it.MEDID equals m.MEDID into mms
                    from mm in mms.DefaultIfEmpty()
                    join e2 in unitOfWork.GetRepository<LTC_VISITHOSPITAL>().dbSet on nn.VISITHOSP equals e2.HOSPNO into vr_e2
                    from vr_emp2 in vr_e2.DefaultIfEmpty()
                    join e3 in unitOfWork.GetRepository<LTC_VISITDEPT>().dbSet on nn.VISITDEPT equals e3.DEPTNO into vr_e3
                    from vr_emp3 in vr_e3.DefaultIfEmpty()
                    join e4 in unitOfWork.GetRepository<LTC_VISITDOCTOR>().dbSet on nn.VISITDOCTOR equals e4.DEPTNO into vr_e4
                    from vr_emp4 in vr_e4.DefaultIfEmpty()
                    select new VisitPrescription
                    {
                        PId = it.PID,
                        SeqNo = it.SEQNO,
                        MedId = it.MEDID,
                        TakeQty = it.TAKEQTY,
                        Qty = it.QTY,
                        Freq = it.FREQ,
                        Freqday = it.FREQDAY,
                        Freqqty = it.FREQQTY,
                        TakeWay = it.TAKEWAY,
                        Freqtime = it.FREQTIME,
                        LongFlag = it.LONGFLAG,
                        UseFlag = it.USEFLAG,
                        StartDate = it.STARTDATE,
                        EndDate = it.ENDDATE,
                        Description = it.DESCRIPTION,
                        OrgId = it.ORGID,
                        EngName = mm.ENGNAME,
                        CnName = it.CNNAME,
                        MedKind = mm.MEDKIND,
                        FeeNo = nn.FEENO,
                        TakeTime = it.TAKETIME,
                        VisitDoctorName = vr_emp4.DOCNAME,
                        VisitHospName = vr_emp2.HOSPNAME,
                        VisitDeptName = vr_emp3.DEPTNAME,
                        VisitType = nn.VISITTYPE,
                        TakeDays = nn.TAKEDAYS
                    };
            q = q.Where(m => ((DateTime)m.StartDate).Year == year && ((DateTime)m.StartDate).Month == month && ((DateTime)m.EndDate).Year == year && ((DateTime)m.EndDate).Month == month && m.FeeNo == feeNo && m.UseFlag != false);
            q = q.OrderByDescending(m => m.TakeTime);
            response.TotalRecordsNum = q.Count();
            if (response.TotalRecordsNum != 0)
            {
                response.GeneralDescriptionInfo = q.FirstOrDefault().CnName;
                var medRecord = string.Empty;
                if (q.FirstOrDefault().Freqday.HasValue && q.FirstOrDefault().Freqqty.HasValue)
                {
                    if (q.FirstOrDefault().Freqday == 0)
                    {

                        response.GeneralDescriptionInfo += ",1天/";
                    }
                    else
                    {
                        response.GeneralDescriptionInfo += "," + q.FirstOrDefault().Freqday + "天/";
                    }
                    if (q.FirstOrDefault().Freqqty == 0)
                    {

                        response.GeneralDescriptionInfo += "1次";
                    }
                    else
                    {
                        response.GeneralDescriptionInfo += q.FirstOrDefault().Freqqty + "次";
                    }
                }
            }
            else
            {
                response.GeneralDescriptionInfo = "无用药记录";
            }
            return response;

        }
        #endregion

        #region 体检记录(生化检查记录)
        public HealthRecords GetBiochemistryInfo(int feeNo)
        {
            var response = new HealthRecords();
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            List<LTC_CHECKREC> regQuestion = unitOfWork.GetRepository<LTC_CHECKREC>().dbSet.Where(m => m.FEENO == feeNo && ((DateTime)m.CHECKDATE).Year == year && ((DateTime)m.CHECKDATE).Month == month).OrderByDescending(m => m.CHECKDATE).ToList();

            response.TotalRecordsNum = regQuestion.Count;
            if (response.TotalRecordsNum != 0)
            {
                if (string.IsNullOrEmpty(regQuestion[0].DISEASEDESC))
                {
                    response.GeneralDescriptionInfo = "病况:无";
                }
                else
                {
                    response.GeneralDescriptionInfo = "病况:" + regQuestion[0].DISEASEDESC;
                }
            }
            else
            {
                response.GeneralDescriptionInfo = "无体检记录";
            }

            return response;
        }
        #endregion

        #region 评估记录
        public HealthRecords GetEvaluationInfo(int feeNo)
        {
            var response = new HealthRecords();
            var hea = from n in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(m => m.FEENO == feeNo && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId && (m.QUESTIONID == (int)QuestionCode.ADL || m.QUESTIONID == (int)QuestionCode.MMSE || m.QUESTIONID == (int)QuestionCode.IADL || m.QUESTIONID == (int)QuestionCode.SORE || m.QUESTIONID == (int)QuestionCode.FALL))
                      join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.EVALUATEBY equals e.EMPNO into res
                      join q in unitOfWork.GetRepository<LTC_QUESTION>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on n.QUESTIONID equals q.QUESTIONID into ques
                      from re in res.DefaultIfEmpty()
                      from question in ques.DefaultIfEmpty()
                      orderby n.EVALDATE descending
                      select new
                      {
                          OutValue = n,
                          EmpName = re.EMPNAME,
                          QuesName = question.QUESTIONNAME
                      };

            response.TotalRecordsNum = hea.Count();
            if (response.TotalRecordsNum != 0)
            {
                response.GeneralDescriptionInfo = hea.FirstOrDefault().QuesName + "评估结果：" + hea.FirstOrDefault().OutValue.ENVRESULTS;

                response.ADLEvaluation = GetEvaluation((int)QuestionCode.ADL, feeNo);
                response.MMSEEvaluation = GetEvaluation((int)QuestionCode.MMSE, feeNo);
                response.IADLEvaluation = GetEvaluation((int)QuestionCode.IADL, feeNo);
                response.SoreEvaluation = GetEvaluation((int)QuestionCode.SORE, feeNo);
                response.FallEvaluation = GetEvaluation((int)QuestionCode.FALL, feeNo);
            }
            else
            {
                response.GeneralDescriptionInfo = "无评估记录数据";
            }
            return response;
        }
        #endregion

        public BaseResponse<IList<DrugRecord>> QuerydrugRecord(BaseRequest<DrugRecordinfoFilter> request)
        {
            var response = base.Query<LTC_DRUGRECORD, DrugRecord>(request, (q) =>
            {
                q = q.Where(m => m.FEENO == request.Data.FeeNo && m.ISDELETE != true && m.STATUS != 8);
                if (request.Data.StartDate != null)
                {
                    q = q.Where(m => m.TAKETIME >= request.Data.StartDate);
                }
                if (request.Data.EndDate != null)
                {
                    q = q.Where(m => m.TAKETIME <= request.Data.EndDate);
                }
                q = q.OrderByDescending(m => m.FEENO);
                return q;
            });
            return response;
        }

        public EvaluationRecord GetEvaluation(int questionid, int feeNo)
        {
            var response = new EvaluationRecord();
            var hea = from n in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(m => m.FEENO == feeNo && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId && m.QUESTIONID == questionid)
                      join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.EVALUATEBY equals e.EMPNO into res
                      from re in res.DefaultIfEmpty()
                      orderby n.EVALDATE descending
                      select new
                      {
                          OutValue = n,
                          EmpName = re.EMPNAME,
                      };
            response.TotalRecordNum = hea.Count();
            if (response.TotalRecordNum != 0)
            {
                response.QUESTIONID = hea.FirstOrDefault().OutValue.QUESTIONID;
                response.EVALDATE = hea.FirstOrDefault().OutValue.EVALDATE;
                response.EVALUATEBY = hea.FirstOrDefault().EmpName;
                response.SCORE = hea.FirstOrDefault().OutValue.SCORE;
                response.ENVRESULTS = hea.FirstOrDefault().OutValue.ENVRESULTS;
            }
            return response;
        }


    }
}
