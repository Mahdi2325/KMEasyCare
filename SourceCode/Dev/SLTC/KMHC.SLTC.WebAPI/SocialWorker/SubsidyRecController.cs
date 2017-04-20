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
    [RoutePrefix("api/subsidyrec")]
    public class SubsidyRecController : BaseController
    {
        ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        //GET api/SubsidyRec
        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int currentPage, int pageSize)
        {
            //var response = socialWorkerService.QuerySubsidy(request);
            //return Ok(response);
            BaseRequest<SubsidyFilter> request = new BaseRequest<SubsidyFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            //request.Data.Name = keyWord;

            request.Data.FeeNo = feeNo;

            var response = service.QuerySubsidy(request);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(SubsidyView baseRequest)
        {
            baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateDate = DateTime.Now;
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
 

            var response = service.SaveSubsidy(baseRequest);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetSubsidyById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteSubsidyById(id);
            return Ok(response);
        }
    }


}
