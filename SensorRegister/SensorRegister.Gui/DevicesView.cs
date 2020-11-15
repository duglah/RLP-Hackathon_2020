using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using NStack;
using ReactiveUI;
using SensorRegister.Core.Api.ThingsNetwork;
using SensorRegister.Core.ViewModels;
using SensorRegister.Gui.Utils;
using Terminal.Gui;

namespace SensorRegister.Gui
{
    public class DevicesView : BaseWindow<DevicesViewModel>
    {
        List<ThingsDeviceModel> _devices = new List<ThingsDeviceModel>();
        readonly Label statusLabel = new Label(" ");
        private ListView _devicesList;

        public DevicesView(DevicesViewModel viewModel) : base(viewModel, "Sensoren")
        {
            ViewBuilder.Create()
                .SetHorizontalMargin(2)
                .Add(statusLabel, width: Dim.Fill())
                .Below(new Label("-------------------------"))
                .Below(_devicesList = new ListView()
                {
                    LayoutStyle = LayoutStyle.Absolute,
                })
                .Below(new Label("-------------------------"))
                .Below(new Label(" "))
                .Below(AddNewDeviceButton("add new sensor"))
                .Build(this);

            ViewModel.OnError.Subscribe(err =>
            {
                MessageBox.ErrorQuery(40, 15, err.Message.Length > 40 ? err.Message.Substring(0, 40) : err.Message, err.Message + "\n" + err.StackTrace, "damn");
            }).DisposeWith(_disposable);

            Observable.FromEvent<ListViewItemEventArgs>(h => _devicesList.OpenSelectedItem += h, h => _devicesList.OpenSelectedItem -= h)
                .Select(evt => evt.Value as ThingsDeviceModel)
                .Subscribe(device => ViewModel.ShowDevice(device))
                .DisposeWith(_disposable);

            LoadDevices(statusLabel);
        }

        private async Task LoadDevices(Label statusLabel)
        {
            statusLabel.Text = "loading...";
            _devices = await ViewModel.LoadDevices() ?? new List<ThingsDeviceModel>();
            statusLabel.Text = $"";
            Title = $"{_devices.Count} Sensoren";

            
            _devicesList.Frame = new Rect(new Point(2, 2), new Size(Frame.Width - 2, _devices.Count));
            await _devicesList.SetSourceAsync(_devices);
        }


        Button AddNewDeviceButton(string label)
        {
            var addDeviceButton = new Button(label)
            {
                LayoutStyle = LayoutStyle.Computed,
                Width = label.Length + 4
            };
            Terminal.Gui.EventExtensions.Events(addDeviceButton)
                .Clicked
                .InvokeCommand(ViewModel, x => x.AddNewDevice)
                .DisposeWith(_disposable);

            return addDeviceButton;
        }
    }
}