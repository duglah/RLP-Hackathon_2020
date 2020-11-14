using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SensorRegister.Core.Api.DataCake
{
    public class Device
    {
        private readonly string _apiToken;

        private const string DevicesApiUri = "https://api.datacake.co/v1/devices";

        public Device(string apiToken)
        {
            _apiToken = apiToken;
        }
        
        /// <summary>
        /// Creates device
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="devEui"></param>
        /// <param name="plan"></param>
        /// <param name="name"></param>
        /// <returns>ID</returns>
        public async Task<string> CreateDevice(string workspace, string devEui, string plan, string name)
        {
            const string requestString = "{\n" +
                                         "    \"operationName\": \"createLoraDevice\",\n" +
                                         "    \"variables\": {\n" +
                                         "        \"input\": {\n" +
                                         "            \"workspace\": \" + workspace + \",\n" +
                                         "            \"name\": \" + name +\",\n" +
                                         "            \"kind\": \"elsys-ers-co2\",\n" +
                                         "            \"network\": \"TTN\",\n" +
                                         "            \"devEui\": \" + devEui + \",\n" +
                                         "            \"appEui\": \"\",\n" +
                                         "            \"appKey\": \"\",\n" +
                                         "            \"webhookNeedsAuthentication\": false,\n" +
                                         "            \"plan\": \" + plan + \",\n" +
                                         "            \"planCode\": \"\",\n" +
                                         "            \"setupVariables\": [],\n" +
                                         "            \"brand\": \"datacake\"\n" +
                                         "        }\n" +
                                         "    },\n" +
                                         "    \"query\": \"mutation createLoraDevice($input: CreateLoraDeviceInputType!) {\\n  createLoraDevice(input: $input) {\\n    ok\\n    device {\\n      id\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"\n" +
                                         "}";
            
            
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _apiToken);
            
            var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");
            
            var result = await client.PostAsync(DevicesApiUri, httpContent);
            
            var s = await result.Content.ReadAsStringAsync();

            var o = JObject.Parse(s);
            
            var ok = (bool)o.SelectToken("data.createLoraDevice.ok");
            if(!ok)
                throw new Exception("Not ok :/");
            
            var id = (string)o.SelectToken("data.createLoraDevice.device.id");
            return id;
        }
    }
}