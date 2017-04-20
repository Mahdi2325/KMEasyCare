using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KMHC.SLTC.Business.Interface
{
    public interface IDC_SysAdminService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_TeamActivitydtlModel>> QueryTeamActivitydtl(BaseRequest<DC_TeamActivitydtlFilter> request);
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<DC_TeamActivitydtlModel> SaveTeamActivitydtl(DC_TeamActivitydtlModel request);
        /// <summary>
        /// 删除住民信息
        /// </summary>
        /// <param name="regNo"></param>
        BaseResponse DeleteTeamActivitydtl(int ID);

        /// <summary>
        /// 取字典主列表
        /// </summary>
        /// <param name="regNo"></param>
        BaseResponse<IList<DC_COMMFILEModel>> QueryDCcommfile(BaseRequest<DC_COMMFILEFilter> request);
        BaseResponse<IList<DC_COMMDTLModel>> QueryDCCOMMDTL(string ITEMTYPE);
        
        BaseResponse<DC_COMMFILEModel>GetDCcommfile(string id);
        BaseResponse<DC_COMMFILEModel> SaveDCCOMMFILE(DC_COMMFILEModel request);
        BaseResponse<DC_COMMDTLModel> SaveDCCOMMDtl(DC_COMMDTLModel request);
        BaseResponse DeleteDCcommfile(string id);
        BaseResponse DeleteDCcOMMDTL(string type,string code);
        
 
    }
}
