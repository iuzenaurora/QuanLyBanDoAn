using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDoAn.Models
{
    public class TopSanPhamModel
    {
        public string TenSP { get; set; }
        public string AnhBia { get; set; }
        public decimal? GiaBan { get; set; }
        public int? DaBan { get; set; }
        public decimal? TongTien { get; set; }
    }
}