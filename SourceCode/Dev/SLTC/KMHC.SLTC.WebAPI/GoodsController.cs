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
    [RoutePrefix("api/goods")]
    public class GoodsController : BaseController
    {
        readonly IGoodsManageService _goodsManageService = IOCContainer.Instance.Resolve<IGoodsManageService>();

        // GET api/manufactures
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, string name, string type,string no)
        {
            BaseRequest<GoodsFilter> request = new BaseRequest<GoodsFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { Name = name,Type = type,No = no}
            };
            var response = _goodsManageService.QueryGoods(request);
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = _goodsManageService.GetGoods(id);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Goods baseRequest)
        {
            var response = _goodsManageService.SaveGoods(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = _goodsManageService.DeleteGoods(id);
            return Ok(response);
        }
    }
}