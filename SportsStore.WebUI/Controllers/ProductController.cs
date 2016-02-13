using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;


namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {        
        public int PageSize = 4;
        private IProductRepository repository;
        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }
        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel();
            model.Products = repository.Products
                .Where(p => category==null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);
            model.pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = category == null ? repository.Products.Count() :
                                                repository.Products
                                                    .Where(x => x.Category == category)
                                                    .Count()
            };
            model.CurrentCategory = category;
            
                             
            return View(model);            
                            
        }
       
    }
}


  //<connectionStrings>
  //  <add name = "DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-SportsStore.WebUI-20160127045513.mdf;Initial Catalog=aspnet-SportsStore.WebUI-20160127045513;Integrated Security=True" providerName="System.Data.SqlClient" />
  //</connectionStrings>