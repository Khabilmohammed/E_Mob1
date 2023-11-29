using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using E_mob_shoppy.Models.ViewModel;
using E_mob_shoppy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;
using System.Collections.Generic;
using Product = E_mob_shoppy.Models.Product;

namespace E_mob_shoppy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController:Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }   


        public IActionResult index()
        {
            List<Product> productlist=_unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
           
            return View(productlist);
        }


        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category
               .GetAll().Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Category_Id.ToString()
               }),
                Product = new Product()
            };

            if(id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.ProductId == id, includeProperties: "ProductImages") ;
                return View(productVM);
            }
           
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,List<IFormFile> files)
        {
            
            if (ModelState.IsValid)
            {
                if (productVM.Product.ProductId == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Upadte(productVM.Product);
                }

                _unitOfWork.Save();
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (IFormFile file in files) 
                    {

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"Images\Products\product-" + productVM.Product.ProductId;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        using (var filestream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl= @"\"+productPath+@"\"+fileName,
                            ProductId=productVM.Product.ProductId,
                        };

                        if (productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>(); 
                        }

                        productVM.Product.ProductImages.Add(productImage);

                    }
                    _unitOfWork.Product.Upadte(productVM.Product);
                    _unitOfWork.Save();
                  
                }


                TempData["success"] = "The Data is Created/updated Successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {

                productVM.CategoryList = _unitOfWork.Category
          .GetAll().Select(u => new SelectListItem
          {
              Text = u.Name,
              Value = u.Category_Id.ToString()
          });
                return View(productVM);
            }
        }

        public IActionResult DeleteImage(int ImageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.productImageId == ImageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {

                    var OldImagePath =
                         Path.Combine(_webHostEnvironment.WebRootPath,
                         imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                    }

                }
                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }
       


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objproductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objproductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var producteToBeDeleted = _unitOfWork.Product.Get(u => u.ProductId == id);

            if (producteToBeDeleted == null)
            {
                return Json(new {sucess=false,message="Eroor while deleting"});
            }
            /* var OldImagePath =
                           Path.Combine(_webHostEnvironment.WebRootPath,
                           producteToBeDeleted.ImageUrl.TrimStart('\\'));

             if (System.IO.File.Exists(OldImagePath))
             {
                 System.IO.File.Delete(OldImagePath);
             }*/
            string productPath = @"Images\Products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (!Directory.Exists(finalPath))
            {
                string[] filePath = Directory.GetFiles(finalPath);

                foreach (string filePath2 in filePath)
                {
                    System.IO.File.Delete(filePath2);
                }
                Directory.Delete(finalPath);
            }

            _unitOfWork.Product.Remove(producteToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true,message="Deleted Successfully" });
        }


        #endregion
    }
}
