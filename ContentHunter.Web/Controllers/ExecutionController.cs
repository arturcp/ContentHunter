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

        public JsonResult Status()
        {
            var list = (from i in db.Instructions
                       where i.State
                       select i.Id).ToList<int>();

            //bool isFinished = list.Count == 0;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Start(int id)
        {
            if (id <= 0)
            {
                List<Instruction> instructions = db.Instructions.ToList<Instruction>();

                foreach (Instruction input in instructions)
                {
                    new Thread(Execute).Start(input);
                }
            }
            else
            {
                Instruction instruction = db.Instructions.Find(id);

                if (instruction != null)
                    new Thread(Execute).Start(instruction);
            }

            //ViewBag.Message = "Instructions are executing";
            //return new RedirectResult("/Instruction");
            return new EmptyResult();
        }
                

        private void Execute(object data)
        {
            Instruction input = (Instruction)data;
            input.Execute();
        }
    }
}
