using System.Windows;
using Pyratron.PyraChat.UI.Models;
using Pyratron.PyraChat.UI.ViewModels;

namespace Pyratron.PyraChat.UI.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.Main;
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var channel = e.NewValue as UiChannel;
            if (channel != null)
                ViewModelLocator.Main.SelectedChannelCommand.Execute(channel);
        }
    }
}