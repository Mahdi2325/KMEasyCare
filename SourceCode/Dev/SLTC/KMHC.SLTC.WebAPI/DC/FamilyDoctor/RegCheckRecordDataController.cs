using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.FamilyDoctor
{
    [RoutePrefix("api/RegCheckRecordData")]
    public class RegCheckRecordDataController : BaseController
    {
        IDC_FamilyDoctorService service = IOCContainer.Instance.Resolve<IDC_FamilyDoctorService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(DateTime? startDate, DateTime? endDate, bool displayType, string testItem, string regNo, int currentPage, int pageSize)
        {
            BaseRequest<DC_RegCheckRecordDataFilter> request = new BaseRequest<DC_RegCheckRecordDataFilter>();
            //request.Data.StartDate = startDate;
            //request.Data.EndDate = endDate;
            //request.Data.DisplayType = displayType;
            //request.Data.TestItem = testItem;
            //request.Data.RegNo = regNo;
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var regCheckRecordDataList = service.QueryRegCheckRecordData(request);
            return Ok(regCheckRecordDataList);
        }
    }
}
