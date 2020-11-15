using System.Collections.Generic;
using System.Linq;
using GraphQL.Client.Abstractions.Utilities;

namespace SensorRegister.Core.Api.DataCake.Models
{
    public class DataCakeDashboard
    {
        public string Name { get; set; } = "Dashboard";
        public List<DataCakeSensor> Widgets { get; set; }

        //quick and dirty 👺
        public override string ToString()
            => $@"[
      {{
        ""name"": ""{Name}"",
        ""widgets"": {{{
                    Widgets.Skip(1).Aggregate(Widgets.FirstOrDefault()?.ToString() ?? "", (s, sensor) => s + ",\n" + sensor)
              }}}
      }}
      ]".Replace("\"","\\\"");
    }
}
/*
 [
  {
    "name": "Dashboard",
    "widgets": {
      "-->DEVICE-ID<--": {
        "widget": "Value",
        "layouts": {
          "lg": {
            "w": 2,
            "h": 2,
            "x": 0,
            "y": 2,
            "i": "-->DEVICE-ID<--",
            "moved": false,
            "static": false
          }
        },
        "meta": {
          "title": { "en": "-->TITLE<--", "de": "-->TITLE<--" },
          "decimalPlaces": 2,
          "timerangeOperation": {
            "other": false,
            "start": "",
            "end": "",
            "operator": ""
          },
          "field": {
            "device": "e1c1da0d-d6c9-4806-a960-b432a4aa41f3",
            "fieldName": "CO2",
            "fieldType": "INT"
          }
        }
      }
    }
  }
]

 */
