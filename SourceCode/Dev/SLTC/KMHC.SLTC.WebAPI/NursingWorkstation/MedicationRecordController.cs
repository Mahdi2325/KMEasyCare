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

namespace KMHC.SLTC.WebAPI.NursingWorkstation
{

    [RoutePrefix("api/medicationRecord")]
    public class MedicationRecordController : BaseController
    {
        INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();
        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]int CurrentPage, int PageSize, long feeNo, DateTime? StartDate, DateTime? EndDate)
        {
            VisitPrescriptionFilter filter = new VisitPrescriptionFilter
            {
                FeeNo = feeNo,
                StartDate = StartDate,
                EndDate = EndDate
                //StartDate = DateTime.Parse(StartDate),
                //EndDate = DateTime.Parse(EndDate)
            };
            BaseRequest<VisitPrescriptionFilter> request = new BaseRequest<VisitPrescriptionFilter>
            {
                Data = filter,
                CurrentPage = CurrentPage,
                PageSize = PageSize
            };
            var response = service.QueryVisitPrescription(request);
            return Ok(response);
        }
        //[Route(""), HttpGet]
        //public IHttpActionResult Get([FromUri]int CurrentPage, int PageSize, long feeNo)
        //{
        //    VisitPrescriptionFilter filter = new VisitPrescriptionFilter
        //    {
        //        FeeNo = feeNo
        //        //,
        //        //StartDate = StartDate,
        //        //EndDate = EndDate
        //        //StartDate = DateTime.Parse(StartDate),
        //        //EndDate = DateTime.Parse(EndDate)
        //    };
        //    BaseRequest<VisitPrescriptionFilter> request = new BaseRequest<VisitPrescriptionFilter>
        //    {
        //        Data = filter,
        //        CurrentPage = CurrentPage,
        //        PageSize = PageSize
        //    };
        //    var response = service.QueryVisitPrescription(request);
        //    return Ok(response);
        //}
    }
}
