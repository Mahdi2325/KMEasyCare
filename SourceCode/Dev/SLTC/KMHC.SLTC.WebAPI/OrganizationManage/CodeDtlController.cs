using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;


namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/codedtl")]
    public class CodeDtlController : BaseController
    {
        readonly IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();




        // GET api/manufactures
        [Route(""), HttpGet]
        public IHttpActionResult Get(string type)
        {
            BaseRequest<CommonFilter> request = new BaseRequest<CommonFilter>
            {
                PageSize = 0,
                Data = { Keywords = type }
            };
            var response = service.QueryCodeDtl(request);
            return Ok(response);
        }

        //[Route(""), HttpGet]
        //public IHttpActionResult Get(string type,string id)
        //{
        //    var response = service.GetCodeFile(id);
        //    return Ok(response.Data);
        //}

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(CodeDtl baseRequest)
        {
            var response = service.SaveCodeDtl(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("")]
        public IHttpActionResult Delete(string type, string code)
        {
            var response = service.DeleteCodeDtl(code, type);
            return Ok(response);
        }
    }
}