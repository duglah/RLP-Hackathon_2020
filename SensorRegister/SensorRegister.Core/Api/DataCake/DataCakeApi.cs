using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SensorRegister.Core.Api.DataCake.Models;
using SensorRegister.Core.Api.ThingsNetwork;

namespace SensorRegister.Core.Api.DataCake
{
    public class DataCakeApi
    {
        public static async Task GetDashboard()
        {
            using (HttpClient client = new HttpClient())
            {
                var query = @"{dashboard(id:""9372e22c-e56e-4ed4-8277-e81357dc31e7""){id,name,dashboards}}";

                var encodedQuery = System.Net.WebUtility.UrlEncode(query);

                //TODO: touch SensorRegister.Core/Secrets.cs
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("token", Secrets.DataCakeApiToken);

                var result = await client.GetAsync(
                    "https://api.datacake.co/graphql?query=" + encodedQuery);

                var s = await result.Content.ReadAsStringAsync();
                if (!result.IsSuccessStatusCode) throw new Exception(s);
            }
        }

        public static async Task UpdateDashboard(IList<ThingsDeviceModel> sensors)
        {
            using (HttpClient client = new HttpClient())
            {
                var query = new UpdateDashboardMutation
                {
                    Dashboard = new DataCakeDashboard
                    {
                        Widgets = sensors.Take(1).Select(sensor => new DataCakeSensor {Title = sensor.DeviceId})
                            .ToList()
                    }
                }.ToString().Replace("\n", "");

                var encodedQuery = query; //System.Net.WebUtility.UrlEncode(query);

                //TODO: touch SensorRegister.Core/Secrets.cs
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("token", Secrets.DataCakeApiToken);

                var result = await client.PostAsync(
                    "https://api.datacake.co/graphql?query=" + encodedQuery, new StringContent(encodedQuery));

                var s = await result.Content.ReadAsStringAsync();
                if (!result.IsSuccessStatusCode) throw new Exception(s);
            }
        }
    }
}