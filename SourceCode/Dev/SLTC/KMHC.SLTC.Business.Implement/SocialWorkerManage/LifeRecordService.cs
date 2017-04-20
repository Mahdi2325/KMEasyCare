/*
 创建人：杨金高
 创建日期:2016-03-09
 描述:生活记录
 */
using AutoMapper;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Implement.Base;
using KMHC.SLTC.Business.Interface.SocialWorkerManage;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.SocialWorkerManage
{
    public class LifeRecordService:BaseService,ILifeRecordService
    {
        /// <summary>
        /// 获取生活记录列表　
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<LifeRecordModel>> QueryLifeRecord(BaseRequest<LifeRecordFilter> request)
        {
            var response = base.Query<LTC_LIFERECORDS, LifeRecordModel>(request, (q) => {
                if (!string.IsNullOrEmpty(request.Data.Comments))
                {
                    q = q.Where(m => m.COMMENTS == request.Data.Comments);
                }
                q = q.OrderBy(m => m.ORGID);
                return q;
            });
            return response;
        }
        /// <summary>
        /// 获取生活记录
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<LifeRecordModel> GetLifeRecordById(int regNo)
        {
            return base.Get<LTC_LIFERECORDS, LifeRecordModel>((q) => q.REGNO == regNo);
        }

        /// <summary>
        /// 删除生活记录
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeleteLifeRecordById(int regNo)
        {
            return base.Delete<LTC_LIFERECORDS>(regNo);
        }

        /// <summary>
        /// 保存生活记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<LifeRecordModel> SaveLifeRecord(LifeRecordModel request)
        {
            return base.Save<LTC_LIFERECORDS, LifeRecordModel>(request, (q) => q.REGNO == request.RegNo);
        }


        public BaseResponse<LifeRecordModel> GetLifeRecordById(long id)
        {
            throw new NotImplementedException();
        }

        public BaseResponse DeleteLifeRecordById(long id)
        {
            throw new NotImplementedException();
        }
    }
}
