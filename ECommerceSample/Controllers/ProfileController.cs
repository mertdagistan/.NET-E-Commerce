using ECommerce.Entity;
using ECommerce.Repository;
using ECommerceSample.Areas.Admin.Models.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceSample.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        // GET: Profile
        InstanceResult<Member> result = new InstanceResult<Member>();
        MemberRepository mr = new MemberRepository();
        OrderRepository or = new OrderRepository();
        InvoiceRepository ir = new InvoiceRepository();
        public ActionResult ProfilePage(int id)
        {
            if (TempData["profileSuccess"]!=null)
            {
                ViewBag.Success = TempData["profileSuccess"];
            }
            else
            {
                ViewBag.Success = TempData["Success"];
            }
            
            int memberId = mr.List().ProcessResult.FirstOrDefault(t => t.FirstName == User.Identity.Name).UserId;
            if (memberId == id)
            {
                Member mb = mr.GetObjById(id).ProcessResult;
                return View(mb);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public ActionResult EditProfile(int id)
        {
            Member mb = mr.GetObjById(id).ProcessResult;
            return View(mb);
        }
        [HttpPost]
        public ActionResult EditProfile(Member model)
        {
            
            result.resultint = mr.UpdateProfile(model);
            if (result.resultint.ProcessResult>0)
            {
                TempData["profileSuccess"] = "Your profile information was successfully updated.";
                return Redirect("~/Profile/ProfilePage/"+model.UserId);
            }
            else
            {
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult EditPassword(int id,string m)
        {
            
            ViewBag.Message = TempData["Msg"];
            Member mb = mr.GetObjById(id).ProcessResult;
            return View(mb);
        }
        [HttpPost]
        public ActionResult EditPassword(Member model, string oldPassowrd)
        {
            if (oldPassowrd==mr.List().ProcessResult.FirstOrDefault(t=>t.UserId==model.UserId).Password)
            {
                result.resultint = mr.UpdatePw(model);
                if (result.resultint.ProcessResult > 0)
                {
                    TempData["Success"] = "Your password has been successfully changed.";
                    return Redirect("~/Profile/ProfilePage/" + model.UserId);
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                TempData["Msg"] = "You entered an incorrect old password. Please try again.";
                return Redirect("~/Profile/EditPassword/"+model.UserId);
            }
            

        }


        public ActionResult OrderHistory(int id)
        {

            OrderDetailRep or = new OrderDetailRep();
           

            return View(ir.List().ProcessResult.Where(t => t.Order.MemberId == id));
        }

        public ActionResult OrderInformation(int id)
        {
            Invoice inv = ir.GetObjById(id).ProcessResult;
            return View(inv);
        }

    }
}