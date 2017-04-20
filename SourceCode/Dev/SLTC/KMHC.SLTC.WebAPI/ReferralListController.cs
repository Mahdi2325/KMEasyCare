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
   //转诊
  [RoutePrefix("api/referralListZ")]
    //加载列表
    public class ReferralListController : BaseController
    {

      readonly INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();

      [Route(""), HttpGet]
      public IHttpActionResult Get(int CurrentPage, int PageSize, int FEENO)
      {
          BaseRequest<RecordFilter> request = new BaseRequest<RecordFilter>
          {
              CurrentPage = CurrentPage,
              PageSize = PageSize,
              Data = { FEENO = FEENO }
          };
          //调用接口
          var response = _INursingRecord.QueryReferralLis(request);
          return Ok(response);
      }

      // 删除
      // DELETE api/syteminfo/5
      [Route("{id}")]
      public IHttpActionResult Delete(int id)
      {
          var response = _INursingRecord.DeleteReferralLis(id);
          return Ok(response);
      }

      /// <summary>
      /// 插入新的
      /// </summary>
      /// <returns></returns>
      [Route("")]
      public IHttpActionResult Post(TranSferVisit baseRequest)
      {
          var response = _INursingRecord.insertReferralLis(baseRequest);

          return Ok(response);
      }
    /// <summary>
    /// 获得用户信息的
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
      [Route("")]
      public IHttpActionResult Get()
      {
          var response = _INursingRecord.GetName();

          return Ok(response);

      }

    }
}

