using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KMHC.SLTC.Business.Interface
{
    public interface IDC_RegFileService
    {
        /// <summary>
        /// 获取个案基本资料列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_RegFileModel>> QueryPersonBasic(BaseRequest<DC_RegFileFilter> request);

        /// <summary>
        /// 获取个案基本资料
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<DC_RegFileModel> GetPersonBasicById(string id);

        /// <summary>
        /// 保存个案基本资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegFileModel> SavePersonBasic(DC_RegFileModel request);

        /// <summary>
        /// 删除个案基本资料
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse DeletePersonBasicById(string id);
    }
}

