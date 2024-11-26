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
    public class PropertyController : Controller
    {
        private PRMSEntities db = new PRMSEntities();

        // GET: Property
        public ActionResult Index()
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                var properties = db.Properties.Include(p => p.PropertyManager);
                return View(properties.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Property/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Property property = db.Properties.Find(id);
                if (property == null)
                {
                    return HttpNotFound();
                }
                return View(property);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Property/Create
        public ActionResult Create()
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                ViewBag.PropertyManagerId = new SelectList(db.PropertyManagers, "PropertyManagerId", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Property/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PropertyId,Address,PropertyManagerId")] Property property)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (ModelState.IsValid)
                {
                    db.Properties.Add(property);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.PropertyManagerId = new SelectList(db.PropertyManagers, "PropertyManagerId", "Name", property.PropertyManagerId);
                return View(property);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Property/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Property property = db.Properties.Find(id);
                if (property == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PropertyManagerId = new SelectList(db.PropertyManagers, "PropertyManagerId", "Name", property.PropertyManagerId);
                return View(property);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }  
        }

        // POST: Property/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PropertyId,Address,PropertyManagerId")] Property property)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(property).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.PropertyManagerId = new SelectList(db.PropertyManagers, "PropertyManagerId", "Name", property.PropertyManagerId);
                return View(property);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Property/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Property property = db.Properties.Find(id);
                if (property == null)
                {
                    return HttpNotFound();
                }
                return View(property);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Property/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner" || Session["Role"].ToString() == "Manager"))
            {
                Property property = db.Properties.Find(id);
                db.Properties.Remove(property);
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

        public ActionResult AvailableProperties()
        {
            // Get all properties
            var allProperties = db.Properties.Include(p => p.PropertyManager).ToList();

            // Get all rented property IDs
            var rentedPropertyIds = db.Tenants.Select(t => t.PropertyId).ToList();

            // Filter out rented properties
            var availableProperties = allProperties.Where(p => !rentedPropertyIds.Contains(p.PropertyId)).ToList();

            return View(availableProperties);
        }
    }
}
