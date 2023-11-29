using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Upadte(Product obj)
        {
            var objFromDb=_db.Products.FirstOrDefault(u=>u.ProductId == obj.ProductId);
            if (objFromDb != null)
            {
                objFromDb.ProductDescription = obj.ProductDescription;
                objFromDb.ProductName = obj.ProductName;
                objFromDb.Listprice50 = obj.Listprice50;
                objFromDb.Listprice100 = obj.Listprice100;
                objFromDb.ListPrice=obj.Listprice100;
                objFromDb.ProductQuantity = obj.ProductQuantity;
                objFromDb.Category_Id = obj.Category_Id;
                
                objFromDb.Price = obj.Price;
                objFromDb.ProductImages = obj.ProductImages;
               /* if(obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;  
                }*/
            }
        }
    }
}
