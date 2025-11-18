using DoAnThuVienNhom12.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        // ===============================
        // GET: Đăng nhập
        // ===============================
        public ActionResult DangNhap()
        {
            // Nếu đã đăng nhập => tự chuyển đến trang phù hợp
            if (Session["Role"] != null)
            {
                string role = Session["Role"].ToString();
                if (role == "Admin")
                    return RedirectToAction("Index", "Admin");
                else if (role == "Thủ thư")
                    return RedirectToAction("Index", "ThuThu");
                else
                    return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // ===============================
        // POST: Đăng nhập
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                ViewBag.ThongBao = "⚠️ Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!";
                return View();
            }

            // Kiểm tra tài khoản
            var user = db.NguoiDungs.FirstOrDefault(u =>
                u.TenDangNhap == tenDangNhap &&
                u.MatKhau == matKhau &&
                u.TrangThai == true);

            if (user != null)
            {
                // Lấy vai trò
                var vaiTro = db.VaiTroes.FirstOrDefault(v => v.MaVaiTro == user.MaVaiTro);
                var role = vaiTro != null ? vaiTro.TenVaiTro : "Thủ thư";

                // Ghi session
                Session["TaiKhoan"] = user.HoTen;
                Session["Role"] = role;
                Session["UserID"] = user.MaNguoiDung;
                Session["TenDangNhap"] = user.TenDangNhap;

                // Cập nhật lần đăng nhập cuối
                user.LanDangNhapCuoi = DateTime.Now;
                db.SaveChanges();

                // Chuyển hướng theo vai trò
                if (role == "Admin")
                    return RedirectToAction("Index", "Admin");
                else if (role == "Thủ thư")
                    return RedirectToAction("Index", "ThuThu");
                else if (role == "Độc giả")
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("DangNhap");
            }

            ViewBag.ThongBao = "❌ Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }

        // ===============================
        // Đăng xuất
        // ===============================
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }

        // ===============================
        // AccessDenied (Khi bị chặn quyền)
        // ===============================
        public ActionResult AccessDenied()
        {
            ViewBag.Message = "🚫 Bạn không có quyền truy cập vào trang này!";
            return View();
        }
    }
}
