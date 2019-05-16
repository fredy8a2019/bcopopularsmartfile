using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["idETAPA"] = 1;
            return View();
        }
    }
}
