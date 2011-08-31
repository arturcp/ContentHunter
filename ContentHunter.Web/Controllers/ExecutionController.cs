using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContentHunter.Web.Models;
using System.Threading;

namespace ContentHunter.Web.Controllers
{
    public class ExecutionController : Controller
    {
        ContentHunterDB db = new ContentHunterDB();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Show()
        {
            var list = (from i in db.Instructions
                       where i.State
                       select i).ToList<Instruction>();

            bool isFinished = list.Count == 0;
            return Json(isFinished, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {

            List<Instruction> instructions = db.Instructions.ToList<Instruction>();

            foreach (Instruction input in instructions)
            {
                new Thread(Execute).Start(input);
            }

            ViewBag.Message = "Instructions are executing";
            return View("Index");
        }

        private void Execute(object data)
        {
            Instruction input = (Instruction)data;
            input.Execute();
        }
    }
}
