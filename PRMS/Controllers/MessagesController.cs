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
    public class MessagesController : Controller
    {
        private PRMSEntities db = new PRMSEntities();

        // GET: Messages
        public ActionResult Index()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var userRole = Session["Role"] as string;
            var userId = Session["UserId"] as int?;

            if (userRole == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<PossibleMessagesDataViewModel> relevantData = new List<PossibleMessagesDataViewModel>();

            switch (userRole)
            {
                case "Owner":
                    relevantData = db.PropertyManagers
                        .Where(pm => pm.OwnerId == userId)
                        .Select(pm => new PossibleMessagesDataViewModel { Id = pm.PropertyManagerId, Name = pm.Name })
                        .ToList();
                    break;
                case "Manager":
                    var owner = db.PropertyManagers
                        .Where(pm => pm.PropertyManagerId == userId)
                        .Select(pm => new PossibleMessagesDataViewModel { Id = pm.PropertyOwner.OwnerId, Name = pm.PropertyOwner.Name })
                        .FirstOrDefault();
                    var tenants = db.Tenants
                        .Where(t => t.Property.PropertyManagerId == userId)
                        .Select(t => new PossibleMessagesDataViewModel { Id = t.TenantId, Name = t.Name })
                        .ToList();
                    if (owner != null)
                    {
                        relevantData.Add(owner);
                    }
                    relevantData.AddRange(tenants);
                    break;
                case "Tenant":
                    var manager = db.Tenants
                        .Where(t => t.TenantId == userId)
                        .Select(t => new PossibleMessagesDataViewModel { Id = t.Property.PropertyManager.PropertyManagerId, Name = t.Property.PropertyManager.Name })
                        .FirstOrDefault();
                    if (manager != null)
                    {
                        relevantData.Add(manager);
                    }
                    break;
                default:
                    break;
            }

            return View(relevantData);
        }




        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,SenderId,ReceiverId,SenderType,ReceiverType,MessageContent,Timestamp")] Message message)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,SenderId,ReceiverId,SenderType,ReceiverType,MessageContent,Timestamp")] Message message)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Role"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
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
