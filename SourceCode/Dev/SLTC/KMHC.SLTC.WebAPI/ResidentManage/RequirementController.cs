using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Base;

namespace KMHC.SLTC.WebAPI
{
     [RoutePrefix("api/requirement")]
    public class RequirementController : BaseController
    {
         readonly IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();


         [Route(""), HttpGet]
         public IHttpActionResult Get(int feeNo, int CurrentPage, int PageSize)
         {
             BaseRequest<DemandFilter> request = new BaseRequest<DemandFilter>();
             request.CurrentPage = CurrentPage;
             request.PageSize = PageSize;
             request.Data.FeeNO = feeNo;             
             var response = service.QueryDemand(request);
             return Ok(response);
         }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Demand baseRequest)
        {            
            var response = service.SaveDemand(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteDemand(id);
            return Ok(response);
        }
    }
}
