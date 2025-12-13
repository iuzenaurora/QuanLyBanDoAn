using BanDoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BanDoAn.Controllers
{
    [RoutePrefix("api/APIFood")]
    public class APIFoodController : ApiController
    {
        QL_SanPhamEntities db = new QL_SanPhamEntities();

        // GET: Danh sách sản phẩm
        [HttpGet]
        [Route("GetSanPham")]
        public IHttpActionResult GetSanPham()
        {
            var danhSach = db.tblSanPham.ToList();
            return Ok(danhSach);
        }

        // GET: Danh sách loại
        [HttpGet]
        [Route("GetLoaiMonAn")]
        public IHttpActionResult GetLoaiMonAn()
        {
            var danhSach = db.tblDanhMucCon.ToList();
            return Ok(danhSach);
        }

        // POST: Thêm loại món ăn
        [HttpPost]
        [Route("ThemLoaiMonAn")]
        public IHttpActionResult ThemLoaiMonAn(tblDanhMucCon data) { 
            try { 
                if (db.tblDanhMucCon.Any(x => x.MaDM == data.MaDM)) 
                { 
                    return BadRequest("Mã loại này đã tồn tại");
                } 
                if (ModelState.IsValid) 
                { 
                    db.tblDanhMucCon.Add(data); 
                    db.SaveChanges(); 
                        return Ok("Thêm loại sản phẩm thành công"); 
                } 
                else 
                { 
                    return BadRequest(ModelState); 
                } 
            } 
            catch (Exception ex) 
            { 
                return InternalServerError(ex); 
            } 
        }

        // PUT: Sửa loại món ăn
        [HttpPut]
        [Route("SuaLoaiMonAn")]
        public IHttpActionResult SuaLoaiMonAn(tblDanhMucCon data) 
        { 
            try 
            { 
                var loaiMonAn = db.tblDanhMucCon.FirstOrDefault(x => x.MaDM == data.MaDM); 
                if (loaiMonAn == null) 
                { 
                    return NotFound(); 
                } 
                if (ModelState.IsValid) 
                {
                    loaiMonAn.TenDM = data.TenDM;
                    loaiMonAn.MaLoai = data.MaLoai;
                    db.SaveChanges(); return Ok("Cập nhật loại sản phẩm thành công"); 
                } 
                else 
                { 
                    return BadRequest(ModelState); 
                } 
            } 
            catch (Exception ex) { 
                return InternalServerError(ex); 
            } 
        }

        // DELETE: Xóa loại món ăn
        [HttpDelete]
        [Route("XoaLoaiMonAn/{maloaimonan}")]
        public IHttpActionResult XoaLoaiMonAn(int maloaimonan) 
        { 
            try 
            { 
                var loaiSP = db.tblDanhMucCon.FirstOrDefault(x => x.MaDM == maloaimonan); 
                if (loaiSP == null) 
                { 
                    return NotFound(); 
                } 
                if (ModelState.IsValid) 
                { 
                    db.tblDanhMucCon.Remove(loaiSP); 
                    db.SaveChanges(); 
                    return Ok("Xóa loại sản phẩm thành công"); 
                } 
                else 
                { 
                    return BadRequest(ModelState); 
                } 
            } 
            catch (Exception ex) { 
                return InternalServerError(ex); 
            } 
        }

        [HttpGet]
        [Route("GetDonHang")]
        public IHttpActionResult GetDonHang()
        {
            var dshoadon = db.tblHoaDon.ToList();
            return Ok(dshoadon);
        }

        // PUT: Cập nhật đơn hàng
        [HttpPut]
        [Route("CapNhatDonHang")]
        public IHttpActionResult CapNhatDonHang(tblHoaDon data)
        {
            try
            {
                var hoaDon = db.tblHoaDon.FirstOrDefault(x => x.MaHD == data.MaHD);

                if (hoaDon == null)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    hoaDon.MaKH = data.MaKH;
                    hoaDon.MaNV = data.MaNV;
                    hoaDon.NgayLap = data.NgayLap;
                    hoaDon.TongTien = data.TongTien;
                    hoaDon.TinhTrang = data.TinhTrang;
                    hoaDon.DiaChiGiaoHang = data.DiaChiGiaoHang;
                    hoaDon.DaThanhToan = data.DaThanhToan;

                    db.SaveChanges();

                    return Ok("Cập nhật đơn hàng thành công");
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

}
