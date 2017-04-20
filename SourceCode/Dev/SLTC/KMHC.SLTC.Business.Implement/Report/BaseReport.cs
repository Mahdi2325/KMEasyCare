using KMHC.Infrastructure.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.Business.Implement.Report
{
    public abstract class BaseReport
    {
        public string TemplateName;
        public long ParamId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Order;
        public string Preview()
        {
            if (string.IsNullOrWhiteSpace(TemplateName))
                throw new Exception("TemplateName为空！");
            using (var doc = new WordDocument())
            {
                doc.Load(TemplateName);
                Operation(doc);
                return doc.SavePDF();
            }
        }

        public void Download()
        {
            if (string.IsNullOrWhiteSpace(TemplateName))
                throw new Exception("TemplateName为空！");
            using (var doc = new WordDocument())
            {
                doc.Load(TemplateName);
                Operation(doc);
                Util.DownloadFile(doc.SaveDoc());
            }
        }

        protected abstract void Operation(WordDocument doc);


        protected void BindData(object obj, WordDocument doc)
        {
            var t = obj.GetType();
            foreach (var field in t.GetProperties())
            {
                if (field.PropertyType == typeof(DateTime?) || field.PropertyType == typeof(DateTime))
                {
                    doc.ReplaceText(field.Name, field.GetValue(obj) == null ? "" : ((DateTime)field.GetValue(obj)).ToString("yyyy-MM-dd"));
                }
                else
                {
                    doc.ReplaceText(field.Name, field.GetValue(obj) == null ? "" : field.GetValue(obj).ToString());
                }

            }
        }

        protected DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        protected void BindData(object obj, WordDocument doc, Dictionary<string, string> dict)
        {
            var t = obj.GetType();
            foreach (var field in t.GetProperties())
            {
                if (field.PropertyType == typeof(DateTime?) || field.PropertyType == typeof(DateTime))
                {
                    dict.Add(field.Name, field.GetValue(obj) == null ? "" : ((DateTime)field.GetValue(obj)).ToString("yyyy-MM-dd"));
                }
                else
                {
                    dict.Add(field.Name, field.GetValue(obj) == null ? "" : field.GetValue(obj).ToString());
                }
            }
        }

        protected void InitData(Type type, WordDocument doc)
        {
            foreach (var field in type.GetProperties())
            {
                doc.ReplaceText(field.Name, "");
            }
        }

        protected void InitValue(int start, int end, WordDocument doc)
        {
            for (var i = start; i <= end; i++)
            {
                doc.ReplaceText("Value" + i.ToString("00"), "");
            }
        }


        protected string GetOrgName(string orgId)
        {
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            var org = organizationManageService.GetOrg(orgId);
            return org.Data == null ? "" : org.Data.OrgName;
        }

    }
}
