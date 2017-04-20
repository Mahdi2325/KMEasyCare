using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using KM.Common;

namespace KMHC.SLTC.WebAPI.ResidentManage
{
    [RoutePrefix("api/leaveHosp")]
    public class LeaveHospController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int CurrentPage, int PageSize, string OrgId)
        {
            BaseRequest<LeaveHospFilter> request = new BaseRequest<LeaveHospFilter>
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = { FeeNo = feeNo, OrgId = OrgId }
            };
            var response = service.QueryLeaveHosp(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(DateTime sDate, DateTime eDate, string keyWord,int levStatus, int currentPage, int pageSize)
        {
            var response = service.QueryLeaveHospList(sDate, eDate, keyWord, levStatus, currentPage, pageSize);
            return Ok(response);
        }

        [Route("QueryRegInHosStatusList"), HttpGet]
        public IHttpActionResult QueryRegInHosStatusList()
        {
            var response = service.QueryRegInHosStatusList();
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int FeeNo, string OrgId)
        {
            BaseRequest<LeaveHospFilter> request = new BaseRequest<LeaveHospFilter>
            {
                Data = { FeeNo = FeeNo, OrgId = OrgId }
            };
            var response = service.GetNewLeaveHosp(request);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Get(int Id)
        {
            var response = service.GetLeaveHosp(Id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(LeaveHosp baseRequest)
        {
            var response = service.SaveLeaveHosp(baseRequest);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Delete(int Id)
        {
            var response = service.DeleteLeaveHosp(Id);
            return Ok(response);
        }

    }
}

