using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContentHunter.Web.Models;

namespace ContentHunter.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Welcome to ASP.NET MVC!";
            string url = "http://leitoracompulsiva.wordpress.com/";
            LeitoraCompulsiva lc = new LeitoraCompulsiva();
            string content = lc.Parse(url);

            ViewBag.Message = content;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
