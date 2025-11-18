using System;
using System.Linq;
using System.Web.Mvc;
using DoAnThuVienNhom12.Models;
using DoAnThuVienNhom12.Filters;

namespace DoAnThuVienNhom12.Controllers
{
    [AuthorizeRole("Admin")]
    public class AdminController : Controller
    {
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();


        // =============================
        // HÀM CẬP NHẬT TRẠNG THÁI AUTO
        // =============================
        private void CapNhatTrangThaiPhieuMuon()
        {
            var today = DateTime.Now.Date;
            var list = db.PhieuMuons.ToList();

            foreach (var p in list)
            {
                // Nếu đã trả thì không cập nhật nữa
                if (p.TrangThai == "Đã trả")
                    continue;

                // Nếu chưa trả thì xét hạn
                if (p.NgayHenTra < today)
                    p.TrangThai = "Quá hạn";
                else
                    p.TrangThai = "Đang mượn";
            }

            db.SaveChanges();
        }


        // =============================
        // DASHBOARD ADMIN
        // =============================
        public ActionResult Index()
        {
            // Cập nhật trạng thái trước khi thống kê
            CapNhatTrangThaiPhieuMuon();

            ViewBag.TongSach = db.Saches.Count();
            ViewBag.TongDocGia = db.DocGias.Count();
            ViewBag.TongKho = db.Khoes.Count();
            ViewBag.TongKe = db.KeSaches.Count();

            // Đang mượn
            ViewBag.DangMuon = db.PhieuMuons
                .Count(pm => pm.TrangThai == "Đang mượn");

            // Quá hạn
            ViewBag.QuaHan = db.PhieuMuons
                .Count(pm => pm.TrangThai == "Quá hạn");

            return View();
        }

        // ===========================================
        // Khu vực Admin – có thể mở rộng thêm module
        // ===========================================
    }
}
