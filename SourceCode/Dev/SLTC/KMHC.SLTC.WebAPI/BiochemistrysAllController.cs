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
    [RoutePrefix("api/Biochemistrylist")]
    public class BiochemistrysAllController : BaseController
    {

        readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();

        /// <summary>
        /// 插入到字表的信息
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post(CheckRecCollection baseRequest)
        {   
            var response = _INursingRecord.insertCheckRec(baseRequest);

            return Ok(response);
        }

        /// <summary>
        /// 插入到字表的信息
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult get()
        {
            var response = _INursingRecord.GetProduceCode();

            return Ok(response);
        }

        // 删除子列表中的东西
        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(int id, int type)
        {
            var response = _INursingRecord.DeletesBiochemistry(id, type);
            return Ok(response);
        }

        /// <summary>
        /// 插入到字表的信息
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 插入到字表的信息
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get([FromUri]string code)
        {
            var response = _INursingRecord.Checkitem(code);

            return Ok(response);

        }



     
    }
}
