using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Runtime.Remoting.Messaging;
using BanDoAn.Models;

namespace BanDoAn.Controllers
{
    public class HomeController : Controller
    {
        QL_SanPhamEntities db = new QL_SanPhamEntities();
        public ActionResult Index()
        {
            return View(db.tblSanPham);
        }

        public ActionResult _MenuThucDon()
        {
            return PartialView(db.tblLoaiSP);
        }

        public ActionResult _DanhMucThucDon()
        {
            return PartialView(db.tblLoaiSP);
        }

        public ActionResult SanPhamTheoDM(int MaDM)
        {
            var listsp = db.tblSanPham.Where(sp => sp.MaDM == MaDM);
            return View("Index", listsp);
        }

        public ActionResult SanPhamTheoLoai(int MaLoai)
        {
            var listsp = db.tblSanPham.Where(sp => sp.tblDanhMucCon.MaLoai == MaLoai);
            return View("Index", listsp);
        }

        public ActionResult TimKiem(string keyword)
        {
            ViewBag.keyword = keyword;
            var list = db.tblSanPham.Where(x => x.TenSP.ToLower().Contains(keyword.ToLower().Trim()));
            return View("Index", list);
        }

        public ActionResult Detail(int id)
        {
            tblSanPham sp = db.tblSanPham.FirstOrDefault(x => x.MaSP == id);
            ViewBag.ListSPLienQuan = db.tblSanPham.Where(x => x.MaDM == sp.MaDM && x.MaSP != id).Take(4).ToList();
            return View(sp);
        }

        public ActionResult QLThucDon()
        {
            return View(db.tblSanPham);
        }

        public ActionResult AddProduct()
        {
            ViewBag.ListDanhMuc = db.tblDanhMucCon.Include("tblLoaiSP").ToList();
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddOnSubmit(tblSanPham sp, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                string Dir = "~/Content/images";

                // XỬ LÝ UPLOAD FILE
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    filename = Path.GetFileName(fileUpload.FileName);
                    string physicalDir = Server.MapPath(Dir);

                    if (!Directory.Exists(physicalDir))
                    {
                        Directory.CreateDirectory(physicalDir);
                    }

                    var path = Path.Combine(physicalDir, filename);
                    fileUpload.SaveAs(path);
                }

                // GÁN DỮ LIỆU SẢN PHẨM
                tblSanPham newSP = new tblSanPham();
                newSP.TenSP = sp.TenSP;
                newSP.MaDM = sp.MaDM;
                newSP.GiaBan = sp.GiaBan;
                newSP.MoTa = sp.MoTa;
                newSP.AnhBia = filename;

                // LƯU DATABASE
                db.tblSanPham.Add(newSP);
                db.SaveChanges();
                TempData["ThongBao"] = "Thêm sản phẩm thành công!";
                TempData["LoaiThongBao"] = "success";
            }
            else
            {
                TempData["ThongBao"] = "Thêm sản phẩm thất bại!";
                TempData["LoaiThongBao"] = "error";
            }

            return RedirectToAction("QLThucDon", "Home"); ;
        }

        public ActionResult EditProduct(int id)
        {
            ViewBag.ListDanhMuc = db.tblDanhMucCon.Include("tblLoaiSP").ToList();
            tblSanPham sp = db.tblSanPham.FirstOrDefault(x => x.MaSP == id);
            return PartialView(sp);
        }

        public ActionResult EditOnSubmit(tblSanPham sp, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                tblSanPham existingSP = db.tblSanPham.FirstOrDefault(x => x.MaSP == sp.MaSP);
                if (existingSP != null)
                {
                    string filename = existingSP.AnhBia;
                    string Dir = "~/Content/images";
                    // XỬ LÝ UPLOAD FILE
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        filename = Path.GetFileName(fileUpload.FileName);
                        string physicalDir = Server.MapPath(Dir);
                        if (!Directory.Exists(physicalDir))
                        {
                            Directory.CreateDirectory(physicalDir);
                        }
                        var path = Path.Combine(physicalDir, filename);
                        fileUpload.SaveAs(path);
                    }
                    // CẬP NHẬT DỮ LIỆU SẢN PHẨM
                    existingSP.TenSP = sp.TenSP;
                    existingSP.MaDM = sp.MaDM;
                    existingSP.GiaBan = sp.GiaBan;
                    existingSP.MoTa = sp.MoTa;
                    existingSP.AnhBia = filename;
                    // LƯU DATABASE
                    db.SaveChanges();

                    TempData["ThongBao"] = "Cập nhật sản phẩm thành công!";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Sản phẩm không tồn tại!";
                    TempData["LoaiThongBao"] = "warning";
                }
            }
            else
            {
                TempData["ThongBao"] = "Cập nhật sản phẩm thất bại!";
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction("QLThucDon", "Home");
        }

        public ActionResult DeleteOnSubmit(int id)
        {
            tblSanPham sp = db.tblSanPham.FirstOrDefault(x => x.MaSP == id);
            if (sp != null)
            {
                db.tblSanPham.Remove(sp);
                db.SaveChanges();

                TempData["ThongBao"] = "Xóa sản phẩm thành công!";
                TempData["LoaiThongBao"] = "success";
            }
            else
            {
                TempData["ThongBao"] = "Sản phẩm không tồn tại!";
                TempData["LoaiThongBao"] = "warning";
            }
            return RedirectToAction("QLThucDon", "Home"); ;
        }

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

            return RedirectToAction("History");
        }

        public ActionResult QLDonHang()
        {
            var orders = db.tblHoaDon.Include("tblKhachHang").OrderByDescending(x => x.NgayLap);
            return View(orders);
        }

        [HttpPost]
        public ActionResult UpdateOrderStatus(int MaHD, int TinhTrang)
        {
            var order = db.tblHoaDon.FirstOrDefault(x => x.MaHD == MaHD);
            if (order != null)
            {
                order.TinhTrang = TinhTrang;

                // Nếu trạng thái là "Đã hủy" (3) hoặc "Đã giao" (2), có thể cập nhật thêm logic
                // Ví dụ: Nếu đã giao (2) thì set DaThanhToan = true (tùy nghiệp vụ)
                if (TinhTrang == 2)
                {
                    order.DaThanhToan = true;
                }

                db.SaveChanges();
                TempData["ThongBao"] = "Cập nhật trạng thái đơn hàng #" + MaHD + " thành công!";
                TempData["LoaiThongBao"] = "success";
            }
            else
            {
                TempData["ThongBao"] = "Không tìm thấy đơn hàng!";
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction("QLDonHang");
        }

        public ActionResult ThongKe()
        {
            // 1. Thống kê tổng quan
            // Chỉ tính những đơn hàng có TinhTrang = 2 (Đã giao)
            var donHangThanhCong = db.tblHoaDon.Where(x => x.TinhTrang == 2);

            decimal tongDoanhThu = donHangThanhCong.Any() ? donHangThanhCong.Sum(x => x.TongTien).GetValueOrDefault() : 0;
            int tongDonHang = donHangThanhCong.Count();
            int tongSanPhamBan = 0;

            if (donHangThanhCong.Any())
            {
                // Join với chi tiết hóa đơn để đếm tổng sản phẩm bán ra
                tongSanPhamBan = (from hd in donHangThanhCong
                                  join ct in db.tblChiTietHoaDon on hd.MaHD equals ct.MaHD
                                  select ct.SoLuong).Sum() ?? 0;
            }

            ViewBag.TongDoanhThu = tongDoanhThu;
            ViewBag.TongDonHang = tongDonHang;
            ViewBag.TongSanPham = tongSanPhamBan;

            // 2. Dữ liệu biểu đồ doanh thu (30 ngày gần nhất - 1 Tháng)
            // Thay đổi từ -6 thành -30 để lấy dữ liệu 1 tháng
            var motThangQua = DateTime.Today.AddDays(-30);
            var dataDoanhThu = db.tblHoaDon
                .Where(x => x.TinhTrang == 2 && x.NgayLap >= motThangQua)
                .ToList() // Thực thi query lấy dữ liệu thô về trước
                .GroupBy(x => x.NgayLap.Value.Date)
                .Select(g => new {
                    Ngay = g.Key.ToString("dd/MM"),
                    DoanhThu = g.Sum(x => x.TongTien)
                })
                .OrderBy(x => x.Ngay)
                .ToList();

            // Chuyển dữ liệu sang dạng List để View dễ dùng
            ViewBag.ChartLabels = dataDoanhThu.Select(x => x.Ngay).ToList();
            ViewBag.ChartValues = dataDoanhThu.Select(x => x.DoanhThu).ToList();

            // 3. Top 5 sản phẩm bán chạy
            // Sử dụng ViewModel TopSanPhamModel để tránh lỗi RuntimeBinderException ở View
            var topSanPham = (from ct in db.tblChiTietHoaDon
                              join sp in db.tblSanPham on ct.MaSP equals sp.MaSP
                              join hd in db.tblHoaDon on ct.MaHD equals hd.MaHD
                              where hd.TinhTrang == 2 // Chỉ tính đơn thành công
                              group ct by new { sp.TenSP, sp.AnhBia, sp.GiaBan } into g
                              orderby g.Sum(x => x.SoLuong) descending
                              select new TopSanPhamModel
                              {
                                  TenSP = g.Key.TenSP,
                                  AnhBia = g.Key.AnhBia,
                                  GiaBan = g.Key.GiaBan,
                                  DaBan = g.Sum(x => x.SoLuong),
                                  TongTien = g.Sum(x => x.SoLuong * x.GiaBan)
                              }).Take(5).ToList();

            ViewBag.TopSanPham = topSanPham;

            return View();
        }
    }
}
