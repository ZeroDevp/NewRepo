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
    public class AuthorsController : Controller
    {
        // GET: PrivatePages/Authors
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;
            var author = db.NhaCungCaps.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                author = author.Where(p => p.TenNCC.Contains(search));
            }

            var totalItems = author.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = author.OrderBy(p => p.TenNCC).ToPagedList(pageNumber, pageSize);

            ViewBag.search = search;
            ViewBag.pageNumber = pageNumber;
            ViewBag.totalPages = totalPages;

            return View(items);
        }



        public ActionResult Details(int id)
        {
            NhaCungCap author = db.NhaCungCaps.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhaCungCap author)
        {
            if (ModelState.IsValid)
            {

                db.NhaCungCaps.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            NhaCungCap author = db.NhaCungCaps.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NhaCungCap author)
        {
            if (ModelState.IsValid)
            {

                // Cập nhật đối tượng Sach trong cơ sở dữ liệu
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();

                // Chuyển hướng người dùng đến trang Index
                return RedirectToAction("Index");
            }

            return View(author);
        }


        // GET: Sach/Delete/5
        public ActionResult Delete(int id)
        {
            NhaCungCap author = db.NhaCungCaps.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Sach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhaCungCap author = db.NhaCungCaps.Find(id);
            db.NhaCungCaps.Remove(author);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}