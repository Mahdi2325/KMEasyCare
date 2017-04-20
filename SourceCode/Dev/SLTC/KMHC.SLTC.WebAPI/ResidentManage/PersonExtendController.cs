using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.ResidentManage
{
     [RoutePrefix("api/PersonExtend")]
    public class PersonExtendController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query( int currentPage, int pageSize,string FEENO)
        {
            BaseRequest<PersonExtendFilter> request = new BaseRequest<PersonExtendFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(FEENO);
            var o = serializer.Deserialize(new JsonTextReader(sr), typeof(long[]));
            request.Data.FEENO = o as long[];
            var responseByQuery = service.QueryPersonExtend2(request);
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
                    Floor = item.Floor,
                    BedNo = item.BedNo,
                    PhotoPath = item.Relation.PhotoPath,
                    InDate = item.InDate,
                    ResidengNo=item.ResidengNo
                });
            });
            return Ok(response);
        }
    }
}
