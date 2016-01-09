using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.UI.Models;

namespace Pyratron.PyraChat.UI.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public ObservableCollection<ChatLine> Lines
        {
            get { return lines; }
            set
            {
                lines = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ChatLine> lines;

        public ChatViewModel(bool designTime)
        {
            Lines = new ObservableCollection<ChatLine>();
        }

        public void AddLine(PrivateMessage privateMessage)
        {
            var user = ViewModelLocator.Main.GetUser(privateMessage.BaseMessage.User);
            Lines.Add(new ChatLine(user, privateMessage.Text));
        }
    }
}