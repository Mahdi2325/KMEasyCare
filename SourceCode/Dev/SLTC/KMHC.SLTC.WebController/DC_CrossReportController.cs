using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.DC.Report;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Business.Interface.DC.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController
{
    public partial class DC_ReportController : ReportBaseController
    {
        IDC_CrossReportService CrossManageService = IOCContainer.Instance.Resolve<IDC_CrossReportService>();

        public ActionResult PreviewNurseCare()
        {
            string templateName = Request["templateName"];
            string recId = Request["recId"];

            string FeeNo = Request["FeeNo"];

            string year = Request["year"];

            string month = Request["month"];
            string seqNo = Request["seqNo"];
            ReportRequest request = new ReportRequest();

            if (templateName != null)
            {
                switch (templateName)
                {
                    case "DCC1.1":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCC1.1", this.NurseCareOperation, request);
                        }
                        break;

                    case "DCC1.2":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.GeneratePDF("DCC1.2", this.DayLifeOperation, request);
                        }
                        break;
                    case "DCC1.3":
                        if (!string.IsNullOrEmpty(FeeNo))
                        {
                            request.feeNo = int.Parse(FeeNo);
                            request.year = int.Parse(year);
                            request.month = int.Parse(month);
                            this.GeneratePDF("DCC1.3",this.AbOperation, request);
                        }
                        break;
                    case "DCC3.0":
                        if (!string.IsNullOrEmpty(seqNo))
                        {
                            request.seqNo = long.Parse(seqNo);
                            this.GeneratePDF("DCC3.0", this.TransdisciplinaryOperation, request);
                        }
                        break;                   
                }
            }
            return View("Preview");
        }

        //Add by Duke on 20160801 导出
        public void DownLoadCrossReport()
        {
            string templateName = Request["templateName"];
            string recId = Request["recId"];

            string FeeNo = Request["FeeNo"];

            string year = Request["year"];

            string month = Request["month"];
            string seqNo = Request["seqNo"];
            ReportRequest request = new ReportRequest();

            if (templateName != null)
            {
                switch (templateName)
                {
                    case "DCC1.1":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCC1.1", this.NurseCareOperation, request);
                        }
                        break;

                    case "DCC1.2":
                        if (!string.IsNullOrEmpty(recId))
                        {
                            request.id = int.Parse(recId);
                            this.Download("DCC1.2", this.DayLifeOperation, request);
                        }
                        break;
                    case "DCC1.3":
                        if (!string.IsNullOrEmpty(FeeNo))
                        {
                            request.feeNo = int.Parse(FeeNo);
                            request.year = int.Parse(year);
                            request.month = int.Parse(month);
                            this.Download("DCC1.3", this.AbOperation, request);
                        }
                        break;
                    case "DCC3.0":
                        if (!string.IsNullOrEmpty(seqNo))
                        {
                            request.seqNo = long.Parse(seqNo);
                            this.Download("DCC3.0", this.TransdisciplinaryOperation, request);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 护理及生活照顾服务记录表
        /// </summary>
        /// <param name="doc"></param>
        private void AbOperation(WordDocument doc, ReportRequest request)
        {
            var question = CrossManageService.GetAb(request.feeNo, request.year, request.month);
            //根据求出  
            if (question != null)
            {
                BindData(question.Data.ab, doc);
                DataTable dt = new DataTable();
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                for (int i = 0; i < question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dt.Columns.Add("");

                }
                var dr = dt.NewRow();
                dr = dt.NewRow();
                dr[0] = null;
                for (int i = 0; i < question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = i + 1;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i-1].DELUSION;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].VISUALILLUSION;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].MISDEEM;
                }

                dt.Rows.Add(dr);
                //这边是第四个
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = "重复问同样问题";
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count+1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].REPEATASKING;
                }
               
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = "重复语言";
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].REPEATLANGUAGE;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = "重复行为";
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].REPEATBEHAVIOR;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = null;
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].VERBALATTACK;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = "肢体攻击行为";
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {

                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].BODYATTACK;
                }

                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].GETLOST;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].ROAM;
                }

                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].SLEEPDISORDER;
                }

                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = null;
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].FORGETEAT;
                }

                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = "拒绝饮食";
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].REFUSALTOEAT;
                }

                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = null;
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].EXPOSEDBODYPARTS;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = null;
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].NOTWEARCLOTHES;
                }

                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = null;
                for (int i = 2; i <= question.Data.AbNormaleMotionlist.Count + 1; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 2].INAPPROPRIATETOUCH;
                }

                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].COLLECTION;
                }

                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].IRRITABILITY;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].COMPLAIN;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].NOTCOOPERATE;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].REFUSEHYGIENE;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] = question.Data.AbNormaleMotionlist[i - 1].NOINTEREST;
                }
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = null;
                for (int i = 1; i <= question.Data.AbNormaleMotionlist.Count; i++)
                {
                    dr[i + 1] =null;
                }
                dt.Rows.Add(dr);
                doc.FillQuestion(0, dt, question.Data.AbNormaleMotionlist.Count);

           
            }
        }
        /// <summary>
        /// 护理及生活照顾服务记录表
        /// </summary>
        /// <param name="doc"></param>
        private void NurseCareOperation(WordDocument doc, ReportRequest request)
        {
            var question = CrossManageService.GetNurseCareById(request.id);
            //根据求出  
            if (question != null)
            {
                BindData(question.Data.NurseingLifeCareREC, doc);
                DataTable dt = new DataTable();
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                var dr = dt.NewRow();
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = null;
                    dr[2] = disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY9)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY9)[1];
                    dr[3] = disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY9)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY9)[1];
                    dr[4] = disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY9)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY9)[1];
                    dr[5] = disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY9)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY9)[1];
                    dr[6] = disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY9)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY9)[1];
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = null;
                    dr[2] = disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY11)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY11)[1];
                    dr[3] = disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY11)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY11)[1];
                    dr[4] = disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY11)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY11)[1];
                    dr[5] = disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY11)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY11)[1];
                    dr[6] = disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY11)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY11)[1];
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = null;
                    dr[2] = disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY14)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY14)[1];
                    dr[3] = disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY14)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY14)[1];
                    dr[4] = disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY14)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY14)[1];
                    dr[5] = disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY14)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY14)[1];
                    dr[6] = disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY14)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY14)[1];
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = null;
                    dr[2] = disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY15)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY15)[1];
                    dr[3] = disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY15)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY15)[1];
                    dr[4] = disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY15)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY15)[1];
                    dr[5] = disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY15)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY15)[1];
                    dr[6] = disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY15)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY15)[1];
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = null;
                    dr[2] = disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY16)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[0].ACTIVITY16)[1];
                    dr[3] = disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY16)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[1].ACTIVITY16)[1];
                    dr[4] = disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY16)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[2].ACTIVITY16)[1];
                    dr[5] = disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY16)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[3].ACTIVITY16)[1];
                    dr[6] = disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY16)[0] + "\n" + disposearr(question.Data.NurseingLifeCareEDTL[4].ACTIVITY16)[1];
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = null;
                    dr[2] = question.Data.NurseingLifeCareEDTL[0].BODYTEMPERATURE + "/" + question.Data.NurseingLifeCareEDTL[0].PULSE + "/" + question.Data.NurseingLifeCareEDTL[0].BREATH;
                    dr[3] = question.Data.NurseingLifeCareEDTL[1].BODYTEMPERATURE + "/" + question.Data.NurseingLifeCareEDTL[1].PULSE + "/" + question.Data.NurseingLifeCareEDTL[1].BREATH;
                    dr[4] = question.Data.NurseingLifeCareEDTL[2].BODYTEMPERATURE + "/" + question.Data.NurseingLifeCareEDTL[2].PULSE + "/" + question.Data.NurseingLifeCareEDTL[2].BREATH;
                    dr[5] = question.Data.NurseingLifeCareEDTL[3].BODYTEMPERATURE + "/" + question.Data.NurseingLifeCareEDTL[3].PULSE + "/" + question.Data.NurseingLifeCareEDTL[3].BREATH;
                    dr[6] = question.Data.NurseingLifeCareEDTL[4].BODYTEMPERATURE + "/" + question.Data.NurseingLifeCareEDTL[4].PULSE + "/" + question.Data.NurseingLifeCareEDTL[4].BREATH;
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = null;
                    dr[2] = question.Data.NurseingLifeCareEDTL[0].SBP + "/" + question.Data.NurseingLifeCareEDTL[0].DBP;
                    dr[3] = question.Data.NurseingLifeCareEDTL[1].SBP + "/" + question.Data.NurseingLifeCareEDTL[1].DBP;
                    dr[4] = question.Data.NurseingLifeCareEDTL[2].SBP + "/" + question.Data.NurseingLifeCareEDTL[2].DBP;
                    dr[5] = question.Data.NurseingLifeCareEDTL[3].SBP + "/" + question.Data.NurseingLifeCareEDTL[3].DBP;
                    dr[6] = question.Data.NurseingLifeCareEDTL[4].SBP + "/" + question.Data.NurseingLifeCareEDTL[4].DBP;
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = null;
                    dr[1] = null;
                    dr[2] = question.Data.NurseingLifeCareEDTL[0].MEDICINE;
                    dr[3] = question.Data.NurseingLifeCareEDTL[1].MEDICINE;
                    dr[4] = question.Data.NurseingLifeCareEDTL[2].MEDICINE;
                    dr[5] = question.Data.NurseingLifeCareEDTL[3].MEDICINE;
                    dr[6] = question.Data.NurseingLifeCareEDTL[4].MEDICINE;
                    dt.Rows.Add(dr);
                   doc.FillTable(0, dt, "", "", 1);
            }
        }
        //输入数组生成数组
        public string[] disposearr(string tt)
        {
            string[] result = new string[2];

            result = tt.Split('|');

            if (result[0] == ",undefined,undefined")
            {
                result[0] = null;
            }
            else if (result[0] == "" || result[0] == null)
            {

                result[0] = "";
            
            }

            else
            {

                result[0] = result[0].Split(',')[1];

            }
            return result;
        }
        public string[] disposearr1(string tt)
        {
            string[] result = new string[3];
            result = tt.Split('|');
            if (result.Length == 0)
            {
                return null;
            }
            else
            {
                for (var i = 0; i < result.Length; i++)
                {
                    
                    if (result[i] == "undefined,undefined")
                    {
                        result[i] = "/";
                       
                    }
                   else if (result[i].Split(',')[0] == "undefined")
                    {
                        result[i] = "/" + result[i].Split(',')[1];
                       
                    
                    }
                    else if (result[i].Split(',')[1] == "undefined")
                    {
                        result[i] = result[i].Split(',')[0] + "/";
                     

                    }
                    else
                    {

                        result[i] = result[i].Split(',')[0] + "/" + result[i].Split(',')[1];
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 日常生活照顾记录表
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="request"></param>
        private void DayLifeOperation(WordDocument doc, ReportRequest request)
        {
            var question = CrossManageService.GetDayLifeById(request.id);

            if (question != null)
            {
                BindData(question.Data.DayLifeRec, doc);

                DataTable dt = new DataTable();
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                dt.Columns.Add("");
                var dr = dt.NewRow();
                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = "9:00   茶水";
                dr[2] = question.Data.DayLifeCaredtl[0].TEA9;
                dr[3] = question.Data.DayLifeCaredtl[1].TEA9;
                dr[4] = question.Data.DayLifeCaredtl[2].TEA9;
                dr[5] = question.Data.DayLifeCaredtl[3].TEA9;
                dr[6] = question.Data.DayLifeCaredtl[4].TEA9;
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = "9:00";
                dr[2] = question.Data.DayLifeCaredtl[0].SNACKTEA9;
                dr[3] = question.Data.DayLifeCaredtl[1].SNACKTEA9;
                dr[4] = question.Data.DayLifeCaredtl[2].SNACKTEA9;
                dr[5] = question.Data.DayLifeCaredtl[3].SNACKTEA9;
                dr[6] = question.Data.DayLifeCaredtl[4].SNACKTEA9;
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = "午餐量(盘/碗)餐食形式";
                dr[2] = question.Data.DayLifeCaredtl[0].LUNCH;
                dr[3] = question.Data.DayLifeCaredtl[1].LUNCH;
                dr[4] = question.Data.DayLifeCaredtl[2].LUNCH;
                dr[5] = question.Data.DayLifeCaredtl[3].LUNCH;
                dr[6] = question.Data.DayLifeCaredtl[4].LUNCH;
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = "汤量";
                dr[2] = question.Data.DayLifeCaredtl[0].SOUPAMOUNT;
                dr[3] = question.Data.DayLifeCaredtl[1].SOUPAMOUNT;
                dr[4] = question.Data.DayLifeCaredtl[2].SOUPAMOUNT;
                dr[5] = question.Data.DayLifeCaredtl[3].SOUPAMOUNT;
                dr[6] = question.Data.DayLifeCaredtl[4].SOUPAMOUNT;
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = "14:00  茶水";
                dr[2] = question.Data.DayLifeCaredtl[0].TEA14;
                dr[3] = question.Data.DayLifeCaredtl[1].TEA14;
                dr[4] = question.Data.DayLifeCaredtl[2].TEA14;
                dr[5] = question.Data.DayLifeCaredtl[3].TEA14;
                dr[6] = question.Data.DayLifeCaredtl[4].TEA14;
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = null;
                dr[1] = "15:30";
                dr[2] = question.Data.DayLifeCaredtl[0].SNACKTEA1530;
                dr[3] = question.Data.DayLifeCaredtl[1].SNACKTEA1530;
                dr[4] = question.Data.DayLifeCaredtl[2].SNACKTEA1530;
                dr[5] = question.Data.DayLifeCaredtl[3].SNACKTEA1530;
                dr[6] = question.Data.DayLifeCaredtl[4].SNACKTEA1530;
                dt.Rows.Add(dr);

              

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("");
                dt1.Columns.Add("");
                dt1.Columns.Add("");
                dt1.Columns.Add("");
                dt1.Columns.Add("");
                dt1.Columns.Add("");


                dr = dt1.NewRow();
                dr[0] ="  午休";
                dr[1] = question.Data.DayLifeCaredtl[0].NOONBREAK;
                dr[2] = question.Data.DayLifeCaredtl[1].NOONBREAK;
                dr[3] = question.Data.DayLifeCaredtl[2].NOONBREAK;
                dr[4] = question.Data.DayLifeCaredtl[3].NOONBREAK;
                dr[5] = question.Data.DayLifeCaredtl[4].NOONBREAK;
                dt1.Rows.Add(dr);


                DataTable dt2 = new DataTable();
                dt2.Columns.Add("");
                dt2.Columns.Add("");
                dt2.Columns.Add("");
                dt2.Columns.Add("");
                dt2.Columns.Add("");
                dt2.Columns.Add("");
                dt2.Columns.Add("");



                dr = dt2.NewRow();
                dr[0] = null;
                dr[1] = "刷牙、漱口";
                dr[2] = question.Data.DayLifeCaredtl[0].BRUSHINGTEETH;
                dr[3] = question.Data.DayLifeCaredtl[1].BRUSHINGTEETH;
                dr[4] = question.Data.DayLifeCaredtl[2].BRUSHINGTEETH;
                dr[5] = question.Data.DayLifeCaredtl[3].BRUSHINGTEETH;
                dr[6] = question.Data.DayLifeCaredtl[4].BRUSHINGTEETH;
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr[0] = null;
                dr[1] = "会阴、冲洗";
                dr[2] = question.Data.DayLifeCaredtl[0].PERINEALWASHING;
                dr[3] = question.Data.DayLifeCaredtl[1].PERINEALWASHING;
                dr[4] = question.Data.DayLifeCaredtl[2].PERINEALWASHING;
                dr[5] = question.Data.DayLifeCaredtl[3].PERINEALWASHING;
                dr[6] = question.Data.DayLifeCaredtl[4].PERINEALWASHING;
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr[0] = null;
                dr[1] = "其    他";
                dr[2] = question.Data.DayLifeCaredtl[0].OTHERCLEAN;
                dr[3] = question.Data.DayLifeCaredtl[1].OTHERCLEAN;
                dr[4] = question.Data.DayLifeCaredtl[2].OTHERCLEAN;
                dr[5] = question.Data.DayLifeCaredtl[3].OTHERCLEAN;
                dr[6] = question.Data.DayLifeCaredtl[4].OTHERCLEAN;
                dt2.Rows.Add(dr);


                DataTable dt3 = new DataTable();
                dt3.Columns.Add("");
                dt3.Columns.Add("");
                dt3.Columns.Add("");
                dt3.Columns.Add("");
                dt3.Columns.Add("");
                dt3.Columns.Add("");
                dt3.Columns.Add("");
                dt3.Columns.Add("");

                dr = dt3.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = "量";
                dr[3] = question.Data.DayLifeCaredtl[0].SHITAMOUNT;
                dr[4] = question.Data.DayLifeCaredtl[1].SHITAMOUNT;
                dr[5] = question.Data.DayLifeCaredtl[2].SHITAMOUNT;
                dr[6] = question.Data.DayLifeCaredtl[3].SHITAMOUNT;
                dr[7] = question.Data.DayLifeCaredtl[4].SHITAMOUNT;
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = "颜色";
                dr[3] = question.Data.DayLifeCaredtl[0].SHITCOLOR;
                dr[4] = question.Data.DayLifeCaredtl[1].SHITCOLOR;
                dr[5] = question.Data.DayLifeCaredtl[2].SHITCOLOR;
                dr[6] = question.Data.DayLifeCaredtl[3].SHITCOLOR;
                dr[7] = question.Data.DayLifeCaredtl[4].SHITCOLOR;
                dt3.Rows.Add(dr);

                 dr = dt3.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = "性质";
                dr[3] = question.Data.DayLifeCaredtl[0].SHITNATURE;
                dr[4] = question.Data.DayLifeCaredtl[1].SHITNATURE;
                dr[5] = question.Data.DayLifeCaredtl[2].SHITNATURE;
                dr[6] = question.Data.DayLifeCaredtl[3].SHITNATURE;
                dr[7] = question.Data.DayLifeCaredtl[4].SHITNATURE;
                dt3.Rows.Add(dr);
               dr = dt3.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = null;
                dr[3] = question.Data.DayLifeCaredtl[0].URINECOLOR;
                dr[4] = question.Data.DayLifeCaredtl[1].URINECOLOR;
                dr[5] = question.Data.DayLifeCaredtl[2].URINECOLOR;
                dr[6] = question.Data.DayLifeCaredtl[3].URINECOLOR;
                dr[7] = question.Data.DayLifeCaredtl[4].URINECOLOR;
                dt3.Rows.Add(dr);
                DataTable dt4 = new DataTable();
              
                dt4.Columns.Add("");
                dt4.Columns.Add("");
                dt4.Columns.Add("");
                dt4.Columns.Add("");
                dt4.Columns.Add("");
                dt4.Columns.Add("");
                dt4.Columns.Add("");
                dr = dt4.NewRow();
                dr[0] = null;
                dr[1] = "如厠情况";
                dr[2] = question.Data.DayLifeCaredtl[0].TOILET;
                dr[3] = question.Data.DayLifeCaredtl[1].TOILET;
                dr[4] = question.Data.DayLifeCaredtl[2].TOILET;
                dr[5] = question.Data.DayLifeCaredtl[3].TOILET;
                dr[6] = question.Data.DayLifeCaredtl[4].TOILET;
                dt4.Rows.Add(dr);

                dr = dt4.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = disposearr1(question.Data.DayLifeCaredtl[0].TOILETTIME)[0] + "\n" + disposearr1(question.Data.DayLifeCaredtl[0].TOILETTIME)[1] + "\n" + disposearr1(question.Data.DayLifeCaredtl[0].TOILETTIME)[2];
                dr[3] = disposearr1(question.Data.DayLifeCaredtl[1].TOILETTIME)[0] + "\n" + disposearr1(question.Data.DayLifeCaredtl[1].TOILETTIME)[1] + "\n" + disposearr1(question.Data.DayLifeCaredtl[1].TOILETTIME)[2];
                dr[4] = disposearr1(question.Data.DayLifeCaredtl[2].TOILETTIME)[0] + "\n" + disposearr1(question.Data.DayLifeCaredtl[2].TOILETTIME)[1] + "\n" + disposearr1(question.Data.DayLifeCaredtl[2].TOILETTIME)[2];
                dr[5] = disposearr1(question.Data.DayLifeCaredtl[3].TOILETTIME)[0] + "\n" + disposearr1(question.Data.DayLifeCaredtl[3].TOILETTIME)[1] + "\n" + disposearr1(question.Data.DayLifeCaredtl[3].TOILETTIME)[2];
                dr[6] = disposearr1(question.Data.DayLifeCaredtl[4].TOILETTIME)[0] + "\n" + disposearr1(question.Data.DayLifeCaredtl[4].TOILETTIME)[1] + "\n" + disposearr1(question.Data.DayLifeCaredtl[4].TOILETTIME)[2];
                dt4.Rows.Add(dr);
                dr = dt4.NewRow();
                dr[0] = null;
                dr[1] = null;
                dr[2] = question.Data.DayLifeCaredtl[0].EQUIPMENT;
                dr[3] = question.Data.DayLifeCaredtl[1].EQUIPMENT;
                dr[4] = question.Data.DayLifeCaredtl[2].EQUIPMENT;
                dr[5] = question.Data.DayLifeCaredtl[3].EQUIPMENT;
                dr[6] = question.Data.DayLifeCaredtl[4].EQUIPMENT;
                dt4.Rows.Add(dr);


                doc.FillTable(0, dt1, "", "", 7);

                doc.FillTable(0, dt2, "", "", 8);

                doc.FillTable(0, dt3, "", "", 11);

                doc.FillTable(0, dt4, "", "", 15);

                doc.FillTable(0, dt, "", "", 1);
            }
            //根据求出  


        }

       
        private void TransdisciplinaryOperation(WordDocument doc, ReportRequest request)
        {
            IDC_TransdisciplinaryPlan service = IOCContainer.Instance.Resolve<IDC_TransdisciplinaryPlan>();
            DC_MultiteamCarePlanRecModel model = service.QueryMultiCarePlanRec(request.seqNo).Data;
            var basic = reportManageService.GetBasicInfoById(model.FEENO);
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            if (basic != null)
            {
                doc.ReplaceText("OrgName", org.Data.OrgName);
                doc.ReplaceText("RegName", basic.Data.RegName);
                if (!string.IsNullOrEmpty(basic.Data.NurseAidesName))
                {
                    doc.ReplaceText("NurseAidesName", basic.Data.NurseAidesName);
                }
                else
                {
                    doc.ReplaceText("NurseAidesName", "");
                }
             
            }
            BindData(model.PlanEval, doc);

            string imgPath = model.PlanEval.ECOLOGICALMAP;
            if (!string.IsNullOrEmpty(imgPath))
            {
                string mapPath = Server.MapPath(VirtualPathUtility.GetDirectory("~")) + imgPath.Substring(1);
                try
                {
                    doc.InsertImage("InsertImage", mapPath, 300, 200);
                }
                catch (Exception ex)
                { doc.ReplaceText("InsertImage", ""); }
            }
            else
            {
                doc.ReplaceText("InsertImage", "");
            }

            if (model.CarePlan.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("MAJORTYPE");
                dt.Columns.Add("QUESTIONTYPE");
                dt.Columns.Add("ACTIVITY");
                dt.Columns.Add("TRACEDESC");
                foreach (DC_MultiteamCarePlanModel carePlan in model.CarePlan)
                {
                    var dr = dt.NewRow();
                    dr["MAJORTYPE"] = carePlan.MAJORTYPE;
                    dr["QUESTIONTYPE"] = carePlan.QUESTIONTYPE;
                    dr["ACTIVITY"] = carePlan.ACTIVITY;
                    dr["TRACEDESC"] = carePlan.TRACEDESC;
                    dt.Rows.Add(dr);
                }
                doc.FillTable(1, dt, "", "", 1);//因调整打印模板，做对应修改
            }

        }

    }
}

