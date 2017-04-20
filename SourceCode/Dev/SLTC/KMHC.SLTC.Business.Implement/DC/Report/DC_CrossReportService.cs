using AutoMapper;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC.Report;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.DC.Report
{
    public class DC_CrossReportService : BaseService, IDC_CrossReportService 
    {
        #region 护理及生活照顾服务记录表
        /// <summary>
        /// 获取个案生活史
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<NurseingLife3> GetNurseCareById(int id)
        {
          
            //加载子项目
            var response = new BaseResponse<NurseingLife3>();

            //这边获取list的集合   
            NurseingLife3 CheckReclist = new NurseingLife3();

            NurseingLifeWeeks DayLifeCarereclist = new NurseingLifeWeeks();

         

            var ID = Convert.ToInt32(id);

            // 这边只有一条的信息
            //regQuestion = unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().dbSet.Where(m => m.ID == ID).OrderByDescending(m => m.CREATEDATE).ToList()[0];


            StringBuilder sb = new StringBuilder();

            string sql = string.Format("select LTC_ORG.ORGNAME as OrgName, nurse.ID,nurse.RESIDENTNO as Res,nurse.SEX,nurse.REGNAME,nurse.NURSEAIDES as Nur,nurse.WEEKSTARTDATE,nurse.SECURITYMEASURES,nurse.ARTICLESCARRIED,nurse.MEDICATIONINSTRUCTIONS,nurse.ACTIVITYSUMMARY,nurse.QUESTIONBEHAVIOR,nurse.REMARKS,date_add(WEEKSTARTDATE, interval 0 day)as WEEK1,date_add(WEEKSTARTDATE, interval 1 day)as WEEK2,date_add(WEEKSTARTDATE, interval 2 day)as WEEK3,date_add(WEEKSTARTDATE, interval 3 day)as WEEK4,date_add(WEEKSTARTDATE, interval 4 day)as WEEK5  from  DC_NURSEINGLIFECAREREC  as nurse inner join LTC_ORG on  nurse.ORGID=LTC_ORG.ORGID where nurse.DELFLAG=0 and nurse.ID='" + ID + "'");
            sb.Append(sql); 
            //这边默认的是时间
            using (TWSLTCContext context = new TWSLTCContext())
            {
                DayLifeCarereclist = context.Database.SqlQuery<NurseingLifeWeeks>(sb.ToString()).ToList()[0];
            }


            List<DC_NurseingLifeCareEDTL> DayLifeCaredtlist = new List<DC_NurseingLifeCareEDTL>();

            if (DayLifeCarereclist.ID > 0)
            {
                List<DC_NURSEINGLIFECAREDTL> DayLifeCaredtl = unitOfWork.GetRepository<DC_NURSEINGLIFECAREDTL>().dbSet.Where(m => m.ID == DayLifeCarereclist.ID).ToList();

                Mapper.CreateMap<DC_NURSEINGLIFECAREDTL, DC_NurseingLifeCareEDTL>();

                Mapper.Map(DayLifeCaredtl, DayLifeCaredtlist);
            }

            CheckReclist.NurseingLifeCareREC = DayLifeCarereclist;
            CheckReclist.NurseingLifeCareEDTL = DayLifeCaredtlist;

            response.Data = CheckReclist;

            return response;
        }

        public string GetNurseCareBycode(string CodeId)
        {

            if (CodeId != null && CodeId != "")
            {
    
                string[] result = new string[3];

                result = CodeId.Split(',');
                var id = result[1];
                var code = result[2];
               // 这边获取list的集合   
                List<ABFilter>  ABFilterList = new  List<ABFilter>();
                  //这边默认本周为0
                  StringBuilder sb = new StringBuilder();

                  string sql="select DC_TEAMACTIVITY.ACTIVITYNAME,DC_TEAMACTIVITYDTL.TITLENAME,DC_TEAMACTIVITYDTL.ITEMNAME from DC_TEAMACTIVITY inner join DC_TEAMACTIVITYDTL on DC_TEAMACTIVITY.SEQNO=DC_TEAMACTIVITYDTL.SEQNO where 1=1 ";

                  if (code != "" && code != null && code != "undefined")
                  {
                      sql = sql + "and DC_TEAMACTIVITY.ACTIVITYCODE='"+code+"'";
                  }
                  if (id != "" && id != null && id != "undefined")
                  {
                      sql = sql + "and DC_TEAMACTIVITYDTL.ID='"+id+"'";
                  }
                  else
                  {
                      sql = sql + "and 1<>1";
                  }

                  sb.Append(sql);
                  //这边默认的是时间
                  using (TWSLTCContext context = new TWSLTCContext())
                  {
                      ABFilterList = context.Database.SqlQuery<ABFilter>(sb.ToString()).ToList();
                  }
                  if (ABFilterList.Count>0)
                  {
                      return ABFilterList[0].ITEMNAME + "(" + ABFilterList[0].TITLENAME + ")";
                  }
                  else
                  {
                      return "";

                  }
            }
            else
            {
                return null;
            
            }
        }

        #endregion

        #region 日常生活照顾记录表

        public BaseResponse<DayLife2> GetDayLifeById(int id)
        {
           
                //加载子项目
                var response = new BaseResponse<DayLife2>();

                //这边获取list的集合   
                DayLife2 CheckReclist = new DayLife2();

                DayLifeWeeks DayLifeCarereclist = new DayLifeWeeks();

                var ID = Convert.ToInt32(id);

                // 这边只有一条的信息
                //regQuestion = unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().dbSet.Where(m => m.ID == ID).OrderByDescending(m => m.CREATEDATE).ToList()[0];


                StringBuilder sb = new StringBuilder();

                string sql = string.Format("select nurse.ID,nurse.RESIDENTNO as Res,nurse.CONTACTMATTERS,nurse.FAMILYMESSAGE,nurse.SEX,nurse.REGNAME,nurse.NURSEAIDES as Nur,nurse.WEEKSTARTDATE,date_add(WEEKSTARTDATE, interval 0 day)as WEEK1,date_add(WEEKSTARTDATE, interval 1 day)as WEEK2,date_add(WEEKSTARTDATE, interval 2 day)as WEEK3,date_add(WEEKSTARTDATE, interval 3 day)as WEEK4,date_add(WEEKSTARTDATE, interval 4 day)as WEEK5  from  DC_DAYLIFECAREREC  as nurse where nurse.DELFLAG=0 and nurse.ID='" + ID + "'");
                sb.Append(sql);
                //这边默认的是时间
                using (TWSLTCContext context = new TWSLTCContext())
                {
                    DayLifeCarereclist = context.Database.SqlQuery<DayLifeWeeks>(sb.ToString()).ToList()[0];
                }


                List<DC_DayLifeCaredtl> DayLifeCaredtlist = new List<DC_DayLifeCaredtl>();

                if (DayLifeCarereclist.ID > 0)
                {
                    List<DC_DAYLIFECAREDTL> DayLifeCaredtl = unitOfWork.GetRepository<DC_DAYLIFECAREDTL>().dbSet.Where(m => m.ID == DayLifeCarereclist.ID).ToList();

                    Mapper.CreateMap<DC_DAYLIFECAREDTL, DC_DayLifeCaredtl>();

                    Mapper.Map(DayLifeCaredtl, DayLifeCaredtlist);
                }

                CheckReclist.DayLifeRec = DayLifeCarereclist;
                CheckReclist.DayLifeCaredtl = DayLifeCaredtlist;

                response.Data = CheckReclist;

                return response;
            
         
        }


        public BaseResponse<AbNormaleMotionRec> GetAb(long feeNo, int year, int month)
        {
            //加载子项目
            var response = new BaseResponse<AbNormaleMotionRec>();
    
            //这边获取list的集合   
            AbNormaleMotionRec AbNormaleMotionRec = new AbNormaleMotionRec();

            ABFilter ABFilter = new ABFilter();

            List<DC_AbNormaleMotionRec> DC_AbNormaleMotionRec = new List<DC_AbNormaleMotionRec>();

            List<DC_ABNORMALEMOTIONREC> regQuestion = unitOfWork.GetRepository<DC_ABNORMALEMOTIONREC>().dbSet.Where(m => m.YEAR == year && m.MONTH == month && m.FEENO == feeNo).ToList();

            Mapper.CreateMap<DC_ABNORMALEMOTIONREC, DC_AbNormaleMotionRec>();

            Mapper.Map(regQuestion, DC_AbNormaleMotionRec);

          

            NurseingLifeWeeks DayLifeCarereclist = new NurseingLifeWeeks();

            if (DC_AbNormaleMotionRec.Count > 0)
            {

                StringBuilder sb = new StringBuilder();

                string sql = string.Format("select LTC_ORG.ORGNAME as OrgName from DC_ABNORMALEMOTIONREC as nurse  inner join LTC_ORG  on nurse.ORGID=LTC_ORG.ORGID ");

                sb.Append(sql);
                //这边默认的是时间
                using (TWSLTCContext context = new TWSLTCContext())
                {
                    DayLifeCarereclist = context.Database.SqlQuery<NurseingLifeWeeks>(sb.ToString()).ToList()[0];
                }
                ABFilter.REGNAME = DC_AbNormaleMotionRec[0].REGNAME;
                ABFilter.SEX = DC_AbNormaleMotionRec[0].SEX;
                ABFilter.RESIDENTNO = DC_AbNormaleMotionRec[0].RESIDENTNO;
                ABFilter.Res = DC_AbNormaleMotionRec[0].RESIDENTNO;
                ABFilter.Day = year + "." + month;
                ABFilter.NURSEAIDES = DC_AbNormaleMotionRec[0].NURSEAIDES;
                ABFilter.Nur = DC_AbNormaleMotionRec[0].NURSEAIDES;
                ABFilter.OrgName = DayLifeCarereclist.OrgName;

            }
            AbNormaleMotionRec.AbNormaleMotionlist = DC_AbNormaleMotionRec;
            AbNormaleMotionRec.ab = ABFilter;
            response.Data = AbNormaleMotionRec;
            return response;
         
        }


        #endregion
    }

}

