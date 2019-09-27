using ECommerce.Common;
using ECommerce.Entity;
using ECommerce.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerceSample.Areas.Admin.Models.ResultModel;
using ECommerceSample.Areas.Admin.Models.ViewModel;
using System.IO;

namespace ECommerceSample.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: Admin/Product
        ProductRepository pr = new ProductRepository();
        //Result<List<Product>> result = new Result<List<Product>>();
        //Result<int> resultint = new Result<int>();
        //Result<Product> resultt = new Result<Product>();
        InstanceResult<Product> result = new InstanceResult<Product>();
        CategoryRepository cr = new CategoryRepository();
        BrandRepository br = new BrandRepository();

         
        public ActionResult List()
        {
            result.resultList = pr.List();
            return View(result.resultList.ProcessResult);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            ViewBag.Message = TempData["Msg"];
            ProductViewModel pwm = new ProductViewModel();
            List<SelectListItem> CatList = new List<SelectListItem>();
            List<SelectListItem> BrandList = new List<SelectListItem>();
            foreach (Category item in cr.List().ProcessResult)
            {
                CatList.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });
            }
            foreach (Brand item in br.List().ProcessResult)
            {
                BrandList.Add(new SelectListItem { Value = item.BrandId.ToString(), Text = item.BrandName });
            }
            pwm.BrandList = BrandList;
            pwm.CategoryList = CatList;
            pwm.Product = null;
            return View(pwm);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductViewModel model, HttpPostedFileBase photo)
        {
            string PhotoName = "";
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                    string path = Server.MapPath("~/Upload/" + PhotoName);
                    photo.SaveAs(path);
                }
                model.Product.Photo = PhotoName;
                result.resultint = pr.Insert(model.Product);
                if (result.resultint.ProcessResult > 0)
                {
                    return RedirectToAction("List");
                }
                return View(model);
            }
            else
            {
                TempData["Msg"] = "Please Upload Pictures";
                return Redirect("~/Admin/Product/AddProduct");
            }
           
            
        }

        public ActionResult Edit(int id)
        {
            ProductViewModel pwm = new ProductViewModel();
            List<SelectListItem> CatList = new List<SelectListItem>();
            List<SelectListItem> BrandList = new List<SelectListItem>();
            foreach (Category item in cr.List().ProcessResult)
            {
                CatList.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });
            }
            foreach (Brand item in br.List().ProcessResult)
            {
                BrandList.Add(new SelectListItem { Value = item.BrandId.ToString(), Text = item.BrandName });
            }
            pwm.BrandList = BrandList;
            pwm.CategoryList = CatList;
            pwm.Product = pr.GetObjById(id).ProcessResult;
            return View(pwm);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel model, HttpPostedFileBase photo)
        {
            string photoName = model.Product.Photo;
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    string ext = Path.GetExtension(photo.FileName);
                    photoName = Guid.NewGuid().ToString().Replace("-", "");
                    if (ext == ".jpg")
                        photoName += ext;
                    else if (ext == ".png")
                        photoName += ext;
                    else if (ext == ".bmp") 
                    photoName += ext;
                    else
                    {
                        ViewBag.Mesaj = "Lutfen .jpg,.png,.bmp tipinde resim yukleyiniz";

                        return View(model);
                    }
                    string path = Server.MapPath("~/Upload/" + photoName);
                    photo.SaveAs(path);
                }
            }
            model.Product.Photo = photoName;
            result.resultint = pr.Update(model.Product);
            if (result.resultint.ProcessResult > 0)
                return RedirectToAction("List");
            else
                return View(model);
        }

        public ActionResult Delete(int id)
        {
            result.resultint = pr.Delete(id);
            return RedirectToAction("List", new { @m = result.resultint.UserMessage, @id = id });
        }
    }
}