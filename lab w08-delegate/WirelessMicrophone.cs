/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

using DeviceManagement.Models;
using DeviceManagement.Exceptions;

namespace DeviceManagement.Devices
{
    /// <summary>
    /// Thiết bị Micro không dây.
    /// </summary>
    public class WirelessMicrophone : TeachingDevice
    {
        public int BatteryLevel { get; private set; } = 100;
        public int SignalQuality { get; private set; } = 100;

        public WirelessMicrophone(string id, string name) : base(id, name) { }

        public void UpdateBatteryLevel(int newLevel)
        {
            if (newLevel < 0 || newLevel > 100)
            {
                throw new InvalidDeviceReadingException(Id, newLevel, $"Mức pin micro không hợp lệ: {newLevel}%");
            }

            int oldLevel = BatteryLevel;
            BatteryLevel = newLevel;

            if (BatteryLevel == 0 && oldLevel != 0)
            {
                RaiseAlert("Micro hết pin hoàn toàn (0%).", AlertSeverity.Critical);
            }
            else if (BatteryLevel < 30 && oldLevel >= 30)
            {
                RaiseAlert($"Mức pin micro xuống thấp ({BatteryLevel}%).", AlertSeverity.Warning);
            }
        }

        public void UpdateSignalQuality(int newQuality)
        {
            if (newQuality < 0 || newQuality > 100)
            {
                throw new InvalidDeviceReadingException(Id, newQuality, $"Chất lượng tín hiệu micro không hợp lệ: {newQuality}%");
            }

            int oldQuality = SignalQuality;
            SignalQuality = newQuality;

            if (SignalQuality == 0 && oldQuality != 0)
            {
                RaiseAlert("Chất lượng tín hiệu micro bằng 0 (mất sóng hoàn toàn).", AlertSeverity.Critical);
            }
            else if (SignalQuality < 40 && oldQuality >= 40)
            {
                RaiseAlert($"Chất lượng tín hiệu micro kém ({SignalQuality}%).", AlertSeverity.Warning);
            }
        }
    }
}