using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bcpp.Models;

namespace bcpp.Controllers
{
    public class AkcieController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /Akcie/

        public ActionResult Index()
        {
            var akcie = db.akcie.Include("firma");
            return View(akcie.ToList());
        }

        //
        // GET: /Akcie/Details/5

        public ActionResult Details(int id = 0)
        {
            akcie akcie = db.akcie.Single(a => a.akcie_id == id);
            if (akcie == null)
            {
                return HttpNotFound();
            }
            return View(akcie);
        }

        //
        // GET: /Akcie/Create

        public ActionResult Create()
        {
            ViewBag.firma_id = new SelectList(db.firma, "firma_id", "ICO");
            return View();
        }

        //
        // POST: /Akcie/Create

        [HttpPost]
        public ActionResult Create(akcie akcie)
        {
            if (ModelState.IsValid)
            {
                db.akcie.AddObject(akcie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.firma_id = new SelectList(db.firma, "firma_id", "ICO", akcie.firma_id);
            return View(akcie);
        }

        //
        // GET: /Akcie/Edit/5

        public ActionResult Edit(int id = 0)
        {
            akcie akcie = db.akcie.Single(a => a.akcie_id == id);
            if (akcie == null)
            {
                return HttpNotFound();
            }
            ViewBag.firma_id = new SelectList(db.firma, "firma_id", "ICO", akcie.firma_id);
            return View(akcie);
        }

        //
        // POST: /Akcie/Edit/5

        [HttpPost]
        public ActionResult Edit(akcie akcie)
        {
            if (ModelState.IsValid)
            {
                db.akcie.Attach(akcie);
                db.ObjectStateManager.ChangeObjectState(akcie, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.firma_id = new SelectList(db.firma, "firma_id", "ICO", akcie.firma_id);
            return View(akcie);
        }

        //
        // GET: /Akcie/Delete/5

        public ActionResult Delete(int id = 0)
        {
            akcie akcie = db.akcie.Single(a => a.akcie_id == id);
            if (akcie == null)
            {
                return HttpNotFound();
            }
            return View(akcie);
        }

        //
        // POST: /Akcie/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            akcie akcie = db.akcie.Single(a => a.akcie_id == id);
            db.akcie.DeleteObject(akcie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}