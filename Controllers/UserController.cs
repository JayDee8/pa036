using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bcpp.Models;
using WebMatrix.WebData;
using System.Web.Security;

namespace bcpp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /User/
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var uzivatel = db.uzivatel.Include("adresa");
            return View(uzivatel.ToList());
        }

        //
        // GET: /User/Details/5
        [Authorize(Roles = "admin")]
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
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            uzivatel uzivatel = db.uzivatel.Single(u => u.uzivatel_id == id);
            if (uzivatel == null)
            {
                return HttpNotFound();
            }

            var adr = db.adresa
                .ToList()
                .Select(s => new
                {
                    adresa_id = s.adresa_id,
                    fullAddress = string.Format("{0} {1}", s.ulice, s.mesto)
                });

            ViewBag.adresa_id = new SelectList(adr, "adresa_id", "fullAddress", uzivatel.adresa_id);
            return View(uzivatel);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uzivatel uzivatel)
        {
            if (ModelState.IsValid)
            {
                db.uzivatel.Attach(uzivatel);
                db.ObjectStateManager.ChangeObjectState(uzivatel, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var adr = db.adresa
                .ToList()
                .Select(s => new
                {
                    adresa_id = s.adresa_id,
                    fullAddress = string.Format("{0} {1}", s.ulice, s.mesto)
                });

            ViewBag.adresa_id = new SelectList(adr, "adresa_id", "fullAddress");
            return View(uzivatel);
        }

        //
        // GET: /User/Delete/5
        [Authorize(Roles="admin")]
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            uzivatel uzivatel = db.uzivatel.Single(u => u.uzivatel_id == id);
            db.uzivatel.DeleteObject(uzivatel);

           /* MembershipUser mu = Membership.GetUser(id);

            var membership = (SimpleMembershipProvider)Membership.Provider;
            membership.DeleteAccount(mu.UserName);
            */
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