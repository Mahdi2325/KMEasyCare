using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC
{
  public  interface IDC_CrossDayLife
  {
      #region 日常生活照顾记录

      BaseResponse SaveDayLife(DayLife request);

      BaseResponse<DayLife> QueryDayLife(int FeeNo, int year, int num);

      //历史记录

      BaseResponse<IList<DC_DayLifeCarerec>> QueryShowDayLife(int FEENO);

      BaseResponse<DayLife> QueryShowDayLifeList(string id);

      BaseResponse<DayLife> yearweek();

      BaseResponse DeleteDayLife(int id);

      BaseResponse<IList<DC_TeamActivityModel>> GetTeamAct(string code);

      BaseResponse<IList<DC_COMMDTLModel>> getCY();
      
      #endregion

      #region 护理及生活照顾记录服务记录表

      BaseResponse<NurseingLife> QueryNurseingLife(int FeeNo, int year, int num);


      BaseResponse SaveNurseingLife(NurseingLife1 request);

      BaseResponse<IList<DC_NurseingLifeCareREC>> QueryShowNurseingLife(int FeeNo);

      BaseResponse DeleteNuring(int id);

      BaseResponse<NurseingLife> QueryShowNurseList(string id);

      

      #endregion

      #region 问题行为与异常情绪记录表

      BaseResponse SaveAB(AbNormaleMotionRec request);

      BaseResponse<AbNormaleMotionRec> QueryAB(int FeeNo, int year, int month);


      BaseResponse<IList<ABFilter>> QueryHISAB(int FeeNo);

      BaseResponse DeleteAB(int regno,int year,int  month);
      


      #endregion

      #region 跨专业团队服务计划表


      BaseResponse<DC_SwRegEvalPlanModel> QuerySWREGEVALPLAN(int FEEONO);


      BaseResponse SaveSWREGEVALPLAN(DC_MultiteaMcarePlanEvalModel request);



      BaseResponse<IList<DC_MultiteaMcarePlanEvalModel>> QueryHisMultiteaMcarePlanEval(string REGNO);


      BaseResponse<DC_MultiteaMcarePlanEvalModel> QueryHisMultiteaMcare(int ID);

      BaseResponse DeleteultiteaMcare(int id);
      

      #endregion
       
      #region  跨专业团队服务计划表

      BaseResponse SaveMULTITEAM(MultiteamCarePlanRec request);



      BaseResponse<IList<DC_MultiteamCarePlanRecModel>> QueryHisMULTITEAM(string REGNO);

      BaseResponse<MultiteamCarePlanRec> QueryShowHis(int ID);


      BaseResponse DeleteMULTITEAM(int id);
      

      
      
      #endregion


  }
}

