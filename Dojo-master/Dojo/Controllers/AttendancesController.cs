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
    public class AttendancesController : Controller
    {
        private AdtProjectEntities db = new AdtProjectEntities();

        // GET: Attendances

        public ActionResult Index(string sortOrder, string searchString, string datestring, string townString, string currentFilter, int? page)
        {
            var attendances = db.Attendances.Include(a => a.Student);
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
           

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (datestring != null)
            {
                page = 1;
            }
            else
            {
                datestring = currentFilter;
            }
            ViewBag.CurrentFilter = searchString ?? datestring;

            var attendance = db.Attendances.ToList();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    attendance = db.Attendances.Where(x => x.Student.STUDENT_Fname.ToUpper().Contains(searchString.ToUpper())).ToList();

                }
                //if (!string.IsNullOrEmpty(datestring))
                //{
                //    attendance = db.Attendances.Where(x => x.Date.ToShortDateString().ToUpper().Contains(datestring.ToUpper())).ToList();

                //}
              
                switch (sortOrder)
                {
                    case "name_desc":
                        attendance = attendance.OrderByDescending(a => a.STUDENT_ID).ToList();
                        break;
                    case "id_desc":
                        attendance = attendance.OrderByDescending(a => a.Attendance_ID).ToList();
                        break;
                    
                    case "date_desc":
                        attendance = attendance.OrderByDescending(a => a.Date).ToList();
                        break;
                    default:
                        attendance = attendance.OrderBy(u => u.Attendance_ID).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(attendance.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new Attendance());
        }
        //public ActionResult Index()
        //{
        //  //  var attendances = db.Attendances.Include(a => a.Student);
        //    return View(db.Attendances.ToList());
        //}

        // GET: Attendances/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            ViewBag.STUDENTs = db.Students.ToList();

            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Attendance_ID,STUDENT_ID,Date")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                var count = db.Attendances.ToList().Count();
                attendance.Attendance_ID = (count + 1).ToString();
                attendance.Date = DateTime.Now;
                var student = db.Students.SingleOrDefault(b => b.STUDENT_ID == attendance.STUDENT_ID);
                //Update Attendance count of Student
                student.AttendanceCount = (Convert.ToInt64(student.AttendanceCount) + 1).ToString();

                db.Attendances.Add(attendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", attendance.STUDENT_ID);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", attendance.STUDENT_ID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Attendance_ID,STUDENT_ID,Date")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", attendance.STUDENT_ID);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Attendance attendance = db.Attendances.Find(id);
            db.Attendances.Remove(attendance);
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
