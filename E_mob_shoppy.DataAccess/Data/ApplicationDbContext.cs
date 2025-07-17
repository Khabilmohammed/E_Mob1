using E_mob_shoppy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace E_mob_shoppy.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        } 

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<Wishlist> wishlists { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<WalletHistory> WalletHistories { get; set; }

        public DbSet<Offer> offers { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData
                (new Category { Category_Id = 1, Name = "Mobile", Description = "HHhihkj", DisplayOrder = 1 },
                new Category { Category_Id = 2, Name = "Laptop", Description = "HHhihkj", DisplayOrder = 1 }
                
                );



            modelBuilder.Entity<Coupon>().HasData
               (new Coupon { CouponId = 1, Code = "abc123", DiscountAmount = 50 },
               new Coupon { CouponId = 2, Code = "abcd123", DiscountAmount = 100 }

               );


            modelBuilder.Entity<Offer>().HasData
               (new Offer { OfferId = 1, OfferName = "Summer Offer", Offertype = "Product", OfferDiscount = 10.00, OfferDescription = "Clearance items at discounted prices" },
               new Offer { OfferId = 2, OfferName = "ChristmasOffer", Offertype = "Category", OfferDiscount = 20.98, OfferDescription = "up 30 price at discounted " }

               );


            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductName = "Apple iPhone 15",
                    ProductDescription = "iPhone 15. boasts an advanced dual-camera system that allows you to click mesmerising pictures with immaculate clarity. Furthermore, the lightning-fast A15 Bionic chip allows for seamless multitasking, elevating your performance to a new dimension. A big leap in battery life, a durable design, and a bright Super Retina XDR display facilitate boosting your user experience.",
                    ListPrice = 620000,
                    Price = 59900,
                    Listprice50 = 57000,
                    Listprice100 = 50000,
                    ProductQuantity = 1000,
                    Category_Id = 1,
                   


                },


                 new Product
                 {
                     ProductId = 2,
                     ProductName = "Samsung z Fold",
                     ProductDescription = "Samsung zfold. boasts an advanced dual-camera system that allows you to click mesmerising pictures with immaculate clarity. Furthermore, the lightning-fast A15 Bionic chip allows for seamless multitasking, elevating your performance to a new dimension. A big leap in battery life, a durable design, and a bright Super Retina XDR display facilitate boosting your user experience.",
                     ListPrice = 660000,
                     Price = 52900,
                     Listprice50 = 51000,
                     Listprice100 = 49000,
                     ProductQuantity = 1000,
                     Category_Id = 1,
                     


                 },



                  new Product
                  {
                      ProductId = 3,
                      ProductName = "Apple iPhone 13",
                      ProductDescription = "\r\niPhone 13. boasts an advanced dual-camera system that allows you to click mesmerising pictures with immaculate clarity. Furthermore, the lightning-fast A15 Bionic chip allows for seamless multitasking, elevating your performance to a new dimension. A big leap in battery life, a durable design, and a bright Super Retina XDR display facilitate boosting your user experience.",
                      ListPrice = 620000,
                      Price = 59900,
                      Listprice50 = 57000,
                      Listprice100 = 50000,
                      ProductQuantity = 1000, 
                      Category_Id = 1,
                     


                  }

                );




        }
    }
}
