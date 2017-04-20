using AutoMapper;
/*
创建人: 张祥
创建日期:2016-04-09
说明:就诊医院管理
*/
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class VisitManageService : BaseService, IVisitManageService
    {
        #region 就诊医院
        public BaseResponse<IList<VisitHospital>> QueryVisitHospital(BaseRequest<VisitHospitalFilter> request)
        {
            BaseResponse<IList<VisitHospital>> response = new BaseResponse<IList<VisitHospital>>();
            Mapper.CreateMap<LTC_VISITHOSPITAL, VisitHospital>();
            Mapper.CreateMap<LTC_VISITDEPT, VisitDept>();
            Mapper.CreateMap<LTC_VISITDOCTOR, VisitDoctor>();
            var q = from m in unitOfWork.GetRepository<LTC_VISITHOSPITAL>().dbSet
                    select m;
            q = q.Where(m => m.ORGID == request.Data.OrgId);
            q = q.OrderBy(m => m.HOSPNO);
            List<LTC_VISITHOSPITAL> list = q.ToList();
            var data = new List<VisitHospital>();
            var visitHospital = new VisitHospital();
            foreach (var item in list)
            {
                visitHospital.HospName = item.HOSPNAME;
                visitHospital.HospNo = item.HOSPNO;
                visitHospital.OrgId = item.ORGID;
                visitHospital.VisitDept = Mapper.Map<IList<VisitDept>>(item.LTC_VISITDEPT);
                visitHospital.VisitDoctor = Mapper.Map<IList<VisitDoctor>>(item.LTC_VISITDOCTOR);
                data.Add(visitHospital);
                visitHospital = new VisitHospital();
            }

            response.Data = data;
            return response;
        }
        #endregion

        #region 就诊科室
        public BaseResponse<IList<VisitDept>> QueryVisitDept(BaseRequest<VisitDeptFilter> request)
        {
            BaseResponse<IList<VisitDept>> response = new BaseResponse<IList<VisitDept>>();
            Mapper.CreateMap<LTC_VISITDEPT, VisitDept>();
            var q = from m in unitOfWork.GetRepository<LTC_VISITDEPT>().dbSet
                    select m;
            q = q.Where(m => m.HOSPNO == request.Data.HospNo);
            q = q.OrderByDescending(m => m.DEPTNO);
            List<LTC_VISITDEPT> list = q.ToList();
            response.Data = Mapper.Map<IList<VisitDept>>(list);
            return response;
        }
        #endregion

        #region 就诊医师
        public BaseResponse<IList<VisitDoctor>> QueryVisitDoctor(BaseRequest<VisitDoctorFilter> request)
        {
            BaseResponse<IList<VisitDoctor>> response = new BaseResponse<IList<VisitDoctor>>();
            Mapper.CreateMap<LTC_VISITDOCTOR, VisitDoctor>();
            var q = from m in unitOfWork.GetRepository<LTC_VISITDOCTOR>().dbSet
                    select m;
            q = q.Where(m => m.DEPTNO == request.Data.DeptNo);
            q = q.OrderByDescending(m => m.DOCNO);
            List<LTC_VISITDOCTOR> list = q.ToList();
            response.Data = Mapper.Map<IList<VisitDoctor>>(list);
            return response;
        }
        #endregion

        #region 国际分类
        public BaseResponse<IList<Icd9_Disease>> QueryIcd9(BaseRequest<Icd9_DiseaseFilter> request)
        {
            BaseResponse<IList<Icd9_Disease>> response = new BaseResponse<IList<Icd9_Disease>>();
            Mapper.CreateMap<LTC_ICD9_DISEASE, Icd9_Disease>();
            var q = from m in unitOfWork.GetRepository<LTC_ICD9_DISEASE>().dbSet
                    select m;
            q = q.Where(m => m.ENGNAME.Contains(request.Data.KeyWord) || m.ICDCODE.Contains(request.Data.KeyWord));
            q = q.OrderBy(m => m.ICDCODE);
            //List<LTC_ICD9_DISEASE> list = q.ToList();
            //response.Data = Mapper.Map<IList<Icd9_Disease>>(list);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                response.Data = Mapper.Map<IList<Icd9_Disease>>(list);
            }
            else
            {
                var list = q.ToList();
                response.Data = Mapper.Map<IList<Icd9_Disease>>(list);
            }
            return response;
        }
        #endregion

        #region 服药
        public BaseResponse<IList<LTC_DRGFREQREF>> QueryFreq(BaseRequest<FreqFilter> request)
        {
            BaseResponse<IList<LTC_DRGFREQREF>> response = new BaseResponse<IList<LTC_DRGFREQREF>>();
            Mapper.CreateMap<LTC_DRGFREQREF, LTC_DRGFREQREF>();
            var q = from m in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet
                    select m;
            q = q.Where(m => m.FREQNO.Trim().ToUpper().Contains(request.Data.KeyWord.ToUpper()) || m.FREQNAME.Trim().ToUpper().Contains(request.Data.KeyWord.ToUpper()));
            q = q.OrderBy(m => m.FREQNO);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                response.Data = Mapper.Map<IList<LTC_DRGFREQREF>>(list);
            }
            else
            {
                var list = q.ToList();
                response.Data = Mapper.Map<IList<LTC_DRGFREQREF>>(list);
            }
            return response;
        }

        #endregion
    }
}

