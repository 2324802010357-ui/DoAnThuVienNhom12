using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class SachController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Sách";
            return View();
        }

        public ActionResult Search(string q)
        {
            ViewBag.Title = "Tìm kiếm";
            ViewBag.Query = q;
            return View();
        }

        public ActionResult Detail(int id = 0)
        {
            ViewBag.Title = "Chi tiết sách";
            ViewBag.Id = id;
            return View();
        }
    }
}