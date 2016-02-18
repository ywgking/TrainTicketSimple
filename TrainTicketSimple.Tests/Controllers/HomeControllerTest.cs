using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrainTicektSimple;
using TrainTicektSimple.Controllers;

namespace TrainTicektSimple.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

      
        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void PostTicket()
        {

            HomeController controller = new HomeController();
            int Dates = int.Parse(DateTime.Now.AddDays(2).ToString("yyyyMMdd"));
            string Trans="CRH-001";
          
            Random myrandom = new Random(DateTime.Now.Millisecond);
            for(int i=0;i<10000;i++)
            {
              int start=  myrandom.Next(10)+1;
              int steps = myrandom.Next(10-start);

              ViewResult result = controller.PostTicket(Dates, Trans, start,start+steps) as ViewResult;
              Assert.IsNotNull(result.ViewBag.Message);
            }
        }
    }
}
