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
    public class MainController : Controller
    {
        private dbEntities db = new dbEntities();

        public ActionResult Index()
        {
            MainPageModel M = new MainPageModel();
            M.AModel = db.akcie.ToList();
            M.IModel = db.index_PX.ToList();
            return View(M);
        }
    }
}
