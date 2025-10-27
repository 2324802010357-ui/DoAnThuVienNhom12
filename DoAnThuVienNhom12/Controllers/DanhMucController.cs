using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class DanhMucController : Controller
    {
        // Dữ liệu mẫu danh mục sách
        private static List<DanhMucSachModel> _danhMucSachs = new List<DanhMucSachModel>
        {
            new DanhMucSachModel { MaDanhMuc = "DM001", TenDanhMuc = "Văn học", MoTa = "Sách văn học Việt Nam và thế giới", SoLuongSach = 150, TrangThai = "Hoạt động" },
            new DanhMucSachModel { MaDanhMuc = "DM002", TenDanhMuc = "Khoa học", MoTa = "Sách khoa học tự nhiên", SoLuongSach = 89, TrangThai = "Hoạt động" },
            new DanhMucSachModel { MaDanhMuc = "DM003", TenDanhMuc = "Lịch sử", MoTa = "Sách lịch sử Việt Nam và thế giới", SoLuongSach = 67, TrangThai = "Hoạt động" },
            new DanhMucSachModel { MaDanhMuc = "DM004", TenDanhMuc = "Tin học", MoTa = "Sách công nghệ thông tin", SoLuongSach = 45, TrangThai = "Tạm ngưng" },
            new DanhMucSachModel { MaDanhMuc = "DM005", TenDanhMuc = "Ngoại ngữ", MoTa = "Sách học ngoại ngữ", SoLuongSach = 32, TrangThai = "Hoạt động" }
        };

        // Hiển thị danh sách danh mục
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý danh mục sách";
            return View(_danhMucSachs);
        }

        // Thêm danh mục sách mới
        [HttpPost]
        public ActionResult ThemDanhMucSach(DanhMucSachModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra mã danh mục đã tồn tại
                    if (_danhMucSachs.Any(dm => dm.MaDanhMuc == model.MaDanhMuc))
                    {
                        return Json(new { success = false, message = "Mã danh mục đã tồn tại!" });
                    }

                    // Kiểm tra tên danh mục đã tồn tại
                    if (_danhMucSachs.Any(dm => dm.TenDanhMuc.ToLower() == model.TenDanhMuc.ToLower()))
                    {
                        return Json(new { success = false, message = "Tên danh mục đã tồn tại!" });
                    }

                    model.SoLuongSach = 0; // Mới tạo chưa có sách
                    _danhMucSachs.Add(model);
                    return Json(new { success = true, message = "Thêm danh mục sách thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Sửa thông tin danh mục sách
        [HttpPost]
        public ActionResult SuaThongTinDanhMucSach(DanhMucSachModel model)
        {
            try
            {
                var existingDanhMuc = _danhMucSachs.FirstOrDefault(dm => dm.MaDanhMuc == model.MaDanhMuc);
                if (existingDanhMuc != null)
                {
                    // Kiểm tra tên danh mục trùng với danh mục khác
                    if (_danhMucSachs.Any(dm => dm.TenDanhMuc.ToLower() == model.TenDanhMuc.ToLower() && dm.MaDanhMuc != model.MaDanhMuc))
                    {
                        return Json(new { success = false, message = "Tên danh mục đã được sử dụng!" });
                    }

                    // Cập nhật thông tin
                    existingDanhMuc.TenDanhMuc = model.TenDanhMuc;
                    existingDanhMuc.MoTa = model.MoTa;
                    existingDanhMuc.TrangThai = model.TrangThai;

                    return Json(new { success = true, message = "Cập nhật danh mục sách thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy danh mục!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Xóa danh mục sách
        [HttpPost]
        public ActionResult XoaDanhMucSach(string maDanhMuc)
        {
            try
            {
                var danhMuc = _danhMucSachs.FirstOrDefault(dm => dm.MaDanhMuc == maDanhMuc);
                if (danhMuc != null)
                {
                    // Kiểm tra danh mục có sách hay không
                    if (danhMuc.SoLuongSach > 0)
                    {
                        return Json(new { success = false, message = "Không thể xóa danh mục đang có sách!" });
                    }

                    _danhMucSachs.Remove(danhMuc);
                    return Json(new { success = true, message = "Xóa danh mục sách thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy danh mục!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Tìm kiếm danh mục
        [HttpPost]
        public ActionResult TimKiemDanhMuc(string searchTerm, string trangThai)
        {
            var result = _danhMucSachs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = result.Where(dm => 
                    dm.MaDanhMuc.ToLower().Contains(searchTerm.ToLower()) ||
                    dm.TenDanhMuc.ToLower().Contains(searchTerm.ToLower()) ||
                    dm.MoTa.ToLower().Contains(searchTerm.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(trangThai) && trangThai != "Tất cả")
            {
                result = result.Where(dm => dm.TrangThai == trangThai);
            }

            return PartialView("_DanhMucList", result.ToList());
        }

        // Lấy thông tin danh mục để sửa
        [HttpGet]
        public ActionResult GetDanhMuc(string maDanhMuc)
        {
            var danhMuc = _danhMucSachs.FirstOrDefault(dm => dm.MaDanhMuc == maDanhMuc);
            if (danhMuc != null)
            {
                return Json(new { 
                    success = true, 
                    data = danhMuc 
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Không tìm thấy danh mục!" }, JsonRequestBehavior.AllowGet);
        }
    }

    // Model cho danh mục sách
    public class DanhMucSachModel
    {
        public string MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }
        public int SoLuongSach { get; set; }
        public string TrangThai { get; set; }
    }
}