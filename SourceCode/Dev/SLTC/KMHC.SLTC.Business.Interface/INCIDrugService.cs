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
    public interface INCIDrugService : IBaseService
    {
        /// <summary>
        /// 查询护理险药品
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>药品数据信息</returns>
        BaseResponse<IList<NCIDrug>> Query(BaseRequest<NCIDrugFilter> request);
        /// <summary>
        /// 获取NCIDrug
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NCIDrug> Get(string drugCode);
    }
}
