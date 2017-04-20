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
    [RoutePrefix("api/regevaluate")]
    public class RegEvaluateController : BaseController
    {
        ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo,int  currentPage,int pageSize )
        {
            BaseRequest<RegEvaluateFilter> request = new BaseRequest<RegEvaluateFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data.FeeNo = feeNo;

            var response = socialWorkerService.QueryRegEvaluate(request);

            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = socialWorkerService.DeleteRegEvaluateById(id);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Save(RegEvaluateModel request)
        {
            //request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            //request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;
            //request.Operator = SecurityHelper.CurrentPrincipal.EmpNo;

            //request.Operator = SecurityHelper.CurrentPrincipal.EmpNo;
            var response = socialWorkerService.SaveRegEvaluate(request);
            return Ok(response);

        }
    }
}
