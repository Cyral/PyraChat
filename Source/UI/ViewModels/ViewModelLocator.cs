/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Pyratron.PyraChat.UI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Pyratron.PyraChat.UI.ViewModels
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
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<ChatViewModel>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register(() => new MainViewModel(true));
                SimpleIoc.Default.Register(() => new ChatViewModel(true));
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register(() => new MainViewModel(false));
                SimpleIoc.Default.Register(() => new ChatViewModel(false));
            }
        }

        public static MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public static ChatViewModel Chat => ServiceLocator.Current.GetInstance<ChatViewModel>();

        public static void Cleanup()
        {
            Main.Cleanup();
            Chat.Cleanup();
        }
    }
}