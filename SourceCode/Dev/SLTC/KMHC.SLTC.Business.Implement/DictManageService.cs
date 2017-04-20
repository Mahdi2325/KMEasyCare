/*
 * 描述:User
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 2/18/2016 12:02:29 PM   Admin            15986707042@163.com    创建 
 *  
 */

using System;
using AutoMapper;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KMHC.Infrastructure;

namespace KMHC.SLTC.Business.Implement
{
    public class DictManageService : BaseService, IDictManageService
    {
        #region 字典
        public BaseResponse<IList<CodeValue>> QueryCode(BaseRequest<CodeFilter> request)
        {
            Mapper.CreateMap<LTC_CODEDTL_REF, CodeValue>();
            var response = new BaseResponse<IList<CodeValue>>();

            var q = from m in unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet
                    select m;
            List<LTC_CODEDTL_REF> list = null;
            if (!string.IsNullOrEmpty(request.Data.ItemType))
            {
                q = q.Where(o => o.ITEMTYPE == request.Data.ItemType);
                response.RecordsCount = q.Count();
                if (request != null && request.PageSize > 0)
                {
                    list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    list = q.ToList();
                }
                response.Data = Mapper.Map<IList<CodeValue>>(list);
            }
            else if (request.Data.ItemTypes != null)
            {
                q = q.Where(o => request.Data.ItemTypes.Contains(o.ITEMTYPE));
                response.RecordsCount = q.Count();
                if (request != null && request.PageSize > 0)
                {
                    list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    list = q.ToList();
                }
                response.Data = Mapper.Map<IList<CodeValue>>(list);
            }
            return response;
        }

        public BaseResponse<CodeValue> GetCode(string id, string pId)
        {
            return base.Get<LTC_CODEDTL_REF, CodeValue>((q) => q.ITEMCODE == id && q.ITEMTYPE == pId);
        }

        public BaseResponse<CodeValue> SaveCode(CodeValue request)
        {
            return base.Save<LTC_CODEDTL_REF, CodeValue>(request, (q) => q.ITEMCODE == request.ItemCode && q.ITEMTYPE==request.ItemType);
        }

        public int DeleteCode(string id, string pId)
        {
            Expression<Func<LTC_CODEDTL_REF, bool>> filter = p => p.ITEMCODE == id && p.ITEMTYPE == pId;
            return base.Delete<LTC_CODEDTL_REF>(filter);
        }

        public BaseResponse<IList<CodeValue>> QueryCode(CodeFilter request)
        {
            Mapper.CreateMap<LTC_CODEDTL_REF, CodeValue>();
                //.ForMember(d=>d.Name, opt=>opt.MapFrom(s=>s.ITEMNAME))
                //.ForMember(d=>d.Value, opt=>opt.MapFrom(s=>s.ITEMCODE))
                //.ForMember(d=>d.Parent, opt=>opt.MapFrom(s=>s.ITEMTYPE));
            var response = new BaseResponse<IList<CodeValue>>();
            var q = unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Select(m => m);
            if (!string.IsNullOrEmpty(request.ItemType))
            {
                q = q.Where(o => o.ITEMTYPE == request.ItemType);
            }
            else if (request.ItemTypes != null)
            {
                q = q.Where(o => request.ItemTypes.Contains(o.ITEMTYPE));
            }
            response.Data = Mapper.Map<IList<CodeValue>>(q.ToList());
            return response;
        }
        #endregion

        public BaseResponse<IList<CommonUseWord>> QueryCommonUseWord(CommonUseWordFilter request)
        {
            Mapper.CreateMap<LTC_COMMFILE, CommonUseWord>();
            var response = new BaseResponse<IList<CommonUseWord>>();
            var q = unitOfWork.GetRepository<LTC_COMMFILE>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).Select(m => m);
            if (!string.IsNullOrEmpty(request.TypeName))
            {
                q = q.Where(o => o.TYPENAME.Trim() == request.TypeName);
            }
            else if (request.TypeNames != null)
            {
                q = q.Where(o => request.TypeNames.Contains(o.TYPENAME.Trim()));
            }
            var queryList = q.ToList();
            queryList.ForEach(m =>
                {
                    m.ITEMNAME = m.ITEMNAME.Trim();
                    m.TYPENAME = m.TYPENAME.Trim();
                });
            response.Data = Mapper.Map<IList<CommonUseWord>>(queryList);
            return response;
        }

        #region NCI_FINANCIALMONTH 

        public LTC_NCIFinancialMonth GetFeeIntervalByMonth(string month)
        {
           try
           {
               Mapper.CreateMap<LTC_NCIFINANCIALMONTH, LTC_NCIFinancialMonth>();
               LTC_NCIFINANCIALMONTH nciFinancialMonth = unitOfWork.GetRepository<LTC_NCIFINANCIALMONTH>().dbSet.Where(x => x.GOVID == SecurityHelper.CurrentPrincipal.GovId && x.MONTH == month).FirstOrDefault();
               LTC_NCIFinancialMonth monthInfo = Mapper.Map<LTC_NCIFinancialMonth>(nciFinancialMonth);
               monthInfo.IntervalDays = DictHelper.CalcMonthIntervalDay(monthInfo);
               return monthInfo;
           }
           catch(Exception ex)
           {
               throw new Exception(ex.ToString());
           }
        
        }

        #endregion
        public  LTC_NCIFinancialMonth GetFeeIntervalByDate(string date)
        {
            return DictHelper.GetFeeIntervalByDate(date);
        }
        public LTC_NCIFinancialMonth GetFeeIntervalByDate(DateTime d)
        {
            return DictHelper.GetFeeIntervalByDate(d);
        }
        public LTC_NCIFinancialMonth GetFeeIntervalByYearMonth(string yearMonth)
        {
            return DictHelper.GetFeeIntervalByYearMonth(yearMonth);
        }
        public DateTime GetFeeIntervalEndDateByDate(DateTime d)
        {
            return DictHelper.GetFeeIntervalEndDateByDate(d);
        }
    }
}