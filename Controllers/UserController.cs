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
    public class UserController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(db.uzivatel.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            uzivatel uzivatel = db.uzivatel.Single(u => u.uzivatel_id == id);
            if (uzivatel == null)
            {
                return HttpNotFound();
            }
            return View(uzivatel);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(uzivatel uzivatel)
        {
            if (ModelState.IsValid)
            {
                db.uzivatel.AddObject(uzivatel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uzivatel);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            uzivatel uzivatel = db.uzivatel.Single(u => u.uzivatel_id == id);
            if (uzivatel == null)
            {
                return HttpNotFound();
            }
            return View(uzivatel);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(uzivatel uzivatel)
        {
            if (ModelState.IsValid)
            {
                db.uzivatel.Attach(uzivatel);
                db.ObjectStateManager.ChangeObjectState(uzivatel, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uzivatel);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            uzivatel uzivatel = db.uzivatel.Single(u => u.uzivatel_id == id);
            if (uzivatel == null)
            {
                return HttpNotFound();
            }
            return View(uzivatel);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            uzivatel uzivatel = db.uzivatel.Single(u => u.uzivatel_id == id);
            db.uzivatel.DeleteObject(uzivatel);
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