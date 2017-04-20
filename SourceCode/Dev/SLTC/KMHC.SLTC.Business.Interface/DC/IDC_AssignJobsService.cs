using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC
{
    public interface IDC_AssignJobsService
    {
        #region 工作提醒
        BaseResponse<IList<DC_TaskRemind>> QueryTaskList(BaseRequest<DC_AssignJobsFilter> request);
        object QueryAssTask(BaseRequest<AssignTaskJobFilter> request);
        BaseResponse ChangeRecStatus(int id, bool? recStatus, DateTime? finishDate);
        BaseResponse<DC_TaskRemind> SaveTaskRemind(DC_TaskRemind baseRequest);

        BaseResponse<IList<DC_TaskEmpModel>> QueryEmpList();

        BaseResponse SaveAllocateTask(long ID, IList<DC_TaskEmpModel> empList);
        
        #endregion

        #region 全局变量
        BaseResponse<GlobalVariable> GetGlobalVariable();
        #endregion

        #region 工作照会
        BaseResponse SaveAssignWorkNote(DC_ReAllocateTaskModel list);
        #endregion

    }
}
