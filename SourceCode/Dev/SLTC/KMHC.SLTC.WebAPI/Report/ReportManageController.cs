using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/ReportManage")]
    public class ReportManageController : ApiController
    {
        IReportManageService service = IOCContainer.Instance.Resolve<IReportManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult QueryWithManage(int currentPage, int pageSize)
        {
            BaseRequest<ReportFilter> request = new BaseRequest<ReportFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.Data.Status = true;
            BaseResponse<IList<ReportModel>> response = service.QueryReport(request);
            if (response.RecordsCount == 0) {
                request.Data.OrgId = "000";
                request.Data.Status = null;
                response = service.QueryReport(request);
            }
            BaseResponse<List<ReportSetModel>> newResponse = new BaseResponse<List<ReportSetModel>>();
            if (response.RecordsCount > 0)
            {
                newResponse.Data = new List<ReportSetModel>();
                var group = response.Data.ToLookup(it => it.MajorType);
                foreach (var item in group)
                {
                    ReportSetModel newItem = new ReportSetModel();
                    newItem.MajorType = item.Key;
                    newItem.Items = item.OrderBy(it=>it.Name).ToList();
                    newResponse.Data.Add(newItem);
                }
            }
            return Ok(newResponse);
        }

        [Route("Set"), HttpGet]
        public IHttpActionResult QueryWithSet(int currentPage, int pageSize)
        {
            BaseRequest<ReportFilter> request = new BaseRequest<ReportFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.OrgId = "000";
            BaseResponse<IList<ReportModel>> response = service.QueryReport(request);
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.Data.Status = true;
            BaseResponse<IList<ReportModel>> responseSelf = service.QueryReport(request);
            foreach (var item in response.Data) {
                item.Status = responseSelf.Data.Any(it => it.Code == item.Code);
            }

            BaseResponse<List<ReportSetModel>> newResponse = new BaseResponse<List<ReportSetModel>>();
            if (response.RecordsCount > 0)
            {
                newResponse.Data = new List<ReportSetModel>();
                var group = response.Data.ToLookup(it => it.MajorType);
                foreach (var item in group)
                {
                    ReportSetModel newItem = new ReportSetModel();
                    newItem.MajorType = item.Key;
                    newItem.Items = item.OrderBy(it=>it.Name).ToList();
                    newResponse.Data.Add(newItem);
                }
            }
            return Ok(newResponse);
        }

        [Route("Set"), HttpPost]
        public IHttpActionResult Save(List<ReportSetModel> request)
        {
            service.SaveReport(SecurityHelper.CurrentPrincipal.OrgId, request);
            return Ok();
        }
    }
}