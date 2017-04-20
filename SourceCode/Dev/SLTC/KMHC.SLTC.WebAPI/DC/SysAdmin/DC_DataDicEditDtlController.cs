using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;

namespace KMHC.SLTC.WebAPI.DC.SysAdmin
{
    [RoutePrefix("api/DCDataDicEditDtl")]
    public class DC_DataDicEditController : BaseController
    {
        IDC_SysAdminService service = IOCContainer.Instance.Resolve<IDC_SysAdminService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(string ITEMTYPE, int currentPage, int pageSize)
        {
            BaseRequest<DC_TeamActivitydtlFilter> request = new BaseRequest<DC_TeamActivitydtlFilter>();
           // request.CurrentPage = currentPage;
           // request.PageSize = pageSize;
           // if (keyWord == null)
           // { request.Data.SEQNO = 0; }
           // else
           // { request.Data.SEQNO = 0;}
           // request.Data.TITLENAME = keyWord;

            var responseByQuery = service.QueryDCCOMMDTL(ITEMTYPE);
            var response = new BaseResponse<List<object>>(new List<object>());
            //response.CurrentPage = responseByQuery.CurrentPage;
            //response.PagesCount = responseByQuery.PagesCount;
            //response.RecordsCount = responseByQuery.RecordsCount;
            Array.ForEach(responseByQuery.Data.ToArray(), (item) =>
            {
               response.Data.Add(new
                {

                    ITEMCODE = item.ITEMCODE,
                    ITEMNAME = item.ITEMNAME,
                    ITEMTYPE = item.ITEMTYPE,
                    DESCRIPTION = item.DESCRIPTION,
                    ORDERSEQ  =item.ORDERSEQ,
                    UPDATEDATE =item.UPDATEDATE,
                    UPDATEBY = item.UPDATEBY

                });
         
            });

            return Ok(response); 
            
        }

        [Route("")]
        public IHttpActionResult Post(DC_COMMDTLModel baseRequest)
        {

            var response = service.SaveDCCOMMDtl(baseRequest);
            return Ok(response);
            
            
        }

        [Route("")]
        public IHttpActionResult Delete(string type,string code ) 
        {
             
             var response = service.DeleteDCcOMMDTL(type,code);
             return Ok(response);
            
            
        }


        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {

            var response = service.GetDCcommfile(id);
            return Ok(response);


        }

    }
}