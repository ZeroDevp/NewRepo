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
    public class TheloaiController : Controller
    {
        // GET: PrivatePages/Categories
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;
            var cate = db.TheLoais.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                cate = cate.Where(p => p.TenTL.Contains(search));
            }

            var totalItems = cate.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = cate.OrderBy(p => p.TenTL).ToPagedList(pageNumber, pageSize);

            ViewBag.search = search;
            ViewBag.pageNumber = pageNumber;
            ViewBag.totalPages = totalPages;

            return View(items);
        }





        public ActionResult Details(int id)
        {
            TheLoai cate = db.TheLoais.Find(id);
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TheLoai TL)
        {
            if (ModelState.IsValid)
            {

                db.TheLoais.Add(TL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(TL);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            TheLoai cate = db.TheLoais.Find(id);
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TheLoai cate)
        {
            if (ModelState.IsValid)
            {

                // Cập nhật đối tượng Sach trong cơ sở dữ liệu
                db.Entry(cate).State = EntityState.Modified;
                db.SaveChanges();

                // Chuyển hướng người dùng đến trang Index
                return RedirectToAction("Index");
            }

            return View(cate);
        }


        public ActionResult Delete(int id)
        {
            TheLoai cate = db.TheLoais.Find(id);
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TheLoai cate = db.TheLoais.Find(id);
            db.TheLoais.Remove(cate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}