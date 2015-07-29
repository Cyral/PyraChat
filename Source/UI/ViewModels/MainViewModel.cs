using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.IRC.Messages.Send;

namespace Pyratron.PyraChat.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand EnterCommand { get; }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                RaisePropertyChanged();
            }
        }

        private readonly Client irc;
        private string text;

        public MainViewModel()
        {
            EnterCommand = new RelayCommand(OnEnter);

            irc = new Client("frogbox.es", 6667, new User("My_Name", "My Real Name", "USFF0000"));
            irc.MessageReceived += message => Text += message + Environment.NewLine;
            irc.Connect += client => irc.SendMessage(new PrivateMessage("#Pyratron", "Testing123"));
            //irc.Notice += (client, user, notice) => Text += notice + Environment.NewLine;
            irc.Start();
        }

        private void OnEnter()
        {
            Text = string.Empty;
        }
    }
}