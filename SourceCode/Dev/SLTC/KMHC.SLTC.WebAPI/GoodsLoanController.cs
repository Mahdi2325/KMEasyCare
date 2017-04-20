using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;


namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/goodsLoan")]
    public class GoodsLoanController : BaseController
    {
        readonly IGoodsManageService _goodsManageService = IOCContainer.Instance.Resolve<IGoodsManageService>();

        // GET api/manufactures
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, int goodsId, DateTime? startDate,DateTime? endDate)
        {
            BaseRequest<GoodsRecordFilter> request = new BaseRequest<GoodsRecordFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { GoodsId = goodsId, StartDate = startDate , EndDate = endDate}
            };
            var response = _goodsManageService.QueryGoodsLoan(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = _goodsManageService.GetGoodsLoan(id);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(GoodsLoan baseRequest)
        {
            var response = _goodsManageService.SaveGoodsLoan(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = _goodsManageService.DeleteGoodsLoan(id);
            return Ok(response);
        }
    }
}