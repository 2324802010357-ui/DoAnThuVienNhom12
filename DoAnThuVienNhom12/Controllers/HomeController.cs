using DoAnThuVienNhom12.Filters;
using DoAnThuVienNhom12.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    // 🔹 Trang chủ dành cho Admin và Thủ thư
    [AuthorizeRole("Admin", "Thủ thư")]
    public class HomeController : Controller
    {
        // Kết nối database
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        // ===================== TRANG CHỦ =====================
        public ActionResult Index()
        {
            try
            {
                // Thống kê tổng hợp
                var tongSach = db.Saches.Count();
                var tongDocGia = db.DocGias.Count();
                var dangMuon = db.PhieuMuons.Count(pm => pm.TrangThai == "Đang mượn");
                var quaHan = db.PhieuMuons.Count(pm => pm.TrangThai == "Đang mượn" && pm.NgayHenTra < DateTime.Now);

                // Truyền dữ liệu sang View
                ViewBag.TongSach = tongSach;
                ViewBag.TongDocGia = tongDocGia;
                ViewBag.DangMuon = dangMuon;
                ViewBag.QuaHan = quaHan;

                // Lấy danh sách sách phổ biến (Top 6)
                var sachPhoBien = db.Saches
                    .OrderByDescending(s => s.LuotMuon)
                    .ThenByDescending(s => s.LuotXem)
                    .Take(6)
                    .Select(s => new
                    {
                        s.MaSach,
                        s.TenSach,
                        s.AnhBia,
                        s.LuotMuon,
                        s.LuotXem,
                        DanhMuc = s.DanhMuc.TenDanhMuc,
                        TacGia = s.TacGia.TenTacGia,
                        s.TinhTrang
                    })
                    .ToList();

                ViewBag.SachPhoBien = sachPhoBien;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi tải dữ liệu: " + ex.Message;
                return View();
            }
        }

        // ===================== GIỚI THIỆU =====================
        public ActionResult About()
        {
            ViewBag.Title = "Giới thiệu hệ thống thư viện";
            ViewBag.Message = "Hệ thống quản lý thư viện trực tuyến - Dự án nhóm 12 (TDMU).";
            return View();
        }

        // ===================== LIÊN HỆ =====================
        public ActionResult Contact()
        {
            ViewBag.Title = "Liên hệ";
            ViewBag.Message = "Thông tin liên hệ:  Email: support@library.com | Hotline: 0901 234 567";
            return View();
        }

        // ===================== GIẢI PHÓNG NGUỒN =====================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
