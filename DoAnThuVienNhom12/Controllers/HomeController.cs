using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Trang chủ";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "Giới thiệu";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Liên hệ";
            return View();
        }
    }
}