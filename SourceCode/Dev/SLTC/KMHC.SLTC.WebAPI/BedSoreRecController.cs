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
    [RoutePrefix("api/prssores")]
    public class BedSoreRecController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(long feeNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<BedSoreRecFilter> request = new BaseRequest<BedSoreRecFilter>()
            {
                Data = new BedSoreRecFilter()
                {
                    FeeNo = feeNo,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                },

                CurrentPage = currentPage,
                PageSize = pageSize,

            };
            var response = service.QueryBedSoreRecExtend(request);
            return Ok(response);
        }

        // GET
        [Route("{FeeNo}"), HttpGet]
        public IHttpActionResult Get(long feeNo)
        {
            BaseRequest<BedSoreRecFilter> request = new BaseRequest<BedSoreRecFilter>()
            {
                Data = new BedSoreRecFilter()
                {
                    FeeNo = feeNo,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                }
            };
            var response = service.GetBedSoreRecExtend(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(BedSoreRec request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;
            var response= service.SaveBedSoreRec(request);
            return Ok(response);
        }


        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteBedSoreRec(id);
            return Ok(response);
        }
    }
}
