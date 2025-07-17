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
    public class AddressController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddressVM addressVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            addressVM.Address.ApplicationUserId = userId;

            if (ModelState.IsValid)
            {
                _unitOfWork.Address.Add(addressVM.Address);
                _unitOfWork.Save();

                TempData["success"] = "Address added successfully.";
                return RedirectToAction("Summary", "Cart", new { area = "Customer" });
            }

            TempData["error"] = "Validation failed.";
            return View(addressVM);
        }
    }
}
