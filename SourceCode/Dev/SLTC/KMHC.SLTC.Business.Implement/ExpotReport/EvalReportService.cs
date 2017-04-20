using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class EvalReportService : BaseService, IEvalReportService
    {
        public List<EvalReportHeader> GetEvalData(long _feeNo, DateTime? startDate, DateTime? endDate, string code, string floorId)
        {
            var q = from a in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet
                    join g in unitOfWork.GetRepository<LTC_QUESTION>().dbSet.Where(m => m.ORGID == "tpl") on a.QUESTIONID equals g.QUESTIONID
                    join b in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on a.REGNO equals b.REGNO
                    join c in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals c.REGNO
                    join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on c.FLOOR equals j.FLOORID into aj
                    join k in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on c.ROOMNO equals k.ROOMNO into ak
                    join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.EVALUATEBY equals d.EMPNO into ad
                    join o in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals o.ORGID into ao
                    from e in ad.DefaultIfEmpty()
                    from f in ao.DefaultIfEmpty()
                    from l in aj.DefaultIfEmpty()
                    from m in ak.DefaultIfEmpty()
                    select new EvalReportHeader()
                    {
                        RecordId = a.RECORDID,
                        Name = b.NAME,
                        FeeNo = c.FEENO,
                        ResidengNo = b.RESIDENGNO,
                        Area = l.FLOORNAME + m.ROOMNAME,
                        BedNo = c.BEDNO,
                        Brithdate = b.BRITHDATE,
                        CreateDate = a.EVALDATE,
                        NextDate = a.NEXTEVALDATE,
                        EvaluateBy = e.EMPNAME,
                        Result = a.ENVRESULTS,
                        Score = a.SCORE,
                        OrgId = f.ORGID,
                        Org = f.ORGNAME,
                        QuestionId = a.QUESTIONID,
                        QuestionCode = g.CODE,
                        FloorId = l.FLOORID
                    };
            q = q.Where(m => m.QuestionCode == code);
            if (floorId != "")
            {
                q = q.Where(m => m.FloorId == floorId);
            }
            else
            {
                if (_feeNo != 0)
                {
                    q = q.Where(m => m.FeeNo == _feeNo);
                }
            }
            if (startDate.HasValue)
            {
                q = q.Where(m => m.CreateDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                q = q.Where(m => m.CreateDate <= endDate.Value);
            }
            var evalDataList = q.ToList();
            foreach (var question in evalDataList)
            {
                question.answer = GetAnswers(question.RecordId);

                if (question.CreateDate.HasValue)
                {
                    question.CDTW = string.Format("{0}/{1}/{2}", question.CreateDate.Value.Year, question.CreateDate.Value.Month, question.CreateDate.Value.Day);
                }
                if (question.NextDate.HasValue)
                {
                    question.NDTW = string.Format("{0}/{1}/{2}", question.NextDate.Value.Year, question.NextDate.Value.Month, question.NextDate.Value.Day);
                }
                if (question.QuestionId.HasValue)
                {
                    var regQuestionList = (from a in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(o => o.QUESTIONID == question.QuestionId)
                                           select new
                                           {
                                               FeeNo = a.FEENO,
                                           }).ToList();
                    question.OneEvaluateTotal = regQuestionList.Count(it => it.FeeNo == question.FeeNo);
                    question.EvaluateTotal = regQuestionList.Count;
                }
                //if (question.Brithdate.HasValue)
                //{
                //    question.Age = (DateTime.Now.Year - question.Brithdate.Value.Year).ToString(); ;
                //}
            }
            return evalDataList;
        }
        public IEnumerable<Answer> GetAnswers(long recordId)
        {
            var answers = (from a in unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().dbSet.Where(o => o.RECORDID == recordId && o.MAKERID.HasValue && o.RECORDID.HasValue)
                           join b in unitOfWork.GetRepository<LTC_MAKERITEMLIMITEDVALUE>().dbSet.Where(o => o.LIMITEDVALUE.HasValue) on a.LIMITEDVALUEID equals b.LIMITEDVALUEID
                           select new Answer()
                           {
                               Id = a.MAKERID,
                               SurveyId = a.RECORDID.Value,

                               Score = b.LIMITEDVALUE,
                               Value = b.LIMITEDVALUENAME
                           }).ToList();
            return answers;
        }
    }
}
