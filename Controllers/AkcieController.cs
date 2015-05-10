using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bcpp.Models;
using WebMatrix.WebData;
using System.Data.Objects;
using bcpp.Filters;
using System.Linq.Dynamic;

namespace bcpp.Controllers
{

    class DistinctItemComparer : IEqualityComparer<historie_akcie>
    {

        public bool Equals(historie_akcie x, historie_akcie y)
        {
            return x.akcie_id == y.akcie_id;
        }

        public int GetHashCode(historie_akcie obj)
        {
            return obj.akcie_id.GetHashCode();
        }
    }

    [InitializeSimpleMembership]
    [Authorize]
    public class AkcieController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /Akcie/

        public ActionResult Index(int page = 1, int pageSize = 12, string sort = "nazev", string sortdir = "DESC")
        {
            
            var akcie = db.akcie.Include("historie_akcie");
            
            
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            
            var records = new PagedList<AkcieUserModel>();

            IEnumerable<portfolio> mojePortfolio = db.portfolio.Where(p => p.uzivatel_id == userId);
            IEnumerable<historie_akcie> ha = from h in db.historie_akcie group h by h.akcie_id into g let maxDate = g.Max(r => r.datum) from rowGroup in g where rowGroup.datum == maxDate select rowGroup;


            var content =
                (
                    from a in db.akcie
                    join h in ha
                        on a.akcie_id equals h.akcie_id
                    join p in db.portfolio
                        on new { a.akcie_id, id = userId } equals new { p.akcie_id, id = p.uzivatel_id } into ap
                    from row in ap.DefaultIfEmpty()
                    select new AkcieUserModel { akcie_id = a.akcie_id, nazev = a.nazev, zkratka = a.zkratka, cena_nakup = h.cena_nakup, cena_prodej = h.cena_prodej, datum = h.datum, pocet = (row == null ? 0 : row.pocet) }
                );

            records.Content = content.OrderBy(sort + " " + sortdir).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            if (WebSecurity.IsAuthenticated)
            {
                float? wallet = db.uzivatel.Single(a => a.uzivatel_id == userId).penezenka;
                if (wallet == null)
                    ViewBag.wallet = 0;
                else
                    ViewBag.wallet = wallet;
            }

            records.TotalRecords = content.Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        //
        // GET: /Akcie/Details/5

        public ActionResult Details(int id = 0)
        {
            akcie akcie = db.akcie.Single(a => a.akcie_id == id);
            if (akcie == null)
            {
                return HttpNotFound();
            }
            return View(akcie);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}