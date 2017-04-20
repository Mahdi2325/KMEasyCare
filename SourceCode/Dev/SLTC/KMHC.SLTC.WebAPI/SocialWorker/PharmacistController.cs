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
    [RoutePrefix("api/pharmacistevals")]
    public class PharmacistController : BaseController
    {          
        ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int CurrentPage, int PageSize)
        {
            BaseRequest<PharmacistFilter> request = new BaseRequest<PharmacistFilter>();

            request.CurrentPage = CurrentPage;

            request.PageSize = PageSize;

            request.Data.FeeNo = feeNo;

            var response = socialWorkerService.QueryPharmacist(request);

            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = socialWorkerService.DeletePharmacistById(id);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Save(Pharmacist request)
        {
            //request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.CreateBy = "01"; //SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;
  
            var response = socialWorkerService.SavePharmacist(request);
            return Ok(response);

        }
    }
}
