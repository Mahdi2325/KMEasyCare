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
     [RoutePrefix("api/Injection")]
    public class InjectionController : BaseController
    {
         INursingManageService injectionSvc = IOCContainer.Instance.Resolve<INursingManageService>();


         [Route(""), HttpGet]
         public IHttpActionResult Query(int currentPage, int pageSize, int regno)
        {

            NursingFilter request = new NursingFilter();

            request.RegNO = regno;
            request.CurrentPage = currentPage;

            request.PageSize = pageSize;
             

            try
            {
                var response = injectionSvc.QueryInjection(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }

        // GET api/syteminfo/5
         [Route("{ID}")]
         public IHttpActionResult Get(string ID)
         {
             try
             {
                 var response = injectionSvc.GetInjection(ID);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 LogHelper.WriteError(ex.ToString());
                 return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
             }
         }

         [Route(""), HttpGet]
         public IHttpActionResult Get([FromUri]int regNo)
         {
             try
             {
                 var response = injectionSvc.GetInjecttionByRegNo(regNo);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 LogHelper.WriteError(ex.ToString());
                 return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
             }
         }

        //// POST api/syteminfo
          [Route("")]
         public IHttpActionResult Post(InjectionView baseRequest)
         {
             try
             {
                 var response = injectionSvc.SaveInjection(baseRequest);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 LogHelper.WriteError(ex.ToString());
                 return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
             }
         }

        //// DELETE api/syteminfo/5
         [Route("{ID}")]
         public IHttpActionResult Delete(int ID)
         {
             try
             {
                 var response = injectionSvc.DeleteInjection(ID);
                 return Ok(response);
             }
             catch (Exception ex)
             {
                 LogHelper.WriteError(ex.ToString());
                 return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
             }
         }
    }
}
