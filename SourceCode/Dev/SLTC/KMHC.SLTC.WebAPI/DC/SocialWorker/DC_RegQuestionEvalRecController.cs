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
     [RoutePrefix("api/RegQuestionEvalRec")]
    public class DC_RegQuestionEvalRecController:BaseController
    {
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();
        //GET api/SubsidyRec
         /// <summary>
         /// 查询所有问题列表
         /// </summary>
         /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult QueryQuestion(int feeNo)
        {
            BaseResponse<IList<DC_QuestionModel>> response = new BaseResponse<IList<DC_QuestionModel>>();
            response = service.QueryQuestion(feeNo, SecurityHelper.CurrentPrincipal.OrgId);

            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult QueryQuestion(int feeNo,int type)
        {
            BaseResponse<IList<DC_RegQuestionEvalRecModel>> response = new BaseResponse<IList<DC_RegQuestionEvalRecModel>>();
            response = service.QueryRegQuestionHistory(feeNo);

            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(List<DC_QuestionModel> baseRequest)
        {
            var response = service.SaveRegQuestionItem(baseRequest);
            //baseRequest.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
            //var response = service.SaveRegQuestionItem(baseRequest);
            return Ok(response);
            
        }
        [Route("{id}")]
        public IHttpActionResult Get(int questionId)
        {
            var response = service.GetQuestionById(questionId);
            return Ok(response);
        }
        //[Route("{id}")]
        //public IHttpActionResult Delete(int id)
        //{
        //    var response = service.DeleteSwRegEvalPlanById(id);
        //    return Ok(response);
        //}
    }
}
