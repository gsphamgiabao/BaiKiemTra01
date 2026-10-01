using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSpeedLogistics
{
    public class QuanLyPhuongTien
    {
        private readonly List<PhuongTien> _items = new List<PhuongTien>();

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt == null) throw new ArgumentNullException(nameof(pt));
            _items.Add(pt);
        }

        public void DisplayAll()
        {
            foreach (var pt in _items)
            {
                Console.WriteLine($"{pt.GetInfo()}, GiaLanBanh: {pt.TinhGiaLanBanh():N0} VND");
            }
        }

        public PhuongTien FindMaxGiaLanBanh()
        {
            return _items.OrderByDescending(p => p.TinhGiaLanBanh()).FirstOrDefault();
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<PhuongTien>();
            return _items.Where(p => p.TenHang.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }
    }
}
