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
    public interface INSMedicalMaterialManageService : IBaseService
    {
        /// <summary>
        /// 查询机构耗材
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>耗材数据信息</returns>
        BaseResponse<IList<NSMedicalMaterial>> Query(BaseRequest<NSMedicalMaterialFilter> request);
        /// <summary>
        /// 获取NSMedicalMaterial
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NSMedicalMaterial> Get(int id);
        /// <summary>
        /// 获取NSMedicalMaterial
        /// </summary>
        /// <param name="mcMaterialCode"></param>
        /// <returns></returns>
        BaseResponse<NSMedicalMaterial> GetByMCMedicalMaterialCode(string mcMaterialCode);
        /// <summary>
        /// 保存NSMedicalMaterial
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<NSMedicalMaterial> Save(NSMedicalMaterial request);
        /// <summary>
        /// 删除NSMedicalMaterial
        /// </summary>
        /// <param name="id"></param>
        BaseResponse Delete(int id);
    }
}
