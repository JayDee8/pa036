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
    public class IndexPXController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /IndexPX/

        public ActionResult Index()
        {
            return View(db.index_PX.ToList());
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}