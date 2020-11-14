using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SensorRegister.Core.Api.ThingsNetwork;

namespace SensorRegister.Core.ViewModels
{
    [DataContract]
    public class DevicesViewModel : DisposableViewModel
    {
        readonly Router _router;
        readonly Subject<Exception> _onError = new Subject<Exception>();
        readonly Subject<ThingsDeviceModel> _onDeviceLoaded = new Subject<ThingsDeviceModel>();


        public DevicesViewModel(Router router)
        {
            _router = router;

            AddNewDevice = ReactiveCommand.Create(() => { });
            AddNewDevice.Subscribe(unit => { _router.ShowAddSensor(); }).DisposeWith(_disposable);
        }

        public async Task<List<ThingsDeviceModel>> LoadDevices()
        {
            try
            {
                var devicesJson = await ThingsNetworkDevicesApi.GetDevices();
                var devices = new ThingsDeviceModel[] { }.ToList();
                devices.ForEach(_onDeviceLoaded.OnNext);
                return devices;
            }
            catch (Exception err)
            {
                _onError.OnNext(err);
                return null;
            }
        }


        [IgnoreDataMember] public ReactiveCommand<Unit, Unit> AddNewDevice { get; }

        public IObservable<Exception> OnError => _onError;
        public IObservable<ThingsDeviceModel> OnDeviceLoaded => _onDeviceLoaded;
    }
}