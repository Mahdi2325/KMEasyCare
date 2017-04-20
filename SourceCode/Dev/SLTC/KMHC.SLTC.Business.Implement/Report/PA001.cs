using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.Report
{
    public class PA001 : BaseReport
    {
        protected override void Operation(Infrastructure.Word.WordDocument doc)
        {
            int recordId = (int)ParamId;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            IResidentManageService ResidentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            var Person = ResidentManageService.GetPersonExtend(recordId);
            if (Person == null)
            {
                return;
            }
            if (Person.Data.ImgUrl != null)
            {
                string mapPath = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, Person.Data.ImgUrl.Replace("/", @"\"));
                if (File.Exists(mapPath))
                {
                    doc.InsertImage("photo", mapPath, 150, 200);
                }
                else
                {
                    doc.ReplaceText("photo", "");
                }

            }
            else
            {
                doc.ReplaceText("photo", "");
            }
            CodeFilter codeFilter = new CodeFilter();
            codeFilter.ItemTypes = new string[] { "A00.030", "A00.032", "A00.035", "A00.001", "A00.007", "A00.008", "A00.011", "A00.002" };
            var dict = (List<CodeValue>)dictManageService.QueryCode(codeFilter).Data;
            CodeValue findItem;
            Person.Data.Sex = dict.Find(it => it.ItemType == "A00.001" && it.ItemCode == Person.Data.Sex) != null ?
                dict.Find(it => it.ItemType == "A00.001" && it.ItemCode == Person.Data.Sex).ItemName : "";
            Person.Data.servicetype = dict.Find(it => it.ItemType == "A00.002" && it.ItemCode == Person.Data.servicetype) != null ?
               dict.Find(it => it.ItemType == "A00.002" && it.ItemCode == Person.Data.servicetype).ItemName : "";
            Person.Data.Education = dict.Find(it => it.ItemType == "A00.007" && it.ItemCode == Person.Data.Education) != null ?
               dict.Find(it => it.ItemType == "A00.007" && it.ItemCode == Person.Data.Education).ItemName : "";
            Person.Data.ReligionCode = dict.Find(it => it.ItemType == "A00.008" && it.ItemCode == Person.Data.ReligionCode) != null ?
               dict.Find(it => it.ItemType == "A00.008" && it.ItemCode == Person.Data.ReligionCode).ItemName : "";
            Person.Data.MerryFlag = dict.Find(it => it.ItemType == "A00.011" && it.ItemCode == Person.Data.MerryFlag) != null ?
               dict.Find(it => it.ItemType == "A00.011" && it.ItemCode == Person.Data.MerryFlag).ItemName : "";
            Person.Data.CAddress1 = Person.Data.City2 + "" + Person.Data.Address2 + "" + Person.Data.Address2dtl;
            Person.Data.CAddress2 = Person.Data.City1 + "" + Person.Data.Address1 + "" + Person.Data.Address1dtl;
            BindData(Person.Data, doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            dt.Columns.Add("c4");
            dt.Columns.Add("c5");
            dt.Columns.Add("c6");
            if (Person.Data.RelationDtl != null)
            {

                if (Person.Data.RelationDtl.Count > 0)
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
                else
                {
                    var dr = dt.NewRow();
                    dr["c1"] = "";
                    dr["c2"] = "";
                    dr["c3"] = "";
                    dr["c4"] = "";
                    dr["c5"] = "";
                    dr["c6"] = "";
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                var dr = dt.NewRow();
                dr["c1"] = "";
                dr["c2"] = "";
                dr["c3"] = "";
                dr["c4"] = "";
                dr["c5"] = "";
                dr["c6"] = "";
                dt.Rows.Add(dr);
            }
            doc.FillTable(0, dt, "", "", 32);
            dt.Dispose();
        }
    }
}
