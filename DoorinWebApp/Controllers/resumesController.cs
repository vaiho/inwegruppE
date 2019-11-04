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

            return View(fullResume);
        }

        [HttpPost]
        public ActionResult AddMyCompetences(FullResume objectResume)
        {
            FullResumeOperations resumeOperations = new FullResumeOperations();
            var fullResume = resumeOperations.GetFullResumeById(objectResume.Resume_id);
            fullResume.SelectedCompetenceId = objectResume.SelectedCompetenceId;

            if (fullResume.MyCompetences.Count == 0)
            {
                for (int i = 0; i < fullResume.MyCompetences.Count; i++)
                {
                    if (fullResume.Competences[i].competence_id == fullResume.SelectedCompetenceId)
                    {
                        fullResume.MyCompetences.Add(fullResume.Competences[i]);
                        int lastComp = fullResume.MyCompetences.Count;
                        lastComp--;
                        resumeOperations.AddMyCompetences(fullResume.MyCompetences[lastComp].competence_id, fullResume.Resume_id);

                        //vet inte vad jag ska lägga i "num" nu när jag gör en SQL-fråga?
                        int num = db.SaveChanges();
                        return Json(num);
                    }
                }
            }
            else
            {

                foreach (var competence in fullResume.MyCompetences)
                {
                    if (competence.competence_id == fullResume.SelectedCompetenceId)
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
                         
                                //vet inte vad jag ska lägga i "num" nu när jag gör en SQL-fråga?
                                int num = db.SaveChanges();
                                return Json(num);
                            }
                        }
                    }
                }
            }
                              
            return View(fullResume);
        }

        // POST: resumes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "resume_id,freelancer_id,driving_license,profile")] resume resume)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resume).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "freelancers", new { id = resume.freelancer_id });
                //return RedirectToAction("Index");
            }
            ViewBag.freelancer_id = new SelectList(db.freelancer, "freelancer_id", "firstname", resume.freelancer_id);
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
                "Yes",
                "No",
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
