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
            /*string url = "http://leitoracompulsiva.wordpress.com/";
            LeitoraCompulsiva lc = new LeitoraCompulsiva();
            string content = lc.ParseHtml(url);

            ViewBag.Message = content;
            return View();*/

            Input input = new Input()
            {
                Id = 1,
                Url = "http://leitoracompulsiva.wordpress.com/",
                IsRecursive = false,
                Engine = "LeitoraCompulsiva",
                Type = Input.InputType.Html                
            };

            Output output = input.Execute();
            
            ViewBag.Message = output.Content;
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
