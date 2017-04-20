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
    [RoutePrefix("api/PersonDayLife")]
    public class DC_RegDayLifeController:BaseController
    {
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(string feeNo, int currentPage, int pageSize)
        {
            BaseRequest<DC_RegDayLifeFilter> request = new BaseRequest<DC_RegDayLifeFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

            request.Data.FeeNo = Convert.ToInt32(feeNo);

            var response = service.QueryDayLife(request);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(DC_RegDayLifeModel baseRequest)
        {
            baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveDayLife(baseRequest);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetDayLifeById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteDayLifeById(id);
            return Ok(response);
        }
    }
}
