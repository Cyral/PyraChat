using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;
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

            irc = new Client("frogbox.es", 6667, new User("My_Name", "My Real Name", "Tester_T"));
            irc.IRCMessage += message => Text += message.Text + Environment.NewLine;
            irc.Connect += () =>
            {
                irc.Send(new JoinMessage("#Pyratron"));
                irc.Send(new PrivateMessage("#Pyratron", "Testing123"));
            };
            irc.Message += message =>
            {
                if (message.Text == "!test")
                    irc.Send(new PrivateMessage(message.BaseMessage.Channel.Name, "Received command."));
            };
            //irc.ReplyMOTDEnd += delegate(MOTDEndMessage message) { Text += message.MOTD + Environment.NewLine; };
            irc.Start();
        }

        private void OnEnter()
        {
            Text = string.Empty;
        }
    }
}