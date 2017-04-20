using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController
{
    public partial class DC_ReportController: ReportBaseController
    {
        IDC_TransdisciplinaryPlan tms = IOCContainer.Instance.Resolve<IDC_TransdisciplinaryPlan>();

        //public ActionResult Timeline(long feeNo,string name, string startDate,string endDate,int tag)
        //{
        //    BaseResponse<CasesTimeline> result = tms.QueryCasesTimeline(feeNo, startDate, endDate, tag);
        //    timelineContainer t = new timelineContainer();
        //    timelineContext timeline = new timelineContext();

        //    CasesTimeline data = result.Data;
        //    if (data!=null)
        //    {
        //        timeline.headline = name;
        //        timeline.type = "default";
        //        timeline.startDate = Util.ToTimelineDate(data.StartDate);
        //        timeline.asset = new asset() { media = "/Scripts/timeline/assets/img/time_icon.png", credit = "", caption="" };
        //        string homePageText="照顾计划";
        //        if (tag==(int)MajorType.社工照顾计划)
        //        {
        //            homePageText = "社工照顾计划";
        //        }
        //        else if (tag == (int)MajorType.护理照顾计划)
        //        {
        //            homePageText = "护理照顾计划";
        //        }
        //        timeline.text = string.Format(Constants.TimelineText, homePageText);

        //        timeline.date = new List<timelineData>();

        //        foreach(TimelineContainer context in data.TimelineContainer)
        //        {
                  
        //            foreach(Timeline text in context.Timeline)
        //            {
        //                timelineData td = new timelineData();
        //                td.startDate = Util.ToTimelineDate(text.StartDate);
        //                td.headline =context.MajorType;
        //                td.text = GetTimelineText(text, context.MajorType);

        //                timeline.date.Add(td);
        //            }
                   
        //        }
        //        t.timeline = timeline;
        //        string fileName= TextHelper.WriteKey(JsonConvert.SerializeObject(t));
        //        ViewBag.Url = fileName;
        //    }

        //    return View();
        //}

        public ActionResult TransdisciplinaryTimeline()
        {
            return View();
        }

        public JsonResult Timeline(long feeNo, string name, string startDate, string endDate, int tag)
        {
            BaseResponse<CasesTimeline> result = tms.QueryCasesTimeline(feeNo, startDate, endDate, tag);

            return Json(new { result = true, data = result.Data }, JsonRequestBehavior.AllowGet);
        }

     
         
        

        public static string GetTimelineText(Timeline text,string majorType)
        {
             StringBuilder sb = new StringBuilder();
          
                sb.Append(string.Format(Constants.TimelineTextHeaderS, "诊断问题及护理措施"));
                if (majorType == MajorType.护理照顾计划.ToString())
                {
                sb.Append(string.Format(Constants.TimelineTextHeaderC,text.IADLSCORE,text.MMSESCORE, text.ADLSCORE,text.GODSSCORE));
                }
                sb.Append(Constants.TimelineTextHeaderE);
                foreach (TimelineText plan in text.Plan)
                {
                    sb.Append(Constants.TimelineContentHeaderS);
                    sb.Append(plan.QUESTIONTYPE);
                    sb.Append(Constants.TimelineContentHeaderC);
                    if (plan.ACTIVITY != null && plan.ACTIVITY.Count>0)
                    {
                        foreach (string lable in plan.ACTIVITY)
                        {
                            sb.Append(string.Format(Constants.TimelineContentLable, lable));
                        }
                    }
                    sb.Append(Constants.TimelineContentHeaderE);
                }
         
            return sb.ToString();
        }
    }
}

