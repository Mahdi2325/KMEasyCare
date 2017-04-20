using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KM.Common;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface.Tests
{
    [TestClass]
    public class IDictManageServiceTest
    {
        IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();

        [TestMethod]
        public void TestSaveVitalsign()
        {
            CodeFilter request = new CodeFilter();
            request.ItemType = "A00.001";
            var response = dictManageService.QueryCode(request);
            response = dictManageService.QueryCode(request);
            Assert.AreEqual(response.ResultCode, 0);
        }
    }
}
