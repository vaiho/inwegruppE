using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            var allFreelancersList = fpop.GetFreelancersList(); //Hämtar alla frilansare

            //Viewbags här
            ViewBag.Competence = GetCompetences();
            ViewBag.Technology = GetTechnologies();

            if (!String.IsNullOrEmpty(searchString)) //Om söksträngen inte är NULL
            {
                var list = from s in allFreelancersList select s; //Sparar alla frilansare i variabel

                //Kollar om söksträngen finns bland kompetenser, teknologier, förnamn eller efternamn
                list = list.Where(x => x.CompetencesList.Any(z => z.name.Contains(searchString)) || x.TechnologysList.Any(z => z.name.Contains(searchString)) || x.Firstname.Contains(searchString) || x.Lastname.Contains(searchString));
            
                //Returnerar den filtrerade listan
                return View(list.ToList());
            }

            //Annars skickas en ofiltrerad lista tillbaka
            return View(allFreelancersList);



            //if (id == null)
            //    return View(fpop.GetFreelancersList());
            //else
            //    return View(fpop.FilterByCompetence(id));
        }
        private List<competence> GetCompetences()
        {

            List<competence> CList = new List<competence>();
            var competencelist = (from c in db.competence
                                  select new { c.name, c.competence_id }).ToList();

            foreach (var v in competencelist)
            {
                competence item = new competence();
                item.name = v.name;
                item.competence_id = v.competence_id;
                CList.Add(item);
            }

            return (CList);
        }

        private List<technology> GetTechnologies()
        {
            List<technology> TList = new List<technology>();
            var technologylist = (from t in db.technology
                                  select new { t.name }).ToList();

            foreach (var t in technologylist)
            {
                technology item = new technology();
                item.name = t.name;
                TList.Add(item);
            }

            return (TList);
        }

        // GET: freelancers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //freelancer freelancer = db.freelancer.Find(id);
            FreelancerOperations fop = new FreelancerOperations();
            freelancer freelancer = fop.GetFreelancerById(id);

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
            //Johans test
            var countries = GetCountries();
            freelancer fr = new freelancer();
            freelancer.country = GetSelectListItems(countries); 
            

            if (ModelState.IsValid)
            {
                db.freelancer.Add(freelancer);
                resume r = new resume();
                r.profile = "";
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

        public ActionResult SaveFreelancer(int? id) //Sparar freelancer i tabellen customer_freelancer
        {
            int c = 5; //hårdkodad customer

            if (id != null)
            {
                FreelancerProfileOperations fpop = new FreelancerProfileOperations();
                fpop.SaveFreelancerToCustomerList(id, c);
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
    }
}
