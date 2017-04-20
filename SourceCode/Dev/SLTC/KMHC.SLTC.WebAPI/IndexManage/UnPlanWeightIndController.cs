using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    using KM.Common;
    using KMHC.SLTC.Business.Entity;
    using KMHC.SLTC.Business.Entity.Base;
    using KMHC.SLTC.Business.Entity.Filter;
    using KMHC.SLTC.Business.Entity.Model;
    using KMHC.SLTC.Business.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/UnPlanWeightInd")]
    public class UnPlanEdipdController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(long regNo = 0, long feeNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<UnPlanWeightIndFilter> request = new BaseRequest<UnPlanWeightIndFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.RegNo = regNo;
            request.Data.FeeNo = feeNo;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.QueryUnPlanWeightInd(request);
            return Ok(response);
        }
		[Route(""), HttpGet]
		public IHttpActionResult Query(string date1,string date2,long regNo = 0, long feeNo = 0, int currentPage = 1, int pageSize = 10) {
			BaseRequest<UnPlanWeightIndFilter> request = new BaseRequest<UnPlanWeightIndFilter>();
			request.CurrentPage = currentPage;
			request.PageSize = pageSize;
			request.Data.RegNo = regNo;
			request.Data.FeeNo = feeNo;
			request.Data.ThisRecDate1 = Convert.ToDateTime(date1);
			request.Data.ThisRecDate2 = Convert.ToDateTime(date2);
			request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
			var response = service.QueryUnPlanWeightInd(request);
			return Ok(response);
		}
        [Route("{id}")]
        public IHttpActionResult Get(long id)
        {
            var response = service.GetUnPlanWeightInd(id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(UnPlanWeightInd baseRequest)
        {
            var response = service.SaveUnPlanWeightInd(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteUnPlanWeightInd(id);
            return Ok(response);
        }

        [HttpGet, Route("MultiQuery")]
        public IHttpActionResult QueryUnPlanWeightList(int currentPage, int pageSize, string FloorName,string RoomName)
        {
            BaseRequest<UnPlanWeightIndFilter> request = new BaseRequest<UnPlanWeightIndFilter>();
            request.Data.FloorName = FloorName;
            request.Data.RoomName = RoomName;
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var response = service.QueryUnPlanWeightList(request);

            return Ok(response);
        }
        [Route("MultiSave"), HttpPost]
        public IHttpActionResult MultiSave(List<UnPlanWeightInd> request)
        {
            var response = service.SaveList(request);
            return Ok(response);
        }
    }
}
