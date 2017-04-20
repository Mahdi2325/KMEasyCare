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
    [RoutePrefix("api/AssignTaskNew")]
    public class AssignTaskNewController : BaseController
    {
        INursingWorkstationService AssignTaskService = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        /// <summary>
        /// 查询工作提醒信息
        /// </summary>
        /// <param name="currentPage">页数</param>
        /// <param name="pageSize">页距</param>
        /// <param name="sDate">开始日期</param>
        /// <param name="eDate">结束日期</param>
        /// <param name="recStatus">是否已完成</param>
        /// <param name="newRecFlag">是否未读</param>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, DateTime? sDate, DateTime? eDate, bool? recStatus = null, bool? newRecFlag = null, string taskStatus = null)
        {
            BaseRequest<AssignTaskFilter> request = new BaseRequest<AssignTaskFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { 
                    SDate = sDate,
                    EDate = eDate,
                    RecStatus = recStatus,
                    NewRecFlag = newRecFlag,
                    TaskStatus = taskStatus,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                    Assignee = SecurityHelper.CurrentPrincipal.EmpNo
                }
            };
            var response = AssignTaskService.QueryAssignTask(request);
            return Ok(response);
        }


        [Route("")]
        public IHttpActionResult Post(AssignTask baseRequest)
        {
            var response = AssignTaskService.SaveAssignTask(baseRequest);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Get(int Id, bool recStatus, DateTime? finishDate, bool newrecFlag)
        {
            var response = AssignTaskService.ChangeRecStatus(Id, recStatus, finishDate, newrecFlag);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Get(int Id, bool newRecFlag)
        {
            var response = AssignTaskService.ChangeNewRecStatus(Id, newRecFlag);
            return Ok(response);
        }

        [Route("multisave")]
        public IHttpActionResult Post(TaskEmpFileList list)
        {
            var response = AssignTaskService.ReAssignTask(list.OldTask, list.TaskEmpFiles);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Delete(int Id)
        {
            var response = AssignTaskService.DeleteAssignTask(Id);
            return Ok(response);
        }


    }

}
