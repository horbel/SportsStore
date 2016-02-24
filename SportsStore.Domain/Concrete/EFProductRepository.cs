using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;

namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Product> Products
        {
            get
            {
                return context.Products;
            }
        }

        public Product DeleteProduct(int productID)
        {
            Product dbEntity = context.Products.Find(productID);
            if(dbEntity!=null)
            {
                context.Products.Remove(dbEntity);
                context.SaveChanges();
            }
            return dbEntity;
        }

        public void SaveProduct(Product product)
        {
            if(product.ProductID==0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntity = context.Products.Find(product.ProductID);
                if(dbEntity!=null)
                {
                    dbEntity.Name = product.Name;
                    dbEntity.Category = product.Category;
                    dbEntity.Description = product.Description;
                    dbEntity.Price = product.Price;
                }
                
            }
            context.SaveChanges();
        }
    }
}
