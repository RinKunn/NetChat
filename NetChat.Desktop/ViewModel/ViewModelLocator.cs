using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using NetChat.Desktop.ViewModel.Messenger;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using NetChat.Desktop.Services.Messaging.Users;

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
        static ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<MainViewModel>();
                builder.RegisterType<DefaultUserLoader>().As<IUserLoader>();
                var container = builder.Build();
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }

        public static void Cleanup()
        {
            
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
    }

    public static class LocatorServices
    {
        public static TService GetService<TService>(this IServiceLocator locator)
        {
            return (TService)locator.GetService(typeof(TService));
        }
    }
}