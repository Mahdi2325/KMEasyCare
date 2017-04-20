using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class VisitorInOutRecService : BaseService, IVisitorInOutRecService
    {

        #region 查询来宾出入信息
        /// <summary>
        ///查询来宾出入信息
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <returns>Response</returns>
        public BaseResponse<IList<VisitorInOut>> QueryVisitorInOutList(BaseRequest<VisitorInOutFilter> request)
        {
            BaseResponse<IList<VisitorInOut>> response = new BaseResponse<IList<VisitorInOut>>();
            Mapper.CreateMap<LTC_FAMILYDISCUSSREC, VisitorInOut>();
            var q = ((from m in unitOfWork.GetRepository<LTC_FAMILYDISCUSSREC>().dbSet
                      where m.ISREGVISIT == true
                      join p in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on m.FEENO equals p.FEENO
                      //where p.IPDFLAG == "I"
                      select new VisitorInOut
                      {
                          Id = m.ID,
                          FeeNo = m.FEENO,
                          Interviewee = m.INTERVIEWEE,
                          RecordBy = m.RECORDBY,
                          RecordTime = m.RECORDTIME,
                          StartDate = m.STARTDATE,
                          EndDate = m.ENDDATE,
                          VisitType = m.VISITTYPE,
                          VisitorName = m.VISITORNAME,
                          VisitorSex = m.VISITORSEX,
                          VisitorIdNo = m.VISITORIDNO,
                          VisitorCompany = m.VISITORCOMPANY,
                          VisitorTel = m.VISITORTEL,
                          IsRegVisit = m.ISREGVISIT,
                          Appellation = m.APPELLATION,
                          BloodRelationShip = m.BLOODRELATIONSHIP,
                          Description = m.DESCRIPTION,
                          Remark = m.REMARK,
                          OrgId = m.ORGID,
                          IpdFlag = p.IPDFLAG
                      }).Concat(
                    from m in unitOfWork.GetRepository<LTC_FAMILYDISCUSSREC>().dbSet
                    where m.ISREGVISIT == false
                    select new VisitorInOut
                    {
                        Id = m.ID,
                        FeeNo = m.FEENO,
                        Interviewee = m.INTERVIEWEE,
                        RecordBy = m.RECORDBY,
                        RecordTime = m.RECORDTIME,
                        StartDate = m.STARTDATE,
                        EndDate = m.ENDDATE,
                        VisitType = m.VISITTYPE,
                        VisitorName = m.VISITORNAME,
                        VisitorSex = m.VISITORSEX,
                        VisitorIdNo = m.VISITORIDNO,
                        VisitorCompany = m.VISITORCOMPANY,
                        VisitorTel = m.VISITORTEL,
                        IsRegVisit = m.ISREGVISIT,
                        Appellation = m.APPELLATION,
                        BloodRelationShip = m.BLOODRELATIONSHIP,
                        Description = m.DESCRIPTION,
                        Remark = m.REMARK,
                        OrgId = m.ORGID,
                        IpdFlag = ""
                    }
                    ));
            request.Data.EndDate = request.Data.EndDate.Value.AddDays(1);
            q = q.Where(m => m.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
            q = q.Where(m => m.StartDate >= request.Data.StartDate && m.StartDate < request.Data.EndDate);
            if (!string.IsNullOrEmpty(request.Data.keyWord))
            {
                q = q.Where(m => m.VisitorName.Contains(request.Data.keyWord));
            }
            q = q.OrderByDescending(m => m.StartDate);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<VisitorInOut>();
                foreach (dynamic item in list)
                {
                    VisitorInOut newItem = Mapper.DynamicMap<VisitorInOut>(item);
                    response.Data.Add(newItem);
                }
            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        #endregion

        #region 根据ID删除来宾出入信息
        /// <summary>
        ///根据ID删除来宾出入信息
        /// </summary>
        public BaseResponse DeleteVisitorInOutByID(int visitRecId)
        {
            var response = new BaseResponse();
            unitOfWork.GetRepository<LTC_FAMILYDISCUSSREC>().Delete(m => m.ID == visitRecId);
            unitOfWork.Commit();
            return response;
        }
        #endregion

        #region 保存来宾出入的信息
        /// <summary>
        /// 保存来宾出入的信息
        /// </summary>
        public BaseResponse<VisitorInOut> SaveVisitorInOut(VisitorInOut request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId.ToString();
            request.IsRegVisit = false;
            var response = base.Save<LTC_FAMILYDISCUSSREC, VisitorInOut>(request, (q) => q.ID == request.Id);
            return response;
        }
        #endregion

    }
}
