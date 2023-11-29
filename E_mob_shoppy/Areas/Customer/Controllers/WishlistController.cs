using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using E_mob_shoppy.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_mob_shoppy.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public WishlistVM WishlistVm { get; set; }
        public WishlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
           

            var WishlistItems = _unitOfWork.Wishlist.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Product.ProductImages"
            );
           

          
            return View(WishlistItems);
        }
        [HttpPost]
        public ActionResult AddToWishlist(int productId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
          
            
            var existingWishlistItem = _unitOfWork.Wishlist.Get(u => u.ApplicationUserId == claim.Value && u.ProductId == productId);
            Product product = _unitOfWork.Product.Get(p => p.ProductId == productId);
           
           
            if (existingWishlistItem == null)
            { 
                var newWishlistItem = new Wishlist
                {
                    
                    ApplicationUserId = claim.Value,
                    ProductId = productId,
                    Description = product.ProductDescription,
                    ProductName=product.ProductName,
                    
                };

                _unitOfWork.Wishlist.Add(newWishlistItem);
                _unitOfWork.Save();

                TempData["SuccessMessage"] = "Product added to Wishlist successfully.";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        public ActionResult RemoveFromWishlist(int id)
        {
            var WishlistItem = _unitOfWork.Wishlist.Get(u => u.WishlistId == id);

            if (WishlistItem != null)
            {
                _unitOfWork.Wishlist.Remove(WishlistItem);
                _unitOfWork.Save();

               
                TempData["SuccessMessage"] = "Product removed from Wishlist successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Wishlist item not found.";
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
