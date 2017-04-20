/*************************************************************************************************
 * 描述:日间照顾-社工-个案基本资料
 * 创建日期:2016-5-9
 * 创建人:杨金高
 * **********************************************************************************************/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Data;
using System.Text;
using System.Data.Objects.SqlClient;
using KMHC.SLTC.Repository;
namespace KMHC.SLTC.Business.Implement
{
    public class DC_SocialWorkerService : BaseService, IDC_SocialWorkerService
    {
        #region***********************日照部分--个案基本资料*********************
        /// <summary>
        /// 获取个案基本资料列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_RegFileModel>> QueryPersonBasic(BaseRequest<DC_RegFileFilter> request)
        {
            BaseResponse<IList<DC_RegFileModel>> response = new BaseResponse<IList<DC_RegFileModel>>();

            DC_IpdRegModel model = (from r in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(m=>m.FEENO==request.Data.FeeNo) select new DC_IpdRegModel{FeeNo=r.FEENO,RegNo=r.REGNO}).FirstOrDefault();
            if (model.FeeNo > 0)
            {
                var q =from ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(m=>m.FEENO==model.FeeNo)
                        join reg in  unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO into r
                        from ipd_reg in r.DefaultIfEmpty()
                        join orgs in unitOfWork.GetRepository<LTC_ORG>().dbSet on ipd.ORGID equals orgs.ORGID into o
                        from org in o.DefaultIfEmpty()
                        select new DC_RegFileModel
                        {
                            OrgId = ipd_reg.ORGID,
                            RegNo = ipd_reg.REGNO,
                            RegName = ipd_reg.REGNAME,
                            ResidentNo = ipd.RESIDENTNO,
                            Sex = ipd_reg.SEX,
                            BirthPlace = ipd_reg.BIRTHPLACE,
                            OriginPlace = ipd_reg.ORIGINPLACE,
                            BirthDate = ipd_reg.BIRTHDATE,
                            IdNo = ipd_reg.IDNO,
                            PType = ipd_reg.PTYPE,
                            Phone = ipd_reg.PHONE,
                            LivingAddress = ipd_reg.LIVINGADDRESS,
                            PermanentAddress = ipd_reg.PERMANENTADDRESS,
                            Language = ipd_reg.LANGUAGE,
                            Education = ipd_reg.EDUCATION,
                            LivCondition = ipd_reg.LIVCONDITION,
                            MerryState = ipd_reg.MERRYSTATE,
                            Profession = ipd_reg.PROFESSION,
                            Religion = ipd_reg.RELIGION,
                            EconomicSources = ipd_reg.ECONOMICSOURCES,
                            SourceType = ipd_reg.SOURCETYPE,
                            ObstacleManual = ipd_reg.OBSTACLEMANUAL,
                            DiseaseInfo = ipd_reg.DISEASEINFO,
                            SuretyName = ipd_reg.SURETYNAME,
                            SuretyAge = ipd_reg.SURETYAGE,
                            SuretyUnit = ipd_reg.SURETYUNIT,
                            SuretyAddress = ipd_reg.SURETYADDRESS,
                            SuretyEmail = ipd_reg.SURETYEMAIL,
                            SuretyTitle = ipd_reg.SURETYTITLE,
                            SuretyPhone = ipd_reg.SURETYPHONE,
                            SuretyMobile = ipd_reg.SURETYMOBILE,
                            ContactName1 = ipd_reg.CONTACTNAME1,
                            ContactAge1 = ipd_reg.CONTACTAGE1,
                            ContactTitle1 = ipd_reg.CONTACTTITLE1,
                            ContactEmail1 = ipd_reg.CONTACTEMAIL1,
                            ContactAddress1 = ipd_reg.CONTACTADDRESS1,
                            ContactPhone1 = ipd_reg.CONTACTPHONE1,
                            ContactName2 = ipd_reg.CONTACTNAME2,
                            ContactAge2 = ipd_reg.CONTACTAGE2,
                            ContactTitle2 = ipd_reg.CONTACTTITLE2,
                            ContactEmail2 = ipd_reg.CONTACTEMAIL2,
                            ContactAddress2 = ipd_reg.CONTACTADDRESS2,
                            ContactPhone2 = ipd_reg.CONTACTPHONE2,
                            EcologicalMap = ipd_reg.ECOLOGICALMAP,
                            InDate = ipd.INDATE,
                            ContactMobile1 = ipd_reg.CONTACTMOBILE1,
                            ContactMobile2 = ipd_reg.CONTACTMOBILE2,
                            ContactUnit1 = ipd_reg.CONTACTUNIT1,
                            ContactUnit2 = ipd_reg.CONTACTUNIT2,
                            NickName = ipd_reg.NICKNAME,
                            OrgName = org.ORGNAME
                        };
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                    q = q.Where(m => m.OrgId == request.Data.OrgId);
                if (!string.IsNullOrEmpty(model.RegNo))
                    q = q.Where(m => m.RegNo == model.RegNo);
                if (!string.IsNullOrEmpty(request.Data.IdNo))
                    q = q.Where(m => m.IdNo == request.Data.IdNo);
                if (!string.IsNullOrEmpty(request.Data.Name))
                    q = q.Where(m => m.RegName == request.Data.Name);
                q = q.OrderByDescending(m => m.RegNo);
                response.RecordsCount = q.Count();

                if (request != null && request.PageSize > 0)
                {
                    response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    response.Data = q.ToList();
                }
            }
            return response;
        }

        /// <summary>
        /// 获取个案基本资料
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegFileModel> GetPersonBasicById(string id)
        {
            return base.Get<DC_REGFILE, DC_RegFileModel>((q) => q.REGNO == id);
        }

        /// <summary>
        /// 保存个案基本资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegFileModel> SavePersonBasic(DC_RegFileModel request)
        {
			BaseResponse<DC_RegFileModel> response = base.Save<DC_REGFILE, DC_RegFileModel>(request, (q) => q.REGNO == request.RegNo);
			if (response.Data != null) {
				
				BaseResponse<DC_IpdRegModel> model = base.Get<DC_IPDREG, DC_IpdRegModel>((q) => q.REGNO == response.Data.RegNo); 
				if (model != null) {

					model.Data.ResidentNo = response.Data.ResidentNo;
					BaseResponse<DC_IpdRegModel> returnIpd = base.Save<DC_IPDREG, DC_IpdRegModel>(model.Data, (q) => q.REGNO == model.Data.RegNo);
				}
			}
			
			return response;
        }

        /// <summary>
        /// 删除个案基本资料
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeletePersonBasicById(string id)
        {
            return base.Delete<DC_REGFILE>(id);
        }
        #endregion********************************************

        #region*************************日照部分--个案转介***********************
        /// <summary>
        /// 获取个案转介列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_ReferrallistsModel>> QueryReferrallist(BaseRequest<DC_ReferrallistsFilter> request)
        {
            BaseResponse<IList<DC_ReferrallistsModel>> response = new BaseResponse<IList<DC_ReferrallistsModel>>();
            var q = from reff in unitOfWork.GetRepository<DC_REFERRALLISTS>().dbSet
                    join reg_ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet on reff.FEENO equals reg_ipd.FEENO into reg_ipds
                    from regipd in reg_ipds.DefaultIfEmpty()
                    join reg_file in unitOfWork.GetRepository<DC_REGFILE>().dbSet on regipd.REGNO equals reg_file.REGNO into reg_files
                    from regfile in reg_files.DefaultIfEmpty()
                    //join orgs in unitOfWork.GetRepository<LTC_ORG>().dbSet on reff.ORGID equals orgs.ORGID into o
                    //from org in o.DefaultIfEmpty()
                    select new DC_ReferrallistsModel
                    {
                        Id=reff.ID,
                        FeeNo=reff.FEENO,
                        OrgId=reff.ORGID,
                        SiologicalState = reff.SIOLOGICALSTATE,
                        ProblemStatement = reff.PROBLEMSTATEMENT,
                        ReferralPurpose = reff.REFERRALPURPOSE,
                        DocumentInfo = reff.DOCUMENTINFO,
                        ReferralDate = reff.REFERRALDATE,
                        ReferralUnit = reff.REFERRALUNIT,
                        ReferralResult = reff.REFERRALRESULT,
                        ReplyDate = reff.REPLYDATE,
                        UnitContactor = reff.UNITCONTACTOR,
                        UnitPhone = reff.UNITPHONE,
                        UnitFax = reff.UNITFAX,
                        CreateDate=reff.CREATEDATE,
                        RegName=regfile.REGNAME,
                        SuretyName=regfile.SURETYNAME,
                        SuretyPhone=regfile.SURETYPHONE,
                        Sex=regfile.SEX,
                        InDate=regipd.INDATE,
                        //OrgName=org.ORGNAME
                    };
            if (!string.IsNullOrEmpty(request.Data.OrgId))
                q = q.Where(p => p.OrgId == request.Data.OrgId);
            if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
                q = q.Where(p => p.FeeNo == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.CreateDate);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }

        /// <summary>
        /// 获取个案转介
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<DC_ReferrallistsModel> GetReferralById(int id)
        {
            return base.Get<DC_REFERRALLISTS, DC_ReferrallistsModel>((q) => q.ID == id);
        }

        /// <summary>
        /// 保存个案转介
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_ReferrallistsModel> SaveReferral(DC_ReferrallistsModel request)
        {
            return base.Save<DC_REFERRALLISTS, DC_ReferrallistsModel>(request, (q) => q.ID == request.Id);
        }

        /// <summary>
        /// 删除个案转介
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeleteReferralById(int id)
        {
            return base.Delete<DC_REFERRALLISTS>(id);
        }

        #endregion#

        #region***************************日照部分--1天生活**********************

        /// <summary>
        /// 获取个案生活记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_RegDayLifeModel>> QueryDayLife(BaseRequest<DC_RegDayLifeFilter> request)
        {
            BaseResponse<IList<DC_RegDayLifeModel>> response = new BaseResponse<IList<DC_RegDayLifeModel>>();
            var q = from daylife in unitOfWork.GetRepository<DC_REGDAYLIFE>().dbSet
                    select new DC_RegDayLifeModel
                    {
                        FeeNo = daylife.FEENO,
                        OrgId = daylife.ORGID,
                        Past0 = daylife.PAST0,
                        Past2 = daylife.PAST2,
                        Past4 = daylife.PAST4,
                        Past6 = daylife.PAST6,
                        Past7 = daylife.PAST7,
                        Past8 = daylife.PAST8,
                        Past9 = daylife.PAST9,
                        Past10 = daylife.PAST10,
                        Past11 = daylife.PAST11,
                        Past12 = daylife.PAST12,
                        Past14 = daylife.PAST14,
                        Past15 = daylife.PAST15,
                        Past16 = daylife.PAST16,
                        Past17 = daylife.PAST17,
                        Past18 = daylife.PAST18,
                        Past19 = daylife.PAST19,
                        Past20 = daylife.PAST20,
                        Past21 = daylife.PAST21,
                        Past22 = daylife.PAST22,
                        Past24 = daylife.PAST24,
                        Now0=daylife.NOW0,
                        Now2 = daylife.NOW2,
                        Now4 = daylife.NOW4,
                        Now6 = daylife.NOW6,
                        Now7 = daylife.NOW7,
                        Now8 = daylife.NOW8,
                        Now9 = daylife.NOW9,
                        Now10 = daylife.NOW10,
                        Now11 = daylife.NOW11,
                        Now12 = daylife.NOW12,
                        Now14 = daylife.NOW14,
                        Now15 = daylife.NOW15,
                        Now16 = daylife.NOW16,
                        Now17 = daylife.NOW17,
                        Now18 = daylife.NOW18,
                        Now19 = daylife.NOW19,
                        Now20 = daylife.NOW20,
                        Now21 = daylife.NOW21,
                        Now22 = daylife.NOW22,
                        Now24 = daylife.NOW24,
                        CreateDate = daylife.CREATEDATE,
                        Name=daylife.NAME
                    };
            if (!string.IsNullOrEmpty(request.Data.OrgId))
                q = q.Where(p => p.OrgId == request.Data.OrgId);
            if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
                q = q.Where(p => p.FeeNo == request.Data.FeeNo);
            //if (!string.IsNullOrEmpty(request.Data.CreateDate.ToString()))
            //    q = q.Where(p => DateTime.Parse(Convert.ToDateTime(p.CreateDate).ToShortDateString()) == request.Data.CreateDate);
            q = q.OrderByDescending(m => m.CreateDate);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }

        /// <summary>
        /// 获取个案单笔一天生活
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegDayLifeModel> GetDayLifeById(int feeNo)
        {
            DateTime? _CreateDate= DateTime.Parse(DateTime.Now.ToShortDateString());
            return base.Get<DC_REGDAYLIFE, DC_RegDayLifeModel>((q) => q.FEENO == feeNo);
        }
        /// <summary>
        /// 保存个案生活
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegDayLifeModel> SaveDayLife(DC_RegDayLifeModel request)
        {
            return base.Save<DC_REGDAYLIFE, DC_RegDayLifeModel>(request, (q) => q.Id == request.Id);
        }
        /// <summary>
        /// 删除个案生活
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeleteDayLifeById(int feeNo)
        {
            return base.Delete<DC_REGDAYLIFE>(feeNo);
        }

        #endregion

        #region**************************日照部分--个案生活史********************

        /// <summary>
        /// 获取个案生活史记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_LifeHistoryModel>> QueryLifeHistory(BaseRequest<DC_LifeHistoryFilter> request)
        {
            BaseResponse<IList<DC_LifeHistoryModel>> response = new BaseResponse<IList<DC_LifeHistoryModel>>();
            var q = from hs in unitOfWork.GetRepository<DC_LIFEHISTORY>().dbSet
                    select new DC_LifeHistoryModel
                    {
                        Id=hs.Id,
                        FeeNo = hs.FEENO,
                        OrgId = hs.ORGID,
                        Name=hs.NAME,
                        NickName=hs.NICKNAME,
                        ResidentNo=hs.RESIDENTNO,
                        BirthPlace=hs.BIRTHPLACE,
                        FamilyEnvironment=hs.FAMILYENVIRONMENT,
                        ChildHoodExperience=hs.CHILDHOODEXPERIENCE,
                        School=hs.SCHOOL,
                        ProudDeeds=hs.PROUDDEEDS,
                        Romance=hs.ROMANCE,
                        MerryInfo=hs.MERRYINFO,
                        MportantPeople=hs.MPORTANTPEOPLE,
                        WorkHistory=hs.WORKHISTORY,
                        ServiceHistory=hs.SERVICEHISTORY,
                        Religious=hs.RELIGIOUS,
                        Living=hs.LIVING,
                        PositivePersonality=hs.POSITIVEPERSONALITY,
                        NegativePersonality=hs.NEGATIVEPERSONALITY,
                        FamilyTroubled=hs.FAMILYTROUBLED,
                        SoothingEmotion=hs.SOOTHINGEMOTION,
                        Skill=hs.SKILL,
                        FavoriteDress=hs.FAVORITEDRESS,
                        Foodlike=hs.FOODLIKE,
                        Animallike=hs.ANIMALLIKE,
                        HolidayActivity=hs.HOLIDAYACTIVITY,
                        NotlikeThings=hs.NOTLIKETHINGS,
                        InterestedThings=hs.INTERESTEDTHINGS,
                        CreateBy=hs.CREATEBY,
                        CreateDate = hs.CREATEDATE
                    };
            if (!string.IsNullOrEmpty(request.Data.OrgId))
                q = q.Where(p => p.OrgId == request.Data.OrgId);
            if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
                q = q.Where(p => p.FeeNo == request.Data.FeeNo);
            if (!string.IsNullOrEmpty(request.Data.CreateDate.ToString()))
                q = q.Where(p => p.CreateDate == request.Data.CreateDate);
            q = q.OrderByDescending(m => m.CreateDate);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }

        /// <summary>
        /// 获取个案单笔生活史
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<DC_LifeHistoryModel> GetLifeHistoryById(int feeNo)
        {
            return base.Get<DC_LIFEHISTORY, DC_LifeHistoryModel>((q) => q.FEENO == feeNo);
        }
        /// <summary>
        /// 保存个案生活史
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_LifeHistoryModel> SaveLifeHistory(DC_LifeHistoryModel request)
        {
            return base.Save<DC_LIFEHISTORY, DC_LifeHistoryModel>(request, (q) => q.Id == request.Id);
        }
        /// <summary>
        /// 删除个案生活史
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeleteLifeHistoryById(int feeNo)
        {
            return base.Delete<DC_LIFEHISTORY>(feeNo);
        }

        #endregion

        #region************************日照部分--收案／结案表********************

        /// <summary>
        /// 获取个案结案列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_IpdRegModel>> QueryIpdRegOut(BaseRequest<DC_IpdRegFilter> request)
        {
            BaseResponse<IList<DC_IpdRegModel>> response = new BaseResponse<IList<DC_IpdRegModel>>();
            var q = from ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet
                    join ipd_file in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipd.REGNO equals ipd_file.REGNO into ipd_files
                    from ipdFile in ipd_files.DefaultIfEmpty()
                    join orgs in unitOfWork.GetRepository<LTC_ORG>().dbSet on ipd.ORGID equals orgs.ORGID into o
                    from org in o.DefaultIfEmpty()
                    select new DC_IpdRegModel
                    {
                        FeeNo = ipd.FEENO,
                        RegNo=ipd.REGNO,
                        OrgId = ipd.ORGID,
                        StationCode=ipd.STATIONCODE,
                        IpdFlag=ipd.IPDFLAG,
                        ResidentNo=ipd.RESIDENTNO,
                        CloseReason=ipd.CLOSEREASON,
                        OutDate=ipd.OUTDATE,
                        ProvideService=ipd.PROVIDESERVICE,
                        SvrContent=ipd.SVRCONTENT,
                        CreateDate=ipd.CREATEDATE,
                        RegName=ipdFile.REGNAME,
                        Sex=ipdFile.SEX,
                        BirthDate=ipdFile.BIRTHDATE,
                        IdNo=ipdFile.IDNO,
                        Phone=ipdFile.PHONE,
                        LivingAddress=ipdFile.LIVINGADDRESS,
                        PermanentAddress=ipdFile.PERMANENTADDRESS,
                        MerryState=ipdFile.MERRYSTATE,
                        InDate=ipd.INDATE,
                        SocialWorker=ipd.SOCIALWORKER,
                        NurseAides=ipd.NURSEAIDES,
                        NurseNo=ipd.NURSENO,
                        //OrgName=org.ORGNAME
                    };
            
            if (!string.IsNullOrEmpty(request.Data.OrgId))
                q = q.Where(p => p.OrgId == request.Data.OrgId);
            if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
                q = q.Where(p => p.FeeNo == request.Data.FeeNo);
        
            q = q.OrderByDescending(p => p.CreateDate);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
 
            return response;
        }

        /// <summary>
        /// 获取结案单笔记录
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<DC_IpdRegModel> GetIpdRegOutById(int feeNo)
        {
            return base.Get<DC_IPDREG, DC_IpdRegModel>((q) => q.FEENO == feeNo);
        }

		/// <summary>
		/// 校验身份证是否存在
		/// </summary>
		/// <param name="idNo"></param>
		/// <returns></returns>
		public BaseResponse<DC_RegFileModel> GetIpdInfo(string idNo) {
			BaseResponse<DC_RegFileModel> response = new BaseResponse<DC_RegFileModel>();
			response.Data = (from r in unitOfWork.GetRepository<DC_REGFILE>().dbSet.Where(m => m.IDNO == idNo  && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId).OrderBy(m=>m.REGNO) 
							 join i in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(m=>m.IPDFLAG=="I") on r.REGNO equals i.REGNO into ip
							 from ipd in ip.DefaultIfEmpty()
							 select new DC_RegFileModel { 
								 IdNo = r.IDNO, 
								 RegNo=r.REGNO,
								 RegName=r.REGNAME,
								 Sex=r.SEX,
								 BirthDate=r.BIRTHDATE,
								 IpdFlag=ipd.IPDFLAG,
								 FeeNo=ipd.FEENO,
								 StationCode=ipd.STATIONCODE,
								 InDate=ipd.INDATE
							 }).FirstOrDefault();
			return response;
		}
        /// <summary>
        /// 保存结案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_IpdRegModel> SaveIpdRegOut(DC_IpdRegModel request)
        {
            return base.Save<DC_IPDREG, DC_IpdRegModel>(request, (q) => q.FEENO == request.FeeNo);
        }
        /// <summary>
        /// 保存收案  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<KMHC.SLTC.Business.Entity.DC_IpdRegModel> SaveIpdRegIn(DC_IpdRegModel request)
        {
            bool hasIn = false;//用于判断是否曾经入住过，但出院了．出院再次入院是允许的
			BaseResponse<DC_IpdRegModel> response = new BaseResponse<DC_IpdRegModel>();
            if (request != null && request.isAdd=="1")
            {
				//根据录入的身份证号检查是否存在数据
                var response_regFile = base.Get<DC_REGFILE, DC_RegFileModel>((q) => { return q.IDNO == request.IdNo ; });

				if (response_regFile.Data != null) {//存在相同身份证字号
					//　如果存在相同身份证证再校验证是否在案，在案提示身份证已存在，结案的话可以再次入案
					var tmp1 = base.Get<DC_IPDREG, DC_IpdRegModel>((p) => { return p.REGNO == response_regFile.Data.RegNo && p.IPDFLAG == "I"; });
					var tmp2 = base.Get<DC_IPDREG, DC_IpdRegModel>((p) => { return p.REGNO == response_regFile.Data.RegNo && p.IPDFLAG == "O"; });
					if (tmp1.Data != null) {
						response.ResultCode = (int)EnumResponseStatus.ExceptionHappened;
						response.ResultMessage = "学员身份证字号'" + request.IdNo + "'已存在,如果要修改资料请至基本资料中修改！";
						return response;
					}
					else if (tmp2.Data != null) {
						hasIn = true;
						request.RegNo = response_regFile.Data.RegNo;
					}
				}
				else if(request.ResidentNo!=null){
					//不存在相同身份证，接着校验日字号
					BaseResponse<DC_IpdRegModel> ipdModel = base.Get<DC_IPDREG, DC_IpdRegModel>((q) => { return q.RESIDENTNO == request.ResidentNo; });
					if (ipdModel.Data != null) {

						response.ResultCode = (int)EnumResponseStatus.ExceptionHappened;
						response.ResultMessage = "日字号已存在！";
						return response;
					}
				}
				else {
					request.RegNo = string.Empty;
				}
            }
            //根据编码规则获取住民RegNo
            if(string.IsNullOrEmpty(request.RegNo))
            {
                //request.RegNo = base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.RegNo);
                request.RegNo = base.GenerateCode("DCRegNo", EnumCodeKey.DCRegNo);
            }
            
            //开始事务，因为涉及到两张表的数据插入
            //Step1:插入主表(DC_REGFILE)
            //Step2:插入从表(DC_IPDREG)
            //unitOfWork.BeginTransaction();
            

            if (request.RegNo.Length> 0)
            {
                if (!hasIn)
                {
                    DC_REGFILE regFile = new DC_REGFILE();
                    regFile.REGNO = request.RegNo;
                    regFile.REGNAME = request.RegName;
                    regFile.BIRTHDATE = request.BirthDate;
                    regFile.IDNO = request.IdNo;
                    regFile.SEX = request.Sex;
                    regFile.ORGID = request.OrgId;
                    unitOfWork.GetRepository<DC_REGFILE>().Insert(regFile);
                }
				if (hasIn) {
					request.FeeNo = 0;
					BaseResponse<IList<DC_IpdRegModel>> ipds = new BaseResponse<IList<DC_IpdRegModel>>();
					var q = (from hs in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(m => m.REGNO == request.RegNo && m.IPDFLAG=="I")
							 select new DC_IpdRegModel { FeeNo=hs.FEENO,RegNo=hs.REGNO,IpdFlag=hs.IPDFLAG});
					ipds.Data = q.ToList();
					foreach (DC_IpdRegModel item in ipds.Data) {
						if (item.IpdFlag == "I") {
							request.FeeNo = item.FeeNo;
							break;
						} 
					}
					
				} 
                response = base.Save<DC_IPDREG, DC_IpdRegModel>(request, (q) => q.FEENO == request.FeeNo);
			
               // unitOfWork.GetRepository<DC_IPDREG>().Insert(request);
               // unitOfWork.Commit();
                
                return response;
            }
            return null;
        }
        /// <summary>
        /// 修改收案资料(主要用于换区)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_IpdRegModel> SaveUpdateIpdRegIn(DC_IpdRegModel request)
        {
               return base.Save<DC_IPDREG, DC_IpdRegModel>(request, (q) => q.FEENO == request.FeeNo);
        }
        /// <summary>
        /// 删除个案结案
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeleteIpdRegOutById(int feeNo)
        {
            return base.Delete<DC_IPDREG>(feeNo);
        }

        #endregion

        #region*******************日照部分--社工个案评估及处遇计划表*************

        /// <summary>
        /// 获取社工个案评估及处遇计划列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_SwRegEvalPlanModel>> QuerySwRegEvalPlan(BaseRequest<DC_SwRegEvalPlanFilter> request)
        {
            BaseResponse<IList<DC_SwRegEvalPlanModel>> response = new BaseResponse<IList<DC_SwRegEvalPlanModel>>();
            var q = from srp in unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet
                    join tgt in unitOfWork.GetRepository<DC_TASKGOALSSTRATEGY>().dbSet on srp.EVALPLANID equals tgt.EVALPLANID into srp_tgt
                    from srps in srp_tgt.DefaultIfEmpty()
            
                    select new DC_SwRegEvalPlanModel
                    {
                        EVALPLANID = srp.EVALPLANID,
                        FEENO = srp.FEENO,
                        RESIDENTNO = srp.RESIDENTNO,
                        EVALDATE = srp.EVALDATE,
                        EVALNUMBER = srp.EVALNUMBER,
                        NEXTEVALDATE = srp.NEXTEVALDATE,
                        INDATE = srp.INDATE,
                        REGNAME = srp.REGNAME,
                        SEX = srp.SEX,
                        BIRTHDATE = srp.BIRTHDATE,
                        IDNO = srp.IDNO,
                        CONTACTNAME = srp.CONTACTNAME,
                        CONTACTPHONE = srp.CONTACTPHONE,
                        CONTACTMOBILE = srp.CONTACTMOBILE,
                        APPELLATION = srp.APPELLATION,
                        LIVINGADDRESS = srp.LIVINGADDRESS,
                        PTYPE = srp.PTYPE,
                        OBSTACLEMANUAL = srp.OBSTACLEMANUAL,
                        SOURCETYPE = srp.SOURCETYPE,
                        TAKECAREREASON = srp.TAKECAREREASON,
                        TAKECARETYPE = srp.TAKECARETYPE,
                        SERVICETYPE = srp.SERVICETYPE,
                        DISEASEINFO = srp.DISEASEINFO,
                        ECOLOGICALMAP = srp.ECOLOGICALMAP,
                        PERSONALHISTORY = srp.PERSONALHISTORY,
                        PHYSIOLOGY = srp.PHYSIOLOGY,
                        PSYCHOLOGY = srp.PSYCHOLOGY,
                        FAMILYSUPPORT = srp.FAMILYSUPPORT,
                        ECONOMICCAPACITY = srp.ECONOMICCAPACITY,
                        SOCIALRESOURCES = srp.SOCIALRESOURCES,
                        SOCIALRESOURCE = srp.SOCIALRESOURCE,
                        CURRENTSUBSIDY = srp.CURRENTSUBSIDY,
                        ASSISTAPPLICATION = srp.ASSISTAPPLICATION,
                        MMSE = srp.MMSE,
                        ADL = srp.ADL,
                        IADL = srp.IADL,
                        GDS = srp.GDS,
                        EMOTIONSTATE = srp.EMOTIONSTATE,
                        BEHAVIOR = srp.BEHAVIOR,
                        ATTITUDE = srp.ATTITUDE,
                        PAYATTENTION = srp.PAYATTENTION,
                        THOUGHT = srp.THOUGHT,
                        UNDERSTANDABILITY = srp.UNDERSTANDABILITY,
                        SOCIALABILITY = srp.SOCIALABILITY,
                        EYESIGHT = srp.EYESIGHT,
                        HEARING = srp.HEARING,
                        EXPRESSION = srp.EXPRESSION,
                        UNDERSTANDING = srp.UNDERSTANDING,
                        FAMILYINTERACTION = srp.FAMILYINTERACTION,
                        RELATIVEINTERACTION = srp.RELATIVEINTERACTION,
                        FRIENDINTERACTION = srp.FRIENDINTERACTION,
                        ELDERINTERACTION = srp.ELDERINTERACTION,
                        ADAPTIVESTATE = srp.ADAPTIVESTATE,
                        LIVINGCONDITION = srp.LIVINGCONDITION,
                        JOBINFO = srp.JOBINFO,
                        DAYTAKECAREHOUR = srp.DAYTAKECAREHOUR,
                        RELATIVESNEEDCARE = srp.RELATIVESNEEDCARE,
                        REPLACEMENT = srp.REPLACEMENT,
                        EASEPRESSURE = srp.EASEPRESSURE,
                        LIFEQUALITY = srp.LIFEQUALITY,
                        FAMILYEXPECT = srp.FAMILYEXPECT,
                        ORGID = srp.ORGID,
                        DELFLAG = srp.DELFLAG,
                        DELDATE = srp.DELDATE,
                     
                    };
            if (!string.IsNullOrEmpty(request.Data._orgId))
                q = q.Where(p => p.ORGID == request.Data._orgId);
            if (!string.IsNullOrEmpty(request.Data._feeno.ToString()))
                q = q.Where(p => p.FEENO == request.Data._feeno);

            q = q.OrderByDescending(p => p.EVALPLANID);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }
        /// <summary>
        /// 社工个案评估历史记录
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_SwRegEvalPlanModel>> QueryRegEvalHistory(int feeNo,int pageIndex,int pageSize)
        {
            BaseResponse<IList<DC_SwRegEvalPlanModel>> response = new BaseResponse<IList<DC_SwRegEvalPlanModel>>();
            var q = (from srp in unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet.Where(m => m.FEENO == feeNo).OrderByDescending(m => m.EVALPLANID)
                            select new DC_SwRegEvalPlanModel {
                                EVALPLANID = srp.EVALPLANID,
                                FEENO = srp.FEENO,
                                RESIDENTNO = srp.RESIDENTNO,
                                EVALDATE = srp.EVALDATE,
                                EVALNUMBER = srp.EVALNUMBER,
                                NEXTEVALDATE = srp.NEXTEVALDATE,
                                INDATE = srp.INDATE,
                                REGNAME = srp.REGNAME,
                                SEX = srp.SEX,
                                BIRTHDATE = srp.BIRTHDATE,
                                IDNO = srp.IDNO,
                                CONTACTNAME = srp.CONTACTNAME,
                                CONTACTPHONE = srp.CONTACTPHONE,
                                CONTACTMOBILE = srp.CONTACTMOBILE,
                                APPELLATION = srp.APPELLATION,
                                LIVINGADDRESS = srp.LIVINGADDRESS,
                                PTYPE = srp.PTYPE,
                                OBSTACLEMANUAL = srp.OBSTACLEMANUAL,
                                SOURCETYPE = srp.SOURCETYPE,
                                TAKECAREREASON = srp.TAKECAREREASON,
                                TAKECARETYPE = srp.TAKECARETYPE,
                                SERVICETYPE = srp.SERVICETYPE,
                                DISEASEINFO = srp.DISEASEINFO,
                                ECOLOGICALMAP = srp.ECOLOGICALMAP,
                                PERSONALHISTORY = srp.PERSONALHISTORY,
                                PHYSIOLOGY = srp.PHYSIOLOGY,
                                PSYCHOLOGY = srp.PSYCHOLOGY,
                                FAMILYSUPPORT = srp.FAMILYSUPPORT,
                                ECONOMICCAPACITY = srp.ECONOMICCAPACITY,
                                SOCIALRESOURCES = srp.SOCIALRESOURCES,
                                SOCIALRESOURCE = srp.SOCIALRESOURCE,
                                CURRENTSUBSIDY = srp.CURRENTSUBSIDY,
                                ASSISTAPPLICATION = srp.ASSISTAPPLICATION,
                                MMSE = srp.MMSE,
                                ADL = srp.ADL,
                                IADL = srp.IADL,
                                GDS = srp.GDS,
                                EMOTIONSTATE = srp.EMOTIONSTATE,
                                BEHAVIOR = srp.BEHAVIOR,
                                ATTITUDE = srp.ATTITUDE,
                                PAYATTENTION = srp.PAYATTENTION,
                                THOUGHT = srp.THOUGHT,
                                UNDERSTANDABILITY = srp.UNDERSTANDABILITY,
                                SOCIALABILITY = srp.SOCIALABILITY,
                                EYESIGHT = srp.EYESIGHT,
                                HEARING = srp.HEARING,
                                EXPRESSION = srp.EXPRESSION,
                                UNDERSTANDING = srp.UNDERSTANDING,
                                FAMILYINTERACTION = srp.FAMILYINTERACTION,
                                RELATIVEINTERACTION = srp.RELATIVEINTERACTION,
                                FRIENDINTERACTION = srp.FRIENDINTERACTION,
                                ELDERINTERACTION = srp.ELDERINTERACTION,
                                ADAPTIVESTATE = srp.ADAPTIVESTATE,
                                LIVINGCONDITION = srp.LIVINGCONDITION,
                                JOBINFO = srp.JOBINFO,
                                DAYTAKECAREHOUR = srp.DAYTAKECAREHOUR,
                                RELATIVESNEEDCARE = srp.RELATIVESNEEDCARE,
                                REPLACEMENT = srp.REPLACEMENT,
                                EASEPRESSURE = srp.EASEPRESSURE,
                                LIFEQUALITY = srp.LIFEQUALITY,
                                FAMILYEXPECT = srp.FAMILYEXPECT,
                                ORGID = srp.ORGID,
                                DELFLAG = srp.DELFLAG,
                                DELDATE = srp.DELDATE,
                            }).ToList();
          
            response.RecordsCount = q.Count();
            if (pageSize > 0)
            {
                response.Data = q.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                response.PagesCount = GetPagesCount(pageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }

        public BaseResponse<EvalPlan> QuerySwRegEvalPlan(int evalPlanId, int feeNo)
        {
            //BaseResponse<EvalPlanModel> response = new BaseResponse<EvalPlanModel>();
            #region
          
            //SwRegEvalPlanModel sepm = (from srp in unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet.Where(m => m.FEENO == feeNo && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
            //                          select new SwRegEvalPlanModel {
            //                              #region 字段
            //                              EVALPLANID = srp.EVALPLANID,
            //                              FEENO = srp.FEENO,
            //                              RESIDENTNO = srp.RESIDENTNO,
            //                              EVALDATE = srp.EVALDATE,
            //                              EVALNUMBER = srp.EVALNUMBER,
            //                              NEXTEVALDATE = srp.NEXTEVALDATE,
            //                              INDATE = srp.INDATE,
            //                              REGNAME = srp.REGNAME,
            //                              SEX = srp.SEX,
            //                              BIRTHDATE = srp.BIRTHDATE,
            //                              IDNO = srp.IDNO,
            //                              CONTACTNAME = srp.CONTACTNAME,
            //                              CONTACTPHONE = srp.CONTACTPHONE,
            //                              CONTACTMOBILE = srp.CONTACTMOBILE,
            //                              APPELLATION = srp.APPELLATION,
            //                              LIVINGADDRESS = srp.LIVINGADDRESS,
            //                              PTYPE = srp.PTYPE,
            //                              OBSTACLEMANUAL = srp.OBSTACLEMANUAL,
            //                              SOURCETYPE = srp.SOURCETYPE,
            //                              TAKECAREREASON = srp.TAKECAREREASON,
            //                              TAKECARETYPE = srp.TAKECARETYPE,
            //                              SERVICETYPE = srp.SERVICETYPE,
            //                              DISEASEINFO = srp.DISEASEINFO,
            //                              ECOLOGICALMAP = srp.ECOLOGICALMAP,
            //                              PERSONALHISTORY = srp.PERSONALHISTORY,
            //                              PHYSIOLOGY = srp.PHYSIOLOGY,
            //                              PSYCHOLOGY = srp.PSYCHOLOGY,
            //                              FAMILYSUPPORT = srp.FAMILYSUPPORT,
            //                              ECONOMICCAPACITY = srp.ECONOMICCAPACITY,
            //                              SOCIALRESOURCES = srp.SOCIALRESOURCES,
            //                              SOCIALRESOURCE = srp.SOCIALRESOURCE,
            //                              CURRENTSUBSIDY = srp.CURRENTSUBSIDY,
            //                              ASSISTAPPLICATION = srp.ASSISTAPPLICATION,
            //                              MMSE = srp.MMSE,
            //                              ADL = srp.ADL,
            //                              IADL = srp.IADL,
            //                              GDS = srp.GDS,
            //                              EMOTIONSTATE = srp.EMOTIONSTATE,
            //                              BEHAVIOR = srp.BEHAVIOR,
            //                              ATTITUDE = srp.ATTITUDE,
            //                              PAYATTENTION = srp.PAYATTENTION,
            //                              THOUGHT = srp.THOUGHT,
            //                              UNDERSTANDABILITY = srp.UNDERSTANDABILITY,
            //                              SOCIALABILITY = srp.SOCIALABILITY,
            //                              EYESIGHT = srp.EYESIGHT,
            //                              HEARING = srp.HEARING,
            //                              EXPRESSION = srp.EXPRESSION,
            //                              UNDERSTANDING = srp.UNDERSTANDING,
            //                              FAMILYINTERACTION = srp.FAMILYINTERACTION,
            //                              RELATIVEINTERACTION = srp.RELATIVEINTERACTION,
            //                              FRIENDINTERACTION = srp.FRIENDINTERACTION,
            //                              ELDERINTERACTION = srp.ELDERINTERACTION,
            //                              ADAPTIVESTATE = srp.ADAPTIVESTATE,
            //                              LIVINGCONDITION = srp.LIVINGCONDITION,
            //                              JOBINFO = srp.JOBINFO,
            //                              DAYTAKECAREHOUR = srp.DAYTAKECAREHOUR,
            //                              RELATIVESNEEDCARE = srp.RELATIVESNEEDCARE,
            //                              REPLACEMENT = srp.REPLACEMENT,
            //                              EASEPRESSURE = srp.EASEPRESSURE,
            //                              LIFEQUALITY = srp.LIFEQUALITY,
            //                              FAMILYEXPECT = srp.FAMILYEXPECT,
            //                              ORGID = srp.ORGID,
            //                              DELFLAG = srp.DELFLAG,
            //                              DELDATE = srp.DELDATE
            //                              #endregion
            //                          }).FirstOrDefault();

            //response.Data.swRegEvalPlanModel = sepm;
            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("SELECT * FROM DC_TASKGOALSSTRATEGY WHERE EVALPLANID={0}  ORDER BY ID DESC ", sepm.EVALPLANID);

            //using (TWSLTCContext context = new TWSLTCContext())
            //{

            //    List<DC_TaskGoalsStrategyModel> tsm = new List<DC_TaskGoalsStrategyModel>();

            //    IList<DC_TASKGOALSSTRATEGY> ts = context.Database.SqlQuery<DC_TASKGOALSSTRATEGY>(sb.ToString()).ToList();

            //    Mapper.CreateMap<DC_SWREGEVALPLAN, SwRegEvalPlanModel>();

            //    Mapper.Map(ts, tsm);

            //    response.Data.TaskGoalsStrategyModel = tsm;

            //    sb.Clear();
            //    sb.Remove(0, sb.Length);

            //}
  #endregion
            BaseResponse<EvalPlan> response = new BaseResponse<EvalPlan>();

            EvalPlan eplan = new EvalPlan();

            SwRegEvalPlan ls = new SwRegEvalPlan();

            StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("SELECT TOP 1 * FROM DC_SWREGEVALPLAN WHERE FEENO={0} AND ORGID='{1}' ", feeNo, SecurityHelper.CurrentPrincipal.OrgId);
            //MySql不支持Top 1 只能写limit 1
            sb.AppendFormat("SELECT  * FROM `DC_SWREGEVALPLAN` WHERE FEENO={0} AND ORGID='{1}'", feeNo, SecurityHelper.CurrentPrincipal.OrgId);
            if (evalPlanId > 0)
                sb.AppendFormat(" AND EVALPLANID={0}", evalPlanId);
            sb.Append(" ORDER BY EVALPLANID DESC LIMIT 1");
            using (TWSLTCContext context = new TWSLTCContext())
            {

                DC_SWREGEVALPLAN plan = context.Database.SqlQuery<DC_SWREGEVALPLAN>(sb.ToString()).FirstOrDefault();
                Mapper.CreateMap<DC_SWREGEVALPLAN, SwRegEvalPlan>();
                Mapper.Map(plan, ls);
        
                sb.Clear();
                sb.Remove(0, sb.Length);
                if (ls!=null)
                {
                    eplan.swRegEvalPlanModel = ls;  
                    List<DC_TaskGoalsStrategyModel> qim = new List<DC_TaskGoalsStrategyModel>();
                    sb.AppendFormat("SELECT EVALPLANID,QUESTIONTYPE,CPDIA,QUESTIONDESC,TREATMENTGOAL,QUESTIONANALYSIS,PLANACTIVITY,RECDATE,RECORDBY,CHECKDATE,CHECKEDBY,UNFINISHREASON,ID,MAJORTYPE,(SELECT ITEMNAME FROM DC_COMMDTL WHERE ITEMTYPE='DC02.049' AND ITEMCODE=EVALUATIONVALUE) AS EVALUATIONVALUE FROM DC_TASKGOALSSTRATEGY WHERE EVALPLANID={0}", ls.EVALPLANID);

                    List<DC_TASKGOALSSTRATEGY> items = context.Database.SqlQuery<DC_TASKGOALSSTRATEGY>(sb.ToString()).ToList();
                    Mapper.CreateMap<DC_TASKGOALSSTRATEGY, DC_TaskGoalsStrategyModel>();
                    Mapper.Map(items, qim);
                    if (items != null)
                    {
                        eplan.TaskGoalsStrategyModel= qim;
                        sb.Clear();
                        sb.Remove(0, sb.Length);
                    }
                }
                response.Data = eplan;
            }
            #region
            
            //取得最新一条记录
          

            //List<DC_TaskGoalsStrategyModel> TaskGoalsStrategyModel = (from ttr in unitOfWork.GetRepository<DC_TASKGOALSSTRATEGY>().dbSet.Where(p => p.EVALPLANID == response.Data.swRegEvalPlanModel.EVALPLANID)
            //                                        join cp in unitOfWork.GetRepository<DC_CAREPLANPROBLEM>().dbSet on ttr.MAJORTYPE equals cp.CPNO into tks
            //                                        from tv in tks.DefaultIfEmpty()
            //                                        select new DC_TaskGoalsStrategyModel
            //                                        {
            //                                            MAJORTYPE = tv.CPNO,
            //                                            MAJORNAME = tv.DIAPR,
            //                                            TREATMENTGOAL = ttr.TREATMENTGOAL,
            //                                            EVALUATIONVALUE = ttr.EVALUATIONVALUE,
            //                                            ID = ttr.ID,
            //                                            CPDIA = ttr.CPDIA,
            //                                            QUESTIONDESC = ttr.QUESTIONDESC,
            //                                            QUESTIONANALYSIS = ttr.QUESTIONANALYSIS,
            //                                            PLANACTIVITY = ttr.PLANACTIVITY,
            //                                            //FEENO = sepm.FEENO,
            //                                            EVALPLANID = ttr.EVALPLANID,
            //                                            //EVALDATE = sepm.EVALDATE,
            //                                            UNFINISHREASON = ttr.UNFINISHREASON
            //                                        }).ToList();
            //if(sem!=null)
            //    response.Data.swRegEvalPlanModel = sem;
            //if(t!=null)
            //    response.Data.TaskGoalsStrategyModel = t.ToList();
           #endregion 
            return response;
        }

        /// <summary>
        /// 获取社工个案评估及处遇计划单笔记录
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<DC_SwRegEvalPlanModel> GetSwRegEvalPlanById(int evalPlanId)
        {
            return base.Get<DC_SWREGEVALPLAN, DC_SwRegEvalPlanModel>((q) => q.EVALPLANID == evalPlanId);
        }

        public BaseResponse<IList<DC_TaskGoalsStrategyModel>> GetTaskGoalyssTrategyById(int evalplanId,int feeNo)
        {
            BaseResponse<IList<DC_TaskGoalsStrategyModel>> response = new BaseResponse<IList<DC_TaskGoalsStrategyModel>>();
            if (evalplanId > 0 || evalplanId==-1)
            {
                
                var q = from tk in unitOfWork.GetRepository<DC_TASKGOALSSTRATEGY>().dbSet //where tk.EVALPLANID==evalplanId
                        join swr in unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet on tk.EVALPLANID equals swr.EVALPLANID into swr_res
                        from swrs in swr_res.DefaultIfEmpty()
                        //join cp in unitOfWork.GetRepository<DC_CAREPLANPROBLEM>().dbSet on tk.MAJORTYPE equals cp.CPNO into tks
                        //from tv in tks.DefaultIfEmpty()
                        select new DC_TaskGoalsStrategyModel
                        {
                            //MAJORTYPE = tv.CPNO,
                            //MAJORNAME = tv.DIAPR,
                            QUESTIONTYPE=tk.QUESTIONTYPE,
                            MAJORTYPE=tk.MAJORTYPE,
                            TREATMENTGOAL = tk.TREATMENTGOAL,
                            EVALUATIONVALUE = tk.EVALUATIONVALUE,
                            ID = tk.ID,
                            CPDIA = tk.CPDIA,
                            QUESTIONDESC = tk.QUESTIONDESC,
                            QUESTIONANALYSIS = tk.QUESTIONANALYSIS,
                            PLANACTIVITY = tk.PLANACTIVITY,
                            FEENO = swrs.FEENO,
                            EVALPLANID=tk.EVALPLANID,
                            EVALDATE=swrs.EVALDATE,
                            UNFINISHREASON=tk.UNFINISHREASON
                        };
                if (evalplanId > 0)
                {
                    q = q.Where(m => m.EVALPLANID == evalplanId);
                }
                if (feeNo > 0)
                    q = q.Where(p => p.FEENO == feeNo);
                q = q.OrderByDescending(m => m.ID);
                response.RecordsCount = q.Count();
                response.Data = q.ToList();
               
            }
            return response;
        }

        /// <summary>
        /// 保存社工个案评估及处遇计划
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_SwRegEvalPlanModel> SaveSwRegEvalPlan(DC_SwRegEvalPlanModel request)
        {
            return base.Save<DC_SWREGEVALPLAN, DC_SwRegEvalPlanModel>(request, (q) => q.EVALPLANID == request.EVALPLANID);
           
        }

        /// <summary>
        /// 删除社工个案评估及处遇计划
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeleteSwRegEvalPlanById(int evalPlanId)
        {
            BaseResponse response = new BaseResponse();
            unitOfWork.BeginTransaction();

            var model = unitOfWork.GetRepository<DC_TASKGOALSSTRATEGY>().dbSet.Where(m => m.EVALPLANID == evalPlanId);
            if (model != null)
            {
                foreach (DC_TASKGOALSSTRATEGY item in model)
                {
                    unitOfWork.GetRepository<DC_TASKGOALSSTRATEGY>().Delete(item.ID);
                }
            }
 
            base.Delete<DC_SWREGEVALPLAN>(evalPlanId);
            //unitOfWork.GetRepository<DC_TASKGOALSSTRATEGY>().Delete(evalPlanId);
            //unitOfWork.Save();

            //unitOfWork.GetRepository<DC_SWREGEVALPLAN>().Delete(evalPlanId);
            //unitOfWork.Save();
            unitOfWork.Commit();
            return response;
            //return base.Delete<DC_SWREGEVALPLAN>(evalPlanId);
        }
		/// <summary>
		/// 删除社工填写的计划
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public BaseResponse DeleteTaskGoalyssTrategyById(int id) {
			return base.Delete<DC_TASKGOALSSTRATEGY>(id);
		}
        /// <summary>
        /// 保存社工个案计划评值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_TaskGoalsStrategyModel> SaveTaskGoalssTrategy(DC_TaskGoalsStrategyModel request)
        {
            return base.Save<DC_TASKGOALSSTRATEGY, DC_TaskGoalsStrategyModel>(request, (q) => q.ID == request.ID);
        }

        #region 调用评估值(ADL,IADL,MMSE...)
        public BaseResponse<DC_EvalQeustionModel> GetEvalQuestionVal(int feeNo, int number, string type)
        {
            BaseResponse<DC_EvalQeustionModel> response = new BaseResponse<DC_EvalQeustionModel>();
            StringBuilder sb = new StringBuilder();
            using (TWSLTCContext context = new TWSLTCContext())
            {
                List<DC_QuestionItemModel> qim = new List<DC_QuestionItemModel>();
                sb.AppendFormat("SELECT (SELECT SCORE FROM DC_EVALQUESTION WHERE FEENO={0} AND QUESTIONCODE='ADL' AND ORGID='{1}' AND EVALNUMBER={2} ORDER BY ID DESC LIMIT 1) AS ADL,", feeNo, SecurityHelper.CurrentPrincipal.OrgId, number);
                sb.AppendFormat("(SELECT  SCORE FROM DC_EVALQUESTION WHERE FEENO={0} AND QUESTIONCODE='IADL' AND ORGID='{1}' AND EVALNUMBER={2} ORDER BY ID DESC LIMIT 1) AS IADL,", feeNo, SecurityHelper.CurrentPrincipal.OrgId, number);
                sb.AppendFormat("(SELECT  SCORE FROM DC_EVALQUESTION WHERE FEENO={0} AND QUESTIONCODE='MMSE' AND ORGID='{1}' AND EVALNUMBER={2} ORDER BY ID DESC LIMIT 1) AS MMSE", feeNo, SecurityHelper.CurrentPrincipal.OrgId, number);
                sb.AppendFormat(",(SELECT SCORE FROM DC_EVALQUESTION WHERE FEENO={0} AND QUESTIONCODE='GDS' AND ORGID='{1}' AND EVALNUMBER=1 ORDER BY ID DESC LIMIT 1) AS GDS", feeNo, SecurityHelper.CurrentPrincipal.OrgId, number);
                DC_EvalQeustionModel items = context.Database.SqlQuery<DC_EvalQeustionModel>(sb.ToString()).FirstOrDefault();
                sb.Clear();
                sb.Remove(0, sb.Length);
                response.Data = items;
            }
            
            
            return response;
        }
        #endregion

        public BaseResponse<IList<DC_TaskGoalsStrategyModel>> CheckAddRec(int feeNo, int number)
        {
            BaseResponse<IList<DC_TaskGoalsStrategyModel>> response = new BaseResponse<IList<DC_TaskGoalsStrategyModel>>();

            var q = from p in unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet.Where(m=>m.FEENO==feeNo && m.EVALNUMBER==number && m.ORGID==SecurityHelper.CurrentPrincipal.OrgId)
                    join t in unitOfWork.GetRepository<DC_TASKGOALSSTRATEGY>().dbSet on p.EVALPLANID equals t.EVALPLANID into tst
                    from ts in tst.DefaultIfEmpty()
                    select new DC_TaskGoalsStrategyModel
                    {
                        EVALUATIONVALUE=ts.EVALUATIONVALUE,
                        EVALNUMBER=p.EVALNUMBER,
                        ID=ts.ID
                    };
            q = q.OrderByDescending(m => m.ID);
            response.Data = q.ToList();
            return response;

        }

        public int? GetMaxNumber(long? feeNo, string orgId)
        {
            BaseResponse<DC_SwRegEvalPlanModel> response = new BaseResponse<DC_SwRegEvalPlanModel>();
            response.Data = (from d in unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet.Where(m => m.FEENO == feeNo && m.ORGID == orgId).OrderByDescending(m => m.EVALPLANID) select new DC_SwRegEvalPlanModel { EVALNUMBER = d.EVALNUMBER }).FirstOrDefault();
            if (response.Data != null)
            {
                return response.Data.EVALNUMBER;
            }
            return 0;
        }

        #endregion

        #region*****************日照部分--家庭照顾者生活品质评估问卷*************

        /// <summary>
        /// 获取家庭照顾者生活品质评估问卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_RegLifeQualityEvalModel>> QueryRegLifeQualityEval(BaseRequest<DC_RegLifeQualityEvalFilter> request)
        {
			BaseResponse<IList<DC_RegLifeQualityEvalModel>> response = new BaseResponse<IList<DC_RegLifeQualityEvalModel>>();
			Mapper.CreateMap<DC_REGLIFEQUALITYEVAL, DC_RegLifeQualityEvalModel>();

			var q = from rl in unitOfWork.GetRepository<DC_REGLIFEQUALITYEVAL>().dbSet.Where(m=>m.ORGID==SecurityHelper.CurrentPrincipal.OrgId) select rl;

			if (request != null && request.Data._id > 0) {
				q = q.Where(m => m.ID == request.Data._id);
			}
			if (request != null && request.Data._feeno > 0) {
				q = q.Where(m => m.FEENO == request.Data._feeno);
			}
			q = q.OrderByDescending(m => m.ID);
			response.RecordsCount = q.Count();
			List<DC_REGLIFEQUALITYEVAL> list = null;
			if (response.RecordsCount > 0) {
				if (request != null && request.PageSize > 0) {
					list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
					response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
				}
				else {
					list = q.ToList();
				}
				response.Data = Mapper.Map<IList<DC_RegLifeQualityEvalModel>>(list);
			}
            return response;


			//BaseResponse<IList<DC_RegLifeQualityEvalModel>> response = new BaseResponse<IList<DC_RegLifeQualityEvalModel>>();
			//Mapper.CreateMap<DC_REGLIFEQUALITYEVAL, DC_RegLifeQualityEvalModel>();
			//var q = 
        }

        /// <summary>
        /// 获取家庭照顾者生活品质评估问卷单笔记录
        /// </summary>
        /// <param name="evalPlanId"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegLifeQualityEvalModel> GetRegLifeQualityEvalById(int id)
        {
            return base.Get<DC_REGLIFEQUALITYEVAL, DC_RegLifeQualityEvalModel>((q) => q.ID == id);
        }
        /// <summary>
        /// 保存家庭照顾者生活品质评估问卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegLifeQualityEvalModel> SaveRegLifeQualityEval(DC_RegLifeQualityEvalModel request)
        {
            return base.Save<DC_REGLIFEQUALITYEVAL, DC_RegLifeQualityEvalModel>(request, (q) => q.ID == request.Id);
        }


        /// <summary>
        /// 删除家庭照顾者生活品质评估问卷
        /// </summary>
        /// <param name="evalPlanId"></param>
        /// <returns></returns>
        public BaseResponse DeleteRegLifeQualityEvalById(int id)
        {
            return base.Delete<DC_REGLIFEQUALITYEVAL>(id);
        }

        #endregion

        #region*******************日照部分--受托长辈适应程度评估表***************

        /// <summary>
        /// 获取所有问题列表,第一次评估时调用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_QuestionModel>> QueryQuestion(string orgId)
        {

            BaseResponse<IList<DC_QuestionModel>> response = new BaseResponse<IList<DC_QuestionModel>>();
            List<DC_QuestionModel> qm = new List<DC_QuestionModel>();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM DC_QUESTION WHERE ORGID='{0}'",SecurityHelper.CurrentPrincipal.OrgId);

            using (TWSLTCContext context = new TWSLTCContext())
            {
                List<DC_QUESTION> questions = context.Database.SqlQuery<DC_QUESTION>(sb.ToString()).ToList();
                Mapper.CreateMap<DC_QUESTION, DC_QuestionModel>();
                Mapper.Map(questions, qm);

                sb.Clear();
                sb.Remove(0, sb.Length);
                for (int i = 0; i < qm.Count; i++)
                {
                    DC_QuestionModel _q = qm[i];

                    List<DC_QuestionItemModel> qim = new List<DC_QuestionItemModel>();
                    sb.AppendFormat("SELECT * FROM DC_QUESTIONITEM WHERE QUESTIONID={0} ", _q.QUESTIONID);
                    // sb.AppendFormat("SELECT * FROM DC_QUESTIONITEM A LEFT JOIN DC_REGQUESTIONDATA B ON A.ITEMID=B.ITEMID  WHERE QUESTIONID={0} ", _q.QUESTIONID);
                    //if(feeNo>0)
                    //    sb.AppendFormat(" AND ")
                    List<DC_QUESTIONITEM> items = context.Database.SqlQuery<DC_QUESTIONITEM>(sb.ToString()).ToList();
                    Mapper.CreateMap<DC_QUESTIONITEM, DC_QuestionItemModel>();
                    Mapper.Map(items, qim);
                    if (items != null)
                    {
                        qm[i].QuestionItem = qim;
                        sb.Clear();
                        sb.Remove(0, sb.Length);
                    }


                    List<DC_QuestionValueModel> questionValue = new List<DC_QuestionValueModel>();
                    sb.AppendFormat("SELECT * FROM DC_QUESTIONVALUE WHERE QUESTIONID={0}", _q.QUESTIONID);
                    List<DC_QUESTIONVALUE> val = context.Database.SqlQuery<DC_QUESTIONVALUE>(sb.ToString()).ToList();
                    Mapper.CreateMap<DC_QUESTIONVALUE, DC_QuestionValueModel>();
                    Mapper.Map(val, questionValue);
                    qm[i].QuestionValue = questionValue;
                    sb.Clear();
                    sb.Remove(0, sb.Length);
                }
            }
            response.Data = qm.ToList();
            return response;
        }

        public BaseResponse<IList<DC_QuestionModel>> QueryQuestion(int feeNo, string orgId)
        {
            BaseResponse<IList<DC_QuestionModel>> response = new BaseResponse<IList<DC_QuestionModel>>();
            var rec = (from qrec in unitOfWork.GetRepository<DC_REGQUESTIONEVALREC>().dbSet.Where(m => m.FEENO == feeNo).OrderByDescending(m => m.EVALRECID) select new DC_RegQuestionEvalRecModel { EvalRecId = qrec.EVALRECID }).FirstOrDefault();

            if (rec!=null)
            {
                response = QueryQuestion(feeNo, orgId, rec.EvalRecId);
            }
            else
            {
                response = QueryQuestion(orgId);
            }
            return response;
        }
        /// <summary>
        /// 加载问题列表最新一条带有评估项值
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_QuestionModel>> QueryQuestion(int feeNo, string orgId, int? evalRecId)
        {
            BaseResponse<IList<DC_QuestionModel>> response = new BaseResponse<IList<DC_QuestionModel>>();
            #region
            List<DC_QuestionModel> qm = new List<DC_QuestionModel>();
			
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM DC_QUESTION WHERE ORGID='{0}'",SecurityHelper.CurrentPrincipal.OrgId);

            using (TWSLTCContext context = new TWSLTCContext())
            {
                List<DC_QUESTION> questions = context.Database.SqlQuery<DC_QUESTION>(sb.ToString()).ToList();
                Mapper.CreateMap<DC_QUESTION, DC_QuestionModel>();
                Mapper.Map(questions, qm);

                sb.Clear();
                sb.Remove(0, sb.Length);
                for (int i = 0; i < qm.Count; i++)
                {
					
                    DC_QuestionModel _q = qm[i];

                    List<DC_QuestionItemModel> qim = new List<DC_QuestionItemModel>();

                    var data = from _rec in unitOfWork.GetRepository<DC_REGQUESTIONEVALREC>().dbSet.Where(m => m.EVALRECID == evalRecId && m.FEENO == feeNo)
                               join regQuestion in unitOfWork.GetRepository<DC_REGEVALQUESTION>().dbSet.Where(m => m.QUESTIONID == _q.QUESTIONID) on _rec.EVALRECID equals regQuestion.EVALRECID into questionlist
                               from _regQuestion in questionlist.DefaultIfEmpty()
                               join questionData in unitOfWork.GetRepository<DC_REGQUESTIONDATA>().dbSet on _regQuestion.SEQ equals questionData.SEQ into questiondata
                               from _questionData in questiondata.DefaultIfEmpty()
                               join questionItem in unitOfWork.GetRepository<DC_QUESTIONITEM>().dbSet on _questionData.ITEMID equals questionItem.ITEMID into qitem
                               from _questionItem in qitem.DefaultIfEmpty()
                               select new DC_QuestionItemModel
                               {
                                   ITEMID = _questionData.ITEMID,
                                   ITEMVALUE = _questionData.ITEMVALUE,
                                   QUESTIONID = _questionItem.QUESTIONID,
								   QUESTIONCODE = _q.QUESTIONCODE,
                                   FEENO = feeNo,
                                   EVALRECID = _rec.EVALRECID,
                                   ID = _questionData.ID,
                                   SEQ = _questionData.SEQ,
                                   ITEMNAME = _questionItem.ITEMNAME,
                                   SHOWNUMBER = _questionItem.SHOWNUMBER
                                   
                               };
                    // data.Where(m=>m.QUESTIONID==_q.QUESTIONID);
                    qim = data.ToList();
                    if (qim.Count > 0)
                    {
                        qm[i].QuestionItem = qim;
                    }

                    List<DC_QuestionValueModel> questionValue = new List<DC_QuestionValueModel>();
                    sb.AppendFormat("SELECT * FROM DC_QUESTIONVALUE WHERE QUESTIONID={0}", _q.QUESTIONID);
                    var dataVal = from vl in unitOfWork.GetRepository<DC_QUESTIONVALUE>().dbSet.Where(m => m.QUESTIONID == _q.QUESTIONID)
                                  select vl;
                    //List<DC_QUESTIONVALUE> val = context.Database.SqlQuery<DC_QUESTIONVALUE>(sb.ToString()).ToList();
                    Mapper.CreateMap<DC_QUESTIONVALUE, DC_QuestionValueModel>();
                    Mapper.Map(dataVal, questionValue);
                    qm[i].QuestionValue = questionValue;
                    sb.Clear();
                    sb.Remove(0, sb.Length);
                }
            }
            response.Data = qm.ToList();
            return response;
            #endregion
        }
       
        /// <summary>
        /// 保存受托长辈适应程度评估
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegQuestionEvalRecModel> SaveRegQuestionItem(List<DC_QuestionModel> request)
        {
            BaseResponse<DC_RegQuestionEvalRecModel> response = new BaseResponse<DC_RegQuestionEvalRecModel>();
            try
            {
                

                if (request != null && request.Count > 0)
                {
                    //unitOfWork.BeginTransaction();//开始事务

                    //Step1:插入主表(DC_REGQUESTIONEVALREC)
                    DC_RegQuestionEvalRecModel evalRec = new DC_RegQuestionEvalRecModel
                    {
                        EvalRecId = request[0].QuestionItem[0].EVALRECID,
                        FeeNo = request[0].FEENO
                        ,
                        Score = request[0].SCORE
                        ,
                        EvalResult = request[0].EVALRESULT
                        ,
                        EvalDate = DateTime.Now
                        ,
                        CreateBy = SecurityHelper.CurrentPrincipal.EmpNo
                        ,
                        OrgId = SecurityHelper.CurrentPrincipal.OrgId
                        ,
                        CreateDate = DateTime.Now
                    };

                    Mapper.CreateMap<DC_RegQuestionEvalRecModel, DC_REGQUESTIONEVALREC>();

                    var model = unitOfWork.GetRepository<DC_REGQUESTIONEVALREC>().dbSet.Where(m => m.EVALRECID == evalRec.EvalRecId).FirstOrDefault();
                    if (model == null)
                    {
                        model = Mapper.Map<DC_REGQUESTIONEVALREC>(evalRec);
                        unitOfWork.GetRepository<DC_REGQUESTIONEVALREC>().Insert(model);
                        unitOfWork.Save();
						evalRec.EvalRecId = model.EVALRECID;
                        //插入两张子表
                        if (model.EVALRECID > 0)
                        {
                            #region
                            foreach (DC_QuestionModel question in request)
                            {
                                //Step2:记录与问题关联表
                                List<DC_REGEVALQUESTION> qList = new List<DC_REGEVALQUESTION>();

                                List<DC_QuestionItemModel> qItemList = question.QuestionItem;

                                foreach (DC_QuestionItemModel qItem in qItemList)
                                {
                                    //插入记录表与问题项表的关联表
                                    DC_RegEvalQuestionModel regEvalQuestion = new DC_RegEvalQuestionModel
                                    {
                                        EVALRECID = model.EVALRECID,
                                        QUESTIONID = qItem.QUESTIONID
                                    };


                                    Mapper.CreateMap<DC_RegEvalQuestionModel, DC_REGEVALQUESTION>();

                                    DC_REGEVALQUESTION evalQuestion = Mapper.Map<DC_REGEVALQUESTION>(regEvalQuestion);

                                    unitOfWork.GetRepository<DC_REGEVALQUESTION>().Insert(evalQuestion);
                                    unitOfWork.Save();


                                    //Step3:插入各项问题值表
                                    DC_RegQuestionDataModel qData = new DC_RegQuestionDataModel
                                    {
                                        ITEMID = qItem.ITEMID
                                        ,
                                        ITEMVALUE = qItem.ITEMVALUE
                                        ,
                                        SEQ = evalQuestion.SEQ
										,EVALRECID=model.EVALRECID
                                    };
                                    Mapper.CreateMap<DC_RegQuestionDataModel, DC_REGQUESTIONDATA>();

                                    DC_REGQUESTIONDATA evalQuestionData = Mapper.Map<DC_REGQUESTIONDATA>(qData);

                                    unitOfWork.GetRepository<DC_REGQUESTIONDATA>().Insert(evalQuestionData);
                                    unitOfWork.Save();
                                }
                            }
                            #endregion

                        }
                    }
                    else
                    {
                        unitOfWork.BeginTransaction();

                        unitOfWork.GetRepository<DC_REGQUESTIONEVALREC>().Update(model);

                        foreach (DC_QuestionModel item in request)
                        {
                            foreach (DC_QuestionItemModel sub in item.QuestionItem)
                            {
                                if (sub.ITEMVALUE != null)
                                {
                                    DC_REGQUESTIONDATA data = new DC_REGQUESTIONDATA
                                    {
                                        ID = sub.ID,
                                        SEQ = sub.SEQ,
                                        ITEMID = sub.ITEMID,
                                        ITEMVALUE = sub.ITEMVALUE
                                    };
                                    unitOfWork.GetRepository<DC_REGQUESTIONDATA>().Update(data);
                                }

                            }

                        }
                        unitOfWork.Commit();
                    }
                    response.Data = evalRec;
                    response.ResultCode = 0;

                    //return response;
                }
            }
            catch (Exception ex)
            {
                response.ResultMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 根据问题ID获取所属分值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_QuestionValueModel>> QueryQuestionValue(BaseRequest<DC_QuestionValueFilter> request)
        {
            BaseResponse<IList<DC_QuestionValueModel>> response = new BaseResponse<IList<DC_QuestionValueModel>>();
            var q = from m in unitOfWork.GetRepository<DC_QUESTIONVALUE>().dbSet
                    select new DC_QuestionValueModel
                    {
                        QUESTIONID = m.QUESTIONID,
                        VALUE = m.VALUE,
                        VALUEID = m.VALUEID
                    };
            if (request.Data.QUESTIONID > 0)
                q = q.Where(m => m.QUESTIONID == request.Data.QUESTIONID);
            response.RecordsCount = q.Count();
            response.Data = q.ToList();
            return response;
        }
        /// <summary>
        /// 获取指定questionId问题
        /// </summary>
        /// <param name="evalPlanId"></param>
        /// <returns></returns>
        public BaseResponse<DC_QuestionModel> GetQuestionById(int questionId)
        {
            return base.Get<DC_QUESTION, DC_QuestionModel>((q) => q.QUESTIONID == questionId);
        }

        /// <summary>
        /// 评估历史记录
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_RegQuestionEvalRecModel>> QueryRegQuestionHistory(int feeNo)
        {
            BaseResponse<IList<DC_RegQuestionEvalRecModel>> response = new BaseResponse<IList<DC_RegQuestionEvalRecModel>>();
            response.Data = (from regRec in unitOfWork.GetRepository<DC_REGQUESTIONEVALREC>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId && m.FEENO == feeNo).OrderByDescending(m => m.EVALRECID)
                             select new DC_RegQuestionEvalRecModel { 
                                EvalRecId=regRec.EVALRECID,
                                EvalDate=regRec.EVALDATE,
                                EvalResult=regRec.EVALRESULT,
                                Score=regRec.SCORE
                             }).ToList();
          
            return response;
        }
        #endregion
    }
}

