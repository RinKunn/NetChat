using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NetChat.Desktop.ViewModel.Commands;

namespace NetChat.Desktop.ViewModel.Base
{
    public abstract class InitingViewModelBase : ViewModelBase
    {
        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set => Set(ref _isLoaded, value);
        }

        private bool _hasLoadingError;
        public bool HasLoadingError
        {
            get => _hasLoadingError;
            set => Set(ref _hasLoadingError, value);
        }

        public InitingViewModelBase() : base()
        {
            IsLoaded = false;
            HasLoadingError = false;
        }

        private IAsyncCommand _initCommand;
        public IAsyncCommand InitCommand => _initCommand
            ?? (_initCommand = new AsyncCommand(InitViewModel));

        public async Task InitViewModel()
        {
            if (HasLoadingError)
                HasLoadingError = false;
            if(!IsLoaded)
            {
                try
                {
                    await Init();
                }
                catch(Exception e)
                {
                    HasLoadingError = true;

                }
            }
        }
        public abstract Task Init();
    }
}
