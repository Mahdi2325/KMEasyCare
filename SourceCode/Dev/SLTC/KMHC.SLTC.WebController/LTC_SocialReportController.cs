using KM.Common;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.DC.Report;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Data;
namespace KMHC.SLTC.WebController
{
    public partial class DC_ReportController : ReportBaseController
    {
        IOrganizationManageService orgserver = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        public ActionResult PreviewLTC_SocialReport()
        {
            //新进住民环境介绍记录表  LTC_NEWEnvironmentRec
            //新进住民环境适应及辅导记录  LTC_NEWEnvironmentTutor
            string templateName = Request["templateName"];
            string feeNo = Request["feeNo"];
            string feeName = Request["feeName"];
            string id = Request["id"];
            ReportRequest request = new ReportRequest();

            if (templateName != null)
            {
                switch (templateName)
                {
                    case "DLN1.1":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.feeName = feeName;
                            this.GeneratePDF("DLN1.1", this.ActivityReport, request);
                        }
                        break;
                    case "DLN1.2":
                        if (!string.IsNullOrEmpty(feeNo))
                        {
                            request.feeNo = long.Parse(feeNo);
                            this.GeneratePDF("DLN1.2", this.NutritionCareReport, request);
                        }
                        break;
                    case "LTC_NEWEnvironmentRec":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.feeName = feeName;
                            this.GeneratePDF("LTC_NEWEnvironmentRec", this.LtcNewResideEntenvRecOperation, request);
                        }
                        break;
                    case "LTC_NEWEnvironmentTutor":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.feeName = feeName;
                            this.GeneratePDF("LTC_NEWEnvironmentTutor", this.NewRegEnvAdaptation, request);
                        }
                        break;
                }
            }
            return View("Preview");
        }

        //新进住民环境适应及辅导记录 zhongyh
        private void NewRegEnvAdaptation(WordDocument doc, ReportRequest request)
        {
            ISocialWorkerManageService reportManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            var bd = reportManageService.GetNewRegEnvAdaptationById(request.id);

            var orginfo = orgserver.GetOrg(bd.Data.ORGID);
            var p = orgserver.GetEmployee(bd.Data.CREATEBY);
            var response = new BaseResponse<List<object>>(new List<object>());

            response.Data.Add
               (new
               {
                   NAME = request.feeName,
                   INDATE = bd.Data.INDATE,
                   W1EVALDATE = bd.Data.W1EVALDATE,
                   INFORMFLAG = (bd.Data.INFORMFLAG == 1) ? "是" : "否",
                   COMMFLAG = (bd.Data.COMMFLAG == 1) ? "是" : "否",
                   INTERPERSONAL = bd.Data.INTERPERSONAL,
                   PARTICIPATION = bd.Data.PARTICIPATION,
                   COORDINATION = bd.Data.COORDINATION,
                   EMOTION = bd.Data.EMOTION,
                   RESISTANCE = bd.Data.RESISTANCE,
                   HELP = bd.Data.HELP,
                   PROCESSACTIVITY = bd.Data.PROCESSACTIVITY,
                   TRACEREC2 = bd.Data.TRACEREC,
                   WEEK=bd.Data.WEEK,
                   W2EVALDATE = bd.Data.W2EVALDATE,
                   W3EVALDATE = bd.Data.W3EVALDATE,
                   W4EVALDATE = bd.Data.W4EVALDATE,
                   W1EVALUATION = bd.Data.EVALUATION,
                   CREATEBY = p.Data!=null?p.Data.EmpName:"",
                   ORG = orginfo.Data.OrgName,

               });
            BindData(response.Data[0], doc);
        }
        //新进住民环境介绍记录表 zhongyh
        private void LtcNewResideEntenvRecOperation(WordDocument doc, ReportRequest request)
        {
            ISocialWorkerManageService reportManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            var bd = reportManageService.GetNewResideEntenvRecById(request.id);

            var orginfo = orgserver.GetOrg(bd.Data.ORGID);

            var response = new BaseResponse<List<object>>(new List<object>());
            DateTime t1 = DateTime.Now;
            DateTime t2 = Convert.ToDateTime(bd.Data.BIRTHDATE);


            response.Data.Add
               (new
               {
                   age = t1.Year - t2.Year,
                   ID = bd.Data.ID,
                   RESIDENGNO = bd.Data.RESIDENGNO,
                   BEDNO = bd.Data.BEDNO,
                   SEX = (bd.Data.SEX == "M") ? "男" : "女",
                   INDATE = bd.Data.INDATE,
                   RECORDDATE = bd.Data.RECORDDATE,
                   BIRTHDATE = bd.Data.BIRTHDATE,
                   FAMILYPARTICIPATION = (bd.Data.FAMILYPARTICIPATION == true) ? "是" : "否",
                   CONTRACTFLAG = ((bd.Data.CONTRACTFLAG == true) ? "01.契约内容说明" : "") + ((bd.Data.LIFEFLAG == true) ? "02.生活公约说明" : ""),

                   STAFF1 = bd.Data.STAFF1,
                   REGULARACTIVITY = ((bd.Data.REGULARACTIVITY == true) ? "01.定期活动说明" : "") + ((bd.Data.NOTREGULARACTIVITY == true) ? "02.不定期活动说明" : ""),

                   STAFF2 = bd.Data.STAFF2,
                   name = request.feeName,
                   BELLFLAG = ((bd.Data.BELLFLAG == true) ? "01.紧急铃" : "") +
                              " " + ((bd.Data.LAMPFLAG == true) ? "02.床头灯" : "") +
                              " " + ((bd.Data.TVFLAG == true) ? "03.电视" : "") +
                              " " + ((bd.Data.LIGHTSWITCH == true) ? "04.电灯开关" : "") +
                              " " + ((bd.Data.ESCAPEDEVICE == true) ? "05.逃生设备" : "") +
                              " " + ((bd.Data.ENVIRONMENT == true) ? "06.公共环境" : "") +
                              " " + ((bd.Data.COMMUNITYFACILITIES == true) ? "07.社区设施" : "") +
                              " " + ((bd.Data.POSTOFFICE == true) ? "邮局" : "") +
                              " " + ((bd.Data.SCHOOL == true) ? "学校" : "") +
                              " " + ((bd.Data.BANK == true) ? "银行" : "") +
                              " " + ((bd.Data.STATION == true) ? "车站" : "") +
                              " " + ((bd.Data.PARK == true) ? "公园" : "") +
                              " " + ((bd.Data.TEMPLE == true) ? "寺庙" : "") +
                              " " + ((bd.Data.HOSPITAL == true) ? "医疗院所" : "") +
                              " " + ((bd.Data.OTHERFACILITIES == true) ? "其他" : ""),

                   CLEANLINESS = ((bd.Data.CLEANLINESS == true) ? "01.个人清洁" : "") +
                              " " + ((bd.Data.MEDICALCARE == true) ? "02.医保保健" : "") +
                              " " + ((bd.Data.MEALSERVICE == true) ? "03.膳食服务" : "") +
                              " " + ((bd.Data.WORKACTIVITIES == true) ? "04.社工拟定活动" : ""),

                   STAFF3 = bd.Data.STAFF3,
                   STAFF4 = bd.Data.STAFF4,


                   PERSONINCHARGE = ((bd.Data.PERSONINCHARGE == true) ? "01.负责人 " : "") +
                              " " + ((bd.Data.DIRECTOR == true) ? "02.主任" : "") +
                              " " + ((bd.Data.NURSE == true) ? "03.护士" : "") +
                              " " + ((bd.Data.NURSEAIDES == true) ? "04.照顾服务员" : "") +
                              " " + ((bd.Data.RESIDENT == true) ? "05.住民" : "") +
                              " " + ((bd.Data.DOCTOR == true) ? "06.医师" : "") +
                              " " + ((bd.Data.SOCIALWORKER == true) ? "07.社工" : "") +
                              " " + ((bd.Data.DIETITIAN == true) ? "08.营养师" : "") +
                              " " + ((bd.Data.OTHERPEOPLE == true) ? "09.其他人员" : ""),

                   STAFF5 = bd.Data.STAFF5,
                   RECORDBY = bd.Data.RECORDBY,
                   FEENO = bd.Data.FEENO,
                   REGNO = bd.Data.REGNO,
                   org = orginfo.Data.OrgName

               });


            BindData(response.Data[0], doc);
        }
        private void ActivityReport(WordDocument doc, ReportRequest request)
        {
            ISocialWorkerManageService reportManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            var bd = reportManageService.GetActivityRequEval(request.id).Data;
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
            bd.FeeName = request.feeName;






            BindData(bd, doc);
        }

        /// <summary>
        /// DLN1.2
        /// </summary>
        /// <param name="doc">doc文件</param>
        /// <param name="request">请求参数</param>
        private void NutritionCareReport(WordDocument doc, ReportRequest request)
        {
            INursingManageService service = IOCContainer.Instance.Resolve<INursingManageService>();
           
            IList<NutrtioncateRec> ncList = service.QueryNutrtioncateRec(new BaseRequest<NutrtioncateRecFilter> { Data = new NutrtioncateRecFilter { FeeNo = request.feeNo } }).Data;

            if (ncList != null && ncList.Count > 0)
            {
                DataTable dt = new DataTable();
                var colCount = ncList.Count;
                for (var i = 0; i < colCount; i++)
                {
                    dt.Columns.Add((i + 1).ToString());

                }
                var dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].Recorddate ;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].DietPattern;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = GetCodeDtlByCodeType(ncList[j].NutrtionPathway, "L00.016");
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = GetCodeDtlByCodeType(ncList[j].DietWay, "L00.017");
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    var dinnerFreqP1 = "";
                    var dinnerFreqP2 = "";
                    var dinnerFreqP3= "";
                    if (ncList[j].DinnerFreq.ToString().Split(',').Count()==3)
                    {
                        dinnerFreqP1 = ncList[j].DinnerFreq.ToString().Split(',')[0];
                        dinnerFreqP2 = ncList[j].DinnerFreq.ToString().Split(',')[1];
                        dinnerFreqP3 = ncList[j].DinnerFreq.ToString().Split(',')[2];
                    }
                    dr[(j + 1).ToString()] = "正餐 " + dinnerFreqP1 + " 次/天\r点心 " + dinnerFreqP2 + " 次/天\r其他 " + dinnerFreqP2 + " 次/天";
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = GetCodeDtlByCodeType(ncList[j].ActivityAbility, "L00.018");
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].Weight;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].OtherDisease;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].Bmi;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = GetCodeDtlByCodeType(ncList[j].WeightEval, "L00.019");
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = GetCodeDtlByCodeType(ncList[j].DietState, "L00.020");
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].WaterUptake;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].Kcal + "kcal/天\r" + "(主食类 " + ncList[j].KcalFood + " 份、\r肉鱼豆蛋 " + ncList[j].KcalFish + " 份、蔬菜 " + ncList[j].KcalVegetables + " 份、\r水果类 " + ncList[j].KcalFruit + " 份、\r油脂类" + ncList[j].KcalGrease + " 份)";
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].Protein;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].Salinity;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].PipeKcal + "kcal/天\r" + "蛋白质 " + ncList[j].PipeProtein + " g、\r冲管水量 " + ncList[j].PipleWater + "c.c.、\r其他水量 " + ncList[j].PipleOtherWater + " c.c.、\rVit补充 " + ncList[j].PipleVit;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].NutrtionDiag;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].SpecialDiet;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].Suggestion;
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                for (var j = 0; j < colCount; j++)
                {
                    dr[(j + 1).ToString()] = ncList[j].Dietitian;
                }
                dt.Rows.Add(dr);

                doc.FillNutritionCateRecTable(0, dt);
            }

        }

       
        /// <summary>
        /// 根据Code和Type查询字典小项
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="type">Type</param>
        /// <returns>CodeDtl</returns>
        private string GetCodeDtlByCodeType(string id, string type)
        {
            var result = "";
            var orgList = orgserver.GetCodeDtl(id, type);
            if (orgList != null && orgList.Data != null)
            {
                result = string.IsNullOrEmpty(orgList.Data.ITEMNAME) ? "" : orgList.Data.ITEMNAME;
            }
            else
            {
                result = "";
            }
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

    }
}

