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
    public class workhistoriesController : Controller
    {
        private doorinDBEntities db = new doorinDBEntities();

        // GET: workhistories
        public ActionResult Index()
        {
            var workhistory = db.workhistory.Include(w => w.resume);
            return View(workhistory.ToList());
        }

        // GET: workhistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            workhistory workhistory = db.workhistory.Find(id);
            if (workhistory == null)
            {
                return HttpNotFound();
            }
            return View(workhistory);
        }

        // GET: workhistories/Create
        public ActionResult Create()
        {
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "driving_license");
            return View();
        }

        // POST: workhistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "workhistory_id,resume_id,employer,position,description,date")] workhistory workhistory)
        {
            if (ModelState.IsValid)
            {
                db.workhistory.Add(workhistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "driving_license", workhistory.resume_id);
            return View(workhistory);
        }

        // GET: workhistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            workhistory workhistory = db.workhistory.Find(id);
            if (workhistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "driving_license", workhistory.resume_id);
            return View(workhistory);
        }

        // POST: workhistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "workhistory_id,resume_id,employer,position,description,date")] workhistory workhistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workhistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "driving_license", workhistory.resume_id);
            return View(workhistory);
        }

        // GET: workhistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            workhistory workhistory = db.workhistory.Find(id);
            if (workhistory == null)
            {
                return HttpNotFound();
            }
            return View(workhistory);
        }

        // POST: workhistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            workhistory workhistory = db.workhistory.Find(id);
            db.workhistory.Remove(workhistory);
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

        public JsonResult InsertWorkhistory(List<workhistory> workhistory)
        {
            if (workhistory == null)
            {
                return Json(0);
            }

            if (workhistory.Count == 0)
            {
                return Json(0);
            }


            var resume_id = workhistory[0].resume_id;
            var existingWorkhistory = db.workhistory.Where(l => l.resume_id == resume_id).ToList();
            db.workhistory.RemoveRange(existingWorkhistory);

            foreach (var w in workhistory)
            {
                if (!(string.IsNullOrEmpty(w.employer) && string.IsNullOrEmpty(w.position) && string.IsNullOrEmpty(w.description) && string.IsNullOrEmpty(w.date)))
                {
                    db.workhistory.Add(w);
                }


            }
            int num = db.SaveChanges();
            return Json(num);
        }


    }
}
