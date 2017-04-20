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
    public class PA002 : BaseReport
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
            if (Person.Data.Genogram != null)
            {
                string mapPath = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, Person.Data.Genogram.Replace("/", @"\"));
                if (File.Exists(mapPath))
                {
                    doc.InsertImage("Genogram", mapPath, 150, 200);
                }
                else
                {
                    doc.ReplaceText("Genogram", "");
                }

            }
            else
            {
                doc.ReplaceText("photo", "");
            }
            CodeFilter codeFilter = new CodeFilter();
            codeFilter.ItemTypes = new string[] { "A00.030", "A00.032", "A00.035", "A00.001", "A00.007", "A00.008"
                , "A00.011", "A00.002","A00.016","A00.020","A00.021","A00.022","A00.038","A00.043","A00.031"};
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
            Person.Data.EATHABITS = dict.Find(it => it.ItemType == "A00.038" && it.ItemCode == Person.Data.EATHABITS) != null ?
              dict.Find(it => it.ItemType == "A00.038" && it.ItemCode == Person.Data.EATHABITS).ItemName : "";
            Person.Data.SOURCETYPE = dict.Find(it => it.ItemType == "A00.016" && it.ItemCode == Person.Data.SOURCETYPE) != null ?
               dict.Find(it => it.ItemType == "A00.016" && it.ItemCode == Person.Data.SOURCETYPE).ItemName : "";
            Person.Data.ECONOMYFLAG = dict.Find(it => it.ItemType == "A00.020" && it.ItemCode == Person.Data.ECONOMYFLAG) != null ?
               dict.Find(it => it.ItemType == "A00.020" && it.ItemCode == Person.Data.ECONOMYFLAG).ItemName : "";
            Person.Data.CASETYPE = dict.Find(it => it.ItemType == "A00.021" && it.ItemCode == Person.Data.CASETYPE) != null ?
               dict.Find(it => it.ItemType == "A00.021" && it.ItemCode == Person.Data.CASETYPE).ItemName : "";
            Person.Data.DISABDEGREE = dict.Find(it => it.ItemType == "A00.022" && it.ItemCode == Person.Data.DISABDEGREE) != null ?
               dict.Find(it => it.ItemType == "A00.022" && it.ItemCode == Person.Data.DISABDEGREE).ItemName : "";
            Person.Data.SubsidyUnit = dict.Find(it => it.ItemType == "A00.043" && it.ItemCode == Person.Data.SubsidyUnit) != null ?
              dict.Find(it => it.ItemType == "A00.043" && it.ItemCode == Person.Data.SubsidyUnit).ItemName : "";
            Person.Data.CAddress1 = Person.Data.City2 + "" + Person.Data.Address2 + "" + Person.Data.Address2dtl;
            Person.Data.CAddress2 = Person.Data.City1 + "" + Person.Data.Address1 + "" + Person.Data.Address1dtl;
            Person.Data.EffectiveTime = Person.Data.BOOKISSUEDATE + "-" + Person.Data.BOOKEXPDATE;
            Person.Data.Living = Person.Data.FloorName + "/" + Person.Data.RoomName;
            Person.Data.ISIdentificationText = Person.Data.ISREEVAL == null ? "是" : (!Person.Data.ISREEVAL.Value ? "是" : "否");
            string[] str = new string[] { "第一类 神经系统构造及精神、心智功能", "第二类 眼，耳及相关机构与感官功能及疼痛",
                "第三类 涉及声音与言语构造及其功能", "第四类 循环、造血、免疫与呼吸系统构造及其功能", 
                "第五类 消化、新陈代谢与内分泌系统相关构造及其功能", "第六类 泌尿与生殖系统相关构造及其功能",
                "第七类 神经、肌肉、骨骼之移动相关构造及其功能", "第八类 皮肤与相关构造及其功能" };
            var Barrier_2 = "";
            if (Person.Data.DISABILITYCATE1!=null)
            {
                Barrier_2 +=Person.Data.DISABILITYCATE1.Value? str[0]:"";
            }
            if (Person.Data.DISABILITYCATE2 != null)
            {
                Barrier_2 += ","+(Person.Data.DISABILITYCATE2.Value ? str[1] : "");
            }
            if (Person.Data.DISABILITYCATE3 != null)
            {
                Barrier_2 += ","+(Person.Data.DISABILITYCATE3.Value ? str[2] : "");
            }
            if (Person.Data.DISABILITYCATE4 != null)
            {
                Barrier_2 += ","+(Person.Data.DISABILITYCATE4.Value ? str[3] : "");
            }
            if (Person.Data.DISABILITYCATE5 != null)
            {
                Barrier_2 += ","+(Person.Data.DISABILITYCATE5.Value ? str[4] : "");
            }
            if (Person.Data.DISABILITYCATE6 != null)
            {
                Barrier_2 += ","+(Person.Data.DISABILITYCATE6.Value ? str[5] : "");
            }
            if (Person.Data.DISABILITYCATE7 != null)
            {
                Barrier_2 += ","+(Person.Data.DISABILITYCATE7.Value ? str[6] : "");
            }
            if (Person.Data.DISABILITYCATE8 != null)
            {
                Barrier_2 += "," + (Person.Data.DISABILITYCATE8.Value ? str[7] : "");
            }
            Person.Data.Barrier_2 = Barrier_2;
            BindData(Person.Data, doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            dt.Columns.Add("c4");
            dt.Columns.Add("c5");
            dt.Columns.Add("c6");
            dt.Columns.Add("c7");
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
                        findItem = dict.Find(it => it.ItemType == "A00.031" && it.ItemCode == item.Kinship);
                        dr["c3"] = findItem != null ? findItem.ItemName : "";
                        findItem = dict.Find(it => it.ItemType == "A00.001" && it.ItemCode == item.Sex);
                        dr["c4"] = findItem != null ? findItem.ItemName : "";
                        dr["c5"] = item.Phone;
                        dr["c6"] = item.Address2;
                        findItem = dict.Find(it => it.ItemType == "A00.035" && it.ItemCode == item.WorkCode);
                        dr["c7"] = findItem != null ? findItem.ItemName : "";
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
            doc.FillTable(0, dt, "", "", 23);
            dt.Dispose();
        }
    }
}

