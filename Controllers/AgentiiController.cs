using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebManagementExcelDatabase;
using System.Linq.Dynamic; //sort
using System.Web.Helpers; //gridView

namespace WebManagementExcelDatabase.Controllers
{
    [Authorize(Roles = "Administrator,Membru")]
    public class AgentiiController : Controller
    {
        private Agentii2Entities25 db = new Agentii2Entities25();

        public ActionResult Index(int page = 1, string sort = "Agentie", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetTables(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        [HttpPost]
        public ActionResult SaveData(List<Agentii_Table> Agentii_Table)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (Agentii2Entities25 dc = new Agentii2Entities25())
                {
                    foreach (var i in Agentii_Table)
                    {

                        dc.Agentii_Table.Add(i);
                    }
                    dc.SaveChanges();
                    status = true;
                    Console.WriteLine(status);
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public List<Agentii_Table> GetTables(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            using (Agentii2Entities25 dc = new Agentii2Entities25())
            {
                var v = (from a in dc.Agentii_Table
                         where
                                 a.Agentie.Contains(search) ||
                                 a.Intrare.Contains(search) ||
                                 a.Iesire.Contains(search) ||
                                 a.Explicatii.Contains(search) ||
                                 a.Operator.Contains(search) ||
                                 a.DataOra.Contains(search) ||
                                 a.Sold.Contains(search) ||
                                 a.UltimaOp.Contains(search) ||
                                 a.Zile.Contains(search)
                         select a
                                );
                totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);   //linq.dynamic
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();
            }
        }

        public void GetExcel()
        {
            List<Agentii_Table> allCust = new List<Agentii_Table>();
            using (Agentii2Entities25 dc = new Agentii2Entities25())
            {
                allCust = dc.Agentii_Table.ToList();
            }

            WebGrid grid = new WebGrid(source: allCust, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                columns: grid.Columns(
                             grid.Column("Agentie", "Agentie"),
                             grid.Column("Intrare", "Intrare"),
                             grid.Column("Iesire", "Iesire"),
                             grid.Column("Explicatii", "Expicatii"),
                             grid.Column("Operator", "Operator"),
                             grid.Column("DataOra", "Data_Ora"),
                             grid.Column("Sold", "Sold"),
                             grid.Column("UltimaOp", "UltimaOp"),
                             grid.Column("Zile", "Zile")
                        )
                    ).ToString();


            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Agentii.xls");
            Response.ContentType = "application/vnd.ms-excel"; //excel

            Response.Write(gridData);
            Response.End();
        }

        // GET: /Agentii/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentii_Table agentii_table = db.Agentii_Table.Find(id);
            if (agentii_table == null)
            {
                return HttpNotFound();
            }
            return View(agentii_table);
        }

        // GET: /Agentii/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Agentii/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NrCrt,Agentie,Intrare,Iesire,Explicatii,Operator,DataOra,Sold,UltimaOp,Zile")] Agentii_Table agentii_table)
        {
            if (ModelState.IsValid)
            {
                db.Agentii_Table.Add(agentii_table);
                db.SaveChanges();
                return RedirectToAction("../Agentii/Index");
            }

            return View(agentii_table);
        }


        // GET: /Agentii/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentii_Table agentii_table = db.Agentii_Table.Find(id);
            if (agentii_table == null)
            {
                return HttpNotFound();
            }
            return View(agentii_table);
        }

        // POST: /Agentii/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NrCrt,Agentie,Intrare,Iesire,Explicatii,Operator,DataOra,Sold,UltimaOp,Zile")] Agentii_Table agentii_table)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentii_table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Agentii/Index");
            }
            return View(agentii_table);
        }

        // GET: /Agentii/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentii_Table agentii_table = db.Agentii_Table.Find(id);
            if (agentii_table == null)
            {
                return HttpNotFound();
            }
            return View(agentii_table);
        }

        // POST: /Agentii/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Agentii_Table agentii_table = db.Agentii_Table.Find(id);
            db.Agentii_Table.Remove(agentii_table);
            db.SaveChanges();
            return RedirectToAction("../Agentii/Index");
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
