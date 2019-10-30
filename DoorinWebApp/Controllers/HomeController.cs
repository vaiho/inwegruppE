using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoorinWebApp.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var competences = db.Items.ToList(); ////Your model that you want to pass
            return View(model);

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Join()
        {
            return View();
        }
    }
}