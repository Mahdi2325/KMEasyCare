using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KMHC.SLTC.Business.Implement.DC
{
    public class DC_FamilyDoctorService : BaseService, IDC_FamilyDoctorService
    {
        #region 健康记录追踪
        public BaseResponse<IList<DC_RegCheckRecordModel>> QueryRegCheckRecord(BaseRequest<DC_RegCheckRecordFilter> request)
        {
            var dateTime = DateTime.Now;
            var regCheckRecordSet = unitOfWork.GetRepository<DC_REGCHECKRECORD>().dbSet;
            var regCheckRecordData = unitOfWork.GetRepository<DC_REGCHECKRECORDDATA>().dbSet;
            //var q = from r in unitOfWork.GetRepository<DC_REGFILE>().dbSet
            //        join rr in regCheckRecordSet on r.REGNO equals rr.REGNO
            //        group rr by new { r.ORGID, r.REGNO, r.REGNAME, r.IDNO, r.SEX, r.BIRTHDATE } into g
            //        let LastItem = g.OrderByDescending(it => it.CHECKDATE).FirstOrDefault()
            //        select new DC_RegCheckRecord
            //        {
            //            OrgId = g.Key.ORGID,
            //            IsAbnormal = g.Any(it=>it.ISABNORMAL == true),
            //            LastUpdateTime = LastItem != null ? LastItem.CHECKDATE : null,
            //            TraceStatus = g.Any(it=>it.TRACESTATUS == true),
            //            Birthdate = g.Key.BIRTHDATE,
            //            ChildRecourdCount = g.Count(),
            //            IdNo = g.Key.IDNO,
            //            RegNo = g.Key.REGNO,
            //            RegName = g.Key.REGNAME,
            //            Sex = g.Key.SEX
            //        };
            var q = from r in unitOfWork.GetRepository<DC_REGFILE>().dbSet
                    let LastItem = regCheckRecordSet.Where(it => it.REGNO == r.REGNO).OrderByDescending(it => it.CHECKDATE).FirstOrDefault()
                    select new DC_RegCheckRecordModel
                    {
                        OrgId = r.ORGID,
                        IsAbnormal = regCheckRecordSet.Any(it => it.REGNO == r.REGNO && it.ISABNORMAL == true),
                        LastUpdateTime = LastItem != null ? LastItem.CHECKDATE : null,
                        //LastUpdateTime = null,
                        TraceStatus = regCheckRecordSet.Any(it => it.REGNO == r.REGNO && it.TRACESTATUS == true),
                        Birthdate = r.BIRTHDATE,
                        ChildRecourdCount = regCheckRecordSet.Count(it => it.REGNO == r.REGNO),
                        IdNo = r.IDNO,
                        RegNo = r.REGNO,
                        RegName = r.REGNAME,
                        Sex = r.SEX
                    };
            if (request.Data.StartDate.HasValue)
            {
                q = q.Where(it => (it.LastUpdateTime.HasValue && it.LastUpdateTime.Value.CompareTo(request.Data.StartDate.Value) >= 0));
            }
            if (request.Data.EndDate.HasValue)
            {
                q = q.Where(it => (it.LastUpdateTime.HasValue && it.LastUpdateTime.Value.CompareTo(request.Data.EndDate.Value) <= 0));
            }
            if (!string.IsNullOrEmpty(request.Data.RegName))
            {
                q = q.Where(it => it.RegName.Contains(request.Data.RegName));
            }
            if (!string.IsNullOrEmpty(request.Data.IdNo))
            {
                q = q.Where(it => it.IdNo.Contains(request.Data.IdNo));
            }
            if (request.Data.DisplayType)
            {
                q = q.Where(it => it.IsAbnormal == request.Data.DisplayType);
            }
            q = q.Where(it => it.OrgId == request.Data.OrgId);
            if (request.Data.TraceStatus.HasValue)
            {
                q = q.Where(it => it.TraceStatus == request.Data.TraceStatus);
            }
            var response = new BaseResponse<IList<DC_RegCheckRecordModel>>();
            response.RecordsCount = q.Count();
            q = q.OrderBy(it => it.RegName);
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

        public BaseResponse<List<DC_RegCheckRecordDtlModel>> GetRegCheckRecordDtl(BaseRequest<DC_RegCheckRecordFilter> request)
        {
            var response = new BaseResponse<List<DC_RegCheckRecordDtlModel>>();
            var q = from rr in unitOfWork.GetRepository<DC_REGCHECKRECORD>().dbSet
                    join ct in unitOfWork.GetRepository<CHECKTEMPLATE>().dbSet on rr.CHECKTEMPLATECODE equals ct.CHECKTEMPLATECODE
                    join rrd in unitOfWork.GetRepository<DC_REGCHECKRECORDDATA>().dbSet on rr.RECORDID equals rrd.RECORDID
                    where rr.REGNO == request.Data.RegNo
                    group rrd by new { rr.RECORDID, rr.CHECKDATE, rr.CHECKTEMPLATECODE, rr.ISABNORMAL, rr.TRACESTATUS, ct.CHECKTEMPLATENAME } into g
                    select new
                    {
                        RecordId = g.Key.RECORDID,
                        CheckDate = g.Key.CHECKDATE,
                        CheckTemplateCode = g.Key.CHECKTEMPLATECODE,
                        CheckTemplateName = g.Key.CHECKTEMPLATENAME,
                        IsAbnormal = g.Key.ISABNORMAL,
                        TraceStatus = g.Key.TRACESTATUS,
                        CheckResultList = g.ToList()
                    };
            response.Data = new List<DC_RegCheckRecordDtlModel>();

            if (request.Data.StartDate.HasValue)
            {
                q = q.Where(it => (it.CheckDate.HasValue && it.CheckDate.Value.CompareTo(request.Data.StartDate.Value) >= 0));
            }
            if (request.Data.EndDate.HasValue)
            {
                q = q.Where(it => (it.CheckDate.HasValue && it.CheckDate.Value.CompareTo(request.Data.EndDate.Value) <= 0));
            }
            if (!string.IsNullOrEmpty(request.Data.CheckTemplateCode))
            {
                q = q.Where(it => it.CheckTemplateCode == request.Data.CheckTemplateCode);
            }
            if (request.Data.DisplayType)
            {
                q = q.Where(it => it.IsAbnormal == request.Data.DisplayType);
            }
            q = q.OrderBy(it => it.CheckDate);
            response.RecordsCount = q.Count();
            IList regCheckRecord = null;
            if (request != null && request.PageSize > 0)
            {
                regCheckRecord = (IList)q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
            }
            else
            {
                regCheckRecord = (IList)q.ToList();
            }
            var checkItemList = unitOfWork.GetRepository<CHECKITEM>().dbSet.ToList();
            foreach (dynamic it in regCheckRecord)
            {
                DC_RegCheckRecordDtlModel item = new DC_RegCheckRecordDtlModel();
                item.RecordId = it.RecordId;
                item.CheckDate = it.CheckDate;
                item.CheckTemplateCode = it.CheckTemplateCode;
                item.CheckTemplateName = it.CheckTemplateName;
                item.IsAbnormal = it.IsAbnormal;
                item.TraceStatus = it.TraceStatus;
                item.CheckResult = string.Empty;
                ((List<DC_REGCHECKRECORDDATA>)it.CheckResultList).ForEach(cr =>
                {
                    var checkItem = checkItemList.Find(ci => ci.CHECKITEMCODE == cr.CHECKITEMCODE);
                    if (checkItem != null)
                    {
                        string chart = "";
                        decimal checkItemValue = 0;
                        decimal lowBound = 0;
                        decimal upBound = 0;
                        if (decimal.TryParse(cr.CHECKITEMVALUE, out checkItemValue) && decimal.TryParse(cr.LOWBOUND, out lowBound))
                        {
                            if (checkItemValue < lowBound)
                            {
                                chart = "<b style='color:red'>↓</b>";
                            }
                        }
                        if (decimal.TryParse(cr.CHECKITEMVALUE, out checkItemValue) && decimal.TryParse(cr.UPBOUND, out upBound))
                        {
                            if (checkItemValue > upBound)
                            {
                                chart = "<b style='color:red'>↑</b>";
                            }
                        }
                        item.CheckResult = string.Format("{0}{1}:{2} {3}{4} ",
                        item.CheckResult,
                        checkItem.CHECKITEMNAME,
                        cr.CHECKITEMVALUE,
                        checkItem.UNITNAME,
                        chart
                        );
                    }
                });
                response.Data.Add(item);
            }
            return response;
        }

        public BaseResponse<DC_RegCheckRecordModel> SaveRegCheckRecord(DC_RegCheckRecordModel request, List<string> fields)
        {
            return base.Save<DC_REGCHECKRECORD, DC_RegCheckRecordModel>(request, (q) => q.RECORDID == request.RecordId, fields);
        }

        public BaseResponse DeleteRegCheckRecord(long regCheckRecordId)
        {
            return base.Delete<DC_REGCHECKRECORD>(regCheckRecordId);
        }
        #endregion

        #region 健康记录数据
        public BaseResponse<IList<DC_RegCheckRecordDataModel>> QueryRegCheckRecordData(BaseRequest<DC_RegCheckRecordDataFilter> request)
        {
            var response = base.Query<DC_REGCHECKRECORDDATA, DC_RegCheckRecordDataModel>(request, (q) =>
            {
                q = q.Where(m => m.RECORDID == request.Data.RecordId);
                q = q.OrderBy(m => m.DATAID);
                return q;
            });
            return response;
        }
        #endregion

        #region 关怀记录
        public BaseResponse<IList<DC_RegNoteRecordModel>> QueryRegNoteRecord(BaseRequest<DC_RegNoteRecordFilter> request)
        {
            BaseResponse<IList<DC_RegNoteRecordModel>> response = new BaseResponse<IList<DC_RegNoteRecordModel>>();
            Mapper.CreateMap<DC_REGVISITRECORD, DC_RegVisitRecordModel>();
            var q = from m in unitOfWork.GetRepository<DC_REGNOTERECORD>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on m.ACTIONUSERCODE equals e.EMPNO
                    select new DC_RegNoteRecordModel
                    {
                        OrgId = m.ORGID,
                        ActionUserCode = m.ACTIONUSERCODE,
                        RecordId = m.RECORDID,
                        RegNo = m.REGNO,
                        NoteContent = m.NOTECONTENT,
                        NoteDate = m.NOTEDATE,
                        NoteName = m.NOTENAME,
                        ViewDate = m.VIEWDATE,
                        ViewStatus = m.VIEWSTATUS,
                        NoteId = m.NOTEID,
                        ActionUserName = e.EMPNAME
                    };

            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            if (request.Data.StartDate.HasValue)
            {
                q = q.Where(it => (it.NoteDate.HasValue && it.NoteDate.Value.CompareTo(request.Data.StartDate.Value) >= 0));
            }
            if (request.Data.EndDate.HasValue)
            {
                q = q.Where(it => (it.NoteDate.HasValue && it.NoteDate.Value.CompareTo(request.Data.EndDate.Value) <= 0));
            }
            q = q.OrderByDescending(m => m.NoteDate);

            response.RecordsCount = q.Count();
            List<DC_RegNoteRecordModel> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }

            response.Data = list;
            return response;
        }

        public BaseResponse<DC_RegNoteRecordModel> SaveRegNoteRecord(DC_RegNoteRecordModel request, List<string> fields)
        {
            return base.Save<DC_REGNOTERECORD, DC_RegNoteRecordModel>(request, (q) => q.RECORDID == request.RecordId, fields);
        }

        public BaseResponse DeleteRegNoteRecord(long regNoteId)
        {
            return base.Delete<DC_REGNOTERECORD>(regNoteId);
        }
        #endregion

        #region 关怀内容
        public BaseResponse<IList<DC_NoteModel>> QueryNote(BaseRequest<DC_NoteFilter> request)
        {
            BaseResponse<IList<DC_NoteModel>> response = new BaseResponse<IList<DC_NoteModel>>();
            Mapper.CreateMap<DC_NOTE, DC_NoteModel>();
            var q = from m in unitOfWork.GetRepository<DC_NOTE>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on m.ACTIONUSERCODE equals e.EMPNO
                    select new DC_NoteModel
                    {
                        OrgId = m.ORGID,
                        ActionUserCode = m.ACTIONUSERCODE,
                        NoteId = m.NOTEID,
                        NoteName = m.NOTENAME,
                        NoteContent = m.NOTECONTENT,
                        ShowNumber = m.SHOWNUMBER,
                        IsShow = m.ISSHOW
                    };

            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            if (!string.IsNullOrEmpty(request.Data.NoteName))
            {
                q = q.Where(m => m.NoteName.Contains(request.Data.NoteName));
            }
            if (request.Data.IsShow.HasValue)
            {
                q = q.Where(m => m.IsShow == request.Data.IsShow.Value);
            }
            q = q.OrderBy(m => m.ShowNumber);

            response.RecordsCount = q.Count();
            List<DC_NoteModel> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }

            response.Data = list;
            return response;
        }

        public BaseResponse<DC_NoteModel> SaveNote(DC_NoteModel request, List<string> fields)
        {
            return base.Save<DC_NOTE, DC_NoteModel>(request, (q) => q.NOTEID == request.NoteId, fields);
        }

        public BaseResponse DeleteNote(long noteId)
        {
            return base.Delete<DC_NOTE>(noteId);
        }
        #endregion

        #region 访谈记录
        public BaseResponse<IList<DC_RegVisitRecordModel>> QueryRegVisitRecord(BaseRequest<DC_RegVisitRecordFilter> request)
        {
            BaseResponse<IList<DC_RegVisitRecordModel>> response = new BaseResponse<IList<DC_RegVisitRecordModel>>();
            Mapper.CreateMap<DC_REGVISITRECORD, DC_RegVisitRecordModel>();
            var q = from m in unitOfWork.GetRepository<DC_REGVISITRECORD>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on m.ACTIONUSERCODE equals e.EMPNO
                    select new DC_RegVisitRecordModel { 
                        OrgId = m.ORGID,
                        ActionUserCode = m.ACTIONUSERCODE,
                        RecordId = m.RECORDID,
                        RegNo = m .REGNO,
                        VisitContent = m.VISITCONTENT,
                        VisitDate = m.VISITDATE,
                        VisitName = m.VISITNAME,
                        ActionUserName = e.EMPNAME
                    };

            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            if (request.Data.StartDate.HasValue)
            {
                q = q.Where(it => (it.VisitDate.HasValue && it.VisitDate.Value.CompareTo(request.Data.StartDate.Value) >= 0));
            }
            if (request.Data.EndDate.HasValue)
            {
                q = q.Where(it => (it.VisitDate.HasValue && it.VisitDate.Value.CompareTo(request.Data.EndDate.Value) <= 0));
            }
            q = q.OrderByDescending(m => m.VisitDate);

            response.RecordsCount = q.Count();
            List<DC_RegVisitRecordModel> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }

            response.Data = list;
            return response;
        }

        public BaseResponse<DC_RegVisitRecordModel> SaveRegVisitRecord(DC_RegVisitRecordModel request, List<string> fields)
        {
            return base.Save<DC_REGVISITRECORD, DC_RegVisitRecordModel>(request, (q) => q.RECORDID == request.RecordId, fields);
        }

        public BaseResponse DeleteRegVisitRecord(long regVisitId)
        {
            return base.Delete<DC_REGVISITRECORD>(regVisitId);
        }
        #endregion

        #region 下拉框数据
        public BaseResponse<IList<CheckTemplateModel>> GetCheckTemplateList(BaseRequest<CheckTemplateFilter> request)
        {
            var response = base.Query<CHECKTEMPLATE, CheckTemplateModel>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.OrgId))
                {
                    q = q.Where(it => it.ORGID == request.Data.OrgId);
                }
                q = q.OrderBy(m => m.SHOWNUMBER);
                return q;
            });
            return response;
        }
        
        #endregion
    }
}

