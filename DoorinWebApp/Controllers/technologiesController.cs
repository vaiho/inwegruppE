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
    public class technologiesController : Controller
    {
        private doorinDBEntities db = new doorinDBEntities();

        // GET: technologies
        public ActionResult Index()
        {
            var technology = db.technology.Include(t => t.competence);
            return View(technology.ToList());
        }

        // GET: technologies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            technology technology = db.technology.Find(id);
            if (technology == null)
            {
                return HttpNotFound();
            }
            return View(technology);
        }

        // GET: technologies/Create
        public ActionResult Create()
        {
            ViewBag.competence_id = new SelectList(db.competence, "competence_id", "name");
            return View();
        }

        // POST: technologies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "technology_id,competence_id,name,rank,core")] technology technology)
        {
            if (ModelState.IsValid)
            {
                db.technology.Add(technology);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.competence_id = new SelectList(db.competence, "competence_id", "name", technology.competence_id);
            return View(technology);
        }

        // GET: technologies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            technology technology = db.technology.Find(id);
            if (technology == null)
            {
                return HttpNotFound();
            }
            ViewBag.competence_id = new SelectList(db.competence, "competence_id", "name", technology.competence_id);
            return View(technology);
        }

        // POST: technologies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "technology_id,competence_id,name,rank,core")] technology technology)
        {
            if (ModelState.IsValid)
            {
                db.Entry(technology).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.competence_id = new SelectList(db.competence, "competence_id", "name", technology.competence_id);
            return View(technology);
        }

        // GET: technologies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            technology technology = db.technology.Find(id);
            if (technology == null)
            {
                return HttpNotFound();
            }
            return View(technology);
        }

        // POST: technologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            technology technology = db.technology.Find(id);
            db.technology.Remove(technology);
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
