using AutoMapper;
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

using System.Globalization;
using System.Data;
using KMHC.SLTC.Repository;
using KMHC.Infrastructure.Cached;

namespace KMHC.SLTC.Business.Implement
{
    public class NursingRecord : BaseService, INursingRecord
    {

        #region 复健
        public BaseResponse<IList<Rehabilitrec>> QueryRehabilition(BaseRequest<RecordFilter> request)
        {
            var response = base.Query<LTC_REHABILITREC, Rehabilitrec>(request, (q) =>
            {
                q = q.Where(m => m.FEENO == request.Data.FEENO);


                //q = q.Where(m => m.REGNO == request.Data.REGNO);

                //if (!string.IsNullOrEmpty(request.Data.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          ))
                //{
                //    q = q.Where(m => m.ITEMNAME.Contains(request.Data.Name));
                //}
                //if (!string.IsNullOrEmpty(request.Data.Type))
                //{
                //    q = q.Where(m => m.Type == request.Data.Type);
                //}
                //if (!string.IsNullOrEmpty(request.Data.No))
                //{
                //    q = q.Where(m => m.No.Contains(request.Data.No));
                //}


                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        /// <summary>
        /// 删除康建列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteRehabilition(int id)
        {
            return base.Delete<LTC_REHABILITREC>(id);
        }

        /// <summary>
        /// 插入新的列表中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public BaseResponse insertRehabilition(Rehabilitrec baseRequest)
        {
            //这边添加的是谁操作的
            //SecurityHelper secu = new SecurityHelper();

            baseRequest.CREATEBY = SecurityHelper.CurrentPrincipal.LoginName;

            baseRequest.INTERVALDAY = Convert.ToInt32(baseRequest.INTERVALDAY);
            return base.Save<LTC_REHABILITREC, Rehabilitrec>(baseRequest, (q) => q.ID == baseRequest.ID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>


        public BaseResponse<IList<Rehabilitrec>> GetRehabilition(BaseRequest<tt> request)
        {

            var response = base.Query<LTC_REHABILITREC, Rehabilitrec>(request, (q) =>
            {
                q = q.Where(m => m.ID == request.Data.ID);

                q = q.OrderBy(m => m.ID);
                return q;
            });
            return response;
        }
        #endregion

        #region 转诊

        public BaseResponse<IList<TranSferVisit>> QueryReferralLis(BaseRequest<RecordFilter> request)
        {
            var response = base.Query<LTC_TRANSFERVISIT, TranSferVisit>(request, (q) =>
            {
                q = q.Where(m => m.FEENO == request.Data.FEENO);


                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        /// <summary>
        /// 删除康建列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteReferralLis(int id)
        {
            return base.Delete<LTC_TRANSFERVISIT>(id);
        }



        /// <summary>
        /// 插入新的列表中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public BaseResponse insertReferralLis(TranSferVisit baseRequest)
        {
            //这边添加的是谁操作的
            //SecurityHelper secu = new SecurityHelper();

            // baseRequest.CREATEBY = SecurityHelper.CurrentPrincipal.LoginName;


            return base.Save<LTC_TRANSFERVISIT, TranSferVisit>(baseRequest, (q) => q.ID == baseRequest.ID);
        }

        /// <summary>
        /// 获取id的名字
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        public BaseResponse<IList<Employee>> GetName()
        {


            var response = new BaseResponse<IList<Employee>>();


            //这边获取list的集合
            List<Employee> CheckReclist = new List<Employee>();

            List<LTC_EMPFILE> regQuestion = unitOfWork.GetRepository<LTC_EMPFILE>().dbSet.ToList();



            Mapper.CreateMap<LTC_EMPFILE, Employee>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;


        }


        #endregion

        #region  生化检查  加载子

        public BaseResponse<IList<CheckRec>> QueryBiochemistry(BaseRequest<RecordFilter> request)
        {

            var response = new BaseResponse<IList<CheckRec>>();

            //这边获取list的集合
            List<CheckRec> CheckReclist = new List<CheckRec>();

            List<LTC_CHECKREC> regQuestion = unitOfWork.GetRepository<LTC_CHECKREC>().dbSet.Where(m => m.FEENO == request.Data.FEENO).ToList();

            Mapper.CreateMap<LTC_CHECKREC, CheckRec>();

            Mapper.Map(regQuestion, CheckReclist);


            foreach (CheckRec CheckRec in CheckReclist)
            {

                List<CheckRecdtl> CheckRecdtl = new List<CheckRecdtl>();

                List<LTC_CHECKRECDTL> re = unitOfWork.GetRepository<LTC_CHECKRECDTL>().dbSet.Where(m => m.RECORDID == CheckRec.RECORDID).ToList();


                Mapper.CreateMap<LTC_CHECKRECDTL, CheckRecdtl>();

                Mapper.Map(re, CheckRecdtl);

                CheckRec.CheckRecdtl = CheckRecdtl;

            }
            if (request.Data.RegNo > 0)
            {
                regQuestion.Where(m => m.REGNO == request.Data.RegNo);
            }
            response.RecordsCount = regQuestion.Count;

            response.Data = CheckReclist.OrderByDescending(m => m.RECORDID).Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
            response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            return response;

        }

        /// <summary>
        /// 根据住id，删除从表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public List<CheckRecdtl> Deleteids(int id)
        {

            List<CheckRecdtl> CheckRecdtl = new List<CheckRecdtl>();

            List<LTC_CHECKRECDTL> re = unitOfWork.GetRepository<LTC_CHECKRECDTL>().dbSet.Where(m => m.RECORDID == id).ToList();


            Mapper.CreateMap<LTC_CHECKRECDTL, CheckRecdtl>();

            Mapper.Map(re, CheckRecdtl);

            return CheckRecdtl;

        }


        /// <summary>
        /// 删除的时候同时删除字表的内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteBiochemistry(int id)
        {
            List<CheckRecdtl> CheckRecdtllist = Deleteids(id);
            if (CheckRecdtllist != null)
            {

                foreach (CheckRecdtl Check in CheckRecdtllist)
                {

                    //删除

                    base.Delete<LTC_CHECKRECDTL>(Check.ID);

                }

            }

            return base.Delete<LTC_CHECKREC>(id);


        }


        /// <summary>
        /// 删除字表下面的内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeletesBiochemistry(int id, int type)
        {
            return base.Delete<LTC_CHECKRECDTL>(id);
        }

        /// <summary>
        /// 插入子项目
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public BaseResponse insertCheckRecdtl(CheckRecdtl baseRequest)
        {

            BaseResponse returnCheckRecdtl = new BaseResponse();
            returnCheckRecdtl = base.Save<LTC_CHECKRECDTL, CheckRecdtl>(baseRequest, (q) => q.ID == baseRequest.ID);



            return returnCheckRecdtl;

        }

        /// <summary>
        /// 插入子项目
        /// </summary>
        /// <param name="id"></param>   
        /// <param name="type"></param>
        /// <returns></returns>
        public BaseResponse insertCheckRecdtls(IList<CheckRecdtl> baseRequest)
        {

            //这边是参数
            BaseResponse returnCheckRecdtl = new BaseResponse();

            foreach (CheckRecdtl cr in baseRequest)
            {

                returnCheckRecdtl = base.Save<LTC_CHECKRECDTL, CheckRecdtl>(cr, (q) => q.ID == cr.ID);

            }

            //returnCheckRecdtl = 

            return returnCheckRecdtl;

        }

        /// <summary>
        /// 这边父项目与子项目相互的插入
        /// </summary>
        /// <param name="id"></param>   
        /// <param name="type"></param>
        /// <returns></returns>
        public BaseResponse insertCheckRec(CheckRecCollection baseRequest)
        {

            BaseResponse returnCheckRecdtl = new BaseResponse();
            returnCheckRecdtl.ResultCode = 0;

                Mapper.CreateMap<CheckRec, LTC_CHECKREC>();

                var model = unitOfWork.GetRepository<LTC_CHECKREC>().dbSet.Where(x => x.RECORDID == baseRequest.CheckRec.RECORDID).FirstOrDefault();


            if (model == null)
            {
                model = Mapper.Map<LTC_CHECKREC>(baseRequest.CheckRec);
                unitOfWork.GetRepository<LTC_CHECKREC>().Insert(model);
                returnCheckRecdtl.ResultCode = 1001;  //添加成功
                // 这边是保存的方法 
                unitOfWork.Save();
            }

            else
            {
                Mapper.Map(baseRequest.CheckRec, model);
                unitOfWork.GetRepository<LTC_CHECKREC>().Update(model);
            }
            //这边先保存。这边保存的id


            if (baseRequest.CheckRecdtl != null && baseRequest.CheckRecdtl.Count > 0)
            {
                foreach (CheckRecdtl ckrt in baseRequest.CheckRecdtl)
                {
                    Mapper.CreateMap<CheckRecdtl, LTC_CHECKRECDTL>();

                    var Ckmodel = unitOfWork.GetRepository<LTC_CHECKRECDTL>().dbSet.Where(x => x.ID == ckrt.ID).FirstOrDefault();
                    //这边进行辅助
                    if (Ckmodel == null)
                    {
                        Ckmodel = Mapper.Map<LTC_CHECKRECDTL>(ckrt);
                        Ckmodel.RECORDID = model.RECORDID;
                        unitOfWork.GetRepository<LTC_CHECKRECDTL>().Insert(Ckmodel);
                    }
                    else
                    {
                        Mapper.Map(ckrt, Ckmodel);
                        unitOfWork.GetRepository<LTC_CHECKRECDTL>().Update(Ckmodel);
                    }
                }

            }

            unitOfWork.Save();

            return returnCheckRecdtl;


        }


        public BaseResponse<IList<CheckGroup>> GetProduceCode()
        {

            var response = new BaseResponse<IList<CheckGroup>>();


            //这边获取list的集合
            List<CheckGroup> CheckReclist = new List<CheckGroup>();

            List<LTC_CHECKGROUP> regQuestion = unitOfWork.GetRepository<LTC_CHECKGROUP>().dbSet.ToList();


            //LTC_CHECKGROUP newCheckGroup = new LTC_CHECKGROUP();
            //newCheckGroup.GROUPCODE = "000";

            //newCheckGroup.GROUPNAME = "---请选择---";

            //regQuestion.Insert(0, newCheckGroup);
            Mapper.CreateMap<LTC_CHECKGROUP, CheckGroup>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;

        }

        public BaseResponse<IList<CheckItem>> GetCheckType(string TYPECODE)
        {


            var response = new BaseResponse<IList<CheckItem>>();


            //这边获取list的集合
            List<CheckItem> CheckReclist = new List<CheckItem>();

            List<LTC_CHECKITEM> regQuestion = unitOfWork.GetRepository<LTC_CHECKITEM>().dbSet.Where(m => m.TYPECODE == TYPECODE).ToList();


            Mapper.CreateMap<LTC_CHECKITEM, CheckItem>();

            Mapper.Map(regQuestion, CheckReclist);


            response.Data = CheckReclist;

            return response;

        }

        ///produceitem
        ///
        public BaseResponse<IList<CheckItem>> produceitem()
        {

            var response = new BaseResponse<IList<CheckItem>>();


            //这边获取list的集合
            List<CheckItem> CheckReclist = new List<CheckItem>();

            List<LTC_CHECKITEM> regQuestion = unitOfWork.GetRepository<LTC_CHECKITEM>().dbSet.ToList();


            Mapper.CreateMap<LTC_CHECKITEM, CheckItem>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;

        }

        public BaseResponse<IList<CheckItem>> Checkitem(string code)
        {

            var response = new BaseResponse<IList<CheckItem>>();

            //这边获取list的集合
            List<CheckItem> CheckReclist = new List<CheckItem>();

            List<LTC_CHECKITEM> regQuestion = unitOfWork.GetRepository<LTC_CHECKITEM>().dbSet.Where(m => m.TYPECODE == code).ToList();


            Mapper.CreateMap<LTC_CHECKITEM, CheckItem>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;

        }

        //根据item获取值的范围的
        public BaseResponse<IList<CheckItem>> GetCheckitem(string code)
        {

            var response = new BaseResponse<IList<CheckItem>>();

            //这边获取list的集合
            List<CheckItem> CheckReclist = new List<CheckItem>();

            List<LTC_CHECKITEM> regQuestion = unitOfWork.GetRepository<LTC_CHECKITEM>().dbSet.Where(m => m.ITEMCODE == code).ToList();


            Mapper.CreateMap<LTC_CHECKITEM, CheckItem>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;

        }

        #endregion

        #region 护士日报表



        public BaseResponse<IList<NurseRpttpr>> QueryNurseRpttpr(BaseRequest<RecordFilter> request)
        {
            #region 生命体征
            var response = new BaseResponse<IList<NurseRpttpr>>();

            List<NurseRpttpr> CheckReclist = new List<NurseRpttpr>();

            List<LTC_NURSERPTTPR> regQuestion = unitOfWork.GetRepository<LTC_NURSERPTTPR>().dbSet.Where(m => m.FEENO == request.Data.FEENO).ToList();

            
            Mapper.CreateMap<LTC_NURSERPTTPR, NurseRpttpr>();

            Mapper.Map(regQuestion, CheckReclist);

            #endregion

            foreach (NurseRpttpr np in CheckReclist)
            {

                List<Vitalsign> CheckRecdtl = new List<Vitalsign>();
                //这边是生命特征的

                //var re = unitOfWork.GetRepository<LTC_VITALSIGN>().dbSet.Where(m => m.FEENO == np.FEENO).Where(m => m.RECORDDATE >= np.RECDATE).Where(m => m.RECORDDATE >= np.RECDATE).OrderBy(m => m.RECORDDATE).ToList().LastOrDefault();

                //这边是out的类型的东西

                //var times = Convert.ToDateTime(np.RECDATE).ToShortDateString().Replace("/", "-");
              var times = Convert.ToDateTime(np.RECDATE).ToString("yyyy-MM-dd");

                // var OutPut = unitOfWork.GetRepository<LTC_OUTVALUE>().dbSet.Where(m => m.FEENO == np.FEENO).Where(m => m.RECDATE == np.RECDATE).Where(m => m.CLASSTYPE == np.CLASSTYPE).ToList().FirstOrDefault();
            }

            response.Data = CheckReclist;

            response.RecordsCount = regQuestion.Count;

            response.Data = CheckReclist.OrderByDescending(m => m.ID).Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
            response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            return response;


        }

        public BaseResponse DeleteNurseDailyReport(int id)
        {
            return base.Delete<LTC_NURSERPTTPR>(id);
        }

        /// <summary>
        /// 生命特征最新的一条
        /// </summary>
        /// <param name="feeno"></param>
        /// <returns></returns>
        public BaseResponse<IList<Vitalsign>> GetNurseDailyReport(int feeno)
        {
            var response = new BaseResponse<IList<Vitalsign>>();

            //这边获取list的集合
            List<Vitalsign> CheckReclist = new List<Vitalsign>();

            List<LTC_VITALSIGN> regQuestion = unitOfWork.GetRepository<LTC_VITALSIGN>().dbSet.Where(m => m.FEENO == feeno).OrderByDescending(m => m.RECORDDATE).ToList();

            Mapper.CreateMap<LTC_VITALSIGN, Vitalsign>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;
        }

        //获得输入值输出值
        public BaseResponse<IList<RecordFilter>> GetOutInt(int feeno, string recdate, string classtype)
        {

            var response = new BaseResponse<IList<RecordFilter>>();

            RecordFilter CrecordFilter = new RecordFilter();

            List<RecordFilter> FrecordFilter = new List<RecordFilter>();

            var times = Convert.ToDateTime(recdate).ToString("yyyy-MM-dd");

            StringBuilder sb = new StringBuilder();
            string sql = string.Format("select * from LTC_OUTVALUE as vs where  vs.FEENO='" + feeno + "'  and DATE_FORMAT(vs.RECDATE,'%Y-%m-%d')='" + times + "' and vs.CLASSTYPE='" + classtype + "' ORDER BY vs.RECDATE  asc  ");

            sb.Append(sql);

            StringBuilder sb1 = new StringBuilder();
            string sql1 = string.Format("select * from LTC_INVALUE as vs where  vs.FEENO='" + feeno + "'  and DATE_FORMAT(vs.RECDATE,'%Y-%m-%d')='" + times + "' and vs.CLASSTYPE='" + classtype + "' ORDER BY vs.RECDATE  asc  ");

            sb1.Append(sql1);

            using (TWSLTCContext context = new TWSLTCContext())
            {
                var OutPut = context.Database.SqlQuery<OutValueModel>(sb.ToString()).ToList().FirstOrDefault();

                var IntPut = context.Database.SqlQuery<InValueModel>(sb1.ToString()).ToList().FirstOrDefault();

                if (OutPut != null)
                {
                    CrecordFilter.outvalue = Convert.ToInt32(OutPut.OutValue);
                }
                if (IntPut != null)
                {
                    CrecordFilter.intvalue = Convert.ToInt32(IntPut.InValue);
                }

            }

            FrecordFilter.Add(CrecordFilter);

            response.Data = FrecordFilter;


            return response;


        }

        public BaseResponse insertNurseRpttpr(NurseRpttpr baseRequest)
        {
            //这边添加的是谁操作的
            //SecurityHelper secu = new SecurityHelper();

            // baseRequest.CREATEBY = SecurityHelper.CurrentPrincipal.LoginName;

            return base.Save<LTC_NURSERPTTPR, NurseRpttpr>(baseRequest, (q) => q.ID == baseRequest.ID);
        }

        //获取是否存在的管路

        public BaseResponse<IList<RecordFilter>> GetNurseDailyReportpipe(int feeno)
        {

            var response = new BaseResponse<IList<RecordFilter>>();

            RecordFilter CrecordFilter = new RecordFilter();

            List<RecordFilter> FrecordFilter = new List<RecordFilter>();

            var q = from pp in unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_PIPELINEEVAL>().dbSet on pp.SEQNO equals e.SEQNO into res
                    from em in res.DefaultIfEmpty()
                    select new
                    {
                        FEENO = pp.FEENO,
                        REMOVEDFLAG = pp.REMOVEDFLAG,
                        RECENTDATE = em.RECENTDATE,
                        PIPELINENAME = pp.PIPELINENAME
                    };

            if (feeno > 0)
            {
                q = q.Where(p => p.FEENO == feeno);
            }

            q = q.Where(p => p.REMOVEDFLAG != true);
            //q = q.Where(p => p.RECENTDATE == Convert.ToDateTime(np.RECDATE).ToString("yyyy-MM-dd"));
            //q = q.OrderBy(p => p.RECENTDATE);


            var list = q.ToList();


            for (int i = 0; i < list.Count; i++)
            {

                if (list[i].PIPELINENAME == "001")
                {
                    CrecordFilter.CATHETER = true;
                }
                else
                {
                    CrecordFilter.CATHETER = false;
                }
                if (list[i].PIPELINENAME == "002")
                {
                    CrecordFilter.TRACHEOSTOMY = true;
                }
                else
                {
                    CrecordFilter.TRACHEOSTOMY = false;
                }
                if (list[i].PIPELINENAME == "003")
                {
                    CrecordFilter.NASOGASTRIC = true;
                }
                else
                {
                    CrecordFilter.NASOGASTRIC = false;
                }
            }

            FrecordFilter.Add(CrecordFilter);

            response.Data = FrecordFilter;


            return response;

        }

        #endregion

        #region 常用语设置

        public BaseResponse<IList<CommFile>> QueryCommFile(BaseRequest<RecordFilter> request)
        {
            var response = new BaseResponse<IList<CommFile>>();
            //这边获取list的集合
            List<CommFile> CheckReclist = new List<CommFile>();

            List<LTC_COMMFILE> commFile = new List<LTC_COMMFILE>();
            if (String.IsNullOrEmpty(request.Data.Name))
            {
                commFile = unitOfWork.GetRepository<LTC_COMMFILE>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).ToList();
            }
            else
            {
                commFile = unitOfWork.GetRepository<LTC_COMMFILE>().dbSet.Where(m => m.TYPENAME.Contains(request.Data.Name) && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId).ToList();

            }
            response.RecordsCount = commFile.Count;

            Mapper.CreateMap<LTC_COMMFILE, CommFile>();

            Mapper.Map(commFile, CheckReclist);

            response.Data = CheckReclist.OrderByDescending(m => m.ID).Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
            response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            return response;
        }
        //常用语插入
        public BaseResponse insertCommfile(List<CommFile> baseRequest)
        {

            var response = new BaseResponse<IList<CommFile>>();
            BaseResponse outcommfile = new BaseResponse();
            foreach (CommFile cf in baseRequest)
            {
                cf.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                outcommfile = base.Save<LTC_COMMFILE, CommFile>(cf, (q) => q.ID == cf.ID);
            }
            ProviderCached.Instance.FlushAll();
            return outcommfile;
        }
        //删除
        public BaseResponse DeleteCOMMFILE(int id)
        {
            return base.Delete<LTC_COMMFILE>(id);
        }
        //批量删除
        public BaseResponse MulDeleteCOMMFILE(List<CommFile> cfs)
        {
            unitOfWork.BeginTransaction();
            BaseResponse deletecommfile = new BaseResponse();
            foreach (CommFile cf in cfs)
            {
                base.Delete<LTC_COMMFILE>(cf.ID);
            }

            unitOfWork.Commit();
            return deletecommfile;
        }

        //根据id获取相关信息


        public BaseResponse<IList<CommFile>> GetCommfile(string id)
        {

            var response = new BaseResponse<IList<CommFile>>();

            //这边获取list的集合
            List<CommFile> CheckReclist = new List<CommFile>();

            List<LTC_COMMFILE> regQuestion = unitOfWork.GetRepository<LTC_COMMFILE>().dbSet.Where(m => m.TYPENAME == id && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId).ToList();
            Mapper.CreateMap<LTC_COMMFILE, CommFile>();

            Mapper.Map(regQuestion, CheckReclist);

            response.Data = CheckReclist;

            return response;
        }

        #endregion

        #region

        public BaseResponse<Person> GetPr(int regon)
        {
            return base.Get<LTC_REGFILE, Person>((q) => q.REGNO == regon);
        }

        #endregion

    }
}

