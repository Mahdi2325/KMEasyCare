using AutoMapper;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Report;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC.Report;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.DC.Report
{
    public class DC_NursingReportService : BaseService, IDC_NursingReportService
    {
        #region 个案药品管理
        public BaseResponse<IList<ReportRegMedicine>> QueryRegMedicineList(BaseRequest<ReportRegMedicine> request)
        {
            BaseResponse<IList<ReportRegMedicine>> response = new BaseResponse<IList<ReportRegMedicine>>();
            Mapper.CreateMap<DC_REGMEDICINE, ReportRegMedicine>();
            var q = from m in unitOfWork.GetRepository<DC_REGMEDICINE>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == request.Data.FeeNo);
            q = q.OrderBy(m => m.CREATEDATE);
            response.RecordsCount = q.Count();
            if (response.RecordsCount > 0)
            {
                response.Data = Mapper.Map<IList<ReportRegMedicine>>(q.ToList());
                response.Data[0].Age = GetAge(response.Data[0].BirthDate ?? DateTime.Now, DateTime.Now);
                response.Data[0].OrgName = GetOrgName(response.Data[0].OrgId);
            }
            else
            {
                response.Data = new List<ReportRegMedicine> { };
            }
            return response;
        }
        #endregion

        #region 护理诊断一览表

        public BaseResponse<IList<ReportRegCpl>> QueryRegCplList(BaseRequest<ReportRegCpl> request)
        {
            BaseResponse<IList<ReportRegCpl>> response = new BaseResponse<IList<ReportRegCpl>>();
            var q = from m in unitOfWork.GetRepository<DC_REGCPL>().dbSet
                    join _reg in unitOfWork.GetRepository<DC_REGFILE>().dbSet on m.REGNO equals _reg.REGNO into temp_reg
                    from reg in temp_reg.DefaultIfEmpty()
                    join org in unitOfWork.GetRepository<LTC_ORG>().dbSet on m.ORGID equals org.ORGID into orgs
                    from _org in orgs.DefaultIfEmpty()
                    select new
                    {
                        RegName = reg.REGNAME,
                        FeeNo = m.FEENO,
                        CpDia = m.CPDIA,
                        StartDate = m.STARTDATE,
                        FinishDate = m.FINISHDATE,
                        CreateDate = m.CREATEDATE,
                        OrgName = _org.ORGNAME,
                    };
            q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.CreateDate);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ReportRegCpl>();
                foreach (dynamic item in list)
                {
                    ReportRegCpl newItem = new ReportRegCpl();
                    newItem.RegName = item.RegName;
                    newItem.FeeNo = item.FeeNo;
                    newItem.CpDia = item.CpDia;
                    newItem.StartDate = item.StartDate;
                    newItem.FinishDate = item.FinishDate;
                    newItem.CreateDate = item.CreateDate;
                    newItem.OrgName = item.OrgName;
                    response.Data.Add(newItem);
                }
            };
            mapperResponse(q.ToList());
            return response;
        }
        #endregion

        #region 需求评估及照顾计划
        public BaseResponse<ReportNurseingPlanEval> QueryNurseingPlanEval(BaseRequest<ReportNurseingPlanEval> request)
        {
            BaseResponse<ReportNurseingPlanEval> response = new BaseResponse<ReportNurseingPlanEval>();
            var NursingPlanPartOne = new DcNurseingPlanEval();
            var NursingPlanPartTwo = new List<ReportEvalQuestionResult>();
            var NursingPlanPartThree = new List<NursingPlan>();
            var LastNursingPlan = new List<NursingPlan>();
            Mapper.CreateMap<DC_NURSEINGPLANEVAL, DcNurseingPlanEval>();
            var q = from m in unitOfWork.GetRepository<DC_NURSEINGPLANEVAL>().dbSet
                    select m;
            q = q.Where(m => m.ID == request.Data.Id);
            q = q.OrderByDescending(m => m.ID);
            if (q.Count() > 0)
            {
                NursingPlanPartOne = Mapper.Map<IList<DcNurseingPlanEval>>(q.ToList())[0];
                NursingPlanPartTwo = QueryPlanEvalList(NursingPlanPartOne.Id).Data;
                var lastId = GetLastNursingPlanEvalId(NursingPlanPartOne.FeeNo, NursingPlanPartOne.EvalNumber);
                if (lastId != 0)
                {
                    LastNursingPlan = GetnursingPlanById(lastId);
                }
                NursingPlanPartThree = GetnursingPlanById(NursingPlanPartOne.Id);
            }
            response.Data = new ReportNurseingPlanEval
            {
                OrgName = GetOrgName(NursingPlanPartOne.OrgId),
                ResidentNo = NursingPlanPartOne.ResidentNo ?? "",
                RegName = NursingPlanPartOne.RegName ?? "",
                Sex = NursingPlanPartOne.Sex ?? "",
                BirthDate = NursingPlanPartOne.BirthDate,
                Age = GetAge(DateTime.Parse((NursingPlanPartOne.BirthDate != null && NursingPlanPartOne.BirthDate.ToString() != "") ? NursingPlanPartOne.BirthDate.ToString() : DateTime.Now.ToString()), DateTime.Now),
                FirstevalDate = NursingPlanPartOne.FirstevalDate,
                EvalNumber = NursingPlanPartOne.EvalNumber,
                EvalDate = NursingPlanPartOne.EvalDate,
                InDate = NursingPlanPartOne.InDate,
                HospitalName = NursingPlanPartOne.HospitalName ?? "",
                FamilySignature = NursingPlanPartOne.FamilySignature ?? "",
                DiseaseInfo = NursingPlanPartOne.DiseaseInfo ?? "",
                OperationInfo = NursingPlanPartOne.OperationInfo ?? "",
                MedicineInfo = ("中药：" + (NursingPlanPartOne.Chinesedrugflag == true ? "有，药物名称：" + NursingPlanPartOne.Chinesedrug : "无") + "\r                西药：" + (NursingPlanPartOne.Westerndrugflag == true ? "有，药物名称：" + NursingPlanPartOne.Westerndrug : "无")) ?? "",
                VisitHospitalN = NursingPlanPartOne.VisitHospitalName ?? "",
                VisitType = (NursingPlanPartOne.VisitType == "001" ? "门诊" : (NursingPlanPartOne.VisitType == "002" ? "急诊" : (NursingPlanPartOne.VisitType == "003" ? "健康检查" : ""))) ?? "",
                VisitNumber = NursingPlanPartOne.VisitNumber,
                HeartRate = NursingPlanPartOne.HeartRate ?? "",
                BreathType = NursingPlanPartOne.BreathType ?? "",
                Height = NursingPlanPartOne.Height,
                Weight = NursingPlanPartOne.Weight,
                WaistLine = NursingPlanPartOne.WaistLine,
                BMIValue = NursingPlanPartOne.BMI,
                IW = NursingPlanPartOne.IdealWeight,
                Appetite = NursingPlanPartOne.Appetite ?? "",
                FoodHabit = NursingPlanPartOne.FoodHabit ?? "",
                FoodType = NursingPlanPartOne.FoodType ?? "",
                EatType = NursingPlanPartOne.EatType ?? "",
                TeethState = NursingPlanPartOne.TeethState ?? "",
                GumsState = NursingPlanPartOne.GumsState ?? "",
                Oralmucosa = NursingPlanPartOne.Oralmucosa ?? "",
                SwallowingAbility = NursingPlanPartOne.SwallowingAbility ?? "",
                MasticatoryAbility = NursingPlanPartOne.MasticatoryAbility ?? "",
                StapleFood = NursingPlanPartOne.StapleFood ?? "",
                Meat = NursingPlanPartOne.Meat ?? "",
                Vegetables = NursingPlanPartOne.Vegetables ?? "",
                Snack = NursingPlanPartOne.Snack ?? "",
                Soup = NursingPlanPartOne.Soup ?? "",
                SkinState = NursingPlanPartOne.SkinState ?? "",
                SkinColor = NursingPlanPartOne.SkinColor ?? "",
                Edema = NursingPlanPartOne.Edema ?? "",
                SkinIntegrity = NursingPlanPartOne.SkinIntegrity,
                SkinPart = NursingPlanPartOne.SkinPart ?? "",
                SkinSize = NursingPlanPartOne.SkinSize ?? "",
                SkinLevel = NursingPlanPartOne.SkinLevel,
                Defecation = NursingPlanPartOne.Defecation ?? "",
                ShitNumber = NursingPlanPartOne.ShitNumber,
                ShitAmOut = NursingPlanPartOne.ShitAmOut ?? "",
                ShitNature = NursingPlanPartOne.ShitNature ?? "",
                AssistedDef = NursingPlanPartOne.AssistedDefecation ?? "",
                IntestinalPeristalsis = NursingPlanPartOne.IntestinalPeristalsis ?? "",
                Micturition = NursingPlanPartOne.Micturition ?? "",
                UrinationNature = NursingPlanPartOne.UrinationNature ?? "",
                AconuresisFlag = NursingPlanPartOne.AconuresisFlag,
                AconuresisInfo = NursingPlanPartOne.AconuresisInfo ?? "",
                UrinationDisposal = NursingPlanPartOne.UrinationDisposal ?? "",
                RightMuscle1 = (NursingPlanPartOne.RightMuscle1 ?? "").Trim(),
                RightMuscle2 = (NursingPlanPartOne.RightMuscle2 ?? "").Trim(),
                LeftMuscle1 = (NursingPlanPartOne.LeftMuscle1 ?? "").Trim(),
                LeftMuscle2 = (NursingPlanPartOne.LeftMuscle2 ?? "").Trim(),
                RightJoint1 = (NursingPlanPartOne.RightJoint1 ?? "").Trim(),
                RightJoint2 = (NursingPlanPartOne.RightJoint2 ?? "").Trim(),
                LeftJoint1 = (NursingPlanPartOne.LeftJoint1 ?? "").Trim(),
                LeftJoint2 = (NursingPlanPartOne.LeftJoint2 ?? "").Trim(),
                Gait = NursingPlanPartOne.Gait ?? "",
                AssistantTool = NursingPlanPartOne.AssistantTool ?? "",
                AssistantSecurity = NursingPlanPartOne.AssistantSecurity ?? "",
                AssistantSuitability = NursingPlanPartOne.AssistantSuitability ?? "",
                ActivityName = NursingPlanPartOne.ActivityName ?? "",
                FallInfo = NursingPlanPartOne.FallInfo ?? "",
                Fall1YearInfo = ((NursingPlanPartOne.Fall1Year == true ? ("有，" + (NursingPlanPartOne.Injuredflag == true ? ("受伤部位：" + NursingPlanPartOne.Injuredpart) : "未受伤")) : "无")) ?? "",
                PastFallIf = NursingPlanPartOne.PastFallInfo ?? "",
                InjuryDisposalInfo = NursingPlanPartOne.InjuryDisposalInfo ?? "",
                PainFreq = NursingPlanPartOne.PainFreq ?? "",
                PainLevel = NursingPlanPartOne.PainLevel ?? "",
                PainPart = NursingPlanPartOne.PainPart ?? "",
                PainNature = NursingPlanPartOne.PainNature ?? "",
                DurationTime = NursingPlanPartOne.DurationTime ?? "",
                EasePainMethod = NursingPlanPartOne.EasePainMethod ?? "",
                VisualAcuity = NursingPlanPartOne.VisualAcuity ?? "",
                AssistTools = NursingPlanPartOne.AssistantTools ?? "",
                ListeningState = NursingPlanPartOne.ListeningState ?? "",
                Nextevaldate = NursingPlanPartOne.Nextevaldate,
                Delusion = NursingPlanPartOne.Delusion ?? "",
                PersonImage = NursingPlanPartOne.PersonImage ?? "",
                Attitude = NursingPlanPartOne.Attitude ?? "",
                EmotionState = NursingPlanPartOne.EmotionState ?? "",
                DisturbingEnv = NursingPlanPartOne.DisturbingEnv ?? "",
                SootheMotion = NursingPlanPartOne.SootheMotion ?? "",
                Behavior = NursingPlanPartOne.Behavior ?? "",
                CommunicationType = NursingPlanPartOne.CommunicationType ?? "",
                CommunicationSkill = NursingPlanPartOne.CommunicationSkill ?? "",
                ProblemBeh = NursingPlanPartOne.ProblemBehavior ?? "",
                adlA1 = GetDetailScore(NursingPlanPartTwo, "进食", "ADL"),
                adlA2 = GetDetailScore(NursingPlanPartTwo, "如厕", "ADL"),
                adlA3 = GetDetailScore(NursingPlanPartTwo, "上下楼梯", "ADL"),
                adlA4 = GetDetailScore(NursingPlanPartTwo, "穿脱衣鞋袜", "ADL"),
                adlA5 = GetDetailScore(NursingPlanPartTwo, "大便控制", "ADL"),
                adlA6 = GetDetailScore(NursingPlanPartTwo, "小便控制", "ADL"),
                adlA7 = GetDetailScore(NursingPlanPartTwo, "移动", "ADL"),
                adlA8 = GetDetailScore(NursingPlanPartTwo, "平地上走动", "ADL"),
                adlA9 = GetDetailScore(NursingPlanPartTwo, "个人卫生", "ADL"),
                lA10 = GetDetailScore(NursingPlanPartTwo, "洗澡", "ADL"),
                ilA1 = GetDetailScore(NursingPlanPartTwo, "使用电话能力", "IADL"),
                ilA2 = GetDetailScore(NursingPlanPartTwo, "上街购物", "IADL"),
                ilA3 = GetDetailScore(NursingPlanPartTwo, "准备食物", "IADL"),
                ilA4 = GetDetailScore(NursingPlanPartTwo, "家事维持", "IADL"),
                ilA5 = GetDetailScore(NursingPlanPartTwo, "清洗衣物", "IADL"),
                ilA6 = GetDetailScore(NursingPlanPartTwo, "使用交通方式", "IADL"),
                ilA7 = GetDetailScore(NursingPlanPartTwo, "服用药物", "IADL"),
                ilA8 = GetDetailScore(NursingPlanPartTwo, "处理财务能力", "IADL"),
                mmseA1 = GetDetailScore(NursingPlanPartTwo, "定向感", "MMSE"),
                mmseA2 = GetDetailScore(NursingPlanPartTwo, "注意力", "MMSE"),
                mmseA3 = GetDetailScore(NursingPlanPartTwo, "计算力", "MMSE"),
                mmseA4 = GetDetailScore(NursingPlanPartTwo, "记忆", "MMSE"),
                mmseA5 = GetDetailScore(NursingPlanPartTwo, "语言", "MMSE"),
                mmseA6 = GetDetailScore(NursingPlanPartTwo, "口语理解及行动能力", "MMSE"),
                mmseA7 = GetDetailScore(NursingPlanPartTwo, "建构力", "MMSE"),
                lastNursingPlan = LastNursingPlan,
                nursingPlan = NursingPlanPartThree,
            };
            return response;
        }

        private long GetLastNursingPlanEvalId(long feeNo, int evalNum)
        {
            long lastId = 0;
            Mapper.CreateMap<DC_NURSEINGPLANEVAL, DcNurseingPlanEval>();
            var q = from m in unitOfWork.GetRepository<DC_NURSEINGPLANEVAL>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == feeNo);
            q = q.Where(m => m.EVALNUMBER == evalNum - 1);
            q = q.OrderByDescending(m => m.ID);
            if (q.Count() > 0)
            {
                lastId = Mapper.Map<IList<DcNurseingPlanEval>>(q.ToList())[0].Id;
            }
            return lastId;
        }

        private static string GetDetailScore(List<ReportEvalQuestionResult> NursingPlanPartTwo, string p1, string p2)
        {
            if (NursingPlanPartTwo.Where(m => m.MAKENAME.Contains(p1) && m.CODE == p2).ToList().Count > 0)
            {
                decimal score = (NursingPlanPartTwo.Where(m => m.MAKENAME.Contains(p1) && m.CODE == p2).ToList())[0].LIMITEDVALUE ?? 0;
                return Math.Round(score, 0).ToString();
            }
            else
            {
                return "";
            }

        }

        private List<NursingPlan> GetnursingPlanById(long id)
        {
            var nursingPlanList = new List<NursingPlan>();
            IList<DC_REGCPL> regCpl = new List<DC_REGCPL>();
            Mapper.CreateMap<DC_REGCPL, DC_REGCPL>();
            var q = from m in unitOfWork.GetRepository<DC_REGCPL>().dbSet
                    select m;
            q = q.Where(m => m.ID == id);
            if (q.Count() > 0)
            {
                regCpl = Mapper.Map<IList<DC_REGCPL>>(q.ToList());
                foreach (var item in regCpl)
                {
                    var nursingPlan = new NursingPlan();
                    nursingPlan.Cpdia = item.CPDIA;
                    if (item.DC_NSCPLACTIVITY.Count > 0)
                    {
                        var activity = "";
                        foreach (var item2 in item.DC_NSCPLACTIVITY)
                        {
                            activity = activity + "● " + item2.CPLACTIVITY.Trim() + "\r";
                        }
                        nursingPlan.Activity = activity;
                    }
                    nursingPlan.Finished = item.FINISHFLAG == true ? "已完成" : "未完成";
                    nursingPlanList.Add(nursingPlan);
                }
            }

            return nursingPlanList;
        }

        private BaseResponse<List<ReportEvalQuestionResult>> QueryPlanEvalList(long id)
        {
            var response = new BaseResponse<List<ReportEvalQuestionResult>>();
            var q = from eq in unitOfWork.GetRepository<DC_EVALQUESTION>().dbSet
                    join _eqr in unitOfWork.GetRepository<DC_EVALQUESTIONRESULT>().dbSet on eq.RECORDID equals _eqr.RECORDID into temp_eqr
                    from eqr in temp_eqr.DefaultIfEmpty()
                    join _mv in unitOfWork.GetRepository<LTC_MAKERITEMLIMITEDVALUE>().dbSet on eqr.LIMITEDVALUEID equals _mv.LIMITEDVALUEID into temp_mv
                    from mv in temp_mv.DefaultIfEmpty()
                    join _m in unitOfWork.GetRepository<LTC_MAKERITEM>().dbSet on eqr.MAKERID equals _m.MAKERID into temp_m
                    from m in temp_m.DefaultIfEmpty()
                    join _qu in unitOfWork.GetRepository<LTC_QUESTION>().dbSet on eqr.QUESTIONID equals _qu.QUESTIONID into temp_qu
                    from qu in temp_qu.DefaultIfEmpty()
                    select new
                    {
                        eq.ID,
                        evarQuestionResult = eqr,
                        mv.LIMITEDVALUE,
                        m.MAKENAME,
                        qu.CODE,
                        qu.ORGID
                    };
            q = q.Where(m => m.ID == id);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ReportEvalQuestionResult>();
                foreach (dynamic item in list)
                {
                    ReportEvalQuestionResult newItem = new ReportEvalQuestionResult();
                    newItem.ID = item.ID;
                    newItem.LIMITEDVALUE = item.LIMITEDVALUE;
                    newItem.MAKENAME = item.MAKENAME;
                    newItem.CODE = item.CODE;
                    newItem.ORGID = item.ORGID;
                    response.Data.Add(newItem);
                }
            };
            mapperResponse(q.ToList());
            return response;
        }
        #endregion

        #region 个案基本资料汇整表
        public BaseResponse<IList<ReportBaseInfoList>> QueryAllRegBaseInfoList(long feeNo)
        {

            var response = new BaseResponse<IList<ReportBaseInfoList>>();
            Mapper.CreateMap<DC_REGBASEINFOLIST, ReportBaseInfoList>();
            var q = from m in unitOfWork.GetRepository<DC_REGBASEINFOLIST>().dbSet
                    select m;
            q = q.Where(m => m.FEENO == feeNo);
            q = q.OrderBy(m => m.ID);
            if (q.Count() > 0)
            {
                response.Data = Mapper.Map<IList<ReportBaseInfoList>>(q.ToList());
                response.Data[0].OrgName = GetOrgName(response.Data[0].OrgId);
            }
            else
            {
                response.Data = null;
            }

            return response;
        }
        #endregion

        #region 个案个别化活动需求初评
        public BaseResponse<ReportRegActivityRequestEval> QueryCurrentRegActivityRequestEval(long id)
        {
            BaseResponse<ReportRegActivityRequestEval> response = new BaseResponse<ReportRegActivityRequestEval>();
            Mapper.CreateMap<DC_REGACTIVITYREQUESTEVAL, ReportRegActivityRequestEval>();
            var q = from m in unitOfWork.GetRepository<DC_REGACTIVITYREQUESTEVAL>().dbSet
                    select m;
            q = q.Where(m => m.ID == id);
            q = q.Where(m => m.DELFLAG == false);
            q = q.OrderByDescending(m => m.EVALDATE);
            if (q.Count() > 0)
            {
                response.Data = Mapper.Map<IList<ReportRegActivityRequestEval>>(q.ToList())[0];
                response.Data.PresStrategy = response.Data.PreserveStrategy;
                response.Data.PromotStrategy = response.Data.PromotionStrategy;
                response.Data.EsStrategy = response.Data.EaseStrategy;
                response.Data.Age = GetAge(DateTime.Parse((response.Data.BirthDate != null && response.Data.BirthDate.ToString() != "") ? response.Data.BirthDate.ToString() : DateTime.Now.ToString()), DateTime.Now);
                response.Data.OrgName = GetOrgName(response.Data.OrgId);
            }
            return response;
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
        //日期格式转换
        public static string dateFormat(string date)
        {
            var result = "";
            if (date != null)
            {
                try
                {
                    result = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        public string GetOrgName(string orgId)
        {
            BaseResponse<LTC_ORG> response = new BaseResponse<LTC_ORG>();
            Mapper.CreateMap<LTC_ORG, LTC_ORG>();
            var q = from m in unitOfWork.GetRepository<LTC_ORG>().dbSet
                    select m;
            q = q.Where(m => m.ORGID == orgId);
            if (q.Count() > 0)
            {
                response.Data = Mapper.Map<IList<LTC_ORG>>(q.ToList())[0];
            }
            return response.Data.ORGNAME;
        }
        #endregion
    }
}

