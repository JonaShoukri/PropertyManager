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
    public class PropertyManagersController : Controller
    {
        private PRMSEntities db = new PRMSEntities();

        // GET: PropertyManagers
        public ActionResult Index()
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner"))
            {
                var propertyManagers = db.PropertyManagers.Include(p => p.PropertyOwner);
                return View(propertyManagers.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: PropertyManagers/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PropertyManager propertyManager = db.PropertyManagers.Find(id);
                if (propertyManager == null)
                {
                    return HttpNotFound();
                }
                return View(propertyManager);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: PropertyManagers/Create
        public ActionResult Create()
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner"))
            {
                ViewBag.OwnerId = new SelectList(db.PropertyOwners, "OwnerId", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: PropertyManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PropertyManagerId,Name,Email,OwnerId,Password")] PropertyManager propertyManager)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner"))
            {
                if (ModelState.IsValid)
                {
                    db.PropertyManagers.Add(propertyManager);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.OwnerId = new SelectList(db.PropertyOwners, "OwnerId", "Name", propertyManager.OwnerId);
                return View(propertyManager);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: PropertyManagers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PropertyManager propertyManager = db.PropertyManagers.Find(id);
                if (propertyManager == null)
                {
                    return HttpNotFound();
                }
                ViewBag.OwnerId = new SelectList(db.PropertyOwners, "OwnerId", "Name", propertyManager.OwnerId);
                return View(propertyManager);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: PropertyManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PropertyManagerId,Name,Email,OwnerId,Password")] PropertyManager propertyManager)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(propertyManager).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.OwnerId = new SelectList(db.PropertyOwners, "OwnerId", "Name", propertyManager.OwnerId);
                return View(propertyManager);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: PropertyManagers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PropertyManager propertyManager = db.PropertyManagers.Find(id);
                if (propertyManager == null)
                {
                    return HttpNotFound();
                }
                return View(propertyManager);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: PropertyManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Role"] != null && (Session["Role"].ToString() == "Owner"))
            {
                PropertyManager propertyManager = db.PropertyManagers.Find(id);
                db.PropertyManagers.Remove(propertyManager);
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
