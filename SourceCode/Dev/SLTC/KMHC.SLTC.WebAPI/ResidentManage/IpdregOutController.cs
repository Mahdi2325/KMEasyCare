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
    [RoutePrefix("api/IpdregOut")]
    public class IpdregOutController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();
        IRegNCIInfoService regservice = IOCContainer.Instance.Resolve<IRegNCIInfoService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get([FromUri]long feeNo)
        {
            var response = service.GetIpdregout(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(Ipdregout request)
        {
            try
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            catch (Exception)
            {
                request.CreateBy = "Test";
                request.CreateDate = DateTime.Now;
                request.OrgId = "1";
            }

            var response = service.SaveIpdregout(request);
            return Ok(response.Data);
        }


        [Route("CheckBillInfo"), HttpGet]
        public IHttpActionResult CheckBillInfo(int feeNo, DateTime? ipdoutTime)
        {
            var response = service.QueryIpdBillInfo(feeNo, ipdoutTime);
            return Ok(response);
        }

        [Route("RegCert"), HttpGet]
        public async Task<object> Query(int feeNo, DateTime? ipdoutTime)
        {
            var response = regservice.GetLTCRegInfo(feeNo);
            response.Data.ipdoutTime = ipdoutTime;
            var result = await HttpClientHelper.LtcHttpClient.PostAsJsonAsync("/api/AuditYearCert/ltccert", response.Data);
            object resultContent = await result.Content.ReadAsAsync<object>();
            return resultContent;
        }


        //[Route(""), HttpGet]
        //public IHttpActionResult Query(string keyWord, int currentPage, int pageSize)
        //{
        //    BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
        //    request.CurrentPage = currentPage;
        //    request.PageSize = pageSize;
        //    long feeNo = 0;
        //    long.TryParse(keyWord, out feeNo);
        //    request.Data.FeeNo = feeNo;
        //    request.Data.BedNo = keyWord;
        //    request.Data.Name = keyWord;
        //    var response = service.QueryResidentExtend(request);
        //    return Ok(response);
        //}

        //[Route("{feeNo}")]
        //public IHttpActionResult Get(int feeNo)
        //{
        //    var response = service.GetResident(feeNo);
        //    return Ok(response);
        //}

        //[Route("")]
        //public IHttpActionResult Post(Resident baseRequest)
        //{
        //    var response = service.SaveResident(baseRequest);
        //    return Ok(response);
        //}

        //[Route("{feeNo}")]
        //public IHttpActionResult Delete(int feeNo)
        //{
        //    var response = service.DeleteResident(feeNo);
        //    return Ok(response);
        //}

        //[Route(""), HttpGet]
        //public IHttpActionResult Get(int regNo,int type)
        //{
        //    var response = new BaseResponse<Boolean>() { Data = service.ExistResident(regNo, new[] { "I", "N" }) };
        //    //var response = service.ExistResident(regNo, new[] {"I", "N"});
        //    return Ok(response);
        //}
    }
}
