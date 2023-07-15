using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaleFashion.Models;

namespace MaleFashion.Areas.PrivatePages.Controllers
{
    public class NewArticlesController : Controller
    {
        // GET: PrivatePages/NewArticles
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        [HttpGet]
        public ActionResult Index()
        {
            BaiViet x = new BaiViet();
            x.NgayDang = DateTime.Now;
            x.LuotXem = 0;
            x.MaTK = 1;
            ViewBag.Thumb = "blog-9.jpg";

            return View(x);
        }
        [HttpPost]
        public ActionResult Index(BaiViet x, HttpPostedFileBase imgUpload)
        {
            x.DaDuyet = false;
            x.NgayDang = DateTime.Now;
            x.MaTK = 1;
            x.LuotXem = 0;
            x.LoaiTin = "QC";
            if (imgUpload != null)
            {
                var fileName = Path.GetFileName(imgUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/img/blog/"), fileName);
                imgUpload.SaveAs(path);
                x.HinhAnh = fileName;
                ViewBag.Thumb = x.HinhAnh;
            }
            else
            {
                x.HinhAnh = "";
            }

            db.BaiViets.Add(x);
            db.SaveChanges();

            return View(x);
        }

    }
}