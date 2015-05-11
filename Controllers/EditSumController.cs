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
    public class EditSumController : BaseController
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /EditSum/

        public ActionResult Index(int id = 0)
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);

            IEnumerable<historie_akcie> ha = from h in db.historie_akcie group h by h.akcie_id into g let maxDate = g.Max(r => r.datum) from rowGroup in g where rowGroup.datum == maxDate select rowGroup;

            historie_akcie h1 = ha.Single(i => i.akcie_id == id);


            PortfolioChangeSumModel model = new PortfolioChangeSumModel();

            model.wallet = db.uzivatel.Single(u => u.uzivatel_id == userId).penezenka;

            portfolio port = db.portfolio.SingleOrDefault(p => p.akcie_id == id && p.uzivatel_id == userId);
            if (port == null)
            {
                port = new portfolio();
                port.akcie_id = id;
                port.uzivatel_id = userId;
            }
            model.pModel = port;
            model.akcieName = db.akcie.Single(a => a.akcie_id == id).nazev;
            model.cena_prodej = h1.cena_prodej;
            model.cena_nakup = h1.cena_nakup;

            return View(model);
        }

        
        [HttpPost]
        public ActionResult Index(PortfolioChangeSumModel model)
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            IEnumerable<historie_akcie> ha = from h in db.historie_akcie group h by h.akcie_id into g let maxDate = g.Max(r => r.datum) from rowGroup in g where rowGroup.datum == maxDate select rowGroup;

            historie_akcie h1 = ha.Single(i => i.akcie_id == model.pModel.akcie_id);

            float sellPrice = (float)h1.cena_prodej;
            float buyPrice = (float)h1.cena_nakup;
            float? wallet = db.uzivatel.Single(u => u.uzivatel_id == userId).penezenka;
            
            if (wallet >= model.sumToBuy*buyPrice && ModelState.IsValid && model.sumToSell <= model.pModel.pocet && (model.sumToBuy != 0 || model.sumToSell != 0))
            {
                int akcieId = model.pModel.akcie_id;
                bool newPortfolio = false;
                model.pModel = db.portfolio.SingleOrDefault(p => p.uzivatel_id == model.pModel.uzivatel_id && p.akcie_id == model.pModel.akcie_id);
                if (model.pModel == null)
                {
                    model.pModel = new portfolio();
                    model.pModel.uzivatel_id = userId;
                    model.pModel.akcie_id = akcieId;
                    newPortfolio = true;
                }
                else
                {
                    db.portfolio.Attach(model.pModel);
                }


                if(model.sumToBuy != 0)
                {
                    wallet -= model.sumToBuy * buyPrice;
                    model.pModel.datum_zmeny = DateTime.Today;
                    model.pModel.nakup = true;
                    model.pModel.pocet += model.sumToBuy;
                    model.pModel.cena = buyPrice;
                }
                if(model.sumToSell != 0)
                {
                    wallet += model.sumToSell * sellPrice;
                    model.pModel.datum_zmeny = DateTime.Today;
                    model.pModel.nakup = false;
                    model.pModel.pocet -= model.sumToSell;
                    model.pModel.cena = sellPrice;
                }
                uzivatel meUser = db.uzivatel.Single(u => u.uzivatel_id == userId);
                db.uzivatel.Attach(meUser);
                meUser.penezenka = wallet;
                db.SaveChanges();

                if(newPortfolio)
                    db.portfolio.AddObject(model.pModel);
                else
                    db.ObjectStateManager.ChangeObjectState(model.pModel, EntityState.Modified);
                db.SaveChanges();
                if (model.sumToBuy != 0)
                    Success(string.Format("Koupeno položek akcií firmy {1}: <b>{0}</b> v celkové hodnotě {2}Kč", model.sumToBuy, model.akcieName, buyPrice * model.sumToBuy), true);
                if (model.sumToSell != 0)
                    Success(string.Format("Prodáno položek akcií firmy {1}: <b>{0}</b> v celkové hodnotě {2}Kč", model.sumToSell, model.akcieName, sellPrice * model.sumToSell), true);

                return RedirectToAction("Index", "Akcie", new { area = "" });
            }
            
            if (wallet < model.sumToBuy * buyPrice)
                Danger("Nemáte dostatek peněz!", true);
            if (model.sumToSell > model.pModel.pocet)
                Danger("Nemůžete prodat více akcií, než máte!", true); 
            else
                Danger("Portfólio nebylo upraveno", true);
            return View(model);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}