using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using System.Collections.Generic;
using KMHC.Infrastructure.Security;

namespace KMHC.SLTC.Business.Interface.Tests
{
    [TestClass]
    public class ICostManageServiceTest
    {
        ICostManageService costManageService = IOCContainer.Instance.Resolve<ICostManageService>();

        [TestMethod]
        public void TestGenerateBill()
        {
            //IAuthenticationService authenticationService = IOCContainer.Instance.Resolve<IAuthenticationService>();
            //ClientUserData clientUserData = new ClientUserData()
            //{
            //    OrgId = "1"
            //};
            //authenticationService.SignIn(clientUserData, true);
            //var response = costManageService.GenerateBill();
            //Assert.AreEqual(response.ResultCode, 0);
        }
    }
}
