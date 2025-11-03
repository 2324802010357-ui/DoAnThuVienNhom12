using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class SachController : Controller
    {
        // Dữ liệu mẫu sách
        private static List<SachModel> _sachs = new List<SachModel>
        {
            new SachModel { MaSach = "S001", TenSach = "Lập trình C# cơ bản", TacGia = "Nguyễn Văn A", TheLoai = "Tin học", DanhMuc = "Công nghệ", NhaXuatBan = "NXB Giáo dục", NamXuatBan = 2023, SoTrang = 350, TinhTrang = "Có sẵn", SoLuong = 10, MoTa = "Sách học lập trình C# từ cơ bản đến nâng cao" },
            new SachModel { MaSach = "S002", TenSach = "Văn học Việt Nam", TacGia = "Trần Thị B", TheLoai = "Văn học", DanhMuc = "Văn học", NhaXuatBan = "NXB Văn học", NamXuatBan = 2022, SoTrang = 280, TinhTrang = "Có sẵn", SoLuong = 5, MoTa = "Tuyển tập văn học Việt Nam qua các thời kỳ" },
            new SachModel { MaSach = "S003", TenSach = "Kinh tế học đại cương", TacGia = "Lê Văn C", TheLoai = "Kinh tế", DanhMuc = "Kinh tế", NhaXuatBan = "NXB Kinh tế", NamXuatBan = 2021, SoTrang = 420, TinhTrang = "Đang mượn", SoLuong = 8, MoTa = "Giáo trình kinh tế học cơ bản" },
            new SachModel { MaSach = "S004", TenSach = "Tiếng Anh giao tiếp", TacGia = "Phạm Thị D", TheLoai = "Ngoại ngữ", DanhMuc = "Ngôn ngữ", NhaXuatBan = "NXB Đại học Quốc gia", NamXuatBan = 2023, SoTrang = 180, TinhTrang = "Có sẵn", SoLuong = 15, MoTa = "Sách học tiếng Anh giao tiếp cơ bản" },
            new SachModel { MaSach = "S005", TenSach = "Lịch sử Việt Nam", TacGia = "Hoàng Văn E", TheLoai = "Lịch sử", DanhMuc = "Lịch sử", NhaXuatBan = "NXB Chính trị Quốc gia", NamXuatBan = 2020, SoTrang = 500, TinhTrang = "Có sẵn", SoLuong = 12, MoTa = "Lịch sử Việt Nam từ thời nguyên thủy đến hiện đại" },
            new SachModel { MaSach = "S006", TenSach = "Khoa học máy tính", TacGia = "Võ Văn F", TheLoai = "Tin học", DanhMuc = "Công nghệ", NhaXuatBan = "NXB Khoa học", NamXuatBan = 2023, SoTrang = 380, TinhTrang = "Có sẵn", SoLuong = 7, MoTa = "Nhập môn khoa học máy tính" }
        };

        // Hiển thị trang chính
        public ActionResult Index()
        {
            ViewBag.Title = "Tìm kiếm sách";
            return View(_sachs);
        }

        // Tìm kiếm sách nâng cao
        [HttpPost]
        public ActionResult TimKiemSach(TimKiemSachModel searchModel)
        {
            var result = _sachs.AsQueryable();

            // Tìm kiếm theo từ khóa chính
            if (!string.IsNullOrEmpty(searchModel.TuKhoa))
            {
                result = result.Where(s => 
                    s.TenSach.ToLower().Contains(searchModel.TuKhoa.ToLower()) ||
                    s.TacGia.ToLower().Contains(searchModel.TuKhoa.ToLower()) ||
                    s.MoTa.ToLower().Contains(searchModel.TuKhoa.ToLower())
                );
            }

            // Lọc theo tác giả
            if (!string.IsNullOrEmpty(searchModel.TacGia))
            {
                result = result.Where(s => s.TacGia.ToLower().Contains(searchModel.TacGia.ToLower()));
            }

            // Lọc theo thể loại
            if (!string.IsNullOrEmpty(searchModel.TheLoai))
            {
                result = result.Where(s => s.TheLoai.ToLower().Contains(searchModel.TheLoai.ToLower()));
            }

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(searchModel.DanhMuc))
            {
                result = result.Where(s => s.DanhMuc.ToLower().Contains(searchModel.DanhMuc.ToLower()));
            }

            // Lọc theo nhà xuất bản
            if (!string.IsNullOrEmpty(searchModel.NhaXuatBan))
            {
                result = result.Where(s => s.NhaXuatBan.ToLower().Contains(searchModel.NhaXuatBan.ToLower()));
            }

            // Lọc theo năm xuất bản
            if (searchModel.NamTu.HasValue)
            {
                result = result.Where(s => s.NamXuatBan >= searchModel.NamTu.Value);
            }

            if (searchModel.NamDen.HasValue)
            {
                result = result.Where(s => s.NamXuatBan <= searchModel.NamDen.Value);
            }

            // Lọc theo tình trạng
            if (!string.IsNullOrEmpty(searchModel.TinhTrang) && searchModel.TinhTrang != "Tất cả")
            {
                result = result.Where(s => s.TinhTrang == searchModel.TinhTrang);
            }

            return PartialView("_BookList", result.ToList());
        }

        // Tìm kiếm đơn giản từ trang chủ
        public ActionResult Search(string q)
        {
            ViewBag.Title = "Kết quả tìm kiếm";
            ViewBag.Query = q;
            
            var result = _sachs.AsQueryable();
            
            if (!string.IsNullOrEmpty(q))
            {
                result = result.Where(s => 
                    s.TenSach.ToLower().Contains(q.ToLower()) ||
                    s.TacGia.ToLower().Contains(q.ToLower()) ||
                    s.TheLoai.ToLower().Contains(q.ToLower())
                );
            }
            
            return View(result.ToList());
        }

        // Chi tiết sách
        public ActionResult Detail(string id)
        {
            var sach = _sachs.FirstOrDefault(s => s.MaSach == id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Title = sach.TenSach;
            ViewBag.Id = id;
            return View(sach);
        }

        // GET: Chỉnh sửa sách
        public ActionResult Edit(string id)
        {
            // Nếu không có id, lấy sách đầu tiên để demo
            if (string.IsNullOrEmpty(id))
            {
                id = "S001";
            }
            
            var sach = _sachs.FirstOrDefault(s => s.MaSach == id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Title = "Cập nhật: " + sach.TenSach;
            ViewBag.Id = id;
            return View(sach);
        }

        // POST: Lưu chỉnh sửa sách
        [HttpPost]
        public ActionResult Edit(SachModel model)
        {
            if (ModelState.IsValid)
            {
                var sach = _sachs.FirstOrDefault(s => s.MaSach == model.MaSach);
                if (sach != null)
                {
                    // Cập nhật thông tin
                    sach.TenSach = model.TenSach;
                    sach.TacGia = model.TacGia;
                    sach.TheLoai = model.TheLoai;
                    sach.DanhMuc = model.DanhMuc;
                    sach.NhaXuatBan = model.NhaXuatBan;
                    sach.NamXuatBan = model.NamXuatBan;
                    sach.SoTrang = model.SoTrang;
                    sach.TinhTrang = model.TinhTrang;
                    sach.SoLuong = model.SoLuong;
                    sach.MoTa = model.MoTa;
                    
                    TempData["SuccessMessage"] = "Cập nhật sách thành công!";
                    return RedirectToAction("Detail", new { id = model.MaSach });
                }
            }
            
            return View(model);
        }

        // Lấy danh sách thể loại
        [HttpGet]
        public ActionResult GetTheLoai()
        {
            var theLoai = _sachs.Select(s => s.TheLoai).Distinct().ToList();
            return Json(theLoai, JsonRequestBehavior.AllowGet);
        }

        // Lấy danh sách tác giả
        [HttpGet]
        public ActionResult GetTacGia()
        {
            var tacGia = _sachs.Select(s => s.TacGia).Distinct().ToList();
            return Json(tacGia, JsonRequestBehavior.AllowGet);
        }

        // Lấy danh sách nhà xuất bản
        [HttpGet]
        public ActionResult GetNhaXuatBan()
        {
            var nxb = _sachs.Select(s => s.NhaXuatBan).Distinct().ToList();
            return Json(nxb, JsonRequestBehavior.AllowGet);
        }
    }

    // Model cho sách
    public class SachModel
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public string DanhMuc { get; set; }
        public string NhaXuatBan { get; set; }
        public int NamXuatBan { get; set; }
        public int SoTrang { get; set; }
        public string TinhTrang { get; set; }
        public int SoLuong { get; set; }
        public string MoTa { get; set; }
    }

    // Model cho tìm kiếm
    public class TimKiemSachModel
    {
        public string TuKhoa { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public string DanhMuc { get; set; }
        public string NhaXuatBan { get; set; }
        public int? NamTu { get; set; }
        public int? NamDen { get; set; }
        public string TinhTrang { get; set; }
    }
}