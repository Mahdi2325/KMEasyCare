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
    public interface INCIMedicalMaterialService : IBaseService
    {
        /// <summary>
        /// 查询护理险耗材
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>耗材数据信息</returns>
        BaseResponse<IList<NCIMedicalMaterial>> Query(BaseRequest<NCIMedicalMaterialFilter> request);
        /// <summary>
        /// 获取NCIMedicalMaterial
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NCIMedicalMaterial> Get(string medicalMaterialCode);
    }
}
