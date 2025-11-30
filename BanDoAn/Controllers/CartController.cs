using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanDoAn.Models;

namespace BanDoAn.Controllers
{
    public class CartController : Controller
    {
        // GET: CartItem
        QL_SanPhamEntities db = new QL_SanPhamEntities();
        public ActionResult ViewCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || cart.list.Count == 0)
            {
                cart = new Cart();
            }
            return PartialView(cart);
        }

        public ActionResult AddToCart(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
            }
            int result = cart.Them(id);
            if (result == 1)
            {
                Session["Cart"] = cart;
            }
            else
            {
                TempData["ThongBao"] = "Lỗi không thêm được sản phẩm vào giỏ hàng!";
                TempData["LoaiThongBao"] = "error";
            }

            TempData["ThongBao"] = "Thêm vào giỏ hàng thành công!";
            TempData["LoaiThongBao"] = "success";

            // ← QUAY LẠI ĐÚNG TRANG TRƯỚC KHI BẤM NÚT
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult _CartModal()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
            }
            return PartialView(cart);
        }

        public ActionResult UpdateSL(int id, int type = 1)
        {
            Cart cart = (Cart)Session["Cart"];
            int result = 1;
            if (type == -1)
            {
                result = cart.Giam(id);
            }
            else
            {
                result = cart.Them(id);
            }

            if (result == 1)
            {
                Session["Cart"] = cart;
            }
            return PartialView("ViewCart", cart);
        }

        public ActionResult OrderOnSubmit()
        {
            var user = (tbltaikhoan)Session["User"];
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }
            Cart cart = (Cart)Session["Cart"];
            if (cart == null || cart.list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            tblHoaDon hd = new tblHoaDon();
            if(user.makh != null)
            {
                hd.MaKH = user.makh;
            }
            else
            {
                hd.MaNV = user.manv;
            }
            hd.NgayLap = DateTime.Now;
            hd.TongTien = cart.tongThanhTien();
            hd.TinhTrang = 1; // 1: Đang chờ xử lý (theo SQL)
            hd.DaThanhToan = true;

            // Lấy địa chỉ từ bảng khách hàng (hoặc cho nhập form riêng nếu muốn)
            var kh = db.tblKhachHang.Find(user.makh);
            hd.DiaChiGiaoHang = kh != null ? kh.DiaChi : "Cập nhật sau";

            db.tblHoaDon.Add(hd);
            db.SaveChanges(); // Lưu để lấy MaHD tự sinh

            // 2. Lưu chi tiết hóa đơn
            foreach (var item in cart.list)
            {
                tblChiTietHoaDon cthd = new tblChiTietHoaDon();
                cthd.MaHD = hd.MaHD;
                cthd.MaSP = item.MaItem;
                cthd.SoLuong = item.soluong;
                cthd.GiaBan = item.dongia;

                db.tblChiTietHoaDon.Add(cthd);
            }
            db.SaveChanges();

            ViewBag.MaHD = hd.MaHD;
            ViewBag.TenKH = user.tblNhanVien != null ? user.tblNhanVien.TenNV : user.tblKhachHang.TenKH;
            ViewBag.TongTien = cart.tongThanhTien();
            Session["Cart"] = null;
            return View();
        }
    }
}