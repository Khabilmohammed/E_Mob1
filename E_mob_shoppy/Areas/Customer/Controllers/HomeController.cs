using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using System.Diagnostics;
using System.Security.Claims;


namespace E_mob_shoppy.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string searchQuery, string category, int page = 1, int pageSize = 20)
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties:"Category,ProductImages") ;
			if (!string.IsNullOrEmpty(searchQuery))
			{
				productList = productList.Where(p => p.ProductName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
			}
			if (!string.IsNullOrEmpty(category) && category != "All category")
			{
				productList = productList.Where(p => p.Category.Name==category);
			}

            int totalProducts = productList.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            productList = productList.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            return View(productList);
        }



        public IActionResult Details(int id)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.ProductId == id, includeProperties: "Category,ProductImages"),
                count = 1,
                ProductId = id
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId=userId;

            ShoppingCart cartFromDb=_unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId==userId && u.ProductId==shoppingCart.ProductId);
            Product product = _unitOfWork.Product.Get(p => p.ProductId == shoppingCart.ProductId);

            if (shoppingCart.count > product.ProductQuantity)
            {
                var quantiy = product.ProductQuantity;
                TempData["StockErorr"] = "Reduce the quantity you have entered !!!! stock is only up to "+quantiy;
                return RedirectToAction(nameof(Details));

            }
            if (cartFromDb != null)
            {
                cartFromDb.count += shoppingCart.count;
                _unitOfWork.ShoppingCart.Upadte(cartFromDb );
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            TempData["success"] = "cart updated successfully";

            _unitOfWork.Save();
           
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}