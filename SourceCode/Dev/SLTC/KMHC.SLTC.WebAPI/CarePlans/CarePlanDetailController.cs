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
    [RoutePrefix("api/carePlanDetail")]
    public class CarePlanDetailController : BaseController
    {
        ICarePlansManageService carePlansSvc = IOCContainer.Instance.Resolve<ICarePlansManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(long feeNo, int currentPage, int pageSize)
        {
            try
            {

                BaseRequest<CarePlanDetailFilter> request = new BaseRequest<CarePlanDetailFilter>();
                request.CurrentPage = currentPage;
                request.PageSize = pageSize;
                request.Data.FeeNo = feeNo;


                var response = carePlansSvc.QueryCarePlanDetail(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
                return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
            }
        }
        [Route(""), HttpGet]
        public object QueryAssignTask(DateTime start, DateTime end, long feeno)
        {

            BaseRequest<AssignTaskFilterByBobDu> request = new BaseRequest<AssignTaskFilterByBobDu>
            {

                Data = { start = start, end = end, feeno = feeno }
            };
            return carePlansSvc.QueryAssTask(request);

        }

        //[Route("")]
        //public IHttpActionResult Post(NSCPL baseRequest)
        //{
        //    try
        //    {
        //        var response = carePlansSvc.SaveActivity(baseRequest);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(ex.ToString());
        //        return Ok(new BaseResponse<string> { ResultCode = (int)EnumResponseStatus.ExceptionHappened, ResultMessage = "操作异常" });
        //    }
        //}

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = carePlansSvc.DeleteCarePlan(id);
            return Ok(response);
        }
    }
}
