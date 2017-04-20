/*********************管路指标conotrller****************************
 * 修改人：杨金高
 * 描述：修改为DB操作方式
 * 修改日期:2016-3-23
 ******************************************************************/


using KM.Common;
using KMHC.Infrastructure;
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
    [RoutePrefix("api/pipelinerec")]
    public class PipelineController : BaseController
    {
        ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo,bool removed, int currentPage, int pageSize)
        {
            BaseRequest<PipelineRecFilter> request = new BaseRequest<PipelineRecFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data.FeeNo = feeNo;
            request.Data.Removed = removed;
            var response = socialWorkerService.QueryPipelineRec(request);

            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = socialWorkerService.GetPipelineRecById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = socialWorkerService.DeletePipelineRecById(id);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Save(PipelineRecModel request)
        {
            request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;
            //request.RecordDate = DateTime.Now;
            request.Operator = SecurityHelper.CurrentPrincipal.EmpName;
            if(request.RemovedFlag==true)
                request.RemoveBy = SecurityHelper.CurrentPrincipal.EmpNo;
            var response = socialWorkerService.SavePipelineRec(request);
            return Ok(response);

        }
    }
}
