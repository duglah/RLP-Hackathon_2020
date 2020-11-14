using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NStack;
using ReactiveUI;
using SensorRegister.Core.Api.ThingsNetwork;
using SensorRegister.Core.ViewModels;
using SensorRegister.Gui.Utils;
using Terminal.Gui;

namespace SensorRegister.Gui
{
    public class AddSensorView : BaseWindow<AddSensorViewModel>
    {
        public AddSensorView(AddSensorViewModel viewModel) : base(viewModel, "Sensor hinzufuegen")
        {
            ViewBuilder.Create()
                // .SetHorizontalMargin(2)
                .Add(new Label(" "))
                .Add(InputField("-Device ID", vm => vm.DeviceID))
                .Add(InputField("-Description", vm => vm.Description))
                .Add(InputField("-Device EUI", vm => vm.DeviceEUI))
                // .Add(InputField("-App Key", vm => vm.AppKey))
                .Add(InputField("-App EUI", vm => vm.AppEUI, "70B3D57ED0035458"))
                .Below(new Label("  "))
                .Below(StatusLabel())
                .Below(new Label("  "))
                .ToTheRight(AddDeviceButton("add"))
                .ToTheRight(new Label("  "))
                .ToTheRight(ClearButton("clear"))
                .Below(new Label("  "))
                .ToTheRight(CancelButton("cancel"))
                .Build(this);

            ViewModel.OnError.Subscribe(err =>
            {
                MessageBox.ErrorQuery(40, 15, err.Message, err.Message+"\n"+err.StackTrace, "damn");
            }).DisposeWith(_disposable);
        }


        Func<ViewBuilder, ViewBuilder> InputField(string label, Expression<Func<AddSensorViewModel, string>> binding,
            string defaultText = "")
        {
            return vb =>
            {
                var inputTextField = new TextField(defaultText);

                ViewModel.WhenAnyValue(binding)
                    .Select(x => ustring.Make(x))
                    .BindTo(inputTextField, x => x.Text)
                    .DisposeWith(_disposable);

                Terminal.Gui.EventExtensions.Events(inputTextField)
                    .TextChanged
                    .Select(old => inputTextField.Text)
                    .DistinctUntilChanged()
                    .Select(x => x.ToString())
                    .BindTo(ViewModel, binding)
                    .DisposeWith(_disposable);

                return vb
                    .Below(new Label(label))
                    .BelowIt(new Label(" -> "))
                    .ToTheRight(inputTextField, width: Dim.Fill());
            };
        }

        Button AddDeviceButton(string label)
        {
            var addDeviceButton = new Button(label)
            {
                LayoutStyle = LayoutStyle.Computed,
                Width = label.Length + 4
            };
            Terminal.Gui.EventExtensions.Events(addDeviceButton)
                .Clicked
                .InvokeCommand(ViewModel, x => x.AddDevice)
                .DisposeWith(_disposable);

            return addDeviceButton;
        }
        
        View StatusLabel()
        {
            var empty = "-----------------------------";
            
            var label = new Label(empty);

            ViewModel.OnDeviceAdded.Subscribe(device => label.Text = $"-> added {device.DeviceId}" ).DisposeWith(_disposable);

            return label;
        }

        Button ClearButton(string label = "Clear")
        {
            var clearButton = new Button(label);
            Terminal.Gui.EventExtensions.Events(clearButton)
                .Clicked
                .InvokeCommand(ViewModel, x => x.Clear)
                .DisposeWith(_disposable);
            return clearButton;
        }
        
        Button CancelButton(string label = "Cancel")
        {
            var cancelButton = new Button(label);
            Terminal.Gui.EventExtensions.Events(cancelButton)
                .Clicked
                .InvokeCommand(ViewModel, x => x.Cancel)
                .DisposeWith(_disposable);
            return cancelButton;
        }
        
    }
}