using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using Moq;
using SportsStore.WebUI.HtmlHelpers;
using System.Web.Mvc;
using System.Linq;
using SportsStore.WebUI.Models;

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
            ProductsListViewModel model = (ProductsListViewModel)controller.List(null,2).Model;
            //assert
            IEnumerable<Product> products = model.Products;
            Assert.IsTrue(products.ToArray().Length == 2);
            Assert.AreEqual(products.ToArray()[0].Name, "P4");
            Assert.AreEqual(products.ToArray()[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;
            // Arrange - create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            //act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            //assert
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>"
                + @"<a class=""selected"" href=""Page2"">2</a>"
                + @"<a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
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
            ProductsListViewModel model = (ProductsListViewModel)controller.List(null,2).Model;
            //assert
            Assert.AreEqual(model.Products.Count(), 2);
            Assert.AreEqual(model.pagingInfo.CurrentPage, 2);
            Assert.AreEqual(model.pagingInfo.TotalPages, 2);
            Assert.AreEqual(model.pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(model.pagingInfo.TotalItems, 5);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "1", Category = "Cat1" },
                new Product {ProductID = 2, Name = "2", Category = "Cat2" },
                new Product {ProductID = 3, Name = "3", Category = "Cat1" },
                new Product {ProductID = 4, Name = "4", Category = "Cat2" },
                new Product {ProductID = 5, Name = "5", Category = "Cat3" },
            }.AsQueryable);
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            //act
            ProductsListViewModel model = (ProductsListViewModel)controller.List("Cat1", 1).Model;
            Product[] products = model.Products.ToArray();
            //assert
            Assert.AreEqual(products.Length, 2);
            Assert.IsTrue(products[0].Name == "1" && products[0].Category == "Cat1");
            Assert.IsTrue(products[1].Name == "3" && products[1].Category == "Cat1");


        }
    }
}
