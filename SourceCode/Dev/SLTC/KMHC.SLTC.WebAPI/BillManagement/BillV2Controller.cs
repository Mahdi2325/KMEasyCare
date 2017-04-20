using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.BillManagement
{
    [RoutePrefix("api/billv2")]
    public class BillV2Controller : BaseController
    {
        DateTime now = DateTime.Now;
        IBillV2Service service = IOCContainer.Instance.Resolve<IBillV2Service>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int FeeNo, DateTime sDate, DateTime eDate)
        {
            BaseRequest<BillV2Filter> request = new BaseRequest<BillV2Filter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FeeNo = FeeNo, StarDate = sDate, EndDate = eDate.AddDays(1).AddSeconds(-1) }
            };
            var response = service.QueryBillV2(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, string billId)
        {
            var response = service.QueryBillV2FeeRecord(feeNo, billId);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(string id)
        {
            var response = service.DeleteBillInfo(id);
            return Ok(response);
        }
        [Route("GetYearMonth"), HttpGet]
        public IHttpActionResult GetYearMonth()
        {
            var response = service.GetYearMonth();
            return Ok(response);
        }
        [Route("GetMonthData"), HttpGet]
        public IHttpActionResult GetMonthData(string date)
        {
            var response = service.GetMonthData(date);
            return Ok(response);
        }
        [Route("GetRSMonFees"), HttpGet]
        public IHttpActionResult GetRsMonFees(string date, long feeNo)
        {
            var response = service.GetRSMonFees(date, feeNo);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult GetRsMonFeeDtl(int currentPage, int pageSize, string date, long feeNo, string feeType)
        {
            var response = service.GetRSMonFeeDtl(currentPage, pageSize, date, feeNo, feeType);
            return Ok(response);
        }
        [Route("CancelMonthData"), HttpGet]
        public async Task<IHttpActionResult> CancelMonthData(string date)
        {
            var nsno = service.GetNsno();
            if (nsno == null)
            {
                return null;
            }
            object response=null;
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/MonthFee?date=" + date + "&nsno=" + nsno);
            if (result.IsSuccessStatusCode)
            {
                response = service.CancelMonthData(date);
            }
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult GetMonthDataList(string date, int currentPage, int pageSize)
        {
            BaseRequest<MonthFeeFilter> request = new BaseRequest<MonthFeeFilter>() { CurrentPage = currentPage, PageSize = pageSize, Data = { date = date } };
            var response = service.GetMonthDataList(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public async Task<object> ShowMonthDataList(long NSMonFeeID, int currentPage, int pageSize)
        {
            var nsno = service.GetNsno();
            if (nsno == null)
            {
                return null;
            }
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/MonthFee/GetResMonthData?NSMonFeeID=" + NSMonFeeID + "&currentPage=" + currentPage + "&pageSize=" + pageSize);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }
        [Route("ShowMonthData"), HttpGet]
        public async Task<object> ShowMonthData(long NSMonFeeID)
        {
            var nsno = service.GetNsno();
            if (nsno == null)
            {
                return null;
            }
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/MonthFee/GetOrgMonthData?NSMonFeeID=" + NSMonFeeID);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }
        [Route("GetOrgMonthDataList"), HttpGet]
        public async Task<object> GetOrgMonthDataList(string beginTime, string endTime)
        {
            var nsno = service.GetNsno();
            if (nsno == null)
            {
                return null;
            }
            var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/MonthFee/GetOrgMonthDataList?beginTime=" + beginTime + "&endTime=" + endTime + "&nsno=" + nsno + "");
            object resultContent = await result.Content.ReadAsAsync<BaseResponse<IList<MonFeeModel>>>();
            return resultContent;
        }
        [Route("UploadMonthData"), HttpGet]
        public async System.Threading.Tasks.Task UploadMonthData(string date)
        {
            var obj =  await GetOrgMonthDataList(date, date);
            BaseResponse<IList<MonFeeModel>> res = obj as BaseResponse<IList<MonFeeModel>>;
            if (res.Data.Count > 0)
            {
                if (res.Data[0].Status == (int)NCIPStatusEnum.Pending)
                {
                    throw new Exception(date+"数据已上传，请先撤回，然后再统一上传");
                }
                else if (res.Data[0].Status > (int)NCIPStatusEnum.Pending)
                {
                    throw new Exception(date + "数据已上传");
                }
            }
            var uData = service.UploadMonthData(date);
            var result = await HttpClientHelper.LtcHttpClient.PostAsJsonAsync("/api/MonthFee", uData);
            //string resultContent = await result.Content.ReadAsStringAsync();
            //var resopnse = JsonConvert.DeserializeObject<BaseResponse>(resultContent);
            if (result.IsSuccessStatusCode)
            {
                service.UpdateBillStatusToUploaded(date);
            }
            //return resultContent;
        }
    }
}
