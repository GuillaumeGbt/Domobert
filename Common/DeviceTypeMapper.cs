using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DeviceTypeMapper
    {
        public static string ToStringType(this eDeviceType deviceType)
        {
            return deviceType switch
            {
                eDeviceType.LIGHT => "Light",
                eDeviceType.HEATER => "Heater",
                eDeviceType.CAMERA => "Camera",
                eDeviceType.ALARM => "Alarm",
                eDeviceType.SOLAR => "Solar",
                eDeviceType.TEMP_HUMI => "TempHumi",
                _ => throw new ArgumentException("Invalid device type", nameof(deviceType))
            };
        }

        public static eDeviceType ToEDeviceType(this string deviceTypeString)
        {
            return deviceTypeString.ToLower() switch
            {
                "light" => eDeviceType.LIGHT,
                "heater" => eDeviceType.HEATER,
                "camera" => eDeviceType.CAMERA,
                "alarm" => eDeviceType.ALARM,
                "solar" => eDeviceType.SOLAR,
                "temphumi" => eDeviceType.TEMP_HUMI,
                _ => throw new ArgumentException("Invalid device type string", nameof(deviceTypeString))
            };
        }
    }
}
