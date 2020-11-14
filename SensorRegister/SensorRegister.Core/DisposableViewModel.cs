using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace SensorRegister.Core
{
    public class DisposableViewModel : ReactiveObject
    {
        protected readonly CompositeDisposable _disposable = new CompositeDisposable();
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}