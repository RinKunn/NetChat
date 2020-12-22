using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using NetChat.Desktop.ViewModel.Messenger;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;

namespace NetChat.Desktop.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<MainViewModel>();
                var container = builder.Build();
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }

        public static void Cleanup()
        {
            
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetService<MainViewModel>();
    }

    public static class LocatorServices
    {
        public static TService GetService<TService>(this IServiceLocator locator)
        {
            return (TService)locator.GetService(typeof(TService));
        }
    }
}