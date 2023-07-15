using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaleFashion.Models;

namespace MaleFashion.Areas.PrivatePages.Controllers
{
    public class DashboardController : Controller
    {
        // GET: PrivatePages/Dashboard
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        // GET: PrivatePages/Dashbroad
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TenDangNhap, string Matkhau)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5.MD5(Matkhau);
                var taiKhoan = TenDangNhap.ToLower();
                var admin = db.TaiKhoans.Where(s => s.TenDangNhap.Equals(taiKhoan) && s.MatKhau.Equals(f_password)).ToList();
                if (admin.Count() > 0)
                {
                    //add session
                    Session["FullName"] = admin.FirstOrDefault().HoTen.ToUpper();
                    Session["AdminID"] = admin.FirstOrDefault().MaQuyen;
                    Session["Account"] = admin.ToList();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Tên người dùng và mật khẩu không chính xác";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}