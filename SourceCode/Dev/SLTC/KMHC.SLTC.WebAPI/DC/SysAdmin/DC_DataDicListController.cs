using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;

namespace KMHC.SLTC.WebAPI.DC.SysAdmin
{
    [RoutePrefix("api/DCDataDicList")]
    public class DC_DataDicListController : BaseController
    {
        IDC_SysAdminService service = IOCContainer.Instance.Resolve<IDC_SysAdminService>();
        IOrganizationManageService orgserver = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord, string keyword2, string MODIFYFLAG,string orgid, int currentPage, int pageSize)
        {
            BaseRequest<DC_COMMFILEFilter> request = new BaseRequest<DC_COMMFILEFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;

            //字典名称和编号
            if (!string.IsNullOrEmpty(keyWord))
            {
                request.Data.ITEMTYPE = keyWord;
                request.Data.TYPENAME = keyWord;
            }
            //字典子项名称和编号
            if (!string.IsNullOrEmpty(keyword2))
            {
                request.Data.SUBITEMNAME = keyword2;
            }
            //字典状态
            if (!string.IsNullOrEmpty(MODIFYFLAG))
            {
                request.Data.MODIFYFLAG = MODIFYFLAG;

            }

            //if (!string.IsNullOrEmpty(orgid))
            //{ request.Data.ORGID = orgid; }
            //else
            //{
            //    request.Data.ORGID = SecurityHelper.CurrentPrincipal.OrgId;//默认查当前用户所在的组织
            //}
            var orginfo = orgserver.GetOrg(request.Data.ORGID);

            var responseByQuery = service.QueryDCcommfile(request);
            var response = new BaseResponse<List<object>>(new List<object>());
            response.CurrentPage = responseByQuery.CurrentPage;
            response.PagesCount = responseByQuery.PagesCount;
            response.RecordsCount = responseByQuery.RecordsCount;
            Array.ForEach(responseByQuery.Data.ToArray(), (item) =>
            {
                response.Data.Add(new
                {
                    ITEMTYPE = item.ITEMTYPE,
                    TYPENAME = item.TYPENAME,
                    MODIFYFLAG = item.MODIFYFLAG,
                    DESCRIPTION = item.DESCRIPTION,
                    ORGID = orginfo.Data.OrgName  //SecurityHelper.CurrentPrincipal.UserId + SecurityHelper.CurrentPrincipal.LoginName
                });

            });//item.ORGID +
            return Ok(response);

        }


        [Route(""), HttpGet]
        public IHttpActionResult Get(string flag, int staus,string datatyp)
        {
            BaseRequest<OrganizationFilter> request = new BaseRequest<OrganizationFilter>
            {
                CurrentPage = 1,
                PageSize = 100,
                Data = { OrgName = "" }
            };

            var responseObject = new BaseResponse<List<object>>(new List<object>());
            if (datatyp == "1")//返回机构下拉数据 
            {
                var response = orgserver.QueryOrg(request);
                
                //responseObject.Data.Add(new
                //{
                //    orgid = "",
                //    orgname = "--请选择--",

                //});
                Array.ForEach(response.Data.ToArray(), (item) =>
                {
                    responseObject.Data.Add(new
                    {
                        orgid = item.OrgId,
                        orgname = item.OrgName,

                    });

                });

                return Ok(responseObject);
            }


            if (datatyp == "2")//返回当前用户
            {

                var responseRole = new BaseRequest<RoleFilter>();
                 var Roletypestr="";
                var RoleIdstr="";
                if (SecurityHelper.CurrentPrincipal.RoleType.Count()>1)
                { 
                    Roletypestr =SecurityHelper.CurrentPrincipal.RoleType[0];
                    RoleIdstr = SecurityHelper.CurrentPrincipal.RoleId[0];
                 
                }
                    
                responseObject.Data.Add(new
                {   
                    Roletype =Roletypestr,
                    RoleId = RoleIdstr,
                    userid = SecurityHelper.CurrentPrincipal.UserId,
                    username =SecurityHelper.CurrentPrincipal.LoginName,

                });
                 
                
                return Ok(responseObject);
            }

            if (datatyp == "3")//返回当前组织
            {
                var orginfo = orgserver.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
                var response = orgserver.QueryOrg(request);
                
                responseObject.Data.Add(new
                {
                    orgid = SecurityHelper.CurrentPrincipal.OrgId,
                    orgname = orginfo.Data.OrgName,

                });

                
            }
            return Ok(responseObject);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(string ITEMTYPE)
        {

            var response = service.GetDCcommfile(ITEMTYPE);

            return Ok(response);


        }

         

        // DELETE api/syteminfo/5
        [Route("{id}")]
        public IHttpActionResult DELETE(string id)
        {
            var response = service.DeleteDCcommfile(id);

            return Ok(response);


        }

         
    }
}
