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
    public class EditSumController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /EditSum/

        public ActionResult Index(int id = 0)
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);

            PortfolioChangeSumModel model = new PortfolioChangeSumModel();
            

            portfolio port = db.portfolio.SingleOrDefault(p => p.akcie_id == id && p.uzivatel_id == userId);
            if (port == null)
            {
                /*port = new portfolio();
                port.uzivatel_id = userId;
                port.akcie_id = id;
                port.
                port = portfolio.Createportfolio( (userId, id, DateTime.Today);
                port.pocet = 0;
                db.portfolio.AddObject(port);
                db.SaveChanges();*/
            }
                model.pModel = port;
            
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
            
            if (wallet >= model.sumToBuy*buyPrice && ModelState.IsValid)
            {
                model.pModel = db.portfolio.Single(p => p.uzivatel_id == model.pModel.uzivatel_id && p.akcie_id == model.pModel.akcie_id);
                db.portfolio.Attach(model.pModel);

                if(model.sumToBuy != 0)
                {
                    wallet -= model.sumToBuy * buyPrice;
                    model.pModel.datum_zmeny = DateTime.Today;
                    model.pModel.nakup = true;
                    model.pModel.pocet = model.sumToBuy;
                }
                if(model.sumToSell != 0)
                {
                    wallet += model.sumToSell * sellPrice;
                    model.pModel.datum_zmeny = DateTime.Today;
                    model.pModel.nakup = false;
                    model.pModel.pocet = model.sumToSell;
                }


                
                db.ObjectStateManager.ChangeObjectState(model.pModel, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index", "Akcie", new { area = "" });
            }
            ViewBag.err = true;
            return View(model);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}