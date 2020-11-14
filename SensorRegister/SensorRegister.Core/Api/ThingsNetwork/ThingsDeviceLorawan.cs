using System;
using System.Text.Json.Serialization;

namespace SensorRegister.Core.Api.ThingsNetwork
{
    public class ThingsDeviceLorawan
    {

        [JsonPropertyName("app_id")]
        public string AppId { get; set; }

        [JsonPropertyName("app_eui")]
        public string AppEUI { get; set; }

        [JsonPropertyName("dev_id")]
        public string DeviceId { get; set; }

        [JsonPropertyName("dev_eui")]
        public string DeviceEUI { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("activation_constraints")]
        public string ActivationConstraints { get; }  = "testing"; //OTAA for real


        public ThingsDeviceLorawan()
        {
        }
    }
}
