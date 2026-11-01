/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

using System;
using System.Collections.Generic;
using DeviceManagement.Devices;
using DeviceManagement.Services;

namespace DeviceManagement.Coordinators
{
    /// <summary>
    /// Lớp điều phối đăng ký/hủy đăng ký sự kiện giữa thiết bị và các dịch vụ.
    /// </summary>
    public class DeviceMonitoringCoordinator : IDisposable
    {
        private readonly IEnumerable<TeachingDevice> devices;
        private readonly ConsoleNotificationService notificationService;
        private readonly DeviceAuditLogger auditLogger;
        private readonly MaintenanceService maintenanceService;
        private bool isSubscribed = false;

        public DeviceMonitoringCoordinator(
            IEnumerable<TeachingDevice> devices,
            ConsoleNotificationService notificationService,
            DeviceAuditLogger auditLogger,
            MaintenanceService maintenanceService)
        {
            this.devices = devices;
            this.notificationService = notificationService;
            this.auditLogger = auditLogger;
            this.maintenanceService = maintenanceService;
        }

        public void Subscribe()
        {
            if (isSubscribed) return;

            foreach (var device in devices)
            {
                // Đăng ký tránh trùng lặp bằng cách gỡ trước rồi gán
                device.StatusChanged -= auditLogger.HandleStatusChanged;
                device.StatusChanged += auditLogger.HandleStatusChanged;

                device.AlertRaised -= notificationService.HandleAlert;
                device.AlertRaised += notificationService.HandleAlert;

                device.AlertRaised -= maintenanceService.HandleCriticalAlert;
                device.AlertRaised += maintenanceService.HandleCriticalAlert;
            }

            isSubscribed = true;
        }

        public void Unsubscribe()
        {
            if (!isSubscribed) return;

            foreach (var device in devices)
            {
                device.StatusChanged -= auditLogger.HandleStatusChanged;
                device.AlertRaised -= notificationService.HandleAlert;
                device.AlertRaised -= maintenanceService.HandleCriticalAlert;
            }

            isSubscribed = false;
        }

        public void Dispose()
        {
            Unsubscribe();
            GC.SuppressFinalize(this);
        }
    }
}