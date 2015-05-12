using System;
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
    [Authorize]
    public class WalletController : BaseController
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /Wallet/

        public ActionResult Index()
        {
            int userId = WebSecurity.CurrentUserId;
            uzivatel model = db.uzivatel.Single(u => u.uzivatel_id == userId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(uzivatel model)
        {
            if (model.penezenka >= 0)
            {
                float? wallet = model.penezenka;
                model = db.uzivatel.Single(u => u.uzivatel_id == model.uzivatel_id);
                db.uzivatel.Attach(model);
                model.penezenka = wallet;
                db.ObjectStateManager.ChangeObjectState(model, EntityState.Modified);
                db.SaveChanges();
                Success(String.Format("Hodnota peněženky upravena. Aktuální hodnota: {0}",wallet), true);
                return RedirectToAction("Index", "Akcie", new { area = "" });
            }
            Danger("Zadaná hodnota není správná!",true);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}