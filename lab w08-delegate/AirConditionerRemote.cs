/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

using DeviceManagement.Models;

namespace DeviceManagement.Devices
{
    /// <summary>
    /// Thiết bị điều khiển điều hòa.
    /// </summary>
    public class AirConditionerRemote : TeachingDevice
    {
        public string AssignedRoom { get; }
        public bool IsResponsive { get; private set; } = true;

        public AirConditionerRemote(string id, string name, string assignedRoom) : base(id, name)
        {
            AssignedRoom = assignedRoom;
        }

        public void UpdateResponseState(bool isResponsive)
        {
            // Chuyển từ phản hồi sang không phản hồi -> Critical
            if (IsResponsive && !isResponsive)
            {
                IsResponsive = false;
                RaiseAlert($"Điều hòa phòng {AssignedRoom} mất kết nối/không phản hồi.", AlertSeverity.Critical);
            }
            else if (!IsResponsive && isResponsive)
            {
                IsResponsive = true;
                RaiseAlert($"Điều hòa phòng {AssignedRoom} đã kết nối trở lại.", AlertSeverity.Information);
            }
        }
    }
}