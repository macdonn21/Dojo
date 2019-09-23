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
    public class CertificatesController : Controller
    {
        private AdtProjectEntities db = new AdtProjectEntities();

        // GET: Certificates
        public ActionResult Index()
        {
            var certificates = db.Certificates.Include(c => c.Rank);
            return View(certificates.ToList());
        }

        // GET: Certificates/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate certificate = db.Certificates.Find(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            return View(certificate);
        }

        // GET: Certificates/Create
        public ActionResult Create()
        {
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title");
            return View();
        }

        // POST: Certificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Certificate_ID,Name,Rank_ID")] Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                var count = db.Certificates.ToList().Count();
                certificate.Certificate_ID = (count + 1).ToString();
                db.Certificates.Add(certificate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", certificate.Rank_ID);
            return View(certificate);
        }

        // GET: Certificates/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate certificate = db.Certificates.Find(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", certificate.Rank_ID);
            return View(certificate);
        }

        // POST: Certificates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Certificate_ID,Name,Rank_ID")] Certificate certificate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(certificate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", certificate.Rank_ID);
            return View(certificate);
        }

        // GET: Certificates/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate certificate = db.Certificates.Find(id);
            if (certificate == null)
            {
                return HttpNotFound();
            }
            return View(certificate);
        }

        // POST: Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Certificate certificate = db.Certificates.Find(id);
            db.Certificates.Remove(certificate);
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
