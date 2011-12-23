using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ContentHunter.Web.Models;

namespace ContentHunter.Web.Controllers
{
    public class APIController : Controller
    {
        private ContentHunterDB db = new ContentHunterDB();

        public JsonResult Index(string tag)
        {
            List<CrawlerResult> contents = db.CrawlerResults.Where<CrawlerResult>(x => x.Tags.Contains(tag)).ToList<CrawlerResult>();
            return Json(contents, JsonRequestBehavior.AllowGet);
        }

    }
}
