using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC.Report
{
    public interface IDC_CrossReportService
    {
        #region 护理及生活照顾服务记录表

        BaseResponse<NurseingLife3> GetNurseCareById(int id);

         string GetNurseCareBycode(string CodeId);

        #endregion

        #region 日常生活照顾记录表

         BaseResponse<DayLife2> GetDayLifeById(int id);

        #endregion

        //GetAb
        #region 问题行为

         BaseResponse<AbNormaleMotionRec> GetAb(long feeNo,int year,int month);

        #endregion




    }
}

