using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTful_API.Controllers;
using RESTful_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace crse_test
{
    [TestClass]
    public class ChildrenUnitTest 
    {

        [TestMethod]
        public void GetAllChildren()
        {
            var controller = new ChildrenController();
            List<SwimmerViewModel> result = controller.GetChildren();
            Assert.IsNotNull(result.Count());
        }
    }
}
