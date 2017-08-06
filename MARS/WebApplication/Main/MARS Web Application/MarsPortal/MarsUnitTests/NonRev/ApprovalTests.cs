using System;
using Mars.App.Classes.Phase4Dal.NonRev;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.NonRev
{
    [TestClass]
    public class ApprovalTests
    {
        [TestMethod]
        public void ApprovalHistory()
        {
            using (var dataAccess = new ApprovalDataAccess(null))
            {
                dataAccess.BuildApprovalList(5);
            }
        }
    }
}
