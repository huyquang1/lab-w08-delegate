/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

using DeviceManagement.Models;
using DeviceManagement.Exceptions;

namespace DeviceManagement.Devices
{
    /// <summary>
    /// Thiết bị điều khiển máy chiếu.
    /// </summary>
    public class ProjectorRemote : TeachingDevice
    {
        public int BatteryLevel { get; private set; } = 100;

        public ProjectorRemote(string id, string name) : base(id, name) { }

        public void UpdateBatteryLevel(int newLevel)
        {
            if (newLevel < 0 || newLevel > 100)
            {
                throw new InvalidDeviceReadingException(Id, newLevel, $"Mức pin máy chiếu không hợp lệ: {newLevel}%");
            }

            int oldLevel = BatteryLevel;
            BatteryLevel = newLevel;

            // Quy tắc xử lý mức pin
            if (BatteryLevel == 0 && oldLevel != 0)
            {
                RaiseAlert("Máy chiếu hết pin hoàn toàn (0%).", AlertSeverity.Critical);
            }
            else if (BatteryLevel < 20 && oldLevel >= 20)
            {
                RaiseAlert($"Mức pin máy chiếu xuống thấp ({BatteryLevel}%).", AlertSeverity.Warning);
            }
        }
    }
}