using KM.Common;
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
    [RoutePrefix("api/Biochemistrys")]
    public class BiochemistrysController : BaseController
    {

        readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();


      
        /// <summary>
        /// 插入到字表的信息
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post(IList<CheckRecdtl> baseRequest)
        {
            var response = _INursingRecord.insertCheckRecdtls(baseRequest);

            return Ok(response);
        }



        /// <summary>
        ///获取项目
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            var response = _INursingRecord.produceitem();

            return Ok(response);

        }

        [Route("")]
        public IHttpActionResult Get([FromUri]string code)
        {
            var response = _INursingRecord.GetCheckitem(code);

            return Ok(response);

        }

        ///// <summary>
        ///// 根据类型的id 获取下面的所有的子项目
        ///// </summary>
        ///// <returns></returns>
        //[Route("{code}"),HttpGet]
        //public IHttpActionResult get(string code)
        //{

        //    var response = _INursingRecord.GetCheckType(code);

        //    return Ok(response);
        //}
    }
}

