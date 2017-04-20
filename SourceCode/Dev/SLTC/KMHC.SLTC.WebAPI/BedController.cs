using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;


namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/beds")]
    public class BedController : BaseController
    {
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        // GET 
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage = 1, int pageSize = 100, string keyWords = "", string bedStatus = "")
        {
            var request = new BaseRequest<BedBasicFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = new BedBasicFilter()
                {
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                    KeyWords = keyWords,
                    BedStatus = bedStatus
                }
            };
            var response = organizationManageService.QueryBedBasicExtend(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{bedNO}")]
        public IHttpActionResult Get(string bedNO)
        {
            var response = organizationManageService.GetBedBasic(bedNO);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post([FromBody]BedBasic baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = organizationManageService.SaveBedBasic(baseRequest);
            return Ok(response);
        }

        [Route("changeBed")]
        public IHttpActionResult changeBed(ChangeBedModel request)
        {
            var response = organizationManageService.ChangeBed(request);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            var response = organizationManageService.DeleteBedBasic(id);
            return Ok(response);
        }
    }
}