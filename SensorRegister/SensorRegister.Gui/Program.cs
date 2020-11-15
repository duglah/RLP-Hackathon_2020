using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using ReactiveUI;
using SensorRegister.Core;
using SensorRegister.Core.ViewModels;
using Terminal.Gui;

namespace SensorRegister.Gui
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            RxApp.MainThreadScheduler = TerminalScheduler.Default;
            RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;

            var router = new Router();

            router.ShowLogin = () => ShowWindow(new LoginView(new LoginViewModel(router)));
            router.ShowDevices = () => ShowWindow(new DevicesView(new DevicesViewModel(router)));
            router.ShowAddSensor = (device) => ShowWindow(new AddSensorView(new AddSensorViewModel(router, device)));

            router.ShowDevices();
        }

        static void ShowWindow<TViewModel>(BaseWindow<TViewModel> window) where TViewModel : DisposableViewModel
        {
            Application.Current?.Dispose();
            Application.Run(window);
        }
    }
}