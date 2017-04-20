using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.NurseCare
{
    [RoutePrefix("api/DCRegBaseInfoList")]
    public class DC_RegBaseInfoListController : BaseController
    {
        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();


        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo,int CurrentPage, int PageSize)
        {
            DC_RegBaseInfoListFilter filter = new DC_RegBaseInfoListFilter
            {
                FeeNo = feeNo,
            };
            BaseRequest<DC_RegBaseInfoListFilter> request = new BaseRequest<DC_RegBaseInfoListFilter>
            {
                Data = filter
            };
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            var response = service.QueryAllRegBaseInfoList(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo)
        {
            DC_RegBaseInfoListFilter filter = new DC_RegBaseInfoListFilter
            {
                FeeNo = feeNo,
            };
            BaseRequest<DC_RegBaseInfoListFilter> request = new BaseRequest<DC_RegBaseInfoListFilter>
            {
                Data = filter
            };
            var response = service.QueryRegBaseInfoList(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo,string regNo,int cnt)
        {
            DC_RegBaseInfoListFilter filter = new DC_RegBaseInfoListFilter
            {
                FeeNo=feeNo,
                RegNo = regNo,
                Cnt = cnt,
            };
            BaseRequest<DC_RegBaseInfoListFilter> request = new BaseRequest<DC_RegBaseInfoListFilter>
            {
                Data = filter
            };
            var response = service.QueryRegBaseInfo(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult post(DC_RegBaseInfoList request)
        {
            try
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            catch
            {
                request.OrgId = "0000000001";
            }
            var response = service.saveRegBaseInfoList(request);
            return Ok(response);
        }
    }
}
