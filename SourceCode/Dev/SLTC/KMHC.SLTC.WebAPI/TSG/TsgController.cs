using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.TSG;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.TSG
{

    [RoutePrefix("api/category")]
    public class TsgController : BaseController
    {
        ITsgService service = IOCContainer.Instance.Resolve<ITsgService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query()
        {
            BaseRequest<TsgCategoryFilter> request = new BaseRequest<TsgCategoryFilter>();
            var response = service.QueryTsgData(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(string type)
        {
            BaseRequest<TsgCategoryFilter> request = new BaseRequest<TsgCategoryFilter>();
            var response = service.QueryTsgCategory(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyword, int CurrentPage, int PageSize)
        {
            BaseRequest<TsgQuestionFilter> request = new BaseRequest<TsgQuestionFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new TsgQuestionFilter
                {
                    KeyWord = keyword,
                },
            };
            var response = service.QueryTsgQuestion(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetTsgQuestion(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(TsgQuestionData request)
        {
            var response = service.SaveTsgQuestion(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteTsgQuestion(id);
            return Ok(response);
        }
    }
}
