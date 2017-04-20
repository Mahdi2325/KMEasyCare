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
using System.Net.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/DeductionInfo")]
    public class DeductionController : BaseController
    {
        IDeductionService service = IOCContainer.Instance.Resolve<IDeductionService>();
        [Route("")]
        public IHttpActionResult Post(DeductionFilter request)
        {
            BaseRequest<DeductionFilter> baserequest = new BaseRequest<DeductionFilter>
            {
                CurrentPage = request.CurrentPage,
                PageSize = request.PageSize,
                Data = {
                     StartTime = request.StartTime,
                     EndTime = request.EndTime,
                     NsNo = request.NsNo
                }
            };
            var response = service.QueryDeductionList(baserequest);
            return Ok(response);
        }

        [Route("saveDeduction")]
        public IHttpActionResult SaveDeduction(NCIDeductionModel request)
        {
            var response = service.SaveDeduction(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public async Task<IHttpActionResult> Get(string Key, string Type, int CurrentPage, int PageSize)
        {
            if (Type == "1")
            {
                var request = new BaseRequest
                {
                    CurrentPage = CurrentPage,
                    PageSize = PageSize,
                };
                var response = service.GetDeductionList(request, Key);
                return Ok(response);
            }
            else
            {
                var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/Deduction?NSMonFeeID=" + Key + "&currentPage=" + CurrentPage + "&pageSize=" + PageSize);
                object resultContent = await result.Content.ReadAsAsync<object>();
                return Ok(resultContent);
            }
            
        }
    }
}
