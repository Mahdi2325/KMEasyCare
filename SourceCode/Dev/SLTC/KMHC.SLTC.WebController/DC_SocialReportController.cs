using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.DC.Report;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Interface.DC.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController
{
    public partial class DC_ReportController : ReportBaseController
    {
        IDC_SocialReportService reportManageService = IOCContainer.Instance.Resolve<IDC_SocialReportService>();
        IDC_SocialWorkerService service = IOCContainer.Instance.Resolve<IDC_SocialWorkerService>();
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        public ActionResult Preview()
        {
            string templateName = Request["templateName"];
            string recId = Request["recId"];
           
            ReportRequest request=new ReportRequest();
           
            if (templateName != null)
            {
                switch (templateName)
                {
                    case "DCS1.1":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCS1.1", this.LiftHistoryOperation, request);
                        }
                        break;
                    case "DCS1.2":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCS1.2", this.ReferralOperation, request);
                        }
                        break;
                    //
                    case "DCS1.3":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCS1.3", this.BasicInfoOperation, request);
                        }
                        break;
                    case "DCS1.4":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCS1.4", this.RegLifeQualityEvalOperation, request);
                        }
                        break;
                    case "DCS1.5":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCS1.5", this.IpdOutOperation, request);
                        }
                        break;
                    case "DCS1.7":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCS1.7", this.IpdInOperation, request);
                        }
                        break;
                    case "DCS1.6":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCS1.6", this.RegQuestionEvalOperation, request);
                        }
                        break;
                    case "DCS1.8":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCS1.8", this.OnDayLifeOperation, request);
                        }
                        break;
                    case "DCS1.9":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            
                            request.id = int.Parse(recId);
                            request.feeNo = Convert.ToInt32(Request["feeNo"]);
                            this.GeneratePDF("DCS1.9", this.OnSocialEvalOperation, request);
                        }
                        break;
                }
            }
            return View();
        }

        //Add By Duke on 20160801 导出
        public ActionResult DownLoadSocialReport()
        {
            string templateName = Request["templateName"];
            string recId = Request["recId"];

            ReportRequest request = new ReportRequest();

            if (templateName != null)
            {
                switch (templateName)
                {
                    case "DCS1.1":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCS1.1", this.LiftHistoryOperation, request);
                        }
                        break;
                    case "DCS1.2":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCS1.2", this.ReferralOperation, request);
                        }
                        break;
                    //
                    case "DCS1.3":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCS1.3", this.BasicInfoOperation, request);
                        }
                        break;
                    case "DCS1.4":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCS1.4", this.RegLifeQualityEvalOperation, request);
                        }
                        break;
                    case "DCS1.5":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCS1.5", this.IpdOutOperation, request);
                        }
                        break;
                    case "DCS1.7":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCS1.7", this.IpdInOperation, request);
                        }
                        break;
                    case "DCS1.6":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCS1.6", this.RegQuestionEvalOperation, request);
                        }
                        break;
                    case "DCS1.8":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCS1.8", this.OnDayLifeOperation, request);
                        }
                        break;
                    case "DCS1.9":
                        if (!string.IsNullOrEmpty(recId))
                        {

                            request.id = int.Parse(recId);
                            request.feeNo = Convert.ToInt32(Request["feeNo"]);
                            this.Download("DCS1.9", this.OnSocialEvalOperation, request);
                        }
                        break;
                }
            }
            return View();
        }
        /// <summary>
        /// 收案
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="request"></param>
        private void IpdInOperation(WordDocument doc, ReportRequest request)
        {
            var question = reportManageService.GetIpdRegInById(request.id);
            if (question == null) return;
            BindData(question.Data, doc);
        }
        /// <summary>
        /// 结案
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="request"></param>
        private void IpdOutOperation(WordDocument doc, ReportRequest request)
        {
            var question = reportManageService.GetIpdRegInById(request.id);
            if (question == null) return;
            BindData(question.Data, doc);
        }
        /// <summary>
        /// 家庭照顾者生活品质评估问卷
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="request"></param>
        private void RegLifeQualityEvalOperation(WordDocument doc, ReportRequest request)
        {
            
            var question = service.GetRegLifeQualityEvalById(request.id);
            var basic = reportManageService.GetBasicInfoById(Convert.ToInt32(question.Data.FeeNo));

            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            if (basic != null)
            {
                doc.ReplaceText("Org", org.Data.OrgName);
                doc.ReplaceText("ResidentNo", basic.Data.ResidentNo);
                doc.ReplaceText("Name", basic.Data.RegName);
                doc.ReplaceText("Nick", basic.Data.NickName == null ? "" : basic.Data.NickName);
            }
            if (question == null) return;
            else
                question.Data.Ability = question.Data.FamilyAbility;
            BindData(question.Data, doc);
        }
        /// 个案基本资料
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="request"></param>
        private void BasicInfoOperation(WordDocument doc, ReportRequest request)
        {
            var question = reportManageService.GetBasicInfoById(request.id);
            if (question == null) return;
            BindData(question.Data, doc);

            string imgPath = question.Data.EcologicalMap;
            if (imgPath != null)
            {
                try
                {
					string mapPath = Server.MapPath(VirtualPathUtility.GetDirectory("~")) + imgPath.Substring(1);
                    doc.InsertImage("InsertImage", mapPath, 300, 200);

 
					

                }
                catch (Exception ex)
                {
                    doc.ReplaceText("InsertImage", "");
                    //应跳转至错误提示页
                }
                
            }
            else
            {
                doc.ReplaceText("InsertImage", "");
            }
        }
        /// <summary>
        /// 个案生活史
        /// </summary>
        /// <param name="doc"></param>
        private void LiftHistoryOperation(WordDocument doc, ReportRequest request)
        {
            //IDC_SocialReportService reportManageService = IOCContainer.Instance.Resolve<IDC_SocialReportService>();
            var question = reportManageService.GetLifeHistoryById(request.id);
            if (question == null) return;
            BindData(question.Data, doc);
        }
        /// <summary>
        /// 个案转介
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="request"></param>
        private void ReferralOperation(WordDocument doc, ReportRequest request)
        {
            var question = reportManageService.GetReferralById(request.id);
            if (question == null) return;
            BindData(question.Data, doc);

            var basic = reportManageService.GetBasicInfoById(Convert.ToInt32(question.Data.FeeNo));

            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            if (basic != null)
            {
                doc.ReplaceText("Org", org.Data.OrgName);
                string imgPath = basic.Data.EcologicalMap;
                if (imgPath != null)
                {
                    
                    //doc.InsertImage("InsertImage", mapPath, 300, 200);
                    try
                    {
						string mapPath = Server.MapPath(VirtualPathUtility.GetDirectory("~")) + imgPath.Substring(1);
                        doc.InsertImage("InsertImage", mapPath, 300, 200);
                    }
                    catch (Exception ex)
                    {
                        //应跳转至错误提示页
                    }
                }
            }

            
        }
        /// <summary>
        /// 受托长辈适应程度评估表
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="request"></param>
        private void RegQuestionEvalOperation(WordDocument doc, ReportRequest request)
        {
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            
            var eval = reportManageService.GetRegQuestionEvalRec(request.id,1);

            var basic = reportManageService.GetBasicInfoById(Convert.ToInt32(eval.Data[0].FEENO));

            doc.ReplaceText("Org", org.Data.OrgName);
            doc.ReplaceText("ResidentNo", basic.Data.ResidentNo==null?"":basic.Data.ResidentNo);
            doc.ReplaceText("Name", basic.Data.RegName);
            doc.ReplaceText("Sex", basic.Data.Sex);
            doc.ReplaceText("Age", (DateTime.Now.Year-Convert.ToDateTime(basic.Data.BirthDate).Year).ToString());
            doc.ReplaceText("InDate", Convert.ToDateTime(basic.Data.InDate).ToShortDateString());
            doc.ReplaceText("EvalResult", eval.Data[0].EVALRESULT);
            doc.ReplaceText("Score", eval.Data[0].SCORE.ToString());//
            if (eval.Data != null && eval.Data.Count > 0) {
                DataTable dt1 = BuildTable(reportManageService.GetRegQuestionEvalRec(request.id, 1), 1);
                doc.FillTable(0, dt1, "", "",0);
                dt1.Dispose();

                DataTable dt2 = BuildTable(reportManageService.GetRegQuestionEvalRec(request.id, 2), 2);
                doc.FillTable(1, dt2, "", "", 0);
                dt2.Dispose();

                var eval3 = reportManageService.GetRegQuestionEvalRec(request.id, 3);
                if (eval3 != null)
                {
                    foreach (var item in eval3.Data)
                    {
                        if (item.ITEMNAME.Trim() == "护理人员")
                            doc.ReplaceText("ItemValue1", item.ITEMVALUE == null ? "0" : item.ITEMVALUE.ToString());
                        if (item.ITEMNAME.Trim() == "社工人员")
                            doc.ReplaceText("ItemValue2", item.ITEMVALUE == null ? "0" : item.ITEMVALUE.ToString());
                        if (item.ITEMNAME.Trim() == "照服员")
                            doc.ReplaceText("ItemValue3", item.ITEMVALUE == null ? "0" : item.ITEMVALUE.ToString());
                        if (item.ITEMNAME.Trim() == "志工")
                            doc.ReplaceText("ItemValue4", item.ITEMVALUE == null ? "0" : item.ITEMVALUE.ToString());
                        if (item.ITEMNAME.Trim() == "其他受托长辈")
                            doc.ReplaceText("ItemValue5", item.ITEMVALUE == null ? "0" : item.ITEMVALUE.ToString());
                    }
                }

                DataTable dt4 = BuildTable(reportManageService.GetRegQuestionEvalRec(request.id, 4),4);
                doc.FillTable(3, dt4, "", "", 0);
                dt4.Dispose();

                DataTable dt5 = BuildTable(reportManageService.GetRegQuestionEvalRec(request.id, 5), 5);
                doc.FillTable(4, dt5, "", "", 0);
                dt5.Dispose();
            }
            
            //if (question == null) return;
            //BindData(question.Data, doc);
        }

        private static DataTable BuildTable(Business.Entity.Base.BaseResponse<List<Business.Entity.DC_RegQuestionDataModel>> eval, int qId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            dt.Columns.Add("c4");

            BuildRow(dt,qId);
            DataRow r1 = dt.NewRow();
            int? totalSocre = 0;
            for (var n = 0; n < eval.Data.Count; n++)
            {
                var dr = dt.NewRow();
                dr["c1"] = eval.Data[n].SHOWNUMBER;
                dr["c2"] = eval.Data[n].ITEMNAME;
                dr["c3"] = eval.Data[n].ITEMVALUE;
                dr["c4"] = "";
                totalSocre += eval.Data[n].ITEMVALUE;
                dt.Rows.Add(dr);

            }
            r1["c1"] = "";
            r1["c2"] = "合计";
            r1["c3"] = totalSocre.ToString();
            r1["c4"] = "";
            dt.Rows.Add(r1);
            return dt;
        }

        private static void BuildRow(DataTable dt,int qId)
        {
            DataRow r = dt.NewRow();
            switch (qId)
            {
                case 1:
                    r["c1"] = "";
                    r["c2"] = "环境与位置";
                    r["c3"] = "分值";
                    r["c4"] = "说明";
                    break;
                case 2:
                    r["c1"] = "";
                    r["c2"] = "";
                    r["c3"] = "分值";
                    r["c4"] = "说明";
                    break;
                case 4:
                    r["c1"] = "";
                    r["c2"] = "";
                    r["c3"] = "分值";
                    r["c4"] = "说明";
                    break;
                case 5:
                    r["c1"] = "";
                    r["c2"] = "";
                    r["c3"] = "分值";
                    r["c4"] = "说明";
                    break;
            }
            
            dt.Rows.Add(r);
        }
        
        private void OnDayLifeOperation(WordDocument doc, ReportRequest request)
        {
            var question = reportManageService.GetOneDayLifeById(request.id);
            if (question == null) return;
            BindData(question.Data, doc);
        }
        /// <summary>
        /// 社工个案评估与处遇计划表
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="request"></param>
        private void OnSocialEvalOperation(WordDocument doc, ReportRequest request)
        {
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            doc.ReplaceText("Org", org.Data.OrgName);

            var basic = reportManageService.GetBasicInfoById(Convert.ToInt32(request.feeNo));

            var question = service.QuerySwRegEvalPlan(request.id,Convert.ToInt32(request.feeNo));
            if (question == null) return;
            BindData(question.Data.swRegEvalPlanModel, doc);
            doc.ReplaceText("YEAR", Convert.ToDateTime(question.Data.swRegEvalPlanModel.INDATE).Year.ToString());
            doc.ReplaceText("MONTH", Convert.ToDateTime(question.Data.swRegEvalPlanModel.INDATE).Month.ToString());
            doc.ReplaceText("DAY", Convert.ToDateTime(question.Data.swRegEvalPlanModel.INDATE).Day.ToString());
            doc.ReplaceText("AGE",(DateTime.Now.Year- Convert.ToDateTime(question.Data.swRegEvalPlanModel.BIRTHDATE).Year).ToString());

            string imgPath = basic.Data.EcologicalMap;
            if (imgPath != null) {
                
                //string imagePath = string.Format(@"{0}\{1}", mapPath, basic.Data.EcologicalMap);
                //doc.InsertImage("InsertImage", mapPath, 300, 200);
                try
                {
					string mapPath = Server.MapPath(VirtualPathUtility.GetDirectory("~")) + imgPath.Substring(1);
                    doc.InsertImage("InsertImage", mapPath, 300, 200);
                }
                catch (Exception ex)
                {
                    doc.ReplaceText("InsertImage", "");
                    //应跳转至错误提示页
                }
            }
            else
            {
                doc.ReplaceText("InsertImage","");
            }
            

            DataTable dt = new DataTable();
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            DataRow r = dt.NewRow();
            r["c1"] = "前次工作目标";
            r["c2"] = "执行情况";
            r["c3"] = "执行成效";
            dt.Rows.Add(r);
      
            for (var n = 0; n < question.Data.TaskGoalsStrategyModel.Count; n++)
            {
                var dr = dt.NewRow();
                dr["c1"] = question.Data.TaskGoalsStrategyModel[n].QUESTIONTYPE;
                dr["c2"] = question.Data.TaskGoalsStrategyModel[n].CPDIA;
                dr["c3"] = question.Data.TaskGoalsStrategyModel[n].EVALUATIONVALUE;
    
                dt.Rows.Add(dr);

            }
            doc.FillTable(1, dt, "", "", 0);
            dt.Dispose();

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("c1");
            dt1.Columns.Add("c2");
            dt1.Columns.Add("c3");
            dt1.Columns.Add("c4");
            dt1.Columns.Add("c5");
            dt1.Columns.Add("c6");
            DataRow r1 = dt1.NewRow();
            r1["c1"] = "需求类别";
            r1["c2"] = "项目";
            r1["c3"] = "问题描述";
            r1["c4"] = "处遇目标";
            r1["c5"] = "阻助力分析";
            r1["c6"] = "介入方式或资源连接";
            dt1.Rows.Add(r1);
       
            for (var n = 0; n < question.Data.TaskGoalsStrategyModel.Count; n++)
            {
                var dr = dt1.NewRow();
                dr["c1"] = question.Data.TaskGoalsStrategyModel[n].QUESTIONTYPE;
                dr["c2"] = question.Data.TaskGoalsStrategyModel[n].CPDIA;
                dr["c3"] = question.Data.TaskGoalsStrategyModel[n].QUESTIONDESC;
                dr["c4"] = question.Data.TaskGoalsStrategyModel[n].TREATMENTGOAL;
                dr["c5"] = question.Data.TaskGoalsStrategyModel[n].QUESTIONANALYSIS;
                dr["c6"] = question.Data.TaskGoalsStrategyModel[n].PLANACTIVITY;
                dt1.Rows.Add(dr);

            }
            doc.FillTable(2, dt1, "", "", 0);
            dt1.Dispose();
        }
    }
}

