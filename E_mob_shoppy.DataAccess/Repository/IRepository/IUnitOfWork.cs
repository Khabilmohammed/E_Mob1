using E_mob_shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }

        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUsersRepository ApplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        ICouponRepository Coupon { get; }
        IWishlistRepository Wishlist { get; }
        IProductImageRepository ProductImage { get; }
       
        IOfferRepository Offer { get; }

        IWalletHistoryRepository WalletHistory { get; }
        IRepository<Address> Address { get; }

        void Save();
    }
}
