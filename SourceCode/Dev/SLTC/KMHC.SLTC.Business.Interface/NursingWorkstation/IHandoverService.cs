using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.NursingWorkstation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IHandoverService : IBaseService
    {
        #region LTC_WORKITEM
        /// <summary>
        /// ItemType H:行政交班 L:巡视记录
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        BaseResponse<IList<WorkItemModel>> GetWorkItemByType(string itemType);
        #endregion

        #region LTC_HandOver

        BaseResponse<HandoverRecord> GetHandoverRecordByDate(DateTime handoverDate);

        #endregion

       

        #region LTC_HandRecordDtl

        BaseResponse<LTC_HandoverDtl> SaveHandoverDtl(LTC_HandoverDtl handoverRecord);
        #endregion

    }
}
