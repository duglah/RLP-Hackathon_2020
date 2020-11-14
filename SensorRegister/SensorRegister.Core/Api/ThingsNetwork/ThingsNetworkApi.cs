using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SensorRegister.Core.Api.ThingsNetwork
{

    public class ThingsNetworkDevicesApi
    {
        private static readonly HttpClient client = new HttpClient();


        private static readonly string app_id = "brickmakers_office_air_quality";
        private static readonly string key = "ttn-account-v2.t-jufy2MlhfpzZ0gj4H9iBdF6_AdTHmNIlnVubthxVs";


        public ThingsNetworkDevicesApi() { }

        public static async Task GetDevices()
        {
            client.DefaultRequestHeaders.Add("Authorization", $"key {key}");

            try {
                var devices = await client.GetStringAsync("http://eu.thethings.network:8084/applications/brickmakers_office_air_quality/devices/");

                Console.Write(devices);
            } catch
            {
                Console.WriteLine("ex");
            }

        }
    }



}
