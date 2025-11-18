using DoAnThuVienNhom12.Filters;
using DoAnThuVienNhom12.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    [AuthorizeRole("Admin", "Thủ thư")]
    public class KeSachController : Controller
    {
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        // ===================== DANH SÁCH KỆ SÁCH =====================
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý kệ sách";
            ViewBag.KhoList = db.Khoes.ToList();

            // ✅ Cập nhật lại số lượng sách hiện tại cho từng kệ
            var keSachs = db.KeSaches.Include(k => k.Kho).ToList();
            foreach (var ke in keSachs)
            {
                ke.SoLuongHienTai = db.Saches.Count(s => s.MaKe == ke.MaKe);
            }

            // ✅ Không lưu DB để tránh ghi đè vô ý, chỉ dùng để hiển thị
            return View(keSachs);
        }

        // ===================== THÊM KỆ SÁCH =====================
        [HttpPost]
        public ActionResult ThemKe(string TenKe, int MaKho, string ViTri, int SucChua)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TenKe))
                    return Json(new { success = false, message = "Tên kệ không được để trống!" });

                bool existed = db.KeSaches.Any(k => k.TenKe.ToLower() == TenKe.ToLower() && k.MaKho == MaKho);
                if (existed)
                    return Json(new { success = false, message = "Kệ này đã tồn tại trong kho!" });

                var newKe = new KeSach
                {
                    TenKe = TenKe.Trim(),
                    MaKho = MaKho,
                    ViTri = ViTri?.Trim(),
                    SucChua = SucChua,
                    SoLuongHienTai = 0
                };

                db.KeSaches.Add(newKe);
                db.SaveChanges();
                return Json(new { success = true, message = "✅ Thêm kệ sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi: " + ex.Message });
            }
        }

        // ===================== SỬA THÔNG TIN KỆ =====================
        [HttpPost]
        public ActionResult SuaKe(int MaKe, string TenKe, int MaKho, string ViTri, int SucChua)
        {
            try
            {
                var ke = db.KeSaches.Find(MaKe);
                if (ke == null)
                    return Json(new { success = false, message = "Không tìm thấy kệ!" });

                bool existed = db.KeSaches.Any(k => k.TenKe.ToLower() == TenKe.ToLower() && k.MaKho == MaKho && k.MaKe != MaKe);
                if (existed)
                    return Json(new { success = false, message = "Tên kệ đã tồn tại trong kho này!" });

                ke.TenKe = TenKe.Trim();
                ke.MaKho = MaKho;
                ke.ViTri = ViTri?.Trim();
                ke.SucChua = SucChua;

                db.SaveChanges();
                return Json(new { success = true, message = "✅ Cập nhật thông tin kệ thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi: " + ex.Message });
            }
        }

        // ===================== XÓA KỆ SÁCH =====================
        [HttpPost]
        public ActionResult XoaKe(int MaKe)
        {
            try
            {
                var ke = db.KeSaches.Find(MaKe);
                if (ke == null)
                    return Json(new { success = false, message = "Không tìm thấy kệ!" });

                bool hasBooks = db.Saches.Any(s => s.MaKe == MaKe);
                if (hasBooks)
                    return Json(new { success = false, message = "Không thể xóa kệ đang chứa sách!" });

                db.KeSaches.Remove(ke);
                db.SaveChanges();
                return Json(new { success = true, message = "✅ Xóa kệ sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi: " + ex.Message });
            }
        }

        // ===================== CHI TIẾT KỆ SÁCH =====================
        public ActionResult Detail(int id)
        {
            try
            {
                // ✅ Tắt Lazy Loading & Proxy để tránh hiển thị DynamicProxies
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                var ke = db.KeSaches
                    .Include(k => k.Kho)
                    .FirstOrDefault(k => k.MaKe == id);

                if (ke == null)
                {
                    ViewBag.Error = "Không tìm thấy kệ sách.";
                    return View();
                }

                // ✅ Lấy danh sách sách có Include đầy đủ
                var sachList = db.Saches
                    .Include(s => s.DanhMuc)
                    .Include(s => s.TacGia)
                    .Include(s => s.NhaXuatBan)
                    .Where(s => s.MaKe == id)
                    .ToList();

                // ✅ Cập nhật số lượng thật
                ke.SoLuongHienTai = sachList.Count;

                ViewBag.SachTrongKe = sachList;
                return View(ke);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View();
            }
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
