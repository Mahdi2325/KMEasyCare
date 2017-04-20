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
using KMHC.Infrastructure;
using System.Threading.Tasks;

namespace KMHC.SLTC.WebAPI
{
     [RoutePrefix("api/Resident")]
    public class ResidentController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord, string ipdFlag, int currentPage, int pageSize)
        {
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            long feeNo = 0;
            long.TryParse(keyWord, out feeNo);
            request.Data.FeeNo = feeNo;
            request.Data.BedNo = keyWord;
            request.Data.Name = keyWord;
            request.Data.ResidengNo = keyWord;
            request.Data.IpdFlag = ipdFlag;
            var response = service.QueryResidentExtend(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(long feeNo, int currentPage, int pageSize)
        {
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.FeeNo = feeNo;
            var response = service.QueryResidentExtend(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(string regName, string floorId,string residentNo)
        {
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            request.CurrentPage = 0;
            request.PageSize = 0;
            request.Data.FloorId = floorId;
            request.Data.Name = regName;
            request.Data.ResidengNo = residentNo;
            request.Data.IpdFlag = "I";
            var response = service.QueryResidentExtend(request);
            return Ok(response);
        } 

        [Route(""), HttpGet]
        public IHttpActionResult Query(string idNo, int currentPage = 1, int pageSize = 1000)
        {
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.IdNo = idNo.ToUpper();
            var response = service.QueryLocaResInfo(idNo);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(string floorName, string roomName)
        {
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.Data.FloorName = floorName;
            request.Data.RoomName = roomName;
            var response = service.QueryResidentByName(request);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetResident(feeNo);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public async Task<object> Query(string idNo, int type)
        {
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/Appcert?idNo=" + idNo + "&type=" + SecurityHelper.CurrentPrincipal.OrgId);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }

        [Route("")]
        public IHttpActionResult Post(Resident baseRequest)
        {
            var response = service.SaveResident(baseRequest);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Delete(int feeNo)
        {
            var response = service.DeleteResident(feeNo);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int regNo,int type)
        {
            var response = new BaseResponse<Boolean>() { Data = service.ExistResident(regNo, new[] { "I", "N" }) };
            //var response = service.ExistResident(regNo, new[] {"I", "N"});
            return Ok(response);
        }
    }
}
