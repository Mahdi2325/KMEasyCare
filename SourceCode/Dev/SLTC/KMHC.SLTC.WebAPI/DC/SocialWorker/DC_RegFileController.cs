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
    [RoutePrefix("api/PersonBasic")]
    public class DC_RegFileController:BaseController
    {
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();
        //GET api/SubsidyRec
        [Route(""), HttpGet]
        public IHttpActionResult Query(string orgId,int feeNo,string idNo,string name, int currentPage, int pageSize)
        {
            //var response = socialWorkerService.QuerySubsidy(request);
            //return Ok(response);
            BaseRequest<DC_RegFileFilter> request = new BaseRequest<DC_RegFileFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;
            if (string.IsNullOrEmpty(orgId))
                orgId = SecurityHelper.CurrentPrincipal.OrgId;
            else
                request.Data.OrgId = orgId;
            if (!string.IsNullOrEmpty(idNo))
                request.Data.IdNo = idNo;
            if (!string.IsNullOrEmpty(name))
                request.Data.Name = name;
           // request.Data.RegNo = regNo;
            request.Data.FeeNo = feeNo;
            var response = service.QueryPersonBasic(request);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(DC_RegFileModel baseRequest)
        {
            baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateDate = DateTime.Now;
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;


            var response = service.SavePersonBasic(baseRequest);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var response = service.GetPersonBasicById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            var response = service.DeletePersonBasicById(id);
            return Ok(response);
        }
    }
}
