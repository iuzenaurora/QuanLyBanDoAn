using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDoAn.Models
{
    public class Cart
    {
        public List<CartItem> list;

        //Ban đầu chưa có sản phẩm trong giỏ hàng
        public Cart()
        {
            list = new List<CartItem>();
        }

        //Trong giỏ hàng đã có sản phẩm
        public Cart(List<CartItem> cartItems)
        {
            list = cartItems;
        }

        public int soLuongMH()
        {
            return list.Count();
        }

        public int tongSLHang()
        {
            return list.Sum(x => x.soluong);
        }

        public decimal tongThanhTien()
        {
            return list.Sum(x => x.thanhtien);
        }

        public int Them(int id)
        {
            var sp = list.Find(x => x.MaItem == id);
            if (sp != null)
            {
                sp.soluong++;
            }
            else
            {
                CartItem newItem = new CartItem(id);
                if (newItem != null)
                {
                    list.Add(newItem);
                }
                else
                {
                    return -1;
                }
            }
            return 1;
        }

        public int Xoa(int id)
        {
            var sp = list.Find(x => x.MaItem == id);
            if (sp != null)
            {
                list.Remove(sp);
                return 1;
            }
            return -1;
        }

        public int Giam(int id)
        {
            var sp = list.Find(x => x.MaItem == id);
            if (sp != null)
            {
                sp.soluong--;
                if (sp.soluong <= 0)
                {
                    list.Remove(sp);
                }
                return 1;
            }
            return -1;
        }
    }
}