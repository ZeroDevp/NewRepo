using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaleFashion.Areas.PrivatePages.Controllers
{
    public class UserController : Controller
    {
        // GET: PrivatePages/User
        public ActionResult Index()
        {
            return View();
        }
    }
}