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
    public class TallersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tallers
        public ActionResult Index()
        {
            return View(db.Talleres.ToList());
        }

        // GET: Tallers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller taller = db.Talleres.Find(id);
            if (taller == null)
            {
                return HttpNotFound();
            }
            return View(taller);
        }

        // GET: Tallers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tallers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Taller,Nombre_Taller,Descripcion")] Taller taller)
        {
            if (ModelState.IsValid)
            {
                db.Talleres.Add(taller);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taller);
        }

        // GET: Tallers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller taller = db.Talleres.Find(id);
            if (taller == null)
            {
                return HttpNotFound();
            }
            return View(taller);
        }

        // POST: Tallers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Taller,Nombre_Taller,Descripcion")] Taller taller)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taller);
        }

        // GET: Tallers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller taller = db.Talleres.Find(id);
            if (taller == null)
            {
                return HttpNotFound();
            }
            return View(taller);
        }

        // POST: Tallers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Taller taller = db.Talleres.Find(id);
            db.Talleres.Remove(taller);
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
