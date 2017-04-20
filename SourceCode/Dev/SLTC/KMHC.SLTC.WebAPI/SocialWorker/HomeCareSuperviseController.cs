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
    [RoutePrefix("api/homecaresupervise")]
    public class HomeCareSuperviseController:BaseController
    {
        ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int currentPage, int pageSize)
        {
            BaseRequest<HomeCareSuperviseFilter> request = new BaseRequest<HomeCareSuperviseFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data.FeeNo = feeNo;

            var response = socialWorkerService.QueryHomeCareSupervise(request);

            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = socialWorkerService.GetHomeCareSuperviseById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = socialWorkerService.DeleteHomeCareSuperviseById(id);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Save(HomeCareSuperviseModel request)
        {
            request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

            var response = socialWorkerService.SaveHomeCareSupervise(request);
            return Ok(response);
        }
    }
}
