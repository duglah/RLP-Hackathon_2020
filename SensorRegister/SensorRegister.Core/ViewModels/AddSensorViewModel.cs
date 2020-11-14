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


        public AddSensorViewModel(Router router)
        {
            _router = router;
            AddDevice = ReactiveCommand.Create(() => { });
            AddDevice.Subscribe(async unit =>
            {
                try
                {
                    var device = new ThingsDeviceModel
                    {
                        AppId = ThingsNetworkDevicesApi.app_id,
                        Description = Description,
                        DeviceId = DeviceID,
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

                    Beep();
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
                DeviceID = string.Empty;
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

        [Reactive, DataMember] public string DeviceID { get; set; } = string.Empty;
        [Reactive, DataMember] public string Description { get; set; } = "RLP Hackathon 2020";
        [Reactive, DataMember] public string DeviceEUI { get; set; } = Utils.RandomByteString(8);

        [Reactive, DataMember] public string AppKey { get; set; } = string.Empty;

        [Reactive, DataMember] public string AppEUI { get; set; } = "70B3D57ED0035458"; 

        [IgnoreDataMember] public ReactiveCommand<Unit, Unit> AddDevice { get; }

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