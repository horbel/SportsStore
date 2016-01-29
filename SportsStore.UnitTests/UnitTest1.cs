﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using Moq;
//using System.Web.Mvc;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1" },
                new Product {ProductID = 2, Name = "P2" },
                new Product {ProductID = 3, Name = "P3" },
                new Product {ProductID = 4, Name = "P4" },
                new Product {ProductID = 5, Name = "P5" },
            }.AsQueryable);
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            //act
            IEnumerable<Product> products = (IEnumerable<Product>)controller.List(2).Model;
            //assert
            Assert.IsTrue(products.ToArray().Length == 2);
            Assert.AreEqual(products.ToArray()[0].Name, "P4");
            Assert.AreEqual(products.ToArray()[1].Name, "P5");
        }
    }
}