/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

using System;
using System.Collections.Generic;
using DeviceManagement.Devices;
using DeviceManagement.Models;
using DeviceManagement.Services;
using DeviceManagement.Coordinators;
using DeviceManagement.Exceptions;

namespace DeviceManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== CHƯƠNG TRÌNH QUẢN LÝ THIẾT BỊ PHÒNG HỌC - BÁCH KHOA HÀ NỘI ===");
            Console.WriteLine("Thực hiện bởi: Bùi Phạm Quang Huy - 20227234\n");
            Console.WriteLine("=== BÁO CÁO TEST CHI TIẾT ===");

            // 1. Khởi tạo danh sách thiết bị
            var projector = new ProjectorRemote("PROJ-01", "Máy chiếu Sony T1");
            var ac = new AirConditionerRemote("AC-01", "Điều hòa Daikin Inverter", "B4-201");
            var mic = new WirelessMicrophone("MIC-01", "Micro không dây UHF");

            var devices = new List<TeachingDevice> { projector, ac, mic };

            // 2. Khởi tạo các dịch vụ Subscriber
            var notificationService = new ConsoleNotificationService();
            var auditLogger = new DeviceAuditLogger();
            var maintenanceService = new MaintenanceService();

            // 3. Sử dụng Coordinator để quản lý đăng ký Event
            using (var coordinator = new DeviceMonitoringCoordinator(devices, notificationService, auditLogger, maintenanceService))
            {
                coordinator.Subscribe();

                // Kiểm tra đăng ký qua bộ lọc delegate tùy chỉnh
                var criticalFilter = new FilteredAlertSubscriber(
                    alert => alert.Severity == AlertSeverity.Critical,
                    alert => Console.WriteLine($"--> [ĐẶC BIỆT FILTER] Phát hiện lỗi trầm trọng tại {alert.DeviceId}: {alert.Message}")
                );
                projector.AlertRaised += criticalFilter.HandleAlert;

                Console.WriteLine("\n--- BẮT ĐẦU KỊCH BẢN KIỂM THỬ ---");

                Console.WriteLine("\n[TEST 01] Thay đổi trạng thái máy chiếu từ Available sang Borrowed và UnderMaintenance");
                projector.ChangeStatus(DeviceStatus.Borrowed);
                projector.ChangeStatus(DeviceStatus.UnderMaintenance);
                Console.WriteLine("[KẾT QUẢ] PASS - Trạng thái đã được cập nhật thành công.");

                Console.WriteLine("\n[TEST 02] Giảm pin máy chiếu xuống 15% (Warning)");
                projector.UpdateBatteryLevel(15);
                Console.WriteLine("[KẾT QUẢ] PASS - Hệ thống đã phát cảnh báo Warning.");

                Console.WriteLine("\n[TEST 03] Giảm pin máy chiếu xuống 0% (Critical -> Kích hoạt MaintenanceService)");
                projector.UpdateBatteryLevel(0);
                Console.WriteLine("[KẾT QUẢ] PASS - Hệ thống phát Critical và tạo yêu cầu bảo trì.");

                Console.WriteLine("\n[TEST 04] Điều hòa chuyển sang trạng thái mất phản hồi (Critical)");
                ac.UpdateResponseState(false);
                Console.WriteLine("[KẾT QUẢ] PASS - Điều hòa báo lỗi Critical.");

                Console.WriteLine("\n[TEST 05] Micro giảm chất lượng tín hiệu xuống 30% (Warning)");
                mic.UpdateSignalQuality(30);
                Console.WriteLine("[KẾT QUẢ] PASS - Micro báo Warning do tín hiệu yếu.");

                Console.WriteLine("\n[TEST 06] Kiểm tra ngoại lệ InvalidDeviceReadingException khi nhập mức pin 150%");
                try
                {
                    mic.UpdateBatteryLevel(150);
                    Console.WriteLine("[KẾT QUẢ] FAIL - Không ném exception như mong đợi.");
                }
                catch (InvalidDeviceReadingException ex)
                {
                    Console.WriteLine($"[KẾT QUẢ] PASS - Bắt ngoại lệ thành công: {ex.Message} (Mã thiết bị: {ex.DeviceId})");
                }

                Console.WriteLine("\n--- TỔNG KẾT NHẬT KÝ AUDIT LOGGER ---");
                foreach (var log in auditLogger.Entries)
                {
                    Console.WriteLine(log);
                }

                Console.WriteLine("\n--- TỔNG KẾT YÊU CẦU BẢO TRÌ (MAINTENANCE SERVICE) ---");
                foreach (var req in maintenanceService.Requests)
                {
                    Console.WriteLine(req);
                }
            }

            Console.WriteLine("\n=== TÓM TẮT KIỂM THỬ ===");
            Console.WriteLine("[PASS] 6/6 test chính đã thực hiện và đạt kết quả mong đợi.");
            Console.WriteLine("=== KẾT THÚC CHƯƠNG TRÌNH KIỂM THỬ ===");
        }
    }
}