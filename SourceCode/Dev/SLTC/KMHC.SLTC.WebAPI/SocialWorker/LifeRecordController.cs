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
    [RoutePrefix("api/liferecords")]
    public class LifeRecordController:BaseController
    {
        ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();

        [Route(""),HttpGet]
        public IHttpActionResult Query(int feeNo, int currentPage, int pageSize)
        {
            //var response = socialWorkerService.QuerySubsidy(request);
            //return Ok(response);
            BaseRequest<LifeRecordFilter> request = new BaseRequest<LifeRecordFilter>();

            request.CurrentPage = currentPage;

            request.PageSize = pageSize;

            request.Data.FeeNo = feeNo;

            var response = socialWorkerService.QueryLifeRecord(request);

            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult GetLifeRecordById(int id)
        {
            var response = socialWorkerService.GetLifeRecordById(id);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = socialWorkerService.DeleteLifeRecordById(id);
            return Ok(response);
        }
        //[Route("")]
        //public IHttpActionResult Save(LifeRecordModel request)
        //{
        //    request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
        //    request.CreateDate = DateTime.Now;
        //    request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

        //    var response = socialWorkerService.SaveLifeRecord(request);
        //    return Ok(response);
        //}

        #region 生活记录批量操作 2016-04-27 BY 史垚祎
        /// <summary>
        /// 查询生活记录列表
        /// </summary>
        /// <param name="floorName"></param>
        /// <param name="roomName"></param>
        /// <param name="recordDate"></param>
        /// <returns></returns>
        [HttpGet, Route("")]
        public IHttpActionResult QueryLifeRecordList(string floorName, string roomName,int currentPage, int pageSize)
        {
            BaseRequest<LifeRecordListFilter> request = new BaseRequest<LifeRecordListFilter>();
            request.Data.FloorName = floorName;
            request.Data.RoomName = roomName;
			//request.Data.RecordDate = DateTime.Parse(recordDate.ToShortDateString());
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var response = socialWorkerService.QueryLifeRecordList(request);

            return Ok(response);
        }

        /// <summary>
        /// 批量保存生活记录
        /// </summary>
		/// <param name="request"></param>RecordDate
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Save(List<LifeRecordListModel> request)
        {
            var response = socialWorkerService.SaveList(request);
            return Ok(response);
        }
		//[Route("")]
		//public IHttpActionResult Save(LifeRecordModel request) {
		//	var response = socialWorkerService.SaveLifeRecord(request);
		//	return Ok(response);
		//}
        #endregion
    }
}
