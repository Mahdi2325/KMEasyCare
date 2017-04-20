using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.ChargeInput
{

    [RoutePrefix("api/CostEntry")]
    public class CostEntryController : BaseController
    {
        ICostService service = IOCContainer.Instance.Resolve<ICostService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, long feeNo)
        {
            var response = new BaseResponse<IList<CommonRecord>>();
            BaseRequest<ServiceRecordFilter> request = new BaseRequest<ServiceRecordFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = new ServiceRecordFilter
                {
                    FeeNo = feeNo,
                }
            };
            response = service.QueryCommonRec(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int currentPage, int pageSize, string keyWord)
        {
            BaseRequest<PackageRelatedFilter> request = new BaseRequest<PackageRelatedFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { keyWord = keyWord }
            };
            var response = service.QueryNsDrugByKeyWord(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(string type, int id)
        {
            if (type == "D")
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
            else if (type == "M")
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
            else if (type == "S")
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
        public IHttpActionResult Post(CommonRec Request)
        {
            if (Request.RecType == "D")
            {
                service.SaveDrugRec(Request.drugRec);
            }
            else if (Request.RecType == "M")
            {
                service.SaveMaterialRec(Request.materialRec);
            }
            else if (Request.RecType == "S")
            {
                service.SaveServiceRec(Request.serviceRec);
            }
            return Ok("");
        }

        [Route("updateRecord")]
        public IHttpActionResult Post(DeleteRec delRec)
        {
            if (delRec.RecType == null)
            {
                service.DeleteChargeGroup(delRec.CgcrId);
            }
            else if (delRec.RecType == "D")
            {
                service.DeleteDrugRec(delRec.RecId);
            }
            else if (delRec.RecType == "M")
            {
                service.DeleteMaterialRec(delRec.RecId);
            }
            else if (delRec.RecType == "S")
            {
                service.DeleteServiceRec(delRec.RecId);
            }
            return Ok("");
        }
    }
}
