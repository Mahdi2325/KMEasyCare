using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Report;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController.ExportReport
{
    public partial class SettleAccountReportController : ReportBaseController
    {
        ISettleAccountService settleAccountService = IOCContainer.Instance.Resolve<ISettleAccountService>();
        public ActionResult SettleAccountReport()
        {
            string templateName = Request["templateName"];
            string beginTime = Request["beginDate"];
            string endTime = Request["endDate"];
            string feeNo = Request["feeNo"];
            ReportRequest request = new ReportRequest();
            request.feeNo = Convert.ToInt64(feeNo);
            request.beginTime = beginTime;
            request.endTime = endTime;

            if (templateName != null)
            {
                switch (templateName)
                {
                    case "SettleAccountReport":

                        this.GeneratePDF("SettleAccountReport", this.printSettleAccount, request);
                        break;
                }
            }
            return View("Preview");
        }

        private void printSettleAccount(WordDocument doc, ReportRequest request)
        {
            var hopDays = 0;

            var beginTime = string.Empty;
            var endTime = string.Empty;
            var invoiceNo = string.Empty;

            decimal nCIItemTotalCost = 0;
            decimal nCIPay = 0;
            decimal nCIItemSelfPay = 0;
            decimal totalNciPay = 0;

            decimal amt1 = 0;
            decimal amt2 = 0;
            decimal amt3 = 0;
            decimal amt4 = 0;
            decimal amt5 = 0;
            decimal amt6 = 0;
            decimal amt7 = 0;
            decimal amt8 = 0;
            decimal amt9 = 0;
            decimal amt11 = 0;
            decimal amt12 = 0;

            var snCIItemTotalCost = string.Empty;
            var snCIPay = string.Empty;
            var snCIItemSelfPay = string.Empty;
            var stotalNciPay = string.Empty;

            var samt1 = string.Empty;
            var samt2 = string.Empty;
            var samt3 = string.Empty;
            var samt4 = string.Empty;
            var samt5 = string.Empty;
            var samt6 = string.Empty;
            var samt7 = string.Empty;
            var samt8 = string.Empty;
            var samt9 = string.Empty;
            var samt11 = string.Empty;
            var samt12 = string.Empty;

            var AuditNumber = string.Format("{0}{1}{2}{3}{4}{5}", DateTime.Now.Year.ToString(),
                            DateTime.Now.Month < 10
                                ? "0" + DateTime.Now.Month.ToString()
                                : DateTime.Now.Month.ToString(),
                            DateTime.Now.Day < 10
                                ? "0" + DateTime.Now.Day.ToString()
                                : DateTime.Now.Day.ToString(),
                            DateTime.Now.Hour < 10
                                ? "0" + DateTime.Now.Hour.ToString()
                                : DateTime.Now.Hour.ToString(),
                            DateTime.Now.Minute < 10
                                ? "0" + DateTime.Now.Minute.ToString()
                                : DateTime.Now.Minute.ToString(),
                            DateTime.Now.Second < 10
                                ? "0" + DateTime.Now.Second.ToString()
                                : DateTime.Now.Second.ToString()
                            );

            var paymentResponse = settleAccountService.QueryBillV2(request.feeNo, request.beginTime, request.endTime);

            if (paymentResponse.Data != null)
            {
                if (paymentResponse.Data.Count > 0)
                {
                    if (paymentResponse.Data.Count == 1)
                    {
                        hopDays = paymentResponse.Data[0].HospDay ?? 0;
                        beginTime = string.Format("{0:d}", paymentResponse.Data[0].BalanceStartTime);
                        endTime = string.Format("{0:d}", paymentResponse.Data[0].BalanceEndTime);
                        invoiceNo = paymentResponse.Data[0].InvoiceNo;
                        nCIItemTotalCost = paymentResponse.Data[0].NCIItemTotalCost;
                        nCIPay = paymentResponse.Data[0].NCIPay;
                        nCIItemSelfPay = paymentResponse.Data[0].NCIItemSelfPay;
                        totalNciPay = paymentResponse.Data[0].TotalNciPay ?? 0;
                    }
                    else
                    {

                        nCIItemTotalCost = (from s in paymentResponse.Data
                                            select s.NCIItemTotalCost).Sum();
                        nCIPay = (from s in paymentResponse.Data
                                  select s.NCIPay).Sum();
                        nCIItemSelfPay = (from s in paymentResponse.Data
                                          select s.NCIItemSelfPay).Sum();


                        var balanceMinTime = (from s in paymentResponse.Data
                                              select s.BalanceStartTime).Min();
                        beginTime = string.Format("{0:d}", balanceMinTime);

                        var balanceMaxTime = (from s in paymentResponse.Data
                                              select s.BalanceEndTime).Max();
                        endTime = string.Format("{0:d}", balanceMaxTime);

                        totalNciPay = paymentResponse.Data[0].TotalNciPay ?? 0;
                        foreach (var item in paymentResponse.Data)
                        {

                            hopDays += item.HospDay ?? 0;
                            if (!string.IsNullOrEmpty(invoiceNo))
                            {
                                if (invoiceNo.IndexOf(item.InvoiceNo) > -1)
                                {
                                }
                                else
                                {
                                    if (invoiceNo == string.Empty)
                                    {
                                        invoiceNo = item.InvoiceNo;
                                    }
                                    else
                                    {
                                        invoiceNo += "," + item.InvoiceNo;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            var tempresponse = new BaseResponse<SettleAccountModel>();
            tempresponse = settleAccountService.QuerySettleAccountInfo(request.feeNo);

            var feeRecordResponse = settleAccountService.QueryRecord(request.feeNo, request.beginTime, request.endTime);
            if (feeRecordResponse.Data != null)
            {
                foreach (var item in feeRecordResponse.Data)
                {
                    if (item.MCType == "001")
                    {
                        amt11 = amt11 + item.Cost;
                    }
                    else if (item.MCType == "002")
                    {
                        amt12 = amt12 + item.Cost;
                    }

                    if (item.ChargeTypeId == "001")
                    {
                        amt1 = amt1 + item.Cost;
                    }
                    else if (item.ChargeTypeId == "002")
                    {
                        amt2 = amt2 + item.Cost;
                    }
                    else if (item.ChargeTypeId == "003")
                    {
                        amt3 = amt3 + item.Cost;
                    }
                    else if (item.ChargeTypeId == "004")
                    {
                        amt4 = amt4 + item.Cost;
                    }
                    else if (item.ChargeTypeId == "005")
                    {
                        amt5 = amt5 + item.Cost;
                    }
                    else if (item.ChargeTypeId == "006" || item.ChargeTypeId == "009")
                    {
                        amt6 = amt6 + item.Cost;
                    }
                    else if (item.ChargeTypeId == "007")
                    {
                        amt7 = amt7 + item.Cost;
                    }
                    else if (item.ChargeTypeId == "008")
                    {
                        amt8 = amt8 + item.Cost;
                    }
                    else if (item.ChargeTypeId == "010")
                    {
                        amt9 = amt9 + item.Cost;
                    }
                }
            }

            snCIItemTotalCost = nCIItemTotalCost == 0 ? "0.00" : nCIItemTotalCost.ToString();
            snCIPay = nCIPay == 0 ? "0.00" : nCIPay.ToString();
            snCIItemSelfPay = nCIItemSelfPay == 0 ? "0.00" : nCIItemSelfPay.ToString();
            stotalNciPay = totalNciPay == 0 ? "0.00" : totalNciPay.ToString();
            samt1 = amt1 == 0 ? "0.00" : amt1.ToString();
            samt2 = amt2 == 0 ? "0.00" : amt2.ToString();
            samt3 = amt3 == 0 ? "0.00" : amt3.ToString();
            samt4 = amt4 == 0 ? "0.00" : amt4.ToString();
            samt5 = amt5 == 0 ? "0.00" : amt5.ToString();
            samt6 = amt6 == 0 ? "0.00" : amt6.ToString();
            samt7 = amt7 == 0 ? "0.00" : amt7.ToString();
            samt8 = amt8 == 0 ? "0.00" : amt8.ToString();
            samt9 = amt9 == 0 ? "0.00" : amt9.ToString();
            samt11 = amt11 == 0 ? "0.00" : amt11.ToString();
            samt12 = amt12 == 0 ? "0.00" : amt12.ToString();

            var response = new BaseResponse<List<object>>(new List<object>());
            response.Data.Add
               (new
               {
                   AuditNo = AuditNumber,
                   Year = DateTime.Now.Year.ToString(),
                   Month = DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString(),
                   Day = DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString(),
                   OrgName = tempresponse.Data.OrgName,
                   Name = tempresponse.Data.Name,
                   Sex = tempresponse.Data.Sex == "F" ? "女" : "男",
                   BirthDate = string.Format("{0:d}", tempresponse.Data.BrithDate),
                   IdNo = tempresponse.Data.IDNo,
                   ResidengNo = tempresponse.Data.ResidengNo,
                   RsType = tempresponse.Data.RSType == "001" ? "社会" : tempresponse.Data.RSType == "002" ? "三无" : "残疾",
                   CareType = tempresponse.Data.CareTypeId == "1" ? "一级专护" : tempresponse.Data.CareTypeId == "2" ? "二级专护" : "机构护理",
                   ContactPhone = tempresponse.Data.ContactPhone,
                   FeeNo = tempresponse.Data.FeeNo,
                   BeginTime = beginTime,
                   EndTime = endTime,
                   InvoiceNo = invoiceNo,
                   Hop = hopDays,
                   NCIPayLevel = tempresponse.Data.NCIPayLevel,
                   NCIPayScale = tempresponse.Data.NCIPayScale,
                   NCIPay = Math.Round(tempresponse.Data.NCIPay * hopDays, 2),
                   DiseaseDiag = tempresponse.Data.DiseaseDiag,
                   Amt1 = samt1,
                   Amt2 = samt2,
                   Amt3 = samt3,
                   Amt4 = samt4,
                   Amt5 = samt5,
                   Amt6 = samt6,
                   Amt7 = samt7,
                   Amt8 = samt8,
                   Amt9 = samt9,
                   AmtJL = samt11,
                   AmtYL = samt12,
                   AmtSum = snCIItemTotalCost,
                   AmtHSXJBXJE = snCIPay,
                   AmtZFJE = snCIItemSelfPay,
                   AmtDX = CmycurD(snCIPay),
                   AmtLJYBXJE = stotalNciPay
               });

            BindData(response.Data[0], doc);
        }

        /// <summary>
        /// 转换人民币大小金额
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns>返回大写形式</returns>
        public string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string str3 = "";    //从原num值中取出的值
            string str4 = "";    //数字的字符串形式
            string str5 = "";  //人民币大写金额形式
            int i;    //循环变量
            int j;    //num的值乘以100的字符串长度
            string ch1 = "";    //数字的汉语读法
            string ch2 = "";    //数字位的汉字读法
            int nzero = 0;  //用来计算连续的零值是几个
            int temp;            //从原num值中取出的值

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式
            j = str4.Length;      //找出最高位
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分

            //循环取出每一位需要转换的值
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值
                temp = Convert.ToInt32(str3);      //转换为数字
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整”
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /// <summary>
        /// 一个重载，将字符串先转换成数字在调用CmycurD(decimal num)
        /// </summary>
        /// <param name="num">用户输入的金额，字符串形式未转成decimal</param>
        /// <returns></returns>
        public string CmycurD(string numstr)
        {
            try
            {
                decimal num = Convert.ToDecimal(numstr);
                return CmycurD(num);
            }
            catch
            {
                return "非数字形式！";
            }
        }
    }
}
