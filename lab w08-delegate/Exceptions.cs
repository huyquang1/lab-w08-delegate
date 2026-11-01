/********************************/
// Mã sinh viên: 20227234
// Họ tên: Bùi Phạm Quang Huy
/********************************/

using System;

namespace DeviceManagement.Exceptions
{
    /// <summary>
    /// Ngoại lệ phát sinh khi đọc giá trị thông số thiết bị không hợp lệ (ngoài khoảng 0-100).
    /// </summary>
    public class InvalidDeviceReadingException : Exception
    {
        public string DeviceId { get; }
        public int InvalidValue { get; }

        public InvalidDeviceReadingException(string deviceId, int invalidValue, string message) 
            : base(message)
        {
            DeviceId = deviceId;
            InvalidValue = invalidValue;
        }
    }
}