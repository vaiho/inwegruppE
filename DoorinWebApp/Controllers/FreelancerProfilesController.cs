using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoorinWebApp.Models;
using DoorinWebApp.Models.Operations;
using DoorinWebApp.Viewmodel;

namespace DoorinWebApp.Controllers
{
    public class FreelancerProfilesController : Controller
    {
        private doorinDBEntities db = new doorinDBEntities();
        // GET: FreelancerProfiles
        public ActionResult Index()
        {
            int id = 3;
            freelancer freelancer = db.freelancer.Find(id);
            FreelancerProfileOperations fpop = new FreelancerProfileOperations();

            //return View(fpop.GetProfileDetails());
            //return View(fpop.GetProfile(id));
            return View(fpop.GetFreelancer(id));
        }

    }
}