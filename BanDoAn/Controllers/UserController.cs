using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoAn.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        QL_SanPhamEntities db = new QL_SanPhamEntities();
        public ActionResult optLogin()
        {
            return PartialView();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginOnSubmit(FormCollection collect)
        {
            var tk = collect["Username"];
            var mk = collect["Password"];

            var user = db.tbltaikhoan.FirstOrDefault(u => u.username == tk && u.matkhau == mk);

            if(user != null)
            {
                TempData["ThongBao"] = "Đăng nhập thành công!";
                TempData["LoaiThongBao"] = "success";

                Session["User"] = user;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ThongBao"] = "Sai tài khoản hoặc mật khẩu!";
                TempData["LoaiThongBao"] = "warning";
                return RedirectToAction("Login");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult RegisterOnSubmit(FormCollection collect)
        {
            var tenkh = collect["Fullname"];
            var gt = collect["GioiTinh"];
            var namsinh = Convert.ToInt32(collect["NamSinh"]);
            var email = collect["Email"];
            var sdt = collect["SoDienThoai"];
            var diachi = collect["DiaChi"];
            var mk1 = collect["Pass"];
            var mk2 = collect["RePass"];

            if(mk1 != mk2)
            {
                TempData["ThongBao"] = "Mật khẩu không khớp nhau!";
                TempData["LoaiThongBao"] = "error";

                return RedirectToAction("Register");
            }

            var existingUser = db.tbltaikhoan.FirstOrDefault(u => u.username == email);
            if(existingUser != null)
            {
                TempData["ThongBao"] = "Tài khoản này đã tồn tại!";
                TempData["LoaiThongBao"] = "warning";

                return RedirectToAction("Register");
            }

            tblKhachHang newCustomer = new tblKhachHang();
            newCustomer.TenKH = tenkh;
            newCustomer.GioiTinh = gt;
            newCustomer.NamSinh = namsinh;
            newCustomer.Email = email;
            newCustomer.SoDienThoai = sdt;
            newCustomer.DiaChi = diachi;
            db.tblKhachHang.Add(newCustomer);
            db.SaveChanges();

            tbltaikhoan newUser = new tbltaikhoan();
            newUser.username = email;
            newUser.matkhau = mk1;
            newUser.makh = newCustomer.MaKH;
            newUser.mavaitro = 3;
            db.tbltaikhoan.Add(newUser);
            db.SaveChanges();

            var user = db.tbltaikhoan.Include("tblKhachHang").Include("tblVaiTro").FirstOrDefault(u => u.username == email);

            Session["User"] = user;

            TempData["ThongBao"] = "Đăng ký tài khoản thành công!";
            TempData["LoaiThongBao"] = "success";

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            TempData["ThongBao"] = "Đăng xuất thành công!";
            TempData["LoaiThongBao"] = "success";

            Session["User"] = null;
            Session["Cart"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}