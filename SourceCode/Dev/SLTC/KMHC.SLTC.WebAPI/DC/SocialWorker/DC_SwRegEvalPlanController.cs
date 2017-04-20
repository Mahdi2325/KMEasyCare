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
    [RoutePrefix("api/SwRegEvalPlan")]
    public class DC_SwRegEvalPlanController : BaseController
    {
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();
        [Route(""), HttpGet]
        public IHttpActionResult QueryRegEvalHistory(int feeNo, int currentPage, int pageSize)
        {
            var response = service.QueryRegEvalHistory(feeNo,currentPage,pageSize);
			//request.CurrentPage = currentPage;

			//request.PageSize = pageSize;
            return Ok(response);
        }
      
        [Route(""),HttpGet]
        public IHttpActionResult Query(int feeNo,int evalPlanId)
        {
            BaseResponse<EvalPlan> response = service.QuerySwRegEvalPlan(evalPlanId, feeNo);
            return Ok(response);
        }
       
        /// <summary>
        /// 获取最新一笔量表评估值(ADL,IADL,MMSE...)
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult QueryEvalValue(int feeNo, int num, string type)
        {
            BaseRequest<DC_EvalQeustionFilter> request = new BaseRequest<DC_EvalQeustionFilter>();

             request.Data.ORGID = SecurityHelper.CurrentPrincipal.OrgId;

            request.Data.FEENO = feeNo;

            var response = service.GetEvalQuestionVal(feeNo,num,type);

            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult CheckAddRec(int feeNo,int number)
        {
            BaseRequest<DC_TaskGoalsStrategyFilter> request = new BaseRequest<DC_TaskGoalsStrategyFilter>();

            var response = service.CheckAddRec(feeNo,number);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(DC_SwRegEvalPlanModel baseRequest)
        {
            if (string.IsNullOrWhiteSpace(baseRequest.ORGID))
            {
                baseRequest.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
            }
           // baseRequest.EVALNUMBER = GetMaxNumber(baseRequest.FEENO) + 1;
            var response = service.SaveSwRegEvalPlan(baseRequest);
            return Ok(response);
        }
        public int? GetMaxNumber(long? feeNo)
        {
            //int GetMaxNumber(int feeNo,string orgId)
            int? result = service.GetMaxNumber(feeNo, SecurityHelper.CurrentPrincipal.OrgId);
            return result;
        }
        [Route("{id}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetSwRegEvalPlanById(feeNo);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteSwRegEvalPlanById(id);
            return Ok(response);
        }
    }
}
