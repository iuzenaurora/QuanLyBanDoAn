using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDoAn.Models
{
    public class CartItem
    {
        QL_SanPhamEntities db = new QL_SanPhamEntities();
        public int MaItem { get; set; }
        public string TenItem { get; set; }
        public string AnhItem { get; set; }
        public int soluong { get; set; }
        public decimal dongia { get; set; }
        public decimal thanhtien
        {
            get { return soluong * dongia; }
        }

        public CartItem(int id)
        {
            tblSanPham sp = db.tblSanPham.FirstOrDefault(x => x.MaSP == id);
            MaItem = sp.MaSP;
            TenItem = sp.TenSP;
            AnhItem = sp.AnhBia;
            soluong = 1;
            dongia = sp.GiaBan.Value;
        }
    }
}