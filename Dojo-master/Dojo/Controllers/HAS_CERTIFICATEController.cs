using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dojo.Models;

namespace Dojo.Controllers
{
    public class HAS_CERTIFICATEController : Controller
    {
        private AdtProjectEntities db = new AdtProjectEntities();

        // GET: HAS_CERTIFICATE
        public ActionResult Index()
        {
            var hAS_CERTIFICATE = db.HAS_CERTIFICATE.Include(h => h.Certificate).Include(h => h.Student);
            return View(hAS_CERTIFICATE.ToList());
        }

        // GET: HAS_CERTIFICATE/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HAS_CERTIFICATE hAS_CERTIFICATE = db.HAS_CERTIFICATE.Find(id);
            if (hAS_CERTIFICATE == null)
            {
                return HttpNotFound();
            }
            return View(hAS_CERTIFICATE);
        }

        // GET: HAS_CERTIFICATE/Create
        public ActionResult Create()
        {
            ViewBag.Certificate_ID = new SelectList(db.Certificates, "Certificate_ID", "Name");
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname");
            return View();
        }

        // POST: HAS_CERTIFICATE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "STUDENT_ID,Certificate_ID,DateObtained,Issue_ID")] HAS_CERTIFICATE hAS_CERTIFICATE)
        {
            if (ModelState.IsValid)
            {
                //update Id and date
                var count = db.HAS_CERTIFICATE.ToList().Count();
                hAS_CERTIFICATE.Issue_ID = (count + 1).ToString();
                hAS_CERTIFICATE.DateObtained = DateTime.Now.ToShortDateString();

                //get student id and update rank

                var student = db.Students.SingleOrDefault(b => b.STUDENT_ID == hAS_CERTIFICATE.STUDENT_ID);
                var cert = db.Certificates.SingleOrDefault(b => b.Certificate_ID == hAS_CERTIFICATE.Certificate_ID);
                student.Rank_ID = cert.Rank_ID;
               // student.HAS_CERTIFICATE.Add(hAS_CERTIFICATE);

                // update 

                db.HAS_CERTIFICATE.Add(hAS_CERTIFICATE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Certificate_ID = new SelectList(db.Certificates, "Certificate_ID", "Name", hAS_CERTIFICATE.Certificate_ID);
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", hAS_CERTIFICATE.STUDENT_ID);
            return View(hAS_CERTIFICATE);
        }

        // GET: HAS_CERTIFICATE/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HAS_CERTIFICATE hAS_CERTIFICATE = db.HAS_CERTIFICATE.Find(id);
            if (hAS_CERTIFICATE == null)
            {
                return HttpNotFound();
            }
            ViewBag.Certificate_ID = new SelectList(db.Certificates, "Certificate_ID", "Name", hAS_CERTIFICATE.Certificate_ID);
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", hAS_CERTIFICATE.STUDENT_ID);
            return View(hAS_CERTIFICATE);
        }

        // POST: HAS_CERTIFICATE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "STUDENT_ID,Certificate_ID,DateObtained,Issue_ID")] HAS_CERTIFICATE hAS_CERTIFICATE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hAS_CERTIFICATE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Certificate_ID = new SelectList(db.Certificates, "Certificate_ID", "Name", hAS_CERTIFICATE.Certificate_ID);
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", hAS_CERTIFICATE.STUDENT_ID);
            return View(hAS_CERTIFICATE);
        }

        // GET: HAS_CERTIFICATE/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HAS_CERTIFICATE hAS_CERTIFICATE = db.HAS_CERTIFICATE.Find(id);
            if (hAS_CERTIFICATE == null)
            {
                return HttpNotFound();
            }
            return View(hAS_CERTIFICATE);
        }

        // POST: HAS_CERTIFICATE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HAS_CERTIFICATE hAS_CERTIFICATE = db.HAS_CERTIFICATE.Find(id);
            db.HAS_CERTIFICATE.Remove(hAS_CERTIFICATE);
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
