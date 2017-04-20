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

    [RoutePrefix("api/NutritionCareRec")]
    public class NutritionCareRecController : BaseController
    {
        readonly INursingManageService service = IOCContainer.Instance.Resolve<INursingManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]int CurrentPage, int PageSize, long feeNo)
        {
            NutrtioncateRecFilter filter = new NutrtioncateRecFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<NutrtioncateRecFilter> request = new BaseRequest<NutrtioncateRecFilter>
            {
                Data = filter
            };
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            var response = service.QueryNutrtioncateRec(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(NutrtioncateRec request)
        {
            var response = service.SaveNutrtioncateRec(request);
            return Ok(response.Data);
        }

        [Route("")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteNutrtioncateRec(id);
            return Ok(response);
        }
    }
}
