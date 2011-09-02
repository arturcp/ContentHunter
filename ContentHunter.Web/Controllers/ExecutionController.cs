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
                       select new { Id = i.Id, Running = i.State, IsRecurrent = i.IsRecurrent, HasRun = i.FinishedAt.HasValue });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Start(int id)
        {
            if (id <= 0)
            {
                List<Instruction> instructions = (from i in db.Instructions.ToList<Instruction>()
                                                  where !i.State && (i.IsRecurrent || !i.StartedAt.HasValue)
                                                  select i).ToList<Instruction>();
                
                foreach (Instruction input in instructions)
                {
                    new Thread(Execute).Start(input);
                }
                
                return new RedirectResult("/");
            }
            else
            {
                Instruction instruction = db.Instructions.Find(id);

                if (instruction != null)
                    new Thread(Execute).Start(instruction);

                return new EmptyResult();
            }            
        }
                

        private void Execute(object data)
        {
            Instruction input = (Instruction)data;
            input.Execute();
        }
    }
}
