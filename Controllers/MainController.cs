using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bcpp.Models;
using WebMatrix.WebData;
using PagedList;

namespace bcpp.Controllers
{
    public class MainController : Controller
    {
        private dbEntities db = new dbEntities();

        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            MainPageModel M = new MainPageModel();
            var akcie = db.akcie.Include("historie_akcie");
            IEnumerable<historie_akcie> ha = from h in db.historie_akcie group h by h.akcie_id into g let maxDate = g.Max(r => r.datum) from rowGroup in g where rowGroup.datum == maxDate select rowGroup;

            IEnumerable<akcie2> content =
                (
                    from a in db.akcie
                    join h in ha
                        on a.akcie_id equals h.akcie_id
                    select new akcie2 { akcie_id = a.akcie_id, nazev = a.nazev, zkratka = a.zkratka, cena_nakup = h.cena_nakup, cena_prodej = h.cena_prodej, datum = h.datum }
                );

            ViewBag.CurrentSort = sortOrder;
            int pageSize = 10;
            
            int pageNumber = (page ?? 1);

            var indexPX = from s in db.index_PX select s;
            var index2PX = from s in db.index_PX select s;

            indexPX = indexPX.OrderByDescending(s => s.datum);
            //index2PX = index2PX.OrderByDescending(s => s.datum);
            M.AModel = content;
            M.IModel = indexPX.ToPagedList(pageNumber, pageSize);//db.index_PX.ToList();
            M.I2Model = index2PX.ToList();

            return View(M);
        }
    }
}
