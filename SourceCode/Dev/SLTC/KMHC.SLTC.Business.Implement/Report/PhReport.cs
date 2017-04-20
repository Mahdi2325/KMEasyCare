using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.Report
{
    public class PhReport : BaseReport
    {
        ICarePlanReportService carePlanservice = IOCContainer.Instance.Resolve<ICarePlanReportService>();
        INursingManageService nursingSvc = IOCContainer.Instance.Resolve<INursingManageService>();
        IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        protected override void Operation(WordDocument doc)
        {
            int id = Convert.ToInt32(ParamId);
            var now = DateTime.Now;
            var dtdc = new DateTime(1900, 1, 1);
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            doc.ReplaceText("Year", StartDate.Year.ToString());
            doc.ReplaceText("Month ", StartDate.Month.ToString());

            DoctorCheckRecFilter filter = new DoctorCheckRecFilter
            {
                Id = id
            };
            BaseRequest<DoctorCheckRecFilter> request = new BaseRequest<DoctorCheckRecFilter>
            {
                Data = filter
            };
            request.CurrentPage = 1;
            request.PageSize = 1000;
            var respone = carePlanservice.QueryDocCheckRecData(request);
            if (respone.Data != null && respone.Data.Count > 0)
            {
                doc.ReplaceText("bs1", respone.Data[0].Bs.HasValue ? respone.Data[0].Bs.ToString() : "");
                doc.ReplaceText("spo2", respone.Data[0].Bs.HasValue ? respone.Data[0].Oxygen.ToString() : "");
                doc.ReplaceText("OtherDesc", respone.Data[0].OtherDesc);
                doc.ReplaceText("PhyName", respone.Data[0].DocName);

                var bedrequest = new BaseRequest<BedBasicFilter>()
                {
                    CurrentPage = 1,
                    PageSize = 1000,
                    Data = new BedBasicFilter()
                    {
                        OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                        KeyWords = ""
                    }
                };
                var bedresponse = organizationManageService.QueryBedBasicExtend(bedrequest);
                if (bedresponse.Data != null)
                {
                    var bedinfo = bedresponse.Data.Where(o => o.FEENO == Convert.ToInt64(respone.Data[0].FeeNo)).FirstOrDefault();
                    if (bedinfo != null)
                    {
                        doc.ReplaceText("BedNo",bedinfo.BedNo);
                        doc.ReplaceText("Name", bedinfo.ResidentName);
                    }
                    else
                    {
                        doc.ReplaceText("BedNo", "");
                        doc.ReplaceText("Name", "");
                    }
                }
                else
                {
                    doc.ReplaceText("BedNo", "");
                    doc.ReplaceText("Name", "");
                }


                var careresponse = nursingSvc.QueryLatestCareDemand(Convert.ToInt64(respone.Data[0].FeeNo));
                if (careresponse.Data != null)
                {
                    #region 护理记录

                    var caredemandevalprivew = reportManageService.GetCareDemandHis(Convert.ToInt32(careresponse.Data.ID), SecurityHelper.CurrentPrincipal.OrgId);
                    if (caredemandevalprivew != null)
                    {
                        doc.ReplaceText("v01", caredemandevalprivew.v01);
                        doc.ReplaceText("v02", caredemandevalprivew.v02);
                        doc.ReplaceText("v03", caredemandevalprivew.v03);
                        doc.ReplaceText("v04", caredemandevalprivew.v04);
                        doc.ReplaceText("v05", caredemandevalprivew.v05);
                        doc.ReplaceText("v08", caredemandevalprivew.v08);
                        doc.ReplaceText("v09", caredemandevalprivew.v09);
                        doc.ReplaceText("v10", caredemandevalprivew.v10);
                        doc.ReplaceText("vs08", caredemandevalprivew.vs08);
                        doc.ReplaceText("v34", caredemandevalprivew.v34);
                        doc.ReplaceText("v31", caredemandevalprivew.v31);
                        doc.ReplaceText("v30", caredemandevalprivew.v30);
                        doc.ReplaceText("v13", caredemandevalprivew.v13);
                        doc.ReplaceText("v51", caredemandevalprivew.v51);
                        doc.ReplaceText("v52", caredemandevalprivew.v52);
                        doc.ReplaceText("v55", caredemandevalprivew.v55);
                        doc.ReplaceText("v58", caredemandevalprivew.v58);
                        doc.ReplaceText("v77", caredemandevalprivew.v77);
                        doc.ReplaceText("v73", caredemandevalprivew.v73);
                        doc.ReplaceText("Ill_history", caredemandevalprivew.Ill_history);
                    }
                    else
                    {
                        clearCare(doc);
                    }

                    #endregion


                    #region 就医用药
                    VisitDocRecordsFilter visfilter = new VisitDocRecordsFilter
                    {
                        FeeNo = respone.Data[0].FeeNo,
                    };
                    BaseRequest<VisitDocRecordsFilter> visrequest = new BaseRequest<VisitDocRecordsFilter>
                    {
                        Data = visfilter,
                        CurrentPage = 1,
                        PageSize = 1000,
                    };
                    var visResponse = nursingSvc.QueryVisitDocRecData(visrequest);
                    if (visResponse.Data != null && visResponse.Data.Count > 0)
                    {
                        var seqNo = visResponse.Data.OrderByDescending(o => o.VisitDate).ToList().FirstOrDefault().SeqNo;

                        VisitPrescriptionFilter prefilter = new VisitPrescriptionFilter
                        {
                            SeqNo = seqNo
                        };
                        BaseRequest<VisitPrescriptionFilter> Prerequest = new BaseRequest<VisitPrescriptionFilter>
                        {
                            Data = prefilter
                        };
                        var preresponse = nursingSvc.QueryVisitPreData(Prerequest);
                        if (preresponse.Data != null && preresponse.Data.Count > 0)
                        {
                            var medicationRecord = string.Empty;
                            foreach (var item in preresponse.Data)
                            {
                                if (!string.IsNullOrEmpty(item.ChnName))
                                {
                                    medicationRecord += item.ChnName;
                                }
                            }
                            doc.ReplaceText("MedicationRecord", medicationRecord);
                        }
                        else
                        {
                            doc.ReplaceText("MedicationRecord", "");
                        }

                    }
                    else
                    {
                        doc.ReplaceText("MedicationRecord", "");
                    }
                    #endregion

                }
                else
                {
                    clearCare(doc);
                    doc.ReplaceText("MedicationRecord", "");

                }
            }
            else
            {
                doc.ReplaceText("bs1", "");
                doc.ReplaceText("spo2", "");
                doc.ReplaceText("OtherDesc", "");
                doc.ReplaceText("PhyName", "");
                clearCare(doc);
            }

        }

        public void clearCare(WordDocument doc)
        {
            doc.ReplaceText("v01", "");
            doc.ReplaceText("v02", "");
            doc.ReplaceText("v03", "");
            doc.ReplaceText("v04", "");
            doc.ReplaceText("v05", "");
            doc.ReplaceText("v08", "");
            doc.ReplaceText("v09", "");
            doc.ReplaceText("v10", "");
            doc.ReplaceText("vs08", "");
            doc.ReplaceText("v34", "");
            doc.ReplaceText("v31", "");
            doc.ReplaceText("v13", "");
            doc.ReplaceText("v30", "");
            doc.ReplaceText("v51", "");
            doc.ReplaceText("v52", "");
            doc.ReplaceText("v55", "");
            doc.ReplaceText("v58", "");
            doc.ReplaceText("v77", "");
            doc.ReplaceText("v73", "");
            doc.ReplaceText("Ill_history", "");



        }
    }
}
