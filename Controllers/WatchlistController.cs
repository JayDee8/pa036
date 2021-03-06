﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bcpp.Models;
using WebMatrix.WebData;
using bcpp.Filters;

namespace bcpp.Controllers
{
    [InitializeSimpleMembership]
    public class WatchlistController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /Watchlist/

        public ActionResult Index()
        {
            SledovaneMy sm = new SledovaneMy();
            int userId = WebSecurity.CurrentUserId;
            var sledovane = db.sledovane.Include("akcie").Include("uzivatel").Where(i => i.uzivatel_id == userId);
            if(User.IsInRole("admin"))
                sledovane = db.sledovane.Include("akcie").Include("uzivatel");

            IEnumerable<historie_akcie> ha = from h in db.historie_akcie group h by h.akcie_id into g let maxDate = g.Max(r => r.datum) from rowGroup in g where rowGroup.datum == maxDate select rowGroup;

            var sledovane2 = from s in sledovane 
                        join f in db.firma 
                        on s.akcie.firma_id equals f.firma_id 
                        join h in ha 
                        on s.akcie_id equals h.akcie_id 
                        select new MyModelSledovane { idSledovane = s.sledovane_id, nazevSledovane = s.nazev, nazevAkcie = s.akcie.nazev, nazevFirmy = f.nazev, cenaNakup = h.cena_nakup, cenaProdej = h.cena_prodej, datum = h.datum };
           
            sm.sled = sledovane2;
            sm.akcieIds = sledovane.Select(i => i.akcie_id).ToList();
            sm.akcieNames = sledovane.Select(i => i.akcie.nazev).ToList();

            return View(sm);
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
            if (User.IsInRole("user"))
            {
                int userId = WebSecurity.CurrentUserId;
                var volneAkcie = (from a in db.akcie
                                  join s in db.sledovane on new { a.akcie_id,uid = userId } equals new { s.akcie_id,uid = s.uzivatel_id} into subset
                                  from c in subset.DefaultIfEmpty()
                                  where c == null
                                  select a);
                ViewBag.akcie_id = new SelectList(volneAkcie, "akcie_id", "nazev");
            }
            else
            {
                ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev");
            }

            if(User.IsInRole("admin"))
                ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno");
            else
                ViewBag.uzivatel_id = WebSecurity.CurrentUserId;

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
                sledovane.uzivatel_id = WebSecurity.CurrentUserId;
                db.sledovane.AddObject(sledovane);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (User.IsInRole("user"))
            {
                var volneAkcie = (from a in db.akcie
                                  join s in db.sledovane on a.akcie_id equals s.akcie_id into subset
                                  from c in subset.DefaultIfEmpty()
                                  where c == null
                                  select a);
                ViewBag.akcie_id = new SelectList(volneAkcie, "akcie_id", "nazev");
            }
            else
            {
                ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev");
            }

            if (User.IsInRole("admin"))
                ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno");
            else
                ViewBag.uzivatel_id = WebSecurity.CurrentUserId;

            return View(sledovane);
        }

        //
        // GET: /Watchlist/Edit/5

        public ActionResult Edit(int id = 0)
        {
            sledovane sledovane;
            if (User.IsInRole("admin"))
                sledovane = db.sledovane.Single(p => p.sledovane_id == id);
            else
                sledovane = db.sledovane.Single(p => p.sledovane_id == id && p.uzivatel_id == WebSecurity.CurrentUserId);
            
            if (sledovane == null)
            {
                return HttpNotFound();
            }
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", sledovane.akcie_id);
            if (User.IsInRole("admin"))
                ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno");
            else
                ViewBag.uzivatel_id = WebSecurity.CurrentUserId;
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
            if (User.IsInRole("admin"))
                ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno");
            else
                ViewBag.uzivatel_id = WebSecurity.CurrentUserId;
            return View(sledovane);
        }

        //
        // GET: /Watchlist/Delete/5

        public ActionResult Delete(int id = 0)
        {
            sledovane sledovane;
            if (User.IsInRole("admin"))
                sledovane = db.sledovane.Single(p => p.sledovane_id == id);
            else
                sledovane = db.sledovane.Single(p => p.sledovane_id == id && p.uzivatel_id == WebSecurity.CurrentUserId);
            
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