using DoAnThuVienNhom12.Filters;
using DoAnThuVienNhom12.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    [AuthorizeRole("Admin", "Thủ thư")]
    public class KhoController : Controller
    {
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        // ===================== DANH SÁCH KHO =====================
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý kho sách";

            var khoList = db.Khoes
                .Include(k => k.KeSaches)
                .ToList();

            return View(khoList);
        }

        // ===================== THÊM KHO =====================
        [HttpPost]
        public ActionResult ThemKho(string TenKho, string DiaChi, string MoTa, int SucChua)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TenKho))
                    return Json(new { success = false, message = "❌ Tên kho không được để trống!" });

                if (SucChua <= 0)
                    return Json(new { success = false, message = "❌ Sức chứa phải lớn hơn 0!" });

                bool existed = db.Khoes.Any(k => k.TenKho.ToLower() == TenKho.ToLower());
                if (existed)
                    return Json(new { success = false, message = "⚠️ Kho này đã tồn tại!" });

                var newKho = new Kho
                {
                    TenKho = TenKho.Trim(),
                    DiaChi = string.IsNullOrWhiteSpace(DiaChi) ? "Chưa có" : DiaChi.Trim(),
                    MoTa = string.IsNullOrWhiteSpace(MoTa) ? "Không có mô tả" : MoTa.Trim(),
                    SucChua = SucChua,
                    SoLuongHienTai = 0,
                    TrangThai = "Hoạt động"
                };

                db.Khoes.Add(newKho);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Thêm kho mới thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi khi thêm kho: " + ex.Message });
            }
        }

        // ===================== SỬA THÔNG TIN KHO =====================
        [HttpPost]
        public ActionResult SuaKho(int MaKho, string TenKho, string DiaChi, string MoTa, int SucChua)
        {
            try
            {
                var kho = db.Khoes.Find(MaKho);
                if (kho == null)
                    return Json(new { success = false, message = "❌ Không tìm thấy kho cần sửa!" });

                if (string.IsNullOrWhiteSpace(TenKho))
                    return Json(new { success = false, message = "⚠️ Tên kho không được để trống!" });

                bool existed = db.Khoes.Any(k => k.TenKho.ToLower() == TenKho.ToLower() && k.MaKho != MaKho);
                if (existed)
                    return Json(new { success = false, message = "⚠️ Tên kho này đã tồn tại!" });

                kho.TenKho = TenKho.Trim();
                kho.DiaChi = string.IsNullOrWhiteSpace(DiaChi) ? "Chưa có" : DiaChi.Trim();
                kho.MoTa = string.IsNullOrWhiteSpace(MoTa) ? "Không có mô tả" : MoTa.Trim();
                kho.SucChua = SucChua;

                db.SaveChanges();

                return Json(new { success = true, message = "✅ Cập nhật thông tin kho thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi khi sửa kho: " + ex.Message });
            }
        }

        // ===================== XÓA KHO =====================
        [HttpPost]
        public ActionResult XoaKho(int MaKho)
        {
            try
            {
                var kho = db.Khoes.Find(MaKho);
                if (kho == null)
                    return Json(new { success = false, message = "❌ Không tìm thấy kho cần xóa!" });

                bool hasShelves = db.KeSaches.Any(k => k.MaKho == MaKho);
                if (hasShelves)
                    return Json(new { success = false, message = "⚠️ Không thể xóa kho đang có kệ sách!" });

                db.Khoes.Remove(kho);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Xóa kho thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi khi xóa kho: " + ex.Message });
            }
        }

        // ===================== CHI TIẾT KHO =====================
        public ActionResult ChiTiet(int id)
        {
            try
            {
                var kho = db.Khoes
                    .Include(k => k.KeSaches)
                    .FirstOrDefault(k => k.MaKho == id);

                if (kho == null)
                {
                    ViewBag.Error = "Không tìm thấy kho.";
                    return View();
                }

                ViewBag.KeTrongKho = kho.KeSaches.ToList();
                return View(kho);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View();
            }
        }

        // ===================== PHIẾU NHẬP & LỊCH SỬ =====================
        public ActionResult PhieuNhap()
        {
            ViewBag.Title = "Phiếu nhập kho";
            var phieuNhapList = db.PhieuNhapKhoes
                .Include(p => p.NhanVien)
                .OrderByDescending(p => p.NgayNhap)
                .ToList();
            return View(phieuNhapList);
        }

        public ActionResult LichSu()
        {
            ViewBag.Title = "Lịch sử nhập kho";
            var lichSu = db.PhieuNhapKhoes
                .Include(p => p.NhanVien)
                .OrderByDescending(p => p.NgayNhap)
                .ToList();

            return View(lichSu);
        }

        // ===================== GIẢI PHÓNG NGUỒN =====================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
