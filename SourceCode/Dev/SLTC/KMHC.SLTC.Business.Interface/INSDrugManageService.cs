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
    public interface INSDrugManageService : IBaseService
    {
        /// <summary>
        /// 查询机构药品
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>药品数据信息</returns>
        BaseResponse<IList<NSDrug>> Query(BaseRequest<NSDrugFilter> request);
        /// <summary>
        /// 获取NSDrug
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NSDrug> Get(int id);
        /// <summary>
        /// 获取NSDrug
        /// </summary>
        /// <param name="mcDrugCode"></param>
        /// <returns></returns>
        BaseResponse<NSDrug> GetByMCDrugCode(string mcDrugCode);
        /// <summary>
        /// 保存NSDrug
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<NSDrug> Save(NSDrug request);
        /// <summary>
        /// 删除NSDrug
        /// </summary>
        /// <param name="id"></param>
        BaseResponse Delete(int id);
    }
}
