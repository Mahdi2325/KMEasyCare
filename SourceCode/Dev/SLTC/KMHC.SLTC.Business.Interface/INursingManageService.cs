/*****************************************************************************
 * Creator:	Lei Chen
 * Create Date: 2016-03-08
 * Modifier:
 * Modify Date:
 * Description:针剂
 ******************************************************************************/
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
    public interface INursingManageService : IBaseService
    {
        #region 针剂
        BaseResponse<IList<Vaccineinject>> QueryInjection(NursingFilter request);


        BaseResponse<InjectionView> GetInjection(object id);

        BaseResponse<InjectionView> SaveInjection(InjectionView request);

        BaseResponse DeleteInjection(int id);


        BaseResponse<IList<InjectionView>> GetInjecttionByRegNo(int regNo);
        #endregion

        #region 评估量表
        BaseResponse<IList<EvaluationRecord>> QueryEvaluationList(NursingFilter request);
        BaseResponse<QUESTION> GetQuetion(int qId, long? regNo, long? recordId);
        BaseResponse<QUESTION> GetQuetionByCode(string Code);
        BaseResponse<REGQUESTION> GetREGQuetion(long recordId);
        BaseResponse SaveQuetion(REGQUESTION request);
        Calculation CalcEvaluation(QUESTION question);
        BaseResponse<REGQUESTION> GetLatestRegQuetion(long feeNo, int quetionId);

        BaseResponse<IList<REGQUESTION>> GetEvaluationHisOver(BaseRequest<NursingFilter> request);

        BaseResponse<object> GetEvalRecsForExtApi(BaseRequest<EvalRecFilter> request);

        BaseResponse DeleteEvaluation(long recordId);
        BaseResponse<CareDemandEval> SaveCareDemand(CareDemandEval request);
        BaseResponse<CareDemandEval> GetCareDemand(long demandId);
        BaseResponse DeleteNurDemandEval(long recordId);
        #endregion

        #region 团体活动批量评估

        BaseResponse SaveBatchQuetion(List<REGQUESTION> request);
      
        #endregion

        #region 药品管理
        /// <summary>
        /// 查询药品信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Medicine>> QueryMedData(BaseRequest<MedicineFilter> request);
        /// <summary>
        /// 保存药品信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<Medicine> SaveMedData(Medicine request);
        /// <summary>
        /// 删除药品信息
        /// </summary>
        /// <param name="MEDID"></param>
        /// <returns></returns>
        BaseResponse DeleteMedData(long MEDID);
        /// <summary>
        /// 获取单个药品信息
        /// </summary>
        /// <param name="medId"></param>
        /// <returns></returns>
        BaseResponse<Medicine> GetMedData(int medId);
        #endregion


        #region 护理需求
        BaseResponse<IList<EvaluationRecord>> QueryNurDemandEvalList(NursingFilter request);
        BaseResponse<IList<EvaluationResult>> QueryEvaluationResult(long feeNo);

        BaseResponse<IList<Person>> QueryPerson(int regno);

        BaseResponse<IList<CareDemandEval>> QueryCareDemandHis(NursingFilter request);
        BaseResponse<CareDemandEval> QueryLatestCareDemand(long feeNo);
        #endregion

        #region 用医就药
        /// <summary>
        /// 获取就诊记录表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<VisitDocRecords>> QueryVisitDocRecData(BaseRequest<VisitDocRecordsFilter> request);
        /// <summary>
        /// 保存就诊信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<VisitDocRecords> SaveVisitDocRecData(VisitDocRecords request);
        /// <summary>
        /// 删除就诊信息
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        BaseResponse DeleteVisitDocRecData(long SEQNO);
        /// <summary>
        /// 获取处方信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<VisitPrescription>> QueryVisitPreData(BaseRequest<VisitPrescriptionFilter> request);
        /// <summary>
        /// 保存处方信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<VisitPrescription> SaveVisitPreData(VisitPrescription request);
        /// <summary>
        /// 删除处方信息
        /// </summary>
        /// <param name="SEQNO"></param>
        /// <returns></returns>
        BaseResponse DeleteVisitPreData(long SEQNO);

        #endregion

        BaseResponse<IList<NutrtioncateRec>> QueryNutrtioncateRec(BaseRequest<NutrtioncateRecFilter> request);

        BaseResponse<NutrtioncateRec> GetNutrtioncateRec(long id);

        BaseResponse<NutrtioncateRec> SaveNutrtioncateRec(NutrtioncateRec request);

        BaseResponse DeleteNutrtioncateRec(long id);

    }
}

