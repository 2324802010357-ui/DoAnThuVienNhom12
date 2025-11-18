using System;
using System.Linq;
using System.Web.Mvc;
using DoAnThuVienNhom12.Models;

namespace DoAnThuVienNhom12.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        // ======================= TRANG DANH SÁCH =======================
        public ActionResult Index()
        {
            var list = db.DanhMucs
                .Select(dm => new DanhMucSachModel
                {
                    MaDanhMuc = dm.MaDanhMuc,
                    TenDanhMuc = dm.TenDanhMuc,
                    MoTa = dm.MoTa,
                    TrangThai = dm.TrangThai,
                    SoLuongSach = dm.Saches.Count()
                })
                .OrderBy(x => x.TenDanhMuc)
                .ToList();

            return View(list);
        }

        // ======================= THÊM DANH MỤC =======================
        [HttpPost]
        public JsonResult ThemDanhMuc(string TenDanhMuc, string MoTa, string TrangThai)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TenDanhMuc))
                    return Json(new { success = false, message = "⚠️ Tên danh mục không được để trống!" });

                // Kiểm tra trùng tên
                bool existed = db.DanhMucs.Any(dm => dm.TenDanhMuc.ToLower() == TenDanhMuc.ToLower());
                if (existed)
                    return Json(new { success = false, message = "❌ Tên danh mục đã tồn tại!" });

                var newDM = new DanhMuc
                {
                    TenDanhMuc = TenDanhMuc.Trim(),
                    MoTa = MoTa?.Trim(),
                    TrangThai = string.IsNullOrEmpty(TrangThai) ? "Hoạt động" : TrangThai,
                    NgayTao = DateTime.Now
                };

                db.DanhMucs.Add(newDM);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Thêm danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // ======================= SỬA DANH MỤC =======================
        [HttpPost]
        public JsonResult SuaDanhMuc(int MaDanhMuc, string TenDanhMuc, string MoTa, string TrangThai)
        {
            try
            {
                var dm = db.DanhMucs.Find(MaDanhMuc);
                if (dm == null)
                    return Json(new { success = false, message = "❌ Không tìm thấy danh mục!" });

                // Kiểm tra trùng tên
                bool existed = db.DanhMucs.Any(x => x.TenDanhMuc.ToLower() == TenDanhMuc.ToLower() && x.MaDanhMuc != MaDanhMuc);
                if (existed)
                    return Json(new { success = false, message = "⚠️ Tên danh mục đã được sử dụng!" });

                dm.TenDanhMuc = TenDanhMuc.Trim();
                dm.MoTa = MoTa?.Trim();
                dm.TrangThai = TrangThai?.Trim();
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Cập nhật danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // ======================= XÓA DANH MỤC =======================
        [HttpPost]
        public JsonResult XoaDanhMuc(int MaDanhMuc)
        {
            try
            {
                var dm = db.DanhMucs.Find(MaDanhMuc);
                if (dm == null)
                    return Json(new { success = false, message = "❌ Không tìm thấy danh mục!" });

                bool hasBooks = db.Saches.Any(s => s.MaDanhMuc == MaDanhMuc);
                if (hasBooks)
                    return Json(new { success = false, message = "⚠️ Không thể xóa danh mục đang có sách!" });

                db.DanhMucs.Remove(dm);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Xóa danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // ======================= TÌM KIẾM DANH MỤC =======================
        [HttpPost]
        public ActionResult TimKiemDanhMuc(string searchTerm, string trangThai)
        {
            var query = db.DanhMucs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string kw = searchTerm.ToLower();
                query = query.Where(dm =>
                    dm.TenDanhMuc.ToLower().Contains(kw) ||
                    dm.MoTa.ToLower().Contains(kw));
            }

            if (!string.IsNullOrEmpty(trangThai))
                query = query.Where(dm => dm.TrangThai == trangThai);

            var result = query.Select(dm => new DanhMucSachModel
            {
                MaDanhMuc = dm.MaDanhMuc,
                TenDanhMuc = dm.TenDanhMuc,
                MoTa = dm.MoTa,
                TrangThai = dm.TrangThai,
                SoLuongSach = dm.Saches.Count()
            }).OrderBy(x => x.TenDanhMuc).ToList();

            return PartialView("_DanhMucList", result);
        }

        // ======================= LẤY CHI TIẾT DANH MỤC =======================
        [HttpGet]
        public JsonResult GetDanhMuc(int MaDanhMuc)
        {
            var dm = db.DanhMucs.Find(MaDanhMuc);
            if (dm == null)
                return Json(new { success = false, message = "❌ Không tìm thấy danh mục!" }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                success = true,
                data = new
                {
                    dm.MaDanhMuc,
                    dm.TenDanhMuc,
                    dm.MoTa,
                    dm.TrangThai
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
