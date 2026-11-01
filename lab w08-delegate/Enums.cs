/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

namespace DeviceManagement.Models
{
    /// <summary>
    /// Trạng thái hoạt động của thiết bị dạy học.
    /// </summary>
    public enum DeviceStatus
    {
        Available,
        Borrowed,
        UnderMaintenance
    }

    /// <summary>
    /// Mức độ nghiêm trọng của cảnh báo.
    /// </summary>
    public enum AlertSeverity
    {
        Information,
        Warning,
        Critical
    }
}