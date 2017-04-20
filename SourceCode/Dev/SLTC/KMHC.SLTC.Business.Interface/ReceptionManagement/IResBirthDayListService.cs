/*
创建人: 肖国栋
创建日期:2016-03-09
说明:住民管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System;

namespace KMHC.SLTC.Business.Interface
{
    public interface IResBirthDayListService
    {
        /// <summary>
        /// 查询住民的生日
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>离院记录</returns>
        BaseResponse<IList<Person>> QueryResBirthDayList(DateTime sDate, DateTime eDate, string keyWord, int CurrentPage, int PageSize);
       
    }
}
