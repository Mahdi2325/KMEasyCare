/****************************************
 * Author:	Dennis Yang
 * Create Date: 2016-03-16
 * Modifier:
 * Modify Date:
 * Description:Pharmacist(多重用药指标)
 *****************************************/
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
    public interface IPharmacistManageService:IBaseService
    {
        /// <summary>
        /// 获取多重用药指标列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Pharmacist>> QueryPharmacist(PharmacistFilter request);

        /// <summary>
        /// 根据住民RegNo获取单条用药指标
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<IList<Pharmacist>> GetPharmacistByRegNo(int regNo);

        /// <summary>
        /// 删除一条用药指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeletePharmacist(int id);

        /// <summary>
        /// 保存用药指标
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<Pharmacist> SavePharmacist(Pharmacist request);

    }
}
