using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaleFashion.Models;

namespace MaleFashion.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        QLTHOITRANGContext db = new QLTHOITRANGContext();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            LienHe contact = new LienHe();
            contact.EMAIL = fc["Email"];
            contact.HoTen = fc["HoTen"];
            contact.NgayGui = DateTime.Now;
            contact.NoiDung = fc["NoiDung"];
            db.LienHes.Add(contact);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
    }
}