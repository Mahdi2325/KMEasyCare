using KM.Common;
using KMHC.Infrastructure;
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
    [RoutePrefix("medicines")]
    public class DrugController : BaseController
    {
        INursingManageService service = IOCContainer.Instance.Resolve<INursingManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]int CurrentPage, int PageSize, string keyWord)
        {
            MedicineFilter filter = new MedicineFilter {
                KeyWord = keyWord
            };
            BaseRequest<MedicineFilter> request = new BaseRequest<MedicineFilter> { 
            Data=filter,
            CurrentPage = CurrentPage,
            PageSize = PageSize
            };
            var response = service.QueryMedData(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult post(Medicine request)
        {
            try
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            catch
            {
                request.OrgId = "1";
            }
            var response = service.SaveMedData(request);
            return Ok(response.Data);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteMedData(id);
            return Ok(response);
        }

    }
}
