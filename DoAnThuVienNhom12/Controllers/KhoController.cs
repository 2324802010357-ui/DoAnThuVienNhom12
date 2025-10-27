using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class KhoController : Controller
    {
        // GET: Kho
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý kho sách";
            return View();
        }

        // GET: Kho/PhieuNhap
        public ActionResult PhieuNhap()
        {
            ViewBag.Title = "Phiếu nhập kho";
            return View();
        }

        // GET: Kho/LichSu
        public ActionResult LichSu()
        {
            ViewBag.Title = "Lịch sử nhập kho";
            return View();
        }
    }
}
