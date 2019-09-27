using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Entity;
using ECommerce.Repository;
using ECommerce.Common;
using ECommerceSample.Areas.Admin.Models.ResultModel;
using System.Web.Security;
using ECommerceSample.Models;

namespace ECommerceSample.Controllers
{

    public class AccountController : Controller
    {

        // GET: Account
        InstanceResult<Member> result = new InstanceResult<Member>();
        MemberRepository mr = new MemberRepository();

        public ActionResult Register()
        {
            ViewBag.UserRole = new SelectList(UserRoleRepo.List(), "RoleId", "RoleName");
            return View();
        }
        [HttpPost]
        public ActionResult Register(Member model, List<string> UserRole)
        {
            if (UserRole.Contains(""))
            {
                return Redirect("~/Account/Register");
            }
            else
            {
                foreach (string item in UserRole)
                {
                    int UserId = Convert.ToInt32(item);
                    model.RoleId = UserId;
                }
                FormsAuthentication.SetAuthCookie(model.FirstName, true);

                result.resultint = mr.Insert(model);
                if (model.RoleId == 1)
                {
                    return Redirect("~/Admin/Product/List");
                }
                else if (model.RoleId == 2)
                {
                    return Redirect("~/Home/Index");
                }
            }
            return Redirect("~/Account/Register");

        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Error = TempData["Error"];
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Index");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Member model)
        {

            if (ModelState.IsValid)
            {

                using (MyECommerceEntities db = new MyECommerceEntities())
                {
                    var user = db.Members.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.FirstName, true);
                        if (user.RoleId == 1)
                        {
                            return Redirect("~/Admin/Product/List");
                        }
                        else if (user.RoleId == 2)
                        {
                            return Redirect("~/Home/Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Wrong Username/Email and password combination.";
                        return Redirect("~/Account/Login");
                    }

                }

            }
            return Redirect("~/Home/Index");
        }


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}