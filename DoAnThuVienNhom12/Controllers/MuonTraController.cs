using DoAnThuVienNhom12.Filters;
using DoAnThuVienNhom12.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    [AuthorizeRole("Admin", "Thủ thư")]
    public class MuonTraController : Controller
    {
        private readonly DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        // ============================
        // DASHBOARD
        // ============================
        public ActionResult Index()
        {
            var now = DateTime.Now.Date;

            var overdue = db.PhieuMuons
                .Where(p => p.TrangThai == "Đang mượn" && p.NgayHenTra < now)
                .ToList();

            foreach (var p in overdue)
                p.TrangThai = "Quá hạn";

            if (overdue.Any())
                db.SaveChanges();

            ViewBag.DangMuon = db.PhieuMuons.Count(p => p.TrangThai == "Đang mượn");
            ViewBag.QuaHan = db.PhieuMuons.Count(p => p.TrangThai == "Quá hạn");
            ViewBag.DaTra = db.PhieuMuons.Count(p => p.TrangThai == "Đã trả");
            ViewBag.PhiPhat = db.PhieuTras.Sum(p => (decimal?)p.TienPhat) ?? 0;

            var muon = db.PhieuMuons
                .Include("DocGia")
                .OrderByDescending(p => p.NgayMuon)
                .ToList();

            ViewBag.DanhSachPhieuTra = db.PhieuTras
                .Include("PhieuMuon")
                .Include("PhieuMuon.DocGia")
                .OrderByDescending(p => p.NgayTra)
                .ToList();

            return View(muon);
        }

        public ActionResult PhieuMuon() => View();
        public ActionResult TaoPhieuTra() => View("PhieuTra");

        public ActionResult PhieuTra(int? id)
        {
            if (id == null)
                return View(); 

            var pm = db.PhieuMuons
                .Include("DocGia")
                .Include("ChiTietPhieuMuons.Sach")
                .FirstOrDefault(x => x.MaPhieuMuon == id.Value);

            if (pm == null)
            {
                TempData["Error"] = "Không tìm thấy phiếu mượn!";
                return View(); 
            }

            return View(pm);
        }







        // ============================
        // ĐANG MƯỢN
        // ============================
        public ActionResult DangMuon()
        {
            var list = db.PhieuMuons
                .Include("DocGia")
                .Where(p => p.TrangThai == "Đang mượn")
                .OrderByDescending(p => p.NgayMuon)
                .ToList();

            return View(list);
        }

        // ============================
        // QUÁ HẠN
        // ============================
        public ActionResult QuaHan()
        {
            var list = db.PhieuMuons
                .Include("DocGia")
                .Where(p => p.TrangThai == "Quá hạn")
                .OrderBy(p => p.NgayHenTra)
                .ToList();

            return View(list);
        }

        // ============================
        // ĐÃ TRẢ
        // ============================
        public ActionResult DaTraDanhSach()
        {
            var list = db.PhieuTras
                .Include("PhieuMuon")
                .Include("PhieuMuon.DocGia")
                .OrderByDescending(p => p.NgayTra)
                .ToList();

            return View(list);
        }

        // ============================
        // PHÍ PHẠT
        // ============================
        public ActionResult PhiPhat()
        {
            var list = db.PhieuTras
                .Include("PhieuMuon")
                .Include("PhieuMuon.DocGia")
                .Where(p => p.TienPhat > 0)
                .OrderByDescending(p => p.NgayTra)
                .ToList();

            return View(list);
        }

        // ============================
        // AJAX: Danh sách phiếu trả
        // ============================
        public ActionResult DanhSachPhieuTra(string search)
        {
            IQueryable<PhieuTra> q = db.PhieuTras
                .Include("PhieuMuon")
                .Include("PhieuMuon.DocGia");

            if (!string.IsNullOrEmpty(search))
            {
                q = q.Where(p =>
                    p.MaPhieuTra.ToString().Contains(search) ||
                    p.PhieuMuon.DocGia.HoTen.Contains(search));
            }

            return PartialView("_PhieuTraList", q.OrderByDescending(p => p.NgayTra).ToList());
        }
        // ============================
        // AJAX: Tìm phiếu mượn (autocomplete)
        // ============================
        public ActionResult TimPhieuMuon(string term)
        {
            int id = 0;
            int.TryParse(term, out id);

            // BƯỚC 1: Lọc database KHÔNG dùng ToString()
            var query = db.PhieuMuons
                .Where(p => p.MaPhieuMuon == id)
                .OrderByDescending(p => p.NgayMuon)
                .Take(10)
                .ToList();   // <-- từ đây trở đi dùng ToString thoải mái

            // BƯỚC 2: Sau khi ToList, mới được dùng ToString()
            var list = query.Select(p => new
            {
                label = p.MaPhieuMuon.ToString(),
                value = p.MaPhieuMuon.ToString()
            });

            return Json(list, JsonRequestBehavior.AllowGet);
        }






        // ============================
        // AJAX: LẤY THÔNG TIN SÁCH (ĐÃ FIX CHUẨN)
        // ============================
        public ActionResult LayThongTinSach(int maSach)
        {
            var s = db.Saches
                .Include("TacGia")
                .Include("NhaXuatBan")
                .FirstOrDefault(x => x.MaSach == maSach);

            if (s == null)
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                success = true,
                title = s.TenSach,
                author = s.TacGia?.TenTacGia ?? "",
                publisher = s.NhaXuatBan?.TenNXB ?? ""
            }, JsonRequestBehavior.AllowGet);
        }



        // ============================
        // AJAX: LẤY SÁCH TỪ PHIẾU MƯỢN (ĐÃ FIX FULL)
        // ============================
        public ActionResult LaySachTuPhieuMuon(string maPhieu)
        {
            if (string.IsNullOrWhiteSpace(maPhieu))
            {
                return Json(new
                {
                    success = false,
                    message = "Mã phiếu mượn không hợp lệ!"
                }, JsonRequestBehavior.AllowGet);
            }

            maPhieu = new string(maPhieu.Where(char.IsDigit).ToArray());

            int id;
            if (!int.TryParse(maPhieu, out id))
            {
                return Json(new
                {
                    success = false,
                    message = "Không thể chuyển mã phiếu sang số!"
                }, JsonRequestBehavior.AllowGet);
            }

            var pm = db.PhieuMuons
                .Include("DocGia")
                .Include("ChiTietPhieuMuons.Sach")
                .FirstOrDefault(p => p.MaPhieuMuon == id);

            if (pm == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Không tìm thấy phiếu mượn!"
                }, JsonRequestBehavior.AllowGet);
            }

            // 4) Lấy danh sách sách
            var books = pm.ChiTietPhieuMuons.Select(c => new
            {
                code = c.MaSach.ToString(),
                title = c.Sach.TenSach,
                author = c.Sach.TacGia?.TenTacGia ?? "",
                publisher = c.Sach.NhaXuatBan?.TenNXB ?? ""
            }).ToList();

            return Json(new
            {
                success = true,
                reader = pm.DocGia.HoTen,
                dueDate = pm.NgayHenTra?.ToString("yyyy-MM-dd"),
                books = books
            }, JsonRequestBehavior.AllowGet);
        }





        // ============================
        // SAVE PHIẾU MƯỢN
        // ============================
        [HttpPost]
        public ActionResult SavePhieuMuon(PhieuMuonPayload req)
        {
            try
            {
                if (req == null || string.IsNullOrEmpty(req.studentId))
                    return Json(new { success = false, message = "Thiếu dữ liệu!" });

                var dg = db.DocGias.FirstOrDefault(x => x.MaSinhVien == req.studentId);

                if (dg == null)
                {
                    dg = new DocGia
                    {
                        HoTen = req.studentName,
                        MaSinhVien = req.studentId,
                        
                    };
                    db.DocGias.Add(dg);
                    db.SaveChanges();
                }

                var pm = new PhieuMuon
                {
                    MaDocGia = dg.MaDocGia,
                    NgayMuon = DateTime.Parse(req.borrowDate),
                    NgayHenTra = DateTime.Parse(req.dueDate),
                    TrangThai = "Đang mượn",
                    SoLuongSach = req.books.Count
                };

                db.PhieuMuons.Add(pm);
                db.SaveChanges();

                foreach (var code in req.books)
                {
                    int ma = int.Parse(code);

                    db.ChiTietPhieuMuons.Add(new ChiTietPhieuMuon
                    {
                        MaPhieuMuon = pm.MaPhieuMuon,
                        MaSach = ma,
                        SoLuong = 1
                    });

                    var sach = db.Saches.Find(ma);
                    if (sach != null)
                        sach.SoLuongCoSan--;
                }

                db.SaveChanges();

                return Json(new { success = true, message = "Tạo phiếu mượn thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // ============================
        // SAVE PHIẾU TRẢ
        // ============================
        [HttpPost]
        public ActionResult SavePhieuTra(PhieuTraPayload req)
        {
            try
            {
                if (req == null)
                    return Json(new { success = false, message = "Thiếu dữ liệu!" });

                // FIX: nếu books null thì tạo danh sách rỗng
                if (req.books == null)
                    req.books = new System.Collections.Generic.List<ReturnBookPayload>();

                var pm = db.PhieuMuons
                    .Include("ChiTietPhieuMuons")
                    .FirstOrDefault(x => x.MaPhieuMuon == req.borrowId);

                if (pm == null)
                    return Json(new { success = false, message = "Không tìm thấy phiếu mượn!" });

                DateTime ngayTra = DateTime.Parse(req.returnDate);

                // TÍNH TRỄ HẠN
                int soNgayTre = 0;
                int tienTre = 0;

                if (pm.NgayHenTra.HasValue && ngayTra.Date > pm.NgayHenTra.Value.Date)
                {
                    soNgayTre = (ngayTra.Date - pm.NgayHenTra.Value.Date).Days;
                    tienTre = soNgayTre * 2000;
                }

                // TÍNH HƯ HỎNG
                int tienHu = 0;

                foreach (var b in req.books)
                {
                    if (b.status == "Hư hỏng nhẹ") tienHu += 10000;
                    else if (b.status == "Mất sách") tienHu += 50000;
                }


                // TẠO PHIẾU TRẢ
                var pt = new PhieuTra
                {
                    MaPhieuMuon = pm.MaPhieuMuon,
                    NgayTra = ngayTra,
                    SoNgayTre = soNgayTre,
                    TienPhat = tienTre + tienHu,
                    GhiChu = tienHu > 0 ? "Sách hư hỏng" : ""
                };

                db.PhieuTras.Add(pt);

                // CẬP NHẬT SỐ LƯỢNG SÁCH
                foreach (var b in req.books)
                {
                    int maSach = int.Parse(b.code);

                    var sach = db.Saches.Find(maSach);
                    if (sach != null && b.status != "Mất sách")
                        sach.SoLuongCoSan++;
                }

                pm.TrangThai = "Đã trả";

                db.SaveChanges();

                return Json(new { success = true, message = "Xác nhận trả sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        // ============================
        // XÓA PHIẾU TRẢ
        // ============================
        [HttpPost]
        public ActionResult XoaPhieuTra(int MaPhieuTra)
        {
            var pt = db.PhieuTras.Find(MaPhieuTra);
            if (pt == null)
                return Json(new { success = false, message = "Không tìm thấy!" });

            db.PhieuTras.Remove(pt);
            db.SaveChanges();

            return Json(new { success = true, message = "Xóa phiếu trả thành công!" });
        }
        [HttpPost]
        public ActionResult XoaPhieuMuon(int MaPhieuMuon)
        {
            try
            {
                var pm = db.PhieuMuons.Find(MaPhieuMuon);
                if (pm == null)
                    return Json(new { success = false, message = "Phiếu mượn không tồn tại!" });

                // Nếu phiếu đang mượn → không cho xóa
                if (pm.TrangThai == "Đang mượn" || pm.TrangThai == "Quá hạn")
                {
                    return Json(new
                    {
                        success = false,
                        message = "Phiếu mượn đang mượn sách – không thể xóa!"
                    });
                }

                // Nếu đã trả thì xóa Phiếu Trả trước
                var phieuTra = db.PhieuTras.FirstOrDefault(x => x.MaPhieuMuon == MaPhieuMuon);
                if (phieuTra != null)
                {
                    db.PhieuTras.Remove(phieuTra);
                }

                // Xóa Chi tiết phiếu mượn
                var chitiets = db.ChiTietPhieuMuons.Where(x => x.MaPhieuMuon == MaPhieuMuon).ToList();
                foreach (var ct in chitiets)
                {
                    db.ChiTietPhieuMuons.Remove(ct);
                }

                // Xóa phiếu mượn
                db.PhieuMuons.Remove(pm);
                db.SaveChanges();

                return Json(new { success = true, message = "Đã xóa phiếu mượn thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }
 








        // ============================
        // PAYLOAD MODELS
        // ============================
        public class PhieuMuonPayload
        {
            public string studentName { get; set; }
            public string studentId { get; set; }
            public string classId { get; set; }
            public string borrowDate { get; set; }
            public string dueDate { get; set; }
            public System.Collections.Generic.List<string> books { get; set; }
        }

        public class PhieuTraPayload
        {
            public PhieuTraPayload()
            {
                books = new List<ReturnBookPayload>();
            }

            public int borrowId { get; set; }
            public string returnDate { get; set; }
            public List<ReturnBookPayload> books { get; set; }
        }


        public class ReturnBookPayload
        {
            public string code { get; set; }
            public string status { get; set; }
            public int fine { get; set; }
        }
    }
}
