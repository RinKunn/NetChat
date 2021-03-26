using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel.Factories
{
    public interface IViewModelFactory
    {
        TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;
    }
}
