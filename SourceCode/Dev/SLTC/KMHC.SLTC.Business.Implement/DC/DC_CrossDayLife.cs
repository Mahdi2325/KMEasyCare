using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.DC
{


    public class DC_CrossDayLife : BaseService, IDC_CrossDayLife
    {
        #region 生活照顾记录表
        //日常生活记录表的保存
        public BaseResponse SaveDayLife(DayLife request)
        {
            BaseResponse returnCheckRecdtl = new BaseResponse();

            Mapper.CreateMap<DC_DayLifeCarerec, DC_DAYLIFECAREREC>();
            //这边有bug
            var model = unitOfWork.GetRepository<DC_DAYLIFECAREREC>().dbSet.Where(x => x.ID == request.DayLifeRec.ID).Where(x => x.DELFLAG != true).FirstOrDefault();
            //添加新的时候，添加里面的信息
            if (model == null)
            {
                model = Mapper.Map<DC_DAYLIFECAREREC>(request.DayLifeRec);
                model.DELFLAG = false;
                model.CREATEDATE = DateTime.Now;
               
                unitOfWork.GetRepository<DC_DAYLIFECAREREC>().Insert(model);
                // 这边是保存的方法 
                unitOfWork.Save();
            }
            else
            {
                Mapper.Map(request.DayLifeRec, model);
                model.DELFLAG = false;
                unitOfWork.GetRepository<DC_DAYLIFECAREREC>().Update(model);
            }

            if (request.DayLifeCaredtl != null && request.DayLifeCaredtl.Count > 0)
            {
                foreach (DC_DayLifeCaredtl ckrt in request.DayLifeCaredtl)
                {
                    Mapper.CreateMap<DC_DayLifeCaredtl, DC_DAYLIFECAREDTL>();

                    var Ckmodel = unitOfWork.GetRepository<DC_DAYLIFECAREDTL>().dbSet.Where(x => x.SEQNO == ckrt.SEQNO).FirstOrDefault();
                    //这边进行辅助
                    if (Ckmodel == null)
                    {
                        Ckmodel = Mapper.Map<DC_DAYLIFECAREDTL>(ckrt);
                        Ckmodel.ID = model.ID;
                        if (Ckmodel.HOLIDAYFLAG == "True")
                        {
                            Ckmodel.HOLIDAYFLAG = "True";
                        }
                        else
                        {
                            Ckmodel.HOLIDAYFLAG = "False";
                        }
                        unitOfWork.GetRepository<DC_DAYLIFECAREDTL>().Insert(Ckmodel);
                    }
                    else
                    {
                        Mapper.Map(ckrt, Ckmodel);
                        unitOfWork.GetRepository<DC_DAYLIFECAREDTL>().Update(Ckmodel);
                    }
                }
            }
            unitOfWork.Save();
            return returnCheckRecdtl;
        }
        /// <summary>
        /// 这边是查询的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DayLife> QueryDayLife(int FeeNo, int year, int num)
        {
            //加载子项目
            var response = new BaseResponse<DayLife>();

            //这边获取list的集合   
            DayLife CheckReclist = new DayLife();

            DC_DayLifeCarerec DayLifeCarereclist = new DC_DayLifeCarerec();

            DC_DAYLIFECAREREC regQuestion = new DC_DAYLIFECAREREC();

            List<DC_DAYLIFECAREREC> regQuestionlist = new List<DC_DAYLIFECAREREC>();
            //这边默认本周为0

            StringBuilder sb = new StringBuilder();
            string sql = string.Format("select * from DC_DAYLIFECAREREC where DC_DAYLIFECAREREC.FEENO='" + FeeNo + "' and DC_DAYLIFECAREREC.WEEKNUMBER='" + num + "' and DATE_FORMAT( current_timestamp(),'%Y' )='" + year + "' and DC_DAYLIFECAREREC.DELFLAG<>1");
            sb.Append(sql);

            //这边默认的是时间

            using (TWSLTCContext context = new TWSLTCContext())
            {
                var daylife = context.Database.SqlQuery<DC_DAYLIFECAREREC>(sb.ToString()).ToList().FirstOrDefault();

                if (daylife != null)
                {
                    DayLifeCarereclist.REGNO = daylife.REGNO;
                    DayLifeCarereclist.REGNAME = daylife.REGNAME;
                    DayLifeCarereclist.SEX = daylife.SEX;
                    DayLifeCarereclist.FAMILYMESSAGE = daylife.FAMILYMESSAGE;
                    DayLifeCarereclist.CONTACTMATTERS = daylife.CONTACTMATTERS;
                    DayLifeCarereclist.ID = daylife.ID;
                    DayLifeCarereclist.NURSEAIDES = daylife.NURSEAIDES;
                    DayLifeCarereclist.RESIDENTNO = daylife.RESIDENTNO;
                    DayLifeCarereclist.WEEKNUMBER = Convert.ToString(daylife.WEEKNUMBER);
                    DayLifeCarereclist.WEEKSTARTDATE = daylife.WEEKSTARTDATE;
                }

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
            //  response.PagesCount = regQuestionlist.Count;

            return response;
        }


        /// <summary>
        ///删除护理及圣湖照顾服务记录表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteDayLife(int id)
        {

            BaseResponse returnCheckRecdtl = new BaseResponse();

            var model = unitOfWork.GetRepository<DC_DAYLIFECAREREC>().dbSet.Where(x => x.ID == id).FirstOrDefault();

            if (model != null)
            {

                model.DELFLAG = true;
                unitOfWork.GetRepository<DC_DAYLIFECAREREC>().Update(model);
                unitOfWork.Save();
            }

            return returnCheckRecdtl;

        }

        /// <summary>
        /// 这边是查询的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_DayLifeCarerec>> QueryShowDayLife(int FEENO)
        {
            var response = new BaseResponse<IList<DC_DayLifeCarerec>>();


            //这边获取list的集合
            List<DC_DayLifeCarerec> CheckReclist = new List<DC_DayLifeCarerec>();

            List<DC_DAYLIFECAREREC> regQuestion = unitOfWork.GetRepository<DC_DAYLIFECAREREC>().dbSet.Where(m => m.FEENO == FEENO).Where(m => m.DELFLAG != true).ToList();

            Mapper.CreateMap<DC_DAYLIFECAREREC, DC_DayLifeCarerec>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;

        }

        //点击编辑的历史的记录
        public BaseResponse<DayLife> QueryShowDayLifeList(string id)
        {
            //加载子项目
            var response = new BaseResponse<DayLife>();

            //这边获取list的集合   
            DayLife CheckReclist = new DayLife();

            DC_DayLifeCarerec DayLifeCarereclist = new DC_DayLifeCarerec();

            DC_DAYLIFECAREREC regQuestion = new DC_DAYLIFECAREREC();

            var ID = Convert.ToInt32(id);

            // 这边只有一条的信息
            regQuestion = unitOfWork.GetRepository<DC_DAYLIFECAREREC>().dbSet.Where(m => m.ID == ID).OrderByDescending(m => m.CREATEDATE).ToList()[0];



            Mapper.CreateMap<DC_DAYLIFECAREREC, DC_DayLifeCarerec>();

            Mapper.Map(regQuestion, DayLifeCarereclist);

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

        public static int WeekOfYear(DateTime dt, CultureInfo ci)
        {
            return ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);

        }

        //获取当前是本年的第几周
        public BaseResponse<DayLife> yearweek()
        {
            var response = new BaseResponse<DayLife>();

            int week = WeekOfYear(DateTime.Now, new CultureInfo("zh-CN"));

            response.PagesCount = week;

            return response;
        }
        //计算一年有多是周
        public static int GetYearWeekCount(int strYear)
        {
            string returnStr = "";
            System.DateTime fDt = DateTime.Parse(strYear.ToString() + "-01-01");
            int k = Convert.ToInt32(fDt.DayOfWeek);//得到该年的第一天是周几
            if (k == 1)
            {
                int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                int countWeek = countDay / 7 + 1;
                return countWeek;
            }
            else
            {
                int countDay = fDt.AddYears(1).AddDays(-1).DayOfYear;
                int countWeek = countDay / 7 + 2;
                return countWeek;
            }
        }

        //活动。
        public BaseResponse<IList<DC_TeamActivityModel>> GetTeamAct(string Act)
        {
            if (Act == "a")
            {
                Act = "A";
            }
            if (Act == "b")
            {

                Act = "B";
            }
            BaseResponse<IList<DC_TeamActivityModel>> response = new BaseResponse<IList<DC_TeamActivityModel>>();
            //List<DC_TeamActivityModel> TeamAct = new List<DC_TeamActivityModel>();

            var q = from pp in unitOfWork.GetRepository<DC_TEAMACTIVITY>().dbSet
                    join e in unitOfWork.GetRepository<DC_TEAMACTIVITYDTL>().dbSet on pp.SEQNO equals e.SEQNO
                    select new DC_TeamActivityModel
                    {
                      
                        ID = e.ID,
                        SEQNO = pp.SEQNO,
                        ACTIVITYCODE = pp.ACTIVITYCODE,
                        ACTIVITYNAME = pp.ACTIVITYNAME,
                        ORGID = pp.ORGID,
                        TITLENAME = e.TITLENAME,
                        ITEMNAME = e.ITEMNAME
                    };

            q = q.Where(p => p.ACTIVITYCODE == Act);


            response.Data = q.ToList();

            return response;

        }




        #endregion

        #region 护理及生活照顾服务记录表
        public BaseResponse<NurseingLife> QueryNurseingLife(int FeeNo, int year, int num)
        {
            //加载子项目
            var response = new BaseResponse<NurseingLife>();

            //这边获取list的集合   
            NurseingLife CheckReclist = new NurseingLife();

            DC_NurseingLifeCareREC DayLifeCarereclist = new DC_NurseingLifeCareREC();

            DC_NURSEINGLIFECAREREC regQuestion = new DC_NURSEINGLIFECAREREC();

            List<DC_NURSEINGLIFECAREREC> regQuestionlist = new List<DC_NURSEINGLIFECAREREC>();
            //这边默认本周为0

            StringBuilder sb = new StringBuilder();


            string sql = string.Format("select * from DC_NURSEINGLIFECAREREC where DC_NURSEINGLIFECAREREC.FEENO='" + FeeNo + "' and DC_NURSEINGLIFECAREREC.WEEKNUMBER='" + num + "' and DATE_FORMAT( current_timestamp(),'%Y' )='" + year + "' and DC_NURSEINGLIFECAREREC.DELFLAG<>1");
            sb.Append(sql);

            //这边默认的是时间

            using (TWSLTCContext context = new TWSLTCContext())
            {
                var daylife = context.Database.SqlQuery<DC_NURSEINGLIFECAREREC>(sb.ToString()).ToList().FirstOrDefault();

                if (daylife != null)
                {

                    DayLifeCarereclist.REGNO = daylife.REGNO;
                    DayLifeCarereclist.REGNAME = daylife.REGNAME;
                    DayLifeCarereclist.SEX = daylife.SEX;

                    DayLifeCarereclist.ID = daylife.ID;

                    DayLifeCarereclist.NURSEAIDES = daylife.NURSEAIDES;

                    DayLifeCarereclist.SECURITYMEASURES = daylife.SECURITYMEASURES;

                    DayLifeCarereclist.ARTICLESCARRIED = daylife.ARTICLESCARRIED;

                    DayLifeCarereclist.MEDICATIONINSTRUCTIONS = daylife.MEDICATIONINSTRUCTIONS;
                    DayLifeCarereclist.ACTIVITYSUMMARY = daylife.ACTIVITYSUMMARY;
                    DayLifeCarereclist.QUESTIONBEHAVIOR = daylife.QUESTIONBEHAVIOR;
                    DayLifeCarereclist.REMARKS = daylife.REMARKS;
                }
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
            //  response.PagesCount = regQuestionlist.Count;

            return response;
        }
        //保存护理及生活照顾记录表

        public BaseResponse SaveNurseingLife(NurseingLife1 request)
        {
            BaseResponse returnCheckRecdtl = new BaseResponse();

            Mapper.CreateMap<DC_NurseingLifeCareREC, DC_NURSEINGLIFECAREREC>();
            //这边有bug
            var model = unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().dbSet.Where(x => x.ID == request.NurseingLifeCareREC.ID).Where(x => request.NurseingLifeCareREC.DELFLAG != true).FirstOrDefault();
            //添加新的时候，添加里面的信息
            if (model == null)
            {
                model = Mapper.Map<DC_NURSEINGLIFECAREREC>(request.NurseingLifeCareREC);

                //model.CREATEDATE = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                model.DELFLAG = false;
                model.CREATEDATE = DateTime.Now;

                unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().Insert(model);
                // 这边是保存的方法 
                unitOfWork.Save();
            }
            else
            {
                Mapper.Map(request.NurseingLifeCareREC, model);
                model.DELFLAG = false;
                unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().Update(model);
            }

            if (request.NurseingLifeCareEDTL != null && request.NurseingLifeCareEDTL.Count > 0)
            {

                foreach (NurseingLifeList ckrt in request.NurseingLifeCareEDTL)
                {
                    Mapper.CreateMap<DC_NurseingLifeCareEDTL, DC_NURSEINGLIFECAREDTL>();

                    var Ckmodel = unitOfWork.GetRepository<DC_NURSEINGLIFECAREDTL>().dbSet.Where(x => x.SEQNO == ckrt.NurseingLifeCare.SEQNO).FirstOrDefault();
                    //这边进行辅助
                    if (Ckmodel == null)
                    {

                        Ckmodel = Mapper.Map<DC_NURSEINGLIFECAREDTL>(ckrt.NurseingLifeCare);
                       
                          Ckmodel.ACTIVITY9 = Ckmodel.ACTIVITY9 + "|" + ckrt.Checkcy1;
                        
                       
                            Ckmodel.ACTIVITY11 = Ckmodel.ACTIVITY11 + "|" + ckrt.Checkcy2;
                        

                        
                            Ckmodel.ACTIVITY14 = Ckmodel.ACTIVITY14 + "|" + ckrt.Checkcy3;
                        

                       
                            Ckmodel.ACTIVITY15 = Ckmodel.ACTIVITY15 + "|" + ckrt.Checkcy4;
                        
                        
                            Ckmodel.ACTIVITY16 = Ckmodel.ACTIVITY16 + "|" + ckrt.Checkcy5;
                        
                         Ckmodel.ID = model.ID;


                         if (Ckmodel.HOLIDAYFLAG == "True")
                         {
                             Ckmodel.HOLIDAYFLAG = "True";
                         }
                         else
                         {
                             Ckmodel.HOLIDAYFLAG = "False";
                         }

                        unitOfWork.GetRepository<DC_NURSEINGLIFECAREDTL>().Insert(Ckmodel);
                    }
                    else
                    {
                        Mapper.Map(ckrt.NurseingLifeCare, Ckmodel);

                        Ckmodel.ACTIVITY9 = Ckmodel.ACTIVITY9 + "|" + ckrt.Checkcy1;


                        Ckmodel.ACTIVITY11 = Ckmodel.ACTIVITY11 + "|" + ckrt.Checkcy2;



                        Ckmodel.ACTIVITY14 = Ckmodel.ACTIVITY14 + "|" + ckrt.Checkcy3;



                        Ckmodel.ACTIVITY15 = Ckmodel.ACTIVITY15 + "|" + ckrt.Checkcy4;


                        Ckmodel.ACTIVITY16 = Ckmodel.ACTIVITY16 + "|" + ckrt.Checkcy5;
                        unitOfWork.GetRepository<DC_NURSEINGLIFECAREDTL>().Update(Ckmodel);
                    }
                }

            }
            //var tt = GetYearWeekCount(2020);   这边是
            unitOfWork.Save();
            return returnCheckRecdtl;
        }
        /// <summary>
        /// 这边是查询的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_NurseingLifeCareREC>> QueryShowNurseingLife(int FeeNo)
        {
            var response = new BaseResponse<IList<DC_NurseingLifeCareREC>>();

            //这边获取list的集合
            List<DC_NurseingLifeCareREC> CheckReclist = new List<DC_NurseingLifeCareREC>();

            List<DC_NURSEINGLIFECAREREC> regQuestion = unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().dbSet.Where(m => m.FEENO == FeeNo).Where(m => m.DELFLAG != true).ToList();

            Mapper.CreateMap<DC_NURSEINGLIFECAREREC, DC_NurseingLifeCareREC>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;
        }

        /// <summary>
        ///删除护理及圣湖照顾服务记录表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteNuring(int id)
        {

            BaseResponse returnCheckRecdtl = new BaseResponse();

            var model = unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().dbSet.Where(x => x.ID == id).FirstOrDefault();

            if (model != null)
            {

                model.DELFLAG = true;
                unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().Update(model);
                unitOfWork.Save();
            }

            return returnCheckRecdtl;

        }

        //点击编辑的历史的记录
        public BaseResponse<NurseingLife> QueryShowNurseList(string id)
        {
            //加载子项目
            var response = new BaseResponse<NurseingLife>();

            //这边获取list的集合   
            NurseingLife CheckReclist = new NurseingLife();



            DC_NurseingLifeCareREC DayLifeCarereclist = new DC_NurseingLifeCareREC();

            DC_NURSEINGLIFECAREREC regQuestion = new DC_NURSEINGLIFECAREREC();

            var ID = Convert.ToInt32(id);

            // 这边只有一条的信息
            regQuestion = unitOfWork.GetRepository<DC_NURSEINGLIFECAREREC>().dbSet.Where(m => m.ID == ID).OrderByDescending(m => m.CREATEDATE).ToList()[0];



            Mapper.CreateMap<DC_NURSEINGLIFECAREREC, DC_NurseingLifeCareREC>();

            Mapper.Map(regQuestion, DayLifeCarereclist);


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

        #endregion

        #region 问题行为与情绪异常记录表

        public BaseResponse SaveAB(AbNormaleMotionRec baseRequest)
        {

            BaseResponse tt = new BaseResponse();

            if (baseRequest != null && baseRequest.AbNormaleMotionlist.Count > 0)
            {
                foreach (DC_AbNormaleMotionRec ckrt in baseRequest.AbNormaleMotionlist)
                {
                    Mapper.CreateMap<DC_AbNormaleMotionRec, DC_ABNORMALEMOTIONREC>();

                    var Ckmodel = unitOfWork.GetRepository<DC_ABNORMALEMOTIONREC>().dbSet.Where(x => x.ID == ckrt.ID).FirstOrDefault();

                    //这边进行辅助
                    if (Ckmodel == null)
                    {
                        Ckmodel = Mapper.Map<DC_ABNORMALEMOTIONREC>(ckrt);
                        Ckmodel.SEX = baseRequest.ab.SEX;
                        Ckmodel.REGNAME = baseRequest.ab.REGNAME;
                        Ckmodel.RESIDENTNO = baseRequest.ab.RESIDENTNO;
                        Ckmodel.REGNO = baseRequest.ab.REGNO;
                        Ckmodel.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                        Ckmodel.DELFLAG = false;
                        Ckmodel.NURSEAIDES = baseRequest.ab.NURSEAIDES;
                        Ckmodel.RECORDDATE = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        Ckmodel.YEAR = baseRequest.ab.year;
                        Ckmodel.MONTH = baseRequest.ab.month;
                        Ckmodel.FEENO = baseRequest.ab.FEENO;

                        unitOfWork.GetRepository<DC_ABNORMALEMOTIONREC>().Insert(Ckmodel);
                        unitOfWork.Save();
                    }
                    else
                    {
                        Mapper.Map(ckrt, Ckmodel);

                        Ckmodel.NURSEAIDES = baseRequest.ab.NURSEAIDES;


                        Ckmodel.RESIDENTNO = baseRequest.ab.RESIDENTNO;

                        Ckmodel.SEX = baseRequest.ab.SEX;

                        Ckmodel.REGNAME = baseRequest.ab.REGNAME;

                        unitOfWork.GetRepository<DC_ABNORMALEMOTIONREC>().Update(Ckmodel);
                        unitOfWork.Save();
                    }
                }
            }

            return tt;
        }

        public BaseResponse<IList<DC_COMMDTLModel>> getCY()
        {

            string code = "DC03.023";

            var response = new BaseResponse<IList<DC_COMMDTLModel>>();

            //这边获取list的集合
            List<DC_COMMDTLModel> CheckReclist = new List<DC_COMMDTLModel>();

            List<DC_COMMDTL> regQuestion = unitOfWork.GetRepository<DC_COMMDTL>().dbSet.Where(m => m.ITEMTYPE == code).ToList();

            Mapper.CreateMap<DC_COMMDTL, DC_COMMDTLModel>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;
        }



        public BaseResponse<AbNormaleMotionRec> QueryAB(int FeeNo, int year, int month)
        {
            //加载子项目
            var response = new BaseResponse<AbNormaleMotionRec>();

            //这边获取list的集合   
            AbNormaleMotionRec AbNormaleMotionRec = new AbNormaleMotionRec();

            ABFilter ABFilter = new ABFilter();

            List<DC_AbNormaleMotionRec> DC_AbNormaleMotionRec = new List<DC_AbNormaleMotionRec>();

            List<DC_ABNORMALEMOTIONREC> regQuestion = unitOfWork.GetRepository<DC_ABNORMALEMOTIONREC>().dbSet.Where(m => m.YEAR == year && m.MONTH == month && m.FEENO == FeeNo).ToList();

            Mapper.CreateMap<DC_ABNORMALEMOTIONREC, DC_AbNormaleMotionRec>();

            Mapper.Map(regQuestion, DC_AbNormaleMotionRec);

            if (DC_AbNormaleMotionRec.Count > 0)
            {
                ABFilter.REGNAME = DC_AbNormaleMotionRec[0].REGNAME;
                ABFilter.SEX = DC_AbNormaleMotionRec[0].SEX;
                ABFilter.RESIDENTNO = DC_AbNormaleMotionRec[0].RESIDENTNO;
            }

            AbNormaleMotionRec.AbNormaleMotionlist = DC_AbNormaleMotionRec;
            AbNormaleMotionRec.ab = ABFilter;
            response.Data = AbNormaleMotionRec;
            return response;
        }

        public BaseResponse<IList<ABFilter>> QueryHISAB(int FeeNo)
        {
            //加载子项目
            var response = new BaseResponse<IList<ABFilter>>();

            //这边获取list的集合   
            List<ABFilter> ABFilterList = new List<ABFilter>();
            //这边默认本周为0
            StringBuilder sb = new StringBuilder();
            string sql = string.Format("select DC_ABNORMALEMOTIONREC.MONTH,DC_ABNORMALEMOTIONREC.YEAR,REGNAME,RESIDENTNO,SEX,FEENO,DELFLAG,NURSEAIDES from DC_ABNORMALEMOTIONREC group by DC_ABNORMALEMOTIONREC.MONTH,DC_ABNORMALEMOTIONREC.YEAR,REGNAME,RESIDENTNO,SEX,FEENO,DELFLAG,NURSEAIDES  having  DC_ABNORMALEMOTIONREC.FEENO='" + FeeNo + "'");
            sb.Append(sql);
            //这边默认的是时间
            using (TWSLTCContext context = new TWSLTCContext())
            {
                ABFilterList = context.Database.SqlQuery<ABFilter>(sb.ToString()).ToList();
            }
            response.Data = ABFilterList;
            return response;

        }

        /// <returns></returns>
        public BaseResponse DeleteAB(int regno, int year, int month)
        {

            BaseResponse returnCheckRecdtl = new BaseResponse();

            var model = unitOfWork.GetRepository<DC_ABNORMALEMOTIONREC>().dbSet.Where(x => x.FEENO == regno).Where(x => x.YEAR == year).Where(x => x.MONTH == month).ToList();

            foreach (DC_ABNORMALEMOTIONREC ck in model)
            {

                base.Delete<DC_ABNORMALEMOTIONREC>(ck.ID);
            }

            return returnCheckRecdtl;
        }

        #endregion

        #region 跨专业团队服务计划表


        /// <summary>
        /// 这边是查询的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_SwRegEvalPlanModel> QuerySWREGEVALPLAN(int feeono)
        {
            var response = new BaseResponse<DC_SwRegEvalPlanModel>();

            //这边获取list的集合
            DC_SwRegEvalPlanModel CheckReclist = new DC_SwRegEvalPlanModel();

            DC_SWREGEVALPLAN regQuestion = unitOfWork.GetRepository<DC_SWREGEVALPLAN>().dbSet.Where(m => m.FEENO == feeono).OrderByDescending(m => m.EVALPLANID).FirstOrDefault();

            Mapper.CreateMap<DC_SWREGEVALPLAN, DC_SwRegEvalPlanModel>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;

        }
        //保存跨专业团队服务计划表
        public BaseResponse SaveSWREGEVALPLAN(DC_MultiteaMcarePlanEvalModel baseRequest)
        {
            baseRequest.CREATEDATE = DateTime.Now;
            return base.Save<DC_MULTITEAMCAREPLANEVAL, DC_MultiteaMcarePlanEvalModel>(baseRequest, (q) => q.SEQNO == baseRequest.SEQNO);
        }



        /// <summary>
        /// 这边是查询的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_MultiteaMcarePlanEvalModel>> QueryHisMultiteaMcarePlanEval(string REGNO)
        {
            var response = new BaseResponse<IList<DC_MultiteaMcarePlanEvalModel>>();

            //这边获取list的集合
            List<DC_MultiteaMcarePlanEvalModel> CheckReclist = new List<DC_MultiteaMcarePlanEvalModel>();

            List<DC_MULTITEAMCAREPLANEVAL> regQuestion = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANEVAL>().dbSet.Where(m => m.REGNO == REGNO).ToList();

            Mapper.CreateMap<DC_MULTITEAMCAREPLANEVAL, DC_MultiteaMcarePlanEvalModel>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;
        }


        //历史记录的编辑 
        public BaseResponse<DC_MultiteaMcarePlanEvalModel> QueryHisMultiteaMcare(int ID)
        {

            var response = new BaseResponse<DC_MultiteaMcarePlanEvalModel>();

            //这边获取list的集合
            DC_MultiteaMcarePlanEvalModel CheckReclist = new DC_MultiteaMcarePlanEvalModel();

            DC_MULTITEAMCAREPLANEVAL regQuestion = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANEVAL>().dbSet.Where(m => m.SEQNO == ID).FirstOrDefault();

            Mapper.CreateMap<DC_MULTITEAMCAREPLANEVAL, DC_MultiteaMcarePlanEvalModel>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;
        }

        /// <summary>
        /// 删除康建列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteultiteaMcare(int id)
        {
            return base.Delete<DC_MULTITEAMCAREPLANEVAL>(id);
        }

        #endregion

        #region  跨专业团队服务计划表2
        public BaseResponse SaveMULTITEAM(MultiteamCarePlanRec baseRequest)
        {

            BaseResponse returnCheckRecdtl = new BaseResponse();

            Mapper.CreateMap<DC_MultiteamCarePlanRecModel, DC_MULTITEAMCAREPLANREC>();

            //这边有bug
            var model = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x => x.SEQNO == baseRequest.MultiteamCarePlanRe.SEQNO).FirstOrDefault();
            //添加新的时候，添加里面的信息
            if (model == null)
            {
                model = Mapper.Map<DC_MULTITEAMCAREPLANREC>(baseRequest.MultiteamCarePlanRe);

                model.CREATEDATE = DateTime.Now;
                model.EVALDATE = DateTime.Now;

                unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().Insert(model);
                // 这边是保存的方法 
                unitOfWork.Save();
            }
            else
            {
                Mapper.Map(baseRequest.MultiteamCarePlanRe, model);

                unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().Update(model);
            }

            if (baseRequest.MultiteamCar != null && baseRequest.MultiteamCar.Count > 0)
            {
                foreach (DC_MultiteamCarePlanModel ckrt in baseRequest.MultiteamCar)
                {
                    Mapper.CreateMap<DC_MultiteamCarePlanModel, DC_MULTITEAMCAREPLAN>();

                    var Ckmodel = unitOfWork.GetRepository<DC_MULTITEAMCAREPLAN>().dbSet.Where(x => x.ID == ckrt.ID).FirstOrDefault();
                    //这边进行辅助
                    if (Ckmodel == null)
                    {

                        Ckmodel = Mapper.Map<DC_MULTITEAMCAREPLAN>(ckrt);
                        Ckmodel.SEQNO = model.SEQNO;
                        unitOfWork.GetRepository<DC_MULTITEAMCAREPLAN>().Insert(Ckmodel);
                    }
                    else
                    {
                        Mapper.Map(ckrt, Ckmodel);
                        Ckmodel.SEQNO = model.SEQNO;
                        unitOfWork.GetRepository<DC_MULTITEAMCAREPLAN>().Update(Ckmodel);
                    }
                }

            }
            unitOfWork.Save();
            return returnCheckRecdtl;
        }

        //查询最新的跨专业团队计划表


    

        public BaseResponse<IList<DC_MultiteamCarePlanRecModel>> QueryHisMULTITEAM(string REGNO)
        {

            var response = new BaseResponse<IList<DC_MultiteamCarePlanRecModel>>();

            //这边获取list的集合
            List<DC_MultiteamCarePlanRecModel> CheckReclist = new List<DC_MultiteamCarePlanRecModel>();

            List<DC_MULTITEAMCAREPLANREC> regQuestion = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(m => m.REGNO == REGNO).ToList();

            Mapper.CreateMap<DC_MULTITEAMCAREPLANREC, DC_MultiteamCarePlanRecModel>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;
        }




        //历史记录的编辑 
        public BaseResponse<MultiteamCarePlanRec> QueryShowHis(int ID)
        {

            var response = new BaseResponse<MultiteamCarePlanRec>();

            MultiteamCarePlanRec responselist = new MultiteamCarePlanRec();



            DC_MULTITEAMCAREPLANREC model = new DC_MULTITEAMCAREPLANREC();
            DC_MultiteamCarePlanRecModel Tmodel = new DC_MultiteamCarePlanRecModel();


            model = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x => x.SEQNO == ID).FirstOrDefault();



            Mapper.CreateMap<DC_MULTITEAMCAREPLANREC, DC_MultiteamCarePlanRecModel>();

            Mapper.Map(model, Tmodel);


            List<DC_MultiteamCarePlanModel> TMultiteamList = new List<DC_MultiteamCarePlanModel>();

            if (model != null)
            {


                List<DC_MULTITEAMCAREPLAN> MultiteamList = new List<DC_MULTITEAMCAREPLAN>();

                MultiteamList = unitOfWork.GetRepository<DC_MULTITEAMCAREPLAN>().dbSet.Where(x => x.SEQNO == model.SEQNO).ToList();


                Mapper.CreateMap<DC_MULTITEAMCAREPLAN, DC_MultiteamCarePlanModel>();

                Mapper.Map(MultiteamList, TMultiteamList);


            }

            responselist.MultiteamCarePlanRe = Tmodel;
            responselist.MultiteamCar = TMultiteamList;

            response.Data = responselist;

            return response;
        }




        /// <summary>
        /// 删除的时候同时删除字表的内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteMULTITEAM(int id)
        {
            List<DC_MultiteamCarePlanModel> CheckRecdtllist = Deleteids(id);
            if (CheckRecdtllist != null)
            {

                foreach (DC_MultiteamCarePlanModel Check in CheckRecdtllist)
                {

                    //删除

                    base.Delete<DC_MULTITEAMCAREPLAN>(Check.ID);

                }

            }

            return base.Delete<DC_MULTITEAMCAREPLANREC>(id);


        }


        public List<DC_MultiteamCarePlanModel> Deleteids(int id)
        {

            List<DC_MultiteamCarePlanModel> CheckRecdtl = new List<DC_MultiteamCarePlanModel>();

            List<DC_MULTITEAMCAREPLAN> re = unitOfWork.GetRepository<DC_MULTITEAMCAREPLAN>().dbSet.Where(m => m.SEQNO == id).ToList();


            Mapper.CreateMap<DC_MULTITEAMCAREPLAN, DC_MultiteamCarePlanModel>();

            Mapper.Map(re, CheckRecdtl);

            return CheckRecdtl;

        }





        //public BaseResponse DeleteMULTITEAM(int id)
        //{

        //    BaseResponse returnCheckRecdtl = new BaseResponse();

        //    //DC_MultiteamCarePlanRecModel
        //    var model = unitOfWork.GetRepository<DC_MULTITEAMCAREPLANREC>().dbSet.Where(x => x.SEQNO == id).FirstOrDefault();

        //    if (model != null)
        //    {

        //        return base.Delete<DC_MULTITEAMCAREPLANEVAL>(id);
        //    }

        //    return returnCheckRecdtl;
        //}

        #endregion
    }

}

