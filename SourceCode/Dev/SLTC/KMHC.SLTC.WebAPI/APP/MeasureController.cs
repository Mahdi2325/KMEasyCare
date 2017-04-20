using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;
using KMHC.Infrastructure;
using System.Collections.Generic;

namespace KMHC.SLTC.WebAPI.APP
{
     [RoutePrefix("api/measure")]
    public class MeasureController : BaseController
    {
         INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

         [Route("GetMeasureDetailByDate"), HttpGet]
         public IHttpActionResult GetMeasureDetailByDate(long feeNo, DateTime date )
         {
             BaseRequest<MeasureFilter> request = new BaseRequest<MeasureFilter>();
             request.Data.FeeNo = feeNo;
             request.Data.MeaSuredTime = date;
             var response = service.GetMeasureDetailByDate(request);
             return Ok(response);
         }

         [Route("QueryMeasureHistory"), HttpGet]
         public IHttpActionResult GetMeasureTotalHistory(long feeno, [FromUri]DateTime? date = null, int currentPage = 1, int pageSize = 10)
         {
             BaseRequest<MeasureFilter> request = new BaseRequest<MeasureFilter>();
             request.CurrentPage = currentPage;
             request.PageSize = pageSize;
             request.Data.FeeNo = feeno;
             request.Data.MeaSuredTime = date;
             var response = service.GetMeasureTotalHistory(request);
             return Ok(response);
         }

         [Route("SaveMeasure"), HttpPost]
         public IHttpActionResult SaveMeasure(List<MeasureFilter> request)
         {
             var response = service.SaveMeasure(request);
             return Ok(response);
         }

         [Route("SaveWeight"), HttpPost]
         public IHttpActionResult SaveWeight(WeightFilter request)
         {
             var response = service.SaveWeight(request);
             return Ok(response);
         }

         [Route("QueryItemList"), HttpGet]
         public IHttpActionResult QueryItemList(long feeno, string itemcode, [FromUri]DateTime? date=null, int currentPage = 1, int pageSize = 10)
         {
             BaseRequest<MeasureFilter> request = new BaseRequest<MeasureFilter>();
             request.CurrentPage = currentPage;
             request.PageSize = pageSize;
             request.Data.FeeNo = feeno;
             request.Data.MeasureItemCode = itemcode;
             request.Data.MeaSuredTime = date;
             BaseResponse response = null;
             if (itemcode=="w_001")
             {
                 response = service.GeWeightList(request);
             }
             else
             {
                 response = service.GetItemList(request);
             }

             return Ok(response);
         }

         [Route("GetRoom"), HttpGet]
         public IHttpActionResult GetRoom(string roomName = "", string floorId = "",string orgId="", int currentPage = 1, int pageSize = 1000)
         {
             IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
             BaseRequest<OrgRoomFilter> request = new BaseRequest<OrgRoomFilter>
             {
                 CurrentPage = currentPage,
                 PageSize = pageSize,
                 Data = { RoomName = roomName, OrgId = orgId, FloorId = floorId }
             };
             var response = organizationManageService.QueryOrgRoomForApp(request);
             return Ok(response);
         }

         [Route("GetFloor"), HttpGet]
         public IHttpActionResult GetFloor(int currentPage = 1, int pageSize = 10,string orgId="", string floorName = "")
         {
             IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
             BaseRequest<OrgFloorFilter> request = new BaseRequest<OrgFloorFilter>
             {
                 CurrentPage = currentPage,
                 PageSize = pageSize,
                 Data = { FloorName = floorName, OrgId = orgId }
             };
             var response = organizationManageService.QueryOrgFloorFromApp(request);
             return Ok(response);
         }

         [Route("QueryResident"), HttpGet]
         public IHttpActionResult QueryResident(string orgId = "", string floorId = "", string roomNo = "", string name = "", int currentPage = 1, int pageSize = 10)
         {

             IResidentManageService rsService = IOCContainer.Instance.Resolve<IResidentManageService>();
             BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
             request.Data.OrgId = orgId;
             request.Data.FloorId = floorId;
             request.Data.RoomNo = roomNo;
             request.Data.Name = name;
             request.CurrentPage = currentPage;
             request.PageSize = pageSize;
             var response = rsService.QueryResidentByFloorAndRoom(request);
             return Ok(response);
         }

         [Route("GetBSDType"), HttpGet]
         public IHttpActionResult GetBSDType()
         {
             IDictManageService codeService = IOCContainer.Instance.Resolve<IDictManageService>();
             BaseResponse<Dictionary<string, string>> response = new BaseResponse<Dictionary<string, string>>();
             response.Data = new Dictionary<string, string>();
             CodeFilter request = new CodeFilter();
             request.ItemType = "F00.001";
             var codeList = codeService.QueryCode(request);
             foreach (var code in codeList.Data)
             {
                 response.Data.Add("S006"+code.ItemCode,code.ItemName);
             }
             return Ok(response);
         }
    }
}
