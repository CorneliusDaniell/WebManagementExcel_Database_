using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebManagementExcelDatabase;
using System.Web.Security;

namespace Arta.Controllers
{
    
    public class ContController : Controller
    {

        private Agentii2Entities25 db = new Agentii2Entities25();
        /*
                // GET: Cont
                public ActionResult Index()
                {
                    return View();
                }

                public ActionResult LoginRegister()
                {
                    return View();
                }
                */
        public ActionResult UserRegistration()
        {
            ViewBag.ID_NumeFunctie = new SelectList(db.Functies, "ID_NumeFunctie", "ID_NumeFunctie");
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(Membriimetadata2 userdet)
        {

            if (ModelState.IsValid)
            {
                Membrii membri = new Membrii();
                membri.ID_Username = userdet.Username;
                membri.Nume = userdet.Nume;
                membri.Prenume = userdet.Prenume;
                membri.Email = userdet.Email;
                membri.Parola = userdet.Password;
                membri.ID_NumeFunctie = userdet.ID_NumeFunctie;

                db.Membriis.Add(membri);
                db.SaveChanges();


                if (userdet.ID_NumeFunctie == "Administrator")
                {
                    Session["ID_Username"] = membri.ID_Username;
                    FormsAuthentication.SetAuthCookie(membri.ID_Username, false);
                    return RedirectToAction("../Admin/Index");
                }
                else if (userdet.ID_NumeFunctie == "Membru")
                {
                    Session["ID_Username"] = membri.ID_Username;
                    FormsAuthentication.SetAuthCookie(membri.ID_Username, false);
                    return RedirectToAction("../User/Index");
                }
                else
                {
                    return RedirectToAction("UserRegistration");
                }
            }
            ViewBag.ID_NumeFunctie = new SelectList(db.Functies, "ID_NumeFunctie", "ID_NumeFunctie");

            return View(userdet);

        }

    }
}