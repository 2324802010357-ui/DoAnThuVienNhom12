using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class MuonTraController : Controller
    {
        // Dữ liệu mẫu phiếu mượn
        private static List<PhieuMuonModel> _phieuMuons = new List<PhieuMuonModel>
        {
            new PhieuMuonModel { MaPhieu = "PM001", MaDocGia = "DG001", TenDocGia = "Nguyễn Văn An", NgayMuon = DateTime.Now.AddDays(-5), NgayTra = DateTime.Now.AddDays(9), TrangThai = "Đang mượn", SoLuongSach = 3 },
            new PhieuMuonModel { MaPhieu = "PM002", MaDocGia = "DG002", TenDocGia = "Trần Thị Bình", NgayMuon = DateTime.Now.AddDays(-10), NgayTra = DateTime.Now.AddDays(-3), TrangThai = "Quá hạn", SoLuongSach = 2 },
            new PhieuMuonModel { MaPhieu = "PM003", MaDocGia = "DG003", TenDocGia = "Lê Văn Cường", NgayMuon = DateTime.Now.AddDays(-15), NgayTra = DateTime.Now.AddDays(-1), TrangThai = "Đã trả", SoLuongSach = 1 }
        };

        private static List<PhieuTraModel> _phieuTras = new List<PhieuTraModel>
        {
            new PhieuTraModel { MaPhieu = "PT001", MaPhieuMuon = "PM003", MaDocGia = "DG003", TenDocGia = "Lê Văn Cường", NgayTra = DateTime.Now.AddDays(-1), SoSachTra = 1, TinhTrangSach = "Tốt", PhiPhat = 0 },
            new PhieuTraModel { MaPhieu = "PT002", MaPhieuMuon = "PM004", MaDocGia = "DG004", TenDocGia = "Phạm Thị Dung", NgayTra = DateTime.Now.AddDays(-2), SoSachTra = 2, TinhTrangSach = "Hư hỏng nhẹ", PhiPhat = 50000 }
        };

        // 1. Quản lý mượn - Trang chính
        public ActionResult Index()
        {
            ViewBag.Title = "Quản lý mượn trả";
            return View();
        }

        // Tạo phiếu mượn
        public ActionResult PhieuMuon()
        {
            ViewBag.Title = "Tạo phiếu mượn";
            return View();
        }

        // Cập nhật phiếu mượn  
        [HttpPost]
        public ActionResult CapNhatPhieuMuon(PhieuMuonModel model)
        {
            try
            {
                var phieu = _phieuMuons.FirstOrDefault(p => p.MaPhieu == model.MaPhieu);
                if (phieu != null)
                {
                    phieu.NgayTra = model.NgayTra;
                    phieu.TrangThai = model.TrangThai;
                    return Json(new { success = true, message = "Cập nhật phiếu mượn thành công!" });
                }
                return Json(new { success = false, message = "Không tìm thấy phiếu mượn!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Tìm kiếm phiếu mượn
        [HttpPost]
        public ActionResult TimKiemPhieuMuon(string searchTerm, string trangThai)
        {
            var result = _phieuMuons.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = result.Where(p => 
                    p.MaPhieu.Contains(searchTerm) ||
                    p.MaDocGia.Contains(searchTerm) ||
                    p.TenDocGia.ToLower().Contains(searchTerm.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(trangThai) && trangThai != "Tất cả")
            {
                result = result.Where(p => p.TrangThai == trangThai);
            }

            return PartialView("_PhieuMuonList", result.ToList());
        }

        // Xóa phiếu mượn
        [HttpPost]
        public ActionResult XoaPhieuMuon(string maPhieu)
        {
            try
            {
                var phieu = _phieuMuons.FirstOrDefault(p => p.MaPhieu == maPhieu);
                if (phieu != null)
                {
                    _phieuMuons.Remove(phieu);
                    return Json(new { success = true, message = "Xóa phiếu mượn thành công!" });
                }
                return Json(new { success = false, message = "Không tìm thấy phiếu mượn!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // 2. Quản lý trả
        public ActionResult PhieuTra()
        {
            ViewBag.Title = "Tạo phiếu trả";
            return View();
        }

        // Cập nhật phiếu trả
        [HttpPost] 
        public ActionResult CapNhatPhieuTra(PhieuTraModel model)
        {
            try
            {
                var phieu = _phieuTras.FirstOrDefault(p => p.MaPhieu == model.MaPhieu);
                if (phieu != null)
                {
                    phieu.TinhTrangSach = model.TinhTrangSach;
                    phieu.PhiPhat = model.PhiPhat;
                    return Json(new { success = true, message = "Cập nhật phiếu trả thành công!" });
                }
                return Json(new { success = false, message = "Không tìm thấy phiếu trả!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Tìm kiếm phiếu trả
        [HttpPost]
        public ActionResult TimKiemPhieuTra(string searchTerm)
        {
            var result = _phieuTras.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = result.Where(p => 
                    p.MaPhieu.Contains(searchTerm) ||
                    p.MaDocGia.Contains(searchTerm) ||
                    p.TenDocGia.ToLower().Contains(searchTerm.ToLower())
                );
            }

            return PartialView("_PhieuTraList", result.ToList());
        }

        // Xóa phiếu trả
        [HttpPost]
        public ActionResult XoaPhieuTra(string maPhieu)
        {
            try
            {
                var phieu = _phieuTras.FirstOrDefault(p => p.MaPhieu == maPhieu);
                if (phieu != null)
                {
                    _phieuTras.Remove(phieu);
                    return Json(new { success = true, message = "Xóa phiếu trả thành công!" });
                }
                return Json(new { success = false, message = "Không tìm thấy phiếu trả!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // API lấy dữ liệu
        [HttpGet]
        public ActionResult GetPhieuMuonData()
        {
            return Json(_phieuMuons, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPhieuTraData()
        {
            return Json(_phieuTras, JsonRequestBehavior.AllowGet);
        }
    }

    // Models
    public class PhieuMuonModel
    {
        public string MaPhieu { get; set; }
        public string MaDocGia { get; set; }
        public string TenDocGia { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime NgayTra { get; set; }
        public string TrangThai { get; set; }
        public int SoLuongSach { get; set; }
    }

    public class PhieuTraModel
    {
        public string MaPhieu { get; set; }
        public string MaPhieuMuon { get; set; }
        public string MaDocGia { get; set; }
        public string TenDocGia { get; set; }
        public DateTime NgayTra { get; set; }
        public int SoSachTra { get; set; }
        public string TinhTrangSach { get; set; }
        public decimal PhiPhat { get; set; }
    }
}