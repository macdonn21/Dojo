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
    public class TestsController : Controller
    {
        private AdtProjectEntities db = new AdtProjectEntities();

        // GET: Tests
        public ActionResult Index()
        {
            // var tests = db.Tests.Include(t => t.Rank);
            ViewBag.TotalProfit = TotalProfit();
            return View(db.Tests.ToList());
        }

        // GET: Tests/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // GET: Tests/Create
        public ActionResult Create()
        {
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title");
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Test_ID,Name,Description,Price,Rank_ID")] Test test)
        {
            if (ModelState.IsValid)
            {
                var count = db.Tests.ToList().Count();
                test.Test_ID = (count + 1).ToString();

                db.Tests.Add(test);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", test.Rank_ID);
            return View(test);
        }

        // GET: Tests/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", test.Rank_ID);
            return View(test);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Test_ID,Name,Description,Price,Rank_ID")] Test test)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", test.Rank_ID);
            return View(test);
        }

        // GET: Tests/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }
        public string TotalProfit()
        {

            var tests = db.Tests.ToList();
            var total = tests.Sum(i => Convert.ToDecimal(i.Profit));

            return total.ToString();
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Test test = db.Tests.Find(id);
            db.Tests.Remove(test);
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
