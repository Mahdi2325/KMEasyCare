using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/DrugManage")]
    public class DrugManageController : BaseController
    {

        IDC_ResidentManageService service = IOCContainer.Instance.Resolve<IDC_ResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get(long feeNo)
        {
            DcRegMedicine filter = new DcRegMedicine
            {
                FeeNo = feeNo
            };
            BaseRequest<DcRegMedicine> request = new BaseRequest<DcRegMedicine>
            {
                Data = filter
            };
            var response = service.QueryMedicine(request);
            return Ok(response);
        }

        [Route()]
        public IHttpActionResult post(DcRegMedicine Data)
        {
            var response = new BaseResponse<DcRegMedicine>
            {
                Data = Data
            };
            service.saveMedicine(response);
            return Ok(response);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteMedicine(id);
            return Ok(response);
        }
    }
}
