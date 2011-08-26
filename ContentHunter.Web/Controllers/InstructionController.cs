using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContentHunter.Web.Models;

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

        //
        // GET: /Instruction/Details/5

        public ViewResult Details(int id)
        {
            Instruction instruction = db.Instructions.Find(id);
            return View(instruction);
        }

        //
        // GET: /Instruction/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Instruction/Create

        [HttpPost]
        public ActionResult Create(Instruction instruction)
        {
            if (ModelState.IsValid)
            {
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
            return View(instruction);
        }

        //
        // POST: /Instruction/Edit/5

        [HttpPost]
        public ActionResult Edit(Instruction instruction)
        {
            if (ModelState.IsValid)
            {
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
            db.Instructions.Remove(instruction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}