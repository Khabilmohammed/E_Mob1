
using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using E_mob_shoppy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public  IActionResult Create(Coupon obj)
        {
            var coupenExist =  _UnitOfWork.Coupon.Get(u => u.Code == obj.Code);
            var existingCouponNames = _UnitOfWork.Coupon.GetAll().Select(c => c.Code).ToList();
            if (existingCouponNames.Contains(obj.Code))
            {
                TempData["existingCouponNamesError"] = "Already Exits This Coupon";
                return View();
            }
            if (obj.DiscountAmount == null)
            {
                TempData["discountError"] = "please enter the discount amount";
                return View();
            }
            if (coupenExist == null)
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.Coupon.Add(obj);
                    _UnitOfWork.Save();
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
        public  ActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Coupon? couponFromDb =  _UnitOfWork.Coupon.Get(u => u.CouponId == id);

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
        public  ActionResult Update(Coupon? obj)
        {
            var existingCouponNames = _UnitOfWork.Coupon.GetAll().Where(c => c.CouponId != obj.CouponId).Select(c => c.Code).ToList();
            if (existingCouponNames.Contains(obj.Code))
            {
                TempData["existingCouponNamesError"] = "Already Exits This Coupon";
                return View();
            }
            if (obj.DiscountAmount == null)
            {
                TempData["discountError"] = "please enter the discount amount";
                return View();
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Coupon.Upadte(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Coupon Updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }




        #region APICALLS
        [HttpGet]
        public  IActionResult GetAll()
        {
            List<Coupon> objCouponList = _UnitOfWork.Coupon.GetAll().ToList();
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
