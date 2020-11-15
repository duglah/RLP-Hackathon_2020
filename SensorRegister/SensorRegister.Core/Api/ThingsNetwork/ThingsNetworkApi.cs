using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SensorRegister.Core.Api.DataCake;

namespace SensorRegister.Core.Api.ThingsNetwork
{
    public class ThingsNetworkDevicesApi
    {
        public static readonly string HackathonDeviceIdPrefix = "hackathon_";
        public static readonly string app_id = "brickmakers_office_air_quality";
        //TODO: touch SensorRegister.Core/Secrets.cs
        private static readonly string key = Secrets.ThingsNetworkApiKey;

        public static async Task AddDevice(ThingsDeviceModel device)
        {
            if (!device.DeviceId.StartsWith(HackathonDeviceIdPrefix))
                throw new Exception("Device ID must start with "+HackathonDeviceIdPrefix);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", key);
            var json = JsonConvert.SerializeObject(device);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result =
                await client.PostAsync(
                    $"http://eu.thethings.network:8084/applications/brickmakers_office_air_quality/devices",
                    httpContent);

            if (!result.IsSuccessStatusCode)
            {
                var err = await result.Content.ReadAsStringAsync();
                throw new Exception(err);
            }
        }

        public static async Task DeleteDevice(string deviceId)
        {
            if (!deviceId.StartsWith(HackathonDeviceIdPrefix))
                throw new Exception("this is not a hackathon device!");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", key);

            var result =
                await client.DeleteAsync(
                    $"http://eu.thethings.network:8084/applications/brickmakers_office_air_quality/devices/{deviceId}");

            if (!result.IsSuccessStatusCode)
            {
                var err = await result.Content.ReadAsStringAsync();
                throw new Exception(err);
            }
        }

        public static async Task<List<ThingsDeviceModel>> GetDevices()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", key);

                var result = await client.GetAsync(
                    "http://eu.thethings.network:8084/applications/brickmakers_office_air_quality/devices");

                var s = await result.Content.ReadAsStringAsync();
                if (!result.IsSuccessStatusCode) throw new Exception(s);
                var devices = JsonConvert.DeserializeObject<GetDevicesResponse>(s).Devices;

                return devices.Where(dev => dev.DeviceId.StartsWith(ThingsNetworkDevicesApi.HackathonDeviceIdPrefix))
                    .ToList();
            }
        }

        public class GetDevicesResponse
        {
            [JsonProperty("devices")] public List<ThingsDeviceModel> Devices { get; set; }
        }
    }
}