using BanDoAn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoAn.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        QL_SanPhamEntities db = new QL_SanPhamEntities();
        public ActionResult QuanTri()
        {
            // Lấy Top 5 món bán chạy nhất hệ thống (Tính trên các đơn đã hoàn tất)
            var topSanPham = (from ct in db.tblChiTietHoaDon
                              join sp in db.tblSanPham on ct.MaSP equals sp.MaSP
                              join hd in db.tblHoaDon on ct.MaHD equals hd.MaHD
                              where hd.TinhTrang == 2 // Chỉ tính đơn thành công
                              group ct by new { sp.TenSP, sp.AnhBia, sp.GiaBan } into g
                              orderby g.Sum(x => x.SoLuong) descending
                              select new TopSanPhamModel // Sử dụng Model đã tạo
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

        //===================================================================
        //                     Quản lý sản phẩm 
        //===================================================================
        public ActionResult QLThucDon(int[] selectedLoai)
        {
            ViewBag.ListLoai = db.tblLoaiSP.ToList();
            var query = db.tblSanPham.Include("tblDanhMucCon").AsQueryable();

            if (selectedLoai != null && selectedLoai.Length > 0)
            {
                query = query.Where(x => x.tblDanhMucCon != null && selectedLoai.Contains(x.tblDanhMucCon.MaLoai.Value));
                ViewBag.SelectedLoai = selectedLoai;
            }

            return View(query.OrderByDescending(x => x.MaSP).ToList());
        }

        public ActionResult Create()
        {
            ViewBag.ListDanhMuc = db.tblDanhMucCon.Include("tblLoaiSP").ToList();
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateOnSubmit(tblSanPham sp, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                string Dir = "~/Content/images";
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    filename = Path.GetFileName(fileUpload.FileName);
                    string physicalDir = Server.MapPath(Dir);
                    if (!Directory.Exists(physicalDir)) Directory.CreateDirectory(physicalDir);
                    fileUpload.SaveAs(Path.Combine(physicalDir, filename));
                }

                tblSanPham newSP = new tblSanPham();
                newSP.TenSP = sp.TenSP;
                newSP.MaDM = sp.MaDM;
                newSP.GiaBan = sp.GiaBan;
                newSP.MoTa = sp.MoTa;
                newSP.AnhBia = filename;

                db.tblSanPham.Add(newSP);
                db.SaveChanges();
                TempData["ThongBao"] = "Thêm sản phẩm thành công!";
                TempData["LoaiThongBao"] = "success";
            }
            return RedirectToAction("QLThucDon");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ListDanhMuc = db.tblDanhMucCon.Include("tblLoaiSP").ToList();
            tblSanPham sp = db.tblSanPham.FirstOrDefault(x => x.MaSP == id);
            if (sp != null)
            {
                return PartialView(sp);
            }
            TempData["ThongBao"] = "Sản phẩm không tồn tại!";
            TempData["LoaiThongBao"] = "warning";
            return RedirectToAction("QLThucDon");
        }

        public ActionResult EditOnSubmit(tblSanPham sp, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                tblSanPham existingSP = db.tblSanPham.FirstOrDefault(x => x.MaSP == sp.MaSP);
                if (existingSP != null)
                {
                    string filename = existingSP.AnhBia;
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        string Dir = "~/Content/images";
                        filename = Path.GetFileName(fileUpload.FileName);
                        string physicalDir = Server.MapPath(Dir);
                        if (!Directory.Exists(physicalDir)) Directory.CreateDirectory(physicalDir);
                        fileUpload.SaveAs(Path.Combine(physicalDir, filename));
                    }
                    existingSP.TenSP = sp.TenSP;
                    existingSP.MaDM = sp.MaDM;
                    existingSP.GiaBan = sp.GiaBan;
                    existingSP.MoTa = sp.MoTa;
                    existingSP.AnhBia = filename;
                    db.SaveChanges();
                    TempData["ThongBao"] = "Cập nhật thành công!";
                    TempData["LoaiThongBao"] = "success";
                }
            }
            return RedirectToAction("QLThucDon");
        }

        public ActionResult Delete(int id)
        {
            tblSanPham item = db.tblSanPham.FirstOrDefault(x => x.MaSP == id);
            return View(item);
        }

        // POST: Product/DeleteOnSubmit
        [HttpPost] // Đảm bảo chỉ thực hiện POST cho việc xóa
        public ActionResult DeleteOnSubmit(int id)
        {
            tblSanPham sp = db.tblSanPham.FirstOrDefault(x => x.MaSP == id);
            if (sp != null)
            {
                db.tblSanPham.Remove(sp);
                db.SaveChanges();
                TempData["ThongBao"] = "Xóa thành công!";
                TempData["LoaiThongBao"] = "success";
                return RedirectToAction("QLThucDon");
            }
            return RedirectToAction("Delete");
        }

        //==============================================================
        //                      Quản lý đơn hàng
        //==============================================================
        public ActionResult QLDonHang(int? statusFilter)
        {
            var query = db.tblHoaDon.Include("tblKhachHang").AsQueryable();
            if (statusFilter.HasValue)
            {
                query = query.Where(x => x.TinhTrang == statusFilter.Value);
                ViewBag.CurrentStatus = statusFilter.Value;
            }
            var orders = query.OrderByDescending(x => x.NgayLap).ToList();
            return View(orders);
        }

        public ActionResult GetOrderDetails(int id)
        {
            // Lấy danh sách chi tiết hóa đơn, bao gồm thông tin sản phẩm
            var details = db.tblChiTietHoaDon.Include("tblSanPham")
                                             .Where(x => x.MaHD == id)
                                             .ToList();
            return PartialView("OrderDetails", details);
        }

        [HttpPost]
        public ActionResult UpdateOrderStatus(int MaHD, int TinhTrang)
        {
            var order = db.tblHoaDon.FirstOrDefault(x => x.MaHD == MaHD);
            if (order != null)
            {
                order.TinhTrang = TinhTrang;
                if (TinhTrang == 2) order.DaThanhToan = true;
                db.SaveChanges();
                TempData["ThongBao"] = "Cập nhật đơn hàng #" + MaHD + " thành công!";
                TempData["LoaiThongBao"] = "success";
            }
            return RedirectToAction("QLDonHang");
        }

        //=======================================================================
        //                             Thống kê
        //=======================================================================
        public ActionResult ThongKe()
        {
            // --- 1. SỐ LIỆU TỔNG QUAN (INFO CARDS) ---
            var donHangThanhCong = db.tblHoaDon.Where(x => x.TinhTrang == 2);
            decimal tongDoanhThu = donHangThanhCong.Any() ? donHangThanhCong.Sum(x => x.TongTien).GetValueOrDefault() : 0;
            int tongKhachHang = db.tblKhachHang.Count();
            int tongSanPham = db.tblSanPham.Count();

            int tongSoLuongBan = 0;
            if (donHangThanhCong.Any())
            {
                tongSoLuongBan = (from hd in donHangThanhCong
                                  join ct in db.tblChiTietHoaDon on hd.MaHD equals ct.MaHD
                                  select ct.SoLuong).Sum() ?? 0;
            }

            ViewBag.TongDoanhThu = tongDoanhThu;
            ViewBag.TongKhachHang = tongKhachHang;
            ViewBag.TongSanPham = tongSanPham;
            ViewBag.TongSoLuongBan = tongSoLuongBan;

            var namHienTai = DateTime.Now.Year;

            // --- 2. BIỂU ĐỒ DOANH SỐ HÀNG THÁNG (LINE CHART) ---
            var doanhThuThang = db.tblHoaDon
                .Where(x => x.TinhTrang == 2 && x.NgayLap.HasValue && x.NgayLap.Value.Year == namHienTai)
                .GroupBy(x => x.NgayLap.Value.Month)
                .Select(g => new { Thang = g.Key, DoanhThu = g.Sum(x => x.TongTien) })
                .OrderBy(x => x.Thang)
                .ToList();

            decimal[] monthlyData = new decimal[12];
            foreach (var item in doanhThuThang) monthlyData[item.Thang - 1] = item.DoanhThu ?? 0;

            ViewBag.MonthlySalesData = monthlyData;
            ViewBag.MonthlySalesLabels = new string[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" };

            // --- 3. BIỂU ĐỒ HOẠT ĐỘNG KHÁCH HÀNG (LINE CHART) ---
            var usersActivity = db.tblHoaDon
                .Where(x => x.TinhTrang == 2 && x.NgayLap.HasValue && x.NgayLap.Value.Year == namHienTai)
                .GroupBy(x => x.NgayLap.Value.Month)
                .Select(g => new { Thang = g.Key, SoKhach = g.Select(x => x.MaKH).Distinct().Count() })
                .ToList();

            int[] userActiveData = new int[12];
            foreach (var item in usersActivity) userActiveData[item.Thang - 1] = item.SoKhach;

            ViewBag.UserActiveData = userActiveData;


            // --- 4. BIỂU ĐỒ DOANH THU 7 NGÀY GẦN NHẤT (BAR CHART - Thay thế Top món bán chạy) ---
            // Y: Giá tiền, X: Ngày tháng
            DateTime today = DateTime.Today;
            DateTime sevenDaysAgo = today.AddDays(-6);

            var rawData = db.tblHoaDon
                .Where(x => x.TinhTrang == 2 && x.NgayLap >= sevenDaysAgo)
                .Select(x => new { x.NgayLap, x.TongTien })
                .ToList();

            List<string> weeklyLabels = new List<string>();
            List<decimal> weeklyValues = new List<decimal>();

            for (int i = 0; i < 7; i++)
            {
                DateTime date = sevenDaysAgo.AddDays(i);
                string labelDate = date.ToString("dd/MM");

                decimal dailyRevenue = rawData
                    .Where(x => x.NgayLap.HasValue && x.NgayLap.Value.Date == date)
                    .Sum(x => x.TongTien) ?? 0;

                weeklyLabels.Add(labelDate);
                weeklyValues.Add(dailyRevenue);
            }

            ViewBag.WeeklyRevenueLabels = weeklyLabels;
            ViewBag.WeeklyRevenueValues = weeklyValues;


            // --- 5. PHÂN BỐ DANH MỤC (DOUGHNUT CHART) ---
            var categoryStats = (from ct in db.tblChiTietHoaDon
                                 join sp in db.tblSanPham on ct.MaSP equals sp.MaSP
                                 join dm in db.tblDanhMucCon on sp.MaDM equals dm.MaDM
                                 join loai in db.tblLoaiSP on dm.MaLoai equals loai.MaLoai
                                 join hd in db.tblHoaDon on ct.MaHD equals hd.MaHD
                                 where hd.TinhTrang == 2
                                 group ct by loai.TenLoai into g
                                 select new { Label = g.Key, Value = g.Sum(x => x.SoLuong) }).ToList();

            ViewBag.CategoryLabels = categoryStats.Select(x => x.Label).ToList();
            ViewBag.CategoryData = categoryStats.Select(x => x.Value).ToList();

            return View();
        }
    }
}