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
    public class Taller2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Taller2
        public ActionResult Index()
        {
            return View(db.Taller2.ToList());
        }

        // GET: Taller2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller2 taller2 = db.Taller2.Find(id);
            if (taller2 == null)
            {
                return HttpNotFound();
            }
            return View(taller2);
        }

        // GET: Taller2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Taller2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Taller2,Nombre_Taller,Descripcion,CantidadParticipantes")] Taller2 taller2)
        {
            if (ModelState.IsValid)
            {
                db.Taller2.Add(taller2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taller2);
        }

        // GET: Taller2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller2 taller2 = db.Taller2.Find(id);
            if (taller2 == null)
            {
                return HttpNotFound();
            }
            return View(taller2);
        }

        // POST: Taller2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Taller2,Nombre_Taller,Descripcion,CantidadParticipantes")] Taller2 taller2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taller2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taller2);
        }

        // GET: Taller2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taller2 taller2 = db.Taller2.Find(id);
            if (taller2 == null)
            {
                return HttpNotFound();
            }
            return View(taller2);
        }

        // POST: Taller2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Taller2 taller2 = db.Taller2.Find(id);
            db.Taller2.Remove(taller2);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public JsonResult getDescripcion(int id)
        {
            var taller = db.Taller2.Find(id);
            string descripcion = taller.Descripcion;
            int CantidadParticipantes = taller.CantidadParticipantes;
            int cantRegistrados = taller.cantRegistrados;
            var res = new
            {
                descripcion = descripcion,
                CantidadParticipantes = CantidadParticipantes,
                cantRegistrados = cantRegistrados
            };
            return Json(res, JsonRequestBehavior.AllowGet);
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
