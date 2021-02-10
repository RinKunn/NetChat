using GalaSoft.MvvmLight;

namespace NetChat.Desktop.ViewModel
{
    public interface IViewModelFactory
    {
        TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;
    }
}
