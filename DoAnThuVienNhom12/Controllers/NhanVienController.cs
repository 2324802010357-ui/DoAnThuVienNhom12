using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class NhanVienController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Nhân sự";
            return View();
        }
    }
}