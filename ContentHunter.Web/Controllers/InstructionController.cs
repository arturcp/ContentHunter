using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContentHunter.Web.Models;
using ContentHunter.Web.Models.Engines;
using WebToolkit.Converter;
using GameTools;
using System.Threading;
using System.Collections;

namespace ContentHunter.Web.Controllers
{ 
    public class InstructionController : Controller
    {
        private ContentHunterDB db = new ContentHunterDB();

        //
        // GET: /Instruction/

        public ViewResult Index()
        {
            return View(db.Instructions.ToList());
        }

        public ActionResult Details(int id)
        {
            return View(db.Instructions.Find(id));
        }

        //
        // GET: /Instruction/Create

        public ActionResult Create()
        {
            ViewBag.Engines = Crawler.GetEngines();
            return View();
        } 

        //
        // POST: /Instruction/Create

        [HttpPost]
        public ActionResult Create(Instruction instruction)
        {
            if (ModelState.IsValid)
            {
                ManageSchedule(instruction);
                db.Instructions.Add(instruction);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(instruction);
        }
        
        //
        // GET: /Instruction/Edit/5
 
        public ActionResult Edit(int id)
        {
            Instruction instruction = db.Instructions.Find(id);
            ViewBag.Engines = Crawler.GetEngines();
            return View(instruction);
        }

        //
        // POST: /Instruction/Edit/5

        [HttpPost]
        public ActionResult Edit(Instruction instruction)
        {
            if (ModelState.IsValid)
            {
                ManageSchedule(instruction);
                db.Entry(instruction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instruction);
        }

        //
        // GET: /Instruction/Delete/5
 
        public ActionResult Delete(int id)
        {
            Instruction instruction = db.Instructions.Find(id);
            return View(instruction);
        }

        //
        // POST: /Instruction/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {   
            Instruction instruction = db.Instructions.Find(id);
            instruction.Unschedule();
            db.Instructions.Remove(instruction);
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void ManageSchedule(Instruction instruction)
        {
            if (SafeConvert.ToBool(Request["schedule"]))
                instruction.Schedule();
            else
            {
                instruction.FrequencyUnit = 0;
                instruction.FrequencyValue = 0;
                instruction.ScheduledTo = null;
                instruction.Unschedule();
            }
        }
    }
}