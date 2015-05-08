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
    public class WatchlistController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /Watchlist/

        public ActionResult Index()
        {
            var sledovane = db.sledovane.Include("akcie").Include("uzivatel");
            return View(sledovane.ToList());
        }

        //
        // GET: /Watchlist/Details/5

        public ActionResult Details(int id = 0)
        {
            sledovane sledovane = db.sledovane.Single(s => s.sledovane_id == id);
            if (sledovane == null)
            {
                return HttpNotFound();
            }
            return View(sledovane);
        }

        //
        // GET: /Watchlist/Create

        public ActionResult Create()
        {
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev");
            ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno");
            return View();
        }

        //
        // POST: /Watchlist/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sledovane sledovane)
        {
            if (ModelState.IsValid)
            {
                db.sledovane.AddObject(sledovane);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", sledovane.akcie_id);
            ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno", sledovane.uzivatel_id);
            return View(sledovane);
        }

        //
        // GET: /Watchlist/Edit/5

        public ActionResult Edit(int id = 0)
        {
            sledovane sledovane = db.sledovane.Single(s => s.sledovane_id == id);
            if (sledovane == null)
            {
                return HttpNotFound();
            }
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", sledovane.akcie_id);
            ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno", sledovane.uzivatel_id);
            return View(sledovane);
        }

        //
        // POST: /Watchlist/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sledovane sledovane)
        {
            if (ModelState.IsValid)
            {
                db.sledovane.Attach(sledovane);
                db.ObjectStateManager.ChangeObjectState(sledovane, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", sledovane.akcie_id);
            ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno", sledovane.uzivatel_id);
            return View(sledovane);
        }

        //
        // GET: /Watchlist/Delete/5

        public ActionResult Delete(int id = 0)
        {
            sledovane sledovane = db.sledovane.Single(s => s.sledovane_id == id);
            if (sledovane == null)
            {
                return HttpNotFound();
            }
            return View(sledovane);
        }

        //
        // POST: /Watchlist/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sledovane sledovane = db.sledovane.Single(s => s.sledovane_id == id);
            db.sledovane.DeleteObject(sledovane);
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