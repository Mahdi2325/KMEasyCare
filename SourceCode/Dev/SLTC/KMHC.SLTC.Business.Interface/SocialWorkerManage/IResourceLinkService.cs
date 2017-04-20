using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface.SocialWorkerManage
{
    public interface IResourceLinkService : IBaseService
    {
        /// <summary>
        /// 获取资源连接列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ResourceLinkModel>> QueryResourceLink(BaseRequest<ResourceLinkFilter> request);

        /// <summary>
        /// 获取单个资源信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<ResourceLinkModel> GetResourceLink(long id);

        /// <summary>
        /// 保存资源连接信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<ResourceLinkModel> SaveResourceLink(ResourceLinkModel request);

        /// <summary>
        /// 根据指定资源ID进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteResourceLink(long id);
    }
}

