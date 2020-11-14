using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
            var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"http://eu.thethings.network:8084/applications/brickmakers_office_air_quality/devices/{device.DeviceId}", httpContent);
            var s = await result.Content.ReadAsStringAsync();
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