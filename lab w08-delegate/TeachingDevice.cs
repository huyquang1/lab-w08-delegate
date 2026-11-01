/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

using System;
using DeviceManagement.Models;
using DeviceManagement.Events;

namespace DeviceManagement.Devices
{
    /// <summary>
    /// Lớp trừu tượng đại diện cho một thiết bị dạy học trong phòng học.
    /// </summary>
    public abstract class TeachingDevice
    {
        public string Id { get; }
        public string Name { get; }
        public DeviceStatus Status { get; private set; }

        public event EventHandler<DeviceStatusChangedEventArgs>? StatusChanged;
        public event EventHandler<DeviceAlertEventArgs>? AlertRaised;

        protected TeachingDevice(string id, string name)
        {
            Id = id;
            Name = name;
            Status = DeviceStatus.Available;
        }

        public void ChangeStatus(DeviceStatus newStatus)
        {
            if (newStatus == Status)
            {
                return;
            }

            DeviceStatus oldStatus = Status;
            Status = newStatus;

            OnStatusChanged(new DeviceStatusChangedEventArgs(Id, oldStatus, newStatus, DateTime.Now));
        }

        protected void RaiseAlert(string message, AlertSeverity severity)
        {
            OnAlertRaised(new DeviceAlertEventArgs(Id, message, severity, DateTime.Now));
        }

        protected virtual void OnStatusChanged(DeviceStatusChangedEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        protected virtual void OnAlertRaised(DeviceAlertEventArgs e)
        {
            AlertRaised?.Invoke(this, e);
        }
    }
}