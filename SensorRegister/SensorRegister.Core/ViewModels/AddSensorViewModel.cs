using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using System.Threading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SensorRegister.Core.Api.ThingsNetwork;

namespace SensorRegister.Core.ViewModels
{
    [DataContract]
    public class AddSensorViewModel : DisposableViewModel
    {
        readonly ObservableAsPropertyHelper<bool> _isValid;
        readonly Router _router;
        readonly Subject<Exception> _onError = new Subject<Exception>();
        readonly Subject<ThingsDeviceModel> _onDeviceAdded = new Subject<ThingsDeviceModel>();
        private ThingsDeviceModel _showModel;

        public AddSensorViewModel(Router router, ThingsDeviceModel showModel = null)
        {
            _router = router;
            _showModel = showModel;

            if (_showModel != null)
            {
                DeviceID = showModel.DeviceId;
                DeviceEUI = showModel.Device.DeviceEUI;
                Description = showModel.Description;
                AppKey = showModel.AppId;
                AppEUI = showModel.Device.AppEUI;
            }

            AddDevice = ReactiveCommand.Create(() => { });
            AddDevice.Subscribe(async unit =>
            {
                try
                {
                    if (!DeviceID.StartsWith(ThingsNetworkDevicesApi.HackathonDeviceIdPrefix))
                        throw new Exception("Device ID must start with " +
                                            ThingsNetworkDevicesApi.HackathonDeviceIdPrefix);

                    var device = new ThingsDeviceModel
                    {
                        AppId = ThingsNetworkDevicesApi.app_id,
                        Description = Description,
                        DeviceId = DeviceID.Trim().ToLower(),
                        Device = new ThingsDeviceLorawan
                        {
                            DeviceId = DeviceID,
                            AppId = ThingsNetworkDevicesApi.app_id,
                            DeviceEUI = DeviceEUI,
                            AppEUI = AppEUI
                        }
                    };
                    
                    await ThingsNetworkDevicesApi.AddDevice(device);

                    _onDeviceAdded.OnNext(device);

                    _router.ShowDevices();
                }
                catch (Exception err)
                {
                    _onError.OnNext(err);
                }
            }).DisposeWith(_disposable);

            DeleteDevice = ReactiveCommand.Create(() => { });
            DeleteDevice.Subscribe(async unit =>
            {
                try
                {
                    await ThingsNetworkDevicesApi.DeleteDevice(DeviceID);
                    Beep();
                    _router.ShowDevices();
                }
                catch (Exception err)
                {
                    Beep(2);
                    _onError.OnNext(err);
                }
            }).DisposeWith(_disposable);

            Clear = ReactiveCommand.Create(() => { });
            Clear.Subscribe(unit =>
            {
                DeviceID = ThingsNetworkDevicesApi.HackathonDeviceIdPrefix;
                DeviceEUI = Utils.RandomByteString(8);
            }).DisposeWith(_disposable);

            Cancel = ReactiveCommand.Create(() => { });
            Cancel.Subscribe(_ => _router.ShowDevices()).DisposeWith(_disposable);
        }

        void Beep(int times = 1)
        {
            while (times > 0)
            {
                Console.Beep();
                if (times-- > 0) Thread.Sleep(200);
            }
        }

        [Reactive, DataMember] public string DeviceID { get; set; } = ThingsNetworkDevicesApi.HackathonDeviceIdPrefix;
        [Reactive, DataMember] public string Description { get; set; } = "RLP Hackathon 2020";
        [Reactive, DataMember] public string DeviceEUI { get; set; } = Utils.RandomByteString(8);

        [Reactive, DataMember] public string AppKey { get; set; } = string.Empty;

        [Reactive, DataMember] public string AppEUI { get; set; } = "70B3D57ED0035458";

        public bool IsReadOnly => _showModel != null;

        [IgnoreDataMember] public ReactiveCommand<Unit, Unit> AddDevice { get; }
        [IgnoreDataMember] public ReactiveCommand<Unit, Unit> DeleteDevice { get; }

        [IgnoreDataMember] public ReactiveCommand<Unit, Unit> Clear { get; }
        [IgnoreDataMember] public ReactiveCommand<Unit, Unit> Cancel { get; }


        public IObservable<Exception> OnError => _onError;
        public IObservable<ThingsDeviceModel> OnDeviceAdded => _onDeviceAdded;

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}