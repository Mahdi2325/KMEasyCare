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

namespace KMHC.SLTC.WebAPI.EVM
{

    [RoutePrefix("api/evalTempSet")]
    public class EvalTemplateSetController : BaseController
    {
        IOrganizationManageService Service = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, string orgId)
        {
        
            BaseRequest<QuestionFilter> request = new BaseRequest<QuestionFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.OrgId = "tpl";
            request.Data.IsShow = true;
            BaseResponse<IList<LTC_Question>> response = Service.QueryEvalTempSetList(request);
            if (string.IsNullOrEmpty(orgId))
            {
                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            else
            {
                request.Data.OrgId = orgId;
            }
            request.Data.IsShow = true;
            BaseResponse<IList<LTC_Question>> responseSelf = Service.QueryEvalTempSetList(request);
            foreach (var item in response.Data)
            {
                item.Status = responseSelf.Data.Any(it => it.QuestionId == item.QuestionId);
            }

            BaseResponse<List<EvalTempSetModel>> newResponse = new BaseResponse<List<EvalTempSetModel>>();
            if (response.RecordsCount > 0)
            {
                newResponse.Data = new List<EvalTempSetModel>();
                var group = response.Data.ToLookup(it => it.CategoryId);
                foreach (var item in group)
                {
                    EvalTempSetModel newItem = new EvalTempSetModel();
                    newItem.CategoryId = item.Key;
                    newItem.Items = item.OrderBy(it => it.Id).ToList();
                    newResponse.Data.Add(newItem);
                }
            }
            return Ok(newResponse);
        }

        [Route(""), HttpPost]
        public IHttpActionResult post(EvalTempSetMainModel req)
        {
            List<EvalTempSetModel> request = req.Items; 
            Service.SaveEvalTemplateSet(req.OrgId, request);
            return Ok();
        }
    }
}
