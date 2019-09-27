using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Entity;
using ECommerce.Repository;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using ECommerceSample.Models;
using System.Net.Mime;

namespace ECommerceSample.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment

        public string GetEmailTemplate()
        {
            ViewData.Model = ViewData["model"];
            using (StringWriter stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "HtmlTemplate");
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        [HttpGet]
        public ActionResult Pay()
        {
            ViewBag.PaymentTypes = new SelectList(PaymentRepository.List(), "PaymentId", "PaymentName");
            ViewBag.Shipper = new SelectList(ShipperRepository.List(), "Id", "ShipperName");
            ViewBag.ShippingError = TempData["ShippingError"];
            ViewBag.PaymentError = TempData["PaymentError"];
            return View();
        }

        [HttpPost]
        public ActionResult Pay(Invoice model, List<string> PaymentTypes)
        {
            if (PaymentTypes.Contains("") && ((Order)Session["Order"]).ShipperId == null)
            {
                TempData["PaymentError"] = "Please select the preferred payment method to use on this order.";
                TempData["ShippingError"] = "Please select the preferred payment method to use on this order.";
                return RedirectToAction("Pay", "Payment");
            }
            if (((Order)Session["Order"]).ShipperId == null)
            {
                TempData["ShippingError"] = "Please select the preferred payment method to use on this order.";
                return RedirectToAction("Pay", "Payment");
            }
            if (PaymentTypes.Contains(""))
            {
                TempData["PaymentError"] = "Please select the preferred payment method to use on this order.";
                return RedirectToAction("Pay", "Payment");
            }
            else
            {
                model.PaymentDate = DateTime.Now;
                foreach (string item in PaymentTypes)
                {
                    int PaymentId = Convert.ToInt32(item);
                    model.PaymentTypeId = PaymentId;
                }
                model.Address = ((Order)Session["Order"]).Member.Address;
                model.OrderId = ((Order)Session["Order"]).OrderId;
                InvoiceRepository ip = new InvoiceRepository();
                if (ip.Insert(model).IsSucceeded)
                {
                    Order ord = (Order)Session["Order"];
                    OrderRepository ordrep = new OrderRepository();
                    ord.IsPay = true;
                    ordrep.Update(ord);

                    ViewData["model"] = model;
                    var body = GetEmailTemplate();
                    body.Replace("C2", "C2");
                    SendMail sMail = new SendMail();
                    if (sMail.mailGonder(model.Order.Member.Email,  body, model.Order.OrderId,model.Order.Member.FirstName,model.Order.Member.LastName))
                    {
                        ViewBag.Mesaj = "Mesaj iletildi";
                    }
                    else
                    {
                        ViewBag.Mesaj = "Mesaj iletilmedi";
                    }
                    Session.Abandon();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(model);
                }
            }

        }
    }
}