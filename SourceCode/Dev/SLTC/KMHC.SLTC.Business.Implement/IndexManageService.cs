/*
创建人: 肖国栋
创建日期:2016-03-18
说明:指标管理
*/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using KM.Common;

namespace KMHC.SLTC.Business.Implement
{
    public class IndexManageService : BaseService, IIndexManageService
    {
        #region 跌倒指标
        public BaseResponse<IList<FallIncidentEvent>> QueryFallIncidentEvent(BaseRequest<FallIncidentEventFilter> request)
        {
            //var response = base.Query<LTC_FALLINCIDENTEVENT, FallIncidentEvent>(request, (q) =>
            //{
            //    q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    q = q.OrderBy(m => m.ID);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<FallIncidentEvent>> response = new BaseResponse<IList<FallIncidentEvent>>();
            var q = from n in unitOfWork.GetRepository<LTC_FALLINCIDENTEVENT>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.SETTLEBY equals e.EMPNO into res2
                    from re2 in res2.DefaultIfEmpty()
                    select new
                    {
                        FallIncidentEvent = n,
                        RecordName = re.EMPNAME,
                        SettleName = re2.EMPNAME

                    };
            q = q.Where(m => m.FallIncidentEvent.FEENO == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.FallIncidentEvent.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<FallIncidentEvent>();
                foreach (dynamic item in list)
                {
                    FallIncidentEvent newItem = Mapper.DynamicMap<FallIncidentEvent>(item.FallIncidentEvent);
                    newItem.RecordNameBy = item.RecordName;
                    newItem.SettleNameBy = item.SettleName;
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

        public BaseResponse<FallIncidentEvent> GetFallIncidentEvent(long id)
        {
            return base.Get<LTC_FALLINCIDENTEVENT, FallIncidentEvent>((q) => q.ID == id);
        }

        public BaseResponse<FallIncidentEvent> SaveFallIncidentEvent(FallIncidentEvent request)
        {
            return base.Save<LTC_FALLINCIDENTEVENT, FallIncidentEvent>(request, (q) => q.ID == request.ID);
        }

        public BaseResponse DeleteFallIncidentEvent(long id)
        {
            return base.Delete<LTC_FALLINCIDENTEVENT>(id);
        }

        #endregion

        #region 压疮指标
        public BaseResponse<IList<BedSoreRec>> QueryBedSoreRec(BaseRequest<BedSoreRecFilter> request)
        {
            var response = base.Query<LTC_BEDSOREREC, BedSoreRec>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                q = q.OrderBy(m => m.ORGID);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<BedSoreRec>> QueryBedSoreRecExtend(BaseRequest<BedSoreRecFilter> request)
        {
            //var response = base.Query<LTC_BEDSOREREC, BedSoreRec>(request, (q) =>
            //{
            //    if (!string.IsNullOrEmpty(request.Data.OrgId))
            //    {
            //        q = q.Where(m => m.ORGID == request.Data.OrgId && m.FEENO == request.Data.FeeNo);
            //    }
            //    if (!string.IsNullOrWhiteSpace(request.Data.Degree))
            //    {
            //        q = q.Where(m => request.Data.Degree == m.DEGREE);
            //    }
            //    q = q.OrderBy(m => m.CREATEDATE);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<BedSoreRec>> response = new BaseResponse<IList<BedSoreRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_BEDSOREREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.EVALUTEBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        BedSoreRec = n,
                        EvaluteNameBy = re.EMPNAME,
                    };
            q = q.Where(m => m.BedSoreRec.FEENO == request.Data.FeeNo && m.BedSoreRec.ORGID == request.Data.OrgId);
            q = q.OrderByDescending(m => m.BedSoreRec.SEQ);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<BedSoreRec>();
                foreach (dynamic item in list)
                {
                    BedSoreRec newItem = Mapper.DynamicMap<BedSoreRec>(item.BedSoreRec);
                    newItem.EvaluteNameBy = item.EvaluteNameBy;
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
        public BaseResponse<BedSoreRec> GetBedSoreRecExtend(BaseRequest<BedSoreRecFilter> request)
        {
            var response = base.Get<LTC_BEDSOREREC, BedSoreRec>((q) => q.FEENO == request.Data.FeeNo && q.ORGID == request.Data.OrgId);
            if (response != null && response.Data != null && response.Data.Seq > 0)
            {
                Mapper.CreateMap<LTC_BEDSORECHGREC, BedSoreChgrec>();
                List<LTC_BEDSORECHGREC> list = null;
                var entities = from it in unitOfWork.GetRepository<LTC_BEDSORECHGREC>().dbSet where it.SEQ == response.Data.Seq select it;
                list = entities.ToList();
                response.Data.Detail = Mapper.Map<List<BedSoreChgrec>>(list);
            }
            return response;
        }
        public BaseResponse<BedSoreRec> GetBedSoreRec(long seq)
        {
            return base.Get<LTC_BEDSOREREC, BedSoreRec>((q) => q.SEQ == seq);
        }

        public BaseResponse<BedSoreRec> SaveBedSoreRec(BedSoreRec request)
        {
            var result = base.Save<LTC_BEDSOREREC, BedSoreRec>(request, (q) => q.SEQ == request.Seq);

            //当增加主档,并且前端传入的数据含有Detail时,增加detail。
            if (request.Seq > 0 && result != null && request.Detail != null && request.Detail.Count > 0)
            {
                request.Detail.ForEach((p) =>
                {
                    p.Seq = result.Data.Seq;
                    p.CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
                    p.CreateDate = DateTime.Now;
                });
                base.Save<LTC_BEDSORECHGREC, BedSoreChgrec>(request.Detail, (q) => request.Detail.Any(p => p.Id == q.ID));
            }
            return result;
        }

        public BaseResponse DeleteBedSoreRec(long seq)
        {
            base.Delete<LTC_BEDSORECHGREC>(o => o.SEQ == seq);
            return base.Delete<LTC_BEDSOREREC>(seq);
        }
        #endregion

        #region 约束指标
        public BaseResponse<IList<ConstraintRec>> QueryConstraintRec(BaseRequest<ConstraintRecFilter> request)
        {
            var response = base.Query<LTC_CONSTRAINTREC, ConstraintRec>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                q = q.OrderBy(m => m.ORGID);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<ConstraintRec>> QueryConstraintRecExtend(BaseRequest<ConstraintRecFilter> request)
        {
            //var response = base.Query<LTC_CONSTRAINTREC, ConstraintRec>(request, (q) =>
            //{
            //    if (!string.IsNullOrEmpty(request.Data.OrgId))
            //    {
            //        q = q.Where(m => m.ORGID == request.Data.OrgId && m.FEENO == request.Data.FeeNo);
            //    }
            //    q = q.OrderBy(m => m.CREATEDATE);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<ConstraintRec>> response = new BaseResponse<IList<ConstraintRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_CONSTRAINTREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.CANCELEXECBY equals e.EMPNO into res2
                    from re2 in res2.DefaultIfEmpty()
                    select new
                    {
                        ConstraintRec = n,
                        RecordByName = re.EMPNAME,
                        CancelExecByName = re2.EMPNAME
                    };
            q = q.Where(m => m.ConstraintRec.FEENO == request.Data.FeeNo && m.ConstraintRec.ORGID == request.Data.OrgId);
            q = q.OrderByDescending(m => m.ConstraintRec.CREATEDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ConstraintRec>();
                foreach (dynamic item in list)
                {
                    ConstraintRec newItem = Mapper.DynamicMap<ConstraintRec>(item.ConstraintRec);
                    newItem.RecordByName = item.RecordByName;
                    newItem.CancelExecByName = item.CancelExecByName;
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

        public BaseResponse<ConstraintRec> GetConstraintRec(long seqNo)
        {
            return base.Get<LTC_CONSTRAINTREC, ConstraintRec>((q) => q.SEQNO == seqNo);
        }
        public BaseResponse<ConstraintRec> GetConstraintRecExtend(BaseRequest<ConstraintRecFilter> request)
        {

            var response = base.Get<LTC_CONSTRAINTREC, ConstraintRec>((q) => q.FEENO == request.Data.FeeNo && q.ORGID == request.Data.OrgId);
            if (response != null && response.Data != null && response.Data.SeqNo > 0)
            {
                Mapper.CreateMap<LTC_CONSTRAINSBEVAL, ConstrainsBeval>();
                List<LTC_CONSTRAINSBEVAL> list = null;
                var entities = from it in unitOfWork.GetRepository<LTC_CONSTRAINSBEVAL>().dbSet where it.SEQNO == response.Data.SeqNo select it;
                list = entities.ToList();
                response.Data.Detail = Mapper.Map<List<ConstrainsBeval>>(list);
            }
            return response;
        }

        public BaseResponse<ConstraintRec> SaveConstraintRec(ConstraintRec request)
        {
            var response = base.Save<LTC_CONSTRAINTREC, ConstraintRec>(request, (q) => q.SEQNO == request.SeqNo);

            //当增加主档,并且前端传入的数据含有Detail时,增加detail。
            if (response != null && response.Data.SeqNo > 0 && request.Detail != null && request.Detail.Count > 0)
            {
                request.Detail.ForEach((p) =>
                {
                    p.SeqNo = response.Data.SeqNo;
                });
                base.Save<LTC_CONSTRAINSBEVAL, ConstrainsBeval>(request.Detail, (q) => request.Detail.Any(p => p.Id == q.ID));
            }
            return response;
        }

        public BaseResponse DeleteConstraintRec(long seqNo)
        {
            base.Delete<LTC_CONSTRAINSBEVAL>(o => o.SEQNO == seqNo);
            return base.Delete<LTC_CONSTRAINTREC>(seqNo);
        }
        #endregion

        #region 感染指标


        public BaseResponse<IList<InfectionItem>> QueryInfectionItem(BaseRequest<InfectionItemFilter> request)
        {
            var response = base.Query<LTC_INFECTIONITEM, InfectionItem>(request, (q) =>
            {
                q = q.OrderByDescending(m => m.ITEMCODE);
                return q;
            });
            return response;
        }


        public BaseResponse<IList<SymptomItem>> QuerySymptomItem(BaseRequest<SymptomItemFilter> request)
        {
            var response = base.Query<LTC_SYMPOTMITEM, SymptomItem>(request, (q) =>
            {

                if (request != null && !string.IsNullOrEmpty(request.Data.ItemCode))
                {
                    q = q.Where(m => m.ITEMCODE == request.Data.ItemCode);
                }
                q = q.OrderByDescending(m => m.SYMPOTMCODE);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<InfectionInd>> QueryInfectionInd(BaseRequest<InfectionIndFilter> request)
        {
            BaseResponse<IList<InfectionInd>> response = new BaseResponse<IList<InfectionInd>>();
            var q = from n in unitOfWork.GetRepository<LTC_INFECTIONIND>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        InfectionInd = n,
                        EmpName = re.EMPNAME
                    };

            if (request != null && !string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.InfectionInd.ORGID == request.Data.OrgId);
            }
            if (request != null && !string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
            {
                q = q.Where(m => m.InfectionInd.FEENO == request.Data.FeeNo);
            }
            if (request != null && !string.IsNullOrEmpty(request.Data.RegNo.ToString()))
            {
                q = q.Where(m => m.InfectionInd.REGNO == request.Data.RegNo);
            }
            q = q.OrderByDescending(m => m.InfectionInd.RECDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<InfectionInd>();
                foreach (dynamic item in list)
                {
                    InfectionInd newItem = Mapper.DynamicMap<InfectionInd>(item.InfectionInd);
                    newItem.RecordNameBy = item.EmpName;
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

        public BaseResponse<InfectionInd> GetInfectionInd(long seqNo)
        {
            return base.Get<LTC_INFECTIONIND, InfectionInd>((q) => q.SEQNO == seqNo);
        }

        public BaseResponse<InfectionInd> SaveInfectionInd(InfectionInd request)
        {
            if (request.SeqNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_INFECTIONIND, InfectionInd>(request, (q) => q.SEQNO == request.SeqNo);
        }

        public BaseResponse DeleteInfectionInd(long seqNo)
        {
            unitOfWork.GetRepository<LTC_INFECTIONSYMPOTM>().Delete(m => m.SEQNO == seqNo);
            unitOfWork.GetRepository<LTC_LABEXAMREC>().Delete(m => m.SEQNO == seqNo);
            var response = base.Delete<LTC_INFECTIONIND>(seqNo);
            return response;
        }


        public BaseResponse<IList<InfectionSympotm>> QueryInfectionSympotm(BaseRequest<InfectionSympotmFilter> request)
        {
            var response = base.Query<LTC_INFECTIONSYMPOTM, InfectionSympotm>(request, (q) =>
            {
                if (request.Data.SeqNo.HasValue)
                {
                    q = q.Where(m => m.SEQNO == request.Data.SeqNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<InfectionSympotm> GetInfectionSympotm(long id)
        {
            return base.Get<LTC_INFECTIONSYMPOTM, InfectionSympotm>((q) => q.ID == id);
        }

        public BaseResponse<InfectionSympotm> SaveInfectionSympotm(InfectionSympotm request)
        {
            if (request.Id == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
            }
            return base.Save<LTC_INFECTIONSYMPOTM, InfectionSympotm>(request, (q) => q.ID == request.Id);
        }

        public void SaveInfectionSympotms(List<InfectionSympotm> request)
        {
            request.ForEach((item) =>
            {
                if (item.Id == 0)
                {
                    item.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                    item.CreateDate = DateTime.Now;
                }
                base.Save<LTC_INFECTIONSYMPOTM, InfectionSympotm>(item, (q) => q.ID == item.Id);
            });
            return;
        }

        public void DeleteInfectionSympotms(long[] ids)
        {
            foreach (var id in ids)
            {
                base.Delete<LTC_INFECTIONSYMPOTM>(id);
            }
            return;
        }

        public BaseResponse DeleteInfectionSympotm(long id)
        {
            return base.Delete<LTC_INFECTIONSYMPOTM>(id);
        }


        public BaseResponse<IList<LabExamRec>> QueryLabExamRec(BaseRequest<LabExamRecFilter> request)
        {
            var response = base.Query<LTC_LABEXAMREC, LabExamRec>(request, (q) =>
            {
                if (request.Data.SeqNo.HasValue)
                {
                    q = q.Where(m => m.SEQNO == request.Data.SeqNo);
                }
                q = q.OrderByDescending(m => m.EXAMDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<LabExamRec> GetLabExamRec(long id)
        {
            return base.Get<LTC_LABEXAMREC, LabExamRec>((q) => q.ID == id);
        }

        public BaseResponse<LabExamRec> SaveLabExamRec(LabExamRec request)
        {
            if (request.Id == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
            }
            return base.Save<LTC_LABEXAMREC, LabExamRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteLabExamRec(long id)
        {
            return base.Delete<LTC_LABEXAMREC>(id);
        }

        #endregion

        #region 初步疼痛评估
        public BaseResponse<IList<PainEvalRec>> QueryPainEvalRec(BaseRequest<PainEvalRecFilter> request)
        {
            var response = base.Query<LTC_PAINEVALREC, PainEvalRec>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                q = q.OrderBy(m => m.ORGID);
                return q;
            });
            return response;
        }

        /// <summary>
        /// 获取疼痛列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>返回参数</returns>
        public BaseResponse<IList<PainEvalRec>> QueryPainEvalRecExtend(BaseRequest<PainEvalRecFilter> request)
        {
            //var response = base.Query<LTC_PAINEVALREC, PainEvalRec>(request, (q) =>
            //{
            //    if (!string.IsNullOrEmpty(request.Data.OrgId))
            //    {
            //        q = q.Where(m => m.ORGID == request.Data.OrgId && m.FEENO == request.Data.FeeNo);
            //    }
            //    q = q.OrderBy(m => m.ORGID);
            //    return q;
            //});
            //return response;    

            BaseResponse<IList<PainEvalRec>> response = new BaseResponse<IList<PainEvalRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_PAINEVALREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.NEXTEVALUATEBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        PainEvalRec = n,
                        NextEvaluateByName = re.EMPNAME,
                    };
            q = q.Where(m => m.PainEvalRec.FEENO == request.Data.FeeNo);
            q = q.Where(m => m.PainEvalRec.ORGID == request.Data.OrgId);
            q = q.OrderByDescending(m => m.PainEvalRec.ORGID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<PainEvalRec>();
                foreach (dynamic item in list)
                {
                    PainEvalRec newItem = Mapper.DynamicMap<PainEvalRec>(item.PainEvalRec);
                    newItem.NextEvaluateByName = item.NextEvaluateByName;
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

        public BaseResponse<PainEvalRec> GetPainEvalRecExtend(BaseRequest<PainEvalRecFilter> request)
        {
            var response = base.Get<LTC_PAINEVALREC, PainEvalRec>((q) => q.FEENO == request.Data.FeeNo && q.ORGID == request.Data.OrgId);
            if (response != null && response.Data != null && response.Data.SeqNo > 0)
            {
                Mapper.CreateMap<LTC_PAINBODYPART, PainBodyPartRec>();
                List<LTC_PAINBODYPART> list = null;
                var entities = from it in unitOfWork.GetRepository<LTC_PAINBODYPART>().dbSet where it.SEQNO == response.Data.SeqNo select it;
                list = entities.ToList();
                response.Data.Detail = Mapper.Map<List<PainBodyPartRec>>(list);
            }
            return response;
        }

        public BaseResponse<PainEvalRec> GetPainEvalRec(long seqNo)
        {
            return base.Get<LTC_PAINEVALREC, PainEvalRec>((q) => q.SEQNO == seqNo);
        }

        public BaseResponse<PainEvalRec> SavePainEvalRec(PainEvalRec request)
        {
            var result = base.Save<LTC_PAINEVALREC, PainEvalRec>(request, (q) => q.SEQNO == request.SeqNo);

            //当增加主档,并且前端传入的数据含有Detail时,增加detail。
            //if (request.SeqNo > 0 && result != null && request.Detail != null && request.Detail.Count > 0)
            //{
            //    request.Detail.ForEach((p) =>
            //    {
            //        p.SeqNo = result.Data.SeqNo;
            //        p.CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            //        p.CreateDate = DateTime.Now;
            //    });
            //    base.Save<LTC_PAINBODYPART, PainBodyPartRec>(request.Detail, (q) => request.Detail.Any(p => p.Id == q.ID));
            //}
            return result;
        }

        public BaseResponse DeletePainEvalRec(long seqNo)
        {
            base.Delete<LTC_PAINBODYPART>(o => o.SEQNO == seqNo);
            return base.Delete<LTC_PAINEVALREC>(seqNo);
        }
        #endregion

        #region 非计划性减重指标
        public BaseResponse<IList<UnPlanWeightInd>> QueryUnPlanWeightInd(BaseRequest<UnPlanWeightIndFilter> request)
        {
            BaseResponse<IList<UnPlanWeightInd>> response = new BaseResponse<IList<UnPlanWeightInd>>();

            var set = unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().dbSet as IQueryable<LTC_UNPLANWEIGHTIND>;
            if (request != null && !string.IsNullOrEmpty(request.Data.OrgId))
            {
                set = set.Where(o => o.ORGID == request.Data.OrgId);
            }
            if (request != null)
            {
                set = set.Where(o => o.REGNO == request.Data.RegNo && o.FEENO == request.Data.FeeNo);
            }
            if (request.Data.ThisRecDate1 != null)
                set = set.Where(o => o.THISRECDATE >= request.Data.ThisRecDate1);
            if (request.Data.ThisRecDate2 != null)
                set = set.Where(o => o.THISRECDATE <= request.Data.ThisRecDate2);
            var q = from n in set
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    join _reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on n.REGNO equals _reg.REGNO into _regs
                    from re in res.DefaultIfEmpty()
                    from reg in _regs.DefaultIfEmpty()
                    select new
                    {
                        UnPlanWeightInd = n,
                        EmpName = re.EMPNAME,
                        Name=reg.NAME
                    };

            q = q.OrderByDescending(m => m.UnPlanWeightInd.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<UnPlanWeightInd>();
                foreach (dynamic item in list)
                {
                    UnPlanWeightInd newItem = Mapper.DynamicMap<UnPlanWeightInd>(item.UnPlanWeightInd);
                    newItem.RecordNameBy = item.EmpName;
                    newItem.Name = item.Name;
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

        public BaseResponse<UnPlanWeightInd> GetUnPlanWeightInd(long id)
        {
            return base.Get<LTC_UNPLANWEIGHTIND, UnPlanWeightInd>((q) => q.ID == id);
        }

        public BaseResponse<UnPlanWeightInd> SaveUnPlanWeightInd(UnPlanWeightInd request)
        {
            if (request.Id == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_UNPLANWEIGHTIND, UnPlanWeightInd>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteUnPlanWeightInd(long id)
        {
            return base.Delete<LTC_UNPLANWEIGHTIND>(id);
        }

        public BaseResponse<IList<UnPlanWeightInd>> QueryUnPlanWeightList(BaseRequest<UnPlanWeightIndFilter> request)
        {
            BaseResponse<IList<UnPlanWeightInd>> response = new BaseResponse<IList<UnPlanWeightInd>>();
           
                var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
                        join _reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals _reg.REGNO into _regs
                        join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on ipd.FLOOR equals f.FLOORID into fs
                        join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on ipd.ROOMNO equals r.ROOMNO into rs 
                        from reg in _regs.DefaultIfEmpty()
                        from floor in fs.DefaultIfEmpty()
                        from room in rs.DefaultIfEmpty()
                        select new UnPlanWeightInd
                        {
                            FeeNo = ipd.FEENO,
                            RegNo = reg.REGNO,
                            Name = reg.NAME,
                            FloorName = floor.FLOORID,
                            RoomName = room.ROOMNO
                        };



                if (!string.IsNullOrEmpty(request.Data.RoomName)) q = q.Where(m => m.RoomName == request.Data.RoomName);
                if (!string.IsNullOrEmpty(request.Data.FloorName)) q = q.Where(m => m.FloorName == request.Data.FloorName);
                q = q.OrderByDescending(m => m.FeeNo);
                response.RecordsCount = q.Count();
                Action<IList> mapperResponse = (IList list) =>
                {
                    response.Data = Mapper.DynamicMap<List<UnPlanWeightInd>>(list);
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
        public BaseResponse<List<UnPlanWeightInd>> SaveList(List<UnPlanWeightInd> request)
        {

            var list = Mapper.DynamicMap<List<UnPlanWeightInd>>(request);
            List<UnPlanWeightInd> saveList = new List<UnPlanWeightInd>();
            UnPlanWeightInd temp = null;
            Mapper.CreateMap<UnPlanWeightInd, LTC_UNPLANWEIGHTIND>();
            foreach (var item in request)
            {
                temp = new UnPlanWeightInd();
                temp.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                temp.ThisWeight = item.ThisWeight;
                temp.ThisHeight = item.ThisHeight;
                temp.Waist = item.Waist;
                temp.FeeNo = item.FeeNo;
                temp.KneeLen = item.KneeLen;
                temp.RecordBy = item.RecordBy;
                temp.RegNo = item.RegNo;
                temp.ThisRecDate = item.ThisRecDate;
                temp.UnPlanFlag = item.UnPlanFlag;
                temp.Hipline = item.Hipline;
                temp.BMI = item.BMI;
                temp.BMIResults = item.BMIResults;
                var model = Mapper.Map<LTC_UNPLANWEIGHTIND>(temp);
                if (model.ID == 0)
                {
                    unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().Insert(model);
                }
                else
                {
                    unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().Update(model);
                }
            }
            unitOfWork.Save();
            BaseResponse<List<UnPlanWeightInd>> response = new BaseResponse<List<UnPlanWeightInd>>();
            return response;
        }
        #endregion

        #region 非计划性住院指标
        public BaseResponse<IList<UnPlanEdipd>> QueryUnPlanEdipd(BaseRequest<UnPlanEdipdFilter> request)
        {
            var response = base.Query<LTC_UNPLANEDIPD, UnPlanEdipd>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data != null)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo && m.REGNO == request.Data.RegNo);
                }
                q = q.OrderByDescending(m => m.INDATE);
                return q;
            });
            return response;
        }


        public BaseResponse<IList<UnPlanEdipd>> QueryNewUnPlanEdipd(BaseRequest<UnPlanEdipdFilter> request)
        {

            var response = base.Query<LTC_UNPLANEDIPD, UnPlanEdipd>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data != null)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo && m.REGNO == request.Data.RegNo);
                }
                q = q.OrderByDescending(m => m.INDATE);
                return q;
            });
            return response;
        }


        public BaseResponse<UnPlanEdipd> GetUnPlanEdipd(long id)
        {
            return base.Get<LTC_UNPLANEDIPD, UnPlanEdipd>((q) => q.ID == id);
        }

        public BaseResponse<UnPlanEdipd> SaveUnPlanEdipd(UnPlanEdipd request)
        {
            #region 移除管路信息
            var keys = new[] { "001", "002", "003", "004", "005" };
            for (int i = 0; i < 5; i++)
            {
                ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
                socialWorkerManageService.RemovePipelineRec(Convert.ToInt32(request.FeeNo), keys[i], Convert.ToDateTime(request.InDate), "非计划入院自动移除");
            }
            #endregion

            unitOfWork.BeginTransaction();
            if (request.Id == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            var health = new Health();
            health.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
            health.FEENO = request.FeeNo ?? 0;
            health.NOTCOOKREASON = "001"; //住院
            health.STARTDATE = request.InDate;
            health.ENDDATE = request.OutDate;
            base.Save<LTC_REGHEALTH, Health>(health, (q) => q.FEENO == health.FEENO, new List<string> { "ORGID", "FEENO", "NOTCOOKREASON", "STARTDATE", "ENDDATE" });
            var response = base.Save<LTC_UNPLANEDIPD, UnPlanEdipd>(request, (q) => q.ID == request.Id);
            unitOfWork.Commit();
            return response;
        }

        public BaseResponse DeleteUnPlanEdipd(long id)
        {
            return base.Delete<LTC_UNPLANEDIPD>(id);
        }
        #endregion

        #region 压疮指标明细

        public BaseResponse<IList<BedSoreChgrec>> QueryBedSoreChgrec(BaseRequest<BedSoreChgrecFilter> request)
        {
            //var response = base.Query<LTC_BEDSORECHGREC, BedSoreChgrec>(request, (q) =>
            //{
            //    if (request.Data.Seq!=null)
            //    {
            //        q = q.Where(m => m.SEQ == request.Data.Seq);
            //    }
            //    q = q.OrderByDescending(m => m.CREATEDATE);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<BedSoreChgrec>> response = new BaseResponse<IList<BedSoreChgrec>>();
            var q = from n in unitOfWork.GetRepository<LTC_BEDSORECHGREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.NURSE equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        BedSoreChgrec = n,
                        NurseName = re.EMPNAME,
                    };
            q = q.Where(m => m.BedSoreChgrec.SEQ == request.Data.Seq);
            q = q.OrderByDescending(m => m.BedSoreChgrec.CREATEDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<BedSoreChgrec>();
                foreach (dynamic item in list)
                {
                    BedSoreChgrec newItem = Mapper.DynamicMap<BedSoreChgrec>(item.BedSoreChgrec);
                    newItem.NurseName = item.NurseName;
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

        public BaseResponse<BedSoreChgrec> GetBedSoreChgrecExtend(long seq)
        {
            return base.Get<LTC_BEDSORECHGREC, BedSoreChgrec>((q) => q.SEQ == seq);
        }

        public BaseResponse<BedSoreChgrec> GetBedSoreChgrec(long id)
        {
            return base.Get<LTC_BEDSORECHGREC, BedSoreChgrec>((q) => q.ID == id);
        }

        public BaseResponse<BedSoreChgrec> SaveBedSoreChgrec(BedSoreChgrec request)
        {
            return base.Save<LTC_BEDSORECHGREC, BedSoreChgrec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteBedSoreChgrec(long id)
        {
            return base.Delete<LTC_BEDSORECHGREC>(id);
        }
        #endregion

        #region 疼痛明细
        public BaseResponse<IList<PainBodyPartRec>> QueryPainBodyPartRec(BaseRequest<PainBodyPartRecFilter> request)
        {
            var response = base.Query<LTC_PAINBODYPART, PainBodyPartRec>(request, (q) =>
            {
                if (request.Data.SeqNo != null)
                {
                    q = q.Where(m => m.SEQNO == request.Data.SeqNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }
        public BaseResponse<PainBodyPartRec> GetPainBodyPartRecExtend(long seqNo)
        {
            return base.Get<LTC_PAINBODYPART, PainBodyPartRec>((q) => q.SEQNO == seqNo);
        }

        public BaseResponse<PainBodyPartRec> GetPainBodyPartRec(long id)
        {
            return base.Get<LTC_PAINBODYPART, PainBodyPartRec>((q) => q.ID == id);
        }

        public BaseResponse<PainBodyPartRec> SavePainBodyPartRec(PainBodyPartRec request)
        {
            return base.Save<LTC_PAINBODYPART, PainBodyPartRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeletePainBodyPartRec(long id)
        {
            return base.Delete<LTC_PAINBODYPART>(id);
        }
        #endregion

        #region 约束明细

        public BaseResponse<IList<ConstrainsBeval>> QueryConstrainsBeval(BaseRequest<ConstrainsBevalFilter> request)
        {
            //var response = base.Query<LTC_CONSTRAINSBEVAL, ConstrainsBeval>(request, (q) =>
            //{
            //    if (request.Data.SeqNo != null)
            //    {
            //        q = q.Where(m => m.SEQNO == request.Data.SeqNo);
            //    }
            //    q = q.OrderByDescending(m => m.EVALDATE);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<ConstrainsBeval>> response = new BaseResponse<IList<ConstrainsBeval>>();
            var q = from n in unitOfWork.GetRepository<LTC_CONSTRAINSBEVAL>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.EVALUATEBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        ConstrainsBeval = n,
                        EvaluateByName = re.EMPNAME,
                    };
            q = q.Where(m => m.ConstrainsBeval.SEQNO == request.Data.SeqNo);
            q = q.OrderByDescending(m => m.ConstrainsBeval.EVALDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ConstrainsBeval>();
                foreach (dynamic item in list)
                {
                    ConstrainsBeval newItem = Mapper.DynamicMap<ConstrainsBeval>(item.ConstrainsBeval);
                    newItem.EvaluateByName = item.EvaluateByName;
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
        public BaseResponse<ConstrainsBeval> GetConstrainsBevalExtend(long seqNo)
        {
            return base.Get<LTC_CONSTRAINSBEVAL, ConstrainsBeval>((q) => q.SEQNO == seqNo);
        }

        public BaseResponse<ConstrainsBeval> GetConstrainsBeval(long id)
        {
            return base.Get<LTC_CONSTRAINSBEVAL, ConstrainsBeval>((q) => q.ID == id);
        }

        public BaseResponse<ConstrainsBeval> SaveConstrainsBeval(ConstrainsBeval request)
        {
            return base.Save<LTC_CONSTRAINSBEVAL, ConstrainsBeval>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteConstrainsBeval(long id)
        {
            return base.Delete<LTC_CONSTRAINSBEVAL>(id);
        }
        #endregion

    }
}
