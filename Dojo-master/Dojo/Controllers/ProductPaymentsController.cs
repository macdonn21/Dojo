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
    public class ProductPaymentsController : Controller
    {
        private AdtProjectEntities db = new AdtProjectEntities();

        // GET: ProductPayments
        public ActionResult Index()
        {
            var productPayments = db.ProductPayments.Include(p => p.Product).Include(p => p.Student);
            return View(productPayments.ToList());
        }

        // GET: ProductPayments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPayment productPayment = db.ProductPayments.Find(id);
            if (productPayment == null)
            {
                return HttpNotFound();
            }
            return View(productPayment);
        }

        // GET: ProductPayments/Create
        public ActionResult Create()
        {
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "ProductName");
            ViewBag.STUDENTs = db.Students.ToList();
            return View();
        }

        // POST: ProductPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Payment_ID,Product_ID,STUDENT_ID,Date,Quantity,Total")] ProductPayment productPayment)
        {
            if (ModelState.IsValid)
            {
                var count = db.ProductPayments.ToList().Count();
                productPayment.Payment_ID = (count + 1).ToString();
                productPayment.Date = DateTime.Now.ToShortDateString();
                //Logic
                //Get Product price from Product Table using ProductID
                //Calculate Total Cash paid: productPrice * Quantity
                var product = db.Products.SingleOrDefault(b => b.Product_ID == productPayment.Product_ID);
                var price = Convert.ToDecimal(product.SellingPrice);
                var quantity = Convert.ToDecimal(productPayment.Quantity);
                productPayment.Total = (quantity * price).ToString();
                //Update QuantityInStock in Product table : product.Quantityinstock - payment.Quantity 
                //Update QuantitySold in Product table : QuantitySold + payment.Quantity
                var quantityInStock = Convert.ToDecimal(product.QuantityInStock);
                var quantitySold = Convert.ToDecimal(product.QuantitySold);
                product.QuantityInStock = (quantityInStock - quantity).ToString();
                product.QuantitySold = (quantitySold + quantity).ToString();

                //Calculate Profit from product
                var selling = Convert.ToDecimal(product.SellingPrice);
                var cost = Convert.ToDecimal(product.CostPrice);
                product.Profit = ((selling - cost) * Convert.ToDecimal(product.QuantitySold)).ToString();

                db.ProductPayments.Add(productPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "ProductName", productPayment.Product_ID);
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", productPayment.STUDENT_ID);
            return View(productPayment);
        }

        // GET: ProductPayments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPayment productPayment = db.ProductPayments.Find(id);
            if (productPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "ProductName", productPayment.Product_ID);
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", productPayment.STUDENT_ID);
            return View(productPayment);
        }

        // POST: ProductPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Payment_ID,Product_ID,STUDENT_ID,Date,Quantity,Total")] ProductPayment productPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "ProductName", productPayment.Product_ID);
            ViewBag.STUDENT_ID = new SelectList(db.Students, "STUDENT_ID", "STUDENT_Fname", productPayment.STUDENT_ID);
            return View(productPayment);
        }

        // GET: ProductPayments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPayment productPayment = db.ProductPayments.Find(id);
            if (productPayment == null)
            {
                return HttpNotFound();
            }
            return View(productPayment);
        }

        // POST: ProductPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ProductPayment productPayment = db.ProductPayments.Find(id);
            db.ProductPayments.Remove(productPayment);
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
