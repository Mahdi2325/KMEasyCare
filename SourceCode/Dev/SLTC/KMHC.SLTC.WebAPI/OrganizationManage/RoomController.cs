/*
 * 描述:RoomController
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/24/2016 1:50:46 PM    张正泉            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using KM.Common;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/rooms")]
    public class RoomController:BaseController
    {
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        // GET 
        [Route(""), HttpGet]
		public IHttpActionResult Get(string roomName="",string floorName="", int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<OrgRoomFilter> request = new BaseRequest<OrgRoomFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { RoomName = roomName, OrgId = SecurityHelper.CurrentPrincipal.OrgId,FloorId=floorName }
            };
            var response = organizationManageService.QueryOrgRoomExtend(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult GetV2(string roomName, string floorId, string sex, int? emptyBedNum, int currentPage = 1, int pageSize = 100)
        {
            BaseRequest<OrgRoomFilter> request = new BaseRequest<OrgRoomFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { RoomName = roomName, OrgId = SecurityHelper.CurrentPrincipal.OrgId, FloorId = floorId, EmptyBedNumber = emptyBedNum, Sex = sex }
            };
            var response = organizationManageService.QueryOrgRoomExtendV2(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{RoomNo}")]
        public IHttpActionResult Get(string RoomNo)
        {
            var response = organizationManageService.GetOrgRoom(RoomNo);
            return Ok(response);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post([FromBody]OrgRoom baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = organizationManageService.SaveOrgRoom(baseRequest);
            return Ok(response);
        }

        [Route("saveRoom")]
        public IHttpActionResult saveRoom(OrgRoom baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = organizationManageService.SaveOrgRoomAndBeds(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{RoomNo}")]
        public IHttpActionResult Delete(string RoomNo)
        {
            var response = organizationManageService.DeleteOrgRoom(RoomNo);
            return Ok(response);
        }
    }
}
