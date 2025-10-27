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
            ViewBag.Title = "Qu?n l? k? s?ch";
            return View();
        }

        // GET: KeSach/Detail/KE_A
        public ActionResult Detail(string id)
        {
            ViewBag.Title = "Chi ti?t k? s?ch";
            ViewBag.ShelfCode = id;
            return View();
        }
    }
}
