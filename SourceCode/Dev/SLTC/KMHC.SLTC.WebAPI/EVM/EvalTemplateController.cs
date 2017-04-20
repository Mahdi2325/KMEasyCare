using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.OrganizationManage
{

    [RoutePrefix("api/evalTemplate")]
    public class EvalTemplateController : BaseController
    {
        IOrganizationManageService Service = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        //QueryOueList

        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord, string orgId, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<QuestionFilter> request = new BaseRequest<QuestionFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,

                Data =
                {
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                    Questionname = keyWord,
                    QuestionDesc = keyWord,
                }
            };

            if (!string.IsNullOrEmpty(orgId))
            { request.Data.OrgId = orgId; }


            var response = Service.QueryQueList(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult get(int id)
        {
            var response = Service.GetQue(id);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Query(int questionId)
        {
            BaseRequest<MakerItemFilter> request = new BaseRequest<MakerItemFilter>()
            {
                PageSize = 0,
                Data =
                {
                    QuestionId = questionId,
                }
            };
            var response = Service.QueryMakerItemList(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Query(int questionId, string qrMark)
        {
            BaseRequest<QuestionResultsFilter> request = new BaseRequest<QuestionResultsFilter>()
            {
                PageSize = 0,
                Data =
                {
                    QuestionId = questionId,
                }
            };
            var response = Service.QueryQuestionResultsList(request);
            return Ok(response);
        }


        [Route("")]
        public IHttpActionResult Post(LTC_Question request)
        {
            if (string.IsNullOrEmpty(request.OrgId))
            { request.OrgId = SecurityHelper.CurrentPrincipal.OrgId; }
            var response = Service.SaveQuestion(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Delete(int id)
        {
            var response = Service.DeleteQuestion(id);
            return Ok(response);
        }
    }
}
