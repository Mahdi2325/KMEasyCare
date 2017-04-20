using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.Business.Implement.Report
{
    public class H32Report : BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            int careSvrId = (int)ParamId;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            IResidentManageService residentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();
            ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            ICarePlansManageService carePlansManageService = IOCContainer.Instance.Resolve<ICarePlansManageService>();
            var careSvr = socialWorkerManageService.GetCareSvrById(careSvrId);
            if (careSvr.Data == null || !careSvr.Data.FeeNo.HasValue)
            {
                InitData(typeof(CaresvrRecModel), doc);
                doc.ReplaceText("Name", "");
                doc.ReplaceText("Inday", "");
                doc.ReplaceText("Dormitory", "");
                return;
            }
            CodeFilter request = new CodeFilter { ItemTypes = new string[] { "K00.017", "E00.215", "E00.216", "E00.217" } };
            var dict = (List<CodeValue>)dictManageService.QueryCode(request).Data;
            if (careSvr.Data.SvrAddress!=null)
            {
                careSvr.Data.SvrAddress = dict.Find(it => it.ItemType == "K00.017" && it.ItemCode == careSvr.Data.SvrAddress).ItemName;
            }
            if (careSvr.Data.SvrType != null)
            {
                careSvr.Data.SvrType = dict.Find(it => it.ItemType == "E00.215" && it.ItemCode == careSvr.Data.SvrType).ItemName;
            }
            if (careSvr.Data.RelationType != null)
            {
                careSvr.Data.RelationType = dict.Find(it => it.ItemType == "E00.216" && it.ItemCode == careSvr.Data.RelationType).ItemName;
            }
            if (careSvr.Data.EvalStatus != null)
            {
                careSvr.Data.EvalStatus = dict.Find(it => it.ItemType == "E00.217" && it.ItemCode == careSvr.Data.EvalStatus).ItemName;
            }
            if (careSvr.Data.QuestionFocus != null)
            {
                if (careSvr.Data.QuestionLevel != null)
                {
                    var diaPr = carePlansManageService.GetDiaPR("001", careSvr.Data.QuestionLevel);
                    if (diaPr != null && diaPr.Data != null)
                    {
                        careSvr.Data.QuestionFocus = diaPr.Data.Find(it => it.ItemCode == careSvr.Data.QuestionFocus).ItemName;
                    }
                }
            }

            if (careSvr.Data.Carer != null)
            {
                var emp = organizationManageService.GetEmployee(careSvr.Data.Carer);
                if (emp != null)
                {
                    careSvr.Data.Carer = emp.Data.EmpName;
                }
            }

            BindData(careSvr.Data, doc);
            var resident = residentManageService.GetResident(careSvr.Data.FeeNo.Value);
            if (resident != null && resident.Data != null)
            {
                   doc.ReplaceText("Inday", resident.Data.InDate.HasValue ? resident.Data.InDate.Value.ToString("yyyy-MM-dd") : "");
                   doc.ReplaceText("Dormitory", resident.Data.BedNo);
            }
            else
            {
                doc.ReplaceText("Inday",  "");
                doc.ReplaceText("Dormitory", "");
            }

            var person = residentManageService.GetPerson(careSvr.Data.RegNo??0);
            if (person != null && person.Data != null)
            {
                doc.ReplaceText("Name", person.Data.Name);
            }
            else
            {
                doc.ReplaceText("Name", "");
            }
            //doc.ReplaceText("FeeNo", feeNo.ToString());
            //doc.ReplaceText("EvalMinutes", careSvr.Data.EvalMinutes.HasValue ? ((decimal)careSvr.Data.EvalMinutes.Value / 60).ToString("F2") : "");
            //doc.ReplaceText("RecDate", careSvr.Data.RecDate.HasValue ? careSvr.Data.RecDate.Value.ToString("yyyy-MM-dd") : "");
            //doc.ReplaceText("SvrPeople", careSvr.Data.SvrPeople);
            //doc.ReplaceText("Id", careSvrId.ToString());
            //doc.ReplaceText("ProcessActivity", careSvr.Data.ProcessActivity);
            //doc.ReplaceText("QuestionLevel", careSvr.Data.QuestionLevel);
            //doc.ReplaceText("QuestionDesc", careSvr.Data.QuestionDesc);
            //doc.ReplaceText("TreatDesc", careSvr.Data.TreatDesc);
            //doc.ReplaceText("EvalDesc", careSvr.Data.EvalDesc);
        }
    }
}
