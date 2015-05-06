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
    public class FirmaController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /Firma/

        public ActionResult Index()
        {
            var firma = db.firma.Include("adresa");
            return View(firma.ToList());
        }

        //
        // GET: /Firma/Details/5

        public ActionResult Details(int id = 0)
        {
            firma firma = db.firma.Single(f => f.firma_id == id);
            if (firma == null)
            {
                return HttpNotFound();
            }
            return View(firma);
        }

        //
        // GET: /Firma/Create

        public ActionResult Create()
        {
            ViewBag.adresa_id = new SelectList(db.adresa, "adresa_id", "mesto");
            return View();
        }

        //
        // POST: /Firma/Create

        [HttpPost]
        public ActionResult Create(firma firma)
        {
            if (ModelState.IsValid)
            {
                db.firma.AddObject(firma);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.adresa_id = new SelectList(db.adresa, "adresa_id", "mesto", firma.adresa_id);
            return View(firma);
        }

        //
        // GET: /Firma/Edit/5

        public ActionResult Edit(int id = 0)
        {
            firma firma = db.firma.Single(f => f.firma_id == id);
            if (firma == null)
            {
                return HttpNotFound();
            }
            ViewBag.adresa_id = new SelectList(db.adresa, "adresa_id", "mesto", firma.adresa_id);
            return View(firma);
        }

        //
        // POST: /Firma/Edit/5

        [HttpPost]
        public ActionResult Edit(firma firma)
        {
            if (ModelState.IsValid)
            {
                db.firma.Attach(firma);
                db.ObjectStateManager.ChangeObjectState(firma, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.adresa_id = new SelectList(db.adresa, "adresa_id", "mesto", firma.adresa_id);
            return View(firma);
        }

        //
        // GET: /Firma/Delete/5

        public ActionResult Delete(int id = 0)
        {
            firma firma = db.firma.Single(f => f.firma_id == id);
            if (firma == null)
            {
                return HttpNotFound();
            }
            return View(firma);
        }

        //
        // POST: /Firma/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            firma firma = db.firma.Single(f => f.firma_id == id);
            db.firma.DeleteObject(firma);
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