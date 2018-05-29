using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security; // authentification

namespace WebManagementExcelDatabase.Controllers
{
    public class AccountController : Controller
    {

        Agentii2Entities25 db = new Agentii2Entities25();
        
        public ActionResult Login()
        {
            return View();
            ViewBag.Message = " ";
        }

        [HttpPost]
        public ActionResult Login(Membrii memb)
        {
            var result = db.Membriis.Where(a => a.ID_Username == memb.ID_Username && a.Parola == memb.Parola).ToList();
            if (result.Count() > 0)
            {
                Session["ID_Username"] = result[0].ID_Username;
                FormsAuthentication.SetAuthCookie(result[0].ID_Username, false);
                if(result[0].ID_NumeFunctie == "Administrator")
                {
                    return RedirectToAction("../Admin/Index");
                }
                else if (result[0].ID_NumeFunctie == "Membru")
                {
                    return RedirectToAction("../User/Index");
                }

            }
            else
            {
                ViewBag.Message = "User sau parola incorecta";
            } 
    

            return View(memb);
        }

        public ActionResult Logout()
        {
            Session["ID_Username"] = 0;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

	}
}