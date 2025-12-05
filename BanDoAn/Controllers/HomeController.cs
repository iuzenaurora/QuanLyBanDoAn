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
    }
}
