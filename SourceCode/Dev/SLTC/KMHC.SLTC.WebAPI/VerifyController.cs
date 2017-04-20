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
    [RoutePrefix("api/Verify")]
    public class VerifyController : BaseController
    {
        IResidentManageService VerifyService = IOCContainer.Instance.Resolve<IResidentManageService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, long? feeNo, string orgId)
        {
            BaseRequest<VerifyFilter> request = new BaseRequest<VerifyFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data =
                {
                    FeeNo = feeNo,
                    OrgId = orgId
                }
            };
            var response = VerifyService.QueryVerify(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{FeeNo}")]
        public IHttpActionResult Get(long FeeNo)
        {
            var response = VerifyService.GetVerify(FeeNo);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Verify baseRequest)
        {
            var response = VerifyService.SaveVerify(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{FeeNo}")]
        public IHttpActionResult Delete(long FeeNo)
        {
            var response = VerifyService.DeleteVerify(FeeNo);
            return Ok(response);
        }



    }
}
