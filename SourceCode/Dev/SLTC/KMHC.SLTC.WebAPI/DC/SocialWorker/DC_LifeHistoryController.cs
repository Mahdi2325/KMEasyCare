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
        [RoutePrefix("api/LifeHistory")]
        public class DC_LifeHistoryController : BaseController
        {
            IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();

            //GET api/Referral
            [Route(""), HttpGet]
            public IHttpActionResult Query(string feeNo, int currentPage, int pageSize)
            {
                BaseRequest<DC_LifeHistoryFilter> request = new BaseRequest<DC_LifeHistoryFilter>();

                request.CurrentPage = currentPage;

                request.PageSize = pageSize;

                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

                request.Data.FeeNo = Convert.ToInt32(feeNo);

                //request.Data.CreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());

                var response = service.QueryLifeHistory(request);

                return Ok(response);
            }
            [Route("")]
            public IHttpActionResult Post(DC_LifeHistoryModel baseRequest)
            {
                baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                baseRequest.CreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                
                var response = service.SaveLifeHistory(baseRequest);
                return Ok(response);
            }
            [Route("{id}")]
            public IHttpActionResult Get(string id)
            {
                var response = service.GetLifeHistoryById(int.Parse(id));
                return Ok(response);
            }
            [Route("{id}")]
            public IHttpActionResult Delete(int id)
            {
                var response = service.DeleteLifeHistoryById(id);
                return Ok(response);
            }
        }
   
}
