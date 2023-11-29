
using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using E_mob_shoppy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_mob_shoppy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class OfferController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public OfferController(IUnitOfWork db)
        {
            _UnitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Offer> offerlist = _UnitOfWork.Offer.GetAll().ToList();
            return View(offerlist);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Offer> offerlist = _UnitOfWork.Offer.GetAll().ToList();
            return Json(new { data = offerlist });
        }
        #endregion
    }
}
