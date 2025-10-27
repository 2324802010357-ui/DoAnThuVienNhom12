using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class NhanVienController : Controller
    {
        // Dữ liệu mẫu nhân viên
        private static List<NhanVienModel> _nhanViens = new List<NhanVienModel>
        {
            new NhanVienModel { MaNV = "NV001", HoTen = "Nguyễn Văn Thư", ChucVu = "Thủ thư trưởng", PhongBan = "Phòng Quản lý", SoDienThoai = "0123456789", Email = "thu@library.com", TrangThai = "Đang làm việc", DiaChi = "123 Đường ABC, Quận 1, TP.HCM" },
            new NhanVienModel { MaNV = "NV002", HoTen = "Trần Thị Hoa", ChucVu = "Thủ thư", PhongBan = "Phòng Mượn trả", SoDienThoai = "0987654321", Email = "hoa@library.com", TrangThai = "Đang làm việc", DiaChi = "456 Đường DEF, Quận 2, TP.HCM" },
            new NhanVienModel { MaNV = "NV003", HoTen = "Lê Văn Minh", ChucVu = "Nhân viên IT", PhongBan = "Phòng Kỹ thuật", SoDienThoai = "0345678912", Email = "minh@library.com", TrangThai = "Nghỉ phép", DiaChi = "789 Đường GHI, Quận 3, TP.HCM" },
            new NhanVienModel { MaNV = "NV004", HoTen = "Phạm Thị Lan", ChucVu = "Thủ thư", PhongBan = "Phòng Tài liệu", SoDienThoai = "0567891234", Email = "lan@library.com", TrangThai = "Đang làm việc", DiaChi = "321 Đường JKL, Quận 4, TP.HCM" },
            new NhanVienModel { MaNV = "NV005", HoTen = "Hoàng Văn Nam", ChucVu = "Thủ thư", PhongBan = "Phòng Mượn trả", SoDienThoai = "0789123456", Email = "nam@library.com", TrangThai = "Đang làm việc", DiaChi = "654 Đường MNO, Quận 5, TP.HCM" },
            new NhanVienModel { MaNV = "NV006", HoTen = "Võ Thị Mai", ChucVu = "Nhân viên hành chính", PhongBan = "Phòng Quản lý", SoDienThoai = "0912345678", Email = "mai@library.com", TrangThai = "Đang làm việc", DiaChi = "987 Đường PQR, Quận 6, TP.HCM" }
        };

        // Hiển thị danh sách nhân viên
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý nhân sự";
            return View(_nhanViens);
        }

        // Tìm kiếm nhân viên thư viện
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return PartialView("_StaffList", _nhanViens);
            }

            var filteredStaff = _nhanViens.Where(nv => 
                nv.HoTen.ToLower().Contains(searchTerm.ToLower()) ||
                nv.MaNV.ToLower().Contains(searchTerm.ToLower()) ||
                nv.ChucVu.ToLower().Contains(searchTerm.ToLower()) ||
                nv.PhongBan.ToLower().Contains(searchTerm.ToLower()) ||
                nv.Email.ToLower().Contains(searchTerm.ToLower())
            ).ToList();

            return PartialView("_StaffList", filteredStaff);
        }

        // Thêm nhân viên thư viện
        [HttpPost]
        public ActionResult Add(NhanVienModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra mã nhân viên đã tồn tại
                    if (_nhanViens.Any(nv => nv.MaNV == model.MaNV))
                    {
                        return Json(new { success = false, message = "Mã nhân viên đã tồn tại!" });
                    }

                    // Kiểm tra email đã tồn tại
                    if (_nhanViens.Any(nv => nv.Email == model.Email))
                    {
                        return Json(new { success = false, message = "Email đã được sử dụng!" });
                    }

                    // Thêm nhân viên mới
                    _nhanViens.Add(model);
                    return Json(new { success = true, message = "Thêm nhân viên thành công!" });
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

        // Lấy thông tin nhân viên để sửa
        [HttpGet]
        public ActionResult GetEmployee(string maNV)
        {
            var nhanVien = _nhanViens.FirstOrDefault(nv => nv.MaNV == maNV);
            if (nhanVien != null)
            {
                return Json(new { 
                    success = true, 
                    data = nhanVien 
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Không tìm thấy nhân viên!" }, JsonRequestBehavior.AllowGet);
        }

        // Sửa nhân viên thư viện
        [HttpPost]
        public ActionResult Edit(NhanVienModel model)
        {
            try
            {
                var existingEmployee = _nhanViens.FirstOrDefault(nv => nv.MaNV == model.MaNV);
                if (existingEmployee != null)
                {
                    // Kiểm tra email trùng với nhân viên khác
                    if (_nhanViens.Any(nv => nv.Email == model.Email && nv.MaNV != model.MaNV))
                    {
                        return Json(new { success = false, message = "Email đã được sử dụng bởi nhân viên khác!" });
                    }

                    // Cập nhật thông tin
                    existingEmployee.HoTen = model.HoTen;
                    existingEmployee.ChucVu = model.ChucVu;
                    existingEmployee.PhongBan = model.PhongBan;
                    existingEmployee.SoDienThoai = model.SoDienThoai;
                    existingEmployee.Email = model.Email;
                    existingEmployee.TrangThai = model.TrangThai;
                    existingEmployee.DiaChi = model.DiaChi;

                    return Json(new { success = true, message = "Cập nhật nhân viên thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy nhân viên!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Xóa nhân viên thư viện
        [HttpPost]
        public ActionResult Delete(string maNV)
        {
            try
            {
                var nhanVien = _nhanViens.FirstOrDefault(nv => nv.MaNV == maNV);
                if (nhanVien != null)
                {
                    _nhanViens.Remove(nhanVien);
                    return Json(new { success = true, message = "Xóa nhân viên thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy nhân viên!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Lấy danh sách chức vụ
        [HttpGet]
        public ActionResult GetPositions()
        {
            var positions = new List<string> 
            { 
                "Thủ thư trưởng", 
                "Thủ thư", 
                "Nhân viên IT", 
                "Nhân viên hành chính" 
            };
            return Json(positions, JsonRequestBehavior.AllowGet);
        }

        // Lấy danh sách phòng ban
        [HttpGet]
        public ActionResult GetDepartments()
        {
            var departments = new List<string> 
            { 
                "Phòng Quản lý", 
                "Phòng Mượn trả", 
                "Phòng Tài liệu", 
                "Phòng Kỹ thuật" 
            };
            return Json(departments, JsonRequestBehavior.AllowGet);
        }
    }

    // Model cho nhân viên thư viện
    public class NhanVienModel
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string ChucVu { get; set; }
        public string PhongBan { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string TrangThai { get; set; }
        public string DiaChi { get; set; }
        
        public string GetInitials()
        {
            if (string.IsNullOrEmpty(HoTen)) return "??";
            var words = HoTen.Split(' ');
            if (words.Length >= 2)
                return (words[words.Length - 2][0].ToString() + words[words.Length - 1][0].ToString()).ToUpper();
            return HoTen.Substring(0, Math.Min(2, HoTen.Length)).ToUpper();
        }
        
        public string GetAvatarColor()
        {
            var colors = new[]
            {
                "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
                "linear-gradient(135deg, #e74c3c, #c0392b)",
                "linear-gradient(135deg, #27ae60, #2ecc71)",
                "linear-gradient(135deg, #9b59b6, #8e44ad)",
                "linear-gradient(135deg, #f39c12, #e67e22)",
                "linear-gradient(135deg, #3498db, #2980b9)"
            };
            return colors[Math.Abs(MaNV.GetHashCode()) % colors.Length];
        }
    }
}