using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
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
    public class RegNCIInfoService : BaseService, IRegNCIInfoService
    {

        public BaseResponse UpdateRegInfo(CareInsInfo baserequest)
        {
            BaseResponse response = new BaseResponse();
            var dt = new DateTime(0001, 1, 1);
            if (baserequest.appCertInfo.CertNo == null || baserequest.appCertInfo.InHospDate==null || baserequest.appCertInfo.InHospDate==dt)
            {
                RegNCIInfo req = new RegNCIInfo();
                req.Feeno = baserequest.FeeNo;
                SaveRegNCIInfo(req);
            }
            else
            {
                RegNCIInfo req = new RegNCIInfo();
                req.Feeno = baserequest.FeeNo;
                req.Certno = baserequest.appCertInfo.CertNo;
                req.CertStartTime = Convert.ToDateTime(baserequest.appCertInfo.CertStartTime);
                req.CertexpiredTime = Convert.ToDateTime(baserequest.appCertInfo.CertExpiredTime);
                req.Caretypeid = baserequest.appCertInfo.AgencyapprovedcareType.ToString();
                req.NCIPaylevel = baserequest.appCertInfo.NCIPayLevel;
                req.NCIPayscale = baserequest.appCertInfo.NCIPayScale;
                req.ApplyHosTime = baserequest.appCertInfo.InHospDate;
                SaveRegNCIInfo(req);
                UpdateRegIpd(baserequest);
            }
            return response;
        }

        public void UpdateRegIpd(CareInsInfo request)
        {
            
            var ipdReg = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.FirstOrDefault(m => m.FEENO == request.FeeNo && m.IPDFLAG=="I");
            if (ipdReg != null)
            {
                ipdReg.RSSTATUS = request.appCertInfo.McType.ToString();
                unitOfWork.GetRepository<LTC_IPDREG>().Update(ipdReg);
                var regFile = unitOfWork.GetRepository<LTC_REGFILE>().dbSet.FirstOrDefault(m => m.REGNO == ipdReg.REGNO);
                if (regFile != null)
                {
                    regFile.SSNO = request.appCertInfo.SsNo;
                    regFile.DISEASEDIAG = request.appCertInfo.DiseaseTxt;
                    regFile.BRITHPLACE = request.appCertInfo.Residence;
                    regFile.NAME = request.appCertInfo.Name;
                    regFile.SEX = request.appCertInfo.Gender == "男" ? "M" : request.appCertInfo.Gender == "女" ? "F" : "";
                    unitOfWork.GetRepository<LTC_REGFILE>().Update(regFile);
                }
                unitOfWork.Save();
            } 
        }

        public BaseResponse UpdateRegBalance(long feeno, bool isHasNCI)
        {
            var response = new BaseResponse();
            var model = unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet.Where(x => x.FEENO == feeno && x.STATUS == 0 && x.ISDELETE != true).FirstOrDefault();
            if (model != null)
            {
                model.STATUS = 0;
                model.ISHAVENCI = isHasNCI;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                model.UPDATETIME = DateTime.Now;
                unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().Update(model);
                unitOfWork.Save();
            }
            return response;
        }

        public BaseResponse SaveRegNCIInfo(RegNCIInfo baseRequest)
        {
            BaseResponse response = new BaseResponse();
            Mapper.CreateMap<RegNCIInfo, LTC_REGNCIINFO>();

            var model = unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(x => x.FEENO == baseRequest.Feeno && x.STATUS == 0).FirstOrDefault();
            if (model == null)
            {
                UpdateRegBalance(baseRequest.Feeno, true);
                baseRequest.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                baseRequest.Createtime = DateTime.Now;
                baseRequest.Status = 0;
                model = Mapper.Map<LTC_REGNCIINFO>(baseRequest);
                unitOfWork.GetRepository<LTC_REGNCIINFO>().Insert(model);
                unitOfWork.Save();
            }
            else
            {
                UpdateRegBalance(baseRequest.Feeno, false);
                model.STATUS = 1;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                model.UPDATETIME = DateTime.Now;
                unitOfWork.GetRepository<LTC_REGNCIINFO>().Update(model);

                if (baseRequest.Certno != null)
                {
                    UpdateRegBalance(baseRequest.Feeno, true);
                    baseRequest.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    baseRequest.Createtime = DateTime.Now;
                    baseRequest.Status = 0;
                    var basereq = Mapper.Map<LTC_REGNCIINFO>(baseRequest);
                    unitOfWork.GetRepository<LTC_REGNCIINFO>().Insert(basereq);
                }
                unitOfWork.Save();
            }
            return response;
        }

        public BaseResponse<RegNCIInfo> GetLTCRegInfo(int feeNo)
        {
            var response = new BaseResponse<RegNCIInfo>();
            var q = from m in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(m => m.FEENO == feeNo && m.STATUS == 0)
                    join n in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on m.FEENO equals n.FEENO into nrd
                    from nr in nrd.DefaultIfEmpty()
                    join c in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on nr.REGNO equals c.REGNO into ncd
                    from nc in ncd.DefaultIfEmpty()
                    select new RegNCIInfo
                    {
                        Feeno = m.FEENO,
                        Certno = m.CERTNO,
                        CertStartTime = m.CERTSTARTTIME,
                        CertexpiredTime = m.CERTEXPIREDTIME,
                        ApplyHosTime = m.APPLYHOSTIME,
                        Caretypeid = m.CARETYPEID,
                        NCIPaylevel = m.NCIPAYLEVEL,
                        NCIPayscale = m.NCIPAYSCALE,
                        Createtime = m.CREATETIME,
                        Status = m.STATUS,
                        RegName = nc.NAME,
                        IdNo = nc.IDNO,
                        SsNo = nc.SSNO,
                        Sex = nc.SEX,
                        Diseasediag =  nc.DISEASEDIAG
                    };

            var infoList = q.OrderByDescending(m => m.Createtime).ToList();
            if (infoList != null && infoList.Count > 0)
            {
                var certInfo = infoList.FirstOrDefault();
                if (!string.IsNullOrEmpty(certInfo.Certno))
                {
                    if (!string.IsNullOrEmpty(certInfo.Sex))
                    {
                        if (certInfo.Sex == "F")
                        {
                            certInfo.Sex = "女";
                        }
                        else if (certInfo.Sex == "M")
                        {
                            certInfo.Sex = "男";
                        }
                        else
                        {
                            certInfo.Sex = "";
                        }
                    }
                    response.Data = certInfo;
                }
                else
                {
                    response.ResultCode = -1;
                }
            }
            else
            {
                response.ResultCode = -1;
            }
            return response;
        }
    }
}
