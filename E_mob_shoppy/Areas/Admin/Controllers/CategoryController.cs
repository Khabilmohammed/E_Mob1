
using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using E_mob_shoppy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_mob_shoppy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork db)
        {
            _UnitOfWork = db;
        }
        public IActionResult Index()
        {
            var objCtegory = _UnitOfWork.Category.GetAll().ToList();
            return View(objCtegory);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display Order and Name should be different");
                ModelState.AddModelError("DisplayOrder", "Display Order and Name should be different");
            }

            var objCtegory = _UnitOfWork.Category.GetAll().ToList();
            var existingCategoryNames = _UnitOfWork.Category.GetAll().Select(c => c.Name).ToList();
            if(existingCategoryNames.Contains(obj.Name))
            {
                ModelState.AddModelError("Name", "Al Ready Exits");
            }

            if (ModelState.IsValid) {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "The data is Created";
                return RedirectToAction("Index", "Category");
            }
            return View();
            
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryfromdb = _UnitOfWork.Category.Get(u=>u.Category_Id==id);
            if (categoryfromdb == null)
            {

                return NotFound();
            }
            return View(categoryfromdb);
        }



        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display Order and Name should be different");
                ModelState.AddModelError("DisplayOrder", "Display Order and Name should be different");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Upadte(obj);
                _UnitOfWork.Save();
                TempData["success"] = "The data is updated";
                return RedirectToAction("Index", "Category");
            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryfromdb = _UnitOfWork.Category.Get(u => u.Category_Id == id);
            if (categoryfromdb == null)
            {

                return NotFound();
            }
            return View(categoryfromdb);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int? id) { 
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Category? category = _UnitOfWork.Category.Get(u => u.Category_Id == id);
            if(category == null)
            {
                return NotFound();
            }
            _UnitOfWork.Category.Remove(category);
            _UnitOfWork.Save();
            TempData["success"] = "The data is Removed";
            return RedirectToAction("Index", "Category");
        }
    }
}
