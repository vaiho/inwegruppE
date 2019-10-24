using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoorinWebApp.Models;

namespace DoorinWebApp.Controllers
{
    public class educationsController : Controller
    {
        private doorinDBEntities db = new doorinDBEntities();

        // GET: educations
        public ActionResult Index()
        {
            var education = db.education.Include(e => e.resume);
            return View(education.ToList());
        }

        // GET: educations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            education education = db.education.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // GET: educations/Create
        public ActionResult Create()
        {
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "driving_license");
            return View();
        }

        // POST: educations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "education_id,resume_id,title,description,date")] education education)
        {
            if (ModelState.IsValid)
            {
                db.education.Add(education);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "driving_license", education.resume_id);
            return View(education);
        }

        // GET: educations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            education education = db.education.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "driving_license", education.resume_id);
            return View(education);
        }

        // POST: educations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "education_id,resume_id,title,description,date")] education education)
        {
            if (ModelState.IsValid)
            {
                db.Entry(education).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "driving_license", education.resume_id);
            return View(education);
        }

        // GET: educations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            education education = db.education.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // POST: educations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            education education = db.education.Find(id);
            db.education.Remove(education);
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
