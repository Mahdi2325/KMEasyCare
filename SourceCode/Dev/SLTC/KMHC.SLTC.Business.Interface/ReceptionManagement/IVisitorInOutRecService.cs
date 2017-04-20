/*
创建人: 
创建日期:
说明:
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace KMHC.SLTC.Business.Interface
{
    public interface IVisitorInOutRecService : IBaseService
    {
        /// <summary>
        /// 获取来宾出入记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<VisitorInOut>> QueryVisitorInOutList(BaseRequest<VisitorInOutFilter> request);
        /// 删除来宾出入的信息
        /// </summary>
        /// <param name="regNo"></param>
        BaseResponse DeleteVisitorInOutByID(int visitRecId);
        /// 保存来宾出入的信息
        /// </summary>
        /// <param name="regNo"></param>
        BaseResponse<VisitorInOut> SaveVisitorInOut(VisitorInOut request); 
        
    }
}
