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

namespace KMHC.SLTC.WebAPI.OrganizationManage
{

    [RoutePrefix("api/Commfilelist")]
    public class CommfileController : BaseController
    {

        readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, string  name)
        {
            BaseRequest<RecordFilter> request = new BaseRequest<RecordFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { Name = name}
            };
            //调用接口
            var response = _INursingRecord.QueryCommFile(request);
            return Ok(response);
        }

        /// <summary>
        /// 插入新的
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Post(List<CommFile> baseRequest)
        {
            var response = _INursingRecord.insertCommfile(baseRequest);

            return Ok(response);
        }



        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = _INursingRecord.DeleteCOMMFILE(id);
            return Ok(response);
        }
        [Route("mulDelete")]
       
        public IHttpActionResult mulDelete(List<CommFile> cfs)
        {
            var response = _INursingRecord.MulDeleteCOMMFILE(cfs);
            return Ok(response);
        }
        //根据id获取相关的信息

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var response = _INursingRecord.GetCommfile(id);

            return Ok(response);

        }

    }
}
