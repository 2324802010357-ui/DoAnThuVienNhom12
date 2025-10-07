using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class MuonTraController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Mượn/Trả";
            return View();
        }
        public ActionResult PhieuMuon() => View();
        public ActionResult PhieuTra() => View();
    }
}