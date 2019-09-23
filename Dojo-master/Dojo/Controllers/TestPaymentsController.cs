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
    public class TestPaymentsController : Controller
    {
        private AdtProjectEntities db = new AdtProjectEntities();

        // GET: TestPayments
        public ActionResult Index()
        {
           // var testPayments = db.TestPayments.Include(t => t.Student).Include(t => t.Test);
            return View(db.TestPayments.ToList());
        }

        // GET: TestPayments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestPayment testPayment = db.TestPayments.Find(id);
            if (testPayment == null)
            {
                return HttpNotFound();
            }
            return View(testPayment);
        }

        // GET: TestPayments/Create
        public ActionResult Create()
        {
            ViewBag.STUDENTs = db.Students.ToList();
            ViewBag.Test_ID = new SelectList(db.Tests, "Test_ID", "Name");
            return View();
        }

        // POST: TestPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TestPayment_ID,Test_ID,STUDENT_ID,Date")] TestPayment testPayment)
        {
            if (ModelState.IsValid)
            {
                var count = db.TestPayments.ToList().Count();
                testPayment.TestPayment_ID = (count + 1).ToString();
                testPayment.Date = DateTime.Now;

                //Calculate Profit from test
                var test = db.Tests.SingleOrDefault(b => b.Test_ID == testPayment.Test_ID);
                var cost = Convert.ToDecimal(test.Price);
                if (string.IsNullOrEmpty(test.Profit)) { test.Profit = "0"; }
                var profit = Convert.ToDecimal(test.Profit);

                test.Profit = (profit + cost).ToString();
                
                db.TestPayments.Add(testPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", testPayment.STUDENT_ID);
            ViewBag.Test_ID = new SelectList(db.Tests, "Test_ID", "Name", testPayment.Test_ID);
            return View(testPayment);
        }

        // GET: TestPayments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestPayment testPayment = db.TestPayments.Find(id);
            if (testPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", testPayment.STUDENT_ID);
            ViewBag.Test_ID = new SelectList(db.Tests, "Test_ID", "Name", testPayment.Test_ID);
            return View(testPayment);
        }

        // POST: TestPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TestPayment_ID,Test_ID,STUDENT_ID,Date")] TestPayment testPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", testPayment.STUDENT_ID);
            ViewBag.Test_ID = new SelectList(db.Tests, "Test_ID", "Name", testPayment.Test_ID);
            return View(testPayment);
        }

        // GET: TestPayments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestPayment testPayment = db.TestPayments.Find(id);
            if (testPayment == null)
            {
                return HttpNotFound();
            }
            return View(testPayment);
        }

        // POST: TestPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TestPayment testPayment = db.TestPayments.Find(id);
            db.TestPayments.Remove(testPayment);
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
