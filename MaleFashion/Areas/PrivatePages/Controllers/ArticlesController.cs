using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaleFashion.Models;
using PagedList;

namespace MaleFashion.Areas.PrivatePages.Controllers
{
    public class ArticlesController : Controller
    {
        // GET: PrivatePages/Articles
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        private bool daDuyet;
        public ActionResult Index(int? page, string searchString, string IsActive)
        {
            daDuyet = IsActive != null && IsActive.Equals("1");
            dlGiaoDien(page, searchString);
            return View();
        }
        [HttpPost]
        public ActionResult Detele(int id, int? page, string searchString)
        {
            BaiViet post = db.BaiViets.Find(id);
            db.BaiViets.Remove(post);
            db.SaveChanges();
            return View("Index");
        }



        [HttpPost]
        public ActionResult Active(int id, int? page, string searchString)
        {
            BaiViet post = db.BaiViets.Find(id);
            post.DaDuyet = !daDuyet;
            db.SaveChanges();
            dlGiaoDien(page, searchString);
            return View("Index");
        }
        public ActionResult Edit(int id)
        {
            BaiViet post = db.BaiViets.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost]
        public ActionResult Edit(BaiViet post)
        {
            if (ModelState.IsValid)
            {
                post.MaTK = 1;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }


        private void dlGiaoDien(int? page, string searchString)
        {
            int pageSize = 10; // Số lượng bài viết hiển thị ở mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại
            var baiViets = db.BaiViets.Where(x => x.DaDuyet == daDuyet);
            if (!string.IsNullOrEmpty(searchString))
            {
                baiViets = baiViets.Where(s => s.TenBV.ToLower().Contains(searchString.ToLower()) || s.NoiDung.ToLower().Contains(searchString.ToLower()));
            }
            ViewData["ListPosts"] = baiViets.OrderByDescending(p => p.NgayDang).ToPagedList(pageNumber, pageSize);
            ViewBag.tdCuaNut = daDuyet ? "Cấm hiển thị" : "Kiểm duyệt";
        }

    }
}