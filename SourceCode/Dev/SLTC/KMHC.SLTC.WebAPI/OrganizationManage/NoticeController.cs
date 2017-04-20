using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
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
using KMHC.SLTC.Business.Entity.DC.Model;

namespace KMHC.SLTC.WebAPI.NursingWorkstation
{
    [RoutePrefix("api/Notice")]
    public class NoticeController : BaseController
    {
        readonly IOrganizationManageService service = IOCContainer.Instance.Resolve<IOrganizationManageService>();


        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize,DateTime? sDate,DateTime? eDate)
        {
            var request = new BaseRequest<NoticeFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data =
                {
                    SDate = sDate,
                    EDate = eDate, 
                    OrgId= SecurityHelper.CurrentPrincipal.OrgId
                }
            };
            var response = service.QueryNotices(request);
            return Ok(response);
        }
        

        [Route("")]
        public IHttpActionResult Get(int id)
        {
            var response = service.GetNotice(id);
            return Ok(response);
        }


        [Route("")]
        public IHttpActionResult Post(Notice baseRequest)
        {
            if (baseRequest != null && baseRequest.Id == 0)
            {
                baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                baseRequest.CreateDate=DateTime.Now;
            }
            baseRequest.CreateDate = DateTime.Now;
            var response = service.SaveNotice(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteNotice(id);
            return Ok(response);
        }
    }

}
