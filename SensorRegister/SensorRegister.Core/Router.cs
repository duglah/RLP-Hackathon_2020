using System;

namespace SensorRegister.Core
{
    public class Router
    {
        public Action ShowLogin { get; set; }
        public Action ShowDevices { get; set; }
        public Action ShowAddSensor { get; set; }

        public Router()
        {
        }
    }
}