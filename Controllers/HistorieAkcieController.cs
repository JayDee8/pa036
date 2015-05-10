using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bcpp.Models;
using PagedList;

namespace bcpp.Controllers
{
    public class HistorieAkcieController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /HistorieAkcie/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            int pageSize = 10;
            if (searchString != null){
                page = 1;
            }
            else{
                searchString = currentFilter;
            }
            
            var historie_akcie = from s in db.historie_akcie select s;
            
            int pageNumber = (page ?? 1);

            ViewBag.CurrentFilter = searchString;
            
            if (!String.IsNullOrEmpty(searchString)) {
                historie_akcie = db.historie_akcie.Where(p => p.akcie.nazev.Contains(searchString));
            }

            historie_akcie = historie_akcie.OrderBy(s => s.akcie.akcie_id);

            return View(historie_akcie.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /HistorieAkcie/Details/5

        public ActionResult Details(int id = 0)
        {
            historie_akcie historie_akcie = db.historie_akcie.Single(h => h.historie_id == id);
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
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev");
            return View();
        }

        //
        // POST: /HistorieAkcie/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(historie_akcie historie_akcie)
        {
            if (ModelState.IsValid)
            {
                db.historie_akcie.AddObject(historie_akcie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", historie_akcie.akcie_id);
            return View(historie_akcie);
        }

        //
        // GET: /HistorieAkcie/Edit/5

        public ActionResult Edit(int id = 0)
        {
            historie_akcie historie_akcie = db.historie_akcie.Single(h => h.historie_id == id);
            if (historie_akcie == null)
            {
                return HttpNotFound();
            }
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", historie_akcie.akcie_id);
            return View(historie_akcie);
        }

        //
        // POST: /HistorieAkcie/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(historie_akcie historie_akcie)
        {
            if (ModelState.IsValid)
            {
                db.historie_akcie.Attach(historie_akcie);
                db.ObjectStateManager.ChangeObjectState(historie_akcie, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", historie_akcie.akcie_id);
            return View(historie_akcie);
        }

        //
        // GET: /HistorieAkcie/Delete/5

        public ActionResult Delete(int id = 0)
        {
            historie_akcie historie_akcie = db.historie_akcie.Single(h => h.historie_id == id);
            if (historie_akcie == null)
            {
                return HttpNotFound();
            }
            return View(historie_akcie);
        }

        //
        // POST: /HistorieAkcie/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            historie_akcie historie_akcie = db.historie_akcie.Single(h => h.historie_id == id);
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