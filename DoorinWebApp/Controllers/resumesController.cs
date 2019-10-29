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
            
            id = 5;

            freelancer freelancer = db.freelancer.Find(id);

            FullResumeOperations resumeOperations = new FullResumeOperations();

            return View(resumeOperations.GetFullResumeById(id));
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
                return RedirectToAction("Index");
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
    }
}
