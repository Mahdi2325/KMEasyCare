using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

    namespace KMHC.SLTC.WebAPI.DC.CrossSpeciality
    {
     [RoutePrefix("api/DCDayLifeCarelist")]
        public class DC_CrossListController : BaseController
        {

            IDC_CrossDayLife service = IOCContainer.Instance.Resolve<IDC_CrossDayLife>();




            //[Route(""), HttpGet]
            //public IHttpActionResult Query(int currentPage, int pageSize, int REGNO)
            //{
            //    BaseRequest<RecordFilter> request = new BaseRequest<RecordFilter>
            //    {
            //        CurrentPage = currentPage,
            //        PageSize = pageSize,
            //        Data = { RegNo = REGNO }
            //    };
            //    //调用接口
            //    var response = service.QueryShowDayLife(request);
            //    return Ok(response);
            //}  


            

            [Route("")]
            public IHttpActionResult Get([FromUri]int FEENO)
            {
                var response = service.QueryShowDayLife(FEENO);

                return Ok(response);
            }
        }
    }
