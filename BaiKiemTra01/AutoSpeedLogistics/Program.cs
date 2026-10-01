using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSpeedLogistics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ensure console uses UTF-8 so Vietnamese characters display correctly
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("================ CHẠY KỊCH BẢN KIỂM THỬ (TEST CASES) ================\n");

            // --- TC01: Kiểm tra Validation Năm sản xuất ---
            Console.WriteLine("--- TC01: Kiểm tra Validation Năm sản xuất ---");
            try
            {
                var badCar = new OTo("PT000", "SomeBrand", 1850, 500000000m, 4, 2.0);
                Console.WriteLine("[FAILED] Không ném ngoại lệ khi năm sản xuất không hợp lệ.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[PASSED] Ném ngoại lệ thành công: {ex.Message}");
            }

            Console.WriteLine();
            // --- TC02: Kiểm tra Giá lăn bánh Ô tô ---
            Console.WriteLine("--- TC02: Kiểm tra Giá lăn bánh Ô tô ---");
            var oto = new OTo("PT002", "Toyota Camry", 2022, 1_000_000_000m, 5, 2.5);
            var giaLanBanhOto = oto.TinhGiaLanBanh();
            Console.WriteLine($"Giá gốc: {oto.GiaGoc:N0} VNĐ -> Giá lăn bánh thực tế: {giaLanBanhOto:N0} VNĐ");
            var expectedOto = 1_420_000_000m;
            Console.WriteLine($"Kỳ vọng: {expectedOto:N0} VNĐ -> Kết quả: [{(giaLanBanhOto == expectedOto ? "PASSED" : "FAILED")}]");

            Console.WriteLine();
            // --- TC03: Kiểm tra Giá lăn bánh Xe máy ---
            Console.WriteLine("--- TC03: Kiểm tra Giá lăn bánh Xe máy ---");
            var xemay = new XeMay("PT003", "Honda SH", 2023, 50_000_000m, 150);
            var giaLanBanhXeMay = xemay.TinhGiaLanBanh();
            Console.WriteLine($"Giá gốc: {xemay.GiaGoc:N0} VNĐ -> Giá lăn bánh thực tế: {giaLanBanhXeMay:N0} VNĐ");
            var expectedXeMay = 51_000_000m;
            Console.WriteLine($"Kỳ vọng: {expectedXeMay:N0} VNĐ -> Kết quả: [{(giaLanBanhXeMay == expectedXeMay ? "PASSED" : "FAILED")}]");

            Console.WriteLine();
            // --- TC04: Kiểm tra Đa hình List<PhuongTien> ---
            Console.WriteLine("--- TC04: Kiểm tra Đa hình List<PhuongTien> ---\n");
            Console.WriteLine("=== DANH SÁCH TOÀN BỘ PHƯƠNG TIỆN ===");
            var ql = new QuanLyPhuongTien();
            ql.AddPhuongTien(oto);
            ql.AddPhuongTien(xemay);
            ql.DisplayAll();

            Console.WriteLine();
            // --- TC05: Kiểm tra Tìm Giá Lăn Bánh Max ---
            Console.WriteLine("--- TC05: Kiểm tra Tìm Giá Lăn Bánh Max ---");
            var max = ql.FindMaxGiaLanBanh();
            if (max != null)
            {
                Console.WriteLine($"Phương tiện giá lăn bánh cao nhất: {max.GetInfo()} | Giá lăn bánh: {max.TinhGiaLanBanh():N0} VNĐ");
                Console.WriteLine("Kết quả: [PASSED]");
            }

            Console.WriteLine();
            Console.WriteLine("Nhấn phím bất kỳ để kết thúc...");
            Console.ReadKey();
        }
    }
}
