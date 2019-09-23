using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dojo.Models;
using PagedList;

namespace Dojo.Controllers
{
    public class StudentsController : Controller
    {
        private AdtProjectEntities db = new AdtProjectEntities();

        // GET: Students
        public ActionResult Index(string sortOrder, string searchString, string parentString, string townString, string currentFilter, int? page)
        {
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.RankSortParm = sortOrder == "Rank" ? "rank_desc" : "Rank";
            ViewBag.AttendSortParm = sortOrder == "Attend" ? "att_desc" : "Attend";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (parentString != null)
            {
                page = 1;
            }
            else
            {
                parentString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString ?? parentString;

            var students = db.Students.ToList();
            ViewBag.E_Count = db.Students.ToList().Count();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    students = db.Students.Where(x => x.STUDENT_Fname.ToUpper().Contains(searchString.ToUpper())).ToList();

                }
                if (!string.IsNullOrEmpty(parentString))
                {
                    students = db.Students.Where(x => x.Parent_ID.ToUpper().Contains(parentString.ToUpper())).ToList();

                }
                if (!string.IsNullOrEmpty(townString))
                {
                    students = db.Students.Where(x => x.Town.ToUpper().Contains(townString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        students = students.OrderByDescending(a => a.STUDENT_Fname).ToList();
                        break;
                    case "id_desc":
                        students = students.OrderByDescending(a => a.STUDENT_ID).ToList();
                        break;
                    case "rank_desc":
                        students = students.OrderByDescending(a => a.Rank_ID).ToList();
                        break;
                    case "att_desc":
                        students = students.OrderByDescending(a => a.AttendanceCount).ToList();
                        break;
                    default:
                        students = students.OrderBy(u => u.STUDENT_Lname).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(students.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new Student());
        }

        //public ActionResult Index()
        //{
        //    var students = db.Students.Include(s => s.Membership).Include(s => s.Rank);
        //    return View(students.ToList());
        //}

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Membership_ID = new SelectList(db.Memberships, "Membership_ID", "Name");
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title");
            ViewBag.Parent_ID = new SelectList(db.Ranks, "Parent_ID", "Email");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "STUDENT_ID,STUDENT_Fname,STUDENT_Lname,STUDENT_DOB,StreetName,Town,PostalCode,PhoneNumber,email,DateJoined,Membership_ID,IsActive,IsParent,Rank_ID,Parent_ID,AttendanceCount,MembershipValidTill")] Student student)
        {
           

            if (ModelState.IsValid)
            {
                var emails = db.Students.Where(b => b.email == student.email).Count();
                if(emails > 0)
                {
                    ModelState.AddModelError("email", "This email already exists.");
                }
                var count = db.Students.ToList().Count();
                student.STUDENT_ID = (count + 1).ToString();
                student.DateJoined = DateTime.Now;
                student.AttendanceCount = "0";
                student.Parent_ID = student.Parent_ID ?? " ";

                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Membership_ID = new SelectList(db.Memberships, "Membership_ID", "Name", student.Membership_ID);
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", student.Rank_ID);
            ViewBag.Parent_ID = new SelectList(db.Ranks, "Parent_ID", "Email", student.Parent_ID);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Membership_ID = new SelectList(db.Memberships, "Membership_ID", "Name", student.Membership_ID);
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", student.Rank_ID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "STUDENT_ID,STUDENT_Fname,STUDENT_Lname,STUDENT_DOB,StreetName,Town,PostalCode,PhoneNumber,email,DateJoined,Membership_ID,IsActive,IsParent,Rank_ID,Parent_ID")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.AttendanceCount = student.AttendanceCount;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Membership_ID = new SelectList(db.Memberships, "Membership_ID", "Name", student.Membership_ID);
            ViewBag.Rank_ID = new SelectList(db.Ranks, "Rank_ID", "Title", student.Rank_ID);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
