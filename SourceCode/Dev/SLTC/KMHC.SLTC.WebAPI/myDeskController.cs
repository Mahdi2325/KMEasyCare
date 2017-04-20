using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI {

	[RoutePrefix("api/myDesk")]
	public class myDeskController : BaseController {

		IMyDeskService service = IOCContainer.Instance.Resolve<IMyDeskService>();
        ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
		[Route(""), HttpGet]
		public IHttpActionResult Query(int flag = 0, long UserNo = 0, int currentPage = 1, int pageSize = 10) {

			if (flag == 0) {//院民统计列表

			}
			else if (flag == 1)//本年度入案人数
			{ }
			else if (flag == 2)//本年度结案人数
			{ }
			else if (flag == 3)//床位占用数
			{ }
			else if (flag == 4) { }//压疮
			else {

			}//院内公告
			return Ok();

			/*BaseRequest<FallIncidentEventFilter> request = new BaseRequest<FallIncidentEventFilter>();
			request.CurrentPage = currentPage;
			request.PageSize = pageSize;
			request.Data.FeeNo = UserNo;
			var response = service.QueryFallList(request);

			var count = response.RecordsCount;

			return Ok(response);*/
		}
        [Route(""), HttpGet]
        public object QueryAssignTask(DateTime start, DateTime end)
        {

            BaseRequest<AssignTaskFilterByBobDu> request = new BaseRequest<AssignTaskFilterByBobDu>
            {
               
                Data = { start = start, end = end }
            };
            return service.QueryAssTask(request);
           
        }
        [Route("DashboardDataIn")]
        [HttpGet]
        public object QueryDashboardDataIn()
        {

            DataTable dt = socialWorkerManageService.QueryInData(SecurityHelper.CurrentPrincipal.OrgId);
            return DataTableToJson(dt);
           
        }
        [Route("DashboardDataOut")]
        [HttpGet]
        public object DashboardDataOut()
        {

            DataTable dt = socialWorkerManageService.QueryOutData(SecurityHelper.CurrentPrincipal.OrgId);
            return DataTableToJson(dt);

        }
        [Route("DashboardDataBed")]
        [HttpGet]
        public object DashboardDataBed()
        {
            return socialWorkerManageService.QueryBedData2(SecurityHelper.CurrentPrincipal.OrgId); 
        }
        [Route("DashboardDataBedSore")]
        [HttpGet]
        public object DashboardDataBedSore()
        {

            DataTable dt = socialWorkerManageService.QueryBedSoreData(SecurityHelper.CurrentPrincipal.OrgId);
            return DataTableToJson(dt);

        }
        [Route("QueryKPI")]
        [HttpGet]
        public object QueryKPI()
        {

            return  service.QueryKPI();
        
        }
        private string DataTableToJson(DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append(dt.Rows[i][j].ToString());
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("]");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append(0);
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("]");
                    if (i < (3 - 1))
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }
	}

}

