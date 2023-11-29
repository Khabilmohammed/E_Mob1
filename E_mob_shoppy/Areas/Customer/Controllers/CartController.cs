using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using E_mob_shoppy.Models.ViewModel;
using E_mob_shoppy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace E_mob_shoppy.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool isCartEmpty = true;

            shoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader=new()

            };

			IEnumerable<ProductImage> productImages = _unitOfWork.ProductImage.GetAll();
            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
				cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.ProductId).ToList();
                cart.Price = GetPriceBasedOnQuatity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.count);
                isCartEmpty = false;
            }
            if (isCartEmpty)
            {
                // You can set a message in ViewBag or ViewData to display in your view
                ViewBag.EmptyCartMessage = "Your shopping cart is empty.";
            }
            return View(shoppingCartVM);
        }

		public IActionResult ShowCoupon()
		{
            var validCoupon = _unitOfWork.Coupon.GetAll();
            return View(validCoupon);
		}

        public IActionResult Summary()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


			shoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
				OrderHeader = new()
			};

            shoppingCartVM.OrderHeader.ApplicationUserId=userId;
			shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            shoppingCartVM.OrderHeader.Name=shoppingCartVM.OrderHeader.ApplicationUser.Name;
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCartVM.OrderHeader.streetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.postalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            shoppingCartVM.OrderHeader.state = shoppingCartVM.OrderHeader.ApplicationUser.State;
            
            
           
          
                foreach (var cart in shoppingCartVM.ShoppingCartList)
                {
                    cart.Price = GetPriceBasedOnQuatity(cart);
                    shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.count);
                }
               

            return View(shoppingCartVM);
        }


		
		public  IActionResult Coupon(string coupon, int? OrderTotal)
		{
			if (string.IsNullOrEmpty(coupon) || OrderTotal == null)
			{
				return BadRequest(); 
			}

			var validCoupon =  _unitOfWork.Coupon.Get(u => u.Code == coupon);

			if (validCoupon != null)
			{
				if (OrderTotal >= validCoupon.DiscountAmount)
				{
					double discountPrice;
					double cartTotal = Convert.ToDouble(OrderTotal);
					if (validCoupon.DiscountAmount > 0)
					{
						discountPrice = (double)(cartTotal - validCoupon.DiscountAmount);
					}
					else
					{
						discountPrice = (double)(cartTotal - (cartTotal) * (validCoupon.DiscountAmount / 100));
					}

				     double newTotal = (double)(OrderTotal - discountPrice);

					var response = new
					{
						success = true,
						discountPrice,
						newTotal   
					};

					return Json(response); // Return the discount price
				}
				else
				{
					TempData["error"] = "Order total is below the minimum purchase amount.";

					var responses = new
					{
						success = false,
						errorMessage = "Order total is below the minimum purchase amount."
					};
					return Json(responses);
					// Return an appropriate error response
				}
			}
			TempData["error"] = "Coupon not found.";
			var responsed = new
			{
				success = false,
				errorMessage = "Coupon not found"
			};
			return Json(responsed);
		}



		public  double CouponCheckout(string coupon, int? OrderTotal)
		{
			if (string.IsNullOrEmpty(coupon) || OrderTotal == null)
			{
				return 0; 
			}

			var couponObj =  _unitOfWork.Coupon.Get(u => u.Code == coupon);

			if (couponObj != null)
			{
				if (OrderTotal >= couponObj.DiscountAmount)
				{
					double newTotal;
					double cartTotal = Convert.ToDouble(OrderTotal);
					if (couponObj.DiscountAmount > 0)
					{
						newTotal = (double)(cartTotal - couponObj.DiscountAmount);
					}
					else
					{
						newTotal = (double)(cartTotal - (cartTotal) * (couponObj.DiscountAmount / 100));
					}

					double discountPrice = (double)(OrderTotal - newTotal);

					return discountPrice; // Return the discount price
				}
				else
				{
					TempData["error"] = "Order total is below the minimum purchase amount.";
					return 0;
					// Return an appropriate error response
				}
			}
			TempData["error"] = "Coupon not found.";
			return 0;
		}





		[HttpPost]
        [ActionName("Summary")]
		public ActionResult SummaryPost()
		{


			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");
				
            shoppingCartVM.OrderHeader.OrderDate=System.DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUserId = userId;
		
			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuatity(cart);
				shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.count);
			}
			


			shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            _unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Save();



            foreach(var cart in shoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = shoppingCartVM.OrderHeader.OrderHeaderId,
                    Price= cart.Price,  
                    Count=cart.count,
                };

				_unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
			}
			if (!string.IsNullOrEmpty(shoppingCartVM.OrderHeader.AppliedCouponCode))
			{
				var couponResponse = CouponCheckout(shoppingCartVM.OrderHeader.AppliedCouponCode, (int)shoppingCartVM.OrderHeader.OrderTotal);

				if (couponResponse != null)
				{
					// Update the order total with the new total
					shoppingCartVM.OrderHeader.DiscountAmount = couponResponse;
					shoppingCartVM.OrderHeader.OrderTotal -= (double)couponResponse;

				}
				else
				{
					// Handle the error case when the coupon is not valid or the order total is below the minimum purchase amount

					return RedirectToAction(nameof(Index));
				}
			}
			double totalAmountStripe = shoppingCartVM.OrderHeader.OrderTotal;
			var domain = "https://localhost:44346/";
			
			var options = new SessionCreateOptions
			{

				SuccessUrl = domain + $"customer/cart/OrderConformation?id={shoppingCartVM.OrderHeader.OrderHeaderId}",
				CancelUrl = domain + "customer/cart/Index",

				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = "usd",
							UnitAmount = (long)(totalAmountStripe * 100),
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = "E_MOb_Shoppy",
								Description = "Paying through Stripe"
							}
						},
						Quantity = 1
					}
				},
				Mode = "payment",
		};


            /*foreach (var item in shoppingCartVM.ShoppingCartList)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(totalAmountStripe * 100),

						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.ProductName
						}
					},
					Quantity = item.count
				};
				options.LineItems.Add(sessionLineItem);
			}*/
            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuatity(cart);
                shoppingCartVM.OrderHeader.OrderTotal = (cart.Price * cart.count);

                // Update product stock or quantity
                var product = _unitOfWork.Product.Get(p => p.ProductId == cart.ProductId);
                if (product != null)
                {
                    product.ProductQuantity -= cart.count;
                    _unitOfWork.Product.Upadte(product);
                }
            }

            var service = new SessionService();
				Session session = service.Create(options);
				_unitOfWork.OrderHeader.UpdateStripePaymentId(shoppingCartVM.OrderHeader.OrderHeaderId, session.Id, session.PaymentIntentId);
				_unitOfWork.Save();
				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);
				return RedirectToAction(nameof(OrderConformation), new { id = shoppingCartVM.OrderHeader.OrderHeaderId });
		}


        public IActionResult OrderConformation(int id)
        {


            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u=>u.OrderHeaderId == id,includeProperties:"ApplicationUser");
            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayed)
            {
                //this order of cs
                var service=new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(id,session.Id,session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
				}
			}

            List<ShoppingCart> shoppingCarts= _unitOfWork.ShoppingCart.
                GetAll(u=>u.ApplicationUserId==orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }

		public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.CartId == cartId);
            cartFromDb.count += 1;
            _unitOfWork.ShoppingCart.Upadte(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.CartId == cartId);
            if (cartFromDb.count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.count -= 1;
                _unitOfWork.ShoppingCart.Upadte(cartFromDb);
            }


            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.CartId == cartId);

            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuatity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.count <= 50)
            {
                return shoppingCart.Product.ListPrice;
            }
            else
            {
                if (shoppingCart.count <= 100)
                {
                    return shoppingCart.Product.Listprice50;
                }
                else
                {
                    return shoppingCart.Product.Listprice100;
                }
            }
        }
    }
}
