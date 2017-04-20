/*
创建人: 肖国栋
创建日期:2016-03-18
说明:指标管理
*/
using AutoMapper;
using ExcelReport;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using KMHC.SLTC.Repository.Base;
using NPOI.SS.Formula.Functions;
using System.Data.Linq.SqlClient;

namespace KMHC.SLTC.Business.Implement
{
    public partial class ReportManageService : BaseService, IReportManageService
    {
        string orgId = SecurityHelper.CurrentPrincipal.OrgId;
        // 报表中有1到12个月的数据表格的，使用此方法生成表格参数
        private TableFormatter<YearJoinCategory> NewTableFormatter(BaseResponse<List<YearJoinCategory>> response, SheetParameterContainer sheetParameterContainer, int rowIndex, string category)
        {
            for (int i = 1; i <= 12; i++)
            {
                sheetParameterContainer.ParameterList.Add(new Parameter() { Name = string.Format("{0}_{1}_{2}", category, rowIndex, i), RowIndex = rowIndex, ColumnIndex = i });
            }
            var data = response.Data.FindAll(it => it.Category == category);
            if (data.Count > 0)
            {
                return new TableFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 1)], data,
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 1)], t => t.January),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 2)], t => t.February),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 3)], t => t.March),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 4)], t => t.April),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 5)], t => t.May),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 6)], t => t.June),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 7)], t => t.July),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 8)], t => t.August),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 9)], t => t.September),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 10)], t => t.October),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 11)], t => t.November),
                        new CellFormatter<YearJoinCategory>(sheetParameterContainer[string.Format("{0}_{1}_{2}", category, rowIndex, 12)], t => t.December)
                );
            }
            else
            {
                return null;
            }
        }

        // 生成单元格参数
        private void GenerateFormatter(BaseResponse<IList<REGQUESTION>> response, List<ElementFormatter> elementFormatters, TableParam tableParam)
        {
            int sum = 0;
            foreach (var item in response.Data)
            {
                foreach (var p in tableParam.Cells)
                {
                    int rowIndex = p.RowIndex;
                    int colIndex = p.ColumnIndex;
                    if (tableParam.FillType == EnumFillType.Horizontal)
                    {
                        rowIndex = rowIndex + sum;
                    }
                    else if (tableParam.FillType == EnumFillType.Vertical)
                    {
                        colIndex = colIndex + sum;
                    }
                    string name = string.Format("{0}_{1}_{2}", p.FieldName, rowIndex, colIndex);
                    var newParam = new Parameter() { Name = name, RowIndex = rowIndex, ColumnIndex = colIndex };
                    elementFormatters.Add(new CellFormatter(newParam, item.GetType().GetProperty(p.FieldName).GetValue(item, null)));
                }
                sum++;
            }
        }

        #region H20非计画住院
        private BaseResponse<List<YearJoinCategory>> GetUnPlanedIpdStatisticsData(int year, bool h72Ipd, bool UNPLANFLAG)
        {
            BaseResponse<List<YearJoinCategory>> response = new BaseResponse<List<YearJoinCategory>>();
            var dbSet = unitOfWork.GetRepository<LTC_UNPLANEDIPD>().dbSet.Where(o => o.ORGID == orgId && o.INDATE.HasValue).AsQueryable();
            if (h72Ipd)
            {
                dbSet = dbSet.Where(it => it.H72IPD == true);
            }
            if (UNPLANFLAG)
            {
                dbSet = dbSet.Where(it => it.UNPLANFLAG == true);
            }
            var q = (from a in dbSet
                     let Year = SqlFunctions.DatePart("Year", (DateTime)a.INDATE)
                     let Month = SqlFunctions.DatePart("Month", (DateTime)a.INDATE)
                     group a by new { a.IPDCAUSE, Year, Month } into g
                     where g.Key.Year == year
                     select new StatisticItem { Category = g.Key.IPDCAUSE, Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() });

            var list = q.ToList().GroupBy(it => it.Category).ToList();

            response.Data = new List<YearJoinCategory>();

            list.ForEach(it =>
            {
                var item = new YearJoinCategory() { Category = it.Key };
                item.SetYearJoinCategory(it.ToList());
                response.Data.Add(item);
            });

            return response;
        }

        /// <summary>
        /// 获取当月新住民人次
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>List</returns>
        private BaseResponse<List<YearJoinCategory>> GetNewResidentTotal(int year)
        {
            BaseResponse<List<YearJoinCategory>> response = new BaseResponse<List<YearJoinCategory>>();
            var dbSet = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(o => o.ORGID == orgId && (o.IPDFLAG == "I") && o.INDATE.HasValue && ((DateTime)o.INDATE).Year <= year).AsQueryable();
            var q = (from a in dbSet
                     let Year = SqlFunctions.DatePart("Year", (DateTime)a.INDATE)
                     let Month = SqlFunctions.DatePart("Month", (DateTime)a.INDATE)
                     group a by new { Year, Month } into g
                     where g.Key.Year == year
                     select new StatisticItem { Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() });
            var list = q.ToList();
            response.Data = new List<YearJoinCategory>();

            var item = new YearJoinCategory() { Category = "" };
            item.SetYearJoinCategory(list);
            response.Data.Add(item);
            return response;
        }

        /// <summary>
        /// 获取当月住民总人次
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>List</returns>
        private BaseResponse<List<YearJoinCategory>> GetResidentTotal(int year)
        {
            BaseResponse<List<YearJoinCategory>> response = new BaseResponse<List<YearJoinCategory>>();
            var dbSet = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(o => o.ORGID == orgId && (o.IPDFLAG == "I") && o.INDATE.HasValue && ((DateTime)o.INDATE).Year <= year).AsQueryable();
            var q = (from a in dbSet
                     let Year = SqlFunctions.DatePart("Year", (DateTime)a.INDATE)
                     let Month = SqlFunctions.DatePart("Month", (DateTime)a.INDATE)
                     group a by new { Year, Month } into g
                     where g.Key.Year == year
                     select new StatisticItem { Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() });
            var list = q.ToList();
            response.Data = new List<YearJoinCategory>();

            var item = new YearJoinCategory() { Category = "" };
            item.SetYearJoinCategory(list);
            response.Data.Add(item);
            return response;
        }

        /// <summary>
        /// H20非计划住院
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="templateFile">文件模板路径</param>
        public void DownloadUnPlanedIpdStatisticsFile(int year, string templateFile)
        {
            var response = this.GetUnPlanedIpdStatisticsData(year, true, false);
            var response1 = this.GetUnPlanedIpdStatisticsData(year, false, true);
            var newResponse = this.GetNewResidentTotal(year);
            var newResponse1 = this.GetResidentTotal(year);
            //var workbookParameterContainer = new WorkbookParameterContainer();
            //workbookParameterContainer.Load(@"Template\Template.xml");
            SheetParameterContainer sheetParameterContainer = new SheetParameterContainer() { SheetName = "年统计表" };

            dynamic[] categorys = new dynamic[] { 
                new { category = "001", rowIndex = 5 },
                new { category = "002", rowIndex = 7 }, 
                new { category = "003", rowIndex = 9 }, 
                new { category = "004", rowIndex = 11 },
                new { category = "005", rowIndex = 13 }
            };
            dynamic[] categorys1 = new dynamic[] { 
                new { category = "001", rowIndex = 21 },
                new { category = "002", rowIndex = 23 }, 
                new { category = "003", rowIndex = 25 }, 
                new { category = "004", rowIndex = 27 },
                new { category = "005", rowIndex = 29 }
            };
            List<ElementFormatter> elementFormatters = new List<ElementFormatter>();
            // 生成年份参数
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "year", RowIndex = 0, ColumnIndex = 3 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["year"], year));
            for (int i = 0; i < categorys.Length; i++)
            {
                var item = categorys[i];
                var item1 = categorys1[i];
                // 生成72小时分类参数
                elementFormatters.Add(NewTableFormatter(response, sheetParameterContainer, item.rowIndex, item.category));
                // 生成当月分类参数
                elementFormatters.Add(NewTableFormatter(response1, sheetParameterContainer, item1.rowIndex, item1.category));
            }
            // 生成72小时新入住参数
            elementFormatters.Add(NewTableFormatter(newResponse, sheetParameterContainer, 2, ""));
            // 生成当月新入住参数
            // elementFormatters.Add(NewTableFormatter(newResponse1, sheetParameterContainer, 18, ""));

            #region 当月住民总人次
            int row = 18, col = 0;
            int value = 0;
            int queryMonth = 0;


            var dbSet = unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(o => o.ORGID == orgId && (o.IPDFLAG == "I") && o.INDATE.HasValue && ((DateTime)o.INDATE).Year == year).AsQueryable();
            var q = (from a in dbSet
                     let Year = SqlFunctions.DatePart("Year", (DateTime)a.INDATE)
                     let Month = SqlFunctions.DatePart("Month", (DateTime)a.INDATE)
                     group a by new { Year, Month } into g
                     where g.Key.Year == year
                     select new StatisticItem { Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() });
            var list = q.ToList();
            int maxMonth = 0;
            if (list != null && list.Count > 0)
            {
                maxMonth = list.OrderByDescending(s => s.Month).FirstOrDefault().Month;
            }

            for (int j = 1; j <= 12; j++)
            {
                value = 0;
                col = j;
                if (col > maxMonth)
                {
                    queryMonth = 0;
                }
                else
                {
                    queryMonth = col;
                }

                value =
                    unitOfWork.GetRepository<LTC_IPDREG>()
                        .dbSet.Count(
                            o =>
                                o.ORGID == orgId && (o.IPDFLAG == "I") && o.INDATE.HasValue &&
                                ((DateTime)o.INDATE).Year == year && ((DateTime)o.INDATE).Month <= queryMonth);

                elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("{0}_{1}", row, col), RowIndex = row, ColumnIndex = col }, value));
            }
            #endregion

            ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("年统计表", elementFormatters.Where(it => it != null).ToArray()));
        }
        #endregion

        #region H50约束
        public List<ConstraintRec> GetConstraintList(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            var q = (from a in unitOfWork.GetRepository<LTC_CONSTRAINTREC>().dbSet.Where(o => o.ORGID == orgId && o.STARTDATE.HasValue && o.CANCELDATE.HasValue)
                     let a_Year = SqlFunctions.DatePart("Year", (DateTime)a.STARTDATE)
                     let b_Year = SqlFunctions.DatePart("Year", (DateTime)a.CANCELDATE)
                     let a_Month = SqlFunctions.DatePart("Month", (DateTime)a.STARTDATE)
                     let b_Month = SqlFunctions.DatePart("Month", (DateTime)a.CANCELDATE)
                     where (a_Year == year && a_Month == month) || (b_Year == year && b_Month == month)
                     select new ConstraintRec { ExecReason = a.EXECREASON, Duration = a.DURATION, StartDate = a.STARTDATE, CancelDate = a.CANCELDATE, ConstraintWayCnt = a.CONSTRAINTWAYCNT, Cancel24Flag = a.CANCEL24FLAG ?? false });
            return q.ToList();
        }

        /// <summary>
        /// H50约束报表
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="templateFile">文件路径</param>
        public void DownloadH50(int year, string templateFile)
        {
            var q = (from a in unitOfWork.GetRepository<LTC_CONSTRAINTREC>().dbSet.Where(o => o.ORGID == orgId && o.STARTDATE.HasValue && o.CANCELDATE.HasValue)
                     let a_Year = SqlFunctions.DatePart("Year", (DateTime)a.STARTDATE)
                     let b_Year = SqlFunctions.DatePart("Year", (DateTime)a.CANCELDATE)
                     where a_Year == year || b_Year == year
                     select new { ExecReason = a.EXECREASON, Duration = a.DURATION, StartDate = a.STARTDATE, CancelDate = a.CANCELDATE, ConstraintWayCnt = a.CONSTRAINTWAYCNT, Cacel24Flag = a.CANCEL24FLAG ?? false });

            var list = q.ToList();

            var q1 = (from b in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.AsQueryable().Where(o => o.ORGID == orgId && o.IPDFLAG == "I" && o.INDATE.HasValue)
                      let a_Year = SqlFunctions.DatePart("Year", (DateTime)b.INDATE)
                      where a_Year == year
                      select new { INDATE = b.INDATE });
            var ipdList = q1.ToList();

            dynamic[] execReasons = new dynamic[] { 
                new { ExecReason = "001", rowIndex = 5 },
                new { ExecReason = "002", rowIndex = 7 }, 
                new { ExecReason = "003", rowIndex = 9 }, 
                new { ExecReason = "004", rowIndex = 11 },
                new { ExecReason = "005", rowIndex = 13 },
                new { ExecReason = "006", rowIndex = 15 },
            };
            dynamic[] durations = new dynamic[] { 
                new { Duration = "001", rowIndex = 17 },
                new { Duration = "002", rowIndex = 19 }, 
                new { Duration = "003", rowIndex = 21 }, 
                new { Duration = "004", rowIndex = 23 },
                new { Duration = "005", rowIndex = 25 }
            };
            List<ElementFormatter> elementFormatters = new List<ElementFormatter>();
            // 生成年份参数
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "year", RowIndex = 0, ColumnIndex = 1 }, year));
            #region 表格参数
            int row = 0, col = 0;
            DateTime minDate, maxDate;
            int value = 0;
            for (int i = 0; i < execReasons.Length; i++)
            {
                var item = execReasons[i];
                row = item.rowIndex;
                for (int j = 1; j <= 12; j++)
                {
                    col = j;
                    minDate = new DateTime(year, j, 1);
                    maxDate = new DateTime(year, j, DateTime.DaysInMonth(year, j));
                    value = list.Count(it => it.ExecReason == item.ExecReason &&
                        ((it.StartDate < minDate && it.CancelDate > minDate) ||
                        (it.StartDate >= minDate && it.StartDate <= maxDate)));
                    elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("{0}_{1}", row, col), RowIndex = row, ColumnIndex = col }, value));
                }
            }

            for (int i = 0; i < durations.Length; i++)
            {
                var item = durations[i];
                row = item.rowIndex;
                for (int j = 1; j <= 12; j++)
                {
                    col = j;
                    minDate = new DateTime(year, j, 1);
                    maxDate = new DateTime(year, j, DateTime.DaysInMonth(year, j));
                    value = list.Count(it => it.Duration == item.Duration &&
                        ((it.StartDate < minDate && it.CancelDate > minDate) ||
                        (it.StartDate >= minDate && it.StartDate <= maxDate)));
                    elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("{0}_{1}", row, col), RowIndex = row, ColumnIndex = col }, value));
                }
            }
            #endregion

            #region 多重身体约束(二种以上)住民人数
            row = 27;
            for (int j = 1; j <= 12; j++)
            {
                col = j;
                minDate = new DateTime(year, j, 1);
                maxDate = new DateTime(year, j, DateTime.DaysInMonth(year, j));
                value = list.Count(it => it.ConstraintWayCnt == "002" &&
                    ((it.StartDate < minDate && it.CancelDate > minDate) ||
                    (it.StartDate >= minDate && it.StartDate <= maxDate)));
                elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("{0}_{1}", row, col), RowIndex = row, ColumnIndex = col }, value));
            }
            #endregion

            #region 约束移除成功率(移除约束至少维持24小时以上)之住民人数
            row = 29;
            for (int j = 1; j <= 12; j++)
            {
                col = j;
                minDate = new DateTime(year, j, 1);
                maxDate = new DateTime(year, j, DateTime.DaysInMonth(year, j));
                value = list.Count(it => it.Cacel24Flag &&
                    ((it.StartDate < minDate && it.CancelDate > minDate) ||
                    (it.StartDate >= minDate && it.StartDate <= maxDate)));
                elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("{0}_{1}", row, col), RowIndex = row, ColumnIndex = col }, value));
            }
            #endregion

            #region 当月住民总人日数
            row = 2;
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

            ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("年统计表", elementFormatters.Where(it => it != null).ToArray()));
        }
        #endregion

        #region  H79外出请假记录表
        /// <summary>
        /// 下载H79 外出请假记录
        /// </summary>
        /// <param name="feeNo">住民编号</param>
        /// <param name="templateFile">文件路径</param>
        public void DownloadH79(long feeNo, string templateFile)
        {
            SheetParameterContainer sheetParameterContainer = new SheetParameterContainer() { SheetName = "Sheet1" };
            List<ElementFormatter> elementFormatters = new List<ElementFormatter>();
            var residend = GetResidentInfo(feeNo);
            if (residend == null) return;
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "BedNo", RowIndex = 2, ColumnIndex = 1 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["BedNo"], residend.BedNo));
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "Name", RowIndex = 2, ColumnIndex = 4 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["Name"], residend.Name));
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "Sex", RowIndex = 2, ColumnIndex = 7 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["Sex"], residend.Sex));

            var leaveShop = unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet.Where(o => o.FEENO == feeNo && o.ORGID == orgId)
               .OrderBy(o => o.SHOWNUMBER).Select(o => new LeaveHosp()
               {
                   ShowNumber = o.SHOWNUMBER,
                   StartDate = o.STARTDATE,
                   ReturnDate = o.RETURNDATE,
                   EndDate = o.ENDDATE,
                   LeHour = o.LEHOUR,
                   LeNote = o.LENOTE,
                   ContName = o.CONTNAME,
                   ContTel = o.CONTTEL,
               }).ToList();

            leaveShop = leaveShop.GetRange(0, leaveShop.Count > 1000 ? 1000 : leaveShop.Count);

            if (leaveShop.Count <= 0)
            {
                return;
            }

            for (var row = 0; row < leaveShop.Count; row++)
            {
                var leNoteContent = string.Empty;
                var leNote = leaveShop[row].LeNote;
                if (!string.IsNullOrEmpty(leNote))
                {
                    var codeFile = unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Where(o => o.ITEMTYPE == "A00.048" && o.ITEMCODE == leNote)
                                   .OrderBy(o => o.ITEMCODE).Select(o => new CodeDtl()
                                   {
                                       ITEMCODE = o.ITEMCODE,
                                       ITEMTYPE = o.ITEMTYPE,
                                       ITEMNAME = o.ITEMNAME,
                                   }).ToList();
                    leNoteContent = codeFile.FirstOrDefault().ITEMNAME;
                }
                else
                {
                    leNoteContent = string.Empty;
                }

                for (var col = 0; col < 8; col++)
                {
                    sheetParameterContainer.ParameterList.Add(new Parameter() { Name = string.Format("{0}_{1}", col, row), RowIndex = row + 4, ColumnIndex = col });

                    switch (col)
                    {
                        case 0:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], leaveShop[row].ShowNumber));
                            break;
                        case 1:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], leaveShop[row].StartDate.HasValue ? Convert.ToDateTime(leaveShop[row].StartDate).ToString() : ""));
                            break;
                        case 2:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], leaveShop[row].EndDate.HasValue ? Convert.ToDateTime(leaveShop[row].EndDate).ToString() : ""));
                            break;
                        case 3:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], leaveShop[row].StartDate.HasValue ? Convert.ToDateTime(leaveShop[row].StartDate).ToString() : ""));
                            break;
                        case 4:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], leaveShop[row].LeHour));
                            break;
                        case 5:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], leNoteContent));
                            break;
                        case 6:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], leaveShop[row].ContName));
                            break;
                        case 7:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], leaveShop[row].ContTel));
                            break;
                    }
                }
            }


            ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("Sheet1", elementFormatters.Where(it => it != null).ToArray()));
        }
        #endregion


        #region H77跌倒危险因子评估
        public void DownloadH77(long feeNo, string templateFile, DateTime? startDate, DateTime? endDate)
        {
            SheetParameterContainer sheetParameterContainer = new SheetParameterContainer() { SheetName = "Sheet1" };
            List<ElementFormatter> elementFormatters = new List<ElementFormatter>();
            var residend = GetResidentInfo(feeNo);
            if (residend == null) return;
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "BedNo", RowIndex = 2, ColumnIndex = 1 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["BedNo"], residend.BedNo));
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "Name", RowIndex = 2, ColumnIndex = 4 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["Name"], residend.Name));
            sheetParameterContainer.ParameterList.Add(new Parameter() { Name = "Sex", RowIndex = 2, ColumnIndex = 7 });
            elementFormatters.Add(new CellFormatter(sheetParameterContainer["Sex"], residend.Sex));

            var surveys = unitOfWork.GetRepository<LTC_REGQUESTION>().dbSet.Where(o => o.QUESTIONID == 11 && o.FEENO == feeNo && o.EVALDATE.HasValue && o.EVALDATE >= startDate && o.EVALDATE <= endDate)
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
                return;
            }

            for (var col = 0; col < surveys.Count; col++)
            {
                for (var row = 0; row < 16; row++)
                {
                    sheetParameterContainer.ParameterList.Add(new Parameter() { Name = string.Format("{0}_{1}", col, row), RowIndex = row + 3, ColumnIndex = col + 2 });
                    switch (row)
                    {
                        case 0:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], surveys[col].CreateDate.HasValue ? ((DateTime)surveys[col].CreateDate).ToShortDateString() : ""));
                            break;
                        case 1:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], surveys[col].Score));
                            break;
                        case 3:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 129).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 4:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 130).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 5:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 131).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 6:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 132).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 7:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 133).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 8:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 134).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 9:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 135).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 10:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 136).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 11:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 137).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 12:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 138).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 13:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], answers.Where(o => o.SurveyId == surveys[col].Id && o.Id == 139).Select(o => o.Score).FirstOrDefault()));
                            break;
                        case 14:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], surveys[col].Score));
                            break;
                        case 15:
                            elementFormatters.Add(new CellFormatter(sheetParameterContainer[string.Format("{0}_{1}", col, row)], surveys[col].Result));
                            break;
                    }
                }
            }
        }
        #endregion

        #region P17护理计划
        /// <summary>
        /// P17护理计划
        /// </summary>
        public NSCPLReportView GetNSCPLReportView(int seqNo)
        {
            NSCPLReportView response = new NSCPLReportView();
            var nscplGoal = unitOfWork.GetRepository<LTC_NSCPLGOAL>().dbSet;
            var nscplActivity = unitOfWork.GetRepository<LTC_NSCPLACTIVITY>().dbSet;
            var assessValue = unitOfWork.GetRepository<LTC_ASSESSVALUE>().dbSet;
            var q = (from a in unitOfWork.GetRepository<LTC_NSCPL>().dbSet
                     join b in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on a.REGNO equals b.REGNO
                     join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.EMPNO equals c.EMPNO
                     join o in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals o.ORGID
                     join e in unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Where(o => o.ITEMTYPE == "A00.001") on b.SEX equals e.ITEMCODE into be
                     from bei in be.DefaultIfEmpty()
                     where a.SEQNO == seqNo
                     select new
                     {
                         Org = o.ORGNAME,
                         SeqNo = a.SEQNO,
                         StartDate = a.STARTDATE,
                         EndDate = a.FINISHDATE,
                         RegName = b.NAME,
                         FeeNo = b.RESIDENGNO,
                         Sex = bei != null ? bei.ITEMNAME : "",
                         Brithdate = b.BRITHDATE,
                         EmpName = c.EMPNAME,
                         CpLevel = a.CPLEVEL,
                         CpDiag = a.CPDIAG,
                         CpReason = a.CPREASON,
                         NsDesc = a.NSDESC,
                         CpResult = a.CPRESULT,
                         TotalDays = a.TOTALDAYS,
                         NscplGoalList = nscplGoal.Where(it => it.SEQNO == a.SEQNO),
                         NscplActivityList = nscplActivity.Where(it => it.SEQNO == a.SEQNO),
                         AssessValueList = assessValue.Where(it => it.SEQNO == a.SEQNO)
                     });
            var nscpl = q.FirstOrDefault();
            if (nscpl != null)
            {
                response.Org = nscpl.Org;
                response.SeqNo = nscpl.SeqNo;
                response.StartDate = nscpl.StartDate.HasValue ? Convert.ToDateTime(nscpl.StartDate.Value).ToString("yyyy-MM-dd") : "";
                response.EndDate = nscpl.EndDate.HasValue ? Convert.ToDateTime(nscpl.EndDate.Value).ToString("yyyy-MM-dd") : "";
                response.RegName = nscpl.RegName;
                response.FeeNo = nscpl.FeeNo;
                response.Sex = nscpl.Sex;
                int year = DateTime.Now.Year;
                response.Age = nscpl.Brithdate.HasValue ? (year - nscpl.Brithdate.Value.Year).ToString() : "";
                response.EmpName = nscpl.EmpName;
                response.CpLevel = nscpl.CpLevel;

                int cpDiagNo = Convert.ToInt32(nscpl.CpDiag);
                var cpdiagtxt = unitOfWork.GetRepository<CARE_PLANPROBLEM>().dbSet.Where(o => o.CP_NO == cpDiagNo).ToList().FirstOrDefault().DIAPR;
                response.CpDiag = cpdiagtxt;
                string[] arr = nscpl.CpReason.Split('\\', 'n');
                response.CpReason = AppendN(nscpl.CpReason, 2 - arr.Length);
                response.NsDesc = nscpl.NsDesc;
                response.CpResult = nscpl.CpResult;
                response.TotalDays = nscpl.TotalDays.HasValue ? nscpl.TotalDays.Value.ToString() : "";
                var tmpNscplGoalList = nscpl.NscplGoalList.ToList();
                foreach (var item in tmpNscplGoalList)
                {
                    string tmp = item.FINISHFLAG ?? false ? ("完成时间" + (item.FINISHDATE.HasValue ? "(" + Util.ToTwDate(item.FINISHDATE.Value) + ")" : "")) : item.UNFINISHREASON;
                    response.NscplGoal += string.Format("\r{0} \r{1} {2}\n", "【目标】", item.CPLGOAL, tmp);
                }
                if (!string.IsNullOrEmpty(response.NscplGoal))
                {
                    response.NscplGoal = response.NscplGoal.TrimEnd('\\', 'n');
                }
                response.NscplGoal = AppendN(response.NscplGoal, 3 - tmpNscplGoalList.Count());
                var tmpNscplActivityList = nscpl.NscplActivityList.ToList();
                foreach (var item in tmpNscplActivityList)
                {
                    string tmp = item.FINISHFLAG ?? false ? ("完成时间" + (item.FINISHDATE.HasValue ? "(" + Util.ToTwDate(item.FINISHDATE.Value) + ")" : "")) : item.UNFINISHREASON;
                    response.NscplActivity += string.Format("{0} {1}\n", item.CPLACTIVITY, tmp);
                }
                if (!string.IsNullOrEmpty(response.NscplActivity))
                {
                    response.NscplActivity = response.NscplActivity.TrimEnd('\\', 'n');
                }
                response.NscplActivity = AppendN(response.NscplActivity, 9 - tmpNscplActivityList.Count());
                var tmpAssessValueList = nscpl.AssessValueList.ToList();
                foreach (var item in nscpl.AssessValueList)
                {
                    response.AssessValue += string.Format("{0} {1}\n", Util.ToTwDate(item.RECDATE.Value), item.VALUEDESC);
                }

                if (!string.IsNullOrEmpty(response.AssessValue))
                {
                    response.AssessValue = response.AssessValue.TrimEnd('\\', 'n');
                }

                response.AssessValue = AppendN(response.AssessValue, 9 - tmpAssessValueList.Count());
            }
            return response;
        }
        #endregion

        #region P19给药记录单
        public void DownloadP19(long seqNo, string templateFile)
        {
            var t_vr = unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().dbSet;
            var t_vp = unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().dbSet;
            var t_m = unitOfWork.GetRepository<LTC_MEDICINE>().dbSet;
            var t_d = unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet;

            var t_vh = unitOfWork.GetRepository<LTC_VISITHOSPITAL>().dbSet;
            var t_vd = unitOfWork.GetRepository<LTC_VISITDEPT>().dbSet;
            var t_vdr = unitOfWork.GetRepository<LTC_VISITDOCTOR>().dbSet;

            var t_vdd = unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet;
            var q_vr = (from vr in t_vr
                        join vh in t_vh on vr.VISITHOSP equals vh.HOSPNO into v_vrvh
                        join vd in t_vd on vr.VISITDEPT equals vd.DEPTNO into v_vrvd
                        join vdr in t_vdr on vr.VISITDOCTOR equals vdr.DOCNAME into v_vrvdr
                        from vrvh in v_vrvh.DefaultIfEmpty()
                        from vrvd in v_vrvd.DefaultIfEmpty()
                        from vrvdr in v_vrvdr.DefaultIfEmpty()
                        where vr.SEQNO == seqNo
                        select new { vr, vrvh.HOSPNAME, vrvd.DEPTNAME, vrvdr.DOCNAME });

            var q_vp = (from vp in t_vp
                        where vp.SEQNO == seqNo
                        select vp);

            var q_m = (from m in t_m
                       join vp in t_vp on m.MEDID equals vp.MEDID
                       where vp.SEQNO == seqNo
                       select m);

            var q_d = (from d in t_d
                       join vp in t_vp on d.FREQNO equals vp.FREQ
                       where vp.SEQNO == seqNo
                       select d);

            var item_vr = q_vr.FirstOrDefault();
            var list_vp = q_vp.ToList();
            var list_m = q_m.ToList();
            var list_d = q_d.ToList();

            IResidentManageService residentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            var residentInfo = this.GetResidentInfo(item_vr.vr.FEENO ?? 0);
            var org = organizationManageService.GetOrg(item_vr.vr.ORGID);

            List<ElementFormatter> elementFormatters = new List<ElementFormatter>();
            // 设置公司名称
            elementFormatters.Add(new PartFormatter(new Parameter() { Name = "Org", RowIndex = 0, ColumnIndex = 0 }, residentInfo.Org));
            // 设置时间段
            if (item_vr.vr.STARTDATE.HasValue && item_vr.vr.ENDDATE.HasValue)
            {
                elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n0_30", RowIndex = 0, ColumnIndex = 30 }, string.Format("{0}～{1}", Util.ToTwDate(item_vr.vr.STARTDATE.Value), Util.ToTwDate(item_vr.vr.ENDDATE))));
            }
            else
            {
                elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n0_30", RowIndex = 0, ColumnIndex = 30 }, ""));
            }
            // 姓名
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n1_1", RowIndex = 1, ColumnIndex = 1 }, residentInfo.Name));
            // 收案编号
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n1_3", RowIndex = 1, ColumnIndex = 3 }, residentInfo.ResidengNo));
            // 诊断别
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n1_6", RowIndex = 1, ColumnIndex = 6 }, residentInfo.DiseaseDiag));
            // 性别
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n2_1", RowIndex = 2, ColumnIndex = 1 }, residentInfo.Sex));
            // 年龄
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n2_3", RowIndex = 2, ColumnIndex = 3 }, residentInfo.Age));
            // 体重
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n3_1", RowIndex = 3, ColumnIndex = 1 }, string.Format("{0} kg", residentInfo.Weight)));
            // 区域寝室
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n3_3", RowIndex = 3, ColumnIndex = 3 }, string.Format("{0}/{1}", residentInfo.Floor, residentInfo.RoomNo)));
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n3_4", RowIndex = 3, ColumnIndex = 4 }, residentInfo.BedNo));
            //过敏资料
            var careDemand = QueryLatestCareDemand(item_vr.vr.FEENO ?? 0).Data;
            if (careDemand != null)
            {
                elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n2_6", RowIndex = 2, ColumnIndex = 6 }, string.Format("药物过敏史:{0} 食物过敏:{1} 其他过敏:{2}", careDemand.ALLERGY_DRUG ?? "无", careDemand.ALLERGY_FOOD ?? "无", careDemand.ALLERGY_OTHERS ?? "无")));

            }
            else
            {
                elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n2_6", RowIndex = 2, ColumnIndex = 6 }, "药物过敏史:无 食物过敏:无 其他过敏:无"));

            }

            // 小计
            string value = string.Format("本案药品数量小计：{0}", list_vp.Count);
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n18_0", RowIndex = 18, ColumnIndex = 0 }, value));

            // 生成天数和星期
            if (item_vr.vr.STARTDATE.HasValue && item_vr.vr.ENDDATE.HasValue)
            {
                int startDay = item_vr.vr.STARTDATE.Value.Day;
                int startWeek = (int)item_vr.vr.STARTDATE.Value.DayOfWeek;
                int startMaxDay = DateTime.DaysInMonth(item_vr.vr.STARTDATE.Value.Year, item_vr.vr.STARTDATE.Value.Month);
                int interval = item_vr.vr.ENDDATE.Value.Subtract(item_vr.vr.STARTDATE.Value).Days;
                for (int i = 0; i < 31; i++)
                {
                    if (i > interval)
                    {
                        break;
                    }
                    int col = 6 + i;
                    int nValue = startDay + i;
                    if (nValue > startMaxDay)
                    {
                        nValue = nValue - startMaxDay;
                    }
                    elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("d{0}_{1}", 0, 0), RowIndex = 5, ColumnIndex = col }, nValue));
                    nValue = startWeek + i;
                    nValue = nValue % 7;
                    nValue = nValue == 0 ? 7 : nValue;
                    elementFormatters.Add(new CellFormatter(new Parameter() { Name = string.Format("w{0}_{1}", 0, 0), RowIndex = 6, ColumnIndex = col }, "W" + nValue));
                }
            }

            List<Visit> list = new List<Visit>();
            foreach (var item in list_vp)
            {
                Visit newItem = new Visit();
                var sub_d = list_d.Find(it => it.FREQNO == item.FREQ);
                var sub_m = list_m.Find(it => it.MEDID == item.MEDID);
                var sub_p = list_vp.Find(it => it.MEDID == item.MEDID);
                // 药品说明
                value = "";
                if (sub_m != null)
                {
                    value += string.Format("{0}\n", sub_m.ENGNAME);
                    value += string.Format("{0}\n", sub_m.CHNNAME);
                }
                if (!string.IsNullOrEmpty(item_vr.HOSPNAME) || !string.IsNullOrEmpty(item_vr.DEPTNAME) || !string.IsNullOrEmpty(item_vr.DOCNAME))
                {
                    value += string.Format("{0}{1}{2}\n", string.IsNullOrEmpty(item_vr.HOSPNAME) ? "" : item_vr.HOSPNAME.Trim(), string.IsNullOrEmpty(item_vr.DEPTNAME) ? "" : item_vr.DEPTNAME.Trim(), string.IsNullOrEmpty(item_vr.DOCNAME) ? "" : item_vr.DOCNAME.Trim());
                }
                if (sub_p != null)
                {
                    if (!string.IsNullOrEmpty(sub_p.STARTDATE.ToString()) && !string.IsNullOrEmpty(sub_p.ENDDATE.ToString()))
                    {
                        value += string.Format("{0}～{1}\n", Util.ToTwDate(sub_p.STARTDATE, "HourMin"), Util.ToTwDate(sub_p.ENDDATE, "HourMin"));
                    }
                }
                if (sub_m != null)
                {
                    value += sub_m.USEDESC + "\n";
                    value += sub_m.SIDEEFFECT + "\n";
                }
                //value += string.Format("{0} {1}\n", sub_m.MEDIFACADE, sub_m.MEDICOLOR);
                //value += item_vr.vr.DISEASENAME;
                newItem.Description = value;
                // 用法、用量
                value = "";
                value += string.Format("{0}\n", item.FREQ);
                if (sub_d != null)
                {
                    value += string.Format("{0}\n", sub_d.FREQNAME);
                    value += string.Format("每{0}天{1}次\n", sub_d.FREQDAY, sub_d.FREQQTY);
                }
                value += string.Format("每次{0}{1}/{2}\n", item.QTY, item.DOSAGE, item.TAKEQTY);
                if (sub_m != null)
                {
                    value += string.Format("{0} {1}\n", sub_m.MEDIFACADE, sub_m.MEDICOLOR);
                }
                newItem.Name = value;
                // 本日给药总量
                value = "";
                if (sub_d != null)
                {
                    decimal total = sub_d.FREQQTY ?? 0 * item.QTY ?? 0;
                    value += string.Format("{0} {1}\n", total, item.DOSAGE);
                    value += string.Format("{0}\n", sub_d.FREQTIME);
                }

                newItem.Contrel = value;
                list.Add(newItem);
            }

            /*
            // 药品说明
            value = "";
            foreach (var item in list_m)
            {
                value += string.Format("{0}\n", item.ENGNAME);
                value += string.Format("{0}\n", item.CHNNAME);
            }
            value += string.Format("{0},{1},{2}\n", item_vr.HOSPNAME, item_vr.DEPTNAME, item_vr.DOCNAME);
            value += string.Format("{0}~{1}\n", Util.ToTwDate(item_vr.vr.STARTDATE), Util.ToTwDate(item_vr.vr.ENDDATE));
            value += item_vr.vr.DISEASENAME;
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n7_0", RowIndex = 7, ColumnIndex = 0, WrapText = true }, value.TrimEnd('\\', 'n')));

            // 用法、用量
            value = "";
            foreach (var item in list_vp)
            {
                value += string.Format("{0}\n", item.FREQ);
                var sub = list_d.Find(it => it.FREQNO == item.FREQ);
                value += string.Format("{0}\n", sub.FREQNAME);
                value += string.Format("每{0}天{1}次\n", sub.FREQDAY, sub.FREQQTY);
                value += string.Format("每次{0}{1}/{2}\n", item.QTY, item.DOSAGE, item.TAKEQTY);
                var sub1 = list_m.Find(it => it.MEDID == item.MEDID);
                value += string.Format("{0} {1}\n", sub1.MEDIFACADE, sub1.MEDICOLOR);
            }
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n7_2", RowIndex = 7, ColumnIndex = 2, WrapText = true }, value.TrimEnd('\\', 'n')));

            // 本日给药总量
            value = "";
            foreach (var item in list_vp)
            {
                var sub = list_d.Find(it => it.FREQNO == item.FREQ);
                decimal total = sub.FREQQTY ?? 0 * item.QTY ?? 0;
                value += string.Format("{0} {1}\n", total, item.DOSAGE);
                value += string.Format("{0}\n", sub.FREQTIME);
            }
            elementFormatters.Add(new CellFormatter(new Parameter() { Name = "n7_4", RowIndex = 7, ColumnIndex = 4, WrapText = true }, value.TrimEnd('\\', 'n')));
            */
            elementFormatters.Add(new RepeaterFormatter<Visit>(
                new Parameter() { Name = "rpt_Start", RowIndex = 7, ColumnIndex = 0 },
                new Parameter() { Name = "rpt_End", RowIndex = 14, ColumnIndex = 0 },
                list,
                new CellFormatter<Visit>(new Parameter() { Name = "rpt_c1", RowIndex = 8, ColumnIndex = 0, WrapText = true }, it => it.Description),
                new CellFormatter<Visit>(new Parameter() { Name = "rpt_c2", RowIndex = 8, ColumnIndex = 2, WrapText = true }, it => it.Name),
                new CellFormatter<Visit>(new Parameter() { Name = "rpt_c3", RowIndex = 8, ColumnIndex = 4, WrapText = true }, it => it.Contrel)
            ));

            ExportHelper.ExportToWeb(templateFile, Path.GetFileName(templateFile), new SheetFormatter("给药记录单", elementFormatters.Where(it => it != null).ToArray()));
        }

        public BaseResponse<CareDemandEval> QueryLatestCareDemand(long feeNo)
        {
            BaseResponse<CareDemandEval> response = new BaseResponse<CareDemandEval>();
            Mapper.CreateMap<LTC_CAREDEMANDEVAL, CareDemandEval>();
            LTC_CAREDEMANDEVAL demand = unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet.Where(m => m.FEENO == feeNo).OrderByDescending(x => x.EVALDATE).FirstOrDefault();

            response.Data = Mapper.Map<CareDemandEval>(demand);
            return response;
        }
        #endregion

        public string AppendN(string input, int count)
        {
            for (int i = 0; i < count; i++)
            {
                input = input + "\n";
            }
            return input;
        }

        /// <summary>
        /// 公元年份转民国年份
        /// </summary>
        /// <param name="year"></param>
        /// <param name="isDateFormat"></param>
        /// <returns></returns>
        public string ADToMinYear(int year, bool isDateFormat)
        {
            string minYear = string.Empty;
            if (isDateFormat)
            {
                minYear += "民国";
            }
            minYear += year.ToString();
            if (isDateFormat)
            {
                minYear += "年";
            }
            return minYear;
        }

        public BaseResponse<IList<ReportModel>> QueryReport(BaseRequest<ReportFilter> request)
        {
            var q = from r in unitOfWork.GetRepository<SYS_REPORT>().dbSet
                    select r;

            if (!string.IsNullOrEmpty(request.Data.Code))
            {
                q = q.Where(it => it.CODE == request.Data.Code);
            }
            if (!string.IsNullOrEmpty(request.Data.Name))
            {
                q = q.Where(it => it.NAME == request.Data.Name);
            }
            if (!string.IsNullOrEmpty(request.Data.MajorType))
            {
                q = q.Where(it => it.MAJORTYPE == request.Data.MajorType);
            }
            if (!string.IsNullOrEmpty(request.Data.ReportType))
            {
                q = q.Where(it => it.REPORTTYPE == request.Data.ReportType);
            }
            if (!string.IsNullOrEmpty(request.Data.SysType))
            {
                q = q.Where(it => it.SYSTYPE == request.Data.SysType);
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(it => it.ORGID == request.Data.OrgId);
            }
            if (request.Data.Status.HasValue)
            {
                q = q.Where(it => it.STATUS == request.Data.Status);
            }
            q = q.Where(it => it.CODE != null);
            var response = new BaseResponse<IList<ReportModel>>();
            response.RecordsCount = q.Count();
            q = q.OrderBy(it => new { it.MAJORTYPE, it.NAME });
            List<SYS_REPORT> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }
            Mapper.CreateMap<SYS_REPORT, ReportModel>();
            response.Data = Mapper.Map<IList<ReportModel>>(list);
            return response;
        }

        public BaseResponse<List<ReportSetModel>> SaveReport(string orgId, List<ReportSetModel> request)
        {
            BaseResponse<List<ReportSetModel>> response = new BaseResponse<List<ReportSetModel>>();
            var reportRepository = unitOfWork.GetRepository<SYS_REPORT>();
            var q = from r in reportRepository.dbSet
                    where r.ORGID == orgId
                    select r;

            var oldData = q.ToList();

            Mapper.CreateMap<ReportModel, SYS_REPORT>();
            request.ForEach(rs =>
            {
                rs.Items.ForEach(m =>
                {
                    var findItem = oldData.Find(it => it.CODE == m.Code);
                    if (findItem != null)
                    {
                        if (findItem.STATUS != m.Status)
                        {
                            findItem.STATUS = m.Status;
                            findItem.ORGID = orgId;
                            reportRepository.Update(findItem);
                        }
                    }
                    else
                    {
                        var model = Mapper.Map<SYS_REPORT>(m);
                        model.ORGID = orgId;
                        reportRepository.Insert(model);
                    }
                });
            });
            unitOfWork.Save();
            response.Data = request;
            return response;
        }

        /// <summary>
        /// 查询已结案人员在院天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>时间参数集合</returns>
        public List<TimeDiffEntity> GetIpdOutTotal(int year)
        {
            var q =
                (from a in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(o => o.ORGID == orgId && o.INDATE.HasValue && o.OUTDATE.HasValue)
                 let a_Year = SqlFunctions.DatePart("Year", (DateTime)a.OUTDATE)
                 let a_Month = SqlFunctions.DatePart("Month", (DateTime)a.OUTDATE)
                 where a_Year == year
                 select new TimeDiffEntity
                 {
                     BeginTime = a.INDATE,
                     EndTime = a.OUTDATE,
                 });
            return q.ToList();
        }

        /// <summary>
        /// 获取请假人员离院天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>时间参数集合</returns>
        public List<TimeDiffEntity> GetLeaveHospTotal(int year)
        {
            var q =
                (from a in unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet.Where(o => o.ORGID == orgId && o.STARTDATE.HasValue)
                 let a_Year = SqlFunctions.DatePart("Year", (DateTime)a.STARTDATE)
                 where a_Year == year
                 select new TimeDiffEntity
                 {
                     BeginTime = a.STARTDATE,
                     EndTime = a.ENDDATE,
                 });
            return q.ToList();
        }

        /// <summary>
        /// 非计划人员住院天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>时间参数集合</returns>
        public List<TimeDiffEntity> GetUnPlanIpdTotal(int year)
        {
            var q =
                (from a in unitOfWork.GetRepository<LTC_UNPLANEDIPD>().dbSet.Where(o => o.ORGID == orgId && o.INDATE.HasValue)
                 let a_Year = SqlFunctions.DatePart("Year", (DateTime)a.INDATE)
                 where a_Year == year
                 select new TimeDiffEntity
                 {
                     BeginTime = a.INDATE,
                     EndTime = a.OUTDATE,
                 });
            return q.ToList();
        }
        #region H35
        public CareDemandEvalPrivew GetCareDemandHis(int id, string orgid)
        {
            CareDemandEvalPrivew response = new CareDemandEvalPrivew();

            var q = (from a in unitOfWork.GetRepository<LTC_CAREDEMANDEVAL>().dbSet
                     join s in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals s.FEENO
                     join bed in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on s.BEDNO equals bed.BEDNO into beds
                     join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on s.FLOOR equals f.FLOORID into ffs
                     join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on s.ROOMNO equals r.ROOMNO into rrs
                     join c in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on s.REGNO equals c.REGNO into ccs
                     join d in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals d.ORGID
                     join b in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.EVALUATEBY equals b.EMPNO into res
                     from bb in beds.DefaultIfEmpty()  //床位数据
                     from ff in ffs.DefaultIfEmpty()   //楼层信息
                     from rr in rrs.DefaultIfEmpty()   // 房间信息
                     from cc in ccs.DefaultIfEmpty()    // 住民信息 
                     from re in res.DefaultIfEmpty()   // 员工信息
                     where a.ID == id
                     select new
                     {
                         Org = d.ORGNAME,
                         orgId = a.ORGID,
                         Name = cc.NAME,
                         Number = cc.RESIDENGNO,
                         Living = ff.FLOORNAME,
                         Dormitory = rr.ROOMNAME,
                         Bedno = bb.BEDNO,
                         Brithdate = cc.BRITHDATE,
                         Age = cc.AGE,
                         Date = a.EVALDATE,
                         Next_date = a.NEXTEVALDATE,
                         Personnel = re.EMPNAME,
                         h35_no = a.ID,
                         Ill_history = a.HEALTHDESC,
                         Carrydisease = a.INFECTIONDESC,
                         v01 = a.BODYTEMP,
                         v02 = a.PULSE,
                         v03 = a.BREATHE,
                         v04 = a.SBP,
                         v05 = a.DBP,
                         v90 = a.LIGHTREFLECTION,
                         v06 = a.HEIGHT,
                         v07 = a.WEIGHT,
                         vs08 = a.CONSCIOUSNESS,
                         v08 = a.CONSCIOUSNESS_E,
                         v09 = a.CONSCIOUSNESS_V,
                         v10 = a.CONSCIOUSNESS_M,
                         v11 = a.APPEARANCEDESC,
                         v12 = a.ATTITUDE,
                         v91 = a.HEARTBEAT,
                         v92 = a.LIMBEDEMA_RH,
                         v93 = a.LIMBEDEMA_LH,
                         v94 = a.LIMBEDEMA_RF,
                         v95 = a.LIMBEDEMA_LF,
                         v13 = a.BREATEDESC,
                         v14 = a.SECRETIONDESC,
                         v15 = a.SECRETIONNATURE,
                         v16 = a.SECRETIONAMT,
                         v17 = a.COUGHDESC,
                         v18 = a.BREATHAIDTOOLS,
                         v19 = a.SMOKINGHISTORY,
                         v20 = a.EATTYPE,
                         v21 = a.DIETTYPEDESC,
                         vs22 = a.FOODINTAKEAMT,
                         v22 = a.WATERAMT,
                         v23 = a.WATERENOUGHAMT,
                         v24 = a.ORALMUCOSA,
                         v25 = a.DENTUREFIXED,
                         v26 = a.DENTUREMOVABLE,
                         v27 = a.DENTURESAFETY,
                         v28 = a.DENTUREFIT,
                         v29 = a.DENTUREHEALTH,
                         v96 = a.MOUTHPAIN,
                         v97 = a.TOOTHDESC,
                         v98 = a.SWALLOWDESC,
                         v99 = a.URINESHAPE,
                         v30 = a.MICTURITIONDESC,
                         v31 = a.MICTURITIONAIDTYPE,
                         w01 = a.STOOLSHAPE,
                         v32 = a.DEFECATIONFREQ,
                         v34 = a.DEFECATIONDESC,
                         v35 = a.DEFECATIONAIDDESC,
                         v38 = a.OTHERDEFECATION,
                         w02 = a.GUT_SOUND,
                         w03 = a.GUT_FLATULENCE,
                         w04 = a.GUT_LUMP,
                         v39 = a.SLEEPHOURS_N,
                         v41 = a.SLEEPHOURS_D,
                         v43 = a.SLEEPDESC,
                         v44 = a.SLEEPPILLS,
                         v45 = a.GAIT,
                         v46 = a.PHYSICALIMPAIRMENT,
                         w07 = a.BALANCE_SIT,
                         w08 = a.BALANCE_STAND,
                         w09 = a.BALANCE_WALK,
                         Vc01 = a.MUSCLETYPE_RH,
                         Vc02 = a.MUSCLETYPE_LH,
                         Vc03 = a.MUSCLETYPE_RF,
                         Vc04 = a.MUSCLETYPE_LF,
                         Vc05 = a.MUSCLE_RH,
                         Vc06 = a.MUSCLE_LH,
                         Vc07 = a.MUSCLE_RF,
                         Vc08 = a.MUSCLE_LF,
                         Vc09 = a.JOINTTYPE_RH,
                         Vc10 = a.JOINTTYPE_LH,
                         Vc11 = a.JOINTTYPE_RF,
                         Vc12 = a.JOINTTYPE_LF,
                         v51 = a.SKINDESC,
                         v52 = a.SKINPARTDESC,
                         v53 = a.WOUNDDESC,
                         v54 = a.BEDSOREPART,
                         v55 = a.BEDSORESIZE,
                         v58 = a.BEDSOREDEGREE,
                         v59 = a.SIGHTDESC,
                         w11 = a.SIGHTCORRECTED,
                         v60 = a.SIGHT_L,
                         v61 = a.SIGHT_R,
                         v62 = a.LISTENDESC,
                         w10 = a.HEARINGAID,
                         v63 = a.LISTEN_L,
                         v64 = a.LISTEN_R,
                         v65 = a.SENSATIONDESC,
                         v66 = a.PAINDESC,
                         v67 = a.PAINPART,
                         v68 = a.PAIN_FREQ,
                         v69 = a.PAIN_NUTURE,
                         v70 = a.PAIN_SHOW,
                         v71 = a.PAINDRUGDESC,
                         w06 = "",
                         v72 = a.ILLUSIONDESC,
                         v73 = a.COMMUNICATESKILL,
                         v74 = a.COMMUNICATETYPE,
                         v75 = a.COMMUNICATEDESC,
                         v76 = a.APPELLATION,
                         v77 = a.EMOTION,
                         v78 = "",
                         v79 = a.ALLERGY_DRUG,
                         v80 = a.ALLERGY_FOOD,
                         v81 = a.ALLERGY_OTHERS,
                         w14 = a.ABNORMAL_W,
                         w05 = a.ACCIDENT_W,
                         v82 = a.PAINDEGREEDESC_W,
                         v83 = a.ADLRESULTS,
                         v84 = a.MMSEDESC,
                         v85 = a.iADLRESULTS,
                         v86 = a.KS_RESULTS,
                         v87 = a.YY_RESULTS,
                         v88 = a.FALLRESULTS,
                         v89 = a.PRESSURESORE,
                         w12 = a.CARENEEDS,
                         w13 = a.CAREQUESTION,
                     });

            var careInfo = q.Where(o => o.orgId == orgid).FirstOrDefault();
            if (careInfo != null)
            {
                response.org = careInfo.Org;
                response.Number = careInfo.Number.ToString();
                response.Name = careInfo.Name;
                response.Living = careInfo.Living;
                response.Dormitory = careInfo.Dormitory;
                response.Bedno = careInfo.Bedno;
                if (careInfo.Brithdate.HasValue)
                {
                    response.Bir_y = (careInfo.Brithdate.Value.Year).ToString();
                    response.Bir_m = careInfo.Brithdate.Value.Month.ToString();
                    response.Bir_d = careInfo.Brithdate.Value.Day.ToString();
                }
                else
                {
                    response.Bir_y = "";
                    response.Bir_m = "";
                    response.Bir_d = "";
                }

                response.h35_no = careInfo.h35_no.ToString();

                int year = DateTime.Now.Year;
                if (careInfo.Age.HasValue)
                {
                    response.Age = careInfo.Age.ToString();
                }
                else
                {
                    response.Age = careInfo.Brithdate.HasValue ? (year - careInfo.Brithdate.Value.Year).ToString() : "";
                }
                response.Date = careInfo.Date.HasValue ? Convert.ToDateTime(careInfo.Date).ToString("yyyy-MM-dd") : "";
                response.Next_date = careInfo.Next_date.HasValue ? Convert.ToDateTime(careInfo.Next_date).ToString("yyyy-MM-dd") : "";
                response.Personnel = careInfo.Personnel;
                response.Ill_history = careInfo.Ill_history;
                response.Carrydisease = careInfo.Carrydisease;
                response.v01 = careInfo.v01.HasValue ? careInfo.v01.ToString() : "0";
                response.v02 = careInfo.v02.HasValue ? careInfo.v02.ToString() : "0";
                response.v03 = careInfo.v03.HasValue ? careInfo.v03.ToString() : "0";
                response.v04 = careInfo.v04.HasValue ? careInfo.v04.ToString() : "0";
                response.v05 = careInfo.v05.HasValue ? careInfo.v05.ToString() : "0";
                response.v90 = GetCodedtlInfo(careInfo.v90, "E00.318");  //careInfo.v90;  //眼球对光反射
                response.v06 = careInfo.v06.HasValue ? careInfo.v06.ToString() : "0";
                response.v07 = careInfo.v07.HasValue ? careInfo.v07.ToString() : "0";
                response.vs08 = GetCodedtlInfo(careInfo.vs08, "E00.315"); //careInfo.vs08;
                response.v08 = GetCodedtlInfo(careInfo.v08, "E00.312");  //careInfo.v08;
                response.v09 = GetCodedtlInfo(careInfo.v09, "E00.313");// careInfo.v09;
                response.v10 = GetCodedtlInfo(careInfo.v10, "E00.314"); // careInfo.v10;
                response.v11 = careInfo.v11;
                response.v12 = GetCodedtlInfo(careInfo.v12, "E00.207");
                response.v91 = GetCodedtlInfo(careInfo.v91, "E00.316");
                response.v92 = GetCodedtlInfo(careInfo.v92, "E00.305");
                response.v93 = GetCodedtlInfo(careInfo.v93, "E00.305");
                response.v94 = GetCodedtlInfo(careInfo.v94, "E00.305");
                response.v95 = GetCodedtlInfo(careInfo.v95, "E00.305");

                response.v13 = GetCodedtlInfo(careInfo.v13, "E00.300");
                response.v14 = GetCodedtlInfo(careInfo.v14, "E00.301");
                response.v15 = GetCodedtlInfo(careInfo.v15, "E00.302");
                response.v16 = GetCodedtlInfo(careInfo.v16, "E00.303");
                response.v17 = GetCodedtlInfo(careInfo.v17, "E00.304");

                response.v18 = careInfo.v18;
                response.v19 = careInfo.v19;
                response.v20 = GetCodedtlInfo(careInfo.v20, "E00.203");
                response.v21 = careInfo.v21;
                response.vs22 = careInfo.vs22;
                response.v22 = careInfo.v22;
                response.v23 = careInfo.v23;
                response.v24 = GetCodedtlInfo(careInfo.v24, "E00.306");
                response.v25 = GetCodedtlInfo(careInfo.v25, "E00.307");
                response.v26 = GetCodedtlInfo(careInfo.v26, "E00.308");
                response.v27 = GetCodedtlInfo(careInfo.v27, "E00.309");
                response.v28 = GetCodedtlInfo(careInfo.v28, "E00.309");
                response.v29 = GetCodedtlInfo(careInfo.v29, "E00.310");
                response.v96 = careInfo.v96;
                response.v97 = careInfo.v97;
                response.v98 = careInfo.v98;
                response.v99 = careInfo.v99;
                response.v30 = careInfo.v30;
                response.v31 = careInfo.v31;
                response.w01 = careInfo.w01;
                response.v32 = careInfo.v32;
                response.v34 = careInfo.v34;
                response.v35 = careInfo.v35;
                response.v38 = careInfo.v38;
                response.w02 = careInfo.w02;
                response.w03 = careInfo.w03;
                response.w04 = careInfo.w04;
                response.v39 = careInfo.v39.HasValue ? careInfo.v39.ToString() : "0";
                response.v41 = careInfo.v41.HasValue ? careInfo.v41.ToString() : "0";
                response.v43 = GetCodedtlInfo(careInfo.v43, "F00.060");
                response.v44 = careInfo.v44;
                response.v45 = GetCodedtlInfo(careInfo.v45, "E00.311");
                response.v46 = careInfo.v46;
                response.w07 = careInfo.w07;
                response.w08 = careInfo.w08;
                response.w09 = careInfo.w09;
                response.Vc01 = GetCodedtlInfo(careInfo.Vc01, "E00.321");
                response.Vc02 = GetCodedtlInfo(careInfo.Vc02, "E00.321");
                response.Vc03 = GetCodedtlInfo(careInfo.Vc03, "E00.321");
                response.Vc04 = GetCodedtlInfo(careInfo.Vc04, "E00.321");
                response.Vc05 = GetCodedtlInfo(careInfo.Vc05, "E00.322");
                response.Vc06 = GetCodedtlInfo(careInfo.Vc06, "E00.322");
                response.Vc07 = GetCodedtlInfo(careInfo.Vc07, "E00.322");
                response.Vc08 = GetCodedtlInfo(careInfo.Vc08, "E00.322");
                response.Vc09 = GetCodedtlInfo(careInfo.Vc09, "E00.323");
                response.Vc10 = GetCodedtlInfo(careInfo.Vc10, "E00.323");
                response.Vc11 = GetCodedtlInfo(careInfo.Vc11, "E00.323");
                response.Vc12 = GetCodedtlInfo(careInfo.Vc12, "E00.323");
                response.v51 = careInfo.v51;
                response.v52 = careInfo.v52;
                response.v53 = careInfo.v53;
                response.v54 = careInfo.v54;
                response.v55 = careInfo.v55;
                response.v58 = careInfo.v58;
                response.v59 = careInfo.v59;
                response.w11 = careInfo.w11;
                response.v60 = GetCodedtlInfo(careInfo.v60, "E00.319");
                response.v61 = GetCodedtlInfo(careInfo.v61, "E00.319");
                response.v62 = careInfo.v62;
                response.w10 = GetCodedtlInfo(careInfo.w10, "E00.317");
                response.v63 = GetCodedtlInfo(careInfo.v63, "E00.320");
                response.v64 = GetCodedtlInfo(careInfo.v64, "E00.320");
                response.v65 = careInfo.v65;
                response.v66 = careInfo.v66;
                response.v67 = careInfo.v67;
                response.v68 = careInfo.v68;
                response.v69 = careInfo.v69;
                response.v70 = careInfo.v70;
                response.v71 = careInfo.v71;
                response.w06 = "";
                response.v72 = careInfo.v72;
                response.v73 = careInfo.v73;
                response.v74 = careInfo.v74;
                response.v75 = careInfo.v75;
                response.v76 = careInfo.v76;
                response.v77 = careInfo.v77;
                response.v78 = "";
                response.v79 = careInfo.v79;
                response.v80 = careInfo.v80;
                response.v81 = careInfo.v81;
                response.w14 = careInfo.w14;
                response.w05 = GetCodedtlInfo(careInfo.w05, "K00.018");
                response.v82 = careInfo.v82;
                response.v83 = careInfo.v83;
                response.v84 = careInfo.v84;
                response.v85 = careInfo.v85;
                response.v86 = careInfo.v86;
                response.v87 = careInfo.v87;
                response.v88 = careInfo.v88;
                response.v89 = careInfo.v89;
                response.w12 = careInfo.w12;
                response.w13 = careInfo.w13;
            }

            return response;
        }
        #endregion

        public string GetCodedtlInfo(string itemcode, string itemtype)
        {
            string itemname = string.Empty;
            if (!string.IsNullOrEmpty(itemcode) && !string.IsNullOrEmpty(itemtype))
            {
                var dbSet = unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.AsQueryable();
                var list = dbSet.ToList();
                itemname = list.Where(o => o.ITEMCODE == itemcode && o.ITEMTYPE == itemtype).FirstOrDefault().ITEMNAME;
            }
            else
            {
                itemname = "";
            }
            return itemname;
        }

    }
}
