using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RegistroJATICS.Models;

namespace RegistroJATICS.Controllers
{
    [Authorize(Roles = "admin")]
    public class InstitucionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Institucions
        public ActionResult Index()
        {
            return View(db.Institucions.ToList());
        }

        // GET: Institucions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institucion institucion = db.Institucions.Find(id);
            if (institucion == null)
            {
                return HttpNotFound();
            }
            return View(institucion);
        }

        // GET: Institucions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Institucions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre")] Institucion institucion)
        {
            if (ModelState.IsValid)
            {
                db.Institucions.Add(institucion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(institucion);
        }

        // GET: Institucions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institucion institucion = db.Institucions.Find(id);
            if (institucion == null)
            {
                return HttpNotFound();
            }
            return View(institucion);
        }

        // POST: Institucions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nombre")] Institucion institucion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(institucion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(institucion);
        }

        // GET: Institucions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institucion institucion = db.Institucions.Find(id);
            if (institucion == null)
            {
                return HttpNotFound();
            }
            return View(institucion);
        }

        // POST: Institucions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Institucion institucion = db.Institucions.Find(id);
            db.Institucions.Remove(institucion);
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
