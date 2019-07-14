using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsuranceMvc.Models;

namespace CarInsuranceMvc.Controllers
{
    public class insureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();
        private readonly int dateOfBirth;

        // GET: insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: insuree/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarMake,CarModel,CarYear,DUI,SpeedingTickets,FullCoverage,Quote")] Insuree insuree)
        {


            if (ModelState.IsValid)
            {
                //adding conditions to create accurate quote
                insuree.Quote = 50m;

                var age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (age < 18)
                {
                    insuree.Quote += 100;
                }
                else if (age < 25 || age > 100)
                {
                    insuree.Quote += 25;
                }

                if (insuree.CarYear < 2000 || insuree.CarYear > 2015)
                {
                    insuree.Quote += 25m;
                }

                if (insuree.CarModel == "911 Carrera" && insuree.CarMake == "Porche")
                {
                    insuree.Quote += 50m;
                }
                else if (insuree.CarMake == "Porche")
                {
                    insuree.Quote += 25m;
                }

                if (insuree.SpeedingTickets > 0)
                {
                    insuree.Quote += insuree.SpeedingTickets * 10m;
                }

                if (insuree.DUI == true)
                {
                    insuree.Quote += insuree.Quote / 4;
                }

                if (insuree.FullCoverage == true)
                {
                    insuree.Quote += insuree.Quote / 2;
                }

                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: insuree/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarMake,CarModel,CarYear,DUI,SpeedingTickets,FullCoverage,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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

        //add view for admin
        public ActionResult Admin()
        {
            return View(db.Insurees.ToList());
        }
    }
}
