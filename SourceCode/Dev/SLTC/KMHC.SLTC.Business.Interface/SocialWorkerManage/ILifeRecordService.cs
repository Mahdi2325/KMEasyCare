/*
 * 创建人:杨金高
 * 创建日期：2016-03-09
 * 描述:生活记录
 */
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.SocialWorkerManage
{
    public interface ILifeRecordService:IBaseService
    {
        /// <summary>
        /// 获取生活记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LifeRecordModel>> QueryLifeRecord(BaseRequest<LifeRecordFilter> request);

        /// <summary>
        /// 获取生活记录(指定id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<LifeRecordModel> GetLifeRecordById(int id);

        /// <summary>
        ///　删除生活记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteLifeRecordById(int id);

        /// <summary>
        /// 保存生活记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LifeRecordModel> SaveLifeRecord(LifeRecordModel request);
    }
}
