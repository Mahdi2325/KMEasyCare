using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model.MedicalWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IOwnDrugRecService
    {
        /// <summary>
        /// 查询自带药品记录
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>账单数据信息</returns>
        BaseResponse<IList<OwnDrugRecModel>> QueryOwnDrugRec(BaseRequest<OwnDrugRecFilter> request);

        /// <summary>
        /// 查询自带药品明细
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>账单数据信息</returns>
        BaseResponse<IList<OwnDrugDtlModel>> QueryOwnDrugDtl(BaseRequest<OwnDrugRecFilter> request);
        /// <summary>
        /// 保存自带药品记录
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<OwnDrugRecModel> SaveOwnDrugRec(OwnDrugRecModel request);
        /// <summary>
        /// 保存自带药品明细
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<List<OwnDrugDtlModel>> SaveOwnDrugDtl(OwnDrugDtlList request);
        /// <summary>
        /// 删除自带药品记录
        /// </summary>
        /// <param name="id"></param>
        BaseResponse Delete(int id);
    }
}
