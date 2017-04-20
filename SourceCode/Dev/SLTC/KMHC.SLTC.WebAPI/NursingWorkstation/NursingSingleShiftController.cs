using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/NursingSingleShift")]
    public class NursingSingleShiftController : BaseController
    {
        INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(long regNo, long feeNo, int CurrentPage, int PageSize)
        {
            BaseRequest<NursingHandoverFilter> request = new BaseRequest<NursingHandoverFilter>();
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            request.Data.RegNo = regNo;
            request.Data.FeeNo = feeNo;
            var response = service.QueryNursingHandover(request);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetNursingHandover(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(NursingHandover baseRequest)
        {
            var response = service.SaveNursingHandover(baseRequest);
            return Ok(response);
        }
        [Route("multipSave")]
        public IHttpActionResult Post(List<NursingHandover> baseRequest)
        {
            var response = service.SaveMulNursingHandover(baseRequest);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Delete(int feeNo)
        {
            var response = service.DeleteNursingHandover(feeNo);
            return Ok(response);
        }
    }
}