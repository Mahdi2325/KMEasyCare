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

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/resourcelink")]
    public class ResourceLinkController : BaseController
    {
        //IResourceLinkService resourceService = IOCContainer.Instance.Resolve<IResourceLinkService>();
        ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();


        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int CurrentPage, int PageSize)
        {
            BaseRequest<ResourceLinkFilter> request = new BaseRequest<ResourceLinkFilter>();

            request.CurrentPage = CurrentPage;

            request.PageSize = PageSize;

            //request.Data.Name = keyWord;

            request.Data.FeeNo = feeNo;

            var response = service.QueryResourceLink(request);

            return Ok(response);
            //var response = service.QueryResourceLink(request);
            //return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetResourceLink(id);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Save(ResourceLinkModel baseRequest)
        {
            //baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateDate = DateTime.Now;
            //baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveResourceLink(baseRequest);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteResourceLink(id);
            return Ok(response);
        }
    }
}
