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
                .Add(InputField("-Device EUI", vm => vm.DeviceEUI))
                .Add(InputField("-App Key", vm => vm.AppKey))
                .Add(InputField("-App EUI", vm => vm.AppEUI, "70B3D57ED0035458"))
                .Below(new Label("  "))
                .Below(new Label("----------------------"))
                .Below(new Label("  "))
                .ToTheRight(AddDeviceButton("add"))
                .ToTheRight(new Label("  "))
                .ToTheRight(ClearButton("clear"))
                
                .Build(this);
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

        Button ClearButton(string label = "Clear")
        {
            var clearButton = new Button(label);
            Terminal.Gui.EventExtensions.Events(clearButton)
                .Clicked
                .InvokeCommand(ViewModel, x => x.Clear)
                .DisposeWith(_disposable);
            return clearButton;
        }
        //
        // View ErrorView()
        // {
        //     var label = new Label("");
        //
        //     ViewModel.Errors.Subscribe(err => label.Text = err.Message).DisposeWith(_disposable);
        //
        //     return label;
        // }
    }
}