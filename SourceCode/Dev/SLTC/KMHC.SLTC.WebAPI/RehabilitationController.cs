using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/referral")]
    public class RehabilitationController : BaseController
    {
        //实列化接口对象

        readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();

        //readonly IGoodsManageService _goodsManageService = IOCContainer.Instance.Resolve<IGoodsManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, int FEENO)
        {
            BaseRequest<RecordFilter> request = new BaseRequest<RecordFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FEENO = FEENO }
            };
            //调用接口
            var response = _INursingRecord.QueryRehabilition(request);
            return Ok(response);
        }

        // 删除列表
        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = _INursingRecord.DeleteRehabilition(id);
            return Ok(response);
        }

        /// <summary>
        /// 插入新的
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post(Rehabilitrec baseRequest)
        {
            var response = _INursingRecord.insertRehabilition(baseRequest);

            return Ok(response);
        }   

        /// <summary>
        /// 根据id获取相关的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {

            BaseRequest<tt> request = new BaseRequest<tt>
            {

                Data = { ID = id }
            };


            var response = _INursingRecord.GetRehabilition(request);
            return Ok(response);
        }

        /// <summary>
        /// 获取用户的相关信息
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            var response = _INursingRecord.GetName();

            return Ok(response);

        }


    }
}
