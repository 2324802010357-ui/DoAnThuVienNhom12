using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnThuVienNhom12.Models
{
    public class PhieuMuonRequest
    {
        public string studentId { get; set; }
        public DateTime borrowDate { get; set; }
        public DateTime dueDate { get; set; }
        public List<string> books { get; set; }
    }
}