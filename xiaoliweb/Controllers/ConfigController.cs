using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xiaoliweb.Models;
namespace xiaoliweb.Controllers
{
    public class ConfigController : Controller
    {
        // GET: Config
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertArticleList(Article_listModel am)
        {
            int result = BLL.ArticleBLL.InsertArticleList(am);
            return Json(result);
        }
    }
}