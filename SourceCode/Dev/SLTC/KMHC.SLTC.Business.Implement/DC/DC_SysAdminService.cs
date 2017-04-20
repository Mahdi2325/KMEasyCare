/*************************************************************************************************
 * 描述:日间照顾-社工
 * 创建日期:2016-5-9
 * 创建人:杨金高
 * **********************************************************************************************/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Implement.Other;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Data;
using System.Text;

namespace KMHC.SLTC.Business.Implement
{
    public class DC_SysAdminService : BaseService, IDC_SysAdminService
    {
        
        /// <summary>
        /// 获取活动数据
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>List</returns>
       public BaseResponse<IList<DC_TeamActivitydtlModel>> QueryTeamActivitydtl(BaseRequest<DC_TeamActivitydtlFilter> request)
        {
            BaseResponse<IList<DC_TeamActivitydtlModel>> response = new BaseResponse<IList<DC_TeamActivitydtlModel>>();

            #region 新增活动类型
            var act = unitOfWork.GetRepository<DC_TEAMACTIVITY>().dbSet.Count(o => o.ORGID == request.Data.ORGID);
            if (act <= 0)
            { 
                //创建小组化活动A
                DC_TEAMACTIVITY actinfoA = new DC_TEAMACTIVITY();
                actinfoA.ACTIVITYCODE = "A";
                actinfoA.ACTIVITYNAME = "小组化活动";
                actinfoA.ORGID = request.Data.ORGID;
                unitOfWork.GetRepository<DC_TEAMACTIVITY>().Insert(actinfoA);

                //创建小组化活动
                DC_TEAMACTIVITY actinfoB = new DC_TEAMACTIVITY();
                actinfoB.ACTIVITYCODE = "B";
                actinfoB.ACTIVITYNAME = "个别化活动";
                actinfoB.ORGID = request.Data.ORGID;
                unitOfWork.GetRepository<DC_TEAMACTIVITY>().Insert(actinfoB);

                unitOfWork.Save();
            }
            #endregion


            var q = from TEAMACTIVITY in unitOfWork.GetRepository<DC_TEAMACTIVITY>().dbSet
                    join TEAMACTIVITYDTL in unitOfWork.GetRepository<DC_TEAMACTIVITYDTL>().dbSet on TEAMACTIVITY.SEQNO equals TEAMACTIVITYDTL.SEQNO into itemdtl
                    from ns_d in itemdtl.DefaultIfEmpty() 
                    where (TEAMACTIVITY.ORGID ==  request.Data.ORGID)
                    select new DC_TeamActivitydtlModel
                    {
                        ID = ns_d.ID ,
                        SEQNO = ns_d.SEQNO,
                        TITLENAME = ns_d.TITLENAME,
                        ACTIVITYCODE = TEAMACTIVITY.ACTIVITYCODE,
                        ITEMNAME = ns_d.ITEMNAME,
                    };

            if (!(request.Data.SEQNO == null) && !(request.Data.SEQNO <= 0))
            {
                q = q.Where(m => m.SEQNO == request.Data.SEQNO);
            }

            if (!string.IsNullOrEmpty(request.Data.ACTIVITYCODE))
            {
                q = q.Where(m => m.ACTIVITYCODE == request.Data.ACTIVITYCODE);
            }

            if (!string.IsNullOrEmpty(request.Data.TITLENAME))
            {
                q = q.Where(m => m.TITLENAME.Contains(request.Data.TITLENAME));
            }

            q = q.OrderByDescending(m => m.SEQNO);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
     
            response.Data = q.ToList();

            return response;
           
        }

        //保存数据字典主表
       public BaseResponse<DC_TeamActivitydtlModel> SaveTeamActivitydtl(DC_TeamActivitydtlModel request)
       {


           //---

           BaseResponse<DC_TeamActivitydtlModel> responsePerson = new BaseResponse<DC_TeamActivitydtlModel>();

           Mapper.CreateMap<DC_TeamActivitydtlModel, DC_TEAMACTIVITYDTL>();

           var model = unitOfWork.GetRepository<DC_TEAMACTIVITYDTL>().dbSet.Where(m => m.ID == request.ID).FirstOrDefault();
           if (model == null)
           {
               var act = unitOfWork.GetRepository<DC_TEAMACTIVITY>().dbSet.Where(m=>m.ORGID== request.ORGID && m.ACTIVITYCODE == request.ACTIVITYCODE).ToList().FirstOrDefault().SEQNO;
               request.SEQNO = act;
               model = Mapper.Map<DC_TEAMACTIVITYDTL>(request);

               unitOfWork.GetRepository<DC_TEAMACTIVITYDTL>().Insert(model);
               unitOfWork.Save();
               
           }
           else
           {

               var act = unitOfWork.GetRepository<DC_TEAMACTIVITY>().dbSet.Where(m => m.ORGID == request.ORGID && m.ACTIVITYCODE == request.ACTIVITYCODE).ToList().FirstOrDefault().SEQNO;
               request.SEQNO = act;

               Mapper.Map(request, model);

               unitOfWork.GetRepository<DC_TEAMACTIVITYDTL>().Update(model);

                

           }
           unitOfWork.Save();

           return responsePerson;
           //--

            

           //unitOfWork.BeginTransaction();
           //responsePerson = base.Save<DC_TEAMACTIVITYDTL, DC_TeamActivitydtlModel>(request, (q) => q.ID == request.ID);
          
           //unitOfWork.Commit();
           //return responsePerson;
       }
       /// <summary>
       /// 删除住民信息
       /// </summary>
       /// <param name="regNo"></param>
       public BaseResponse DeleteTeamActivitydtl(int ID)
       {
           unitOfWork.BeginTransaction();
           //var item = unitOfWork.GetRepository<DC_TEAMACTIVITYDTL>().dbSet.Where(m => m.ID == ID).FirstOrDefault();
           //if (item != null)
           var response = base.Delete<DC_TEAMACTIVITYDTL>(ID); 
           unitOfWork.Commit();
           return response;
       }
    
       /// <summary>
       /// 取字典主列表
       /// </summary>
       /// <param name="regNo"></param>
       public BaseResponse<IList<DC_COMMFILEModel>> QueryDCcommfile(BaseRequest<DC_COMMFILEFilter> request)
       {
           BaseResponse<IList<DC_COMMFILEModel>> response = new BaseResponse<IList<DC_COMMFILEModel>>();
           IQueryable<DC_COMMFILEModel> q;
           //子项名称和编号
          // string orgidstr = SecurityHelper.CurrentPrincipal.OrgId;
           if (!string.IsNullOrEmpty(request.Data.SUBITEMNAME))
           {
               var qsub = from DCCOMMDTL in unitOfWork.GetRepository<DC_COMMDTL>().dbSet
                          where ((DCCOMMDTL.ITEMNAME.Contains(request.Data.SUBITEMNAME) || DCCOMMDTL.ITEMCODE.Contains(request.Data.SUBITEMNAME)))
                          select new
                          {
                              ITEMTYPE = DCCOMMDTL.ITEMTYPE,

                          };

               var arritem = qsub.ToArray();
               string querystr = "";
               for (int i = 0; i < arritem.Length; i++)
               {   
                   if (querystr.Length==0)
                   { querystr += "'" + arritem[i].ITEMTYPE + "'"; }
                   else
                   { querystr += ",'" + arritem[i].ITEMTYPE + "'"; }
                   
               }

               if (querystr.Length > 0)
               {
                   q = from DCCOMMFILE in unitOfWork.GetRepository<DC_COMMFILE>().dbSet
                       where (querystr.Contains(DCCOMMFILE.ITEMTYPE) && DCCOMMFILE.ORGID.Equals(request.Data.ORGID))
                       select new DC_COMMFILEModel
                       {
                           ITEMTYPE = DCCOMMFILE.ITEMTYPE,
                           TYPENAME = DCCOMMFILE.TYPENAME,
                           DESCRIPTION = DCCOMMFILE.DESCRIPTION,
                           ORGID = DCCOMMFILE.ORGID,
                           MODIFYFLAG = DCCOMMFILE.MODIFYFLAG

                       };
               }
               else
               {
                   q = from DCCOMMFILE in unitOfWork.GetRepository<DC_COMMFILE>().dbSet
                       where (DCCOMMFILE.ORGID.Equals(request.Data.ORGID))
                       select new DC_COMMFILEModel
                       {
                           ITEMTYPE = DCCOMMFILE.ITEMTYPE,
                           TYPENAME = DCCOMMFILE.TYPENAME,
                           DESCRIPTION = DCCOMMFILE.DESCRIPTION,
                           ORGID = DCCOMMFILE.ORGID,
                           MODIFYFLAG = DCCOMMFILE.MODIFYFLAG

                       };
               }

           }
           else
           {
               q = from DCCOMMFILE in unitOfWork.GetRepository<DC_COMMFILE>().dbSet
                   where (DCCOMMFILE.ORGID.Equals(request.Data.ORGID))
                   select new DC_COMMFILEModel
                   {
                       ITEMTYPE = DCCOMMFILE.ITEMTYPE,
                       TYPENAME = DCCOMMFILE.TYPENAME,
                       DESCRIPTION = DCCOMMFILE.DESCRIPTION,
                       ORGID = DCCOMMFILE.ORGID,
                       MODIFYFLAG = DCCOMMFILE.MODIFYFLAG

                   };

           }
           
           //q = q.Where(m => m.ORGID.Equals(orgidstr));
           //字典编号和名称
           if (!string.IsNullOrEmpty(request.Data.TYPENAME))
           { q = q.Where(m => m.ITEMTYPE.Contains(request.Data.TYPENAME) || m.TYPENAME.Contains(request.Data.TYPENAME));  }
           
           //字典状态
           if (!string.IsNullOrEmpty(request.Data.MODIFYFLAG))
           { q = q.Where(m => m.MODIFYFLAG.Equals(request.Data.MODIFYFLAG) ); }

            

           q = q.OrderByDescending(m => m.ITEMTYPE);
           response.RecordsCount = q.Count();
           if (request != null && request.PageSize > 0)
           {
               response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
               response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
           }
           else
           {
               response.Data = q.ToList();
           }

           

           return response;

       }

    //取某条数据字典下的全部子项
      public  BaseResponse<IList<DC_COMMDTLModel>> QueryDCCOMMDTL(string ITEMTYPE)
       {
           BaseResponse<IList<DC_COMMDTLModel>> response = new BaseResponse<IList<DC_COMMDTLModel>>();

           var q = from DC_COMMDTL in unitOfWork.GetRepository<DC_COMMDTL>().dbSet
                   select new DC_COMMDTLModel
                   {
                       ITEMCODE = DC_COMMDTL.ITEMCODE,
                       ITEMNAME = DC_COMMDTL.ITEMNAME,
                       ITEMTYPE = DC_COMMDTL.ITEMTYPE,
                       DESCRIPTION = DC_COMMDTL.DESCRIPTION,
                        ORDERSEQ  =DC_COMMDTL.ORDERSEQ,
                        UPDATEDATE = DateTime.Now,
                        UPDATEBY = DC_COMMDTL.UPDATEBY
                   };

           if (!string.IsNullOrEmpty(ITEMTYPE))
           {

               q = q.Where(m => m.ITEMTYPE.Equals(ITEMTYPE));


           }
           q = q.OrderBy(m => m.ITEMCODE);
           response.RecordsCount = q.Count();
            
            response.Data = q.ToList();
      
           return response;
       }

       //取某条数据字典
       public BaseResponse<DC_COMMFILEModel> GetDCcommfile(string id)
       {

           return base.Get<DC_COMMFILE, DC_COMMFILEModel>((q) => q.ITEMTYPE == id);

       }

       //删除某条数据字典及子项
       public BaseResponse DeleteDCcommfile(string id)
       {
           BaseResponse response=null;
           unitOfWork.BeginTransaction();
           var item = unitOfWork.GetRepository<DC_COMMFILE>().dbSet.Where(m => m.ITEMTYPE == id).FirstOrDefault();
           if (item != null)
           {
               base.Delete<DC_COMMDTL>(m => m.ITEMTYPE == id);//子项
               response = base.Delete<DC_COMMFILE>(id);//主表数据字典
              
           }
           unitOfWork.Commit();
           return response;
            
       }
       //删除某条数据子项
       public BaseResponse DeleteDCcOMMDTL(string type, string code)
       {
           BaseResponse response = null;
           unitOfWork.BeginTransaction();
           var item = unitOfWork.GetRepository<DC_COMMDTL>().dbSet.Where(m => (m.ITEMTYPE == type && m.ITEMCODE==code)).FirstOrDefault();
           if (item != null)
           {
                base.Delete<DC_COMMDTL>(m => m.ITEMTYPE == type && m.ITEMCODE == code);//子项 
                 
           }
           unitOfWork.Commit();
           return response;
       }

        //保存数据字典
       public BaseResponse<DC_COMMFILEModel> SaveDCCOMMFILE(DC_COMMFILEModel request)
       {
           unitOfWork.BeginTransaction();
           request.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
           var responsePerson = base.Save<DC_COMMFILE, DC_COMMFILEModel>(request, (q) => q.ITEMTYPE == request.ITEMTYPE);
          
           unitOfWork.Commit();
           return responsePerson;
       }

       //保存数据字典子项
       public BaseResponse<DC_COMMDTLModel> SaveDCCOMMDtl(DC_COMMDTLModel request)
       {
           unitOfWork.BeginTransaction();

           var responsePerson = base.Save<DC_COMMDTL, DC_COMMDTLModel>(request, (q) => q.ITEMTYPE == request.ITEMTYPE);

           unitOfWork.Commit();
           return responsePerson;
       }
    }
}

