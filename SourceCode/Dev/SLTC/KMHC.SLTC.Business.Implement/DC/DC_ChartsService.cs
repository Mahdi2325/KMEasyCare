using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.DC
{
    public class DC_ChartsService : BaseService,  IDC_ChartsService
    {
        public List<Chart_AccumulateUU> GetAccumulateUU()
        {
            DateTime endDate=DateTime.Now.Date;
            DateTime startDate = endDate.AddMonths(-1);
            //endDate = endDate.AddDays(1);
            List<Chart_AccumulateUU> accumulateUU = new List<Chart_AccumulateUU>();
            List<DC_IPDREG> ipgOut = unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId && x.OUTDATE >= startDate && x.OUTDATE <= endDate && x.IPDFLAG == "O").OrderBy(o => o.INDATE).ToList();
            int totleInPeople =int.Parse(unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId && x.IPDFLAG == "I").Count().ToString());
            int totleOutPeople = int.Parse(unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId && x.IPDFLAG == "O").Count().ToString());
            int currentInPeople = totleInPeople;
            int currentOutPeople = totleOutPeople;
            int outCount = 0;
            int id= 0;
            for (DateTime d = endDate; d > startDate; d = d.AddDays(-1), id++)
            {
                Chart_AccumulateUU Item = new Chart_AccumulateUU();
              if(ipgOut.Where(x=>x.OUTDATE==d).Count()>0)
              {
                  outCount = ipgOut.Where(x => x.OUTDATE == d).Count();
                  currentInPeople += outCount;
                  currentOutPeople -= outCount;
              }
              Item.ID = id;
              Item.Date = d.ToString("MM/dd");
              Item.ClosedUU = outCount;
              Item.AccumulateUU = currentInPeople;
              Item.AccumulateClosedUU = currentOutPeople;
              accumulateUU.Add(Item);
            }
            return accumulateUU.OrderByDescending(x=>x.ID).ToList();
        }

        public List<Chart_DiseaseDistribution> GetDiseaseDistribution()
        {
            List<Chart_DiseaseDistribution> diseaseDistribution = new List<Chart_DiseaseDistribution>();
            List<string> regFile = new List<string>();
             var q =(from re in unitOfWork.GetRepository<DC_REGFILE>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId && !string.IsNullOrEmpty(x.DISEASEINFO))
                                  join ip in unitOfWork.GetRepository<DC_IPDREG>().dbSet.Where(x => x.ORGID == SecurityHelper.CurrentPrincipal.OrgId&&x.IPDFLAG=="I") on re.REGNO equals ip.REGNO
                                  select new {
                                      re.DISEASEINFO
                                  }).ToList();
             if (q != null)
            {
                regFile = q.Select(x => x.DISEASEINFO).ToList();
            }
            string diseaseInfo =string.Join(",",regFile);
            List<string> diseaseList = new List<string>(diseaseInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            var arr = diseaseList.GroupBy(x=>x);
           
           foreach(var a in arr)
           {
               Chart_DiseaseDistribution disease = new Chart_DiseaseDistribution();
               disease.DiseaseName = a.Key;
               disease.UUCount = a.Count();
               diseaseDistribution.Add(disease);

           }
            return diseaseDistribution.OrderByDescending(x => x.ID).ToList();
        }
    }
}
