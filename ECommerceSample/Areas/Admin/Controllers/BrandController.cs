using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Common;
using ECommerce.Entity;
using ECommerce.Repository;
using ECommerceSample.Areas.Admin.Models.ResultModel;

namespace ECommerceSample.Areas.Admin.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        // GET: Admin/Brand

        BrandRepository br = new BrandRepository();
        //Result<List<Brand>> resultList = new Result<List<Brand>>();
        //Result<int> resultint = new Result<int>();
        //Result<Brand> brandResult = new Result<Brand>();
        InstanceResult<Brand> result = new InstanceResult<Brand>();
        public ActionResult List()
        {
            result.resultList = br.List();
            return View(result.resultList.ProcessResult);
        }
        [HttpGet]
        public ActionResult AddBrand()
        {
            ViewBag.Message = TempData["Msg"];
            Brand b = new Brand();
            b.Photo = "denemeTestdeneme";
            return View(b);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddBrand(Brand model, HttpPostedFileBase photoPath)
        {
            string PhotoName = "";
            if (photoPath != null)
            {
                if (photoPath.ContentLength > 0)
                {
                    PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                    string path = Server.MapPath("~/Upload/" + PhotoName);
                    photoPath.SaveAs(path);
                }
                model.Photo = PhotoName;
                if (ModelState.IsValid)
                {
                    result.resultint = br.Insert(model);
                    if (result.resultint.IsSucceeded)
                        return RedirectToAction("List");
                    else
                    {
                        ViewBag.Mesaj = result.resultint.UserMessage;
                        return View(model);
                    }
                }
                else
                    return View();
            }
            else
            {
                TempData["Msg"] = "Please Upload Pictures";
                return Redirect("~/Admin/Brand/AddBrand");
            }
            
           
        }

        public ActionResult Edit(int id)
        {
            result.TResult = br.GetObjById(id);
            return View(result.TResult.ProcessResult);
        }

        [HttpPost]
        public ActionResult Edit(Brand model, HttpPostedFileBase photoPath)
        {
            string PhotoName = model.Photo;
            if (photoPath != null)
            {
                if (photoPath.ContentLength > 0)
                {
                    PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                    string path = Server.MapPath("~/Upload/" + PhotoName);
                    photoPath.SaveAs(path);
                }
            }
            model.Photo = PhotoName;
            result.resultint = br.Update(model);
            if (result.resultint.IsSucceeded)
            {
                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            result.resultint = br.Delete(id);
            return RedirectToAction("List", new { @mesaj = result.resultint.UserMessage });
        }
    }
}