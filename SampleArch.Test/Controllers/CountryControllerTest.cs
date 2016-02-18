using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleArch.Controllers;
using SampleArch.Model;
using SampleArch.Service;
using System.Web.Mvc;

namespace SampleArch.Test.Controllers
{
    [TestClass]
    public class CountryControllerTest
    {
        private Mock<ICountryService> _countryServiceMock;
        CountryController _objController;
        List<Country> _listCountry;

        [TestInitialize]
        public void Initialize()
        {

            _countryServiceMock = new Mock<ICountryService>();
            _objController = new CountryController(_countryServiceMock.Object);
            _listCountry = new List<Country>() {
             new Country() { Id = 1, Name = "US" },
             new Country() { Id = 2, Name = "India" },
             new Country() { Id = 3, Name = "Russia" }
            };
        }



        [TestMethod]
        public void Country_Get_All()
        {
            //Arrange
            _countryServiceMock.Setup(x => x.GetAll()).Returns(_listCountry);

            //Act
            var result = ((_objController.Index() as ViewResult).Model) as List<Country>;

            //Assert
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual("US", result[0].Name);
            Assert.AreEqual("India", result[1].Name);
            Assert.AreEqual("Russia", result[2].Name);

        }

        [TestMethod]
        public void Valid_Country_Create()
        {
            //Arrange
            Country c = new Country() { Name = "test1"};

            //Act
            var result = (RedirectToRouteResult)_objController.Create(c);

            //Assert 
            _countryServiceMock.Verify(m => m.Add(c), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["action"]);
           
        }

        [TestMethod]
        public void Invalid_Country_Create()
        {
            // Arrange
            Country c = new Country() { Name = ""};
            _objController.ModelState.AddModelError("Error", "Something went wrong");

            //Act
            var result = (ViewResult)_objController.Create(c);

            //Assert
            _countryServiceMock.Verify(m => m.Add(c), Times.Never);
            Assert.AreEqual("", result.ViewName);
        }

    }
}
