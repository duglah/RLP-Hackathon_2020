using System.Reactive.Concurrency;
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
            router.ShowDevices = () => Application.Run(new DevicesView(new DevicesViewModel(router)));
            router.ShowAddSensor = () => Application.Run(new AddSensorView(new AddSensorViewModel(router)));
            
            router.ShowDevices();
        }

        static void ShowWindow<TViewModel>(BaseWindow<TViewModel> window) where TViewModel : DisposableViewModel
        {
            Application.Current?.Dispose();
            Application.Run(window);
        }
    }
}