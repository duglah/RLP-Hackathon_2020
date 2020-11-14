using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SensorRegister.Core.Api.ThingsNetwork
{
    public class ThingsNetworkDevicesApi
    {
        public static readonly string app_id = "brickmakers_office_air_quality";
        private static readonly string key = "ttn-account-v2.t-jufy2MlhfpzZ0gj4H9iBdF6_AdTHmNIlnVubthxVs";


        public ThingsNetworkDevicesApi()
        {
            
        }


        public static async Task AddDevice(ThingsDeviceModel device)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", key);
            var json = JsonConvert.SerializeObject(device);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"http://eu.thethings.network:8084/applications/brickmakers_office_air_quality/devices", httpContent);

            var err = await result.Content.ReadAsStringAsync();
            throw new Exception(err);
        }

        public static async Task<string> GetDevices()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", key);
                try
                {
                    var result = await client.GetAsync(
                        "http://eu.thethings.network:8084/applications/brickmakers_office_air_quality/devices");

                    var s = await result.Content.ReadAsStringAsync();
                    return s;
                }
                catch (Exception e)
                {
                    return e.Message + "\n" + e.StackTrace;
                }
            }
        }
    }
}