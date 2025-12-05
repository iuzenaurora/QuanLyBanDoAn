using BanDoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BanDoAn.Controllers
{
    public class APIFoodController : ApiController
    {
        QL_SanPhamEntities db = new QL_SanPhamEntities();

        [HttpGet]
        public IHttpActionResult GetLoaiSP()
        {
            var danhSach = db.tblLoaiSP.ToList();
            return Ok(danhSach);
        }

        [HttpPost]
        public IHttpActionResult ThemLoaiSP(tblLoaiSP data)
        {
            try
            {
                if (db.tblLoaiSP.Any(x => x.MaLoai == data.MaLoai))
                {
                    return BadRequest("Mã loại này đã tồn tại");
                }

                if (ModelState.IsValid)
                {
                    db.tblLoaiSP.Add(data);
                    db.SaveChanges();
                    return Ok("Thêm loại sản phẩm thành công");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult SuaLoaiSP(tblLoaiSP data)
        {
            try
            {
                var loaiSP = db.tblLoaiSP.FirstOrDefault(x => x.MaLoai == data.MaLoai);
                if(loaiSP == null)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Ok("Cập nhật loại sản phẩm thành công");
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

        [HttpDelete]
        public IHttpActionResult XoaLoaiSP(int maloai)
        {
            try
            {
                var loaiSP = db.tblLoaiSP.FirstOrDefault(x => x.MaLoai == maloai);
                if(loaiSP == null)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    db.tblLoaiSP.Remove(loaiSP);
                    db.SaveChanges();
                    return Ok("Xóa loại sản phẩm thành công");
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
    }
}
