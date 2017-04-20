/*
创建人: 肖国栋
创建日期:2016-03-09
说明:住民管理
*/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Implement.Other;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KM.Common;

namespace KMHC.SLTC.Business.Implement
{
    public class ResBirthDayListService : BaseService, IResBirthDayListService
    {
        public BaseResponse<IList<Person>> QueryResBirthDayList(DateTime sDate, DateTime eDate, string keyWord, int CurrentPage, int PageSize)
        {
            BaseResponse<IList<Person>> response = new BaseResponse<IList<Person>>();
            Mapper.CreateMap<LTC_REGFILE, Person>();
            var q = from m in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                    join p in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on m.REGNO equals p.REGNO where p.IPDFLAG=="I"
                    select m;
            q = q.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
            q = q.Where(m => m.BRITHDATE.Value.Month >= sDate.Month && m.BRITHDATE.Value.Day >= sDate.Day
                && m.BRITHDATE.Value.Month <= eDate.Month && m.BRITHDATE.Value.Day <= eDate.Day);
            if (!string.IsNullOrEmpty(keyWord))
            {
                q = q.Where(m => m.NAME.Contains(keyWord) || m.RESIDENGNO.Contains(keyWord));
            }
            q = q.OrderByDescending(m => m.BRITHDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<Person>();
                foreach (dynamic item in list)
                {
                    Person newItem = Mapper.DynamicMap<Person>(item);
                    response.Data.Add(newItem);
                }
            };
            if (PageSize > 0)
            {
                var list = q.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }
    }
}
