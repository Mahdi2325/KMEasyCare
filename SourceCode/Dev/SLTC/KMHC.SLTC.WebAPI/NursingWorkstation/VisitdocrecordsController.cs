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

namespace KMHC.SLTC.WebAPI.NursingWorkstation
{
    [RoutePrefix("visitdocrecords")]
    public class VisitdocrecordsController : BaseController
    {
        INursingManageService service = IOCContainer.Instance.Resolve<INursingManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]int CurrentPage, int PageSize, long feeNo)
        {
            VisitDocRecordsFilter filter = new VisitDocRecordsFilter
            {
                FeeNo = feeNo
            };
            BaseRequest<VisitDocRecordsFilter> request = new BaseRequest<VisitDocRecordsFilter>
            {
                Data = filter,
                CurrentPage = CurrentPage,
                PageSize = PageSize
            };
            var response = service.QueryVisitDocRecData(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(VisitDocRecords baseRequest)
        {
            baseRequest.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveVisitDocRecData(baseRequest);
            return Ok(response);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteVisitDocRecData(id);
            return Ok(response);
        }
    }

    [RoutePrefix("api/visitHospital")]
    public class VisitHospitalController : BaseController
    {
        IVisitManageService service = IOCContainer.Instance.Resolve<IVisitManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult get()
        {
            var filter = new VisitHospitalFilter
            {
                OrgId = SecurityHelper.CurrentPrincipal.OrgId
            };
            var request = new BaseRequest<VisitHospitalFilter>
            {
                Data = filter
            };
            var response = service.QueryVisitHospital(request);
            return Ok(response);
        }
    }

    [RoutePrefix("api/visitDept")]
    public class VisitDeptController : BaseController
    {
        IVisitManageService service = IOCContainer.Instance.Resolve<IVisitManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]string hospId)
        {
            var filter = new VisitDeptFilter
            {
                HospNo = hospId
            };
            var request = new BaseRequest<VisitDeptFilter>
            {
                Data = filter
            };
            var response = service.QueryVisitDept(request);
            return Ok(response);
        }
    }

    [RoutePrefix("api/visitDoctor")]
    public class VisitDoctorController : BaseController
    {
        IVisitManageService service = IOCContainer.Instance.Resolve<IVisitManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]string deptId)
        {
            var filter = new VisitDoctorFilter
            {
                DeptNo = deptId
            };
            var request = new BaseRequest<VisitDoctorFilter>
            {
                Data = filter
            };
            var response = service.QueryVisitDoctor(request);
            return Ok(response);
        }
    }

    [RoutePrefix("api/icd9")]
    public class Icd9Controller : BaseController
    {
        IVisitManageService service = IOCContainer.Instance.Resolve<IVisitManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]string keyWord, int CurrentPage, int PageSize)
        {
            var filter = new Icd9_DiseaseFilter
            {
                KeyWord = keyWord
            };
            var request = new BaseRequest<Icd9_DiseaseFilter>
            {
                Data = filter,
                CurrentPage = CurrentPage,
                PageSize = PageSize
            };
            var response = service.QueryIcd9(request);
            return Ok(response);
        }
    }

    [RoutePrefix("api/Freq")]
    public class FreqController : BaseController
    {
        IVisitManageService service = IOCContainer.Instance.Resolve<IVisitManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult get([FromUri]string keyWord, int CurrentPage, int PageSize)
        {
            var filter = new FreqFilter
            {
                KeyWord = keyWord
            };
            var request = new BaseRequest<FreqFilter>
            {
                Data = filter,
                CurrentPage = CurrentPage,
                PageSize = PageSize
            };
            var response = service.QueryFreq(request);
            return Ok(response);
        }
    }
}
