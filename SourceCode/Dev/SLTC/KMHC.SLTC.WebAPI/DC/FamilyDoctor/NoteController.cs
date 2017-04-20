using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.FamilyDoctor
{
    [RoutePrefix("api/Note")]
    public class NoteController : BaseController
    {
        IDC_FamilyDoctorService service = IOCContainer.Instance.Resolve<IDC_FamilyDoctorService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(string noteName, int currentPage, int pageSize, sbyte? isShow = null)
        {
            var filter = new DC_NoteFilter
            {
                OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                NoteName = noteName,
                IsShow = isShow
            };
            var request = new BaseRequest<DC_NoteFilter>
            {
                Data = filter
            };
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            var list = service.QueryNote(request);
            return Ok(list);
        }

        [Route(""), HttpPost]
        public IHttpActionResult Save(DC_NoteModel request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveNote(request, null);
            return Ok(response);
        }

        [Route("{noteId}")]
        public IHttpActionResult Delete(long noteId)
        {
            var response = service.DeleteNote(noteId);
            return Ok(response);
        }
    }
}
