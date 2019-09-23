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
    public class MembershipPaymentsController : Controller
    {
        private AdtProjectEntities db = new AdtProjectEntities();

        // GET: MembershipPayments
        public ActionResult Index()
        {
            var membershipPayments = db.MembershipPayments.Include(m => m.Membership);
           
            return View(membershipPayments.ToList());
        }

        // GET: MembershipPayments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipPayment membershipPayment = db.MembershipPayments.Find(id);
            if (membershipPayment == null)
            {
                return HttpNotFound();
            }
            return View(membershipPayment);
        }

        // GET: MembershipPayments/Create
        public ActionResult Create()
        {
            ViewBag.Membership_ID = new SelectList(db.Memberships, "Membership_ID", "Name");
            ViewBag.STUDENTs = db.Students.ToList();
            return View();
        }

        // POST: MembershipPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MembershipPayment_ID,Membership_ID,STUDENT_ID,Date")] MembershipPayment membershipPayment)
        {
            if (ModelState.IsValid)
            {
                //update set ID and Date
                var count = db.MembershipPayments.ToList().Count();
                membershipPayment.MembershipPayment_ID = (count + 1).ToString();
                membershipPayment.Date = DateTime.Now;

                //Update student membership
                var student = db.Students.SingleOrDefault(b => b.STUDENT_ID == membershipPayment.STUDENT_ID);
                membershipPayment.Membership = db.Memberships.SingleOrDefault(b => b.Membership_ID == membershipPayment.Membership_ID);

                student.Membership = membershipPayment.Membership;
                student.MembershipValidTill = DateTime.Now.AddDays(Convert.ToDouble(membershipPayment.Membership.Duration));

                //Calculate Profit from Memberships
                var membership = db.Memberships.SingleOrDefault(b => b.Membership_ID == membershipPayment.Membership_ID);
                var cost = Convert.ToDecimal(membership.Price);
                if (string.IsNullOrEmpty(membership.Profit)) { membership.Profit = "0"; }
                var profit = Convert.ToDecimal(membership.Profit);

                membership.Profit = (profit + cost).ToString();


                db.MembershipPayments.Add(membershipPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", membershipPayment.STUDENT_ID);
            ViewBag.Membership_ID = new SelectList(db.Memberships, "Membership_ID", "Name", membershipPayment.Membership_ID);
            return View(membershipPayment);
        }

        // GET: MembershipPayments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipPayment membershipPayment = db.MembershipPayments.Find(id);
            if (membershipPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", membershipPayment.STUDENT_ID);
            ViewBag.Membership_ID = new SelectList(db.Memberships, "Membership_ID", "Name", membershipPayment.Membership_ID);
            return View(membershipPayment);
        }

        // POST: MembershipPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MembershipPayment_ID,Membership_ID,STUDENT_ID,Date")] MembershipPayment membershipPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membershipPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Membership_ID = new SelectList(db.Memberships, "Membership_ID", "Name", membershipPayment.Membership_ID);
            return View(membershipPayment);
        }

        // GET: MembershipPayments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipPayment membershipPayment = db.MembershipPayments.Find(id);
            if (membershipPayment == null)
            {
                return HttpNotFound();
            }
            return View(membershipPayment);
        }

        // POST: MembershipPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MembershipPayment membershipPayment = db.MembershipPayments.Find(id);
            db.MembershipPayments.Remove(membershipPayment);
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
