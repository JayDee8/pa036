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
    public class HistorieAkcieController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /HistorieAkcie/

        public ActionResult Index()
        {
            return View(db.historie_akcie.ToList());
        }

        //
        // GET: /HistorieAkcie/Details/5

        public ActionResult Details(int id = 0)
        {
            historie_akcie historie_akcie = db.historie_akcie.Single(h => h.id_akcie == id);
            if (historie_akcie == null)
            {
                return HttpNotFound();
            }
            return View(historie_akcie);
        }

        //
        // GET: /HistorieAkcie/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HistorieAkcie/Create

        [HttpPost]
        public ActionResult Create(historie_akcie historie_akcie)
        {
            if (ModelState.IsValid)
            {
                db.historie_akcie.AddObject(historie_akcie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(historie_akcie);
        }

        //
        // GET: /HistorieAkcie/Edit/5

        public ActionResult Edit(int id = 0)
        {
            historie_akcie historie_akcie = db.historie_akcie.Single(h => h.id_akcie == id);
            if (historie_akcie == null)
            {
                return HttpNotFound();
            }
            return View(historie_akcie);
        }

        //
        // POST: /HistorieAkcie/Edit/5

        [HttpPost]
        public ActionResult Edit(historie_akcie historie_akcie)
        {
            if (ModelState.IsValid)
            {
                db.historie_akcie.Attach(historie_akcie);
                db.ObjectStateManager.ChangeObjectState(historie_akcie, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(historie_akcie);
        }

        //
        // GET: /HistorieAkcie/Delete/5

        public ActionResult Delete(int id = 0)
        {
            historie_akcie historie_akcie = db.historie_akcie.Single(h => h.id_akcie == id);
            if (historie_akcie == null)
            {
                return HttpNotFound();
            }
            return View(historie_akcie);
        }

        //
        // POST: /HistorieAkcie/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            historie_akcie historie_akcie = db.historie_akcie.Single(h => h.id_akcie == id);
            db.historie_akcie.DeleteObject(historie_akcie);
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