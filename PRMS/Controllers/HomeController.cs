using PRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRMS.Controllers
{
    public class HomeController : Controller
    {
        private PRMSEntities db = new PRMSEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check in Tenants table
                var tenant = db.Tenants.SingleOrDefault(t => t.Email == model.Email && t.Password == model.Password);
                if (tenant != null)
                {
                    Session["Role"] = "Tenant";
                    Session["UserId"] = tenant.TenantId;
                    return RedirectToAction("Index", "Home");
                }

                // Check in Owners table
                var owner = db.PropertyOwners.SingleOrDefault(o => o.Email == model.Email && o.Password == model.Password);
                if (owner != null)
                {
                    Session["Role"] = "Owner";
                    Session["UserId"] = owner.OwnerId;
                    return RedirectToAction("Index", "Home");
                }

                // Check in Manager table
                var manager = db.PropertyManagers.SingleOrDefault(m => m.Email == model.Email && m.Password == model.Password);
                if (manager != null)
                {
                    Session["Role"] = "Manager";
                    Session["UserId"] = manager.PropertyManagerId;
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Role == "Owner")
                {
                    var owner = new PropertyOwner
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Name
                    };
                    db.PropertyOwners.Add(owner);
                }
                else if (model.Role == "Manager")
                {
                    var manager = new PropertyManager
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Name,
                        OwnerId = 0 
                    };
                    db.PropertyManagers.Add(manager);
                }
                else if (model.Role == "Tenant")
                {
                    var tenant = new Tenant
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Name,
                        PropertyId = 0
                    };
                    db.Tenants.Add(tenant);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}