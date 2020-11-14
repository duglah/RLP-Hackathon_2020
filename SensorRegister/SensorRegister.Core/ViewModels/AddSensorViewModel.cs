using System;
using System.Reactive;
using System.Runtime.Serialization;
using System.Threading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SensorRegister.Core.Api.ThingsNetwork;

namespace SensorRegister.Core.ViewModels
{
    [DataContract]
    public class AddSensorViewModel : ReactiveObject
    {
        readonly ObservableAsPropertyHelper<bool> _isValid;
        readonly Router _router;

        public AddSensorViewModel(Router router)
        {
            _router = router;
            AddDevice = ReactiveCommand.Create(() => { });
            AddDevice.Subscribe(async unit =>
            {
                await ThingsNetworkDevicesApi.AddDevice(new ThingsDeviceModel
                {
                    AppId = ThingsNetworkDevicesApi.app_id,
                    Description = "test description",
                    DeviceId = DeviceID,
                    Device = new ThingsDeviceLorawan
                    {
                        DeviceId = DeviceID,
                        AppId = ThingsNetworkDevicesApi.app_id,
                        DeviceEUI = DeviceEUI,
                        AppEUI = AppEUI
                    }
                });
                Beep();
            });

            Clear = ReactiveCommand.Create(() => { });
            Clear.Subscribe(unit =>
            {
                Beep(2);
                DeviceID = string.Empty;
                DeviceEUI = string.Empty;
                AppKey = string.Empty;
                AppEUI = string.Empty;
            });
        }

        void Beep(int times = 1)
        {
            while (times > 0)
            {
                Console.Beep();
                if (times-- > 0) Thread.Sleep(200);
            }
        }

        [Reactive, DataMember] public string DeviceID { get; set; } = string.Empty;

        [Reactive, DataMember] public bool AutoGenerateDeviceEUI { get; set; } = true;

        [Reactive, DataMember] public string DeviceEUI { get; set; } = string.Empty;

        [Reactive, DataMember] public string AppKey { get; set; } = string.Empty;

        [Reactive, DataMember] public string AppEUI { get; set; } = "70B3D57ED0035458"; //string.Empty;

        [IgnoreDataMember] public ReactiveCommand<Unit, Unit> AddDevice { get; }

        [IgnoreDataMember] public ReactiveCommand<Unit, Unit> Clear { get; }
    }
}