using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.TSG;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class TsgService : BaseService, ITsgService
    {
        public BaseResponse<IList<TsgCategory>> QueryTsgData(BaseRequest<TsgCategoryFilter> request)
        {
            BaseResponse<IList<TsgCategory>> response = new BaseResponse<IList<TsgCategory>>();
            Mapper.CreateMap<TSG_CATEGORY, TsgCategory>();
            Mapper.CreateMap<TSG_QUESTION, TsgQuestion>();
            Mapper.CreateMap<TSG_ANSWER, TsgAnswer>();
            var q = from m in unitOfWork.GetRepository<TSG_CATEGORY>().dbSet
                    select m;
            q = q.Where(m => m.STATUS == true);
            q = q.OrderBy(m => m.ID);
            List<TSG_CATEGORY> list = q.ToList();
            var data = new List<TsgCategory>();
            var tsgCategory = new TsgCategory();
            tsgCategory.Class = "in active";
            foreach (var item in list)
            {
                tsgCategory.Id = item.ID;
                tsgCategory.Code = item.CODE;
                tsgCategory.Name = item.NAME;
                tsgCategory.TsgQuestion = Mapper.Map<IList<TsgQuestion>>(item.TSG_QUESTION);
                foreach (var subItem in tsgCategory.TsgQuestion)
                {
                    foreach (var question in item.TSG_QUESTION)
                    {
                        if (subItem.Id == question.ID)
                        {
                            subItem.TsgAnswer = Mapper.Map<IList<TsgAnswer>>(question.TSG_ANSWER);
                        }
                    }
                }
                data.Add(tsgCategory);
                tsgCategory = new TsgCategory();
                tsgCategory.Class = "";
            }

            response.Data = data;
            return response;
        }

        public BaseResponse<IList<TsgCategory>> QueryTsgCategory(BaseRequest<TsgCategoryFilter> request)
        {
            BaseResponse<IList<TsgCategory>> response = new BaseResponse<IList<TsgCategory>>();
            Mapper.CreateMap<TSG_CATEGORY, TsgCategory>();
            var q = from m in unitOfWork.GetRepository<TSG_CATEGORY>().dbSet
                    select m;
            q = q.Where(m => m.STATUS == true);
            q = q.OrderBy(m => m.ID);
            List<TSG_CATEGORY> list = q.ToList();
            var data = new List<TsgCategory>();
            var tsgCategory = new TsgCategory();
            foreach (var item in list)
            {
                tsgCategory.Id = item.ID;
                tsgCategory.Code = item.CODE;
                tsgCategory.Name = item.NAME;
                data.Add(tsgCategory);
                tsgCategory = new TsgCategory();
            }
            response.Data = data;
            return response;
        }

        public BaseResponse<IList<TsgQuestion>> QueryTsgQuestion(BaseRequest<TsgQuestionFilter> request)
        {
            BaseResponse<IList<TsgQuestion>> response = new BaseResponse<IList<TsgQuestion>>();
            Mapper.CreateMap<TSG_QUESTION, TsgQuestion>();
            Mapper.CreateMap<TSG_ANSWER, TsgAnswer>();
            var q = from m in unitOfWork.GetRepository<TSG_QUESTION>().dbSet
                    select m;
            q = q.Where(m => m.NAME.Contains(request.Data.KeyWord ?? ""));
            q = q.OrderBy(m => m.CATEGORYCODE);
            List<TSG_QUESTION> list = q.ToList();
            var data = new List<TsgQuestion>();
            var tsgQuestion = new TsgQuestion();

            foreach (var item in list)
            {
                tsgQuestion.Id = item.ID;
                tsgQuestion.CategoryCode = item.CATEGORYCODE;
                tsgQuestion.Name = item.NAME;
                tsgQuestion.QueImageUrl = item.QUEIMAGEURL;
                tsgQuestion.Description = item.DESCRIPTION;
                tsgQuestion.OrderSeq = item.ORDERSEQ;
                tsgQuestion.UpdateTime = item.UPDATETIME;
                tsgQuestion.Status = item.STATUS;
                tsgQuestion.TsgAnswer = Mapper.Map<IList<TsgAnswer>>(item.TSG_ANSWER);
                data.Add(tsgQuestion);
                tsgQuestion = new TsgQuestion();
            }

            response.Data = data;
            return response;
        }

        public BaseResponse<TsgQuestionData> GetTsgQuestion(int id)
        {
            var tsgQuestionData = new BaseResponse<TsgQuestionData>()
            {
                Data = new TsgQuestionData()
                {
                    TsgQuestion = new TsgQuestion(),
                    TsgAnswer = new TsgAnswer(),
                }
            };
            tsgQuestionData.Data.TsgQuestion = base.Get<TSG_QUESTION, TsgQuestion>((q) => q.ID == id).Data;
            tsgQuestionData.Data.TsgAnswer = base.Get<TSG_ANSWER, TsgAnswer>((q) => q.QUESTIONID == id).Data;
            return tsgQuestionData;


        }

        public BaseResponse<TsgQuestionData> SaveTsgQuestion(TsgQuestionData request)
        {
            if(request.TsgQuestion.Id==0)
            {
                request.TsgQuestion.Creater = SecurityHelper.CurrentPrincipal.EmpNo;
                request.TsgQuestion.CreateTime = DateTime.Now;
                request.TsgQuestion.Updater = SecurityHelper.CurrentPrincipal.EmpNo;
                request.TsgQuestion.UpdateTime = DateTime.Now;
            }
            else
            {
                request.TsgQuestion.Updater = SecurityHelper.CurrentPrincipal.EmpNo;
                request.TsgQuestion.UpdateTime = DateTime.Now;
            }

            if (request.TsgAnswer.Id == 0)
            {
                request.TsgAnswer.Creater = SecurityHelper.CurrentPrincipal.EmpNo;
                request.TsgAnswer.CreateTime = DateTime.Now;
                request.TsgAnswer.Updater = SecurityHelper.CurrentPrincipal.EmpNo;
                request.TsgAnswer.UpdateTime = DateTime.Now;
            }
            else
            {
                request.TsgAnswer.Updater = SecurityHelper.CurrentPrincipal.EmpNo;
                request.TsgAnswer.UpdateTime = DateTime.Now;
            }
            var response = new BaseResponse<TsgQuestionData>()
            {
                Data = new TsgQuestionData()
                {
                    TsgQuestion = new TsgQuestion(),
                    TsgAnswer = new TsgAnswer(),

                },
            };
            response.Data.TsgQuestion = base.Save<TSG_QUESTION, TsgQuestion>(request.TsgQuestion, (q) => q.ID == request.TsgQuestion.Id).Data;
            request.TsgAnswer.QuestionId = response.Data.TsgQuestion.Id;
            response.Data.TsgAnswer = base.Save<TSG_ANSWER, TsgAnswer>(request.TsgAnswer, (q) => q.ID == request.TsgAnswer.Id).Data;
            return response;
        }

        public BaseResponse DeleteTsgQuestion(int id)
        {

            unitOfWork.BeginTransaction();
            string strSql = String.Format("delete from TSG_ANSWER where QUESTIONID='{0}'", id);
            unitOfWork.GetRepository<TSG_ANSWER>().ExecuteSqlCommand(strSql);
            var result = base.Delete<TSG_QUESTION>(id);
            unitOfWork.Commit();
            return result;
        }
    }
}
