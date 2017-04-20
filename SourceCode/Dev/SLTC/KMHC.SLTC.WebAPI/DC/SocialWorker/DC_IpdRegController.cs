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
    [RoutePrefix("api/IpdReg")]
    public class DC_IpdRegController : BaseController
    {
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();

        //GET api/Referral
        [Route(""), HttpGet]
        public IHttpActionResult Query(string feeNo, int currentPage, int pageSize)
        {
            BaseRequest<DC_IpdRegFilter> request = new BaseRequest<DC_IpdRegFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

            request.Data.FeeNo = Convert.ToInt32(feeNo);

            request.Data.CreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());

            var response = service.QueryIpdRegOut(request);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(DC_IpdRegModel baseRequest)
        {

            baseRequest.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            baseRequest.CreateDate = DateTime.Now;
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            switch (baseRequest.isAdd)
            {
                case "1":
                    baseRequest.IpdFlag = "I";
                    break;
                case "2":
                    baseRequest.IpdFlag = "O";
                    break;
                default:
                    baseRequest.IpdFlag = "I";
                    break;
            }

            if (baseRequest.isAdd == "1")
            {
                var response = service.SaveIpdRegIn(baseRequest);//收案
                return Ok(response);
            }
            else if (baseRequest.isAdd == "2")
            {
                var response = service.SaveIpdRegOut(baseRequest);//结案
                return Ok(response);
            }
            else if (baseRequest.isAdd == "3")
            {
                var response = service.SaveUpdateIpdRegIn(baseRequest);//修改收案资料
                return Ok(response);
            }
            return null;
        }
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var response = service.GetIpdRegOutById(int.Parse(id));
            return Ok(response);
        }
		[Route(""), HttpGet]
		public IHttpActionResult Get(string idNo, int type) {
			var response = service.GetIpdInfo(idNo);
			return Ok(response);
		}
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteIpdRegOutById(id);
            return Ok(response);
        }
    }
}
