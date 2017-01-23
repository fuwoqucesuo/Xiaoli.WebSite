using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xiaoliweb.BLL;
namespace xiaoliweb.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FetchHomeData()
        {
            var dic = new Dictionary<string, object>();
            var listTag = HomeBLL.FetchTags();
            var dataInfo = new
            {
                tags = listTag
            };
            dic.Add("data", dataInfo);
            return Json(dic,JsonRequestBehavior.AllowGet);
        }

    }
}