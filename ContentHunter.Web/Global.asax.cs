using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ContentHunter.Web.Models;
using System.Data;
using GameTools;

namespace ContentHunter.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        ContentHunterDB db = new ContentHunterDB();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Instruction", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            StopRunningInstructions();
            ScheduleInstructions();
        }

        private void StopRunningInstructions()
        {
            ContentHunterDB db = new ContentHunterDB();

            var list = (from i in db.Instructions
                        where i.State
                        select i).ToList<Instruction>();

            foreach (Instruction instruction in list)
            {
                //TODO log error!
                instruction.FinishedAt = DateTime.Now;
                instruction.State = false;
                db.Entry(instruction).State = EntityState.Modified;
            }
            if (list.Count > 0)
                db.SaveChanges();
        }

        private void ScheduleInstructions()
        {
            List<Instruction> instructions = (from i in db.Instructions
                                              where !i.State && (i.IsRecurrent || !i.StartedAt.HasValue) && i.ScheduledTo.HasValue
                                              select i).ToList<Instruction>();

            foreach (Instruction instruction in instructions)
            {
                instruction.Schedule();
            }            
        }
    }
}