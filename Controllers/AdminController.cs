using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebManagementExcelDatabase.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }
    }
}