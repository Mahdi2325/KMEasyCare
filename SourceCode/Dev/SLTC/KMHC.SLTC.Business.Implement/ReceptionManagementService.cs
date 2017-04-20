using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.PackageRelated;

namespace KMHC.SLTC.Business.Implement
{
    public class ReceptionManagementService : BaseService, IReceptionManagementService
    {
        public object GetFrontConsole(DateTime startTime, DateTime endTime)
        {
            var sTime = startTime;
            var eTime = endTime.AddDays(1).AddSeconds(-1);
            var ipdregList = unitOfWork.GetRepository<LTC_IPDREG>()
                .dbSet.Where(w => w.INDATE <= eTime && (w.OUTDATE >= sTime || w.OUTDATE == null)
                                  && w.ORGID == SecurityHelper.CurrentPrincipal.OrgId).Select(s => s).ToList();
            var leaveHospList =
                unitOfWork.GetRepository<LTC_LEAVEHOSP>()
                    .dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
                                      w.STARTDATE <= eTime && (w.RETURNDATE >= sTime || w.RETURNDATE == null))
                    .Select(s => s).ToList();
            var famList = unitOfWork.GetRepository<LTC_FAMILYDISCUSSREC>()
                .dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId &&
                                  w.STARTDATE <= eTime && (w.ENDDATE >= sTime || w.ENDDATE == null))
                .Select(s => s).ToList();
            var bedList =
                unitOfWork.GetRepository<LTC_BEDBASIC>()
                    .dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId && w.UPDATEDATE >= sTime && w.UPDATEDATE <= eTime)
                    .Select(s => s).ToList();
            var resInHospList = unitOfWork.GetRepository<LTC_IPDREG>()
                .dbSet.Where(w => w.INDATE >= sTime && w.INDATE <= endTime
                                  && w.ORGID == SecurityHelper.CurrentPrincipal.OrgId).Select(s => s).ToList();
            List<DateTime> dtList = new List<DateTime>();
            for (; startTime <= endTime; startTime = startTime.AddDays(1))
            {
                dtList.Add(startTime);
            }

            List<object> objList = new List<object>();
            dtList.ForEach(f =>
            {
                var dayStartTime = f;
                var dayEndTime = f.AddDays(1).AddSeconds(-1);
                objList.Add(new
                {
                    day = f.ToString("yyyy-MM-dd"),
                    ipdFee = ipdregList.Where(w => w.INDATE <= dayEndTime && (w.OUTDATE >= dayEndTime || w.OUTDATE == null))
                    .Select(s1 => s1.FEENO).Distinct().ToList(),
                    leaFee = leaveHospList.Where(w => w.STARTDATE <= dayEndTime && (w.RETURNDATE >= dayStartTime || w.RETURNDATE == null)).Select(s1 => s1.FEENO).Distinct().ToList(),
                    famFee = famList.Where(w => w.STARTDATE <= dayEndTime && (w.ENDDATE >= dayStartTime || w.ENDDATE == null)).Select(s1 => s1.FEENO).Distinct().ToList(),
                    bedPreFee = bedList.Where(w => w.BEDSTATUS == BedStatus.Subscribe.ToString() && w.UPDATEDATE >= dayStartTime && w.UPDATEDATE <= dayEndTime).ToList(),
                    bedNoUseFee = bedList.Where(w => w.BEDSTATUS == BedStatus.Empty.ToString() && w.UPDATEDATE >= dayStartTime && w.UPDATEDATE <= dayEndTime).ToList(),
                    resInHospFee = resInHospList.Where(w => w.INDATE >= dayStartTime && w.INDATE <= dayEndTime).Select(s1 => s1.FEENO).Distinct().ToList(),
                });
            });
            return objList;
        }

        public BaseResponse<IList<ConsultRec>> QueryAdvisoryReg(BaseRequest<AdvisoryRegFilter> request)
        {
            var response = new BaseResponse<IList<ConsultRec>>();
            var q = unitOfWork.GetRepository<ConsultRec>().SqlQuery(@"SELECT
    ID Id,
	CONSULTANTNAME ConsultantName,
	CONSULTTIME ConsultTime,
	CONSULTANTPHONE ConsultantPhone,
	APPELLATION Appellation,
	OLDMANNAME OldManName,
	OLDMANSEX OldManSex,
	OLDMANAGE OldManAge,
	CHANNEL Channel,
	EARNESTSTATUS EarnestStatus,
    RESERVATIONBED ReservationBed,
	(
		SELECT
			CALLBACKTIME
		FROM
			LTC_CONSULTCALLBACK
		WHERE
			CONSULTRECID = LTC_CONSULTREC.ID
		ORDER BY
			CALLBACKTIME DESC
		LIMIT 0,
		1
	) CallBackTime
FROM
	LTC_CONSULTREC;

");
            if (request.Data.ConsultStartTime.HasValue)
            {
                q = q.Where(w => w.ConsultTime >= request.Data.ConsultStartTime.Value);
            }
            if (request.Data.ConsultEndTime.HasValue)
            {
                q = q.Where(w => w.ConsultTime <= request.Data.ConsultEndTime.Value.AddDays(1).AddSeconds(-1));
            }
            if (!string.IsNullOrEmpty(request.Data.KeyWords))
            {
                q = q.Where(w => w.ConsultantName.Contains(request.Data.KeyWords) ||
                    w.ConsultantPhone.Contains(request.Data.KeyWords) || w.OldManName.Contains(request.Data.KeyWords));
            }

            q = q.OrderByDescending(o => o.Id);
            response.RecordsCount = q.Count();

            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.Data = list;
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);

            }
            else
            {
                var list = q.ToList();
                response.Data = list;
            }
            return response;
        }
        public BaseResponse<ConsultRec> SaveConsultRec(ConsultRec request)
        {
            if (!request.ConsultTime.HasValue)
            {
                request.ConsultTime = DateTime.Now;
            }
            var consultRecSource = base.Get<LTC_CONSULTREC, ConsultRec>((q) => q.ID == request.Id);
            if (consultRecSource.Data != null)
            {
                if (consultRecSource != null && consultRecSource.Data.ReservationBed != null && consultRecSource.Data.ReservationBed != request.ReservationBed)
                {
                    var bedBasicSource = base.Get<LTC_BEDBASIC, BedBasic>((q) => q.BEDNO == consultRecSource.Data.ReservationBed);
                    if (bedBasicSource.Data != null)
                    {
                        bedBasicSource.Data.BedStatus = BedStatus.Empty.ToString();
                        bedBasicSource.Data.UpdateDate = DateTime.Now;
                        bedBasicSource.Data.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                        base.Save<LTC_BEDBASIC, BedBasic>(bedBasicSource.Data, (q) => q.BEDNO == bedBasicSource.Data.BedNo);
                    }
                }
            }
            var bedBasic = base.Get<LTC_BEDBASIC, BedBasic>((q) => q.BEDNO == request.ReservationBed);
            if (bedBasic.Data != null && bedBasic.Data.BedStatus == BedStatus.Empty.ToString())
            {
                bedBasic.Data.BedStatus = BedStatus.Subscribe.ToString();
                bedBasic.Data.UpdateDate = DateTime.Now;
                bedBasic.Data.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                base.Save<LTC_BEDBASIC, BedBasic>(bedBasic.Data, (q) => q.BEDNO == bedBasic.Data.BedNo);
            }
            var consultRec = base.Save<LTC_CONSULTREC, ConsultRec>(request, (q) => q.ID == request.Id);
            unitOfWork.Commit();
            return consultRec;
        }
        public BaseResponse DeleteConsultRec(long id)
        {
            return base.Delete<LTC_CONSULTREC>(id);
        }
        public BaseResponse<ConsultRec> GetConsultRec(long id)
        {
            return base.Get<LTC_CONSULTREC, ConsultRec>((q) => q.ID == id);
        }
        public BaseResponse<IList<ConsultCallBack>> QueryConsultCallBack(BaseRequest<ConsultCallBackFilter> request)
        {
            var response = base.Query<LTC_CONSULTCALLBACK, ConsultCallBack>(request, (q) =>
            {
                if (request != null && request.Data.CallBackStartTime.HasValue)
                {
                    q = q.Where(m => m.CALLBACKTIME >= request.Data.CallBackStartTime.Value);
                }
                if (request != null && request.Data.CallBackEndTime.HasValue)
                {
                    var callBackEndTime = request.Data.CallBackEndTime.Value.AddDays(1).AddSeconds(-1);
                    q = q.Where(m => m.CALLBACKTIME <= callBackEndTime);
                }
                q = q.Where(m => m.CONSULTRECID == request.Data.ConsultRecId);
                q = q.OrderByDescending(m => m.CALLBACKTIME);
                return q;
            });
            return response;
        }
        public BaseResponse<ConsultCallBack> SaveConsultCallBack(ConsultCallBack request)
        {

            return base.Save<LTC_CONSULTCALLBACK, ConsultCallBack>(request, (q) => q.ID == request.Id);
        }
        public BaseResponse DeleteConsultCallBack(long id)
        {
            return base.Delete<LTC_CONSULTCALLBACK>(id);
        }
        public BaseResponse<ConsultCallBack> GetConsultCallBack(long id)
        {
            return base.Get<LTC_CONSULTCALLBACK, ConsultCallBack>((q) => q.ID == id);
        }
    }
}
