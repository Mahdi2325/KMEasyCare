using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace KMHC.SLTC.Business.Implement
{
    public class ServiceDepositGrantListService : BaseService, IServiceDepositGrantList
    {
        public string GetNsno()
        {
            var org = unitOfWork.GetRepository<LTC_ORG>().dbSet.Where(w => w.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            if (org == null)
            {
                return null;
            }
            return org.NSNO;
        }
    }
}
