using KM.Common;
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

namespace KMHC.SLTC.WebAPI.ResidentManage
{
    [RoutePrefix("api/HealthRecords")]
    public class HealthRecordsController : BaseController
    {

         IHealthRecordsService service = IOCContainer.Instance.Resolve<IHealthRecordsService>();

         [Route(""), HttpGet]
         public IHttpActionResult Get(int feeNo, int type)
         {
             var response = new HealthRecords();
             if (type == 1)
             {
                 response = service.GetMeasurementInfo(feeNo);
             }
             if (type == 2)
             {
                 response = service.GetMedicationInfo(feeNo);
             }
             if (type == 3)
             {
                 response = service.GetBiochemistryInfo(feeNo);
             }
             if (type == 4)
             {
                 response = service.GetEvaluationInfo(feeNo);
             }
             if (type == 5)
             {
                 response = service.GetDrugInfo(feeNo);
             }
             return Ok(response);
         }


         [Route(""), HttpGet]
         public IHttpActionResult Get(int feeNo, string genre, int CurrentPage, int PageSize)
         {
             BaseRequest<HeathRecordFilter> request = new BaseRequest<HeathRecordFilter>
             {
                 CurrentPage = CurrentPage,
                 PageSize = PageSize,
                 Data = { FeeNo = feeNo, ItemCode = genre }
             };
             var response = service.QueryHealthRecordList(request);
             return Ok(response);
         }

         [Route(""), HttpGet]
         public IHttpActionResult get([FromUri]int CurrentPage, int PageSize, long feeNo, DateTime? recStartDate, DateTime? recEndDate)
         {
             DrugRecordinfoFilter filter = new DrugRecordinfoFilter
             {
                 FeeNo = feeNo,
                 StartDate = recStartDate,
                 EndDate = recEndDate
             };
             BaseRequest<DrugRecordinfoFilter> request = new BaseRequest<DrugRecordinfoFilter>
             {
                 Data = filter,
                 CurrentPage = CurrentPage,
                 PageSize = PageSize
             };
             var response = service.QuerydrugRecord(request);
             return Ok(response);
         }

         [Route("")]
         public IHttpActionResult Post(Measurement baseRequest)
         {
             var response = service.SaveMeaSuredRecord(baseRequest);
             return Ok(response);
         }

    }
}
