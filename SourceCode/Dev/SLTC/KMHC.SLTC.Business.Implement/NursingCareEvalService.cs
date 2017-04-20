using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
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
    public class NursingCareEvalService : BaseService, INursingCareEvalService
    {
        public object QueryCareEvalInfo(BaseRequest<NursingFilter> request)
        {
            BaseResponse<IList<NCIEvaluate>> response = new BaseResponse<IList<NCIEvaluate>>();
            var q = from n in unitOfWork.GetRepository<LTC_NCIEVALUATE>().dbSet
                    select new NCIEvaluate
                    {
                        NCIEvaluateid = n.NCIEVALUATEID,
                        Feeno = n.FEENO,
                        Name = n.NAME,
                        Ssno = n.SSNO,
                        Starttime = n.STARTTIME,
                        Residentno = n.RESIDENTNO,
                        Bedno = n.BEDNO, 
                    };
            q = q.Where(m => m.Feeno == request.Data.FeeNo);
            var evallist = q.ToList();
            foreach (var eval in evallist)
            {
                eval.TotalScore = unitOfWork.GetRepository<LTC_NCIEVALUATEDTL>().dbSet.Where(m => m.NCIEVALUATEID == eval.NCIEvaluateid).Sum(m => m.SCORE);
            }

            response.RecordsCount = evallist.Count();

            response.Data = evallist.OrderByDescending(m => m.Starttime).Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
            response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            return response;
        }


        public BaseResponse<ResBasicInfo> GetNursingCareEvalQues(string itemType, int feeNo, int evaluateid)
        {

            var response = new BaseResponse<ResBasicInfo>();
            response.Data = new ResBasicInfo();
            if (evaluateid == -1)
            {
                #region 住民基本详情
                var q = from a in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == feeNo)
                        join e in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on a.REGNO equals e.REGNO into res
                        from re in res.DefaultIfEmpty()
                        select new ResBasicInfo
                        {
                            Feeno = a.FEENO,
                            Name = re.NAME,
                            Ssno = re.SSNO,
                            Residentno = re.RESIDENGNO,
                            Bedno = a.BEDNO,
                        };
                var list = q.ToList();
                if (list != null && list.Count > 0)
                {
                    response.Data = q.FirstOrDefault();
                }
                #endregion
            }
            else
            {
                #region 住民基本详情
                var q = from a in unitOfWork.GetRepository<LTC_NCIEVALUATE>().dbSet.Where(m => m.NCIEVALUATEID == evaluateid)
                        select new ResBasicInfo
                        {
                            NCIEvaluateid = a.NCIEVALUATEID,
                            Feeno = a.FEENO,
                            Name = a.NAME,
                            Ssno = a.SSNO,
                            Starttime = a.STARTTIME,
                            Residentno = a.RESIDENTNO,
                            Bedno = a.BEDNO,
                        };
                var list = q.ToList();
                if (list != null && list.Count > 0)
                {
                    response.Data = q.FirstOrDefault();
                }
                #endregion
            }

            #region 长期护理保险评价表详情
            response.Data.nursingEval = new List<NursingCareEval>();
            List<NursingCareEval> nursingCareEval = new List<NursingCareEval>();
            List<LTC_WORKITEM> workItem = unitOfWork.GetRepository<LTC_WORKITEM>().dbSet.Where(m => m.ITEMTYPE == itemType).OrderBy(m => m.ORDER).ToList();
            Mapper.CreateMap<LTC_WORKITEM, NursingCareEval>();
            Mapper.Map(workItem, nursingCareEval);
            response.Data.nursingEval = nursingCareEval;
            foreach (var subitem in response.Data.nursingEval)
            {
                List<WorkSubitem> subItem = new List<WorkSubitem>();
                List<LTC_WORKSUBITEM> sub = unitOfWork.GetRepository<LTC_WORKSUBITEM>().dbSet.Where(m => m.ITEMCODE == subitem.Itemcode).OrderBy(m => m.ORDER).ToList();
                Mapper.CreateMap<LTC_WORKSUBITEM, WorkSubitem>();
                Mapper.Map(sub, subItem);
                subitem.QuesItem = subItem;
                if (subitem.QuesItem != null && subItem.Count > 0)
                {
                    subitem.MaxTotalnum = sub.Sum(m => m.MAXINUM.Value);
                }
                foreach (var score in subItem)
                {
                    score.scoreOfValue = new List<ScoreOfValue>();
                    for (int i = 0; i <= score.Maxinum; i++)
                    {
                        ScoreOfValue sval = new ScoreOfValue();
                        sval.Score = i + "分";
                        sval.value = i;
                        score.scoreOfValue.Add(sval);
                    }
                    NCIEvaluatedtl evaldtl = new NCIEvaluatedtl();
                    if (evaluateid != -1)
                    {

                        Mapper.CreateMap<LTC_NCIEVALUATEDTL, NCIEvaluatedtl>();
                        var nciEval = unitOfWork.GetRepository<LTC_NCIEVALUATEDTL>().dbSet.Where(m => m.ITEMCODE == subitem.Itemcode && m.SUBITEMID == score.Id && m.NCIEVALUATEID == evaluateid).FirstOrDefault();
                        Mapper.Map(nciEval, evaldtl);
                    }
                    score.ncievaluatedtl = evaldtl;
                }
            }

            #endregion
            return response;
        }

        public object SaveResBasicInfo(ResBasicInfo baserequest)
        {
            var response = new ResBasicInfo();
            if (baserequest.NCIEvaluateid != 0)
            {
                updateBasicInfo(baserequest);
                updatencievaluatedtl(baserequest);
                unitOfWork.Save();
            }
            else
            {
                var nciEvaluateId = InsertBasicInfo(baserequest);
                Insertncievaluatedtl(baserequest, nciEvaluateId);
                unitOfWork.Save();
            }
            return response;
        }

        public void updateBasicInfo(ResBasicInfo baserequest)
        {
            NCIEvaluate evaluate = new NCIEvaluate();
            Mapper.CreateMap<LTC_NCIEVALUATE, NCIEvaluate>();
            var nciEval = unitOfWork.GetRepository<LTC_NCIEVALUATE>().dbSet.Where(m => m.NCIEVALUATEID == baserequest.NCIEvaluateid).FirstOrDefault();
            Mapper.Map(nciEval, evaluate);
            evaluate.Feeno = baserequest.Feeno;
            evaluate.Name = baserequest.Name;
            evaluate.Ssno = baserequest.Ssno;
            evaluate.Starttime = baserequest.Starttime;
            evaluate.Residentno = baserequest.Residentno;
            evaluate.Bedno = baserequest.Bedno;
            evaluate.Updateby = SecurityHelper.CurrentPrincipal.EmpNo;
            evaluate.Updatetime = DateTime.Now;
            base.Save<LTC_NCIEVALUATE, NCIEvaluate>(evaluate, (q) => q.NCIEVALUATEID == evaluate.NCIEvaluateid);
        }

        public void updatencievaluatedtl(ResBasicInfo baserequest)
        {
            if (baserequest != null && baserequest.nursingEval != null && baserequest.nursingEval.Count > 0)
            {
                foreach (var nurEval in baserequest.nursingEval)
                {
                    foreach (var quesitem in nurEval.QuesItem)
                    {
                        NCIEvaluatedtl evaldtl = new NCIEvaluatedtl();
                        Mapper.CreateMap<LTC_NCIEVALUATEDTL, NCIEvaluatedtl>();
                        var nciEval = unitOfWork.GetRepository<LTC_NCIEVALUATEDTL>().dbSet.Where(m => m.ITEMCODE == quesitem.Itemcode && m.SUBITEMID == quesitem.Id && m.NCIEVALUATEID == baserequest.NCIEvaluateid).FirstOrDefault();
                        Mapper.Map(nciEval, evaldtl);
                        evaldtl.Score = quesitem.ncievaluatedtl.Score;
                        base.Save<LTC_NCIEVALUATEDTL, NCIEvaluatedtl>(evaldtl, (q) => q.ID == evaldtl.ID);
                    }
                }
            }
        }

        public int InsertBasicInfo(ResBasicInfo baserequest)
        { 
          NCIEvaluate evaluate = new NCIEvaluate();
          evaluate.Feeno = baserequest.Feeno;
          evaluate.Name = baserequest.Name;
          evaluate.Ssno = baserequest.Ssno;
          evaluate.Starttime = baserequest.Starttime;
          evaluate.Residentno = baserequest.Residentno;
          evaluate.Bedno = baserequest.Bedno;
          evaluate.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
          evaluate.Createtime = DateTime.Now;
          Mapper.CreateMap<NCIEvaluate, LTC_NCIEVALUATE>();
          var model = Mapper.Map<LTC_NCIEVALUATE>(evaluate);
          unitOfWork.GetRepository<LTC_NCIEVALUATE>().Insert(model);
          return model.NCIEVALUATEID;
        }

        public void Insertncievaluatedtl(ResBasicInfo baserequest,int nciEvaluateId)
        {
            if (baserequest != null && baserequest.nursingEval != null && baserequest.nursingEval.Count > 0)
            {
                foreach (var nurEval in baserequest.nursingEval)
                {
                    foreach (var quesitem in nurEval.QuesItem)
                    {
                        NCIEvaluatedtl evaldtl = new NCIEvaluatedtl();
                        evaldtl.NCIEvaluateid = nciEvaluateId;
                        evaldtl.Itemcode = quesitem.Itemcode;
                        evaldtl.Subitemid = quesitem.Id;
                        evaldtl.Score = quesitem.ncievaluatedtl.Score;
                        Mapper.CreateMap<NCIEvaluatedtl, LTC_NCIEVALUATEDTL>();
                        var model = Mapper.Map<LTC_NCIEVALUATEDTL>(evaldtl);
                        unitOfWork.GetRepository<LTC_NCIEVALUATEDTL>().Insert(model);
                    }
                }
            }
        }

        public BaseResponse DeleteEvalCare(long id)
        {
           List<NCIEvaluatedtl> evaldtl = new List<NCIEvaluatedtl>();
            Mapper.CreateMap<LTC_NCIEVALUATEDTL, NCIEvaluatedtl>();
            var nciEval = unitOfWork.GetRepository<LTC_NCIEVALUATEDTL>().dbSet.Where(m => m.NCIEVALUATEID == id).OrderBy(m => m.SUBITEMID).ToList();
            Mapper.Map(nciEval, evaldtl);
            if (evaldtl != null && evaldtl.Count > 0)
            {
                foreach (var item in evaldtl)
                {
                    base.Delete<LTC_NCIEVALUATEDTL>(item.ID);
                }
            }
           return base.Delete<LTC_NCIEVALUATE>(id);
        }
    }
}
