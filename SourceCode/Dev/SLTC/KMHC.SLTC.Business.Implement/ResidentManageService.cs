/*
创建人: 肖国栋
创建日期:2016-03-09
说明:住民管理
*/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Implement.Other;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KM.Common;
using System.Net.Http;
using Newtonsoft.Json;

namespace KMHC.SLTC.Business.Implement
{
    public class ResidentManageService : BaseService, IResidentManageService
    {
        private IResidentBalanceService _residentBalanceService = IOCContainer.Instance.Resolve<IResidentBalanceService>();
        private IDictManageService _service = IOCContainer.Instance.Resolve<IDictManageService>();
        #region 住民
        public BaseResponse<IList<Person>> QueryPerson(BaseRequest<PersonFilter> request)
        {
            var response = base.Query<LTC_REGFILE, Person>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.Name) && !string.IsNullOrEmpty(request.Data.IdNo))
                {
                    q = q.Where(m => m.NAME.Contains(request.Data.Name) || m.IDNO.Contains(request.Data.IdNo));
                }
                if (!string.IsNullOrEmpty(request.Data.IdNo))
                {
                    q = q.Where(m => m.IDNO == request.Data.IdNo);
                }
                if (!string.IsNullOrEmpty(request.Data.ResidengNo))
                {
                    q = q.Where(m => m.RESIDENGNO == request.Data.ResidengNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<Person>> QueryPersonExtend(BaseRequest<PersonFilter> request)
        {
            BaseResponse<IList<Person>> response = new BaseResponse<IList<Person>>();
            var q = from reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                    join rel in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on reg.REGNO equals rel.REGNO into reg_rels
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on reg.REGNO equals ipd.REGNO into reg_ipds
                    from reg_ipd in reg_ipds.DefaultIfEmpty()
                    join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on reg_ipd.FLOOR equals f.FLOORID into fs
                    join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on reg_ipd.ROOMNO equals r.ROOMNO into rs
                    join bed in
                        (from in_bed in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet where in_bed.FEENO != null select in_bed) on reg_ipd.FEENO equals bed.FEENO into ipd_beds
                    from reg_rel in reg_rels.DefaultIfEmpty()
                    from ipd_bed in ipd_beds.DefaultIfEmpty()
                    from floor in fs.DefaultIfEmpty()
                    from room in rs.DefaultIfEmpty()
                    select new Person
                    {
                        FeeNo = reg_ipd.FEENO,
                        InDate = reg_ipd.INDATE,
                        RegNo = reg.REGNO,
                        IdNo = reg.IDNO,
                        CreateDate = reg.CREATEDATE,
                        Name = reg.NAME,
                        Sex = reg.SEX,
                        Age = reg.AGE ?? 0,
                        Floor = ipd_bed.FLOOR,
                        BedNo = ipd_bed.BEDNO,
                        ResidengNo = reg.RESIDENGNO,
                        OrgId = reg.ORGID,
                        Brithdate = reg.BRITHDATE,
                        RsType = reg_ipd.RSTYPE,
                        RsStatus = reg_ipd.RSSTATUS,
                        Race = reg.RACE,
                        Political = reg.POLITICAL,
                        IpdFlag = reg_ipd.IPDFLAG,
                        Relation = new Relation() { PhotoPath = reg_rel.PHOTOPATH },
                        FloorName = floor.FLOORID,
                        RoomName = room.ROOMNO

                    };

            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            if (!string.IsNullOrEmpty(request.Data.keyWord))
            {
                q = q.Where(m => m.Name.ToUpper().Contains(request.Data.keyWord.ToUpper()) || m.IdNo.ToUpper().Contains(request.Data.keyWord.ToUpper())
                    || m.ResidengNo.ToUpper().Contains(request.Data.keyWord.ToUpper()));
            }
            if (!string.IsNullOrEmpty(request.Data.IpdFlag))
            {
                q = q.Where(m => m.IpdFlag == request.Data.IpdFlag);
            }
            if (!string.IsNullOrEmpty(request.Data.FloorName))
            {
                q = q.Where(m => m.FloorName == request.Data.FloorName);
            }
            if (!string.IsNullOrEmpty(request.Data.RoomName))
            {
                q = q.Where(m => m.RoomName == request.Data.RoomName);
            }
            //20160927修改 Duke 排序按登记日期倒叙排列
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
        public BaseResponse<IList<Person>> QueryPersonExtend2(BaseRequest<PersonExtendFilter> request)
        {
            BaseResponse<IList<Person>> response = new BaseResponse<IList<Person>>();
            var q = from reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                    join rel in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on reg.REGNO equals rel.REGNO into reg_rels
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on reg.REGNO equals ipd.REGNO into reg_ipds
                    from reg_ipd in reg_ipds.DefaultIfEmpty()
                    join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on reg_ipd.FLOOR equals f.FLOORID into fs
                    join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on reg_ipd.ROOMNO equals r.ROOMNO into rs
                    join bed in
                        (from in_bed in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet where in_bed.FEENO != null select in_bed) on reg_ipd.FEENO equals bed.FEENO into ipd_beds
                    from reg_rel in reg_rels.DefaultIfEmpty()
                    from ipd_bed in ipd_beds.DefaultIfEmpty()
                    from floor in fs.DefaultIfEmpty()
                    from room in rs.DefaultIfEmpty()
                    select new Person
                    {
                        FeeNo = reg_ipd.FEENO,
                        InDate = reg_ipd.INDATE,
                        RegNo = reg.REGNO,
                        IdNo = reg.IDNO,
                        CreateDate = reg.CREATEDATE,
                        Name = reg.NAME,
                        Sex = reg.SEX,
                        Age = reg.AGE ?? 0,
                        Floor = ipd_bed.FLOOR,
                        BedNo = ipd_bed.BEDNO,
                        ResidengNo = reg.RESIDENGNO,
                        OrgId = reg.ORGID,
                        Brithdate = reg.BRITHDATE,
                        RsType = reg_ipd.RSTYPE,
                        RsStatus = reg_ipd.RSSTATUS,
                        Race = reg.RACE,
                        Political = reg.POLITICAL,
                        IpdFlag = reg_ipd.IPDFLAG,
                        Relation = new Relation() { PhotoPath = reg_rel.PHOTOPATH },
                        FloorName = floor.FLOORID,
                        RoomName = room.ROOMNO

                    };

            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            q = from o in q where request.Data.FEENO.Contains(o.FeeNo.Value) select o;
            //20160927修改 Duke 排序按登记日期倒叙排列
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
        public BaseResponse<Person> GetPerson(int regNo)
        {
            var response = base.Get<LTC_REGFILE, Person>((q) => q.REGNO == regNo);
            BaseRequest<ResidentFilter> request = new BaseRequest<ResidentFilter>();
            request.Data.RegNo = regNo;

            var residentList = this.QueryResident(request);
            if (residentList.Data.Count > 0)
            {
                response.Data.FeeNo = residentList.Data[0].FeeNo;
                response.Data.RsType = residentList.Data[0].RsType;
                response.Data.RsStatus = residentList.Data[0].RsStatus;
            }
            response.Data.RegDisData = this.GetRegDisData(regNo).Data;
            return response;
        }
        public BaseResponse<PersonPreview> GetPersonExtend(int regNo)
        {//LTC_REGRELATION
            BaseResponse<PersonPreview> response = new BaseResponse<PersonPreview>();
            var q = from reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                    where reg.REGNO == regNo
                    join rel in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on reg.REGNO equals rel.REGNO into reg_rels
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on reg.REGNO equals ipd.REGNO into reg_ipds
                    join dtl in unitOfWork.GetRepository<LTC_REGINSDTL>().dbSet on reg.REGNO equals dtl.REGNO into reg_dtls
                    join lth in unitOfWork.GetRepository<LTC_REGHEALTH>().dbSet on reg.REGNO equals lth.REGNO into reg_lths
                    from reg_ipd in reg_ipds.DefaultIfEmpty()
                    join r in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on reg_ipd.FEENO equals r.FEENO into r_ipds
                    join ify in unitOfWork.GetRepository<LTC_IPDVERIFY>().dbSet on reg_ipd.FEENO equals ify.FEENO into reg_ifys
                    join car in unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet on reg_ipd.FEENO equals car.FEENO into reg_cars
                    join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on reg_ipd.FLOOR equals f.FLOORID into fs
                    join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on reg_ipd.ROOMNO equals r.ROOMNO into rs
                    from reg_rel in reg_rels.DefaultIfEmpty()
                    from r_ipd in r_ipds.DefaultIfEmpty()
                    from reg_dtl in reg_dtls.DefaultIfEmpty()
                    from reg_lth in reg_lths.DefaultIfEmpty()
                    from floor in fs.DefaultIfEmpty()
                    from room in rs.DefaultIfEmpty()
                    from reg_ify in reg_ifys.DefaultIfEmpty()
                    from reg_car in reg_cars.DefaultIfEmpty()
                    select new PersonPreview
                    {
                        RegNo = reg.REGNO,
                        IdNo = reg.IDNO,
                        Name = reg.NAME,
                        Sex = reg.SEX,
                        ImgUrl = reg_rel.PHOTOPATH,
                        Brithdate = reg.BRITHDATE,
                        FeeNo = reg_ipd.FEENO,
                        Skill = reg.SKILL,
                        Experience = reg.EXPERIENCE,
                        Habit = reg.HABIT,
                        ReligionCode = reg.RELIGIONCODE,
                        Education = reg.EDUCATION,
                        Race = reg.RACE,
                        Political = reg.POLITICAL,
                        MerryFlag = reg.MERRYFLAG,
                        Language = reg.LANGUAGE,
                        InDate = reg_ipd.INDATE,
                        StateFlag = reg_ipd.STATEFLAG,
                        Height = reg.HEIGHT,
                        ResidengNo = reg.RESIDENGNO,
                        servicetype = reg_ipd.SERVICETYPE,
                        City2 = r_ipd.CITY2,
                        Address2 = r_ipd.ADDRESS2,
                        Address2dtl = r_ipd.ADDRESS2DTL,
                        ContactPhone = r_ipd.CONTACTPHONE,
                        City1 = r_ipd.CITY1,
                        Address1 = r_ipd.ADDRESS1,
                        Address1dtl = r_ipd.ADDRESS1DTL,
                        Email = r_ipd.EMAIL,
                        StateReason = reg_ipd.STATEREASON,
                        Genogram = reg_rel.FAMILYPATH,
                        SOURCETYPE = reg_dtl.SOURCETYPE,
                        INSURANCEDESC = reg_dtl.INSURANCEDESC,
                        PROCDOC = reg_dtl.PROCDOC,
                        INSMARK = reg_dtl.INSMARK,
                        CASETYPE = reg_dtl.CASETYPE,
                        DISABDEGREE = reg_dtl.DISABDEGREE,
                        BOOKISSUEDATE = reg_dtl.BOOKISSUEDATE,
                        BOOKEXPDATE = reg_dtl.BOOKEXPDATE,
                        ISREEVAL = reg_dtl.ISREEVAL,
                        DISABILITYEVALDATE = reg_dtl.DISABILITYREEVALDATE,
                        ECONOMYFLAG = reg_dtl.ECONOMYFLAG,
                        ICDDIAGNOSI = reg_dtl.ICDDIAGNOSI,
                        DISEASEDIAG = reg.DISEASEDIAG,
                        BOOKTYPE = reg_dtl.BOOKTYPE,
                        EATHABITS = reg_lth.EATHABITS,
                        DISABILITYCATE1 = reg_dtl.DISABILITYCATE1,
                        DISABILITYCATE2 = reg_dtl.DISABILITYCATE2,
                        DISABILITYCATE3 = reg_dtl.DISABILITYCATE3,
                        DISABILITYCATE4 = reg_dtl.DISABILITYCATE4,
                        DISABILITYCATE5 = reg_dtl.DISABILITYCATE5,
                        DISABILITYCATE6 = reg_dtl.DISABILITYCATE6,
                        DISABILITYCATE7 = reg_dtl.DISABILITYCATE7,
                        DISABILITYCATE8 = reg_dtl.DISABILITYCATE8,
                        FloorName = floor.FLOORNAME,
                        RoomName = room.ROOMNAME,
                        SubsidyUnit = reg_ify.SUBSIDYUNIT,
                        AIDTOOLS = reg_car.AIDTOOLS
                    };
            response.Data = q.ToList().FirstOrDefault();
            if (response.Data != null)
            {
                response.Data.RelationDtl = unitOfWork.GetRepository<LTC_REGRELATIONDTL>().dbSet.Where(w => w.FEENO == response.Data.FeeNo).Select(s => new RelationDtl
                {
                    Name = s.NAME,
                    Sex = s.SEX,
                    Kinship = s.KINSHIP,
                    Address2 = s.ADDRESS2,
                    Contrel = s.CONTREL,
                    RelationType = s.RELATIONTYPE,
                    Phone = s.PHONE,
                    Address = s.ADDRESS,
                    WorkCode = s.WORKCODE,
                }).ToList();
                var requirements = unitOfWork.GetRepository<LTC_REGDEMAND>().dbSet.Where(w => w.FEENO == response.Data.FeeNo)
                    .OrderByDescending(d => d.RECORDDATE).
                    Select(s => s.CONTENT).FirstOrDefault();
                response.Data.requirements = requirements;
            }
            return response;
        }
        public BaseResponse<RegDisData> GetRegDisData(int regNo)
        {
            var response = new BaseResponse<RegDisData>()
            {
                Data = new RegDisData { },
            };
            response.Data.Regdiseasehis = base.Get<LTC_REGDISEASEHIS, Regdiseasehis>((q) => q.REGNO == regNo).Data;
            var request = new BaseRequest()
            {
                PageSize = 0,
            };
            response.Data.RegdiseasehisDtl = base.Query<LTC_REGDISEASEHISDTL, RegdiseasehisDtl>(request, (q) =>
            {
                q = q.Where(m => m.REGNO == regNo);
                return q;
            }).Data;
            return response;
        }
        public BaseResponse<IList<ScenarioMain>> GetScenario(int sNo)
        {
            BaseResponse<IList<ScenarioMain>> response = new BaseResponse<IList<ScenarioMain>>();
            Mapper.CreateMap<LTC_SCENARIO, ScenarioMain>();
            Mapper.CreateMap<LTC_SCENARIO_ITEM, ScenarioItem>();
            Mapper.CreateMap<LTC_SCENARIO_ITEM_OPTION, ScenarioItemOption>();

            var q = from m in unitOfWork.GetRepository<LTC_SCENARIO>().dbSet
                    select m;
            q = q.Where(m => m.SCENARIO == sNo);
            q = q.OrderBy(m => m.ID);
            List<LTC_SCENARIO> list = q.ToList();
            var data = new List<ScenarioMain>();
            var queItem = new ScenarioMain();
            foreach (var item in list)
            {
                queItem.Id = item.ID;
                queItem.Scenario = item.SCENARIO;
                queItem.CategoryId = item.CATEGORYID;
                queItem.CategoryName = item.CATEGORYNAME;
                queItem.ScenarioItem = Mapper.Map<IList<ScenarioItem>>(item.LTC_SCENARIO_ITEM);
                foreach (var subItem in item.LTC_SCENARIO_ITEM)
                {
                    foreach (var scenarioItem in queItem.ScenarioItem)
                    {
                        if (scenarioItem.Id == subItem.ID)
                        {
                            scenarioItem.ScenarioItemOption = Mapper.Map<IList<ScenarioItemOption>>(subItem.LTC_SCENARIO_ITEM_OPTION);
                        }
                    }
                }
                data.Add(queItem);
                queItem = new ScenarioMain();
            }

            response.Data = data;
            return response;
        }


        public BaseResponse<Person> SavePerson(Person request)
        {
            //if (request != null)
            //{
            //    var response = base.Get<LTC_REGFILE, Person>((q) => q.IDNO == request.IdNo && q.REGNO != request.FeeNo);
            //    if (response != null && response.Data != null)
            //    {
            //        BaseResponse<Person> person = new BaseResponse<Person>();
            //        person.ResultMessage = "该身份证重复,请重新输入！";
            //        response.ResultCode = (int)EnumResponseStatus.ExceptionHappened;
            //        return person;
            //    }
            //}

            if (request.RegNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.CreateDate = DateTime.Now;
                request.RegNo = int.Parse(base.GenerateCode("LCRegNo", EnumCodeKey.LCRegNo));
            }
            unitOfWork.BeginTransaction();
            var responsePerson = base.Save<LTC_REGFILE, Person>(request, (q) => q.REGNO == request.RegNo);
            if (request.Relation != null)
            {
                request.Relation.FeeNo = request.FeeNo ?? 0;
                request.Relation.RegNo = request.RegNo;
                request.Relation.OrgId = request.OrgId;
                this.SaveRelation(request.Relation);
            }
            if (request.RelationDtls != null)
            {
                request.RelationDtls.ForEach(m => m.FeeNo = request.FeeNo ?? 0);
                this.SaveRelationDtl(request.FeeNo ?? 0, request.RelationDtls);
            }
            if (request.AttachArchives != null)
            {
                request.AttachArchives.ForEach(m => { m.FeeNo = request.FeeNo ?? 0; m.RegNo = request.RegNo; m.OrgId = request.OrgId; });
                this.SaveAttachFile(request.FeeNo ?? 0, request.AttachArchives);
            }
            if (request.RegDisData != null)
            {
                if (request.RegDisData.Regdiseasehis != null)
                {
                    request.RegDisData.RegNo = request.RegNo;
                    this.SaveRegdiseasehis(request.RegDisData);
                }
            }
            unitOfWork.Commit();
            return responsePerson;
        }

        public BaseResponse DeletePerson(int regNO)
        {
            unitOfWork.BeginTransaction();
            var regRepository = unitOfWork.GetRepository<LTC_REGFILE>();
            var ipdRepository = unitOfWork.GetRepository<LTC_IPDREG>();
            var ipdRegoutRepository = unitOfWork.GetRepository<LTC_IPDREGOUT>();
            var leaveHospRepository = unitOfWork.GetRepository<LTC_LEAVEHOSP>();
            var regRelationRepository = unitOfWork.GetRepository<LTC_REGRELATION>();
            var regRelationDtlRepository = unitOfWork.GetRepository<LTC_REGRELATIONDTL>();
            var regAttachFileRepository = unitOfWork.GetRepository<LTC_REGATTACHFILE>();
            var familyDiscussRecRepository = unitOfWork.GetRepository<LTC_FAMILYDISCUSSREC>();
            var regHealthRepository = unitOfWork.GetRepository<LTC_REGHEALTH>();
            var reginsDtlRepository = unitOfWork.GetRepository<LTC_REGINSDTL>();
            var ipdVerifyRepository = unitOfWork.GetRepository<LTC_IPDVERIFY>();
            var ipdCloseCaseRepository = unitOfWork.GetRepository<LTC_IPDCLOSECASE>();
            var regDemandRepository = unitOfWork.GetRepository<LTC_REGDEMAND>();

            var ipdList = ipdRepository.dbSet.Where(m => m.REGNO == regNO).ToList();

            ipdList.ForEach(item =>
            {
                ipdRegoutRepository.Delete(m => m.FEENO == item.FEENO);
                leaveHospRepository.Delete(m => m.FEENO == item.FEENO);
                regRelationRepository.Delete(m => m.FEENO == item.FEENO);
                regRelationDtlRepository.Delete(m => m.FEENO == item.FEENO);
                regAttachFileRepository.Delete(m => m.FEENO == item.FEENO);
                familyDiscussRecRepository.Delete(m => m.FEENO == item.FEENO);
                regHealthRepository.Delete(m => m.FEENO == item.FEENO);
                reginsDtlRepository.Delete(m => m.FEENO == item.FEENO);
                ipdVerifyRepository.Delete(m => m.FEENO == item.FEENO);
                ipdCloseCaseRepository.Delete(m => m.FEENO == item.FEENO);
                regDemandRepository.Delete(m => m.FEENO == item.FEENO);

                ipdRepository.Delete(item);
            });
            var response = new BaseResponse();
            regRepository.Delete(regNO);

            var strSql = string.Format("Delete from LTC_REGDISEASEHISDTL where REGNO={0}", regNO);
            unitOfWork.GetRepository<LTC_REGDISEASEHISDTL>().ExecuteSqlCommand(strSql);


            strSql = string.Format("Delete from LTC_REGDISEASEHIS where REGNO={0}", regNO);
            unitOfWork.GetRepository<LTC_REGDISEASEHIS>().ExecuteSqlCommand(strSql);
            unitOfWork.Commit();
            return response;
        }
        #endregion
        #region 疾病史
        /// <summary>
        /// 查询病人疾病史数据
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<Regdiseasehis> QueryRegdiseasehis(int regNo)
        {
            return base.Get<LTC_REGDISEASEHIS, Regdiseasehis>((q) => q.REGNO == regNo);
        }

        /// <summary>
        /// 保存疾病史资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<RegDisData> SaveRegdiseasehis(RegDisData request)
        {
            BaseResponse<RegDisData> response = new BaseResponse<RegDisData>();
            Mapper.Reset();
            Mapper.CreateMap<Regdiseasehis, LTC_REGDISEASEHIS>();
            Mapper.CreateMap<LTC_REGDISEASEHIS, Regdiseasehis>();
            var model = unitOfWork.GetRepository<LTC_REGDISEASEHIS>().dbSet.FirstOrDefault(m => m.REGNO == request.RegNo);
            if (model == null)
            {
                model = Mapper.Map<LTC_REGDISEASEHIS>(request.Regdiseasehis);
                model.REGNO = request.RegNo;
                unitOfWork.GetRepository<LTC_REGDISEASEHIS>().Insert(model);
            }
            else
            {
                Mapper.Map(request.Regdiseasehis, model);
                unitOfWork.GetRepository<LTC_REGDISEASEHIS>().Update(model);
            }
            unitOfWork.Save();
            Mapper.Map(model, request.Regdiseasehis);

            var strSql = string.Format("Delete from LTC_REGDISEASEHISDTL where REGNO={0}", request.RegNo);
            unitOfWork.GetRepository<LTC_REGDISEASEHISDTL>().ExecuteSqlCommand(strSql);
            if (request.RegdiseasehisDtl != null)
            {
                Mapper.Reset();
                Mapper.CreateMap<RegdiseasehisDtl, LTC_REGDISEASEHISDTL>();
                Mapper.CreateMap<LTC_REGDISEASEHISDTL, RegdiseasehisDtl>();
                foreach (var item in request.RegdiseasehisDtl)
                {
                    var modelDetail = unitOfWork.GetRepository<LTC_REGDISEASEHISDTL>().dbSet.FirstOrDefault(m => m.ID == item.Id);
                    if (item.SickTime == DateTime.MinValue)
                    {
                        item.SickTime = null;
                    }
                    if (modelDetail == null)
                    {
                        modelDetail = Mapper.Map<LTC_REGDISEASEHISDTL>(item);
                        modelDetail.REGNO = request.RegNo;
                        unitOfWork.GetRepository<LTC_REGDISEASEHISDTL>().Insert(modelDetail);
                    }
                    else
                    {
                        Mapper.Map(item, modelDetail);
                        modelDetail.REGNO = request.RegNo;
                        unitOfWork.GetRepository<LTC_REGDISEASEHISDTL>().Update(modelDetail);
                    }
                    //unitOfWork.Save();
                    //Mapper.Map(modelDetail, item);
                }
                unitOfWork.Save();
            }
            response.Data = request;
            return response;
        }
        #endregion
        #region 入住
        public BaseResponse<IList<Resident>> QueryResident(BaseRequest<ResidentFilter> request)
        {
            var response = base.Query<LTC_IPDREG, Resident>(request, (q) =>
            {
                if (request.Data.RegNo.HasValue)
                {
                    q = q.Where(m => m.REGNO == request.Data.RegNo);
                }
                q = q.OrderByDescending(m => m.FEENO);
                return q;
            });
            return response;
        }

        /// <summary>
        /// 根据住民身份证查询 当前系统中所有机构存在的如准信息
        /// </summary>
        /// <param name="idNo">身份证号</param>
        /// <returns></returns>
        public BaseResponse<Resident> QueryLocaResInfo(string idNo)
        {
            BaseResponse<Resident> response = new BaseResponse<Resident>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                    join o in unitOfWork.GetRepository<LTC_ORG>().dbSet on ipd.ORGID equals o.ORGID into o_rs
                    from o_r in o_rs.DefaultIfEmpty()
                    select new Resident
                    {
                        FeeNo = ipd.FEENO,
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.NAME,
                        Sex = ipd_r.SEX,
                        Age = (int)ipd_r.AGE,
                        OrgId = ipd_r.ORGID,
                        IpdFlag = ipd.IPDFLAG,
                        IdNo = ipd_r.IDNO,
                        OrgName = o_r.ORGNAME,
                    };

            if (!string.IsNullOrEmpty(idNo))
            {
                q = q.Where(m => m.IdNo == idNo);
            }
            var info = q.ToList();
            response.Data = q.OrderByDescending(m => m.FeeNo).FirstOrDefault();
            if (response.Data != null)
            {
                if (response.Data.IpdFlag == "I" || response.Data.IpdFlag == "N")
                {
                    if (response.Data.OrgId == SecurityHelper.CurrentPrincipal.OrgId)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = "该个案已办理入院,请不要重复入院！";
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = "当前查询的身份证号在" + response.Data.OrgName + "处于入住状态，如需入住本院请到" + response.Data.OrgName + "办理离院手续。";
                    }
                }
                else
                {
                    response.Data =
                        q.Where(m => m.OrgId == SecurityHelper.CurrentPrincipal.OrgId && m.IpdFlag == "O")
                            .OrderByDescending(m => m.FeeNo)
                            .FirstOrDefault();
                }
            }
            return response;
        }

        public BaseResponse<IList<Resident>> QueryResidentExtend(BaseRequest<ResidentFilter> request)
        {
            string _orgId = SecurityHelper.CurrentPrincipal.OrgId;
            BaseResponse<IList<Resident>> response = new BaseResponse<IList<Resident>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { orgId = ipd.ORGID, floor = ipd.FLOOR } equals new { orgId = f.ORGID, floor = f.FLOORID } into ipd_f_set
                    from ipd_f in ipd_f_set.DefaultIfEmpty()
                    where ipd.ORGID == _orgId
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    //join bb in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bb.FEENO into ipd_bbs
                    join rel in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on ipd.FEENO equals rel.FEENO into reg_rels
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                        //from ipd_bb in ipd_bbs.DefaultIfEmpty()
                    from reg_rel in reg_rels.DefaultIfEmpty()
                    select new Resident
                    {
                        FeeNo = ipd.FEENO,
                        //FeeNoString = Convert.ToString(ipd.FEENO),
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.NAME,
                        ResidengNo = ipd_r.RESIDENGNO,
                        Sex = ipd_r.SEX,
                        Age = ipd_r.AGE ?? 0,
                        Floor = ipd.FLOOR,//ipd_bb.FLOOR,
                        FloorName = ipd_f.FLOORNAME,
                        BedNo = ipd.BEDNO,//ipd_bb.BEDNO,
                        OrgId = ipd_r.ORGID,
                        IpdFlag = ipd.IPDFLAG,
                        ImgUrl = reg_rel.PHOTOPATH,//Relation = new Relation() { PhotoPath = reg_rel.PHOTOPATH }
                        InDate = ipd.INDATE,
                        IdNo = ipd_r.IDNO,
                        ResidentInfo = (ipd_r.NAME ?? "") + (ipd_r.RESIDENGNO ?? "") + (ipd.BEDNO ?? ""),
                        BirthDay = ipd_r.BRITHDATE,
                        Height = ipd_r.HEIGHT,
                        Weight = ipd_r.WEIGHT,
                        DiseaseDiag = ipd_r.DISEASEDIAG,
                        FinancialCloseTime = ipd.FINANCIALCLOSETIME,
                        IsFinancialClose = ipd.ISFINANCIALCLOSE
                    };
            if (!string.IsNullOrEmpty(request.Data.IpdFlag))
            {
                if (request.Data.IpdFlag == "I")
                {
                    q = q.Where(m => m.IpdFlag == "I" || m.IpdFlag == "N");
                }
                else
                {
                    q = q.Where(m => m.IpdFlag == "O");
                }

            }
            if (request.Data.FeeNo.HasValue && !string.IsNullOrEmpty(request.Data.BedNo))
            {
                //q = q.Where(m => m.FeeNoString.Contains(request.Data.FeeNoString) || m.BedNo.Contains(request.Data.BedNo) || m.Name.Contains(request.Data.Name));
                q = q.Where(m => m.ResidengNo.Contains(request.Data.ResidengNo) || m.FeeNo == request.Data.FeeNo || m.BedNo.Contains(request.Data.BedNo));
            }
            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.Name.Contains(request.Data.Name));
            }

            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.IdNo == request.Data.IdNo);
            }
            if (!string.IsNullOrEmpty(request.Data.FloorId))
            {
                q = q.Where(m => m.Floor == request.Data.FloorId);
            }
            if (request.Data.FeeNo > 0)
            {
                q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            q = q.OrderByDescending(m => m.FeeNo);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.Distinct().ToList();
            }
            return response;
        }

        public BaseResponse<IList<Resident>> QueryResidentByName(BaseRequest<ResidentFilter> request)
        {
            BaseResponse<IList<Resident>> response = new BaseResponse<IList<Resident>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    join bb in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bb.FEENO into ipd_bbs
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                    from ipd_bb in ipd_bbs.DefaultIfEmpty()
                    join of in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { floorID = ipd_bb.FLOOR, OrgId = ipd_bb.ORGID } equals new { floorID = of.FLOORID, OrgId = of.ORGID } into ipd_bb_ofs
                    join or in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on new { RoomNo = ipd_bb.ROOMNO, OrgId = ipd_bb.ORGID } equals new { RoomNo = or.ROOMNO, OrgId = or.ORGID } into ipd_bb_oms
                    from ipd_bb_of in ipd_bb_ofs.DefaultIfEmpty()
                    from ipd_bb_om in ipd_bb_oms.DefaultIfEmpty()
                    where ipd.IPDFLAG == "I"
                    select new Resident
                    {
                        FeeNo = ipd.FEENO,
                        //FeeNoString = Convert.ToString(ipd.FEENO),
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.NAME,
                        Sex = ipd_r.SEX,
                        Age = ipd_r.AGE ?? 0,
                        Floor = ipd_bb.FLOOR,
                        FloorName = ipd_bb_of.FLOORNAME,
                        RoomNo = ipd_bb.ROOMNO,
                        RoomName = ipd_bb_om.ROOMNAME,
                        BedNo = ipd_bb.BEDNO,
                        OrgId = ipd_r.ORGID,
                        IpdFlag = ipd.IPDFLAG.Trim(),
                        IdNo = ipd_r.IDNO
                    };
            if (!string.IsNullOrEmpty(request.Data.IpdFlag))
            {
                if (request.Data.IpdFlag == "I")
                {
                    q = q.Where(m => m.IpdFlag == "I" || m.IpdFlag == "N");
                }
                else
                {
                    q = q.Where(m => m.IpdFlag == "O");
                }

            }
            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.Name.Contains(request.Data.Name));
            }
            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(m => m.IdNo == request.Data.IdNo);
            }
            if (!string.IsNullOrEmpty(request.Data.FloorName))
            {
                q = q.Where(m => m.FloorName.Contains(request.Data.FloorName));
            }
            if (!string.IsNullOrEmpty(request.Data.RoomName))
            {
                q = q.Where(m => m.RoomName.Contains(request.Data.RoomName));
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            q = q.OrderByDescending(m => m.FeeNo);

            if (q.Count() <= 0)
            {
                response.IsSuccess = false;
                response.ResultCode = 1001;
                response.ResultMessage = "当前房间没有住民！";
            }
            else
            {
                response.RecordsCount = q.Count();
                response.IsSuccess = true;
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

        public BaseResponse<IList<Resident>> QueryResidentByFloorAndRoom(BaseRequest<ResidentFilter> request)
        {
            BaseResponse<IList<Resident>> response = new BaseResponse<IList<Resident>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    join bb in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bb.FEENO into ipd_bbs
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                    from ipd_bb in ipd_bbs.DefaultIfEmpty()
                    join of in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { floorID = ipd_bb.FLOOR, OrgId = ipd_bb.ORGID } equals new { floorID = of.FLOORID, OrgId = of.ORGID } into ipd_bb_ofs
                    join or in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on new { RoomNo = ipd_bb.ROOMNO, OrgId = ipd_bb.ORGID } equals new { RoomNo = or.ROOMNO, OrgId = or.ORGID } into ipd_bb_oms
                    from ipd_bb_of in ipd_bb_ofs.DefaultIfEmpty()
                    from ipd_bb_om in ipd_bb_oms.DefaultIfEmpty()
                    where ipd.IPDFLAG == "I"
                    select new Resident
                    {
                        FeeNo = ipd.FEENO,
                        //FeeNoString = Convert.ToString(ipd.FEENO),
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.NAME,
                        Sex = ipd_r.SEX,
                        Age = ipd_r.AGE ?? 0,
                        Floor = ipd_bb.FLOOR,
                        FloorName = ipd_bb_of.FLOORNAME,
                        RoomNo = ipd_bb.ROOMNO,
                        RoomName = ipd_bb_om.ROOMNAME,
                        BedNo = ipd_bb.BEDNO,
                        OrgId = ipd_r.ORGID,
                        IpdFlag = ipd.IPDFLAG.Trim(),
                        IdNo = ipd_r.IDNO
                    };

            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            if (!string.IsNullOrEmpty(request.Data.FloorId))
            {
                q = q.Where(m => m.Floor == request.Data.FloorId);
            }
            if (!string.IsNullOrEmpty(request.Data.RoomNo))
            {
                q = q.Where(m => m.RoomNo == request.Data.RoomNo);
            }
            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(m => m.Name.ToUpper().Contains(request.Data.Name.ToUpper()));
            }
            q = q.OrderByDescending(m => m.FeeNo);

            if (q.Count() <= 0)
            {
                response.IsSuccess = false;
                response.ResultCode = 1001;
                response.ResultMessage = "没有符合条件的住民。";
            }
            else
            {
                response.RecordsCount = q.Count();
                response.IsSuccess = true;
                if (request != null && request.PageSize > 0)
                {
                    response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    response.Data = q.ToList();
                }

                if (response.Data != null && response.Data.Count != 0)
                {
                    for (int i = 0; i < response.Data.Count; i++)
                    {
                        response.Data[i].Age = CountAge(response.Data[i].IdNo);
                    }
                }
            }

            return response;
        }

        private int CountAge(string idno)
        {
            try
            {
                string Sub_str = idno.Substring(6, 8).Insert(4, "-").Insert(7, "-");	//提取出生年份
                TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(Sub_str));
                if (ts.Days > 0)
                {
                    return ts.Days / 365;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public BaseResponse<Resident> GetResident(long feeNO)
        {
            return base.Get<LTC_IPDREG, Resident>((q) => q.REGNO == feeNO);
        }

        public BaseResponse<object> GetResidentsForExtApiByIdNoList(List<string> idNoList)
        {
            if (idNoList == null || idNoList.Count == 0)
            {
                return null;
            }
            BaseResponse<object> response = new BaseResponse<object>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rs
                    join n in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(m=>m.STATUS==0) on ipd.FEENO equals n.FEENO
                    from ipd_r in ipd_rs.DefaultIfEmpty()
                    select new
                    {
                        FeeNo = ipd.FEENO,
                        RegNo = ipd_r.REGNO,
                        Name = ipd_r.NAME,
                        Sex = ipd_r.SEX,
                        OrgId = ipd_r.ORGID,
                        IpdFlag = ipd.IPDFLAG,
                        IdNo = ipd_r.IDNO,
                        CareType = n.CARETYPEID
                    };

            q = q.Where(m => idNoList.Contains(m.IdNo));

            //TODO, 改进转义方式
            response.Data = q.Select(m=> new
            {
                FeeNo = m.FeeNo,
                RegNo = m.RegNo,
                Name = m.Name,
                Sex = m.Sex == "F"? "女" : "男",
                OrgId = m.OrgId,
                IpdFlag = m.IpdFlag,
                IdNo = m.IdNo,
                CareType = m.CareType == "1" || m.CareType == "2" ? "专护" : "机构护理"
            }).ToList();
            return response;
        }

        public BaseResponse<Resident> SaveResident(Resident request)
        {
            if (!request.InDate.HasValue)
            {
                request.InDate = DateTime.Now;
            }
            var isNewResident = request.FeeNo == 0;

            var response = base.Save<LTC_IPDREG, Resident>(request, (q) => q.FEENO == request.FeeNo);
            if (response.Data == null)
            {
                return new BaseResponse<Resident>()
                {
                    ResultCode = -1,
                    ResultMessage = "保存住民记录失败!"
                };
            }

            if (isNewResident)
            {
                this.InitBalance(response.Data.FeeNo, request.IsHasNCI);
            }
            return response;
        }

        public BaseResponse DeleteResident(long feeNO)
        {
            return base.Delete<LTC_IPDREG>(feeNO);
        }
        private void InitBalance(long feeNo, bool isHasNCI)
        {
            var newBalance = new ResidentBalance()
            {
                //TODO BALANCEID暂无规则, 先用Guid代替
                BalanceID = Guid.NewGuid().ToString("N"),
                FeeNO = feeNo,
                Deposit = 0,
                Blance = 0,
                TotalPayment = 0,
                TotalCost = 0,
                TotalNCIPay = 0,
                TotalNCIOverspend = 0,
                IsHaveNCI = isHasNCI,
                Status = 0,
                Createby = SecurityHelper.CurrentPrincipal.EmpNo,
                CreateTime = DateTime.Now,
                UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo,
                UpdateTime = DateTime.Now,
                IsDelete = false
            };
            _residentBalanceService.Save(newBalance);
        }
        #endregion
        #region 住民在院证明
        public BaseResponse<RegHosProof> QueryRegHosProof(long feeNo)
        {
            var response = new BaseResponse<RegHosProof>();
            var codedtl = unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Where(m => m.ITEMTYPE == "L03.021");
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rf
                    join rr in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on ipd.FEENO equals rr.FEENO into ipd_rr
                    join ri in unitOfWork.GetRepository<LTC_REGINSDTL>().dbSet on ipd.FEENO equals ri.FEENO into ipd_ri
                    join ro in unitOfWork.GetRepository<LTC_ORG>().dbSet on ipd.ORGID equals ro.ORGID into ipd_ro
                    from ipd_rfile in ipd_rf.DefaultIfEmpty()
                    from ipd_rrlation in ipd_rr.DefaultIfEmpty()
                    from ipd_rinsdtl in ipd_ri.DefaultIfEmpty()
                    from ipd_rorg in ipd_ro.DefaultIfEmpty()
                    select new RegHosProof
                    {
                        Org = ipd_rorg.ORGNAME,
                        FeeNo = ipd.FEENO,
                        ResidengNo = ipd_rfile.RESIDENGNO,
                        Name = ipd_rfile.NAME,
                        Sex = ipd_rfile.SEX,
                        PermanentAddress = ipd_rrlation.ADDRESS1,
                        IdNo = ipd_rfile.IDNO,
                        DisabilityGrade = ipd_rinsdtl.DISABILITYGRADE,
                        BrithDay = ipd_rfile.BRITHDATE,
                        InDate = ipd.INDATE,
                    };
            q = q.Where(m => m.FeeNo == feeNo);
            response.Data = q.ToList().FirstOrDefault();
            if (!string.IsNullOrEmpty(response.Data.DisabilityGrade))
            {
                response.Data.DisabilityGrade = codedtl.Where(m => m.ITEMCODE == response.Data.DisabilityGrade).FirstOrDefault().ITEMNAME;
            }
            return response;
        }
        #endregion
        #region 入住审核
        public BaseResponse<IList<Verify>> QueryVerify(BaseRequest<VerifyFilter> request)
        {
            var response = base.Query<LTC_IPDVERIFY, Verify>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<Verify> GetVerify(long feeNo)
        {
            return base.Get<LTC_IPDVERIFY, Verify>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<Verify> SaveVerify(Verify request)
        {
            return base.Save<LTC_IPDVERIFY, Verify>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteVerify(long feeNo)
        {
            return base.Delete<LTC_IPDVERIFY>(feeNo);
        }
        #endregion

        #region 请假记录
        public BaseResponse<IList<LeaveHosp>> QueryLeaveHosp(BaseRequest<LeaveHospFilter> request)
        {
            var response = base.Query<LTC_LEAVEHOSP, LeaveHosp>(request, (q) =>
            {
                q = q.Where(m => m.ISDELETE == false);
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.STARTDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<LeaveHosp>> QueryLeaveHospList(DateTime sDate, DateTime eDate, string keyWord, int levStatus, int CurrentPage, int PageSize)
        {
            BaseResponse<IList<LeaveHosp>> response = new BaseResponse<IList<LeaveHosp>>();
            Mapper.CreateMap<LTC_LEAVEHOSP, LeaveHosp>();
            var q = from lhp in unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on lhp.FEENO equals ipd.FEENO into reg_lhps
                    from reg_lhp in reg_lhps.DefaultIfEmpty()
                        //where reg_lhp.IPDFLAG=="I"
                    join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on reg_lhp.REGNO equals reg.REGNO into reg_ipds
                    from r_ipd in reg_ipds.DefaultIfEmpty()
                    select new LeaveHosp
                    {
                        Id = lhp.ID,
                        ShowNumber = lhp.SHOWNUMBER,
                        FeeNo = lhp.FEENO,
                        ResidengNo = r_ipd.RESIDENGNO,
                        Name = r_ipd.NAME,
                        BedNo = reg_lhp.BEDNO,
                        StartDate = lhp.STARTDATE,
                        EndDate = lhp.ENDDATE,
                        LeHour = lhp.LEHOUR,
                        LeNote = lhp.LENOTE,
                        ReturnDate = lhp.RETURNDATE,
                        Address = lhp.ADDRESS,
                        ContName = lhp.CONTNAME,
                        ContRel = lhp.CONTREL,
                        ContTel = lhp.CONTTEL,
                        LeType = lhp.LETYPE,
                        CreateDate = lhp.CREATEDATE,
                        CreateBy = lhp.CREATEBY,
                        UpdateDate = lhp.UPDATEDATE,
                        UpdateBy = lhp.UPDATEBY,
                        OrgId = lhp.ORGID,
                        IpdFlag = reg_lhp.IPDFLAG,
                        IsDelete = lhp.ISDELETE,
                    };
            q = q.Where(m => m.IsDelete == false);
            eDate = eDate.AddDays(1);
            q = q.Where(m => m.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
            q = q.Where(m => m.StartDate >= sDate && m.StartDate < eDate);
            if (!string.IsNullOrEmpty(keyWord))
            {
                q = q.Where(m => m.Name.Contains(keyWord) || m.ResidengNo.Contains(keyWord));
            }
            if (levStatus == 1)
            {
                q = q.Where(m => m.ReturnDate == null);
            }
            q = q.OrderByDescending(m => m.StartDate);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<LeaveHosp>();
                foreach (dynamic item in list)
                {
                    LeaveHosp newItem = Mapper.DynamicMap<LeaveHosp>(item);
                    response.Data.Add(newItem);
                }
            };
            if (PageSize > 0)
            {
                var list = q.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                response.PagesCount = GetPagesCount(PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }



        public BaseResponse<IList<RegInHosStatusListEntity>> QueryRegInHosStatusList()
        {
            BaseResponse<IList<RegInHosStatusListEntity>> response = new BaseResponse<IList<RegInHosStatusListEntity>>();
            var q = from rci in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet
                    where rci.STATUS == 0
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on rci.FEENO equals ipd.FEENO
                    join lhp in unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet on ipd.FEENO equals lhp.FEENO into regleavehos
                    from rel in regleavehos.DefaultIfEmpty()
                    where ((rel.STARTDATE <= DateTime.Now && rel.RETURNDATE >= DateTime.Now) || (rel.STARTDATE == null && rel.RETURNDATE == null) || (rel.RETURNDATE < DateTime.Now)) && rel.ISDELETE != true
                    join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO
                    select new RegInHosStatusListEntity
                    {
                        Name = reg.NAME,
                        IdNo = reg.IDNO,
                        IpdFlag = ipd.IPDFLAG,
                        InDate = ipd.INDATE,
                        OutDate = ipd.OUTDATE,
                        StartDate = rel.STARTDATE,
                        ReturnDate = rel.RETURNDATE,
                        LeHour = rel.LEHOUR,
                    };
            response.RecordsCount = q.Count();
            response.Data = q.ToList();

            return response;
        }

        /// <summary>
        /// 查询住民最新一笔请假记录
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>离院列表</returns>
        public BaseResponse<IList<LeaveHosp>> GetNewLeaveHosp(BaseRequest<LeaveHospFilter> request)
        {

            var response = base.Query<LTC_LEAVEHOSP, LeaveHosp>(request, (q) =>
            {
                q = q.Where(m => m.ISDELETE == false);
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.STARTDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<LeaveHosp> GetLeaveHosp(long leaveHospId)
        {
            return base.Get<LTC_LEAVEHOSP, LeaveHosp>((q) => q.ID == leaveHospId);
        }

        #region 请假逻辑
        public BaseResponse<LeaveHosp> SaveLeaveHosp(LeaveHosp request)
        {

            #region 移除管路信息
            var keys = new[] { "001", "002", "003", "004", "005" };
            for (int i = 0; i < 5; i++)
            {
                ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
                socialWorkerManageService.RemovePipelineRec(Convert.ToInt32(request.FeeNo), keys[i], Convert.ToDateTime(request.StartDate), "请假外出自动移除");
            }
            #endregion

            if (request.Id == 0)
            {
                request.IsDelete = false;
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.ShowNumber = int.Parse(base.GenerateCode(request.OrgId, EnumCodeKey.LeaveHospId));
            }
            var leaveinfo = base.Save<LTC_LEAVEHOSP, LeaveHosp>(request, (q) => q.ID == request.Id);
            SaveLeaveDeo(leaveinfo);
            return leaveinfo;
        }

        public void SaveLeaveDeo(BaseResponse<LeaveHosp> request)
        {
            var keyDateList = GetDiffDateRangeExtend(request.Data.StartDate.Value, request.Data.ReturnDate.Value);
            foreach (var item in keyDateList)
            {
                var deduction = new NCIDeductionModel();
                deduction.DeductionType = (int)DeductionType.LeaveHosp;
                deduction.Debitmonth = GetLeaveYearMonth(item.startDate);
                deduction.Leaveid = request.Data.Id;
                deduction.Amount = 0;
                deduction.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                deduction.CreatTime = DateTime.Now;
                deduction.IsDelete = false;
                deduction.DeductionStatus = (int)DeductionStatus.UnCharge;
                deduction.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                deduction.Debitdays = GetDiffDateDays(item.startDate, item.endtDate);
                base.Save<LTC_NCIDEDUCTION, NCIDeductionModel>(deduction, (q) => q.ID == deduction.ID);
            }
        }
        #endregion

        public BaseResponse DeleteLeaveHosp(long leaveHospId)
        {
            var response = new BaseResponse();
            if (leaveHospId != 0)
            {
                #region 逻辑删除扣款记录表数据

                #endregion
                var model = unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet.Where(m => m.LEAVEID == leaveHospId).ToList();
                Mapper.CreateMap<LTC_NCIDEDUCTION, NCIDeductionModel>();
                var edductionList = Mapper.Map<List<NCIDeductionModel>>(model);

                if (edductionList != null && edductionList.Count > 0)
                {
                    foreach (var item in edductionList)
                    {
                        item.IsDelete = true;
                        item.Updateby = SecurityHelper.CurrentPrincipal.EmpNo;
                        item.UpdateTime = DateTime.Now;
                        base.Save<LTC_NCIDEDUCTION, NCIDeductionModel>(item, (q) => q.ID == item.ID);
                    }
                }
            }
            var findItem = unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet.Where(m => m.ID == leaveHospId).FirstOrDefault();
            if (findItem != null)
            {
                findItem.ISDELETE = true;
                unitOfWork.Save();
            }
            return response;
        }

        #region 离院辅助方法
        /// <summary>
        /// 计算请假的天数，大于等于 6 小时的时候按照一天计算
        /// </summary>
        /// <param name="dt1">时间1</param>
        /// <param name="dt2">时间2</param>
        /// <returns>相差天数</returns>
        public int GetDiffDateDays(DateTime dt1, DateTime dt2)
        {
            var days = 0;
            TimeSpan ts = dt2 - dt1;
            days = ts.Hours >= 6 ? ts.Days + 1 : ts.Days;
            return days;
        }

        public int GetDiffDays(DateTime dt1, DateTime dt2)
        {
            var days = 0;
            TimeSpan ts = dt2 - dt1;
            days = ts.Days + 1;
            return days;
        }

        public int GetIpdDiffDays(DateTime dt1, DateTime dt2)
        {
            var days = 0;
            TimeSpan ts = dt2 - dt1;
            days = ts.Days;
            return days;
        }

        /// <summary>
        ///  获取时间范围
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>时间范围集合</returns>
        public static List<KeyValueForDate> GetDiffDateRange(DateTime startTime, DateTime endTime)
        {
            List<KeyValueForDate> keyDateList = new List<KeyValueForDate>();
            DateTime dt1 = startTime;
            DateTime dt2 = endTime;
            while (dt1 <= dt2)
            {
                KeyValueForDate keyDate = new KeyValueForDate();
                DateTime dd = new DateTime(dt1.Year, dt1.Month, 1);
                int lastDay = dd.AddMonths(1).AddDays(-1).Day;
                DateTime monthEnd = new DateTime(dt1.Year, dt1.Month, lastDay);

                if (dt1.Year == dt2.Year && dt1.Month == dt2.Month)
                {
                    keyDate.startDate = dt1;
                    keyDate.endtDate = dt2;
                    keyDateList.Add(keyDate);
                    return keyDateList;
                }
                else
                {
                    keyDate.startDate = dt1;
                    keyDate.endtDate = monthEnd;
                    keyDateList.Add(keyDate);
                }
                dt1 = dt1.AddMonths(1);
                dt1 = new DateTime(dt1.Year, dt1.Month, 1);
            }
            return keyDateList;
        }

        public string GetLeaveYearMonth(DateTime dt)
        {
            var IntervalEndDate = DictHelper.GetFeeIntervalEndDateByDate(dt);
           // var eDt = new DateTime(dt.Year, 12, 20);
           // return dt > eDt? dt.Year + 1 + "-" + IntervalEndDate.ToString("MM"): dt.Year + "-" + IntervalEndDate.ToString("MM");、
            return IntervalEndDate.ToString("yyyy-MM");
        }

        public List<KeyValueForDate> GetDiffDateRangeExtend(DateTime startTime, DateTime endTime)
        {
            List<KeyValueForDate> keyDateList = new List<KeyValueForDate>();
            DateTime dt1 = startTime;
            DateTime dt2 = endTime;
            var nowdt = DateTime.Now;
            while (dt1 < dt2)
            {
                KeyValueForDate keyDate = new KeyValueForDate();
                var byDt = new DateTime(dt1.Year, dt1.Month, dt1.Day);
                var IntervalStartDateByDate = DictHelper.GetFeeIntervalStartDateByDate(byDt);
                var IntervalEndDate = DictHelper.GetFeeIntervalEndDateByDate(byDt);
                keyDate.startDate = IntervalStartDateByDate > dt1 ? IntervalStartDateByDate : dt1;
                keyDate.endtDate = IntervalEndDate.AddDays(1).AddSeconds(-1) >= dt2 ? dt2 : IntervalEndDate.AddDays(1).AddSeconds(-1);
                dt1 = IntervalEndDate.AddDays(1);
                keyDateList.Add(keyDate);
            }
            return keyDateList;
        }



        public class KeyValueForDate
        {
            public DateTime startDate { get; set; }

            public DateTime endtDate { get; set; }
        }
        #endregion
        #endregion

        #region 零用金
        public BaseResponse<IList<Deposit>> QueryDeposit(BaseRequest<DepositFilter> request)
        {
            var response = base.Query<LTC_DEPTFILE, Deposit>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                //if (request.Data.FeeNo.HasValue)
                //{
                //    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                //}
                q = q.OrderByDescending(m => m.UPDATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<Deposit> GetDeposit(string deptNo)
        {
            return base.Get<LTC_DEPTFILE, Deposit>((q) => q.DEPTNO == deptNo);
        }

        public BaseResponse<Deposit> SaveDeposit(Deposit request)
        {
            return base.Save<LTC_DEPTFILE, Deposit>(request, (q) => q.DEPTNO == request.DeptNo);
        }

        public BaseResponse DeleteDeposit(string deptNo)
        {
            return base.Delete<LTC_DEPTFILE>(deptNo);
        }
        #endregion

        #region 出院结案
        public BaseResponse<IList<CloseCase>> QueryCloseCase(BaseRequest<CloseCaseFilter> request)
        {
            var response = base.Query<LTC_IPDCLOSECASE, CloseCase>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(m => m.ORGID == request.Data.OrgId);
                }
                q = q.OrderByDescending(m => m.CLOSEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<CloseCase> GetCloseCase(long feeNo)
        {
            return base.Get<LTC_IPDCLOSECASE, CloseCase>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<CloseCase> SaveCloseCase(CloseCase request)
        {
            return base.Save<LTC_IPDCLOSECASE, CloseCase>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse DeleteCloseCase(long feeNo)
        {
            return base.Delete<LTC_IPDCLOSECASE>(feeNo);
        }
        #endregion

        #region 社会福利
        public BaseResponse<IList<ResidentDtl>> QueryResidentDtl(BaseRequest<ResidentDtlFilter> request)
        {
            var response = base.Query<LTC_REGINSDTL, ResidentDtl>(request, (q) =>
            {
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<ResidentDtl> GetResidentDtl(long feeNo)
        {
            return base.Get<LTC_REGINSDTL, ResidentDtl>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<ResidentDtl> SaveResidentDtl(ResidentDtl request)
        {
            return base.Save<LTC_REGINSDTL, ResidentDtl>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse DeleteResidentDtl(long feeNo)
        {
            return base.Delete<LTC_REGINSDTL>(feeNo);
        }
        #endregion

        #region 需求管理
        public BaseResponse<IList<Demand>> QueryDemand(BaseRequest<DemandFilter> request)
        {
            var response = base.Query<LTC_REGDEMAND, Demand>(request, (q) =>
            {
                q = q.Where(m => m.FEENO == request.Data.FeeNO);
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<Demand> GetDemand(long id)
        {
            return base.Get<LTC_REGDEMAND, Demand>((q) => q.ID == id);
        }

        public BaseResponse<Demand> SaveDemand(Demand request)
        {
            return base.Save<LTC_REGDEMAND, Demand>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteDemand(long id)
        {
            return base.Delete<LTC_REGDEMAND>(id);
        }
        #endregion

        #region 健康管理
        public BaseResponse<IList<Health>> QueryHealth(BaseRequest<HealthFilter> request)
        {
            var response = base.Query<LTC_REGHEALTH, Health>(request, (q) =>
            {
                q = q.OrderByDescending(m => m.FEENO);
                return q;
            });
            return response;
        }

        public BaseResponse<Health> GetHealth(long feeNo)
        {
            BaseResponse<Health> health = base.Get<LTC_REGHEALTH, Health>((q) => q.FEENO == feeNo);

            if (health.Data == null)
            {

                LTC_CAREDEMANDEVAL careDemand = unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet.Where(x => x.FEENO == feeNo).OrderByDescending(x => x.EVALDATE).FirstOrDefault();
                if (careDemand != null)
                {
                    health = new BaseResponse<Health>();
                    health.Data = new Health();
                    health.Data.ALLERGY = string.Format("药物过敏史:{0}\r食物过敏:{1}\r其他过敏:{2}", careDemand.ALLERGY_DRUG, careDemand.ALLERGY_FOOD, careDemand.ALLERGY_OTHERS);
                }
            }

            return health;
        }

        public BaseResponse<Health> SaveHealth(Health request)
        {
            return base.Save<LTC_REGHEALTH, Health>(request, (q) => q.FEENO == request.FEENO);
        }

        public BaseResponse DeleteHealth(long feeNo)
        {
            return base.Delete<LTC_REGHEALTH>(feeNo);
        }
        #endregion

        #region 附加文件
        public BaseResponse<IList<AttachFile>> QueryAttachFile(BaseRequest<AttachFileFilter> request)
        {
            var response = base.Query<LTC_REGATTACHFILE, AttachFile>(request, (q) =>
            {
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<AttachFile> GetAttachFile(long feeNo)
        {
            return base.Get<LTC_REGATTACHFILE, AttachFile>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<AttachFile> SaveAttachFile(AttachFile request)
        {
            return base.Save<LTC_REGATTACHFILE, AttachFile>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse<List<AttachFile>> SaveAttachFile(long feeNo, List<AttachFile> request)
        {
            Mapper.CreateMap<AttachFile, LTC_REGATTACHFILE>();
            var attachFileRep = unitOfWork.GetRepository<LTC_REGATTACHFILE>();
            attachFileRep.dbSet.Where(m => m.FEENO == feeNo)
                .Select(m => m)
                .ToList()
                .ForEach(m => attachFileRep.Delete(m));



            var nowTime = DateTime.Now;

            BaseResponse<List<AttachFile>> response = new BaseResponse<List<AttachFile>>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_REGATTACHFILE>(m);
                if (!model.CREATEDATE.HasValue)
                {
                    model.CREATEDATE = nowTime;
                }
                if (string.IsNullOrEmpty(model.CREATEBY))
                {
                    model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                }
                attachFileRep.Insert(model);
            });
            unitOfWork.Save();
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteAttachFile(long Id)
        {
            return base.Delete<LTC_REGATTACHFILE>(Id);
        }
        #endregion

        #region 通信录住民地址
        public BaseResponse<IList<Relation>> QueryRelation(BaseRequest<RelationFilter> request)
        {
            var response = base.Query<LTC_REGRELATION, Relation>(request, (q) =>
            {
                q = q.OrderByDescending(m => m.FEENO);
                return q;
            });
            return response;
        }

        public BaseResponse<Relation> GetRelation(long feeNo)
        {
            return base.Get<LTC_REGRELATION, Relation>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<Relation> SaveRelation(Relation request)
        {
            if (request.FeeNo == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
            }
            return base.Save<LTC_REGRELATION, Relation>(request, (q) => q.FEENO == request.FeeNo);
        }

        public BaseResponse DeleteRelation(long feeNo)
        {
            return base.Delete<LTC_REGRELATION>(feeNo);
        }
        #endregion

        #region 通信录亲属地址
        public BaseResponse<IList<RelationDtl>> QueryRelationDtl(BaseRequest<RelationDtlFilter> request)
        {
            var response = base.Query<LTC_REGRELATIONDTL, RelationDtl>(request, (q) =>
            {
                if (request.Data.FeeNo.HasValue)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.CREATEDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<RelationDtl> GetRelationDtl(long feeNo)
        {
            return base.Get<LTC_REGRELATIONDTL, RelationDtl>((q) => q.FEENO == feeNo);
        }

        public BaseResponse<RelationDtl> SaveRelationDtl(RelationDtl request)
        {
            if (!request.FeeNo.HasValue)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
            }
            return base.Save<LTC_REGRELATIONDTL, RelationDtl>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse<List<RelationDtl>> SaveRelationDtl(long feeNo, List<RelationDtl> request)
        {
            unitOfWork.BeginTransaction();
            Mapper.CreateMap<RelationDtl, LTC_REGRELATIONDTL>();
            var relationDtlRep = unitOfWork.GetRepository<LTC_REGRELATIONDTL>();
            relationDtlRep.dbSet.Where(m => m.FEENO == feeNo)
                .Select(m => m)
                .ToList()
                .ForEach(m => relationDtlRep.Delete(m));

            var nowTime = DateTime.Now;
            BaseResponse<List<RelationDtl>> response = new BaseResponse<List<RelationDtl>>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_REGRELATIONDTL>(m);
                model.CREATEDATE = nowTime;
                model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                relationDtlRep.Insert(model);
            });
            unitOfWork.Save();
            response.Data = request;

            var lastEditItem = request.Where(m => m.EconomyFlag).OrderByDescending(m => m.UpdateDate).FirstOrDefault();
            if (lastEditItem != null)
            {
                Relation relation = new Relation();
                relation.PaymentPerson = lastEditItem.Name;
                relation.BillAddress = lastEditItem.Address2;
                relation.Kinship = lastEditItem.Kinship;
                base.Save<LTC_REGRELATION, Relation>(relation, (q) => q.FEENO == feeNo, new List<string> { "PaymentPerson", "BillAddress", "Kinship" });
            }

            unitOfWork.Commit();
            return response;
        }

        public BaseResponse DeleteRelationDtl(long feeNo)
        {
            return base.Delete<LTC_REGRELATIONDTL>(feeNo);
        }

        public bool ExistResident(long regNo, string[] status)
        {
            return base.unitOfWork.GetRepository<LTC_IPDREG>().Exists(o => o.REGNO == regNo && status.Contains(o.IPDFLAG));
        }

        #endregion

        #region 住民访视
        public BaseResponse<IList<FamilyDiscuss>> QueryFamilyDiscuss(BaseRequest<FamilyDiscussFilter> request)
        {
            var response = base.Query<LTC_FAMILYDISCUSSREC, FamilyDiscuss>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.STARTDATE);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<FamilyDiscuss>> QueryFamilyDiscussExtend(BaseRequest<FamilyDiscussFilter> request)
        {
            BaseResponse<IList<FamilyDiscuss>> response = new BaseResponse<IList<FamilyDiscuss>>();
            var q = from f in unitOfWork.GetRepository<LTC_FAMILYDISCUSSREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on f.RECORDBY equals e.EMPNO into ees
                    from ee in ees.DefaultIfEmpty()
                    select new FamilyDiscuss
                    {
                        Id = f.ID,
                        FeeNo = f.FEENO,
                        //RegNo = f.REGNO,
                        RecordBy = f.RECORDBY,
                        RecordByShow = ee.EMPNAME,
                        StartDate = f.STARTDATE,
                        EndDate = f.ENDDATE,
                        VisitType = f.VISITTYPE,
                        VisitorName = f.VISITORNAME,
                        VisitorSex = f.VISITORSEX,
                        VisitorIdNo = f.VISITORIDNO,
                        VisitorCompany = f.VISITORCOMPANY,
                        IsRegVisit = f.ISREGVISIT,
                        Remark = f.REMARK,
                        Interviewee = f.INTERVIEWEE,
                        Appellation = f.APPELLATION,
                        BloodRelationShip = f.BLOODRELATIONSHIP,
                        //BodyTemp = f.BODYTEMP,
                        //IsGoAbroad = f.ISGOABROAD,
                        //GoAbroadPlace = f.GOABROADPLACE,
                        Description = f.DESCRIPTION,
                        OrgId = f.ORGID
                    };

            if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
            {
                q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            q = q.OrderByDescending(m => m.StartDate);
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

        public BaseResponse<FamilyDiscuss> GetFamilyDiscuss(int Id)
        {
            return base.Get<LTC_FAMILYDISCUSSREC, FamilyDiscuss>((q) => q.ID == Id);
        }

        public BaseResponse<FamilyDiscuss> SaveFamilyDiscuss(FamilyDiscuss request)
        {
            request.IsRegVisit = true;
            if (request.Id == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_FAMILYDISCUSSREC, FamilyDiscuss>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteFamilyDiscuss(int Id)
        {
            return base.Delete<LTC_FAMILYDISCUSSREC>(Id);
        }
        #endregion

        #region 预约登记
        public BaseResponse<IList<Preipd>> QueryPreipd(BaseRequest<PreipdFilter> request)
        {
            var response = new BaseResponse<IList<Preipd>>();
            var q = from pr in unitOfWork.GetRepository<LTC_PREIPD>().dbSet
                    join dp in unitOfWork.GetRepository<LTC_DEPTFILE>().dbSet on pr.DEPTNO equals dp.DEPTNO into dp_p
                    from dp_part in dp_p.DefaultIfEmpty()
                    select new
                    {
                        preipd = pr,
                        DeptName = dp_part.DEPTNAME

                    };
            q = q.Where(m => m.preipd.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
            q = q.OrderByDescending(m => m.preipd.PREFEENO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<Preipd>();
                foreach (dynamic item in list)
                {
                    Preipd newItem = Mapper.DynamicMap<Preipd>(item.preipd);
                    newItem.DeptName = item.DeptName;
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

            //var response = base.Query<LTC_PREIPD, Preipd>(request, (q) =>
            //{
            //    q = q.OrderByDescending(m => m.PREFEENO);
            //    return q;
            //});
            //return response;
        }

        public BaseResponse<Preipd> SavePreipd(Preipd request)
        {
            return base.Save<LTC_PREIPD, Preipd>(request, (q) => q.PREFEENO == request.PreFeeNo);
        }

        public BaseResponse DeletePreipd(long PreFeeNo)
        {
            return base.Delete<LTC_PREIPD>(PreFeeNo);
        }
        #endregion

        #region 出院办理
        public BaseResponse<Ipdregout> GetIpdregout(long feeNo)
        {
            return base.Get<LTC_IPDREGOUT, Ipdregout>((q) => q.FEENO == feeNo);
        }

        public BaseResponse QueryIpdBillInfo(int feeNo, DateTime? ipdoutTime)
        {
            var response = new BaseResponse();
            #region 检验是否存在护理险为结算费用记录信息
            IFeeRecordService feeRecordservice = IOCContainer.Instance.Resolve<IFeeRecordService>();
            var odate = new DateTime(1, 1, 1);
            var edaye = new DateTime(9999, 12, 31);
            BaseRequest<FeeRecordFilter> frRequest = new BaseRequest<FeeRecordFilter>
            {
                Data = { FeeNo = feeNo, SDate = odate, EDate = edaye, TaskStatus = "R0,R1,R2" }
            };

            var frResponse = feeRecordservice.QueryNotGenerateBillRecord(frRequest);
            if (frResponse.Data != null && frResponse.Data.Count > 0)
            {
                response.ResultCode = -1;
                response.ResultMessage = "当前住民存在未生成账单的费用记录数据， 请先生成账单后进行结案作业";
                return response;
            }
            #endregion

            #region 检验是否存在护理险为账单未结算的记录信息
            IBillV2Service billservice = IOCContainer.Instance.Resolve<IBillV2Service>();
            BaseRequest<BillV2Filter> billrequest = new BaseRequest<BillV2Filter>
            {
                CurrentPage = 1,
                PageSize = 1000,
                Data = { FeeNo = feeNo, StarDate = odate, EndDate = edaye }
            };
            var billResponse = billservice.QueryBillV2(billrequest);
            if (billResponse.Data != null && billResponse.Data.Count > 0)
            {
                var billList = billResponse.Data.Where(m => m.Status == (int)BillStatus.NoCharge).ToList();
                if (billList != null && billList.Count > 0)
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "当前住民存在未缴费完成的账单数据， 请先核实账单信息后进行结案作业";
                    return response;
                }
            }
            #endregion
            return response;
        }

        public BaseResponse<Ipdregout> SaveIpdregout(Ipdregout request)
        {
            var response = new BaseResponse<Ipdregout>();
            var cm = Mapper.CreateMap<Ipdregout, LTC_IPDREGOUT>();
            Mapper.CreateMap<LTC_IPDREGOUT, Ipdregout>();
            var reqQueIpdregout = unitOfWork.GetRepository<LTC_IPDREGOUT>().dbSet.Where(m => m.FEENO == request.FeeNo).FirstOrDefault();
            var reqQueCloseCase = unitOfWork.GetRepository<LTC_IPDCLOSECASE>().dbSet.Where(m => m.FEENO == request.FeeNo).FirstOrDefault();
            var reqQueResident = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == request.FeeNo).FirstOrDefault();
            var reqQueBedBasic = unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet.Where(m => m.FEENO == request.FeeNo).FirstOrDefault();
            DateTime? _inDate = new DateTime();
            //清床位信息
            if (reqQueBedBasic != null)
            {
                reqQueBedBasic.FEENO = null;
                reqQueBedBasic.BEDSTATUS = BedStatus.Empty.ToString();
                unitOfWork.GetRepository<LTC_BEDBASIC>().Update(reqQueBedBasic);
            }

            #region 移除管路信息
            var keys = new[] { "001", "002", "003", "004", "005" };
            for (int i = 0; i < 5; i++)
            {
                ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
                socialWorkerManageService.RemovePipelineRec(request.FeeNo, keys[i], DateTime.Now, "结案作业自动移除");
            }
            #endregion



            //更新住民信息表
            if (reqQueResident != null)
            {
                reqQueResident.IPDFLAG = "O";//状态更新为 O 结案
                reqQueResident.OUTDATE = request.CloseDate;//出院日期更新为结案日期
                unitOfWork.GetRepository<LTC_IPDREG>().Update(reqQueResident);
                _inDate = reqQueResident.INDATE;
            }
            //
            if (reqQueCloseCase == null)
            {
                reqQueCloseCase = new LTC_IPDCLOSECASE();
                reqQueCloseCase.FEENO = request.FeeNo;
                reqQueCloseCase.CLOSEFLAG = request.CloseFlag;//结案状态
                reqQueCloseCase.CLOSEDATE = request.CloseDate;//结案日期
                reqQueCloseCase.CLOSEREASON = request.CloseReason;//结案原因
                unitOfWork.GetRepository<LTC_IPDCLOSECASE>().Insert(reqQueCloseCase);
            }
            else
            {
                reqQueCloseCase.CLOSEFLAG = request.CloseFlag;
                reqQueCloseCase.CLOSEDATE = request.CloseDate;
                reqQueCloseCase.CLOSEREASON = request.CloseReason;
                unitOfWork.GetRepository<LTC_IPDCLOSECASE>().Update(reqQueCloseCase);
            }
            //
            if (reqQueIpdregout == null)
            {
                reqQueIpdregout = Mapper.Map<LTC_IPDREGOUT>(request);
                reqQueIpdregout.INDATE = _inDate;
                unitOfWork.GetRepository<LTC_IPDREGOUT>().Insert(reqQueIpdregout);
            }
            else
            {
                reqQueIpdregout.INDATE = _inDate;
                Mapper.Map(request, reqQueIpdregout);
                unitOfWork.GetRepository<LTC_IPDREGOUT>().Update(reqQueIpdregout);
            }

            var regnci = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(x => x.FEENO == request.FeeNo && x.STATUS == 0).FirstOrDefault();
            if (regnci != null)
            {
                regnci.STATUS = 1;
                regnci.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                regnci.UPDATETIME = DateTime.Now;
                unitOfWork.GetRepository<LTC_REGNCIINFO>().Update(regnci);
            }

            unitOfWork.Save();
            Mapper.Map(reqQueIpdregout, request);
            response.Data = request;
            return response;


            //CloseCase requestCloseCase;
            //BaseResponse<CloseCase> brCloseCase = GetCloseCase(request.FeeNo);
            //Resident resident = base.Get<LTC_IPDREG, Resident>((q) => q.FEENO == request.FeeNo).Data;
            //resident.IpdFlag = "O";
            //if (brCloseCase.Data != null)
            //{
            //    requestCloseCase = brCloseCase.Data;
            //    requestCloseCase.CloseFlag = request.CloseFlag;
            //    requestCloseCase.CloseDate = request.CloseDate;
            //    requestCloseCase.CloseReason = request.CloseReason;
            //}
            //else
            //{
            //    requestCloseCase = new CloseCase
            //   {
            //       FeeNo = request.FeeNo,
            //       CloseFlag = request.CloseFlag,
            //       CloseDate = request.CloseDate,
            //       CloseReason = request.CloseReason,
            //   };
            //}
            //request.InDate = resident.InDate;
            //BaseResponse<Ipdregout> baseRes = base.Save<LTC_IPDREGOUT, Ipdregout>(request, (q) => q.FEENO == request.FeeNo);
            //base.Save<LTC_IPDREG, Resident>(resident, (q) => q.FEENO == resident.FeeNo);
            //base.Save<LTC_IPDCLOSECASE, CloseCase>(requestCloseCase, (q) => q.FEENO == request.FeeNo);
            //return baseRes;
        }
        #endregion

        #region 退住院
        public BaseResponse<Resident> GetLeaveNursing(long feeNo)
        {
            var filter = new ResidentDtlFilter
            {
                FeeNo = feeNo
            };
            var request = new BaseRequest<ResidentDtlFilter>
            {
                Data = filter
            };
            var br = new BaseResponse<Resident>();
            var response = new BaseResponse<IList<Resident>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join e1 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.CARER equals e1.EMPNO into ipd_e1
                    join e2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NURSENO equals e2.EMPNO into ipd_e2
                    join e3 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NUTRITIONIST equals e3.EMPNO into ipd_e3
                    join e4 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.PHYSIOTHERAPIST equals e4.EMPNO into ipd_e4
                    join e5 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.DOCTOR equals e5.EMPNO into ipd_e5
                    join bed in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bed.FEENO into ipd_bedb
                    from ipd_ename1 in ipd_e1.DefaultIfEmpty()
                    from ipd_ename2 in ipd_e2.DefaultIfEmpty()
                    from ipd_ename3 in ipd_e3.DefaultIfEmpty()
                    from ipd_ename4 in ipd_e4.DefaultIfEmpty()
                    from ipd_ename5 in ipd_e5.DefaultIfEmpty()
                    from ipd_bed in ipd_bedb.DefaultIfEmpty()
                    select new
                    {

                        resident = ipd,
                        CarerName = ipd_ename1.EMPNAME,
                        NurseName = ipd_ename2.EMPNAME,
                        NutritionistName = ipd_ename3.EMPNAME,
                        PhysiotherapistName = ipd_ename4.EMPNAME,
                        DoctorName = ipd_ename5.EMPNAME,
                        BedStatus = ipd_bed.BEDSTATUS
                    };
            q = q.Where(m => m.resident.FEENO == feeNo);
            q = q.OrderByDescending(m => m.resident.FEENO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<Resident>();
                foreach (dynamic item in list)
                {
                    Resident newItem = Mapper.DynamicMap<Resident>(item.resident);
                    newItem.CarerName = item.CarerName;
                    newItem.NurseName = item.NurseName;
                    newItem.NutritionistName = item.NutritionistName;
                    newItem.PhysiotherapistName = item.PhysiotherapistName;
                    newItem.DoctorName = item.DoctorName;
                    newItem.BedStatus = item.BedStatus;
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
            if (response.Data.Count > 0)
            {

                br.Data = response.Data[0];
            }
            return br;

        }
        public BaseResponse<BedBasic> GetLeaveNursingBedInfo(long feeNo)
        {
            return base.Get<LTC_BEDBASIC, BedBasic>((q) => q.FEENO == feeNo);
        }
        public BaseResponse<Resident> SaveLeaveNursing(Resident resident)
        {
            var response = new BaseResponse<Resident>();
            var reqQueBedBasic = unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet.Where(m => m.FEENO == resident.FeeNo).FirstOrDefault();
            //清床位信息
            if (reqQueBedBasic != null)
            {
                reqQueBedBasic.FEENO = null;
                reqQueBedBasic.BEDSTATUS = BedStatus.Empty.ToString();
                unitOfWork.GetRepository<LTC_BEDBASIC>().Update(reqQueBedBasic);
            }
            var reqQueResident = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == resident.FeeNo).FirstOrDefault();
            var cm = Mapper.CreateMap<Resident, LTC_IPDREG>();
            Mapper.CreateMap<LTC_IPDREG, Resident>();
            Mapper.Map(resident, reqQueResident);
            reqQueResident.IPDFLAG = "D";//状态更新为 D 出院
            unitOfWork.GetRepository<LTC_IPDREG>().Update(reqQueResident);
            unitOfWork.Save();
            Mapper.Map(reqQueResident, resident);
            response.Data = resident;
            return response;




            ////var result = true;
            ////获取床位信息
            //BedBasic requestBedBasic = base.Get<LTC_BEDBASIC, BedBasic>((q) => q.FEENO == resident.FeeNo).Data;
            //if (requestBedBasic != null)
            //{
            //    requestBedBasic.FEENO = null;
            //    requestBedBasic.BedStatus = "E";
            //    //清空住民床位信息
            //    base.Save<LTC_BEDBASIC, BedBasic>(requestBedBasic, (q) => q.BEDNO == requestBedBasic.BedNo);
            //}
            //////获取住民信息
            ////Resident requestResident = base.Get<LTC_IPDREG, Resident>((q) => q.FEENO == resident.FeeNo).Data;
            ////if (resident != null)
            ////{
            //resident.IpdFlag = "D";
            ////更新住民信息
            //return base.Save<LTC_IPDREG, Resident>(resident, (q) => q.FEENO == resident.FeeNo);
            ////}
            ////else
            ////{
            ////    result = false;
            ////}
            ////return result;
        }
        #endregion

        #region 营养晒查

        public BaseResponse<IList<LTCNUTRTION72EVAL>> QueryNutrtionEvalExtend(BaseRequest<NutrtionEvalFilter> request)
        {

            BaseResponse<IList<LTCNUTRTION72EVAL>> response = new BaseResponse<IList<LTCNUTRTION72EVAL>>();
            var q = from f in unitOfWork.GetRepository<LTC_NUTRTION72EVAL>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on f.RECORDBY equals e.EMPNO into ees
                    from ee in ees.DefaultIfEmpty()
                    select new LTCNUTRTION72EVAL
                    {
                        ID = f.ID,
                        FEENO = f.FEENO,
                        REGNO = f.REGNO,
                        RECORDDATE = f.RECORDDATE,
                        RECORDBY = f.RECORDBY,
                        RECORDNAME = ee.EMPNAME,
                        CURRENTWEIGHT = f.CURRENTWEIGHT,
                        IDEALWEIGHT = f.IDEALWEIGHT,
                        HEIGHT = f.HEIGHT,
                        BMI = f.BMI,
                        DIETARY = f.DIETARY,
                        FEEDING = f.FEEDING,
                        BREAKFAST = f.BREAKFAST,
                        LUNCH = f.LUNCH,
                        DINNER = f.DINNER,
                        SNACK = f.SNACK,
                        LIKEFOOD = f.LIKEFOOD,
                        NOTLIKEFOOD = f.NOTLIKEFOOD,
                        ALLERGICFOOD = f.ALLERGICFOOD,
                        GASTROINTESTINAL = f.GASTROINTESTINAL,
                        FUNCTIONALEVAL = f.FUNCTIONALEVAL,
                        FATREDUCTION = f.FATREDUCTION,
                        MUSCLEWEAK = f.MUSCLEWEAK,
                        EDEMA = f.EDEMA,
                        ASCITES = f.ASCITES,
                        BEDSORE = f.BEDSORE,
                        BEDSORELEVEL = f.BEDSORELEVEL,
                        EVALRESULT = f.EVALRESULT,
                        ORGID = f.ORGID,
                        CHEW = f.CHEW,
                        SWALLOW = f.SWALLOW,
                    };

            if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
            {
                q = q.Where(m => m.FEENO == request.Data.FeeNo);
            }
            if (request.Data.StartDate.HasValue && request.Data.EndDate.HasValue)
            {
                q = q.Where(m => m.RECORDDATE >= request.Data.StartDate && m.RECORDDATE <= request.Data.EndDate);
            }
            if (request.Data.StartDate.HasValue && !request.Data.EndDate.HasValue)
            {
                q = q.Where(m => m.RECORDDATE >= request.Data.StartDate);
            }
            if (!request.Data.StartDate.HasValue && request.Data.EndDate.HasValue)
            {
                q = q.Where(m => m.RECORDDATE <= request.Data.EndDate);
            }
            q = q.OrderByDescending(m => m.RECORDDATE);
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

        public BaseResponse<LTCNUTRTION72EVAL> SaveNutrtionEval(LTCNUTRTION72EVAL request)
        {

            if (request.ID == 0)
            {
                request.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_NUTRTION72EVAL, LTCNUTRTION72EVAL>(request, (q) => q.ID == request.ID);
        }


        public BaseResponse<LTCNUTRTION72EVAL> GetNutrtionEval(int Id)
        {
            return base.Get<LTC_NUTRTION72EVAL, LTCNUTRTION72EVAL>((q) => q.ID == Id);
        }

        public BaseResponse DeleteNutrtionEval(int Id)
        {
            return base.Delete<LTC_NUTRTION72EVAL>(Id);
        }
        #endregion

        #region 住民资料 PostCode
        public BaseResponse<IList<ZipFile>> QueryPost(BaseRequest<ZipFileFilter> request)
        {
            BaseResponse<IList<ZipFile>> response = new BaseResponse<IList<ZipFile>>();
            Mapper.CreateMap<LTC_ZIPFILE, ZipFile>();
            var q = from m in unitOfWork.GetRepository<LTC_ZIPFILE>().dbSet
                    select m;
            if (!string.IsNullOrEmpty(request.Data.KeyWord))
            {
                q = q.Where(m => m.POSTCODE.Contains(request.Data.KeyWord) || m.TOWN.Contains(request.Data.KeyWord) || m.CITY.Contains(request.Data.KeyWord));
            }
            q = q.OrderByDescending(m => m.ID);
            response.RecordsCount = q.Count();
            var list = q.OrderBy(x => x.ID).ToList();
            response.Data = Mapper.Map<IList<ZipFile>>(list);
            return response;
        }

        #endregion


        #region 关账作业处理
        public BaseResponse CloseOrCnacleBill(long feeNo, DateTime financialCloseTime, string type)
        {
            var response = new BaseResponse();
            if (type == "close")
            {
                var q = from m in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                        select m;
                q = q.Where(o => o.ISDELETE != true && o.FEENO == feeNo && o.STATUS != (int)BillStatus.Refund);
                var billinfo = q.ToList();
                if (billinfo != null && billinfo.Count > 0)
                {
                    var bill = billinfo.OrderByDescending(o => o.BALANCEENDTIME).FirstOrDefault();
                    if (financialCloseTime < bill.BALANCEENDTIME)
                    {
                        if (bill.STATUS == (int)BillStatus.NoCharge)
                        {
                            response.ResultCode = -1;
                            response.ResultMessage = "存在关账日期之后的账单，请先删除此账单后，再进行关账操作";
                            return response;
                        }
                        else if (bill.STATUS == (int)BillStatus.Charge)
                        {
                            response.ResultCode = -1;
                            response.ResultMessage = "存在关账日期之后的账单，请先此账单退费后，再进行关账操作";
                            return response;
                        }
                        else if (bill.STATUS == (int)BillStatus.Uploaded)
                        {
                            response.ResultCode = (int)BillStatus.Uploaded;
                            response.ResultMessage = bill.BILLMONTH;
                            return response;
                        }
                    }
                }
            }
            else if (type == "Cancle")
            {
                var q = from m in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                        select m;

                q = q.Where(o => o.FEENO == feeNo && o.ISDELETE != true && o.STATUS != (int)BillStatus.Refund && o.ISFINANCIALCLOSE == true);
                var billinfo = q.ToList();
                if (billinfo != null && billinfo.Count > 0)
                {
                    var bill = billinfo.OrderByDescending(o => o.BALANCEENDTIME).FirstOrDefault();
                    if (bill.STATUS == (int)BillStatus.NoCharge)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = "存在关账日期时的账单，请先删除此账单后，再进行关账操作";
                        return response;
                    }
                    else if (bill.STATUS == (int)BillStatus.Charge)
                    {
                        response.ResultCode = -1;
                        response.ResultMessage = "存在关账日期时的账单，请先此账单退费后，再进行关账操作";
                        return response;
                    }
                    else if (bill.STATUS == (int)BillStatus.Uploaded)
                    {
                        response.ResultCode = (int)BillStatus.Uploaded;
                        response.ResultMessage = bill.BILLMONTH;
                        return response;
                    }
                }
            }

            SaveIpdregInfo(type, financialCloseTime, feeNo);
            return response;
        }
        #endregion

        public BaseResponse SaveIpdregInfo(string type, DateTime financialCloseTime,long feeNo)
        {
            var response = new BaseResponse();
            try
            {
                #region 设置关账操作
                if (type == "close")
                {
                    var ipdreg = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == feeNo).FirstOrDefault();
                    if (ipdreg != null)
                    {
                        ipdreg.ISFINANCIALCLOSE = true;
                        ipdreg.FINANCIALCLOSETIME = financialCloseTime;
                        unitOfWork.GetRepository<LTC_IPDREG>().Update(ipdreg);
                        unitOfWork.Save();
                    }
                }
                else if (type == "Cancle")
                {
                    var ipdreg = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.FEENO == feeNo).FirstOrDefault();
                    if (ipdreg != null)
                    {
                        ipdreg.ISFINANCIALCLOSE = false;
                        ipdreg.FINANCIALCLOSETIME = null;
                        unitOfWork.GetRepository<LTC_IPDREG>().Update(ipdreg);
                        unitOfWork.Save();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
            }
            return response;
        }

        public async System.Threading.Tasks.Task<object> GetNsMonFeeInfo(string yearMonth, string Orgid)
        {
            var http = HttpClientHelper.LtcHttpClient;
            var request = new BillV2();
            request.BillMonth = yearMonth;
            request.OrgId = Orgid;
            var monFeeInfo = new MonFeeModel();
            try
            {
                var result = await http.PostAsJsonAsync("/api/monFee/GetNsMonfee", request);
                var resultContent = await result.Content.ReadAsStringAsync();
                monFeeInfo = JsonConvert.DeserializeObject<MonFeeModel>(resultContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return monFeeInfo;
        }

        public long GetResFeeNo(string idNo,string nsNo)
        {
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals r.REGNO into ipd_rf
                    from ipd_rfile in ipd_rf.DefaultIfEmpty()
                    select new Person
                    {
                        FeeNo = ipd.FEENO,
                        IdNo = ipd_rfile.IDNO,
                        OrgId = ipd.ORGID,
                        IpdFlag = ipd.IPDFLAG,
                        CreateDate = ipd_rfile.CREATEDATE,
                    };

            q = q.Where(m => m.IdNo == idNo && m.IpdFlag == "I" && m.OrgId == nsNo);
            var info = q.OrderByDescending(m => m.CreateDate).FirstOrDefault();
            return info == null ? 0 : (int)info.FeeNo;
        }

        public BaseResponse UpdateYearCert(AuditYearCertModel baseRequest)
        {
            var response = new BaseResponse();
            if (baseRequest.HospStatus == 6 && baseRequest.CertStatus == 6)  //强制取消护理险资格
            {
                var model = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(x => x.CERTNO == baseRequest.CertNo && x.STATUS == 0).FirstOrDefault();
                if (model != null)
                {
                    model.STATUS = 1;
                    model.UPDATEBY = "经办管理员";
                    model.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_REGNCIINFO>().Update(model);
                    unitOfWork.Save();
                }
                response.ResultCode = 1001;
            }
            else if (baseRequest.HospStatus == 9 && baseRequest.CertStatus == 9)  //启用护理险资格
            {
                var model = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(x => x.CERTNO == baseRequest.CertNo && x.STATUS == 0).FirstOrDefault();
                if (model != null)
                {
                    model.STATUS = 1;
                    model.UPDATEBY = "经办管理员";
                    model.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_REGNCIINFO>().Update(model);
                    unitOfWork.Save();
                }

                LTC_REGNCIINFO reginfo = new LTC_REGNCIINFO();
                var feeno = GetResFeeNo(baseRequest.IdNo, baseRequest.NsNo);
                if (feeno != 0)
                {
                    reginfo.FEENO = feeno;
                    reginfo.CERTNO = baseRequest.CertNo;
                    reginfo.CERTSTARTTIME = baseRequest.Certstarttime;
                    reginfo.CERTEXPIREDTIME = baseRequest.Certexpiredtime;
                    reginfo.APPLYHOSTIME = baseRequest.Entrytime;
                    reginfo.CARETYPEID = baseRequest.Caretypeid.ToString();
                    reginfo.NCIPAYLEVEL = baseRequest.NCIpaylevel;
                    reginfo.NCIPAYSCALE = baseRequest.NCIpayscale;
                    reginfo.STATUS = 0;
                    reginfo.CREATEBY = "经办管理员";
                    reginfo.CREATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_REGNCIINFO>().Insert(reginfo);
                    unitOfWork.Save();
                }
                response.ResultCode = 1001;
            }
            return response;
        }
    }
}
