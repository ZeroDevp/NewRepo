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
    public class ContactManegementController : Controller
    {
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        // GET: PrivatePages/ContactManegement
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;
            var contact = db.LienHes.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                contact = contact.Where(p => p.EMAIL.Contains(search));
            }

            var totalItems = contact.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = contact.OrderBy(p => p.EMAIL).ToPagedList(pageNumber, pageSize);

            ViewBag.search = search;
            ViewBag.pageNumber = pageNumber;
            ViewBag.totalPages = totalPages;

            return View(items);
        }
        public ActionResult Details(int id)
        {
            LienHe contact = db.LienHes.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            LienHe contact = db.LienHes.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LienHe contact)
        {
            if (ModelState.IsValid)
            {

                // Cập nhật đối tượng Sach trong cơ sở dữ liệu
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();

                // Chuyển hướng người dùng đến trang Index
                return RedirectToAction("Index");
            }

            return View(contact);
        }



        public ActionResult Delete(int id)
        {
            LienHe contact = db.LienHes.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LienHe contact = db.LienHes.Find(id);
            db.LienHes.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}