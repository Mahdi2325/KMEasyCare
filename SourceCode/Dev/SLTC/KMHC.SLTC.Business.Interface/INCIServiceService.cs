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
    public interface INCIServiceService : IBaseService
    {
        /// <summary>
        /// 查询护理险服务项目
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>服务项目数据信息</returns>
        BaseResponse<IList<NCIService>> Query(BaseRequest<NCIServiceFilter> request);
        /// <summary>
        /// 获取NCIService
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NCIService> Get(string serviceCode);
    }
}
