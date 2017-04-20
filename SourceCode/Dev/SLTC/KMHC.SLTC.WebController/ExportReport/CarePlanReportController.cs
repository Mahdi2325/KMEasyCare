using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController.ExportReport
{
    public partial class EvalReportController : ReportBaseController
    {
        ICarePlanReportService carePlanservice = IOCContainer.Instance.Resolve<ICarePlanReportService>();

        public ActionResult CarePlanExport()
        {
            string templateName = Request["templateName"];
            string startDateStr = Request["startDate"];
            string endDateStr = Request["endDate"];
            string feeNo = Request["feeNo"];
            string classType = Request["classType"];
            string floorId = Request["floorId"];
            string printOrder = Request["printOrder"];

            switch (templateName)
            {
                //护理记录
                case "P18Report":
                    DowdloadP18(templateName, startDateStr, endDateStr, feeNo, classType, floorId, printOrder);
                    break;
                //护理需求评估

                case "H10Report":
                    DowdloadH10(templateName, startDateStr, endDateStr, feeNo, floorId);
                    break;
                // 护理需求评估
                case "H35Report":
                    DowdloadH35(templateName, startDateStr, endDateStr, feeNo, floorId);
                    break;
                //个别化活动需求评估及计划
                case "DLN1.1":
                    DowdloadDLN1(templateName, startDateStr, endDateStr, feeNo, floorId);
                    break;
            }
            return View("Export");
        }


        #region 护理记录
        /// <summary>
        /// 护理记录
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="startDateStr"></param>
        /// <param name="endDateStr"></param>
        /// <param name="feeNo"></param>
        /// <param name="classType"></param>
        public void DowdloadP18(string templateName, string startDateStr, string endDateStr, string feeNo, string classType, string floorId, string printOrder)
        {

            using (WordDocument doc = new WordDocument())
            {
                //加载模板
                doc.LoadModelDoc(templateName);
                var carelist = GetCareP18(startDateStr, endDateStr, feeNo, classType, floorId);
                if (carelist != null && carelist.Count > 0)
                {
                    List<string> list = new List<string>();
                    foreach (var item in carelist)
                    {
                        list.Add(item.FEENO.ToString());
                    }

                    var feenolist = list.Distinct().ToList();

                    if (feenolist != null && feenolist.Count > 0)
                    {
                        foreach (var item in feenolist)
                        {

                            doc.NewPartDocument();
                            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
                            INursingWorkstationService nursingWorkstationService = IOCContainer.Instance.Resolve<INursingWorkstationService>();
                            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
                            var residentInfo = carePlanservice.GetExportResidentInfo(Convert.ToInt64(item));
                            if (residentInfo != null)
                            {
                                doc.ReplacePartText("Org", residentInfo.Org ?? "");
                                doc.ReplacePartText("Name", residentInfo.Name ?? "");
                                doc.ReplacePartText("Sex", residentInfo.Sex ?? "未填");
                                doc.ReplacePartText("Age", residentInfo.Age.HasValue ? residentInfo.Age.ToString() : "");
                                doc.ReplacePartText("FeeNo", residentInfo.ResidentNo);
                                doc.ReplacePartText("Floor", residentInfo.Floor ?? "");
                                doc.ReplacePartText("RoomNo", residentInfo.RoomNo ?? "");
                            }
                            else
                            {
                                doc.ReplacePartText("Org", "");
                                doc.ReplacePartText("Name", "");
                                doc.ReplacePartText("Sex", "未填");
                                doc.ReplacePartText("Age", "");
                                doc.ReplacePartText("FeeNo", "");
                                doc.ReplacePartText("Floor", "");
                                doc.ReplacePartText("RoomNo", "");
                            }

                            BaseRequest<NursingRecFilter> nursingRecFilter = new BaseRequest<NursingRecFilter>();
                            nursingRecFilter.CurrentPage = 1;
                            nursingRecFilter.PageSize = 1000;
                            nursingRecFilter.Data.PrintFlag = true;
                            nursingRecFilter.Data.FeeNo = (Convert.ToInt64(item));
                            nursingRecFilter.Data.Order = printOrder;
                            var response = nursingWorkstationService.QueryNursingRec(nursingRecFilter);
                            response.Data = response.Data.Where(o => o.ClassType == classType).ToList();
                            DateTime? startDate = null;
                            DateTime? endDate = null;
                            if (startDateStr != "") startDate = Convert.ToDateTime(startDateStr);
                            if (endDateStr != "") endDate = Convert.ToDateTime(endDateStr).AddDays(1).AddMilliseconds(-1);

                            if (startDate.HasValue && response.Data!= null)
                            {
                                response.Data = response.Data.Where(m => m.RecordDate >= startDate.Value).ToList();
                            }
                            if (endDate.HasValue && response.Data!= null)
                            {
                                response.Data = response.Data.Where(m => m.RecordDate <= endDate.Value).ToList();
                            }
                            CodeFilter codeFilter = new CodeFilter { ItemTypes = new string[] { "J00.005" } };
                            var dict = (List<CodeValue>)dictManageService.QueryCode(codeFilter).Data;

                            DataTable dt = new DataTable();
                            dt.Columns.Add("c1");
                            dt.Columns.Add("c2");
                            dt.Columns.Add("c3");
                            dt.Columns.Add("c4");
                            if (response.Data != null)
                            {
                                foreach (var careitem in response.Data)
                                {
                                    var dr = dt.NewRow();
                                    var findItem = dict.Find(it => it.ItemType == "J00.005" && it.ItemCode == careitem.ClassType);
                                    dr["c1"] = careitem.RecordDate.HasValue ?Convert.ToDateTime(careitem.RecordDate.Value).ToString("yyyy-MM-dd") : "";
                                    dr["c2"] = string.Format("{0} {1}{2}", careitem.RecordDate.HasValue ? careitem.RecordDate.Value.ToString("HH:mm") : "", careitem.ClassType, findItem != null ? findItem.ItemName : "");
                                    dr["c3"] = careitem.Content;
                                    dr["c4"] = careitem.RecordNameBy;
                                    dt.Rows.Add(dr);
                                }
                            }

                            doc.MultiFillTable(0, dt, "", "", 1);
                            doc.AddPartDocument();
                            dt.Dispose();
                        }

                        if (!doc.IsDocNull())
                        {
                            Util.DownloadFile(doc.SaveMarkDoc("护理记录"));
                        }
                    }
                }
            }
            
        }
        #endregion

        #region 护理计画
        public void DowdloadH10(string templateName, string startDateStr, string endDateStr, string feeNo, string floorId)
        {

            using (WordDocument doc = new WordDocument())
            {
                //加载模板
                doc.LoadModelDoc(templateName);
                var carelist = GetCareH10(startDateStr, endDateStr, feeNo, floorId);
                if (carelist != null && carelist.Count > 0)
                {
                    foreach (var item in carelist)
                    {
                        doc.NewPartDocument();

                        IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
                        var nscpl = reportManageService.GetNSCPLReportView( Convert.ToInt32(item.ID));
                        doc.BindReportData(nscpl);
                        doc.AddPartDocument();
                    }
                }
                if (!doc.IsDocNull())
                {
                    Util.DownloadFile(doc.SaveMarkDoc("护理计画"));
                }
            }
        }

        #endregion

        #region 护理需求评估
        public void DowdloadH35(string templateName, string startDateStr, string endDateStr, string feeNo, string floorId)
        {
            using (WordDocument doc = new WordDocument())
            {
                //加载模板
                doc.LoadModelDoc(templateName);
                var carelist = GetCareH35(startDateStr, endDateStr, feeNo, floorId);
                if (carelist != null && carelist.Count > 0)
                {
                    foreach (var item in carelist)
                    {
                        doc.NewPartDocument();
                        IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
                        var caredemandevalprivew = reportManageService.GetCareDemandHis(Convert.ToInt32(item.ID), SecurityHelper.CurrentPrincipal.OrgId);
                        doc.BindReportData(caredemandevalprivew);
                        doc.AddPartDocument();
                    }
                }
                if (!doc.IsDocNull())
                {
                    Util.DownloadFile(doc.SaveDoc("护理需求评估"));
                }
            }

        }
        #endregion

        #region 个别化活动需求评估及计划

        public void DowdloadDLN1(string templateName, string startDateStr, string endDateStr, string feeNo, string floorId)
        {
           
            using (WordDocument doc = new WordDocument())
            {
                //加载模板
                doc.LoadModelDoc(templateName);

                var carelist = GetCarePlanList(startDateStr, endDateStr, feeNo, floorId);
                if (carelist != null && carelist.Count > 0)
                { 
                  foreach(var item in carelist)
                  {

                      doc.NewPartDocument();

                      ISocialWorkerManageService reportManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
                      var bd = reportManageService.GetActivityRequEval(item.Id).Data;
                      var directionsense = GetNameByCodeL00012(bd.Directionsense);
                      directionsense += "（ ";
                      if (bd.Directman == 1)
                      {
                          directionsense += "人 ";
                      }
                      if (bd.Directtime == 1)
                      {
                          directionsense += "时 ";
                      }
                      if (bd.Directaddress == 1)
                      {
                          directionsense += "地 ";
                      }
                      directionsense += "）";
                      bd.Directionsense = directionsense;

                      var memory = GetNameByCodeL00013(bd.Memory);
                      memory += "( ";
                      if (bd.Memoryflag == 1)
                      {
                          memory += "长期";
                      }
                      if (bd.Memoryflag == -1)
                      {
                          memory += "短期";
                      }
                      memory += "）";
                      bd.Memory = memory;

                      if (bd.Talkedwilling != null)
                      {
                          if (bd.Talkedwilling.Trim().ToUpper() == "TRUE")
                          {
                              bd.Talkedwilling = "有意愿及选择参与之活动内容";
                          }
                      }
                      if (bd.Talkednowilling != null)
                      {
                          if (bd.Talkednowilling.Trim().ToUpper() == "TRUE")
                          {
                              bd.Talkedwilling = "无意愿参与活动将安排资源介入或个别辅导";
                          }
                      }
                      if (bd.Nottalked != null)
                      {
                          if (bd.Nottalked.Trim().ToUpper() == "TRUE")
                          {
                              bd.Nottalked = "因生理/心理/认知等障碍由社工预计参与活动";
                          }
                          else
                          {
                              bd.Nottalked = "";
                          }
                      }

                      bd.Vision = GetNameByCodeL00010(bd.Vision);
                      bd.Smell = GetNameByCodeL00010(bd.Smell);
                      bd.Sensation = GetNameByCodeL00010(bd.Sensation);
                      bd.Taste = GetNameByCodeL00010(bd.Taste);
                      bd.Hearing = GetNameByCodeL00010(bd.Hearing);
                      bd.Upperlimb = GetNameByCodeL00010(bd.Upperlimb);
                      bd.Lowerlimb = GetNameByCodeL00010(bd.Lowerlimb);
                      bd.Attention = GetNameByCodeL00011(bd.Attention);

                      bd.Comprehension = GetNameByCodeL00013(bd.Comprehension);

                      bd.Expression = GetNameByCodeL00014(bd.Expression);
                      bd.Artactivity = GetWrapData(bd.Artactivity);
                      bd.Aidsactivity = GetWrapData(bd.Aidsactivity);
                      bd.Severeactivity = GetWrapData(bd.Severeactivity);
                      bd.CarerName = item.CarerName;
                      bd.FeeName = item.FeeName;

                      ActivityRequEval ar = new ActivityRequEval();

                      ar.InDate = bd.InDate.ToString();
                      ar.EvalDate = bd.EvalDate.ToString();
                      ar.FeeName = bd.FeeName;
                      ar.Carer = bd.CarerName;
                      ar.Vision = bd.Vision;
                      ar.Attention = bd.Attention;
                      ar.Smell = bd.Smell;
                      ar.Directionsense = bd.Directionsense;
                      ar.Sensation = bd.Sensation;
                      ar.Comprehension = bd.Comprehension;
                      ar.Taste = bd.Taste;
                      ar.Memory = bd.Memory;
                      ar.Hearing = bd.Hearing;
                      ar.Expression = bd.Expression;
                      ar.Upperlimb = bd.Upperlimb;
                      ar.Othernarrative = bd.Othernarrative;
                      ar.Lowerlimb = bd.Lowerlimb;
                      ar.Hallucination = bd.Hallucination;
                      ar.Delusion = bd.Delusion;
                      ar.Emotion = bd.Emotion;
                      ar.Self = bd.Self;
                      ar.Behaviorcontent = bd.Behaviorcontent;
                      ar.Behaviorfreq = bd.Behaviorfreq;
                      ar.Activity = bd.Activity;
                      ar.Talkedwilling = bd.Talkedwilling;
                      ar.Nottalked = bd.Nottalked;
                      ar.Artactivity = bd.Artactivity;
                      ar.Aidsactivity = bd.Aidsactivity;
                      ar.Severeactivity = bd.Severeactivity;

                      doc.BindReportData(ar);
                      doc.AddPartDocument();
                  }
                }
                if (!doc.IsDocNull())
                {
                    Util.DownloadFile(doc.SaveDoc("个别化活动需求评估及计画"));
                }
            }
        }
        #endregion

        public List<RegActivityRequEval> GetCarePlanList(string _startDate, string _endDate, string _feeNo, string floorId)
        {
            var feeNo = Convert.ToInt32(string.IsNullOrEmpty(_feeNo) ? "0" : _feeNo);
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (_startDate != "") startDate = Convert.ToDateTime(_startDate);
            if (_endDate != "") startDate = Convert.ToDateTime(endDate).AddDays(1).AddMilliseconds(-1);
            return carePlanservice.GetCarePlanData(feeNo, startDate, endDate, floorId);
        }

        public List<ReportInfo> GetCareH35(string _startDate, string _endDate, string _feeNo ,string floorId)
        {
            var feeNo = Convert.ToInt32(string.IsNullOrEmpty(_feeNo) ? "0" : _feeNo);
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (_startDate != "") startDate = Convert.ToDateTime(_startDate);
            if (_endDate != "") endDate = Convert.ToDateTime(_endDate).AddDays(1).AddMilliseconds(-1);
            return carePlanservice.GetCareH35(feeNo, startDate, endDate, floorId);
        }

        public List<ReportInfo> GetCareH10(string _startDate, string _endDate, string _feeNo, string floorId)
        {
            var feeNo = Convert.ToInt32(string.IsNullOrEmpty(_feeNo) ? "0" : _feeNo);
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (_startDate != "") startDate = Convert.ToDateTime(_startDate);
            if (_endDate != "") endDate = Convert.ToDateTime(_endDate).AddDays(1).AddMilliseconds(-1);
            return carePlanservice.GetCareH10(feeNo, startDate, endDate, floorId);
        }

        public List<ReportInfo> GetCareP18(string _startDate, string _endDate, string _feeNo, string classtype, string floorId)
        {
            var feeNo = Convert.ToInt32(string.IsNullOrEmpty(_feeNo) ? "0" : _feeNo);
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (_startDate != "") startDate = Convert.ToDateTime(_startDate);
            if (_endDate != "") endDate = Convert.ToDateTime(_endDate).AddDays(1).AddMilliseconds(-1);
            return carePlanservice.GetCareP18(feeNo, startDate, endDate, classtype, floorId);
        } 

        #region zidian 

        private string GetWrapData(string data)
        {
            if (data != null)
            {
                return data.Replace("，", "\r");
            }
            else
            {
                return "";
            }

        }

        private string GetNameByCodeL00014(string Code)
        {
            var result = "";
            switch (Code)
            {
                case "001":
                    result = "可以各种方式表达";
                    break;
                case "002":
                    result = "无法完整表达";
                    break;
                default:
                    result = "";
                    break;
            };
            return result;
        }
        private string GetNameByCodeL00011(string Code)
        {
            var result = "";
            switch (Code)
            {
                case "001":
                    result = "集中";
                    break;
                case "002":
                    result = "易分散";
                    break;
                case "003":
                    result = "不清";
                    break;
                default:
                    result = "";
                    break;
            };
            return result;
        }

        private string GetNameByCodeL00010(string Code)
        {
            var result = "";
            switch (Code)
            {
                case "001":
                    result = "正常";
                    break;
                case "002":
                    result = "有障碍";
                    break;
                default:
                    result = "";
                    break;
            };
            return result;
        }

        private string GetNameByCodeL00013(string Code)
        {
            var result = "";
            switch (Code)
            {
                case "001":
                    result = "正常";
                    break;
                case "002":
                    result = "缺损";
                    break;
                default:
                    result = "";
                    break;
            };
            return result;
        }

        private string GetNameByCodeL00012(string Code)
        {
            var result = "";
            switch (Code)
            {
                case "001":
                    result = "正常";
                    break;
                case "002":
                    result = "障碍";
                    break;
                default:
                    result = "";
                    break;
            };
            return result;
        }

        #endregion
    }
}

