using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface
{
    public interface IBaseService
    {
        /// <summary>
        /// 生成编码
        /// </summary>
        /// <param name="orgId">组织结构ID</param>
        /// <param name="key">Key</param>
        /// <returns>编码</returns>
        string GenerateCode(string orgId, EnumCodeKey key);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //BaseResponse<IList<T>> Query(BaseRequest<F> request);
        /// <summary>
        /// 获取单一对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //BaseResponse<T> Get(object id);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request"></param>
        //BaseResponse<T> Save(T request);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        //BaseResponse Delete(object id);
    }
}

