using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
namespace KMHC.SLTC.WebAPI.DC.SocialWorker
{
    [RoutePrefix("api/Taskgoalsstrategy")]
    public class DC_TaskGoalyssTrategyController:BaseController
    {
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();
        //GET api/SubsidyRec
        //[Route(""), HttpGet]
        //public IHttpActionResult Query(int evalPlanId, int currentPage, int pageSize)
        //{
        //    BaseRequest<DC_TaskGoalsStrategyFilter> request = new BaseRequest<DC_TaskGoalsStrategyFilter>();

        //    request.CurrentPage = currentPage;

        //    request.PageSize = pageSize;

        //    request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

        //    request.Data.EvalPlanId = evalPlanId;

        //    var response = service.get(request);

        //    return Ok(response);
        //}
        [Route("")]
        public IHttpActionResult Post(DC_TaskGoalsStrategyModel baseRequest)
        {
            //baseRequest. = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveTaskGoalssTrategy(baseRequest);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id,int feeNo)
        {
            if (id > 0 || id==-1)
            {
                var response = service.GetTaskGoalyssTrategyById(id, feeNo);
                return Ok(response);
            }
            return null;
        }
		[Route("{id}")]
		public IHttpActionResult Delete(int id) {
			var response = service.DeleteTaskGoalyssTrategyById(id);
			return Ok(response);
		}
    }
}
