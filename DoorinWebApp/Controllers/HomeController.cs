using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoorinWebApp.Models;
using DoorinWebApp.Viewmodel;

namespace DoorinWebApp.Controllers
{
    public class HomeController : Controller
    {
        doorinDBEntities di = new doorinDBEntities();

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

            //ViewBag.Competence = GetCompetences();
            //ViewBag.Technologies = GetTechnologies();
            //ViewBag.Freelancers = GetFreelancersTest2();
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


        //private List<competence> GetCompetences() {

        //    List<competence> CList = new List<competence>();
        //    var competencelist = (from c in di.competence
        //                          select new { c.name, c.competence_id }).ToList();

        //    foreach (var v in competencelist)
        //    {
        //        competence item = new competence();
        //        item.name = v.name;
        //        item.competence_id = v.competence_id;
        //        CList.Add(item);
        //    }

        //    return (CList);
        //}

        //private List<technology> GetTechnologies()
        //{
        //    List<technology> TList = new List<technology>();
        //    var technologylist = (from t in di.technology
        //                          select new { t.name }).ToList();

        //    foreach (var t in technologylist)
        //    {
        //        technology item = new technology();
        //        item.name = t.name;
        //        TList.Add(item);
        //    }

        //    return (TList);
        //}

        //private List<technology> GetTechnologyBasedOnCompetence()
        //{
        //    var choosencomp = 0;
        //    List<technology> TechBased = new List<technology>();
        //    var techbasedlist = (from t in di.technology
        //                         join c in di.competence on t.competence_id equals c.competence_id
        //                         where t.competence_id == choosencomp
        //                         select new {t.name}).ToList();

        //    return (TechBased);
        //}
        

        //private List<FreelancerProfileVM> GetFreelancersTest2()
        //{
        //    List<FreelancerProfileVM> FList = new List<FreelancerProfileVM>();
        //    var freelancerlist = (from f in di.freelancer
        //                          join r in di.resume on f.freelancer_id equals r.freelancer_id
        //                          //join t in di.technology_resume on r.resume_id equals t.resume_id
        //                          select new { f.firstname, f.lastname, f.resume, r.resume_id }).ToList();

        //    foreach (var f in freelancerlist)
        //    {
        //        FreelancerProfileVM item = new FreelancerProfileVM();
        //        item.Firstname = f.firstname;
        //        item.Lastname = f.lastname;
        //        item.Resume_id = f.resume_id;
        //        FList.Add(item);
        //    }

        //    return (FList);
        //}

        //private List<freelancer> GetFreelancers()
        //{
        //    List<freelancer> FList = new List<freelancer>();
        //    var freelancerlist = (from f in di.freelancer
        //                          select new {f.firstname, f.lastname}).ToList();

        //    foreach (var f in freelancerlist)
        //    {
        //        freelancer item = new freelancer();
        //        item.firstname = f.firstname;
        //        item.lastname = f.lastname;
        //        FList.Add(item);
        //    }

        //    return (FList);
        //}

        

    }
}