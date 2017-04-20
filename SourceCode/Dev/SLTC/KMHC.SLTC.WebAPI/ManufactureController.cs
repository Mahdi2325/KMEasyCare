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
    [RoutePrefix("api/manufactures")]
    public class ManufactureController : BaseController
    {
        readonly IGoodsManageService _goodsManageService = IOCContainer.Instance.Resolve<IGoodsManageService>();

        // GET api/Floor
        [Route(""), HttpGet]
        public IHttpActionResult Query()
        {
            var response = _goodsManageService.QueryManufacture();
            return Ok(response.Data);
        }


        // GET api/manufactures
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, string keyword)
        {
            BaseRequest<CommonFilter> request = new BaseRequest<CommonFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { Keywords = keyword }
            };
            var response = _goodsManageService.QueryManufacture(request);
            //response.PagesCount = 2;
            return Ok(response);
        }

        // GET api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var response = _goodsManageService.GetManufacture(id);
            return Ok(response.Data);
        }

        // POST api/syteminfo
        [Route("")]
        public IHttpActionResult Post(Manufacture baseRequest)
        {
            var response = _goodsManageService.SaveManufacture(baseRequest);
            return Ok(response);
        }

        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = _goodsManageService.DeleteManufacture(id);
            return Ok(response);
        }
    }
}