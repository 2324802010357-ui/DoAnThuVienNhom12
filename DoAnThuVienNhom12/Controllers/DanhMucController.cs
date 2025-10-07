using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class DanhMucController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Danh mục";
            return View();
        }
    }
}