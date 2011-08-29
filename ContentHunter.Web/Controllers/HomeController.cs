using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContentHunter.Web.Models;
using ContentHunter.Web.Models.Engines;

namespace ContentHunter.Web.Controllers
{
    public class HomeController : Controller
    {
        ContentHunterDB db = new ContentHunterDB();

        public ActionResult Index()
        {
            //ViewBag.Message = "Welcome to ASP.NET MVC!";
            /*string url = "http://leitoracompulsiva.wordpress.com/";
            LeitoraCompulsiva lc = new LeitoraCompulsiva();
            string content = lc.ParseHtml(url);

            ViewBag.Message = content;
            return View();*/

            /*Instruction input = new Instruction()
            {
                Id = 1,
                Url = "http://leitoracompulsiva.wordpress.com/",
                IsRecursive = false,
                Engine = "LeitoraCompulsiva",
                Type = Instruction.GetType(Instruction.InputType.Html),
                IsRecurrent = true,
                Category = "Literatura"
            };*/

            //Instruction input = db.Instructions.First<Instruction>();

            /*ContentHunterDB db = new ContentHunterDB();
            db.Instructions.Add(input);
            db.SaveChanges();*/

            //List<CrawlerResult> outputs = input.Execute();

           /* if (ModelState.IsValid)
            {
                db.CrawlerResults.Add(output);
                db.SaveChanges();
            }*/
            
            //ViewBag.Message = output.Content;
            ViewBag.Engines = Crawler.GetEngines();

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            List<Instruction> instructions = db.Instructions.ToList<Instruction>();

            foreach (Instruction input in instructions)
            {
                input.Execute();
            }

            ViewBag.Message = "Instructions executed";
            return View("Create");
        }
    }
}
