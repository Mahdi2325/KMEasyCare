using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class CarePlanReportService : BaseService, ICarePlanReportService
    {


        string orgId = SecurityHelper.CurrentPrincipal.OrgId;
        public List<RegActivityRequEval> GetCarePlanData(long _feeNo, DateTime? startDate, DateTime? endDate, string floorId)
        {

            List<RegActivityRequEval> list = new List<RegActivityRequEval>();
            var q = (from a in unitOfWork.GetRepository<LTC_REGACTIVITYREQUEVAL>().dbSet.Where(o => o.ORGID == orgId)
                     join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.CARER equals e.EMPNO into res
                     join d in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals d.FEENO
                     join h in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on d.REGNO equals h.REGNO
                     join c in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals c.REGNO
                     join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on c.FLOOR equals j.FLOORID into aj
                     from floor in aj.DefaultIfEmpty()
                     from re in res.DefaultIfEmpty()
                     select new RegActivityRequEval
                     {
                         Id = a.ID,
                         regfeeno = a.FEENO,
                         RegEvalDate = a.EVALDATE,
                         Carer = re.EMPNO,
                         CarerName = re.EMPNAME,
                         FeeName = h.NAME,
                         floorId = floor.FLOORID
                     });

            if (!string.IsNullOrEmpty(floorId))
            {
               q = q.Where(m => m.floorId == floorId);
            }
            else
            {
                if (_feeNo != 0)
                {
                    q = q.Where(m => m.regfeeno == _feeNo);
                }
            }
            if (startDate.HasValue)
            {
                q = q.Where(m => m.RegEvalDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                q = q.Where(m => m.RegEvalDate <= endDate.Value);
            }
            return q.ToList();
        }

        public List<ReportInfo> GetCareH35(long _feeNo, DateTime? startDate, DateTime? endDate, string floorId)
        {

            List<ReportInfo> list = new List<ReportInfo>();

            var q = (from a in unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet.Where(o => o.ORGID == orgId)
                     join c in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals c.FEENO
                     join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on c.FLOOR equals j.FLOORID into aj
                     from floor in aj.DefaultIfEmpty()
                     select new ReportInfo
                     {
                         ID = a.ID,
                         FEENO = a.FEENO,
                         EVALDATE = a.EVALDATE,
                         FLOORID = floor.FLOORID

                     });

            if (!string.IsNullOrEmpty(floorId))
            {
               q = q.Where(m => m.FLOORID == floorId);
            }
            else
            {
                if (_feeNo != 0)
                {
                    q = q.Where(m => m.FEENO == _feeNo);
                }
            }
            if (startDate.HasValue)
            {
                q = q.Where(m => m.EVALDATE >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                q = q.Where(m => m.EVALDATE <= endDate.Value);
            }
            return q.ToList();
        }

        public List<ReportInfo> GetCareH10(long _feeNo, DateTime? startDate, DateTime? endDate, string floorId)
        {
            List<ReportInfo> list = new List<ReportInfo>();
            var q = (from a in unitOfWork.GetRepository<LTC_NSCPL>().dbSet.Where(o => o.ORGID == orgId)
                     join c in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals c.REGNO
                     join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on c.FLOOR equals j.FLOORID into aj
                     from floor in aj.DefaultIfEmpty()
                     select new ReportInfo
                     {
                         ID = a.SEQNO,
                         FEENO = a.FEENO,
                         EVALDATE = a.STARTDATE,
                         FLOORID = floor.FLOORID,
                     });

            if (!string.IsNullOrEmpty(floorId))
            {
               q = q.Where(m => m.FLOORID == floorId);
            }
            else
            {
                if (_feeNo != 0)
                {
                    q = q.Where(m => m.FEENO == _feeNo);
                }
            }
            if (startDate.HasValue)
            {
                q = q.Where(m => m.EVALDATE >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                q = q.Where(m => m.EVALDATE <= endDate.Value);
            }
            return q.ToList();
        }

        public List<ReportInfo> GetCareP18(long _feeNo, DateTime? startDate, DateTime? endDate, string classtype, string floorId)
        {
            List<ReportInfo> list = new List<ReportInfo>();
            var q = (from a in unitOfWork.GetRepository<LTC_NURSINGREC>().dbSet.Where(o => o.ORGID == orgId)
                     join c in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals c.REGNO
                     join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on c.FLOOR equals j.FLOORID into aj
                     from floor in aj.DefaultIfEmpty()
                     select new ReportInfo
                     {
                         ID = a.ID,
                         FEENO = a.FEENO,
                         EVALDATE = a.RECORDDATE,
                         CLASSTYPE = a.CLASSTYPE,
                         FLOORID = floor.FLOORID
                     });
            if (!string.IsNullOrEmpty(floorId))
            {
               q = q.Where(m => m.FLOORID == floorId);
            }
            else
            {
                if (_feeNo != 0)
                {
                    q = q.Where(m => m.FEENO == _feeNo);
                }
            }
            if (startDate.HasValue)
            {
                q = q.Where(m => m.EVALDATE >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                q = q.Where(m => m.EVALDATE <= endDate.Value);
            }
            if (!string.IsNullOrEmpty(classtype))
            {
                q = q.Where(m => m.CLASSTYPE == classtype);
            }
            return q.ToList();
        }

        public ExportResidentInfo GetExportResidentInfo(long feeNo)
        {
            var resident = (from a in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                            join b in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals b.REGNO
                            join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on b.FLOOR equals f.FLOORID into ffs
                            from ff in ffs.DefaultIfEmpty()
                            join g in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on b.ROOMNO equals g.ROOMNO into ggs
                            from gg in ggs.DefaultIfEmpty()

                            join c in unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Where(o => o.ITEMTYPE == "A00.001") on a.SEX equals c.ITEMCODE into ac
                            from d in ac.DefaultIfEmpty()
                            let age = a.BRITHDATE.HasValue ? (int?)(DateTime.Now.Year - a.BRITHDATE.Value.Year) : null
                            join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals e.ORGID
                            where b.FEENO == feeNo
                            select new ExportResidentInfo
                            {
                                Org = e.ORGNAME,
                                RegNo = a.REGNO,
                                ResidentNo = a.RESIDENGNO,
                                Name = a.NAME,
                                BedNo = b.BEDNO,
                                BirthDate = a.BRITHDATE,
                                InDate = b.INDATE,
                                Floor = ff.FLOORNAME,
                                RoomNo = gg.ROOMNAME,
                                Age = age,
                                Weight = a.WEIGHT,
                                Sex = d != null ? d.ITEMNAME : ""
                            }).FirstOrDefault();
            return resident;
        }

        public NutritionEvalModel QueryNutritionEval(long id)
        {
            var q = (from ne in unitOfWork.GetRepository<LTC_NUTRTIONEVAL>().dbSet.Where(m => m.ID == id)
                     select new NutritionEvalModel
                     {
                         FEENO = ne.FEENO,
                         NAME = ne.NAME,
                         ID = ne.ID,
                         DISEASEDIAG = ne.DISEASEDIAG,
                         CHEWDIFFCULT = ne.CHEWDIFFCULT,
                         SWALLOWABILITY = ne.SWALLOWABILITY,
                         EATPATTERN = ne.EATPATTERN,
                         DIGESTIONPROBLEM = ne.DIGESTIONPROBLEM,
                         FOODTABOO = ne.FOODTABOO,
                         ACTIVITYABILITY = ne.ACTIVITYABILITY,
                         PRESSURESORE = ne.PRESSURESORE,
                         EDEMA = ne.EDEMA,
                         CURRENTDIET = ne.CURRENTDIET,
                         EATAMOUNT = ne.EATAMOUNT,
                         WATER = ne.WATER,
                         SUPPLEMENTS = ne.SUPPLEMENTS,
                         SNACK = ne.SNACK,
                         EVALDATE = ne.EVALDATE
                     }).FirstOrDefault();
            return q;
        }

        public string GetCodedtlInfo(string itemcode, string itemtype)
        {
            string itemname = string.Empty;
            if (!string.IsNullOrEmpty(itemcode) && !string.IsNullOrEmpty(itemtype))
            {
                var dbSet = unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.AsQueryable();
                var list = dbSet.ToList();
                itemname = list.Where(o => o.ITEMCODE == itemcode && o.ITEMTYPE == itemtype).FirstOrDefault().ITEMNAME;
            }
            else
            {
                itemname = "";
            }
            return itemname;
        }

        public List<ReportCheckRec> QueryBiochemistryList(long feeno, DateTime enddate)
        {

            //这边获取list的集合
            List<ReportCheckRec> CheckReclist = new List<ReportCheckRec>();

            List<LTC_CHECKREC> regQuestion = unitOfWork.GetRepository<LTC_CHECKREC>().dbSet.Where(m => m.FEENO == feeno && m.CHECKDATE <= enddate).ToList();

            Mapper.CreateMap<LTC_CHECKREC, ReportCheckRec>();

            Mapper.Map(regQuestion, CheckReclist);


            foreach (ReportCheckRec CheckRec in CheckReclist)
            {

                List<ReportCheckRecdtl> repc = new List<ReportCheckRecdtl>();
                var re = unitOfWork.GetRepository<LTC_CHECKRECDTL>().dbSet.Where(m => m.RECORDID == CheckRec.RECORDID).ToList();


                repc = (from a in unitOfWork.GetRepository<LTC_CHECKRECDTL>().dbSet.Where(m => m.RECORDID == CheckRec.RECORDID)
                        join e in unitOfWork.GetRepository<LTC_CHECKITEM>().dbSet on a.CHECKITEM equals e.ITEMCODE into res
                        from de in res.DefaultIfEmpty()
                        select new ReportCheckRecdtl
                        {
                            ID = a.ID,
                            CHECKTYPE = a.CHECKTYPE,
                            CHECKITEM = a.CHECKITEM,
                            CHECKITEMNAME = de.ITEMNAME,
                            CHECKRESULTS = a.CHECKRESULTS,
                            DESCRIPTION = a.DESCRIPTION,
                            EVALCOMBO = a.EVALCOMBO,
                        }).ToList();


                CheckRec.ReportCheckRecdtl = repc;
            }

            return CheckReclist;
        }
        public BaseResponse<IList<DoctorCheckRec>> QueryDocCheckRecData(BaseRequest<DoctorCheckRecFilter> request)
        {
            BaseResponse<IList<DoctorCheckRec>> response = new BaseResponse<IList<DoctorCheckRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_DOCTORCHECKREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.DOCNO equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        DoctorCheckRec = n,
                        EmpName = re.EMPNAME
                    };

            q = q.Where(m => m.DoctorCheckRec.ID == request.Data.Id && m.DoctorCheckRec.ORGID == SecurityHelper.CurrentPrincipal.OrgId);

            q = q.OrderByDescending(m => m.DoctorCheckRec.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<DoctorCheckRec>();
                foreach (dynamic item in list)
                {
                    DoctorCheckRec newItem = Mapper.DynamicMap<DoctorCheckRec>(item.DoctorCheckRec);
                    newItem.DocName = item.EmpName;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

    }
}
