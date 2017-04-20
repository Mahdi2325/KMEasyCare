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
    public class IResidentManageServiceTest
    {
        IResidentManageService residentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();

        [TestMethod]
        public void TestSavePerson()
        {
            Person request = new Person();
            request.Name = "testPersonName";
            //request.Address = new Relation();
            //request.Address.Mobile = "13912341234";
            //request.Contact = new List<RelationDtl>();
            //request.Contact.Add(new RelationDtl { Name = "testRelationDtl1" });
            //request.Contact.Add(new RelationDtl { Name = "testRelationDtl2" });
            //request.Contact.Add(new RelationDtl { Name = "testRelationDtl3" });
            request.AttachArchives = new List<AttachFile>();
            request.AttachArchives.Add(new AttachFile { DocPath = "testAttachFile1" });
            request.AttachArchives.Add(new AttachFile { DocPath = "testAttachFile2" });
            request.AttachArchives.Add(new AttachFile { DocPath = "testAttachFile3" });
            var response = residentManageService.SavePerson(request);
            Assert.AreEqual(response.ResultCode, 0);
        }
    }
}
