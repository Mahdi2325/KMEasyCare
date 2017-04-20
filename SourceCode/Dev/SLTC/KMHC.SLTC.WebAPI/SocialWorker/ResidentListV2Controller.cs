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


namespace KMHC.SLTC.WebAPI.SocialWorker
{
    [RoutePrefix("api/residentV2")]
    public class ResidentListV2Controller:BaseController
    {
        ISocialWorkerManageService service = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
		public IHttpActionResult Query(string keyword, string ipdFlag, int currentPage, int pageSize,string FloorName,string RoomName)
        {
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
			request.Data.Name = keyword;
			request.Data.IdNo = keyword;
            request.Data.keyword = keyword;
            request.Data.IpdFlag = ipdFlag;
            request.Data.FloorName = FloorName;
            request.Data.RoomName = RoomName;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.QueryPersonsExtend(request);
          
          
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetResidentIpdById(id);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(RSDList baseRequest)
        {
            var response = service.SaveRSDList(baseRequest);
            return Ok(response);
        }
        [Route("GetNursingHomeAndLeaveRes"), HttpGet]
        public IHttpActionResult GetNursingHomeAndLeaveRes()
        {
            var response = service.GetNursingHomeAndLeaveRes();
            return Ok(response);
        }
    }
}
