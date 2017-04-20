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
     [RoutePrefix("api/OutValue")]
    public class OutValueController : BaseController
    {
         INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

         [Route(""), HttpGet]
         public IHttpActionResult Query(long feeNo = 0, int currentPage = 1, int pageSize = 10)
         {
             BaseRequest<OutValueFilter> request = new BaseRequest<OutValueFilter>();
             request.CurrentPage = currentPage;
             request.PageSize = pageSize;
             request.Data.FeeNo = feeNo;
             var response = service.QueryOutValue(request);
             return Ok(response);
         }

         [Route("{inNo}")]
         public IHttpActionResult Get(long inNo)
         {
             var response = service.GetOutValue(inNo);
             return Ok(response);
         }
         [Route("")]
         public IHttpActionResult GetToNurse(long FEENO,string CLASSTYPE,DateTime RECDATE)
         {
             var response = service.GetOutValueToNurse(FEENO, CLASSTYPE, RECDATE);
             return Ok(response);
         }

         [Route("")]
         public IHttpActionResult Post(List<OutValueModel> baseRequest)
         {
             var response = service.SaveOutValue(baseRequest);
             return Ok(response);
         }

         [Route("{inNo}")]
         public IHttpActionResult Delete(long inNo)
         {
             var response = service.DeleteOutValue(inNo);
             return Ok(response);
         }
    }
}
