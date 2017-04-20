using KM.Common;
using KMHC.SLTC.Business.Entity;
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
     [RoutePrefix("api/Vitalsign")]
    public class VitalsignController : BaseController
    {
         INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

         [Route(""), HttpGet]
         public IHttpActionResult Query(long feeNo = 0, int currentPage = 1, int pageSize = 10)
         {
             BaseRequest<MeasureFilter> request = new BaseRequest<MeasureFilter>();
             request.CurrentPage = currentPage;
             request.PageSize = pageSize;
             request.Data.FeeNo = feeNo;
             var response = service.QueryVitalsign(request);
             return Ok(response);
         }

         [Route("{feeNo}")]
         public IHttpActionResult Get(int feeNo)
         {
             var response = service.GetVitalsign(feeNo);
             return Ok(response);
         }

         [Route("")]
         public IHttpActionResult Get(string itemCode)
         {
             var response = service.GetMeasureItem(itemCode);
             return Ok(response);
         }
         [Route("")]
         public IHttpActionResult GetToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE)
         {
             var response = service.GetVitalsignToNurse(FEENO, CLASSTYPE, RECDATE);
             return Ok(response);
         }
         [Route("")]
         public IHttpActionResult Post(List<Vitalsign> baseRequest)
         {
             var response = service.SaveVitalsign(baseRequest);
             return Ok(response);
         }

         [Route("{seqNo}")]
         public IHttpActionResult Delete(long seqNo)
         {
             var response = service.DeleteVitalsign(seqNo);
             return Ok(response);
         }
    }
}
