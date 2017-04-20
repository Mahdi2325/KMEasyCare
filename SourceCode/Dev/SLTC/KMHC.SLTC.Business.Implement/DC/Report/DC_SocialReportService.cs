using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC.Report;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.DC.Report
{
   public class DC_SocialReportService : BaseService, IDC_SocialReportService
   {
       #region 个案生活史
       /// <summary>
       /// 获取个案生活史
       /// </summary>
       /// <param name="regNo"></param>
       /// <returns></returns>
       public BaseResponse<DC_LifeHistoryModel> GetLifeHistoryById(int id)
       {
           BaseResponse<DC_LifeHistoryModel> response = new BaseResponse<DC_LifeHistoryModel>();
           response.Data = (from history in unitOfWork.GetRepository<DC_LIFEHISTORY>().dbSet.Where(o => o.Id == id)
                            join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on history.CREATEBY equals e.EMPNO into em
                            from emp in em.DefaultIfEmpty()
                            select new DC_LifeHistoryModel()
                           {
                                Id = history.Id,
                                FeeNo = history.FEENO,
                                Name=history.NAME,
                                NickName=history.NICKNAME,
                                Nick = history.NICKNAME,
                                ResidentNo=history.RESIDENTNO,
                                BirthPlace=history.BIRTHPLACE,
                                FamilyEnvironment=history.FAMILYENVIRONMENT,
                                ChildHoodExperience=history.CHILDHOODEXPERIENCE,
                                School=history.SCHOOL,
                                ProudDeeds=history.PROUDDEEDS,
                                Romance=history.ROMANCE,
                                MerryInfo=history.MERRYINFO,
                                MportantPeople=history.MPORTANTPEOPLE,
                                WorkHistory=history.WORKHISTORY,
                                ServiceHistory=history.SERVICEHISTORY,
                                Religious=history.RELIGIOUS,
                                Living=history.LIVING,
                                PositivePersonality=history.POSITIVEPERSONALITY,
                                NegativePersonality=history.NEGATIVEPERSONALITY,
                                FamilyTroubled=history.FAMILYTROUBLED,
                                SoothingEmotion=history.SOOTHINGEMOTION,
                                Skill=history.SKILL,
                                FavoriteDress=history.FAVORITEDRESS,
                                Foodlike=history.FOODLIKE,
                                Animallike=history.ANIMALLIKE,
                                HolidayActivity=history.HOLIDAYACTIVITY,
                                NotlikeThings=history.NOTLIKETHINGS,
                                InterestedThings=history.INTERESTEDTHINGS, 
                                CreateBy=emp.EMPNAME
                           }).FirstOrDefault();
           return response;
       }
       
       #endregion

       #region　个案转介
       /// <summary>
       /// 个案转介单
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public BaseResponse<DC_ReferrallistsModel> GetReferralById(int id)
       {
           BaseResponse<DC_ReferrallistsModel> response = new BaseResponse<DC_ReferrallistsModel>();

           response.Data = (from reff in unitOfWork.GetRepository<DC_REFERRALLISTS>().dbSet.Where(m => m.ID == id && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
                   join reg_ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet on reff.FEENO equals reg_ipd.FEENO into reg_ipds
                   from regipd in reg_ipds.DefaultIfEmpty()
                   join reg_file in unitOfWork.GetRepository<DC_REGFILE>().dbSet on regipd.REGNO equals reg_file.REGNO into reg_files
                   from regfile in reg_files.DefaultIfEmpty()
                   join org in unitOfWork.GetRepository<LTC_ORG>().dbSet on reff.ORGID equals org.ORGID into orgs
                   from _org in orgs.DefaultIfEmpty()
                   
                    //join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on history.CREATEBY equals e.EMPNO into em
                    //from emp in em.DefaultIfEmpty()
                   select new DC_ReferrallistsModel
                   {
                       Id = reff.ID,
                       FeeNo = reff.FEENO,
                       OrgId = reff.ORGID,
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
                       CreateDate = reff.CREATEDATE,
                       RegName = regfile.REGNAME,//个案姓名
                       SuretyName = regfile.SURETYNAME,//主要联系人
                       SuretyPhone = regfile.SURETYPHONE,//主要联系人电话
                       Sex = regfile.SEX,//性别
                       InDate = regipd.INDATE,//收案日期
                       OrgName=_org.ORGNAME,
                       OriginPlace=regfile.ORIGINPLACE,
                       Phone=regfile.PHONE,
                       PermanentAddress=regfile.PERMANENTADDRESS,
                       LivingAddress=regfile.LIVINGADDRESS,
                       Language=regfile.LANGUAGE,
                       MerryState=regfile.MERRYSTATE,
                       ObstacleManual=regfile.OBSTACLEMANUAL,
                       DiseaseInfo=regfile.DISEASEINFO,
                       No=regfile.IDNO,
                       BirthDate=regfile.BIRTHDATE,
                       Religion=regfile.RELIGION
                   }).FirstOrDefault();
           response.Data.Age = (DateTime.Now.Year - Convert.ToDateTime(response.Data.BirthDate).Year);
           return response;
       }
       #endregion

       /// <summary>
       /// 受托长辈适应程度评估表
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public BaseResponse<List<DC_RegQuestionDataModel>> GetRegQuestionEvalRec(int id,int qId)
       {
           BaseResponse<List<DC_RegQuestionDataModel>> response = new BaseResponse<List<DC_RegQuestionDataModel>>();

           response.Data = (from q in unitOfWork.GetRepository<DC_REGQUESTIONEVALREC>().dbSet.Where(m => m.EVALRECID == id && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
                            join qi in unitOfWork.GetRepository<DC_REGEVALQUESTION>().dbSet.Where(m => m.QUESTIONID == qId) on q.EVALRECID equals qi.EVALRECID into qis
                            from qs in qis.DefaultIfEmpty()
                            join qd in unitOfWork.GetRepository<DC_REGQUESTIONDATA>().dbSet on qs.SEQ equals qd.SEQ into qds
                            from qdata in qds.DefaultIfEmpty()
                            join it in unitOfWork.GetRepository<DC_QUESTIONITEM>().dbSet on qdata.ITEMID equals it.ITEMID into its
                            from item in its.DefaultIfEmpty()
                            join ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet on q.FEENO equals ipd.FEENO into i
                            from ipds in i.DefaultIfEmpty()
                            join reg in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipds.REGNO equals reg.REGNO into regs
                            from rgs in regs.DefaultIfEmpty()

                            select new DC_RegQuestionDataModel
                            {
                                QUESTIONID = qs.QUESTIONID,
                                SEQ = qdata.SEQ,
                                ITEMID = qdata.ITEMID,
                                ITEMVALUE = qdata.ITEMVALUE,
                                EVALRECID = q.EVALRECID,
                                INDATE = ipds.INDATE,
                                RESIDENTNO = ipds.RESIDENTNO,
                                NAME = rgs.REGNAME,
                                ITEMNAME = item.ITEMNAME,
                                SHOWNUMBER = item.SHOWNUMBER,
                                EVALRESULT = q.EVALRESULT,
                                SCORE = q.SCORE,
                                FEENO = q.FEENO

                            }).ToList();
           //if (qId > 0)
           //{
           //    response.Data.Where(m => m.QUESTIONID == qId);
           //}
           response.Data.OrderBy(m => m.SHOWNUMBER);
           return response;
       }

       /// <summary>
       /// 收案
       /// </summary>
       /// <param name="regNo"></param>
       /// <returns></returns>
       public BaseResponse<DC_IpdRegModel> GetIpdRegInById(int id)
       {
           BaseResponse<DC_IpdRegModel> response = new BaseResponse<DC_IpdRegModel>();
           response.Data = (from ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(m => m.FEENO == id)
                            join ipd_file in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipd.REGNO equals ipd_file.REGNO into ipd_files
                            from ipdFile in ipd_files.DefaultIfEmpty()
                            join orgs in unitOfWork.GetRepository<LTC_ORG>().dbSet on ipd.ORGID equals orgs.ORGID into o
                            from org in o.DefaultIfEmpty()
                            join c in unitOfWork.GetRepository<DC_COMMDTL>().dbSet.Where(m=>m.ITEMTYPE=="DC02.048") on ipd.STATIONCODE equals c.ITEMCODE into com
                            from comm in com.DefaultIfEmpty()
                            join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.SOCIALWORKER equals e.EMPNO into emp
                            from em in emp.DefaultIfEmpty()
                            join e1 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NURSEAIDES equals e1.EMPNO into emp1
                            from em1 in emp1.DefaultIfEmpty()
                            join e2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NURSENO equals e2.EMPNO into emp2
                            from em2 in emp2.DefaultIfEmpty()
                            select new DC_IpdRegModel
                            {
                                //OrgId = ipd.ORGID,
                                StationCode = comm.ITEMNAME,//ipd.STATIONCODE,
                                //IpdFlag = ipd.IPDFLAG,
                                ResidentNo = ipd.RESIDENTNO,
                                CloseReason = ipd.CLOSEREASON,
                                OutDate = ipd.OUTDATE,
                                ProvideService = ipd.PROVIDESERVICE,
                                SvrContent = ipd.SVRCONTENT,
                                CreateDate = ipd.CREATEDATE,
                                RegName = ipdFile.REGNAME,
                                Sex = ipdFile.SEX,
                                BirthDate = ipdFile.BIRTHDATE,
                                IdNo = ipdFile.IDNO,
                                Phone = ipdFile.PHONE,
                                LivingAddress = ipdFile.LIVINGADDRESS,
                                PermanentAddress = ipdFile.PERMANENTADDRESS,
                                MerryState = ipdFile.MERRYSTATE,
                                InDate = ipd.INDATE,
                                SocialWorker = em.EMPNAME,//ipd.SOCIALWORKER,
                                NurseAides = em1.EMPNAME,//ipd.NURSEAIDES,
                                NurseNo = em2.EMPNAME,//ipd.NURSENO,
                                Org = org.ORGNAME,
                                PrintDate=DateTime.Now
                            }).FirstOrDefault();
           
           return response;
       }
       /// <summary>
       /// 一天生活
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public BaseResponse<DC_RegDayLifeModel> GetOneDayLifeById(int id)
       {
           BaseResponse<DC_RegDayLifeModel> response = new BaseResponse<DC_RegDayLifeModel>();
           response.Data = (from d in unitOfWork.GetRepository<DC_REGDAYLIFE>().dbSet.Where(m => m.Id == id)
                            join p in unitOfWork.GetRepository<DC_IPDREG>().dbSet on d.FEENO equals p.FEENO into ps 
                            from ipd in ps.DefaultIfEmpty()
                            join r in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into reg
                            from regs in reg.DefaultIfEmpty()
                            select new DC_RegDayLifeModel
                            {
                                Name = d.NAME,
                                ResidentNo = ipd.RESIDENTNO,
                                Nick = regs.NICKNAME,
                                Past0=d.PAST0,
                                Past2=d.PAST2,
                                Past4=d.PAST4,
                                Past6=d.PAST6,
                                Past7=d.PAST7,
                                Past8=d.PAST8,
                                Past9=d.PAST9,
                                Past10=d.PAST10,
                                Past11=d.PAST11,
                                Past12=d.PAST12,
                                Past14=d.PAST14,
                                Past15=d.PAST15,
                                Past16=d.PAST16,
                                Past17=d.PAST17,
                                Past18=d.PAST18,
                                Past19=d.PAST19,
                                //Past20=d.PAST20,
                                P = d.PAST20,
                                L=d.PAST21,
                                K=d.PAST22,
                                J=d.PAST24,
                                Now0=d.NOW0,
                                Now2=d.NOW2,
                                Now4=d.NOW4,
                                Now6=d.NOW6,
                                Now7=d.NOW7,
                                Now8=d.NOW8,
                                Now9=d.NOW9,
                                Now10=d.NOW10,
                                Now11=d.NOW11,
                                Now12=d.NOW12,
                                Now14=d.NOW14,
                                Now15=d.NOW15,
                                Now16=d.NOW16,
                                Now17=d.NOW17,
                                Now18=d.NOW18,
                                Now19=d.NOW19,
                                O=d.NOW20,
                                S=d.NOW21,
                                M=d.NOW22,
                                H=d.NOW24
                            }).FirstOrDefault();
           return response;
       }
       /// <summary>
       /// 基本资料
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public BaseResponse<DC_RegFileModel> GetBasicInfoById(long id)
       {
           BaseResponse<DC_RegFileModel> response = new BaseResponse<DC_RegFileModel>();

           DC_IpdRegModel model = (from r in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(m => m.FEENO == id) select new DC_IpdRegModel { FeeNo = r.FEENO, RegNo = r.REGNO }).FirstOrDefault();
           if (model.FeeNo > 0)
           {
               response.Data = (from ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(m => m.FEENO == model.FeeNo)
                       join reg in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO into r
                       from ipd_reg in r.DefaultIfEmpty()
                       join orgs in unitOfWork.GetRepository<LTC_ORG>().dbSet on ipd.ORGID equals orgs.ORGID into o
                       from org in o.DefaultIfEmpty()
                       join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NURSEAIDES equals e.EMPNO into em
                       from emp in em.DefaultIfEmpty()
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
                           SP = ipd_reg.SURETYPHONE,
                           SuretyMobile = ipd_reg.SURETYMOBILE,
                           ContactName1 = ipd_reg.CONTACTNAME1,
                           ContactAge1 = ipd_reg.CONTACTAGE1,
                           ContactTitle1 = ipd_reg.CONTACTTITLE1,
                           ContactEmail1 = ipd_reg.CONTACTEMAIL1,
                           ContactAddress1 = ipd_reg.CONTACTADDRESS1,
                           CP1 = ipd_reg.CONTACTPHONE1,
                           ContactName2 = ipd_reg.CONTACTNAME2,
                           ContactAge2 = ipd_reg.CONTACTAGE2,
                           ContactTitle2 = ipd_reg.CONTACTTITLE2,
                           ContactEmail2 = ipd_reg.CONTACTEMAIL2,
                           ContactAddress2 = ipd_reg.CONTACTADDRESS2,
                           CP2 = ipd_reg.CONTACTPHONE2,
                           EcologicalMap = ipd_reg.ECOLOGICALMAP,
                           InDate = ipd.INDATE,
                           ContactMobile1 = ipd_reg.CONTACTMOBILE1,
                           ContactMobile2 = ipd_reg.CONTACTMOBILE2,
                           ContactUnit1 = ipd_reg.CONTACTUNIT1,
                           ContactUnit2 = ipd_reg.CONTACTUNIT2,
                           NickName = ipd_reg.NICKNAME,
                           OrgName = org.ORGNAME,
                           NurseAidesName=emp.EMPNAME
                       }).FirstOrDefault();
               response.Data.Age = (DateTime.Now.Year - Convert.ToDateTime(response.Data.BirthDate).Year);
           }
           
           return response;
       }

       //public BaseResponse<DC_SwRegEvalPlanModel> GetSwRegEvalPlan(int evalPlanId,int feeNo)
       //{
           
       //}
   }
}

