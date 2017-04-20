/*
创建人: 张祥
创建日期:2016-04-09
说明:就诊医院管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IVisitManageService : IBaseService
    {
        /// <summary>
        /// 获取就诊医院信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<VisitHospital>> QueryVisitHospital(BaseRequest<VisitHospitalFilter> request);
        /// <summary>
        /// 获取就诊部门信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<VisitDept>> QueryVisitDept(BaseRequest<VisitDeptFilter> request);
        /// <summary>
        /// 获取就诊医师信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<VisitDoctor>> QueryVisitDoctor(BaseRequest<VisitDoctorFilter> request);
        /// <summary>
        /// 获取国际分类信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Icd9_Disease>> QueryIcd9(BaseRequest<Icd9_DiseaseFilter> request);
        /// <summary>
        /// 获取用药信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_DRGFREQREF>> QueryFreq(BaseRequest<FreqFilter> request);
    }
}

