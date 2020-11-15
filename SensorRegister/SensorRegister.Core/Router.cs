using System;
using SensorRegister.Core.Api.ThingsNetwork;

namespace SensorRegister.Core
{
    public class Router
    {
        public Action ShowLogin { get; set; }
        public Action ShowDevices { get; set; }
        public Action<ThingsDeviceModel> ShowAddSensor { get; set; }

        public Router()
        {
        }
    }
}