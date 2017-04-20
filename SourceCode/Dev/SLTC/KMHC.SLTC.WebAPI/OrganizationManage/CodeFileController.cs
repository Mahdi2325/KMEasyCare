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
    [RoutePrefix("api/codefile")]
    public class CodeFileController : BaseController
    {
        readonly IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        // GET api/manufactures
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, string keyword)
        {
            BaseRequest<CommonFilter> request = new BaseRequest<CommonFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { Keywords = keyword }
            };
            var response = service.QueryCodeFile(request);
            //response.PagesCount = 2;
            return Ok(response);
        }

        // GET api/syteminfo/5
         [Route(""), HttpGet]
        public IHttpActionResult Get(string  code)
        {
            var response = service.GetCodeFile(code);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(CodeFile baseRequest)
        {
            var response = service.SaveCodeFile(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("")]
        public IHttpActionResult Delete(string ID)
        {
            var response = service.DeleteCodeFile(ID);
            return Ok(response);
        }
    }
}