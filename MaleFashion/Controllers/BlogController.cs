using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaleFashion.Models;

namespace MaleFashion.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        public ActionResult Index()
        {
            var Articles = db.BaiViets.Where(x => x.DaDuyet == true).OrderByDescending(x => x.NgayDang).ToList();
            return View(Articles);
        }


        public ActionResult BlogDetails(int id)
        {
            BaiViet post = db.BaiViets.Where(x => x.MaBV.Equals(id)).First();
            return View(post);
        }
    }
}