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

namespace KMHC.SLTC.WebAPI.SocialWorker {
	[RoutePrefix("api/nutrition")]
	public class NutritionEvalListController:BaseController {
		ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

		[Route(""), HttpGet]
		public IHttpActionResult Query(int feeNo, int currentPage, int pageSize) {
			BaseRequest<NutritionEvalFilter> request = new BaseRequest<NutritionEvalFilter>();

			request.CurrentPage = currentPage;

			request.PageSize = pageSize;

			request.Data.FeeNo = feeNo;

			var response = socialWorkerService.QueryNutritionEval(request);

			return Ok(response);
		}
		[Route(""), HttpGet]
		public IHttpActionResult Query(int feeNo, string code1, string code2, string code3, string s_date, string e_date) {

			var response = socialWorkerService.QueryBiochemistryByDate(feeNo, code1, code2, code3, Convert.ToDateTime(s_date), Convert.ToDateTime(e_date));

			return Ok(response);
		}
        [Route("")]
        public IHttpActionResult Delete(long id)
        {
            var response = socialWorkerService.DeleteNutritionEvalById(id);
			return Ok(response);
		}
		[Route("")]
		public IHttpActionResult Save(NutritionEvalModel request) {
			request.EVALDATE = DateTime.Now;
			var response = socialWorkerService.SaveNutritionEval(request);
			return Ok(response);

		}
	}

	
	 
}
