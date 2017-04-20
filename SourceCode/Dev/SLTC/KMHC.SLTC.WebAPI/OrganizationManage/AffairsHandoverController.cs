using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.OrganizationManage
{
    [RoutePrefix("api/affairshandover")]
    public class AffairsHandoverController : BaseController
    {
        INursingWorkstationService AffairsHandoverService = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, string RecorderName)
        {
            BaseRequest<AffairsHandoverFilter> request = new BaseRequest<AffairsHandoverFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { RecorderName = RecorderName }
            };
            var response = AffairsHandoverService.QueryAffairsHandover(request);
            return Ok(response);
        }


        // GET api/syteminfo/5
        [Route("{Id}")]
        public IHttpActionResult Get(int Id)
        {
            var response = AffairsHandoverService.GetAffairsHandover(Id);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(AffairsHandover baseRequest)
        {
            var response = AffairsHandoverService.SaveAffairsHandover(baseRequest);
            return Ok(response);
        }
        [Route("multipSave")]
        public IHttpActionResult Post(List<AffairsHandover> baseRequest)
        {
            var response = AffairsHandoverService.SaveMulAffairsHandover(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{Id}")]
        public IHttpActionResult Delete(int Id)
        {
            var response = AffairsHandoverService.DeleteAffairsHandover(Id);
            return Ok(response);
        }
    }

}
