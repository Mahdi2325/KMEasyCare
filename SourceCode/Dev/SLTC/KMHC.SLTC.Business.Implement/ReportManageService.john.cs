/*
创建人: 刘美方
创建日期:2016-05-13
说明:报告
*/
using AutoMapper;
using ExcelReport;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KM.Common;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Repository;
using KMHC.SLTC.Repository.Base;
using NPOI.SS.Formula.Functions;

namespace KMHC.SLTC.Business.Implement
{
    public partial class ReportManageService : BaseService, IReportManageService
    {

        #region H49院内感染指标统计表
        public void DownloadInfectionStatisticsFile(int year, string templateFile)
        {
            var response = this.GetInfectionStatisticsData(year);
            SheetParameterContainer sheetParameterContainer = new SheetParameterContainer() { SheetName = "统计总表" };
            dynamic[] categorys = new dynamic[] { 
                new { category = "001", rowIndex = 7 },      //上呼吸道感染人次 
                new { category = "002", rowIndex = 9 },      //下呼吸道感染人次 
                new { category = "003", rowIndex = 14 },    //使用导尿管感染人次 
                new { category = "004", rowIndex = 17 },    //未使用导尿管感染人次
                new { category = "005", rowIndex = 19 },    //疥疮感染人次 
               // new { category = "usedPipe", rowIndex = 13 },   //使用导尿管人日数
              };
            List<ElementFormatter> elementFormatters = new List<ElementFormatter>();
            // 生成年份参数
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "year", RowIndex = 0, ColumnIndex = 1 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["year"], year));

            for (int i = 0; i < categorys.Length; i++)
            {
                var item = categorys[i];
                elementFormatters.Add(NewTableFormatter(response, sheetParameterContainer, item.rowIndex, item.category));
            }

            #region 住民总人日数
            int row = 2, col = 0;
            int queryMonth = 0;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();

            var dbSet = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(o => o.ORGID == orgId && (o.IPDFLAG == "I") && o.INDATE.HasValue && ((DateTime)o.INDATE).Year == year).AsQueryable();
            var ipd = (from a in dbSet
                       let Year = SqlFunctions.DatePart("Year", (DateTime)a.INDATE)
                       let Month = SqlFunctions.DatePart("Month", (DateTime)a.INDATE)
                       group a by new { Year, Month } into g
                       where g.Key.Year == year
                       select new StatisticItem { Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() });
            var ipdRegList = ipd.ToList();
            int maxMonth = 0;
            if (ipdRegList != null && ipdRegList.Count > 0)
            {
                maxMonth = ipdRegList.OrderByDescending(s => s.Month).FirstOrDefault().Month;
            }

            for (int j = 1; j <= 12; j++)
            {
                decimal leaveHospTotal = 0;
                decimal unPlanIpdTotal = 0;
                col = j;
                if (col > maxMonth)
                {
                    queryMonth = 0;
                }
                else
                {
                    queryMonth = col;
                }

                decimal total =
                    unitOfWork.GetRepository<LTC_IPDREG>()
                        .dbSet.Count(
                            o =>
                                o.ORGID == orgId && (o.IPDFLAG == "I") && o.INDATE.HasValue &&
                                ((DateTime)o.INDATE).Year == year && ((DateTime)o.INDATE).Month <= queryMonth);

                decimal ipdRegInTotal = total * DateTime.DaysInMonth(year, j); //当月在院住民总人日数

                decimal ipdRegOutTotal = reportManageService.OutLeaveUnPlanTotal(year, col, reportManageService.GetIpdOutTotal(year));    //当月结案的人日次数

                if (ipdRegInTotal == 0 && ipdRegOutTotal == 0)
                {
                    leaveHospTotal = 0;
                    unPlanIpdTotal = 0;
                }
                else
                {
                    leaveHospTotal = reportManageService.OutLeaveUnPlanTotal(year, col, reportManageService.GetLeaveHospTotal(year)); // 当月请假的人日次数
                    unPlanIpdTotal = reportManageService.OutLeaveUnPlanTotal(year, col, reportManageService.GetUnPlanIpdTotal(year)); //当月非计划住院人日次数
                }
                decimal ipdRegCount = ipdRegInTotal + ipdRegOutTotal - leaveHospTotal - unPlanIpdTotal;   //当月住民总人日数
                elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("{0}_{1}", row, col), RowIndex = row, ColumnIndex = col }, ipdRegCount));
            }
            #endregion

            #region 使用导尿管人日数

            row = 13;
            for (int j = 1; j <= 12; j++)
            {
                decimal sTotal = 0;
                col = j;
                if (col <= maxMonth)
                {
                    sTotal = reportManageService.OutLeaveUnPlanTotal(year, col, reportManageService.GetUsedPipeDaysTotal(year));
                }
                else
                {
                    sTotal = 0;
                }

                elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("{0}_{1}", row, col), RowIndex = row, ColumnIndex = col }, sTotal));
            }
            #endregion
            ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("统计总表", elementFormatters.Where(it => it != null).ToArray()));
        }

        public List<InfectionInd> GetInfectionIndList(DateTime date)
        {
            return unitOfWork.GetRepository<LTC_INFECTIONIND>().dbSet.Where(o => o.IFCDATE.HasValue && ((DateTime)o.IFCDATE).Year == date.Year && ((DateTime)o.IFCDATE).Month == date.Month
                    && !string.IsNullOrEmpty(o.CATEGORY) && o.IFCTYPE == "001").Select(o => new InfectionInd { Category = o.CATEGORY }).ToList();

        }

        private BaseResponse<List<YearJoinCategory>> GetInfectionStatisticsData(int year)
        {
            BaseResponse<List<YearJoinCategory>> response = new BaseResponse<List<YearJoinCategory>>();
            var dbSet = unitOfWork.GetRepository<LTC_INFECTIONIND>().dbSet.Where(o => o.ORGID == orgId && !string.IsNullOrEmpty(o.CATEGORY) && o.IFCTYPE == "001" && o.IFCDATE.HasValue).AsQueryable();
            var q = (from a in dbSet
                     let Year = SqlFunctions.DatePart("Year", (DateTime)a.IFCDATE)
                     let Month = SqlFunctions.DatePart("Month", (DateTime)a.IFCDATE)
                     group a by new { a.CATEGORY, Year, Month } into g
                     where g.Key.Year == year
                     select new StatisticItem { Category = g.Key.CATEGORY, Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() });

            //var dbSetPipe = unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet.AsQueryable().Where(o => o.PIPELINENAME == "002" || o.PIPELINENAME == "003" && o.RECORDDATE.HasValue);
            var dbSetPipe = (from o in unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet
                             join b in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on o.FEENO equals b.FEENO
                             where (o.PIPELINENAME == "002" || o.PIPELINENAME == "003") && o.RECORDDATE.HasValue && b.ORGID == orgId
                             select o);

            var q1 = (from a in dbSetPipe
                      let Year = SqlFunctions.DatePart("Year", (DateTime)a.RECORDDATE)
                      let Month = SqlFunctions.DatePart("Month", (DateTime)a.RECORDDATE)
                      group a by new { Year, Month } into g
                      where g.Key.Year == year
                      select new StatisticItem { Category = "usedPipe", Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() });

            var dbSetResident = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.AsQueryable().Where(o => o.ORGID == orgId && o.IPDFLAG == "I" && o.INDATE.HasValue);

            var q2 = (from a in dbSetResident
                      let Year = SqlFunctions.DatePart("Year", (DateTime)a.INDATE)
                      let Month = SqlFunctions.DatePart("Month", (DateTime)a.INDATE)
                      group a by new { Year, Month } into g
                      where g.Key.Year == year
                      select new StatisticItem { Category = "", Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() });

            var list = q.Union(q1).Union(q2).ToList().GroupBy(it => it.Category).ToList();
            response.Data = new List<YearJoinCategory>();

            list.ForEach(it =>
            {
                var item = new YearJoinCategory() { Category = it.Key };
                item.SetYearJoinCategory(it.ToList());
                response.Data.Add(item);
            });

            return response;
        }


        #endregion


        #region  H76压疮风险评估
        public void DownloadPrsSoreRisk(string templateFile, long feeNo, DateTime? startDate, DateTime? endDate)
        {
            //List<ElementFormatter> elementFormatters = new List<ElementFormatter>
            //{
            //    new CellFormatter(new Parameter() {Name = "BedNo", RowIndex = 2, ColumnIndex = 1}, "BedNo"),
            //    new CellFormatter(new Parameter() {Name = "Name", RowIndex = 2, ColumnIndex = 4}, "Name"),
            //    new CellFormatter(new Parameter() {Name = "Sex", RowIndex = 2, ColumnIndex = 7}, "Sex")
            //};
            //ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("Sheet1", elementFormatters.Where(it => it != null).ToArray()));

            SheetParameterContainer sheetParameterContainer = new SheetParameterContainer() { SheetName = "Sheet1" };
            List<ElementFormatter> elementFormatters = new List<ElementFormatter>();
            var residend = GetResidentInfo(feeNo);
            if (residend == null)
            {
                ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("Sheet1", elementFormatters.Where(it => it != null).ToArray()));
                return;
            }
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "BedNo", RowIndex = 2, ColumnIndex = 1 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["BedNo"], residend.BedNo));
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "Name", RowIndex = 2, ColumnIndex = 4 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["Name"], residend.Name));
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "Sex", RowIndex = 2, ColumnIndex = 7 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["Sex"], residend.Sex));

            //var surveys = from a in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(o => o.QUESTIONID == 10 && o.FEENO == 10)
            //              join b in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.EVALUATEBY equals b.EMPNO into reqEmp
            //              from ab in reqEmp.DefaultIfEmpty()
            //              select new Survey()
            //              {
            //                  Id = a.RECORDID,
            //                  CreateDate = a.EVALDATE,
            //                  Result = a.ENVRESULTS,
            //                  Score = a.SCORE,
            //                  NursingBy = ab != null ? ab.EMPNAME : null,
            //              };

            var surveys = unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(o => o.QUESTIONID == 10 && o.FEENO == feeNo && o.EVALDATE.HasValue && o.EVALDATE >= startDate && o.EVALDATE <= endDate)
                .OrderByDescending(o => o.EVALDATE).Select(o => new Survey()
                {
                    Id = o.RECORDID,
                    Score = o.SCORE,
                    Result = o.ENVRESULTS,
                    CreateDate = o.EVALDATE
                }).ToList();
            surveys = surveys.GetRange(0, surveys.Count > 16 ? 16 : surveys.Count);
            if (surveys.Count <= 0)
            {
                ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("Sheet1", elementFormatters.Where(it => it != null).ToArray()));
                return;
            }

            var recordIds = surveys.Select(o => o.Id).ToArray();

            var answers = (from a in unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().dbSet.Where(o => recordIds.Contains(o.RECORDID.Value) && o.MAKERID.HasValue && o.RECORDID.HasValue)
                           join b in unitOfWork.GetRepository<LTC_MAKERITEMLIMITEDVALUE>().dbSet.Where(o => o.LIMITEDVALUE.HasValue) on a.LIMITEDVALUEID equals b.LIMITEDVALUEID
                           select new Answer()
                           {
                               Id = a.MAKERID,
                               SurveyId = a.RECORDID.Value,
                               Score = b.LIMITEDVALUE
                           }).ToList();

            if (answers.Count <= 0)
            {
                ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("Sheet1", elementFormatters.Where(it => it != null).ToArray()));
                return;
            }

            for (var col = 0; col < surveys.Count; col++)
            {
                for (var row = 0; row < 11; row++)
                {
                    sheetParameterContainer.ParameterList.Add(new Parameter() { Name = string.Format("{0}_{1}", col, row), RowIndex = row + 3, ColumnIndex = col + 3 });
                    switch (row)
                    {
                        case 0:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], surveys[col].CreateDate.HasValue ? ((DateTime)surveys[col].CreateDate).ToShortDateString() : ""));
                            break;
                        case 1:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], surveys[col].Score));
                            break;
                        case 3:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 123).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 4:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 124).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 5:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 125).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 6:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 126).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 7:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 127).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 8:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 128).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 9:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], surveys[col].Score));
                            break;
                        case 10:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], surveys[col].Result));
                            break;

                    }
                }
            }
            ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("Sheet1", elementFormatters.Where(it => it != null).ToArray()));

        }

        public Question GetQuestion(int id)
        {
            var question = (from a in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(o => o.RECORDID == id)
                            join b in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on a.REGNO equals b.REGNO
                            join c in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals c.REGNO
                            join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on c.FLOOR equals j.FLOORID into aj
                            join k in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on c.ROOMNO equals k.ROOMNO into ak
                            join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.EVALUATEBY equals d.EMPNO into ad
                            join o in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals o.ORGID into ao
                            from e in ad.DefaultIfEmpty()
                            from f in ao.DefaultIfEmpty()
                            from l in aj.DefaultIfEmpty()
                            from m in ak.DefaultIfEmpty()
                            select new Question()
                            {
                                Id = a.RECORDID,
                                FeeNo = a.FEENO,
                                ResidengNo = b.RESIDENGNO,
                                CreateDate = a.EVALDATE,
                                NextDate = a.NEXTEVALDATE,
                                Result = a.ENVRESULTS,
                                Score = a.SCORE,
                                Brithdate = b.BRITHDATE,
                                Name = b.NAME,
                                BedNo = c.BEDNO,
                                Area = l.FLOORNAME + m.ROOMNAME,
                                EvaluateBy = e.EMPNAME,
                                OrgId = f.ORGID,
                                Org = f.ORGNAME,
                                QuestionId = a.QUESTIONID
                            }).FirstOrDefault();
            SetQuestionData(question);
            return question;
        }

        private void SetQuestionData(Question question)
        {
            if (question != null)
            {
                question.OrgId = orgId;
                if (question.CreateDate.HasValue)
                {
                    question.CDTW = string.Format("{0}/{1}/{2}", question.CreateDate.Value.Year, question.CreateDate.Value.Month, question.CreateDate.Value.Day);
                }
                if (question.NextDate.HasValue)
                {
                    question.NDTW = string.Format("{0}/{1}/{2}", question.NextDate.Value.Year, question.NextDate.Value.Month, question.NextDate.Value.Day);
                }
                if (question.QuestionId.HasValue)
                {
                    var regQuestionList = (from a in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(o => o.QUESTIONID == question.QuestionId)
                                           select new
                                           {
                                               FeeNo = a.FEENO,
                                           }).ToList();
                    question.OneEvaluateTotal = regQuestionList.Count(it => it.FeeNo == question.FeeNo);
                    question.EvaluateTotal = regQuestionList.Count;
                }
                if (question.Brithdate.HasValue)
                {
                    question.Age = DateTime.Now.Year - question.Brithdate.Value.Year;
                }
            }
        }

        public List<Question> GetQuestionList(long feeNo, int questionId)
        {
            var questionList = (from a in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(o => o.FEENO == feeNo && o.QUESTIONID == questionId)
                                join b in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on a.REGNO equals b.REGNO
                                join c in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals c.REGNO
                                join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on c.FLOOR equals j.FLOORID into aj
                                join k in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on c.ROOMNO equals k.ROOMNO into ak
                                join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.EVALUATEBY equals d.EMPNO into ad
                                join o in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals o.ORGID into ao
                                from e in ad.DefaultIfEmpty()
                                from f in ao.DefaultIfEmpty()
                                from l in aj.DefaultIfEmpty()
                                from m in ak.DefaultIfEmpty()
                                select new Question()
                                {
                                    Id = a.RECORDID,
                                    FeeNo = a.FEENO,
                                    ResidengNo = b.RESIDENGNO,
                                    CreateDate = a.EVALDATE,
                                    NextDate = a.NEXTEVALDATE,
                                    Result = a.ENVRESULTS,
                                    Score = a.SCORE,
                                    Brithdate = b.BRITHDATE,
                                    Name = b.NAME,
                                    BedNo = c.BEDNO,
                                    Area = l.FLOORNAME + m.ROOMNAME,
                                    EvaluateBy = e.EMPNAME,
                                    OrgId = f.ORGID,
                                    Org = f.ORGNAME,
                                    QuestionId = a.QUESTIONID
                                }).ToList();

            questionList.ForEach(question => SetQuestionData(question));
            return questionList;
        }

        public Question GetQuestion(long recordId)
        {
            var questionList = (from a in unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(o => o.RECORDID == recordId)
                                join b in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on a.REGNO equals b.REGNO
                                join c in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals c.REGNO
                                join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on c.FLOOR equals j.FLOORID into aj
                                join k in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on c.ROOMNO equals k.ROOMNO into ak
                                join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.EVALUATEBY equals d.EMPNO into ad
                                join o in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals o.ORGID into ao
                                from e in ad.DefaultIfEmpty()
                                from f in ao.DefaultIfEmpty()
                                from l in aj.DefaultIfEmpty()
                                from m in ak.DefaultIfEmpty()
                                select new Question()
                                {
                                    Id = a.RECORDID,
                                    FeeNo = a.FEENO,
                                    ResidengNo = b.RESIDENGNO,
                                    CreateDate = a.EVALDATE,
                                    NextDate = a.NEXTEVALDATE,
                                    Result = a.ENVRESULTS,
                                    Score = a.SCORE,
                                    Brithdate = b.BRITHDATE,
                                    Name = b.NAME,
                                    BedNo = c.BEDNO,
                                    Area = l.FLOORNAME + m.ROOMNAME,
                                    EvaluateBy = e.EMPNAME,
                                    OrgId = f.ORGID,
                                    Org = f.ORGNAME,
                                    QuestionId = a.QUESTIONID
                                }).ToList();

            questionList.ForEach(question => SetQuestionData(question));
            return questionList.Count > 0 ? questionList[0] : new Question() { };
        }

        public IEnumerable<Answer> GetAnswers(long qId)
        {
            var answers = (from a in unitOfWork.GetRepository<LTC_REGQUESTIONDATA>().dbSet.Where(o => o.RECORDID == qId && o.MAKERID.HasValue && o.RECORDID.HasValue)
                           join b in unitOfWork.GetRepository<LTC_MAKERITEMLIMITEDVALUE>().dbSet.Where(o => o.LIMITEDVALUE.HasValue) on a.LIMITEDVALUEID equals b.LIMITEDVALUEID
                           select new Answer()
                           {
                               Id = a.MAKERID,
                               SurveyId = a.RECORDID.Value,

                               Score = b.LIMITEDVALUE,
                               Value = b.LIMITEDVALUENAME
                           }).ToList();
            return answers;
        }

        #endregion


        public ResidentInfo GetResidentInfo(long feeNo)
        {
            var resident = (from a in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                            join b in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.REGNO equals b.REGNO
                            join j in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on b.FLOOR equals j.FLOORID into aj
                            join k in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId) on b.ROOMNO equals k.ROOMNO into ak
                            join c in unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Where(o => o.ITEMTYPE == "A00.001") on a.SEX equals c.ITEMCODE into ac
                            from l in aj.DefaultIfEmpty()
                            from m in ak.DefaultIfEmpty()
                            from d in ac.DefaultIfEmpty()
                            let age = a.BRITHDATE.HasValue ? (int?)(DateTime.Now.Year - a.BRITHDATE.Value.Year) : null
                            join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals e.ORGID
                            where b.FEENO == feeNo
                            select new ResidentInfo
                            {
                                Org = e.ORGNAME,
                                RegNo = a.REGNO,
                                FeeNo = b.FEENO,
                                Name = a.NAME,
                                BedNo = b.BEDNO,
                                Floor = l.FLOORNAME,
                                RoomNo = m.ROOMNAME,
                                ResidengNo = a.RESIDENGNO,
                                Age = age,
                                Weight = a.WEIGHT,
                                Sex = d != null ? d.ITEMNAME : "",
                                DiseaseDiag=a.DISEASEDIAG,
                            }).FirstOrDefault();
            return resident;
        }



        public IList<Statistic> GetUnPlanEdipd(DateTime date, bool h72Flag = false)
        {
            IQueryable<LTC_UNPLANEDIPD> set = unitOfWork.GetRepository<LTC_UNPLANEDIPD>().dbSet.Where(o => o.ORGID == orgId);
            if (h72Flag)
            {
                set = set.Where(o => o.H72IPD.HasValue && o.H72IPD.Value);
            }
            var list = from a in set.Where(o => o.IPDCAUSE != "" && o.UNPLANFLAG.HasValue && o.UNPLANFLAG.Value
                && o.INDATE.HasValue && ((DateTime)o.INDATE).Year == date.Year && ((DateTime)o.INDATE).Month == date.Month)
                       group a by new { a.IPDCAUSE }
                           into g
                           select new Statistic { Type = g.Key.IPDCAUSE, Total = g.Count() };
            var list1 = from a in list
                        join b in unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Where(o => o.ITEMTYPE == "K00.010") on a.Type equals b.ITEMCODE
                        select new Statistic { Type = b.ITEMNAME, Total = a.Total };
            return list1.ToList();
        }

        public int GetUnPlanEdipdH72Total(DateTime date)
        {
            return unitOfWork.GetRepository<LTC_UNPLANEDIPD>().dbSet.Count(o => o.ORGID == orgId && o.H72IPD.HasValue && o.H72IPD.Value && o.INDATE.HasValue && ((DateTime)o.INDATE).Year == date.Year && ((DateTime)o.INDATE).Month == date.Month);
        }

        public int GetNewResidentTotal(DateTime date)
        {
            return unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Count(o => o.ORGID == orgId && o.IPDFLAG == "I" && o.INDATE.HasValue && ((DateTime)o.INDATE).Year == date.Year && ((DateTime)o.INDATE).Month == date.Month);
        }

        public int GetResidentTotal(DateTime date)
        {
            return unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Count(o => o.ORGID == orgId && (o.IPDFLAG == "I")
                && o.INDATE.HasValue && ((DateTime)o.INDATE).Year <= date.Year && ((DateTime)o.INDATE).Month <= date.Month);
        }

        public IList<Statistic> GetInfection(DateTime date)
        {
            var set = unitOfWork.GetRepository<LTC_INFECTIONIND>().dbSet.Where(o => o.ORGID == orgId && !string.IsNullOrEmpty(o.CATEGORY) && o.IFCTYPE == "001").AsQueryable();
            var list = from a in set.Where(o => o.IFCDATE.HasValue && ((DateTime)o.IFCDATE).Year == date.Year && ((DateTime)o.IFCDATE).Month == date.Month)
                       group a by new { a.CATEGORY }
                           into g
                           select new Statistic { Type = g.Key.CATEGORY, Total = g.Count() };
            return list.ToList();
        }

        public int GetUsedPipeTotal(DateTime date)
        {
            //return unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet.Count(o => (o.PIPELINENAME == "002" || o.PIPELINENAME == "003") &&
            //    o.RECORDDATE.HasValue && ((DateTime)o.RECORDDATE).Year <= date.Year && ((DateTime)o.RECORDDATE).Month <= date.Month);
            return (from o in unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet
                    join b in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on o.FEENO equals b.FEENO
                    where b.ORGID == orgId && (o.PIPELINENAME == "002" || o.PIPELINENAME == "003") && o.RECORDDATE.HasValue
                    && ((DateTime)o.RECORDDATE).Year <= date.Year && ((DateTime)o.RECORDDATE).Month <= date.Month
                    select 0).Count();
        }

        /// <summary>
        /// 获取使用导尿管总人日数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>List</returns>
        public List<TimeDiffEntity> GetUsedPipeDaysTotal(int year, string pipName)
        {
            var q =
                (from o in unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet
                 join b in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on o.FEENO equals b.FEENO
                 where b.ORGID == orgId && (o.PIPELINENAME == pipName) && o.RECORDDATE.HasValue
                 && ((DateTime)o.RECORDDATE).Year == year
                 select new TimeDiffEntity
                 {
                     BeginTime = o.RECORDDATE,
                     EndTime = o.REMOVEDATE,
                 });
            return q.ToList();
        }

        public List<TimeDiffEntity> GetUsedPipeDaysTotal(int year)
        {
            var q =
                (from o in unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet
                 join b in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on o.FEENO equals b.FEENO
                 where b.ORGID == orgId && (o.PIPELINENAME == "002" || o.PIPELINENAME == "003") && o.RECORDDATE.HasValue
                 && ((DateTime)o.RECORDDATE).Year == year
                 select new TimeDiffEntity
                 {
                     BeginTime = o.RECORDDATE,
                     EndTime = o.REMOVEDATE,
                 });
            return q.ToList();
        }

        #region 获取 结案/请假/非计划入院/使用导尿管 住民人日次数
        /// <summary>
        /// 获取 结案/请假/非计划入院 住民人日次数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="list">List集合</param>
        /// <returns>人日次数</returns>
        public int OutLeaveUnPlanTotal(int year, int month, List<TimeDiffEntity> list)
        {
            DateTime minDate, maxDate;
            int Total = 0;

            minDate = new DateTime(year, month, 1);
            maxDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            if (list != null && list.Count > 0)
            {
                foreach (var le in list)
                {
                    if (le.EndTime < minDate || le.BeginTime > maxDate)
                    {
                        Total += 0;
                    }
                    else
                    {
                        if (le.BeginTime < minDate && le.EndTime == null)
                        {
                            Total += DateTime.DaysInMonth(year, month);
                        }
                        else if (le.BeginTime < minDate && le.EndTime > maxDate)
                        {
                            Total += DateTime.DaysInMonth(year, month);
                        }
                        else if (le.BeginTime < minDate && le.EndTime <= maxDate)
                        {
                            DateTime dtThis = new DateTime(Convert.ToInt32(minDate.Year), Convert.ToInt32(minDate.Month), Convert.ToInt32(minDate.Day));
                            DateTime dtLast = new DateTime(Convert.ToInt32(((DateTime)le.EndTime).Year), Convert.ToInt32(((DateTime)le.EndTime).Month), Convert.ToInt32(((DateTime)le.EndTime).Day));
                            Total += new TimeSpan(dtLast.Ticks - dtThis.Ticks).Days;
                        }
                        else if (le.BeginTime >= minDate && le.EndTime == null)
                        {
                            DateTime dtThis = new DateTime(Convert.ToInt32(((DateTime)le.BeginTime).Year), Convert.ToInt32(((DateTime)le.BeginTime).Month), Convert.ToInt32(((DateTime)le.BeginTime).Day));
                            DateTime dtLast = new DateTime(Convert.ToInt32(maxDate.Year), Convert.ToInt32(maxDate.Month), Convert.ToInt32(maxDate.Day));
                            Total += new TimeSpan(dtLast.Ticks - dtThis.Ticks).Days;
                        }
                        else if (le.BeginTime >= minDate && le.EndTime > maxDate)
                        {
                            DateTime dtThis = new DateTime(Convert.ToInt32(((DateTime)le.BeginTime).Year), Convert.ToInt32(((DateTime)le.BeginTime).Month), Convert.ToInt32(((DateTime)le.BeginTime).Day));
                            DateTime dtLast = new DateTime(Convert.ToInt32(maxDate.Year), Convert.ToInt32(maxDate.Month), Convert.ToInt32(maxDate.Day));
                            Total += new TimeSpan(dtLast.Ticks - dtThis.Ticks).Days;
                        }
                        else if (le.BeginTime >= minDate && le.EndTime <= maxDate)
                        {
                            DateTime dtThis = new DateTime(Convert.ToInt32(((DateTime)le.BeginTime).Year), Convert.ToInt32(((DateTime)le.BeginTime).Month), Convert.ToInt32(((DateTime)le.BeginTime).Day));
                            DateTime dtLast = new DateTime(Convert.ToInt32(((DateTime)le.EndTime).Year), Convert.ToInt32(((DateTime)le.EndTime).Month), Convert.ToInt32(((DateTime)le.EndTime).Day));
                            Total += new TimeSpan(dtLast.Ticks - dtThis.Ticks).Days;
                        }
                    }
                }
            }
            else
            {
                Total = 0;
            }
            return Total;
        }
        #endregion
    }
}
