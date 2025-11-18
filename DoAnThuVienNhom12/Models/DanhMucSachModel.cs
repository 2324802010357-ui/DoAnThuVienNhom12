using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnThuVienNhom12.Models
{
    public class DanhMucSachModel
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }
        public int SoLuongSach { get; set; }
    }
}