using DoAnThuVienNhom12.Filters;
using DoAnThuVienNhom12.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    [AuthorizeRole("Admin", "Thủ thư")]
    public class NhanVienController : Controller
    {
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        // ==========================================================
        // 📋 TRANG DANH SÁCH NHÂN VIÊN
        // ==========================================================
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý nhân sự";
            var ds = db.NhanViens.OrderBy(n => n.HoTen).ToList();
            return View(ds);
        }

        // ==========================================================
        // 🔍 TÌM KIẾM NHÂN VIÊN
        // ==========================================================
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            var result = db.NhanViens.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string term = searchTerm.ToLower();
                result = result.Where(nv =>
                    nv.HoTen.ToLower().Contains(term) ||
                    nv.ChucVu.ToLower().Contains(term) ||
                    nv.PhongBan.ToLower().Contains(term) ||
                    nv.Email.ToLower().Contains(term) ||
                    nv.SoDienThoai.ToLower().Contains(term));
            }

            return PartialView("_StaffList", result.OrderBy(n => n.HoTen).ToList());
        }

        // ==========================================================
        // ➕ THÊM NHÂN VIÊN
        // ==========================================================
        [HttpPost]
        public ActionResult Add(NhanVien model)
        {
            try
            {
                // Kiểm tra bắt buộc
                if (string.IsNullOrWhiteSpace(model.HoTen) || string.IsNullOrWhiteSpace(model.Email))
                    return Json(new { success = false, message = "⚠️ Họ tên và Email là bắt buộc!" });

                // Gán giá trị mặc định cho các cột không nhập
                model.NgaySinh = model.NgaySinh == default(DateTime) ? DateTime.Now : model.NgaySinh;
                model.GioiTinh = string.IsNullOrEmpty(model.GioiTinh) ? "Khác" : model.GioiTinh;
                model.Luong = model.Luong == 0 ? 0 : model.Luong;
                model.CMND = model.CMND ?? "";
                model.NgayVaoLam = DateTime.Now;
                model.TrangThai = string.IsNullOrEmpty(model.TrangThai) ? "Đang làm việc" : model.TrangThai;

                // Kiểm tra trùng email
                if (db.NhanViens.Any(n => n.Email == model.Email))
                    return Json(new { success = false, message = "❌ Email đã được sử dụng!" });

                db.NhanViens.Add(model);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Thêm nhân viên thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi khi thêm: " + ex.Message });
            }
        }

        // ==========================================================
        // ✏️ SỬA THÔNG TIN NHÂN VIÊN
        // ==========================================================
        [HttpPost]
        public ActionResult Edit(NhanVien model)
        {
            try
            {
                var nv = db.NhanViens.Find(model.MaNhanVien);
                if (nv == null)
                    return Json(new { success = false, message = "Không tìm thấy nhân viên!" });

                if (db.NhanViens.Any(x => x.Email == model.Email && x.MaNhanVien != model.MaNhanVien))
                    return Json(new { success = false, message = "Email đã được sử dụng bởi người khác!" });

                nv.HoTen = model.HoTen;
                nv.NgaySinh = model.NgaySinh == default(DateTime) ? DateTime.Now : model.NgaySinh;
                nv.GioiTinh = string.IsNullOrEmpty(model.GioiTinh) ? "Khác" : model.GioiTinh;
                nv.CMND = model.CMND ?? "";
                nv.DiaChi = model.DiaChi;
                nv.SoDienThoai = model.SoDienThoai;
                nv.Email = model.Email;
                nv.ChucVu = model.ChucVu;
                nv.PhongBan = model.PhongBan;
                nv.Luong = model.Luong;
                nv.TrangThai = model.TrangThai ?? "Đang làm việc";

                db.SaveChanges();
                return Json(new { success = true, message = "✅ Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi khi sửa: " + ex.Message });
            }
        }

        // ==========================================================
        // 📄 LẤY DỮ LIỆU NHÂN VIÊN THEO ID (phục vụ sửa)
        // ==========================================================
        [HttpGet]
        public JsonResult GetEmployee(int maNV)
        {
            var nv = db.NhanViens.Find(maNV);
            if (nv == null)
                return Json(new { success = false, message = "Không tìm thấy nhân viên!" }, JsonRequestBehavior.AllowGet);

            var data = new
            {
                nv.MaNhanVien,
                nv.HoTen,
                nv.Email,
                nv.ChucVu,
                nv.PhongBan,
                nv.SoDienThoai,
                nv.TrangThai,
                nv.DiaChi
            };
            return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
        }

        // ==========================================================
        // ❌ XÓA NHÂN VIÊN
        // ==========================================================
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var nv = db.NhanViens.Find(id);
                if (nv == null)
                    return Json(new { success = false, message = "Không tìm thấy nhân viên!" });

                db.NhanViens.Remove(nv);
                db.SaveChanges();

                return Json(new { success = true, message = "✅ Xóa nhân viên thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Lỗi khi xóa: " + ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
