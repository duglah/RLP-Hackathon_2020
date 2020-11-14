using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SensorRegister.Core.Api.ThingsNetwork
{
    public class ThingsDeviceModel
    {
        [JsonPropertyName("app_id")]
        public string AppId { get; set; }

        [JsonPropertyName("dev_id")]
        public string DeviceId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("lorawan_device")]
        public ThingsDeviceLorawan Device { get; set; }

        public ThingsDeviceModel()
        {
        }
    }
}
