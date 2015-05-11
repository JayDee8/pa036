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
    public class IndexPXController : Controller
    {
        private dbEntities db = new dbEntities();

        //
        // GET: /IndexPX/

        public ActionResult Index(string sortOrder, string currentFilter, int? page, int pageSize = 10)
        {
            //var index = db.index_PX.ToList();
            var indexPX = from s in db.index_PX select s;
            indexPX = indexPX.OrderByDescending(s => s.datum);
            ViewBag.CurrentSort = sortOrder;
            //int pageSize = 10;

            int pageNumber = (page ?? 1);

            return View(indexPX.ToPagedList(pageNumber, pageSize));
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}