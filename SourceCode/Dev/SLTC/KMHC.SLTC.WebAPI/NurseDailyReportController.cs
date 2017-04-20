using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{

    [RoutePrefix("api/NurseDailyReportList")]
    public class NurseDailyReportController : BaseController
    {

        readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int regno,long feeno)
        {
            BaseRequest<RecordFilter> request = new BaseRequest<RecordFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { RegNo = regno,FEENO=int.Parse(feeno.ToString()) }
            };
            //调用接口
            var response = _INursingRecord.QueryNurseRpttpr(request);
            return Ok(response);
        }


        // 删除列表
        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = _INursingRecord.DeleteNurseDailyReport(id);
            return Ok(response);
        }

        /// <summary>
        ///获取最新的生命体特征
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get(int feeno)
        {
            var response = _INursingRecord.GetNurseDailyReport(feeno);

            return Ok(response);
        }

        /// <summary>
        ///获取最新的生命体特征
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get(int feeno, string recdate, string classtype)
        {
            var response = _INursingRecord.GetOutInt(feeno, recdate, classtype);
            return Ok(response);
        }
        

        /// <summary>
        /// 插入新的
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post(NurseRpttpr baseRequest)
        {
            var response = _INursingRecord.insertNurseRpttpr(baseRequest);

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
