using KM.Common;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Report;
using KMHC.SLTC.Business.Interface.DC.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController
{
    public partial class DC_ReportController : ReportBaseController
    {
        public ActionResult PreviewNursingReport()
        {
            string templateName = Request["templateName"];
            string feeNo = Request["feeNo"];
            string id = Request["id"];
            ReportRequest request = new ReportRequest();

            if (templateName != null)
            {
                switch (templateName)
                {
                    case "DCN1.1":
                        if (!string.IsNullOrEmpty(feeNo))
                        {
                            request.feeNo = int.Parse(feeNo);
                            this.GeneratePDF("DCN1.1", this.RegMedReport, request);
                        }

                        break;
                    case "DCN1.2":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            this.GeneratePDF("DCN1.2", this.RegRequirMentEvalReport, request);                           
                        }

                        break;
                    case "DCN1.3":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            this.GeneratePDF("DCN1.3", this.RegActivityRequestEvalReport, request);
                        }

                        break;
                    case "DCN1.4":
                        if (!string.IsNullOrEmpty(feeNo))
                        {
                            request.feeNo = int.Parse(feeNo);
                            this.GeneratePDF("DCN1.4", this.RegNursingDiagReport, request);
                        }

                        break;
                    case "DCN1.5":
                        if (!string.IsNullOrEmpty(feeNo))
                        {
                            request.feeNo = int.Parse(feeNo);
                            this.GeneratePDF("DCN1.5", this.BasicInfoListReport, request);
                        }

                        break;
                }
            }
            return View("Preview");
        }

        public void DownLoadReport()
        {
            string templateName = Request["templateName"];
            string feeNo = Request["feeNo"];
            string id = Request["id"];
            ReportRequest request = new ReportRequest();

            if (templateName != null)
            {
                switch (templateName)
                {
                    case "DCN1.1":
                        if (!string.IsNullOrEmpty(feeNo))
                        {
                            request.feeNo = int.Parse(feeNo);
                            this.Download("DCN1.1", this.RegMedReport, request);
                        }

                        break;
                    case "DCN1.2":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            this.Download("DCN1.2", this.RegRequirMentEvalReport, request);
                        }

                        break;
                    case "DCN1.3":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            this.Download("DCN1.3", this.RegActivityRequestEvalReport, request);
                        }

                        break;
                    case "DCN1.4":
                        if (!string.IsNullOrEmpty(feeNo))
                        {
                            request.feeNo = int.Parse(feeNo);
                            this.Download("DCN1.4", this.RegNursingDiagReport, request);
                        }

                        break;
                    case "DCN1.5":
                        if (!string.IsNullOrEmpty(feeNo))
                        {
                            request.feeNo = int.Parse(feeNo);
                            this.Download("DCN1.5", this.BasicInfoListReport, request);
                        }

                        break;
                }
            }
        }


        //1.1
        private void RegMedReport(WordDocument doc, ReportRequest request)
        {
            IDC_NursingReportService reportManageService = IOCContainer.Instance.Resolve<IDC_NursingReportService>();

            IList<ReportRegMedicine> rRegMed = reportManageService.QueryRegMedicineList(new BaseRequest<ReportRegMedicine> { Data = new ReportRegMedicine { FeeNo = request.feeNo } }).Data;
            if (rRegMed != null && rRegMed.Count > 0)
            {
                BindData(rRegMed[0], doc);
                DataTable dt = new DataTable();
                dt.Columns.Add("MedicineName");
                dt.Columns.Add("HospitalName");
                dt.Columns.Add("DeptName");
                dt.Columns.Add("TakeQty");
                dt.Columns.Add("TakeProc");
                dt.Columns.Add("TakeDateTime");
                dt.Columns.Add("StartDate");
                dt.Columns.Add("EndDate");
                dt.Columns.Add("BreakDate");
                dt.Columns.Add("BreakReason");
                var dr = dt.NewRow();
                foreach (var item in rRegMed)
                {
                    dr = dt.NewRow();
                    dr["MedicineName"] = item.MedicineName;
                    dr["HospitalName"] = item.HospitalName;
                    dr["DeptName"] = item.DeptName;
                    dr["TakeQty"] = item.TakeQty;
                    dr["TakeProc"] = item.TakeProc;
                    dr["TakeDateTime"] = item.TakeDateTime;

                    DateTime sDate = item.StartDate ?? new DateTime();
                    DateTime eDate = item.EndDate ?? new DateTime();
                    DateTime bDate = item.BreakDate ?? new DateTime();
                    if (item.StartDate == null)
                    {
                        dr["StartDate"] = "";
                    }
                    else
                    {
                        dr["StartDate"] = sDate.ToString("yyyy-MM-dd");
                    }
                    if (item.EndDate == null)
                    {
                        dr["EndDate"] = "";
                    }
                    else
                    {
                        dr["EndDate"] = eDate.ToString("yyyy-MM-dd");
                    }
                    if (item.BreakDate == null)
                    {
                        dr["BreakDate"] = "";
                    }
                    else
                    {
                        dr["BreakDate"] = bDate.ToString("yyyy-MM-dd");
                    }
                    dr["BreakReason"] = item.BreakReason;
                    dt.Rows.Add(dr);
                }
                doc.FillTable(0, dt, "", "", 1);
            }
        }
        //1.2
        private void RegRequirMentEvalReport(WordDocument doc, ReportRequest request)
        {
            IDC_NursingReportService reportManageService = IOCContainer.Instance.Resolve<IDC_NursingReportService>();
            ReportNurseingPlanEval data = reportManageService.QueryNurseingPlanEval(new BaseRequest<ReportNurseingPlanEval> { Data = new ReportNurseingPlanEval { Id = request.id } }).Data;
            BindData(data, doc);
            if (data.lastNursingPlan.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Cpdia");
                dt.Columns.Add("Activity");
                dt.Columns.Add("Finished");
                for (int i = 0; i < data.lastNursingPlan.Count; i++)
                {
                    var dr = dt.NewRow();
                    dr["Cpdia"] = " " + data.lastNursingPlan[i].Cpdia;
                    dr["Activity"] = data.lastNursingPlan[i].Activity;
                    dr["Finished"] = data.lastNursingPlan[i].Finished;
                    dt.Rows.Add(dr);
                }
                doc.FillTable(1, dt, "", "", 1);
            }
            if (data.nursingPlan.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Cpdia");
                dt.Columns.Add("Activity");
                for (int i = 0; i < data.nursingPlan.Count; i++)
                {
                    var dr = dt.NewRow();
                    dr["Cpdia"] = " " + data.nursingPlan[i].Cpdia;
                    dr["Activity"] = data.nursingPlan[i].Activity;
                    dt.Rows.Add(dr);
                }

                doc.FillTable(2, dt, "", "", 1);
            }
        }
        //1.3
        private void RegActivityRequestEvalReport(WordDocument doc, ReportRequest request)
        {
            IDC_NursingReportService reportManageService = IOCContainer.Instance.Resolve<IDC_NursingReportService>();

            ReportRegActivityRequestEval rRegActivityRequestEval = reportManageService.QueryCurrentRegActivityRequestEval(request.id).Data;

            if (rRegActivityRequestEval != null)
            {
                BindData(rRegActivityRequestEval, doc);
            }
        }
        //1.4
        private void RegNursingDiagReport(WordDocument doc, ReportRequest request)
        {
            IDC_NursingReportService reportManageService = IOCContainer.Instance.Resolve<IDC_NursingReportService>();

            IList<ReportRegCpl> rRegNursingDiag = reportManageService.QueryRegCplList(new BaseRequest<ReportRegCpl> { Data = new ReportRegCpl { FeeNo = request.feeNo } }).Data;
            if (rRegNursingDiag != null && rRegNursingDiag.Count > 0)
            {
                BindData(rRegNursingDiag[0], doc);
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("CpDia");
                dt.Columns.Add("StartDate");
                dt.Columns.Add("TargetDate");
                var dr = dt.NewRow();
                var index = 0;
                foreach (var item in rRegNursingDiag)
                {
                    index++;
                    dr = dt.NewRow();
                    dr["ID"] = "#" + index.ToString();
                    dr["CpDia"] = item.CpDia;
                    DateTime sDate = item.StartDate ?? new DateTime();
                    DateTime eDate = item.FinishDate ?? new DateTime();
                    if (item.StartDate == null)
                    {
                        dr["StartDate"] = "";
                    }
                    else
                    {
                        dr["StartDate"] = sDate.ToString("yyyy-MM-dd");
                    }
                    if (item.FinishDate == null)
                    {
                        dr["TargetDate"] = "";
                    }
                    else
                    {
                        dr["TargetDate"] = eDate.ToString("yyyy-MM-dd");
                    }
                    dt.Rows.Add(dr);
                }
                doc.FillTable(0, dt, "", "", 2);
            }
        }
        //1.5
        private void BasicInfoListReport(WordDocument doc, ReportRequest request)
        {
            IDC_NursingReportService reportManageService = IOCContainer.Instance.Resolve<IDC_NursingReportService>();
            IList<ReportBaseInfoList> rbaseInfoList = reportManageService.QueryAllRegBaseInfoList(request.feeNo).Data;
            if (rbaseInfoList != null && rbaseInfoList.Count > 0)
            {
                BindData(rbaseInfoList[0], doc);
                DataTable dt = new DataTable();
                var colCount = rbaseInfoList.Count;
                for (var i = 0; i < colCount; i++)
                {
                    dt.Columns.Add((i + 1).ToString());

                }

                var dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = "第" + rbaseInfoList[j].Cnt + "次";
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].ResidentNo + "/" + rbaseInfoList[j].RegName;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].ContactName + "/" + rbaseInfoList[j].ContactPhone;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Address;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].BirthDate;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Language;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Vs;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Job;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Religion;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].MerryState;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Education;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Height;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Weight;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Bmi;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].WaistLine;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].DiseaseHistory;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].AdlScore;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].IadlScore;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].MmseScore;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].GdsScore;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].UpperDisorder;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].LowerDisorder;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Aphasia;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].VisuallyImpaired;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].HearingImpaired;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].FalseTeethu;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].FalseTeethl;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].NoteatFood;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Likefood;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Questionbehavior;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Checkdate;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Xray;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Syphilis;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Aids;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Hbsag;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].AmibaDysentery;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].InsectEgg;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].BacillusDysentery;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].NextCheckdate;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = rbaseInfoList[j].Medicine;
                }
                dt.Rows.Add(dr);
                doc.FillBasicInfoTable(0, dt);
            }
        }

    }
}
