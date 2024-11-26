using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PRMS.Models;

namespace PRMS.Controllers
{
    public class TenantsController : Controller
    {
        private PRMSEntities db = new PRMSEntities();

        // GET: Tenants
        public ActionResult Index()
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                var tenants = db.Tenants.Include(t => t.Property);
                return View(tenants.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Tenants/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tenant tenant = db.Tenants.Find(id);
                if (tenant == null)
                {
                    return HttpNotFound();
                }
                return View(tenant);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenantId,Name,Email,PropertyId,Password")] Tenant tenant)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (ModelState.IsValid)
                {
                    db.Tenants.Add(tenant);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", tenant.PropertyId);
                return View(tenant);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Tenants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tenant tenant = db.Tenants.Find(id);
                if (tenant == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", tenant.PropertyId);
                return View(tenant);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenantId,Name,Email,PropertyId,Password")] Tenant tenant)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tenant).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", tenant.PropertyId);
                return View(tenant);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }    
        }

        // GET: Tenants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tenant tenant = db.Tenants.Find(id);
                if (tenant == null)
                {
                    return HttpNotFound();
                }
                return View(tenant);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                Tenant tenant = db.Tenants.Find(id);
                db.Tenants.Remove(tenant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
