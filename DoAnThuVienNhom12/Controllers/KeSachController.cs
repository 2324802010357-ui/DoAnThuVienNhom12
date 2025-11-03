using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class KeSachController : Controller
    {
        // GET: KeSach
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý kệ sách";
            return View();
        }

        // GET: KeSach/Detail/KE_A
        public ActionResult Detail(string id)
        {
            ViewBag.Title = "Chi tiết kệ sách";
            ViewBag.ShelfCode = id;
            return View();
        }
    }
}
