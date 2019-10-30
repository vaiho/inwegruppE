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
    public class linksController : Controller
    {
        private doorinDBEntities db = new doorinDBEntities();

        // GET: links
        public ActionResult Index()
        {
            var links = db.links.Include(l => l.resume);
            return View(links.ToList());
        }

        // GET: links/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            links links = db.links.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }
            return View(links);
        }

        // GET: links/Create
        public ActionResult Create()
        {
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "resume_id");
            return View();
        }

        // POST: links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "link_id,resume_id,name,link")] links links)
        {
            if (ModelState.IsValid)
            {
                db.links.Add(links);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "resume_id", links.resume_id);
            return View(links);
        }

        // GET: links/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            links links = db.links.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "resume_id", links.resume_id);
            return View(links);
        }

        // POST: links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "link_id,resume_id,name,link")] links links)
        {
            if (ModelState.IsValid)
            {
                db.Entry(links).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.resume_id = new SelectList(db.resume, "resume_id", "resume_id", links.resume_id);
            return View(links);
        }

        // GET: links/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            links links = db.links.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }
            return View(links);
        }

        // POST: links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            links links = db.links.Find(id);
            db.links.Remove(links);
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

        //public ActionResult Index()
        //{
        //    links Links = new links();
        //    return View(Links.link);
        //}

        public JsonResult InsertLinks(List<links> links)
        {
            foreach (var link in links)
            {
                if (!(string.IsNullOrEmpty(link.name) && string.IsNullOrEmpty(link.link)))
                {
                    db.links.Add(link);
                }

                  
            }
            int num = db.SaveChanges();
            return Json(num);
            //        using (links Links = new links())
            //        {
            //            //Truncate Table to delete all old records.
            //            entities.Database.ExecuteSqlCommand("TRUNCATE TABLE [links]");

            //            //Check for NULL.
            //            if (customers == null)
            //            {
            //                customers = new List<links>();
            //            }

            //            //Loop and insert records.
            //            foreach (Customer customer in customers)
            //            {
            //                Links.link.Add(link);
            //            }
            //            int insertedRecords = entities.SaveChanges();
            //            return Json(insertedRecords);
            //        }
        }


    }
}
