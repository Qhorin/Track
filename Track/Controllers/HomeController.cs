using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Track.Models;

namespace Track.Controllers
{
    public class HomeController : Controller
    {
        TrackDb _db = new TrackDb();
        
        public ActionResult Index()
        {
            var model = _db.Stores.Take(2).ToList();
            
            return View(model); 
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var store =_db.Stores.Find(id);

            return View(store);
        }

        //[HttpPost]
        //public ActionResult Edit(Store store)
        //{
            
        //}

        public ActionResult About(string value)
        {
            //var model = new AboutModel();
            //model.Name = "Jerry";
            //model.Location = "Wilmington, NC";
            //return View(model);
            return File(Server.MapPath("~/Content/Site.css"), "text/css");
            //return Json(new { Value = value, name = "Jerry" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
