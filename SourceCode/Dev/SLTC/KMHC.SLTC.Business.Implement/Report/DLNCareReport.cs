using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.Report
{
    public class DLNCareReport : BaseReport
    {

        ICarePlanReportService carePlanservice = IOCContainer.Instance.Resolve<ICarePlanReportService>();
        IResidentManageService resservice = IOCContainer.Instance.Resolve<IResidentManageService>();
        IIndexManageService indexservice = IOCContainer.Instance.Resolve<IIndexManageService>();
        IOrganizationManageService orgserver = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
        INursingRecord _INursingRecord = IOCContainer.Instance.Resolve<INursingRecord>();

        protected override void Operation(WordDocument doc)
        {
            int id = Convert.ToInt32(ParamId);
            var now = DateTime.Now;
            var dtdc = new DateTime(1900, 1, 1);
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            doc.ReplaceText("year", StartDate.Year.ToString());
            var nurEval = carePlanservice.QueryNutritionEval(id);

            #region 临床检查

            if (nurEval != null)
            {

                #region 住民信息

                var residentInfo = carePlanservice.GetExportResidentInfo(Convert.ToInt64(nurEval.FEENO));

                if (residentInfo != null)
                {
                    doc.ReplaceText("name", residentInfo.Name ?? "");
                    doc.ReplaceText("birdate", residentInfo.BirthDate.HasValue ? Convert.ToDateTime(residentInfo.BirthDate).ToString("yyyy-MM-dd") : "");
                    doc.ReplaceText("age", residentInfo.Age.HasValue ? residentInfo.Age.ToString() : "");
                    doc.ReplaceText("sex", residentInfo.Sex ?? "未填");
                    doc.ReplaceText("bedno", residentInfo.BedNo ?? "");
                    doc.ReplaceText("ResidentNo", residentInfo.ResidentNo);
                    doc.ReplaceText("indate", residentInfo.InDate.HasValue ? Convert.ToDateTime(residentInfo.InDate).ToString("yyyy-MM-dd") : "");
                }
                else
                {
                    clearpersonInfo(doc);
                }

                #endregion

                #region 体重信息
                BaseRequest<NutrtionEvalFilter> requestnur = new BaseRequest<NutrtionEvalFilter>
                {
                    PageSize = 0,
                    Data =
                    {
                        FeeNo = Convert.ToInt64(nurEval.FEENO),
                        OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                        EndDate = StartDate,
                    }
                };
                var ipdresponse = resservice.QueryNutrtionEvalExtend(requestnur);

                if (ipdresponse.Data != null && ipdresponse.Data.Count>0)
                {
                    doc.ReplaceText("iDealWeight", ipdresponse.Data[0].IDEALWEIGHT.HasValue ? ipdresponse.Data[0].IDEALWEIGHT.ToString() : "");
                }
                else
                {
                    doc.ReplaceText("iDealWeight", "");
                }

                BaseRequest<UnPlanWeightIndFilter> request = new BaseRequest<UnPlanWeightIndFilter>();
                request.CurrentPage = 1;
                request.PageSize = 100;
                request.Data.RegNo = Convert.ToInt64(nurEval.REGNO);
                request.Data.FeeNo = Convert.ToInt64(nurEval.FEENO);
                request.Data.ThisRecDate1 = dtdc;
                request.Data.ThisRecDate2 = StartDate;
                request.Data.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                var unresponse = indexservice.QueryUnPlanWeightInd(request);


                if (unresponse.Data != null && unresponse.Data.Count > 0)
                {
                    doc.ReplaceText("height", unresponse.Data[0].ThisHeight.HasValue ? unresponse.Data[0].ThisHeight.ToString() : "");
                    doc.ReplaceText("ThisWeight", unresponse.Data[0].ThisWeight.HasValue ? unresponse.Data[0].ThisWeight.ToString() : "");
                    doc.ReplaceText("kneeLen", unresponse.Data[0].KneeLen.HasValue ? unresponse.Data[0].KneeLen.ToString() : "");

                    var count = 0;
                    if (unresponse.Data.Count >= 11)
                    {
                        count = 11;
                    }
                    else
                    {
                        count = unresponse.Data.Count;
                    }

                    DataTable dt = new DataTable();
                    var colCount = 0;
                    if (unresponse.Data.Count > 11)
                    {
                        colCount = 11;
                    }
                    else
                    {
                        colCount = unresponse.Data.Count;
                    }
                    for (var i = 0; i < colCount; i++)
                    {
                        dt.Columns.Add((i + 1).ToString());
                    }
                    var dr = dt.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        dr[(j + 1).ToString()] = unresponse.Data[j].ThisRecDate;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        dr[(j + 1).ToString()] = unresponse.Data[j].ThisWeight;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        dr[(j + 1).ToString()] = unresponse.Data[j].BMI;
                    }
                    dt.Rows.Add(dr);
                    doc.FillUnplanTable(2, dt);

                }
                else
                {
                    clearUnplanIpd(doc);
                }

      

                #endregion
                #region 生化检查数据

                //调用接口
                var biolist = carePlanservice.QueryBiochemistryList(Convert.ToInt32(nurEval.FEENO), StartDate.AddDays(1));
          
                if (biolist != null && biolist.Count > 0)
                {
                    DataTable dt = new DataTable();
                    var colCount = 0;
                    if (biolist.Count >11)
                    {
                        colCount = 11;
                    }
                    else
                    {
                        colCount = biolist.Count;
                    }
                    for (var i = 0; i < colCount; i++)
                    {
                        dt.Columns.Add((i + 1).ToString());

                    }

                    var dr = dt.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        dr[(j + 1).ToString()] = Convert.ToDateTime(biolist[j].CHECKDATE).ToString("yyyy-MM-dd");
                    }

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        var checkresult = string.Empty;

                        var result = biolist[j].ReportCheckRecdtl.Where(o => o.CHECKITEMNAME == "EASO%(嗜硷性白血球比)").ToList();
                        if (result == null)
                        {
                            checkresult = "";
                        }
                        else if (result != null && result.Count > 0)
                        {
                            checkresult = result.OrderByDescending(o => o.ID).ToList().FirstOrDefault().CHECKRESULTS;
                        }
                        dr[(j + 1).ToString()] = checkresult;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        var checkresult = string.Empty;

                        var result = biolist[j].ReportCheckRecdtl.Where(o => o.CHECKITEMNAME == "ALB(白蛋白)").ToList();
                        if (result == null)
                        {
                            checkresult = "";
                        }
                        else if (result != null && result.Count > 0)
                        {
                            checkresult = result.OrderByDescending(o => o.ID).ToList().FirstOrDefault().CHECKRESULTS;
                        }
                        dr[(j + 1).ToString()] = checkresult;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        var checkresult = string.Empty;

                        var result = biolist[j].ReportCheckRecdtl.Where(o => o.CHECKITEMNAME == "TG(三酸甘油脂)").ToList();
                        if (result == null)
                        {
                            checkresult = "";
                        }
                        else if (result != null && result.Count > 0)
                        {
                            checkresult = result.OrderByDescending(o => o.ID).ToList().FirstOrDefault().CHECKRESULTS;
                        }
                        dr[(j + 1).ToString()] = checkresult;
                    }
                    dt.Rows.Add(dr);

                    doc.FillBioTable(3, dt);
                }

                #endregion

                #region 营养照护单

                INursingManageService service = IOCContainer.Instance.Resolve<INursingManageService>();

                IList<NutrtioncateRec> ncList = service.QueryNutrtioncateRec(new BaseRequest<NutrtioncateRecFilter> { Data = new NutrtioncateRecFilter { FeeNo = Convert.ToInt64(nurEval.FEENO) } }).Data;

                ncList = ncList.Where(o => o.Recorddate <= StartDate.AddDays(1)).ToList();


                if (ncList != null && ncList.Count > 0)
                {
                    DataTable dt = new DataTable();
                    var colCount = 0;
                    if (ncList.Count > 3)
                    {
                        colCount = 3;
                    }
                    else
                    {
                        colCount = ncList.Count;
                    }
                    for (var i = 0; i < colCount; i++)
                    {
                        dt.Columns.Add((i + 1).ToString());

                    }
                    var dr = dt.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        dr[(j + 1).ToString()] = ncList[j].Recorddate;
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
                        var dinnerFreqP3 = "";
                        if (ncList[j].DinnerFreq.ToString().Split(',').Count() == 3)
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

                    doc.FillNutritionCateRecTable(5, dt);
                }

                #endregion


                doc.ReplaceText("diseasediag", nurEval.DISEASEDIAG ?? "");
                doc.ReplaceText("CHEWDIFFCULT", carePlanservice.GetCodedtlInfo( nurEval.CHEWDIFFCULT ,"L03.001"));  //咀嚼困难
                doc.ReplaceText("SWALLOWABILITY", carePlanservice.GetCodedtlInfo(nurEval.SWALLOWABILITY, "L00.004"));  //吞咽能力
                doc.ReplaceText("EATPATTERN", carePlanservice.GetCodedtlInfo(nurEval.EATPATTERN, "L03.003"));  //进食型式  L03.006
                doc.ReplaceText("DIGESTIONPROBLEM", nurEval.DIGESTIONPROBLEM ?? "");
                doc.ReplaceText("FOODTABOO", nurEval.FOODTABOO ?? "");
                doc.ReplaceText("ACTIVITYABILITY", carePlanservice.GetCodedtlInfo(nurEval.ACTIVITYABILITY, "L03.006"));  //活动能力  
                doc.ReplaceText("PRESSURESORE", nurEval.PRESSURESORE ?? "");
                doc.ReplaceText("EDEMA", nurEval.EDEMA ?? "");
                doc.ReplaceText("CURRENTDIET", nurEval.CURRENTDIET ?? "");
                doc.ReplaceText("EATAMOUNT", nurEval.EATAMOUNT ?? "");
                doc.ReplaceText("WATER", nurEval.WATER ?? "");
                doc.ReplaceText("SUPPLEMENTS", nurEval.SUPPLEMENTS ?? "");
                doc.ReplaceText("SNACK", nurEval.SNACK ?? "");
            }
            else
            {
                clearpersonInfo(doc);
                clearUnplanIpd(doc);
                clearClinical(doc);
            }
            #endregion
        }

        #region 清空数据
        /// <summary>
        /// 清空个案信息记录
        /// </summary>
        /// <param name="doc"></param>
        public void clearpersonInfo(WordDocument doc)
        {
            doc.ReplaceText("name", "");
            doc.ReplaceText("birdate", "");
            doc.ReplaceText("age", "");
            doc.ReplaceText("sex", "");
            doc.ReplaceText("no", "");
            doc.ReplaceText("bedno", "");
            doc.ReplaceText("indate", "");
        }

        /// <summary>
        /// 清空非计划减重数据
        /// </summary>
        /// <param name="doc"></param>
        public void clearUnplanIpd(WordDocument doc)
        {
            doc.ReplaceText("height", "");
            doc.ReplaceText("kneeLen", "");
            doc.ReplaceText("ThisWeight", "");
        }

        /// <summary>
        /// 清除临床检查信息
        /// </summary>
        /// <param name="doc"></param>
        public void clearClinical(WordDocument doc)
        {
            doc.ReplaceText("diseasediag", "");
            doc.ReplaceText("CHEWDIFFCULT", "");
            doc.ReplaceText("SWALLOWABILITY", "");
            doc.ReplaceText("EATPATTERN", "");
            doc.ReplaceText("DIGESTIONPROBLEM", "");
            doc.ReplaceText("FOODTABOO", "");
            doc.ReplaceText("ACTIVITYABILITY", "");
            doc.ReplaceText("PRESSURESORE", "");
            doc.ReplaceText("EDEMA", "");
            doc.ReplaceText("CURRENTDIET", "");
            doc.ReplaceText("EATAMOUNT", "");
            doc.ReplaceText("WATER", "");
            doc.ReplaceText("SUPPLEMENTS", "");
            doc.ReplaceText("SNACK", "");
        }
        #endregion

        #region 公共方法
        //获取年龄
        public static string GetAge(DateTime dtBirthday, DateTime dtNow)
        {
            string strAge = string.Empty;                         // 年龄的字符串表示
            int intYear = 0;                                    // 岁
            int intMonth = 0;                                    // 月
            int intDay = 0;                                    // 天
            if (dtBirthday == dtNow)
            {
                return string.Empty;
            }
            // 如果没有设定出生日期, 返回空
            if (string.IsNullOrEmpty(dtBirthday.ToString()))
            {
                return string.Empty;
            }

            // 计算天数
            intDay = dtNow.Day - dtBirthday.Day;
            if (intDay < 0)
            {
                dtNow = dtNow.AddMonths(-1);
                intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            }

            // 计算月数
            intMonth = dtNow.Month - dtBirthday.Month;
            if (intMonth < 0)
            {
                intMonth += 12;
                dtNow = dtNow.AddYears(-1);
            }

            // 计算年数
            intYear = dtNow.Year - dtBirthday.Year;

            // 格式化年龄输出
            if (intYear >= 1)                                            // 年份输出
            {
                strAge = intYear.ToString() + "岁";
            }

            if (intMonth > 0 && intYear <= 5)                           // 五岁以下可以输出月数
            {
                strAge += intMonth.ToString() + "月";
            }

            if (intDay >= 0 && intYear < 1)                              // 一岁以下可以输出天数
            {
                if (strAge.Length == 0 || intDay > 0)
                {
                    strAge += intDay.ToString() + "日";
                }
            }

            return strAge;
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
        #endregion
    }

}

