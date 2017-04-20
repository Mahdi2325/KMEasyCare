using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/InfectionSympotm")]
    public class InfectionSympotmController : BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(long seqNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<InfectionSympotmFilter> request = new BaseRequest<InfectionSympotmFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.SeqNo = seqNo;
            var response = service.QueryInfectionSympotm(request);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetInfectionInd(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(List<InfectionSympotm> baseRequest)
        {
            service.SaveInfectionSympotms(baseRequest);
            return Ok(new BaseResponse<object>());
        }

        [Route("")]
        public IHttpActionResult Delete(string ids)
        {
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var arrIds = ids.Split(',');
                long[] idsL = new long[arrIds.Length];
                for (int i=0;i<arrIds.Length;i++)
                {
                    idsL[i] = int.Parse(arrIds[i]);
                }
                service.DeleteInfectionSympotms(idsL);
            }
           
            return Ok(new BaseResponse<object>());
        }
    }
}