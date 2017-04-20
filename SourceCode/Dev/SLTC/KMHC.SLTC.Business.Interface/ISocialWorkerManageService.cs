/*
创建人: 杨金高
创建日期:2016-03-09
说明:社工/行政
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace KMHC.SLTC.Business.Interface
{
    public interface ISocialWorkerManageService : IBaseService
    {
        #region ***********************补助申请***********************
        /// <summary>
        /// 获取补助申请列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<SubsidyView>> QuerySubsidy(BaseRequest<SubsidyFilter> request);

        /// <summary>
        /// 获取补助申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<SubsidyView> GetSubsidyById(int id);

        /// <summary>
        /// 保存补助申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<SubsidyView> SaveSubsidy(SubsidyView request);

        /// <summary>
        /// 删除补助申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteSubsidyById(int id);
        #endregion

        #region***********************资源连接***********************
        /// <summary>
        /// 获取资源连接列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ResourceLinkModel>> QueryResourceLink(BaseRequest<ResourceLinkFilter> request);

        /// <summary>
        /// 获取单个资源信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<ResourceLinkModel> GetResourceLink(int regNo);

        /// <summary>
        /// 保存资源连接信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<ResourceLinkModel> SaveResourceLink(ResourceLinkModel request);

        /// <summary>
        /// 根据指定资源ID进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteResourceLink(int regNo);
        #endregion

        #region***********************生活记录***********************
        /// <summary>
        /// 获取生活记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LifeRecordModel>> QueryLifeRecord(BaseRequest<LifeRecordFilter> request);
        /// <summary>
        /// 查询生活记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LifeRecordListModel>> QueryLifeRecordList(BaseRequest<LifeRecordListFilter> request);

        /// <summary>
        /// 获取生活记录(指定病历id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<LifeRecordModel> GetLifeRecordById(int id);

        /// <summary>
        ///　删除生活记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteLifeRecordById(int id);

        /// <summary>
        /// 保存生活记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LifeRecordModel> SaveLifeRecord(LifeRecordModel request);

        /// <summary>
        /// 批量保存生活记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<List<LifeRecordModel>> SaveList(List<LifeRecordListModel> request);

        #endregion

        #region *************************转介*************************
        /// <summary>
        /// 获取社工转介列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ReferralRecModel>> QueryReferralRec(BaseRequest<ReferralRecFilter> request);

        /// <summary>
        /// 获取转介
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<ReferralRecModel> GetReferralById(int id);

        /// <summary>
        /// 保存转介
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<ReferralRecModel> SaveReferral(ReferralRecModel request);

        /// <summary>
        /// 删除转介
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteReferralById(int id);
        #endregion

        #region*********************社工服务记录*********************
        /// <summary>
        /// 获取社工记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CaresvrRecModel>> QueryCareSvrRec(BaseRequest<CaresvrRecFilter> request);

        /// <summary>
        /// 获取社工记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CaresvrRecModel> GetCareSvrById(int id);

        /// <summary>
        ///　删除社工记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteCareSvrById(int id);

        /// <summary>
        /// 保存社工记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<CaresvrRecModel> SaveCareSvr(CaresvrRecModel request);

        #endregion

        #region***********************权益申诉***********************
        /// <summary>
        /// 获取申诉列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ComplainRecModel>> QueryComplainRec(BaseRequest<ComplainRecFilter> request);

        /// <summary>
        /// 获取申诉(指定id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<ComplainRecModel> GetComplainRecById(int id);

        /// <summary>
        ///　删除申诉
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteComplainRecById(int id);

        /// <summary>
        /// 保存申诉
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<ComplainRecModel> SaveComplainRec(ComplainRecModel request);
        #endregion

        #region***********************居家督导***********************
        /// <summary>
        /// 获取居家督导服务列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<HomeCareSuperviseModel>> QueryHomeCareSupervise(BaseRequest<HomeCareSuperviseFilter> request);

        /// <summary>
        /// 获取居家督导服务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<HomeCareSuperviseModel> GetHomeCareSuperviseById(int id);

        /// <summary>
        ///　删除居家督导服务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteHomeCareSuperviseById(int id);

        /// <summary>
        /// 保存居家督导服务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<HomeCareSuperviseModel> SaveHomeCareSupervise(HomeCareSuperviseModel request);
        #endregion

        #region***********************管路指标***********************
        /// <summary>
        /// 获取管路
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<PipelineRecModel>> QueryPipelineRec(BaseRequest<PipelineRecFilter> request);

        /// <summary>
        /// 获取单条管路数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<PipelineRecModel> GetPipelineRecById(int seqNo);

        /// <summary>
        ///　删除删除管路
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeletePipelineRecById(int seqNo);

        /// <summary>
        /// 保存管路
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<PipelineRecModel> SavePipelineRec(PipelineRecModel request);

        /// <summary>
        /// 移除管路信息
        /// </summary>
        /// <param name="feeno">FEENO</param>
        /// <param name="pipelineName">管路名称</param>
        /// <param name="removeInfo">移除信息</param>
        void RemovePipelineRec(long feeno, string pipelineName, DateTime dateTime, string removeInfo);

        #region 管路明细

      

        /// <summary>
        /// 获取管路
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<PipelineEvalModel>> QueryPipelineEval(BaseRequest<PipelineEvalFilter> request);


        /// <summary>
        ///　删除删除管路
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeletePipelineEvalById(int id);

        /// <summary>
        /// 保存管路
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<PipelineEvalModel> SavePipelineEval(PipelineEvalModel request);

        /// <summary>
        /// 查询管路更换记录
        /// </summary>
        /// <param name="seqNo">用户标识</param>
        /// <param name="recDate">更换日期</param>
        /// <param name="pipeLineType">管路类型</param>
        /// <returns></returns>
        BaseResponse<PipelineEvalModel> GetPipelineEvalToNurse(int feeNo, DateTime recDate, string pipeLineName);

        #endregion

        #endregion

        #region***********************社工评估***********************
        /// <summary>
        /// 获取社工评估
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<RegEvaluateModel>> QueryRegEvaluate(BaseRequest<RegEvaluateFilter> request);

        /// <summary>
        /// 获取单条社工评估
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<RegEvaluateModel> GetRegEvaluateById(int id);

        /// <summary>
        ///　删除社工评估
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteRegEvaluateById(int id);

        /// <summary>
        /// 保存社工评估
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<RegEvaluateModel> SaveRegEvaluate(RegEvaluateModel request);

        #endregion

		#region **********************营养评估单**********************
		/// <summary>
		/// 获取营养评估单列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		BaseResponse<IList<NutritionEvalModel>> QueryNutritionEval(BaseRequest<NutritionEvalFilter> request);

		BaseResponse<IList<BiochemistryModel>> QueryBiochemistryByDate(int feeNo, string code1, string code2, string code3, DateTime s_date, DateTime e_date);

		/// <summary>
		/// 获取营养评估单
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		BaseResponse<NutritionEvalModel> GetNutritionEvalById(int id);

		/// <summary>
		/// 保存营养评估单
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		BaseResponse<NutritionEvalModel> SaveNutritionEval(NutritionEvalModel request);

		/// <summary>
		/// 删除营养评估单
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        BaseResponse DeleteNutritionEvalById(long id);
		#endregion

        #region***********************新进住民环境介绍记录表***********************
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NewResideEntenvRec>> QueryNewResideEntenvRec(BaseRequest<NewResideEntenvRecFilter> request);

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NewResideEntenvRec> GetNewResideEntenvRecById(int id);
  
        /// <summary>
        ///　删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteNewResideEntenvRecById(int id);

        /// <summary>
        /// 保存记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<NewResideEntenvRec> SaveNewResideEntenvRec(NewResideEntenvRec request);

        #endregion
        #region***********************新进住民环境适应及辅导记录表***********************
        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NewRegEnvAdaptation>> QueryNewRegEnvAdaptation(BaseRequest<NewResideEntenvRecFilter> request);

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<NewRegEnvAdaptation> GetNewRegEnvAdaptationById(int id);

        /// <summary>
        ///　删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteNewRegEnvAdaptationById(int id);

        /// <summary>
        /// 保存记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<NewRegEnvAdaptation> SaveNewRegEnvAdaptation(NewRegEnvAdaptation request);

        #endregion

        #region***********************多重用药***********************
        /// <summary>
        /// 获取多重用药
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Pharmacist>> QueryPharmacist(BaseRequest<PharmacistFilter> request);

        /// <summary>
        /// 获取单条多重用药
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Pharmacist> GetPharmacistById(int id);

        /// <summary>
        ///　删除多重用药
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeletePharmacistById(int id);

        /// <summary>
        /// 保存多重用药
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<Pharmacist> SavePharmacist(Pharmacist request);

        #endregion

        #region***********************院民清册***********************
        /// <summary>
        /// 获取院民清册列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<RSDList>> QueryPersonsExtend(BaseRequest<ResidentFilter> request);

        BaseResponse<IList<RSDList>> GetResidentIpdById(int id);

        ///// <summary>
        ///// 获取单条院民信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public BaseResponse<Person> GetPersonById(int regNo)
        //{
        //    var response = base.Get<LTC_REGFILE, Person>((q) => q.REGNO == regNo);
        //    BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
        //    request.Data.RegNo = regNo;

        //    var residentList = this.QueryResident(request);
        //    if (residentList.Data.Count > 0)
        //    {
        //        response.Data.FeeNo = residentList.Data[0].FeeNo;
        //    }
        //    return response;
        //}

        ///// <summary>
        /////　删除院民信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //BaseResponse DeletePersonById(int id);

        /// <summary>
        /// 保存院民信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<RSDList> SaveRSDList(RSDList request);
        object GetNursingHomeAndLeaveRes();
        #endregion

        #region***********************智能仪表盘*********************



        DataTable QueryInData(string orgId);
	
        DataTable QueryOutData(string orgId);
        DataTable QueryBedData(string orgId);
        object QueryBedData2(string orgId);
        DataTable QueryBedSoreData(string orgId);
        #endregion

        #region
        /// <summary>
        /// 个别化活动历次资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<RegActivityRequEval>> QueryActivityRequEval(BaseRequest<RegActivityRequEvalFilter> request);
        /// <summary>
        /// 最近一笔资料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<RegActivityRequEval> GetActivityRequEval(long id);
        /// <summary>
        /// 保存个别化活动资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<RegActivityRequEval> SaveActivityRequEval(RegActivityRequEval request);
        /// <summary>
        /// 删除个别化活动资料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteActivityRequEval(long id);
        #endregion
    }
}


