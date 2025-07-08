
using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using E_mob_shoppy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;


namespace E_mob_shoppy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CouponController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;


        public CouponController(IUnitOfWork db)
        {
            _UnitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Coupon> objCoupon = _UnitOfWork.Coupon.GetAll().ToList();
            return View(objCoupon);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Create(Coupon obj)
        {
           
            obj.Code = obj.Code?.ToLower();

            
            var couponExist = _UnitOfWork.Coupon.Get(u => u.Code.ToLower() == obj.Code);

            if (couponExist != null)
            {
                TempData["existingCouponNamesError"] = "This coupon code already exists.";
                return View();
            }

            if (obj.DiscountAmount == null)
            {
                TempData["discountError"] = "Please enter the discount amount.";
                return View();
            }

            if (ModelState.IsValid)
            {
                _UnitOfWork.Coupon.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Coupon created successfully";
                return RedirectToAction("Index");
            }

            return View();
        }
        public  ActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Coupon? couponFromDb = _UnitOfWork.Coupon
    .Get(u => u.CouponId == id, tracked: false);

            if (couponFromDb == null)
                return NotFound();
            return View(couponFromDb);
        }
        
        public ActionResult wallet()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var walletExist = _UnitOfWork.ApplicationUser.Get(u => u.Id==userId);
            
            return View(walletExist);
        }

        [HttpPost]
        public IActionResult Update(Coupon obj)
        {
            if (obj == null)
                return NotFound();

            var duplicate = _UnitOfWork.Coupon
                .GetAll()
                .Any(c => c.Code == obj.Code && c.CouponId != obj.CouponId);

            if (duplicate)
            {
                TempData["existingCouponNamesError"] = "Already Exists This Coupon";
                return View(obj);
            }

            if (obj.DiscountAmount == null)
            {
                TempData["discountError"] = "Please enter the discount amount";
                return View(obj);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCoupon = _UnitOfWork.Coupon.Get(c => c.CouponId == obj.CouponId);

                    if (existingCoupon == null)
                        return NotFound();

                    existingCoupon.Code = obj.Code;
                    existingCoupon.DiscountAmount = obj.DiscountAmount;
                    existingCoupon.MinPurchaseAmount = obj.MinPurchaseAmount;
                    existingCoupon.MaxPurchaseAmount = obj.MaxPurchaseAmount;
                    existingCoupon.ExpiryDateTime = obj.ExpiryDateTime;
                    existingCoupon.StartDateTime = obj.StartDateTime;

                    _UnitOfWork.Save();
                    TempData["success"] = "Coupon updated successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "An error occurred while updating the coupon.";
                }
            }

            return View(obj);
        }






        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var objCouponList = _UnitOfWork.Coupon.GetAll().Select(c => new
            {
                couponId = c.CouponId,
                code = c.Code,
                discountAmount = c.DiscountAmount,
                minPurchaseAmount = c.MinPurchaseAmount,
                maxPurchaseAmount = c.MaxPurchaseAmount,
                expiryDateTime = c.ExpiryDateTime
            }).ToList();

            return Json(new { data = objCouponList });
        }
        [HttpDelete]
        public  IActionResult Delete(int? id)
        {
            var couponToBeDeleted =  _UnitOfWork.Coupon.Get(u => u.CouponId == id);
            if (couponToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _UnitOfWork.Coupon.Remove(couponToBeDeleted);
            _UnitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

    }
}
