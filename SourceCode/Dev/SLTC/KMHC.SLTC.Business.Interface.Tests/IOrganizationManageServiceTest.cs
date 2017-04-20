using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;

namespace KMHC.SLTC.Business.Interface.Tests
{
    [TestClass]
    public class IOrganizationManageServiceTest
    {
        IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        [TestMethod]
        public void TestQueryOrganization()
        {
            BaseRequest<OrganizationFilter> request = new BaseRequest<OrganizationFilter>();
            request.Data.OrgID = "1";
            var response = organizationManageService.QueryOrg(request);
            Assert.AreEqual(response.Data.Count, 0);
        }

        [TestMethod]
        public void TestNewBedBasic()
        {
            BedBasic request = new BedBasic();
            request.BedNo = "1";
            request.OrgId = "1";
            request.BedDesc = "1号床位";
            var response = organizationManageService.SaveBedBasic(request);
            Assert.AreEqual(response.Data.BedDesc, "1号床位");
        }

        [TestMethod]
        public void TestModifyBedBasic()
        {
            BedBasic request = new BedBasic();
            request.BedNo = "1";
            request.OrgId = "1";
            request.BedDesc = "2号床位";
            var response = organizationManageService.SaveBedBasic(request);
            Assert.AreEqual(response.Data.BedDesc, "2号床位");
        }

        [TestMethod]
        public void TestQueryBedBasic()
        {
            BaseRequest<BedBasicFilter> request = new BaseRequest<BedBasicFilter>();
            request.Data.BedNo = "1";
            var response = organizationManageService.QueryBedBasic(request);
            Assert.AreEqual(response.Data.Count, 1);
        }

        [TestMethod]
        public void TestDeleteBedBasic()
        {
            var response = organizationManageService.DeleteBedBasic("1");
            Assert.AreEqual(response.ResultCode, 0);
        }
    }
}
