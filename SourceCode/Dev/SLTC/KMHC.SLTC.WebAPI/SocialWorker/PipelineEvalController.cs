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
    [RoutePrefix("api/pipelineeval")]
    public class PipelineEvalController:BaseController
    {
        ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int seqNo)
        {
            BaseRequest<PipelineEvalFilter> request = new BaseRequest<PipelineEvalFilter>();

            request.Data.SeqNo = seqNo;

            var response = socialWorkerService.QueryPipelineEval(request);

            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int seqNo, int CurrentPage, int PageSize)
        {
            BaseRequest<PipelineEvalFilter> request = new BaseRequest<PipelineEvalFilter>();

            request.Data.SeqNo = seqNo;
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;

            var response = socialWorkerService.QueryPipelineEval(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = socialWorkerService.DeletePipelineEvalById(id);
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Save(PipelineEvalModel request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;
           // request.Operator = SecurityHelper.CurrentPrincipal.EmpName;
            
            var response = socialWorkerService.SavePipelineEval(request);

            //PipelineRecModel prm=new PipelineRecModel();
            //prm.PipelineName = request.PipelineName;
            //prm.FeeNo = request.FeeNo;
            //prm.SeqNo=request.SeqNo;
            //var re = socialWorkerService.SavePipelineRec(prm);
            return Ok(response);

        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int feeNo, DateTime recDate, string pipeLineName)
        {
            var response = socialWorkerService.GetPipelineEvalToNurse(feeNo, recDate, pipeLineName);
            return Ok(response);
        }
    }
}
