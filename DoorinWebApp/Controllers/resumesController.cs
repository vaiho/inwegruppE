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
    public class resumesController : Controller
    {
        private doorinDBEntities db = new doorinDBEntities();

        // GET: resumes
        public ActionResult Index()
        {
            var resume = db.resume.Include(r => r.freelancer);
            return View(resume.ToList());
        }

        // GET: resumes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resume resume = db.resume.Find(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            return View(resume);
        }

        // GET: resumes/Create
        public ActionResult Create()
        {
            ViewBag.freelancer_id = new SelectList(db.freelancer, "freelancer_id", "firstname");
            return View();
        }

        // POST: resumes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "resume_id,freelancer_id,driving_license,profile")] resume resume)
        {
            if (ModelState.IsValid)
            {
                db.resume.Add(resume);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.freelancer_id = new SelectList(db.freelancer, "freelancer_id", "firstname", resume.freelancer_id);
            return View(resume);
        }

        // GET: resumes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resume resume = db.resume.Find(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            ViewBag.freelancer_id = new SelectList(db.freelancer, "freelancer_id", "firstname", resume.freelancer_id);
            
            //freelancer freelancer = db.freelancer.Find(id);

            FullResumeOperations resumeOperations = new FullResumeOperations();

            var driving_licence = GetYesOrNo();
            var fullResume = resumeOperations.GetFullResumeById(id);
            fullResume.DrivingLicenceChoice = GetSelectListItems(driving_licence);
            fullResume.Link = db.links.Where(l => l.resume_id == id).ToList();
            fullResume.MyWorkhistory = db.workhistory.Where(w => w.resume_id == id).ToList();

            return View(fullResume);
        }

        [HttpPost]
        public ActionResult AddMyCompetences(competence competence)
        {
            FullResumeOperations resumeOperations = new FullResumeOperations();
            var fullResume = resumeOperations.GetFullResumeById(competence.resume_id);
            fullResume.SelectedCompetenceId = competence.competence_id;

            if (fullResume.MyCompetences.Count == 0)
            {
                for (int i = 0; i < fullResume.Competences.Count; i++)
                {
                    if (fullResume.Competences[i].competence_id == fullResume.SelectedCompetenceId)
                    {
                        fullResume.MyCompetences.Add(fullResume.Competences[i]);
                        int lastComp = fullResume.MyCompetences.Count;
                        lastComp--;
                        resumeOperations.AddMyCompetences(fullResume.MyCompetences[lastComp].competence_id, fullResume.Resume_id);
                        resumeOperations.GetTechnologyList(fullResume);

                        int num = db.SaveChanges();
                        return Json(num);
                    }
                }
            }
            else
            {

                foreach (var comp in fullResume.MyCompetences)
                {
                    if (comp.competence_id == fullResume.SelectedCompetenceId)
                    {
                        // Visa meddelande "Du har redan lagt till den här kompetensen."
                    }
                    else
                    {
                        for (int i = 0; i < fullResume.Competences.Count; i++)
                        {
                            if (fullResume.Competences[i].competence_id == fullResume.SelectedCompetenceId)
                            {
                                fullResume.MyCompetences.Add(fullResume.Competences[i]);
                                int lastComp = fullResume.MyCompetences.Count;
                                lastComp--;
                                resumeOperations.AddMyCompetences(fullResume.MyCompetences[lastComp].competence_id, fullResume.Resume_id);
                                resumeOperations.GetTechnologyList(fullResume);
                                int num = db.SaveChanges();
                                return Json(num);
                            }
                        }
                    }
                }
            }
                              
            return View(fullResume);
        }

        [HttpPost]
        public ActionResult AddMyTecgnologies(technology_resume objectTechnology)
        {
            if (objectTechnology.rank == null && objectTechnology.core_technology == null)
            {
                objectTechnology.rank = 0;
                objectTechnology.core_technology =false;
                
            }
            else if (objectTechnology.rank == null)
            {
                objectTechnology.rank = 0;
            }
            else if (objectTechnology.core_technology == null)
            {
                objectTechnology.core_technology = false;
            }

            FullResumeOperations resumeOperations = new FullResumeOperations();
            resumeOperations.AddMyTechnologies(objectTechnology.technology_id, objectTechnology.resume_id, 
            objectTechnology.core_technology, objectTechnology.rank);
            // CV:t hämtas för att listan med kompetenser ska uppdateras
            FullResume fullResume = resumeOperations.GetFullResumeById(objectTechnology.resume_id);

            int num = db.SaveChanges();
            return Json(num);
        }

        [HttpPost]
        public ActionResult RemoveMyTecgnologies(technology_resume objectTechnology)
        {
            FullResumeOperations resumeOperations = new FullResumeOperations();
            resumeOperations.RemoveMyTechnologies(objectTechnology.technology_id, objectTechnology.resume_id);
            // CV:t hämtas för att listan med kompetenser ska uppdateras
            FullResume fullResume = resumeOperations.GetFullResumeById(objectTechnology.resume_id);

            int num = db.SaveChanges();
            return Json(num);
        }



        [HttpPost]
        public ActionResult RemoveMyCompetences(competence competence)
        {
            FullResumeOperations resumeOperations = new FullResumeOperations();
            resumeOperations.RemoveMyCompetences(competence.competence_id, competence.resume_id);
            // CV:t hämtas för att listan med kompetenser ska uppdateras
            FullResume fullResume = resumeOperations.GetFullResumeById(competence.resume_id);

            int num = db.SaveChanges();
            return Json(num);
        }

        // POST: resumes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "resume_id,freelancer_id,driving_license,profile")] FullResume fullResume)
        {
            resume resume = new resume();
            resume.resume_id = fullResume.Resume_id;
            resume.freelancer_id = fullResume.Freelancer_id;
            resume.driving_license = fullResume.Driving_license;
            resume.profile = fullResume.Profile.Trim();


            
            if (ModelState.IsValid)
            {
                db.Entry(resume).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "freelancers", new { id = resume.freelancer_id });
                //return RedirectToAction("Index");
            }
            //ViewBag.freelancer_id = new SelectList(db.freelancer, "freelancer_id", "firstname", resume.freelancer_id);
            return View(resume);
        }

    

        // GET: resumes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resume resume = db.resume.Find(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            return View(resume);
        }

        // POST: resumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            resume resume = db.resume.Find(id);
            db.resume.Remove(resume);
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
        

        private IEnumerable<string> GetYesOrNo()
        {
            return new List<string>
            {
                "Ja",
                "Nej",
            };
        }

        
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
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
