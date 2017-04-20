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
     [RoutePrefix("api/InValue")]
    public class InValueController : BaseController
    {
         INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

         [Route(""), HttpGet]
         public IHttpActionResult Query(long feeNo = 0, int currentPage = 1, int pageSize = 10)
         {
             BaseRequest<InValueFilter> request = new BaseRequest<InValueFilter>();
             request.CurrentPage = currentPage;
             request.PageSize = pageSize;
             request.Data.FeeNo = feeNo;
             var response = service.QueryInValue(request);
             return Ok(response);
         }

         [Route("{inNo}")]
         public IHttpActionResult Get(long inNo)
         {
             var response = service.GetInValue(inNo);
             return Ok(response);
         }
         [Route("")]
         public IHttpActionResult GetToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE)
         {
             var response = service.GetInValueToNurse(FEENO, CLASSTYPE, RECDATE);
             return Ok(response);
         }
         [Route("")]
         public IHttpActionResult Post(List<InValueModel> baseRequest)
         {
             var response = service.SaveInValue(baseRequest);
             return Ok(response);
         }

         [Route("{inNo}")]
         public IHttpActionResult Delete(long inNo)
         {
             var response = service.DeleteInValue(inNo);
             return Ok(response);
         }
    }
}
