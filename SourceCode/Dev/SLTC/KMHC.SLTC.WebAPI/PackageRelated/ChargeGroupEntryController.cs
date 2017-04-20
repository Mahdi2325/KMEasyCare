using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.PackageRelated;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.PackageRelated
{

    [RoutePrefix("api/chargeGroupRec")]
    public class ChargeGroupEntryController:BaseController
    {
        IPackageRelatedService service = IOCContainer.Instance.Resolve<IPackageRelatedService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(int feeNo, int CurrentPage, int PageSize)
        {
            var request = new BaseRequest<ChargeGroupRecFilter>();
            request.CurrentPage = CurrentPage;
            request.PageSize = PageSize;
            request.Data.FeeNo = feeNo;
            var response = service.QueryChargeGroupRec(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(string type, int id)
        {
            if (type == "1")
            {
                var request = new BaseRequest<DrugRecordFilter>()
                {
                    Data = new DrugRecordFilter()
                    {
                        DrugrecordId = id,
                    }
                };
                var response = service.GetDrugRec(request);
                return Ok(response);
            }
            else if (type == "2")
            {
                var request = new BaseRequest<MaterialRecordFilter>()
                {
                    Data = new MaterialRecordFilter()
                    {
                        MaterialRecordId = id,
                    }
                };
                var response = service.GetMaterialRec(request);
                return Ok(response);
            }
            else if (type == "3")
            {
                var request = new BaseRequest<ServiceRecordFilter>()
                {
                    Data = new ServiceRecordFilter()
                    {
                        ServiceRecordId = id,
                    }
                };
                var response = service.GetServiceRec(request);
                return Ok(response);
            }
            else
            {
                var response = new BaseResponse<ServiceRecordFilter>();
                return Ok(response);
            }

        }

        [Route("")]
        public IHttpActionResult Post(ChargeItemData request)
        {
            var response = service.SaveChargeGroupRec(request);
            return Ok(response);
        }
    }
}
