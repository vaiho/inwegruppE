using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoorinWebApp.Models;
using DoorinWebApp.Models.Operations;
using DoorinWebApp.Viewmodel;

namespace DoorinWebApp.Controllers
{
    public class freelancersController : Controller
    {
        private doorinDBEntities db = new doorinDBEntities();
        // GET: freelancers
        public ActionResult Index(string searchString) 
        {
            FreelancerProfileOperations fpop = new FreelancerProfileOperations();
            FreelancerOperations fop = new FreelancerOperations();

            //Viewbags för filtrering av kompetenser och teknologier 
            ViewBag.Competence = fop.GetAllCompetences();
            ViewBag.Technology = fop.GetAllTechnologies();
    
            return View(fpop.GetFreelancersList());
        }

        // GET: freelancers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            freelancer freelancer = db.freelancer.Find(id);
            FreelancerProfileOperations fpop = new FreelancerProfileOperations();
            freelancer.FreelancerProfileResume = fpop.GetFreelancerProfileById(id);
            
            if (freelancer == null)
            {
                return HttpNotFound();
            }
            return View(freelancer);
        }

        public ActionResult ProfilePage(int? id) 
        {
            FreelancerProfileOperations fpop = new FreelancerProfileOperations();

            return View(fpop.GetFreelancerProfileById(id));
        }


        // GET: freelancers/Create
        public ActionResult Create()
        {
            // Johan testar
            var country = GetCountries();
            var f = new freelancer();
            f.country = GetSelectListItems(country);

            return View(f);
        }


        // POST: freelancers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "freelancer_id,firstname,lastname,address,city,zipcode,phonenumber,email,birthdate,birthcity,nationality,username,password")] freelancer freelancer)
        {
            //Lista av valbara länder
            var countries = GetCountries();
            freelancer fr = new freelancer();
            freelancer.country = GetSelectListItems(countries); 
            

            if (ModelState.IsValid)
            {
                db.freelancer.Add(freelancer);
                resume r = new resume();
                r.profile = " ";
                r.driving_license = " ";
                r.freelancer_id = freelancer.freelancer_id;
                db.resume.Add(r);
                db.SaveChanges();
                var savedid = freelancer.freelancer_id;

                return RedirectToAction("Details", new { id = savedid });
            }

            return View(freelancer);
        }


        //http://localhost:50489/freelancer/Details?freelancer_id=26

        //http://localhost:50489/freelancer/Details?freelancer_id=28

        //http://localhost:50489/freelancers/details/5

        // GET: freelancers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            freelancer freelancer = db.freelancer.Find(id);
            if (freelancer == null)
            {
                return HttpNotFound();
            }
            return View(freelancer);
        }

        // POST: freelancers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "freelancer_id,firstname,lastname,address,city,zipcode,phonenumber,email,birthdate,birthcity,nationality,username,password")] freelancer freelancer)
        {

            if (ModelState.IsValid)
            {
                db.Entry(freelancer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = freelancer.freelancer_id });
            }
            return View(freelancer);
        }

        // GET: freelancers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            freelancer freelancer = db.freelancer.Find(id);
            if (freelancer == null)
            {
                return HttpNotFound();
            }
            return View(freelancer);
        }

        // POST: freelancers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            freelancer freelancer = db.freelancer.Find(id);
            db.freelancer.Remove(freelancer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private IEnumerable<string> GetCountries()
        {
            return new List<string>
            {
                "SWE",
                "NOR",
                "FIN",
                "DEN",
                "RUS",
                "USA",
                "ENG"
            };
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Johans test
            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }
            return selectList;
        }

        [HandleError]
        public ActionResult SaveFreelancer(int? id) //Sparar freelancer i tabellen customer_freelancer
        {
            int c = 5; //hårdkodad customer

            if (id != null)
            {
                try
                {
                    FreelancerProfileOperations fpop = new FreelancerProfileOperations();
                    fpop.SaveFreelancerToCustomerList(id, c);
                }
                catch (SqlException ex)
                {
                    //return;
                    //TODO: Visa ett felmeddelande
                }

                
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFreelancer(int? idfromview)
        {
            if (idfromview == null )
            {
                idfromview = 5;
            }

            int? customer_id = 5; //Hårdkodad customer
            int? id = idfromview; //Markerad freelancer

            FreelancerProfileOperations fpop = new FreelancerProfileOperations();
            fpop.RemoveFreelancerFromCustomerList(id, customer_id);

            //Refreshar samma sida
            return RedirectToAction("SavedFreelancers", "customers");  
        }

        public ActionResult CVPage(int? id)
        {
            FreelancerProfileOperations fpop = new FreelancerProfileOperations();

            return View(fpop.GetFreelancerProfileById(id));
        }
    }
}
