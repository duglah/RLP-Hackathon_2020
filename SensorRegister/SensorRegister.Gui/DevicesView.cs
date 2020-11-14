using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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

        public DevicesView(DevicesViewModel viewModel) : base(viewModel, "Sensoren")
        {
            Rerender();

            ViewModel.OnError.Subscribe(err =>
            {
                MessageBox.ErrorQuery(40, 15, err.Message, err.Message + "\n" + err.StackTrace, "damn");
            }).DisposeWith(_disposable);

            LoadDevices(statusLabel);
        }

        private async Task LoadDevices(Label statusLabel)
        {
            statusLabel.Text = "loading...";
            var devices = await ViewModel.LoadDevices() ?? new List<ThingsDeviceModel>();
            statusLabel.Text = $"";
            Title = $"{devices.Count} Sensoren";
        }

        void Rerender()
        {
            var builder = ViewBuilder.Create()
                .SetHorizontalMargin(2)
                .Add(statusLabel, width: Dim.Fill())
                .Below(new Label("-------------------------"));

            foreach (var device in _devices)
            {
                var label = device.DeviceId;
                builder.Below(new Button(label)
                {
                    LayoutStyle = LayoutStyle.Computed,
                    Width = label.Length + 4
                });
            }

            builder
                .Below(new Label("-------------------------"))
                .Below(new Label(" "))
                .Below(AddNewDeviceButton("add new sensor"))
                .Build(this);
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