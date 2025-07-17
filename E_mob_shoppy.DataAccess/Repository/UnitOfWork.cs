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
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUsersRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public ICouponRepository Coupon { get; private set; }

        public IWishlistRepository Wishlist { get; private set; }

        public IProductImageRepository ProductImage { get; private set; }

        public IOfferRepository Offer { get; private set; }
        public IWalletHistoryRepository WalletHistory { get; private set; } 


        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            ApplicationUser=new ApplicationUserRepository(_db);
            ShoppingCart =new ShoppingCartRepository(_db);
            Category =new CategoryRepository(_db);
            Product=new ProductRepository(_db);
            OrderHeader =new OrderHeaderRepository(_db);
            OrderDetail =new OrderDetailRepository(_db);
            Coupon = new CouponRepository(_db);
            Wishlist=new WishlistRepository(_db);
            ProductImage=new ProductImageRepository(_db);
            Offer=new OfferRepository(_db);
            WalletHistory = new WalletHistoryRepository(_db);
        }
       

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
