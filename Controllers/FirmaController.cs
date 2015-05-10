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
    public class FirmaController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /Firma/

        public ActionResult Index()
        {
            var firma = db.firma.Include("adresa");
            return View(firma.ToList());
        }

        //
        // GET: /Firma/Details/5

        public ActionResult Details(int id = 0)
        {
            firma firma = db.firma.Single(f => f.firma_id == id);
            if (firma == null)
            {
                return HttpNotFound();
            }
            return View(firma);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}