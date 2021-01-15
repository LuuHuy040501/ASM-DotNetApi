using PagedList;
using ReadNewsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadNewsWebApp.Controllers
{
    public class ArticleController : Controller
    {
        private CrawlerDotNetMVCEntities _db;
        public ArticleController()
        {
            _db = new CrawlerDotNetMVCEntities();
        }
        // GET: Article
        public ActionResult Index(int? page)
        {
            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var list = from a in _db.Articles orderby a.CreateAt descending select a;

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Read(int id)
        {
            
            var article = from a in _db.Articles where a.Id == id select a;
            if(article.Count() == 0)
            {
                return RedirectToAction("Index");
            }
            //get random articles
            Random rand = new Random();

            var links = from a in _db.Articles where a.Id != id orderby a.Id descending select a;

            var toSkip = rand.Next(0, links.Count());
            var topFour = links.Skip(toSkip).Take(4);
            ViewBag.realtedArticle = topFour;
       
            return View("Article",article.FirstOrDefault());

        }
    }
}