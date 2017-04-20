using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using KM.Common;
using KMHC.Infrastructure;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.ChargeInputModel;

namespace KMHC.SLTC.WebAPI.ExtDataInterface
{
    //LTC数据对外查询接口
    [RoutePrefix("api/Ext/Nursing")]
    public class ExtNursingDataController : BaseController
    {
        private readonly INursingWorkstationService _nursingWorkstationService = IOCContainer.Instance.Resolve<INursingWorkstationService>();
        private readonly INursingManageService _nursingManageService = IOCContainer.Instance.Resolve<INursingManageService>();
        private readonly IHealthRecordsService _healthRecordsService = IOCContainer.Instance.Resolve<IHealthRecordsService>();
        private readonly ICarePlansManageService _carePlansManageService = IOCContainer.Instance.Resolve<ICarePlansManageService>();
        
        [Route("GetNsRecords"), HttpGet]
        public IHttpActionResult GetNsRecords(int feeNo, DateTime st, DateTime et)
        {
            var response = _nursingWorkstationService.QueryNursingRec(new BaseRequest<NursingRecFilter>()
            {
                PageSize = 999999,
                CurrentPage = 1,
                Data = new NursingRecFilter()
            {
                FeeNo = feeNo,
                SDate = st,
                EDate = et
            }
            });
            return Ok(response);
        }

        [Route("GetMeasuredRecords"), HttpGet]
        public IHttpActionResult GetMeasuredRecords(int feeNo, DateTime st, DateTime et)
        {
            var response = _healthRecordsService.GetMeasureDataForExtApi(new BaseRequest<MeasureDataFilter>()
            {
                PageSize = 999999,
                CurrentPage = 1,
                Data = new MeasureDataFilter()
                {
                    FeeNo = feeNo,
                    SDate = st,
                    EDate = et
                }
            });
            return Ok(response);
        }

        [Route("GetEvalRecords"), HttpGet]
        public IHttpActionResult GetEvalRecords(int feeNo, DateTime st, DateTime et)
        {
            var response = _nursingManageService.GetEvalRecsForExtApi(new BaseRequest<EvalRecFilter>()
            {
                PageSize = 999999,
                CurrentPage = 1,
                Data = new EvalRecFilter()
                {
                    FeeNo = feeNo,
                    SDate = st,
                    EDate = et
                }
            });
            return Ok(response);
        }
        [Route("GetCplRecords"), HttpGet]
        public IHttpActionResult GetCplRecords(int feeNo, DateTime st, DateTime et)
        {
            var response = _carePlansManageService.GetCarePlanForExtApi(new BaseRequest<CarePlanRecFilter>()
            {
                PageSize = 999999,
                CurrentPage = 1,
                Data = new CarePlanRecFilter()
                {
                    FeeNo = feeNo,
                    SDate = st,
                    EDate = et
                }
            });
            return Ok(response);
        }
    }
}
