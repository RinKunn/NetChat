using System;
using Autofac;
using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly ILifetimeScope _scope;

        public ViewModelFactory(Lazy<ILifetimeScope> scope)
        {
            _scope = scope?.Value ?? throw new ArgumentNullException(nameof(scope));
        }

        public TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            return (TViewModel)_scope.ResolveNamed<ViewModelBase>(typeof(TViewModel).ToString());
        }
    }
}
