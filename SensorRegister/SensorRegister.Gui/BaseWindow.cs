using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NStack;
using ReactiveUI;
using SensorRegister.Core;
using SensorRegister.Core.ViewModels;
using SensorRegister.Gui.Utils;
using Terminal.Gui;

namespace SensorRegister.Gui
{
    public abstract class BaseWindow<TViewModel> : Window, IViewFor<TViewModel> where TViewModel : DisposableViewModel
    {
        protected readonly CompositeDisposable _disposable = new CompositeDisposable();
        protected readonly Color backgroundColor = Color.Black;

        public BaseWindow(TViewModel viewModel, string title) : base(title)
        {
            ViewModel = viewModel;
            
            ColorScheme = new ColorScheme
                {Normal = new Terminal.Gui.Attribute(foreground: Color.White, background: backgroundColor)};
        }

        protected override void Dispose(bool disposing)
        {
            _disposable.Dispose();
            ViewModel.Dispose();
            base.Dispose(disposing);
        }

        public TViewModel ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel) value;
        }
    }
}