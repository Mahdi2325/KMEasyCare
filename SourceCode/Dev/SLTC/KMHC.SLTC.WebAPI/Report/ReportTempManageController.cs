#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :2017/3/28
** 说明     :报表管理控制器
******************************************************************/
#endregion

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
using System.Net.Http;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;

namespace KMHC.SLTC.WebAPI.Report
{
    [RoutePrefix("api/ReportTempManage")]
    public class ReportTempManageController : BaseController
    {
        IBillV2Service service = IOCContainer.Instance.Resolve<IBillV2Service>();
        IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
        [Route(""), HttpGet]
        public async Task<IHttpActionResult> Get(int CurrentPage, int PageSize, DateTime? startDate = null, DateTime? endDate = null, string mark = "")
        {
            BaseResponse<object> response = new BaseResponse<object>();
            switch (mark)
            {
                case "MonthFee":
                    var nsno = service.GetNsno();
                    var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/MonthFee?sDate=" + startDate.Value.ToString("yyyy-MM") + "&eDate=" + endDate.Value.ToString("yyyy-MM")+"-"  + "&nsno=" + nsno + "&currentPage=" + CurrentPage + "&pageSize=" + PageSize);
                    var res = result.Content.ReadAsAsync<BaseResponse<IList<TreatmentAccount>>>().Result;
                    response.RecordsCount = res.RecordsCount;
                    response.PagesCount = res.PagesCount;
                    List<PrintMonthFee> list = new List<PrintMonthFee>();
                    try
                    {
                        list = service.GetPrintData(res);
                    }
                    catch
                    {
                        return Ok(response);
                    }
                    object obj = new
                    {
                        dataList = list,
                        tHospDay = list.Sum(s => s.HospDay),
                        tTotalAmount = list.Sum(s => s.TotalAmount),
                        tNCIPay = list.Sum(s => s.NCIPay)
                    };
                    response.Data = obj;
                    break;
            }
            return Ok(response);
        }
        [Route(""), HttpGet]  
        public IHttpActionResult Get(int CurrentPage, int PageSize,long feeNo, DateTime startDate, DateTime endDate, string mark)
        {
            BaseResponse<object> response = new BaseResponse<object>();
            switch (mark)
            {
                case "FeeList":
                    var sDate = dictManageService.GetFeeIntervalByDate(startDate).Month;
                    var eDate = dictManageService.GetFeeIntervalByDate(endDate).Month;
                    var nsno = service.GetNsno();
                    var res = service.QueryBillV2FeeList(feeNo, startDate, endDate);
                    response.RecordsCount = res.RecordsCount;
                    response.PagesCount = res.PagesCount;

                    object obj = new
                    {
                        regInformation = res.Data.regInformation,
                        feeRecordList = res.Data.feeRecordList.GroupBy(
                        x => new
                        {
                            x.ProjectName,
                            x.UnitPrice,
                            x.Units
                        })
                        .Select(g => new
                        {
                            ProjectName = g.Key.ProjectName,
                            UnitPrice = g.Key.UnitPrice,
                            Units = g.Key.Units,
                            Count = g.Sum(a => a.Count),
                            Cost = g.Sum(a => a.Cost)
                        }).OrderByDescending(x => x.ProjectName)
                        .ToList(),
                        totalCount = res.Data.feeRecordList.Sum(s => s.Count),
                        totalCost = res.Data.feeRecordList.Sum(s => s.Cost)
                    };
                    response.Data = obj;
                    break;
            }
            return Ok(response);
        }
    }
}
