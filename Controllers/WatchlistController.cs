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
            var watchlist = db.sledovane.Include("akcie").Include("uzivatel");
            return View(watchlist.ToList());
        }

        //
        // GET: /Watchlist/Details/5

        public ActionResult Details(int id)
        {
            sledovane watchlist = db.sledovane.Single(p => p.uzivatel_id == id);
            if (watchlist == null)
            {
                return HttpNotFound();
            }
            return View(watchlist);
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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Watchlist/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Watchlist/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Watchlist/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Watchlist/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
