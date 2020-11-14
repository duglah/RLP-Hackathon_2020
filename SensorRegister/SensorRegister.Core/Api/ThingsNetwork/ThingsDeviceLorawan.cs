using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SensorRegister.Core.Api.ThingsNetwork
{
    public class ThingsDeviceLorawan
    {

        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("app_eui")]
        public string AppEUI { get; set; }

        [JsonProperty("dev_id")]
        public string DeviceId { get; set; }

        [JsonProperty("dev_eui")]
        public string DeviceEUI { get; set; }

        [JsonProperty("activation_constraints")]
        public string ActivationConstraints { get; }  = "OTAA";//"testing"; //OTAA for real


        public ThingsDeviceLorawan()
        {
        }
    }
}
