using KM.Common;
using KMHC.SLTC.Business.Entity;
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
    [RoutePrefix("api/Biochemistry")]
   public class BiochemistryController : BaseController
    {

        readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();

         [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int FEENO)
         {
             BaseRequest<RecordFilter> request = new BaseRequest<RecordFilter>
             {
                 CurrentPage = currentPage,
                 PageSize = pageSize,
                 Data = { FEENO = FEENO }
             };
             //调用接口
             var response = _INursingRecord.QueryBiochemistry(request);
             return Ok(response);
         }  

         // 删除列表
         // DELETE api/syteminfo/5
         [Route("{id}")]
         public IHttpActionResult Delete(int id)
         {
             var response = _INursingRecord.DeleteBiochemistry(id);
             return Ok(response);
         }

         

         /// <summary>
         /// 插入到字表的信息
         /// </summary>
         /// <returns></returns>
         [Route("")]
         public IHttpActionResult Post(CheckRecdtl baseRequest)
         {
             var response = _INursingRecord.insertCheckRecdtl(baseRequest);

             return Ok(response);
         }

         /// <summary>
         /// 插入到字表的信息
         /// </summary>
         /// <returns></returns>
         [Route("")]
         public IHttpActionResult Get([FromUri]string code)
         {
             var response = _INursingRecord.GetCheckType(code);

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

         //[Route("{FeeNo}"), HttpGet]
         //public IHttpActionResult Get(long feeNo)
         //{
         //    BaseRequest<BedSoreRecFilter> request = new BaseRequest<BedSoreRecFilter>()
         //    {
         //        Data = new BedSoreRecFilter()
         //        {
         //            FeeNo = feeNo,
         //            OrgId = SecurityHelper.CurrentPrincipal.OrgId
         //        }
         //    };
         //    var response = service.GetBedSoreRecExtend(request);
         //    return Ok(response);
         //}
       

    }
}
