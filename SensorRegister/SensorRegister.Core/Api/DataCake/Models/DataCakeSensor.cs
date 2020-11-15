using Newtonsoft.Json;

namespace SensorRegister.Core.Api.DataCake.Models
{
  public class DataCakeSensor
    {
      public string Title { get; set; } = "CO2";
      
      //Things Network
      public string DeviceId { get; set; } = "e1c1da0d-d6c9-4806-a960-b432a4aa41f3";
      public string FieldName { get; set; } = "CO2";

      public int X { get; set; } = 0;
      public int Y { get; set; } = 1;
      public int W { get; set; } = 2;
      public int H { get; set; } = 2;

      //quick and dirty 👺
      public override string ToString()
        => $@"""{DeviceId}"": {{
            ""widget"": ""Value"",
            ""layouts"": {{
               ""lg"": {{
                  ""w"": {W},
                  ""h"": {H},
                  ""x"": {X},
                  ""y"": {Y},
                  ""i"": ""{DeviceId}"",
                  ""moved"": false,
                  ""static"": false
               }}
            }},
            ""meta"": {{
              ""title"": {{ ""en"": ""{Title}"", ""de"": ""{Title}"" }},
              ""decimalPlaces"": 2,
              ""timerangeOperation"": {{
                ""other"": false,
                ""start"": """",
                ""end"": """",
                ""operator"": """"
              }},
              ""field"": {{
                ""device"": ""{DeviceId}"",
                ""fieldName"": ""{FieldName}"",
                ""fieldType"": ""INT""
              }}
            }}
          }}
       ".Replace("\n", "");
    }

    
}