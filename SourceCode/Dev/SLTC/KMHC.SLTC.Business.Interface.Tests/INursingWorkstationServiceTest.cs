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
    public class INursingWorkstationServiceTest
    {
        INursingWorkstationService nursingWorkstationService = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        [TestMethod]
        public void TestSaveVitalsign()
        {
            List<Vitalsign> request = new List<Vitalsign>();
            request.Add(new Vitalsign { OrgId = "001", Bloodsugar = 10 });
            request.Add(new Vitalsign { OrgId = "001", Bloodsugar = 20 });
            request.Add(new Vitalsign { OrgId = "002", Bloodsugar = 30 });
            var response = nursingWorkstationService.SaveVitalsign(request);
            Assert.AreEqual(response.ResultCode, 0);
        }

        [TestMethod]
        public void TestGenerateCode()
        {
            var code = nursingWorkstationService.GenerateCode("0", Entity.EnumCodeKey.Demo);
            Assert.AreEqual(code[0], 'D');
        }
    }
}
