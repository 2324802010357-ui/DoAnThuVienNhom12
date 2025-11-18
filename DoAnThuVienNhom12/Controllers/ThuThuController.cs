using System.Linq;
using System.Web.Mvc;
using DoAnThuVienNhom12.Models;

namespace DoAnThuVienNhom12.Controllers
{
    public class ThuThuController : Controller
    {
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        [HttpGet]
        public ActionResult Index()
        {
            // Nếu chưa đăng nhập → về Login
            if (Session["TaiKhoan"] == null)
                return RedirectToAction("DangNhap", "NguoiDung");

            // Chỉ cho phép role = Thủ thư
            if ((string)Session["Role"] != "Thủ thư")
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}
