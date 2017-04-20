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
    [RoutePrefix("api/NursingRecord")]
    public class NursingRecordController : BaseController
    {
        INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(long? regNo, long feeNo, int CurrentPage, int PageSize)
        {
            BaseRequest<NursingRecFilter> request = new BaseRequest<NursingRecFilter>();
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            request.Data.RegNo = regNo;
            request.Data.FeeNo = feeNo;
            var response = service.QueryNursingRec(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(long regNo, long feeNo, bool PrintFlag)
        {
            BaseRequest<NursingRecFilter> request = new BaseRequest<NursingRecFilter>();
            request.Data.RegNo = regNo;
            request.Data.FeeNo = feeNo;
            request.Data.PrintFlag = true;
            request.CurrentPage = 1;
            request.PageSize = 1000;

            var response = service.QueryPrintInfo(request);
            return Ok(response);
        }


        [Route("{feeNo}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetNursingRec(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(NursingRec baseRequest)
        {
            var response = service.SaveNursingRec(baseRequest);
            return Ok(response);
        }
        [Route("batchSave")]
        public IHttpActionResult Post(NursingRecList baseRequest)
        {
            var response = service.SaveNursingRecList(baseRequest);
            return Ok(response);
        }
        [Route("{feeNo}")]
        public IHttpActionResult Delete(int feeNo)
        {
            var response = service.DeleteNursingRec(feeNo);
            return Ok(response);
        }
    }
}