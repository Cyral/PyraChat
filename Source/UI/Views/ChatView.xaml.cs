using System.Windows.Controls;
using Pyratron.PyraChat.UI.ViewModels;

namespace Pyratron.PyraChat.UI.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.Chat;
        }
    }
}