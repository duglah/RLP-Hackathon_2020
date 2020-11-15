using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SensorRegister.Core.Api.ThingsNetwork
{
    public class ThingsDeviceModel
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("dev_id")]
        public string DeviceId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lorawan_device")]
        public ThingsDeviceLorawan Device { get; set; }

        public ThingsDeviceModel()
        {
        }

        public override string ToString()
        {
            return DeviceId;
        }
    }
}
