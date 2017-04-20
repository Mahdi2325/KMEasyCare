using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController.ExportReport
{
    public partial class ResidentReportController : ReportBaseController
    {
        ISocialWorkerManageService reportManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
        IOrganizationManageService orgserver = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        IResidentManageService residentService = IOCContainer.Instance.Resolve<IResidentManageService>();
        IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
        BaseRequest<ResidentFilter> ResidentFilter = new BaseRequest<ResidentFilter>();
        BaseResponse<IList<Resident>> list = new BaseResponse<IList<Resident>>();
        public ActionResult ResidentExport()
        {
            string templateName = Request["templateName"];
            string startDateStr = Request["startDate"];
            string endDateStr = Request["endDate"];
            string floorId = Request["floorId"] ?? "";
            int feeNo = 0;
            if (Request["feeNo"] != null)
            {
                feeNo = int.Parse(Request["feeNo"]);
            }
            if (floorId != "")
            {
                feeNo = 0;
            }
            ResidentFilter.Data.FeeNo = feeNo;
            ResidentFilter.Data.FloorId = floorId;
            list = residentService.QueryResidentExtend(ResidentFilter);         
            #region 新近住民环境及辅导记录
            if (templateName == "LTC_NEWEnvironmentTutor")
            {
                using (WordDocument doc = new WordDocument())
                {
                    doc.LoadModelDoc(templateName);
                    foreach (Resident r in list.Data)
                    {
                        BaseRequest BaseRequest = new BaseRequest();
                        BaseRequest<NewResideEntenvRecFilter> request = new BaseRequest<NewResideEntenvRecFilter>();
                        request.Data.FEENO = r.FeeNo;
                        var NewRegEnvAdaptation = reportManageService.QueryNewRegEnvAdaptation(request);
                        for (var i = 0; i < NewRegEnvAdaptation.Data.Count; i++)
                        {
                            //实例化一个模板
                            doc.NewPartDocument();
                            var orginfo = orgserver.GetOrg(NewRegEnvAdaptation.Data[i].ORGID);
                            var p = orgserver.GetEmployee(NewRegEnvAdaptation.Data[i].CREATEBY);
                            var response = new BaseResponse<List<object>>(new List<object>());

                            response.Data.Add
                               (new
                               {
                                   NAME = r.Name,
                                   INDATE = NewRegEnvAdaptation.Data[i].INDATE,
                                   W1EVALDATE = NewRegEnvAdaptation.Data[i].W1EVALDATE,
                                   INFORMFLAG = (NewRegEnvAdaptation.Data[i].INFORMFLAG == 1) ? "是" : "否",
                                   COMMFLAG = (NewRegEnvAdaptation.Data[i].COMMFLAG == 1) ? "是" : "否",
                                   INTERPERSONAL = NewRegEnvAdaptation.Data[i].INTERPERSONAL,
                                   PARTICIPATION = NewRegEnvAdaptation.Data[i].PARTICIPATION,
                                   COORDINATION = NewRegEnvAdaptation.Data[i].COORDINATION,
                                   EMOTION = NewRegEnvAdaptation.Data[i].EMOTION,
                                   RESISTANCE = NewRegEnvAdaptation.Data[i].RESISTANCE,
                                   HELP = NewRegEnvAdaptation.Data[i].HELP,
                                   PROCESSACTIVITY = NewRegEnvAdaptation.Data[i].PROCESSACTIVITY,
                                   TRACEREC2 = NewRegEnvAdaptation.Data[i].TRACEREC,
                                   WEEK = NewRegEnvAdaptation.Data[i].WEEK,
                                   W2EVALDATE = NewRegEnvAdaptation.Data[i].W2EVALDATE,
                                   W3EVALDATE = NewRegEnvAdaptation.Data[i].W3EVALDATE,
                                   W4EVALDATE = NewRegEnvAdaptation.Data[i].W4EVALDATE,
                                   W1EVALUATION = NewRegEnvAdaptation.Data[i].EVALUATION,
                                   CREATEBY = NewRegEnvAdaptation.Data[i].CREATEBY,
                                   ORG = orginfo.Data.OrgName,

                               });
                            MultiBindData(response.Data[0], doc);
                            //添加一个模板
                            doc.AddPartDocument();
                        }
                    }

                    //导出一个模板
                    //Util.DownloadFile(doc.SaveDoc());
                    if (!doc.IsDocNull())
                    {
                        Util.DownloadFile(doc.SaveMarkDoc(templateName));
                    }
                }
            }
            #endregion
            #region 新近住民环境介绍记录表
            else if (templateName == "LTC_NEWEnvironmentRec")
            {

                using (WordDocument doc = new WordDocument())
                {
                    doc.LoadModelDoc(templateName);
                    foreach (Resident r in list.Data)
                    {
                        BaseRequest<NewResideEntenvRecFilter> request = new BaseRequest<NewResideEntenvRecFilter>();
                        request.Data.FEENO = r.FeeNo;
                        var NewResideEntenvRec = reportManageService.QueryNewResideEntenvRec(request);
                        for (var i = 0; i < NewResideEntenvRec.Data.Count; i++)
                        {
                            //实例化一个模板
                            doc.NewPartDocument();
                            var orginfo = orgserver.GetOrg(NewResideEntenvRec.Data[i].ORGID);
                            DateTime t1 = DateTime.Now;
                            DateTime t2 = Convert.ToDateTime(NewResideEntenvRec.Data[i].BIRTHDATE);
                            var response = new BaseResponse<List<object>>(new List<object>());

                            response.Data.Add
               (new
               {
                   age = t1.Year - t2.Year,
                   ID = NewResideEntenvRec.Data[i].ID,
                   RESIDENGNO = NewResideEntenvRec.Data[i].RESIDENGNO,
                   BEDNO = NewResideEntenvRec.Data[i].BEDNO,
                   SEX = (NewResideEntenvRec.Data[i].SEX == "M") ? "男" : "女",
                   INDATE = NewResideEntenvRec.Data[i].INDATE,
                   RECORDDATE = NewResideEntenvRec.Data[i].RECORDDATE,
                   BIRTHDATE = NewResideEntenvRec.Data[i].BIRTHDATE,
                   FAMILYPARTICIPATION = (NewResideEntenvRec.Data[i].FAMILYPARTICIPATION == true) ? "是" : "否",
                   CONTRACTFLAG = ((NewResideEntenvRec.Data[i].CONTRACTFLAG == true) ? "01.契约内容说明" : "") + ((NewResideEntenvRec.Data[i].LIFEFLAG == true) ? "02.生活公约说明" : ""),

                   STAFF1 = NewResideEntenvRec.Data[i].STAFF1,
                   REGULARACTIVITY = ((NewResideEntenvRec.Data[i].REGULARACTIVITY == true) ? "01.定期活动说明" : "") + ((NewResideEntenvRec.Data[i].NOTREGULARACTIVITY == true) ? "02.不定期活动说明" : ""),

                   STAFF2 = NewResideEntenvRec.Data[i].STAFF2,
                   name = r.Name,
                   BELLFLAG = ((NewResideEntenvRec.Data[i].BELLFLAG == true) ? "01.紧急铃" : "") +
                              " " + ((NewResideEntenvRec.Data[i].LAMPFLAG == true) ? "02.床头灯" : "") +
                              " " + ((NewResideEntenvRec.Data[i].TVFLAG == true) ? "03.电视" : "") +
                              " " + ((NewResideEntenvRec.Data[i].LIGHTSWITCH == true) ? "04.电灯开关" : "") +
                              " " + ((NewResideEntenvRec.Data[i].ESCAPEDEVICE == true) ? "05.逃生设备" : "") +
                              " " + ((NewResideEntenvRec.Data[i].ENVIRONMENT == true) ? "06.公共环境" : "") +
                              " " + ((NewResideEntenvRec.Data[i].COMMUNITYFACILITIES == true) ? "07.社区设施" : "") +
                              " " + ((NewResideEntenvRec.Data[i].POSTOFFICE == true) ? "邮局" : "") +
                              " " + ((NewResideEntenvRec.Data[i].SCHOOL == true) ? "学校" : "") +
                              " " + ((NewResideEntenvRec.Data[i].BANK == true) ? "银行" : "") +
                              " " + ((NewResideEntenvRec.Data[i].STATION == true) ? "车站" : "") +
                              " " + ((NewResideEntenvRec.Data[i].PARK == true) ? "公园" : "") +
                              " " + ((NewResideEntenvRec.Data[i].TEMPLE == true) ? "寺庙" : "") +
                              " " + ((NewResideEntenvRec.Data[i].HOSPITAL == true) ? "医疗院所" : "") +
                              " " + ((NewResideEntenvRec.Data[i].OTHERFACILITIES == true) ? "其他" : ""),

                   CLEANLINESS = ((NewResideEntenvRec.Data[i].CLEANLINESS == true) ? "01.个人清洁" : "") +
                              " " + ((NewResideEntenvRec.Data[i].MEDICALCARE == true) ? "02.医保保健" : "") +
                              " " + ((NewResideEntenvRec.Data[i].MEALSERVICE == true) ? "03.膳食服务" : "") +
                              " " + ((NewResideEntenvRec.Data[i].WORKACTIVITIES == true) ? "04.社工拟定活动" : ""),

                   STAFF3 = NewResideEntenvRec.Data[i].STAFF3,
                   STAFF4 = NewResideEntenvRec.Data[i].STAFF4,


                   PERSONINCHARGE = ((NewResideEntenvRec.Data[i].PERSONINCHARGE == true) ? "01.负责人 " : "") +
                              " " + ((NewResideEntenvRec.Data[i].DIRECTOR == true) ? "02.主任" : "") +
                              " " + ((NewResideEntenvRec.Data[i].NURSE == true) ? "03.护士" : "") +
                              " " + ((NewResideEntenvRec.Data[i].NURSEAIDES == true) ? "04.照顾服务员" : "") +
                              " " + ((NewResideEntenvRec.Data[i].RESIDENT == true) ? "05.住民" : "") +
                              " " + ((NewResideEntenvRec.Data[i].DOCTOR == true) ? "06.医师" : "") +
                              " " + ((NewResideEntenvRec.Data[i].SOCIALWORKER == true) ? "07.社工" : "") +
                              " " + ((NewResideEntenvRec.Data[i].DIETITIAN == true) ? "08.营养师" : "") +
                              " " + ((NewResideEntenvRec.Data[i].OTHERPEOPLE == true) ? "09.其他人员" : ""),

                   STAFF5 = NewResideEntenvRec.Data[i].STAFF5,
                   RECORDBY = NewResideEntenvRec.Data[i].RECORDBY,
                   FEENO = NewResideEntenvRec.Data[i].FEENO,
                   REGNO = NewResideEntenvRec.Data[i].REGNO,
                   org = orginfo.Data.OrgName

               });
                            MultiBindData(response.Data[0], doc);
                            //添加一个模板
                            doc.AddPartDocument();
                        }
                    }

                    //导出一个模板
                    //Util.DownloadFile(doc.SaveDoc());
                    if (!doc.IsDocNull())
                    {
                        Util.DownloadFile(doc.SaveMarkDoc(templateName));
                    }
                }

            }
            #endregion
            #region 个案基本资料
            else if (templateName == "PA001")
            {
                using (WordDocument doc = new WordDocument())
                {
                    doc.LoadModelDoc(templateName);
                    foreach (Resident r in list.Data)
                    {
                        doc.NewPartDocument();
                        BaseRequest BaseRequest = new BaseRequest();
                        BaseRequest<NewResideEntenvRecFilter> request = new BaseRequest<NewResideEntenvRecFilter>();
                        request.Data.REGNO = r.RegNo;
                        var Person = residentService.GetPersonExtend(r.RegNo.Value);
                        if (Person.Data.ImgUrl != null)
                        {
                            string mapPath = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, Person.Data.ImgUrl.Replace("/", @"\"));
                            if (System.IO.File.Exists(mapPath))
                            {
                                doc.InsertImage("photo", mapPath, 150, 200);
                            }

                        }
                        CodeFilter codeFilter = new CodeFilter();
                        codeFilter.ItemTypes = new string[] { "A00.030", "A00.032", "A00.035", "A00.001", "A00.007", "A00.008", "A00.011" };
                        var dict = (List<CodeValue>)dictManageService.QueryCode(codeFilter).Data;
                        CodeValue findItem;
                        Person.Data.Sex = dict.Find(it => it.ItemType == "A00.001" && it.ItemCode == Person.Data.Sex) != null ?
                            dict.Find(it => it.ItemType == "A00.001" && it.ItemCode == Person.Data.Sex).ItemName : "";
                        Person.Data.Education = dict.Find(it => it.ItemType == "A00.007" && it.ItemCode == Person.Data.Education) != null ?
                           dict.Find(it => it.ItemType == "A00.007" && it.ItemCode == Person.Data.Education).ItemName : "";
                        Person.Data.ReligionCode = dict.Find(it => it.ItemType == "A00.008" && it.ItemCode == Person.Data.ReligionCode) != null ?
                           dict.Find(it => it.ItemType == "A00.008" && it.ItemCode == Person.Data.ReligionCode).ItemName : "";
                        Person.Data.MerryFlag = dict.Find(it => it.ItemType == "A00.011" && it.ItemCode == Person.Data.MerryFlag) != null ?
                           dict.Find(it => it.ItemType == "A00.011" && it.ItemCode == Person.Data.MerryFlag).ItemName : "";
                        MultiBindData(Person.Data, doc);
                        DataTable dt = new DataTable();
                        dt.Columns.Add("c1");
                        dt.Columns.Add("c2");
                        dt.Columns.Add("c3");
                        dt.Columns.Add("c4");
                        dt.Columns.Add("c5");
                        dt.Columns.Add("c6");
                        if (Person.Data.RelationDtl != null)
                        {
                            foreach (var item in Person.Data.RelationDtl)
                            {
                                var dr = dt.NewRow();
                                dr["c1"] = item.Name;
                                findItem = dict.Find(it => it.ItemType == "A00.030" && it.ItemCode == item.Contrel);
                                dr["c2"] = findItem != null ? findItem.ItemName : "";
                                findItem = dict.Find(it => it.ItemType == "A00.032" && it.ItemCode == item.RelationType);
                                dr["c3"] = findItem != null ? findItem.ItemName : "";
                                dr["c4"] = item.Phone;
                                dr["c5"] = item.Address;
                                findItem = dict.Find(it => it.ItemType == "A00.035" && it.ItemCode == item.WorkCode);
                                dr["c6"] = findItem != null ? findItem.ItemName : "";
                                dt.Rows.Add(dr);
                            }
                        }
                        doc.MultiFillTable(0, dt, "", "", 32);
                        doc.AddPartDocument();
                        dt.Dispose();
                    }
                    //导出一个模板
                    Util.DownloadFile(doc.SaveDoc());
                }
            }
            #endregion
            return View("ResidentExport");
        }

    }
}

