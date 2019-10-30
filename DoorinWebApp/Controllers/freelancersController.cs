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
            //List<FreelancerProfileVM> freelancers = new List<FreelancerProfileVM>();
            var freelancers = fpop.GetFreelancersList(); //Hämtar alla frilansare
            var list = from s in freelancers select s; //Sparar alla frilansare i en variabel
            
            List<FreelancerProfileVM> filteredList = new List<FreelancerProfileVM>();


            if (!String.IsNullOrEmpty(searchString)) //Om söksträngen inte är NULL
            {
                //Om man vill söka på namn:
                //list = freelancers.Where(x => x.Firstname.Contains(searchString) || x.Lastname.Contains(searchString));
                
                foreach (var item in list)
                {
                    var a = from comp in item.CompetencesList select comp;
                    var b = from tech in item.TechnologysList select tech;

                    foreach (var it in a)
                        {
                            if (it.name.Contains(searchString))
                            {
                                filteredList.Add(item); //Lägger till freelancer i filtrerad lista om den har söksträngen i sin lista av kompetenser
                            }                      
                        }
                    foreach (var te in b)
                        {
                            if (te.name.Contains(searchString))
                            {
                                filteredList.Add(item); //Lägger till freelancer i filtrerad lista om den har söksträngen i sin lista av teknologier
                            }
                        }
                }
                //Returnerar den filtrerade listan
                return View(filteredList);
            }

            //Annars skickas en ofiltrerad lista tillbaka
            return View(list.ToList());


            //OLD:
            //return View(db.freelancer.ToList()); 1 orginal, 
            //return View(fpop.GetFreelancersList()); 2, om man vill ha hela listan
        }

        // GET: freelancers/Details/5
        public ActionResult Details(int? id)
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
                
                return RedirectToAction("Index");
            }

            return View(freelancer);
        }

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
                return RedirectToAction("Index");
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
    }
}
