using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/Person")]
    public class PersonController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord, int currentPage, int pageSize, string FloorName, string RoomName)
        {
            BaseRequest<PersonFilter> request = new BaseRequest<PersonFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.Name = keyWord;
            request.Data.IdNo = keyWord;
            request.Data.keyWord = keyWord;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            request.Data.IpdFlag = "I";
            request.Data.FloorName = FloorName;
            request.Data.RoomName = RoomName;
            var responseByQuery = service.QueryPersonExtend(request);
            var response = new BaseResponse<List<object>>(new List<object>());
            response.CurrentPage = responseByQuery.CurrentPage;
            response.PagesCount = responseByQuery.PagesCount;
            response.RecordsCount = responseByQuery.RecordsCount;
            Array.ForEach(responseByQuery.Data.ToArray(), (item) =>
            {
                response.Data.Add(new
                {
                    FeeNo = item.FeeNo,
                    RegNo = item.RegNo,
                    IdNo = item.IdNo,
                    CreateDate = item.CreateDate,
                    Name = item.Name,
                    Sex = item.Sex,
                    Age = item.Age,
                    Birthdate = item.Brithdate,
                    RsType = item.RsType,
                    RsStatus =item.RsStatus,
                    Race=item.Race,
                    Political= item.Political,
                    Floor = item.Floor,
                    BedNo = item.BedNo,
                    PhotoPath = item.Relation.PhotoPath,
                    InDate = item.InDate
                });
            });
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Query(int sNo)
        {
            var response = service.GetScenario(sNo);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Query(int regNo,string mark)
        {
            var response = service.QueryRegdiseasehis(regNo);
            return Ok(response);
        }
        [Route("{regNo}")]
        public IHttpActionResult Get(int regNo)
        {
            var response = service.GetPerson(regNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(Person baseRequest)
        {
            var response = service.SavePerson(baseRequest);
            return Ok(response);
        }

        [Route("{regNo}")]
        public IHttpActionResult Delete(int regNo)
        {
            var response = service.DeletePerson(regNo);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize,string name,string idno)
        {
            BaseRequest<PersonFilter> request = new BaseRequest<PersonFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { Name = name, IdNo = idno }
            };
            var response = service.QueryPerson(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(string residentName, string ipdFlag, string IdNo="")
        {
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            BaseResponse<IList<Resident>> response = null;
            if (!string.IsNullOrEmpty(residentName) || !string.IsNullOrEmpty(IdNo))
            {
                request.Data.Name = residentName;
                request.Data.IpdFlag = ipdFlag;
                request.Data.IdNo = IdNo;
                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                response = service.QueryResidentByName(request);
            }
            return Ok(response);
        }
    }
}