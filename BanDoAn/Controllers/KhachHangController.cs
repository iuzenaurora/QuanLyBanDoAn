using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoAn.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QL_SanPhamEntities db = new QL_SanPhamEntities();
        public ActionResult LSDonHang()
        {
            var user = Session["User"] as tbltaikhoan;
            if (user == null || user.makh == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Lấy danh sách đơn hàng của khách, sắp xếp mới nhất lên đầu
            var orders = db.tblHoaDon.Where(x => x.MaKH == user.makh).OrderByDescending(x => x.NgayLap);
            return View(orders);
        }

        public ActionResult CancelOrderOnSubmit(int id)
        {
            var user = Session["User"] as tbltaikhoan;
            if (user == null) return RedirectToAction("Login", "User");

            var order = db.tblHoaDon.FirstOrDefault(x => x.MaHD == id);

            // Kiểm tra bảo mật: Đơn hàng phải tồn tại và đúng là của khách hàng đang đăng nhập
            if (order != null && order.MaKH == user.makh)
            {
                // Kiểm tra trạng thái: Chỉ được hủy khi đơn hàng đang "Chờ xử lý" (ID = 1)
                // Nếu Admin đã duyệt (ID = 2) hoặc đã hủy (ID = 3) thì không được phép
                if (order.TinhTrang == 1)
                {
                    order.TinhTrang = 3; // 3 là trạng thái "Đã hủy" trong Database
                    db.SaveChanges();

                    TempData["ThongBao"] = "Hủy đơn hàng thành công!";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Đơn hàng đang giao hoặc đã hoàn tất, không thể hủy!";
                    TempData["LoaiThongBao"] = "error";
                }
            }
            else
            {
                TempData["ThongBao"] = "Lỗi xác thực đơn hàng!";
                TempData["LoaiThongBao"] = "error";
            }

            return RedirectToAction("LSDonHang");
        }
    }
}