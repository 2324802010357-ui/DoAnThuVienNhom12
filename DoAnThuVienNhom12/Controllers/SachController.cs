using DoAnThuVienNhom12.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Controllers
{
    public class SachController : Controller
    {
        private DoAnThuVienNhom12Entities db = new DoAnThuVienNhom12Entities();

        // ================== DANH SÁCH SÁCH ==================
        [HttpGet]
        public ActionResult Index()
        {
            var list = db.Saches
                .Include(s => s.DanhMuc)
                .Include(s => s.TacGia)
                .Include(s => s.NhaXuatBan)
                .AsNoTracking()
                .OrderByDescending(s => s.NamXuatBan)
                .ToList();

            return View(list);
        }


        // ================== CHI TIẾT SÁCH ==================
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var sach = db.Saches
                .Include(s => s.DanhMuc)
                .Include(s => s.TacGia)
                .Include(s => s.NhaXuatBan)
                .FirstOrDefault(s => s.MaSach == id);

            if (sach == null)
                return HttpNotFound();

            return View(sach);
        }

        // ================== CHỈNH SỬA SÁCH ==================
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.Saches.Find(id);
            if (sach == null)
                return HttpNotFound();

            LoadDropdowns(sach);
            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sach sach)
        {
            if (ModelState.IsValid)
            {
                var oldBook = db.Saches.Find(sach.MaSach);
                if (oldBook == null)
                    return HttpNotFound();

                // ✅ Chỉ cập nhật phần người dùng thực sự thay đổi
                if (!string.IsNullOrWhiteSpace(sach.TenSach))
                    oldBook.TenSach = sach.TenSach;

                if (sach.MaDanhMuc > 0)
                    oldBook.MaDanhMuc = sach.MaDanhMuc;

                if (sach.MaTacGia > 0)
                    oldBook.MaTacGia = sach.MaTacGia;

                if (sach.MaNXB > 0)
                    oldBook.MaNXB = sach.MaNXB;

                if (sach.MaKe > 0)
                    oldBook.MaKe = sach.MaKe;

                if (sach.NamXuatBan > 0)
                    oldBook.NamXuatBan = sach.NamXuatBan;

                if (sach.SoTrang > 0)
                    oldBook.SoTrang = sach.SoTrang;

                if (!string.IsNullOrWhiteSpace(sach.NgonNgu))
                    oldBook.NgonNgu = sach.NgonNgu;

                if (sach.GiaNhap > 0)
                    oldBook.GiaNhap = sach.GiaNhap;

                if (sach.GiaBia > 0)
                    oldBook.GiaBia = sach.GiaBia;

                if (sach.SoLuong > 0)
                    oldBook.SoLuong = sach.SoLuong;

                if (sach.SoLuongCoSan > 0)
                    oldBook.SoLuongCoSan = sach.SoLuongCoSan;

                if (!string.IsNullOrWhiteSpace(sach.TinhTrang))
                    oldBook.TinhTrang = sach.TinhTrang;

                if (!string.IsNullOrWhiteSpace(sach.MoTa))
                    oldBook.MoTa = sach.MoTa;

                if (!string.IsNullOrWhiteSpace(sach.TuKhoa))
                    oldBook.TuKhoa = sach.TuKhoa;

                if (!string.IsNullOrWhiteSpace(sach.AnhBia))
                    oldBook.AnhBia = sach.AnhBia.Trim();

                oldBook.NgayCapNhat = DateTime.Now;

                db.SaveChanges();
                TempData["Success"] = "✅ Cập nhật thành công!";
                return RedirectToAction("Index");
            }

            LoadDropdowns(sach);
            return View(sach);
        }

        // ================== XÓA SÁCH ==================
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sach = db.Saches
                .Include(s => s.DanhMuc)
                .Include(s => s.TacGia)
                .Include(s => s.NhaXuatBan)
                .AsNoTracking()
                .FirstOrDefault(s => s.MaSach == id);

            if (sach == null)
                return HttpNotFound();

            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var sach = db.Saches.Find(id);
            if (sach == null)
                return HttpNotFound();

            db.Saches.Remove(sach);
            db.SaveChanges();
            TempData["Success"] = "🗑️ Xóa thành công!";
            return RedirectToAction("Index");
        }


        // ================== LOAD DROPDOWN ==================
        private void LoadDropdowns(Sach sach = null)
        {
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs.ToList(), "MaDanhMuc", "TenDanhMuc", sach?.MaDanhMuc);
            ViewBag.MaTacGia = new SelectList(db.TacGias.ToList(), "MaTacGia", "TenTacGia", sach?.MaTacGia);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList(), "MaNXB", "TenNXB", sach?.MaNXB);
            ViewBag.MaKe = new SelectList(db.KeSaches.ToList(), "MaKe", "TenKe", sach?.MaKe);
        }

        // ================== DỌN DẸP ==================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            ViewBag.MaKe = new SelectList(db.KeSaches, "MaKe", "TenKe");

            return View();
        }

        // ============================
        // CREATE (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sach model)
        {
            if (ModelState.IsValid)
            {
                model.SoLuongCoSan = model.SoLuong;       // số lượng đầu vào
                model.NgayNhap = System.DateTime.Now;
                model.NgayCapNhat = System.DateTime.Now;
                model.TinhTrang = "Còn hàng";

                db.Saches.Add(model);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Load lại dropdown nếu có lỗi
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", model.MaDanhMuc);
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia", model.MaTacGia);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", model.MaNXB);
            ViewBag.MaKe = new SelectList(db.KeSaches, "MaKe", "TenKe", model.MaKe);

            return View(model);
        }
    }
}
    

