using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.Business.Implement.Report
{
    public class P15Report : BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            BaseRequest<LifeRecordFilter> lifeRecordFilter = new BaseRequest<LifeRecordFilter>();
            lifeRecordFilter.CurrentPage = 1;
            lifeRecordFilter.PageSize = 1000;
            lifeRecordFilter.Data.FeeNo = ParamId;
            lifeRecordFilter.Data.StartDate = StartDate;
            lifeRecordFilter.Data.EndDate = EndDate.AddDays(1);
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            var response = socialWorkerService.QueryLifeRecord(lifeRecordFilter);
            doc.ReplaceText("Org", org.Data.OrgName);

            CodeFilter codeFilter = new CodeFilter();
            codeFilter.ItemTypes = new string[] { "A00.400", "A00.401", "A00.402" };
            var dict = (List<CodeValue>)dictManageService.QueryCode(codeFilter).Data;

            DataTable dt = new DataTable();
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            dt.Columns.Add("c4");
            dt.Columns.Add("c5");
            dt.Columns.Add("c6");
            dt.Columns.Add("c7");
            dt.Columns.Add("c8");
            dt.Columns.Add("c9");
            dt.Columns.Add("c10");
            dt.Columns.Add("c11");
            dt.Columns.Add("c12");
            if (response.Data != null)
            {
                if (response.Data.Count == 0)
                {
                    response.Data.Add(new LifeRecordModel());
                }
                CodeValue findItem;

                foreach (var item in response.Data)
                {
                    var dr = dt.NewRow();
                    dr["c1"] = item.Name;
                    dr["c2"] = item.ResidentsNo;
                    //dr["c3"] = item.Floor + " " + item.RoomNo;
                    if (organizationManageService.GetOrgFloor(item.Floor).Data != null &&
                        organizationManageService.GetOrgRoom(item.RoomNo).Data != null)
                    {
                        dr["c3"] = organizationManageService.GetOrgFloor(item.Floor).Data.FloorName + " " + organizationManageService.GetOrgRoom(item.RoomNo).Data.RoomName;
                    }
                    if (item.RecordDate.HasValue)
                    {
                        var data = item.RecordDate.Value;
                        //dr["c4"] = string.Format("{0}/{1}/{2}", data.Year - 1911, data.Month, data.Day);
                        dr["c4"] = string.Format("{0}/{1}/{2}", data.Year, data.Month, data.Day);
                    }
                    dr["c5"] = item.BodyTemp.HasValue ? item.BodyTemp.Value.ToString("N1") : "";

                    dr["c10"] = string.IsNullOrEmpty(item.AmActivity) ? "" : item.AmActivity.ToString();
                    dr["c11"] = string.IsNullOrEmpty(item.PmActivity) ? "" : item.PmActivity.ToString();

                    findItem = dict.Find(it => it.ItemType == "A00.402" && it.ItemCode == item.Comments);
                    dr["c12"] = findItem != null ? findItem.ItemName : "";
                    //dr["c1"] = item.Name;RecordByName
                    dt.Rows.Add(dr);
                }
            }
            doc.FillTable(0, dt, "", "", 1);
            dt.Dispose();
        }
    }
}
