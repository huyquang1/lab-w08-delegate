/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

using System;
using System.Collections.Generic;
using DeviceManagement.Events;
using DeviceManagement.Models;

namespace DeviceManagement.Services
{
    /// <summary>
    /// Dịch vụ hiển thị thông báo ra màn hình console.
    /// </summary>
    public class ConsoleNotificationService
    {
        public void HandleAlert(object? sender, DeviceAlertEventArgs e)
        {
            Console.WriteLine($"[THÔNG BÁO - {e.Severity}] Thiết bị {e.DeviceId}: {e.Message} (Vào lúc: {e.OccurredAt:HH:mm:ss})");
        }
    }

    /// <summary>
    /// Dịch vụ ghi log lịch sử thay đổi trạng thái thiết bị.
    /// </summary>
    public class DeviceAuditLogger
    {
        private readonly List<string> entries = new();
        public IReadOnlyList<string> Entries => entries.AsReadOnly();

        public void HandleStatusChanged(object? sender, DeviceStatusChangedEventArgs e)
        {
            string log = $"{e.ChangedAt:yyyy-MM-dd HH:mm:ss} | Thiết bị {e.DeviceId}: {e.OldStatus} -> {e.NewStatus}";
            entries.Add(log);
        }
    }

    /// <summary>
    /// Dịch vụ tiếp nhận sự cố và tạo yêu cầu bảo trì khi có lỗi Critical.
    /// </summary>
    public class MaintenanceService
    {
        private readonly List<string> requests = new();
        public IReadOnlyList<string> Requests => requests.AsReadOnly();

        public void HandleCriticalAlert(object? sender, DeviceAlertEventArgs e)
        {
            if (e.Severity != AlertSeverity.Critical)
            {
                return;
            }

            string request = $"[YÊU CẦU BẢO TRÌ KHẩn CẤP] Thiết bị {e.DeviceId}: {e.Message}";
            requests.Add(request);
        }
    }

    /// <summary>
    /// Bộ lọc thông báo linh hoạt sử dụng delegate tự định nghĩa.
    /// </summary>
    public delegate bool AlertFilter(DeviceAlertEventArgs alert);

    public class FilteredAlertSubscriber
    {
        private readonly AlertFilter filter;
        private readonly Action<DeviceAlertEventArgs> action;

        public FilteredAlertSubscriber(AlertFilter filter, Action<DeviceAlertEventArgs> action)
        {
            this.filter = filter;
            this.action = action;
        }

        public void HandleAlert(object? sender, DeviceAlertEventArgs e)
        {
            if (filter(e))
            {
                action(e);
            }
        }
    }
}