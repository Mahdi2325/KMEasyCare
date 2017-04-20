using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/post")]
    public class PostController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult get(string keyWord)
        {
            var filter = new ZipFileFilter
            {
                KeyWord=keyWord
            };
            var request = new BaseRequest<ZipFileFilter>
            {
                Data = filter
            };
            var response = service.QueryPost(request);
            return Ok(response);
        }
    }
}
