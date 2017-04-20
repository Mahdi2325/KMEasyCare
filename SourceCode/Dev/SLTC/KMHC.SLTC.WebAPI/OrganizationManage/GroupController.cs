/*
创建人:张凯
创建日期:2016-03-24
说明: 集团管理 
*/
using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/group")]
    public class GroupController : BaseController
    {
        IOrganizationManageService GroupService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage = 1, int pageSize = 10, string groupName = "")
        {
            BaseRequest<GroupFilter> request = new BaseRequest<GroupFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { GroupName = groupName }
            };
            var response = GroupService.QueryGroup(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{GroupId}")]
        public IHttpActionResult Get(string GroupId)
        {
            var response = GroupService.GetGroup(GroupId);
            return Ok(response);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Groups baseRequest)
        {
            //if (string.IsNullOrEmpty(baseRequest.GroupId))
            //{
            //    baseRequest.GroupId = System.Guid.NewGuid().ToString().Replace("-","").Substring(0,10);
            //}
            var response = GroupService.SaveGroup(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{GroupId}")]
        public IHttpActionResult Delete(string GroupId)
        {
            var response = GroupService.DeleteGroup(GroupId);
            return Ok(response);
        }
    }
}

