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

namespace KMHC.SLTC.WebAPI.SocialWorker
{
    [RoutePrefix("api/NewResideEntenvRec")]
    public class NewResideEntenvRecController : BaseController
    {
        ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, string orgid = "", int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<NewResideEntenvRecFilter> request = new BaseRequest<NewResideEntenvRecFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data.FEENO = feeNo;

            if (!string.IsNullOrEmpty(orgid))
            { request.Data.ORGID = orgid; }
            else
            { request.Data.ORGID = SecurityHelper.CurrentPrincipal.OrgId; }
            

            var response = socialWorkerService.QueryNewResideEntenvRec(request);

            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = socialWorkerService.GetNewResideEntenvRecById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = socialWorkerService.DeleteNewResideEntenvRecById(id);
            return Ok(response);
        }


        [Route("")]
        public IHttpActionResult Save(NewResideEntenvRec request)
        { 
            //request.INDATE = DateTime.Now;
            request.ORGID = SecurityHelper.CurrentPrincipal.OrgId; 
            var response = socialWorkerService.SaveNewResideEntenvRec(request);
            return Ok(response);

        }
    }
}
