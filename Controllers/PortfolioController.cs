using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bcpp.Models;
using bcpp.Filters;
using WebMatrix.WebData;

namespace bcpp.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class PortfolioController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /Portfolio/

        public ActionResult Index()
        {
            IEnumerable<portfolio> portfolio;
            if(User.IsInRole("admin"))
            {
                portfolio = db.portfolio.Include("akcie").Include("uzivatel").Where(p => p.pocet != 0);
            }
            else
            {
                int userId = WebSecurity.CurrentUserId;
                portfolio = db.portfolio.Include("akcie").Include("uzivatel").Where(p => p.pocet != 0 && p.uzivatel_id == userId);
            }
            return View(portfolio.ToList());
        }

        //
        // GET: /Portfolio/Details/5

        public ActionResult Details(int id = 0)
        {
            int userId = id;
            if (id == 0 || !User.IsInRole("admin"))
                userId = WebSecurity.CurrentUserId;

            portfolio portfolio = db.portfolio.SingleOrDefault(p => p.portfolio_id == userId);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }


        //
        // GET: /Portfolio/Create

        public ActionResult Create()
        {
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev");
            if(User.IsInRole("admin"))
                ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno");
            else
                ViewBag.uzivatel_id = WebSecurity.CurrentUserId;
            return View();
        }

        //
        // POST: /Portfolio/Create

        [HttpPost]
        public ActionResult Create(portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                db.portfolio.AddObject(portfolio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", portfolio.akcie_id);
            if (User.IsInRole("admin"))
                ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno");
            else
                ViewBag.uzivatel_id = WebSecurity.CurrentUserId;
            return View(portfolio);
        }

        //
        // GET: /Portfolio/Edit/5

        public ActionResult Edit(int id = 0)
        {
            portfolio portfolio = db.portfolio.Single(p => p.portfolio_id == id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", portfolio.akcie_id);
            ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno", portfolio.uzivatel_id);
            return View(portfolio);
        }

        //
        // POST: /Portfolio/Edit/5

        [HttpPost]
        public ActionResult Edit(portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                db.portfolio.Attach(portfolio);
                db.ObjectStateManager.ChangeObjectState(portfolio, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.akcie_id = new SelectList(db.akcie, "akcie_id", "nazev", portfolio.akcie_id);
            ViewBag.uzivatel_id = new SelectList(db.uzivatel, "uzivatel_id", "jmeno", portfolio.uzivatel_id);
            return View(portfolio);
        }

        //
        // GET: /Portfolio/Delete/5

        public ActionResult Delete(int id = 0)
        {
            portfolio portfolio = db.portfolio.Single(p => p.portfolio_id == id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        //
        // POST: /Portfolio/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            portfolio portfolio = db.portfolio.Single(p => p.portfolio_id == id);
            db.portfolio.DeleteObject(portfolio);
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