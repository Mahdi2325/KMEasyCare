using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface INSServiceManageService : IBaseService
    {
        /// <summary>
        /// 查询机构服务项目
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>服务项目数据信息</returns>
        BaseResponse<IList<NSService>> Query(BaseRequest<NSServiceFilter> request);
        /// <summary>
        /// 获取NSService
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NSService> Get(int id);
        /// <summary>
        /// 获取NSService
        /// </summary>
        /// <param name="mcServiceCode"></param>
        /// <returns></returns>
        BaseResponse<NSService> GetByMCServiceCode(string mcServiceCode);
        /// <summary>
        /// 保存NSService
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<NSService> Save(NSService request);
        /// <summary>
        /// 删除NSService
        /// </summary>
        /// <param name="id"></param>
        BaseResponse Delete(int id);
    }
}
