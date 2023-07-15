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
    public class ProductController : Controller
    {
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        // GET: PrivatePages/Product
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;
            var products = db.SanPhams.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.TenSP.Contains(search));
            }

            var totalItems = products.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Thay vì lấy danh sách sản phẩm của trang hiện tại với phương thức ToList(), bạn sẽ sử dụng phương thức ToPagedList() để lấy danh sách sản phẩm được phân trang.
            var items = products.OrderBy(p => p.TenSP).ToPagedList(pageNumber, pageSize);

            ViewBag.search = search;
            ViewBag.pageNumber = pageNumber;
            ViewBag.totalPages = totalPages;

            return View(items);
        }




        public ActionResult Details(string id)
        {
            SanPham sp = db.SanPhams.Find(id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Cate = new SelectList(db.TheLoais, "MaTL", "TenTL", null);
            ViewBag.NhaCungCap = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(SanPham sp, HttpPostedFileBase file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Nếu người dùng đã chọn một tệp ảnh
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            // Lưu tên tệp ảnh
        //            var fileName = Path.GetFileName(file.FileName);

        //            // Lưu đường dẫn tệp ảnh
        //            var path = Path.Combine(Server.MapPath("~/img/product/"), fileName);

        //            // Lưu tệp ảnh vào hệ thống
        //            file.SaveAs(path);

        //            // Gán đường dẫn tệp ảnh vào đối tượng Sach
        //            sp.HinhSP = fileName;
        //        }
        //        sp.NgayCapNhat = DateTime.Now;
        //        db.SanPhams.Add(sp);

        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(sp);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sp, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Nếu người dùng đã chọn một tệp ảnh
                    if (file != null && file.ContentLength > 0)
                    {
                        // Lưu tên tệp ảnh
                        var fileName = Path.GetFileName(file.FileName);

                        // Lưu đường dẫn tệp ảnh
                        var path = Path.Combine(Server.MapPath("~/img/product/"), fileName);

                        // Lưu tệp ảnh vào hệ thống
                        file.SaveAs(path);

                        // Gán đường dẫn tệp ảnh vào đối tượng SanPham
                        sp.HinhSP = fileName;
                    }
                    sp.NgayCapNhat = DateTime.Now;
                    db.SanPhams.Add(sp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    // Xử lý hoặc ghi log lỗi tại đây
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu.");
                }
            }

            return View(sp);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            SanPham sp = db.SanPhams.Find(id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cate = new SelectList(db.TheLoais, "MaTL", "TenTL");
            ViewBag.NhaCungCap = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");

            return View(sp);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sp, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // Nếu người dùng đã chọn một tệp ảnh để cập nhật
                if (file != null && file.ContentLength > 0)
                {
                    // Xóa tệp ảnh cũ
                    if (sp.HinhSP != null)
                    {
                        var fullPath = Path.Combine(Server.MapPath("~/img/product"), sp.HinhSP);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    // Lưu tên tệp ảnh mới
                    var fileName = Path.GetFileName(file.FileName);

                    // Lưu đường dẫn tệp ảnh mới
                    var path = Path.Combine(Server.MapPath("~/img/product/"), fileName);

                    // Lưu tệp ảnh mới vào hệ thống
                    file.SaveAs(path);

                    // Gán đường dẫn tệp ảnh mới vào đối tượng Sach
                    sp.HinhSP = fileName;
                }

                sp.NgayCapNhat = DateTime.Now;
                // Cập nhật đối tượng Sach trong cơ sở dữ liệu
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();

                // Chuyển hướng người dùng đến trang Index
                return RedirectToAction("Index");
            }

            return View(sp);
        }




        public ActionResult Delete(string id)
        {
            SanPham sp = db.SanPhams.Find(id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sp = db.SanPhams.Find(id);
            db.SanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
