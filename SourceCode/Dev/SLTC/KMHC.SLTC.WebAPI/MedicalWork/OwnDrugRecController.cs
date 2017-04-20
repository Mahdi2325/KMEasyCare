using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model.MedicalWork;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.MedicalWork
{
    [RoutePrefix("api/ownDrugRec")]
    public class OwnDrugRecController : BaseController
    {
        IOwnDrugRecService service = IOCContainer.Instance.Resolve<IOwnDrugRecService>();

        [Route(""), HttpGet]
        public IHttpActionResult QueryOwnDrugDtl(int CurrentPage, int PageSize, int FeeNo, int OwnDrugId)
        {
            BaseRequest<OwnDrugRecFilter> request = new BaseRequest<OwnDrugRecFilter>
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = { FeeNo = FeeNo, OwnDrugId = OwnDrugId }
            };
            var response = service.QueryOwnDrugDtl(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult QueryOwnDrugRec(int CurrentPage, int PageSize, int FeeNo)
        {
            BaseRequest<OwnDrugRecFilter> request = new BaseRequest<OwnDrugRecFilter>
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = { FeeNo = FeeNo }
            };
            var response = service.QueryOwnDrugRec(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Save(OwnDrugRecModel baseRequest)
        {
            var response = service.SaveOwnDrugRec(baseRequest);
            return Ok(response);
        }
        [Route("saveOwnDrugDtl")]
        public IHttpActionResult SaveOwnDrugDtl(OwnDrugDtlList baseRequest)
        {
            var response = service.SaveOwnDrugDtl(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.Delete(id);
            return Ok(response);
        }
    }
}
