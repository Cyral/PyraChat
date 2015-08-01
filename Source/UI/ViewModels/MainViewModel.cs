using System;
using System.Collections.ObjectModel;
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

        public string LogText
        {
            get { return logText; }
            set
            {
                logText = value;
                RaisePropertyChanged();
            }
        }

        public string ChannelText
        {
            get { return channelText; }
            set
            {
                channelText = value;
                RaisePropertyChanged();
            }
        }


        public string ChatLineText
        {
            get { return chatLineText; }
            set
            {
                chatLineText = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                users = value;
                RaisePropertyChanged();
            }
        } 

        private readonly Client irc;
        private string logText, channelText, chatLineText;
        private ObservableCollection<User> users; 

        public MainViewModel(bool designTime)
        {
            Users = new ObservableCollection<User>();
            EnterCommand = new RelayCommand(OnEnter);

            if (!designTime)
            {
                irc = new Client("frogbox.es", 6667, new User("My_Name", "My Real Name", "Tester_T"));
                irc.IRCMessage += message => LogText += message.Text + Environment.NewLine;
                irc.Connect += () =>
                {
                    irc.Send(new JoinMessage("#Pyratron"));
                    irc.Send(new PrivateMessage("#Pyratron", "Testing123"));
                };
                irc.ChannelJoin += message =>
                {
                    ChannelText += "Talking in " + message.Channel.Name + Environment.NewLine;
                    message.Channel.Message += privateMessage => //Subscribe to the joined channel's messages
                    {
                        ChannelText += privateMessage.Text + Environment.NewLine;
                    };
                };
                //irc.ReplyMOTDEnd += delegate(MOTDEndMessage message) { Text += message.MOTD + Environment.NewLine; };
                irc.Start();
            }

            Users.Add(new User("Cyral", "", "") { Rank = UserRank.Owner });
            Users.Add(new User("Kaslai", "", "") { Rank= UserRank.Admin});
            Users.Add(new User("Fer22f", "", "") { Rank = UserRank.Op });
            Users.Add(new User("Pugmatt", "", "") { Rank = UserRank.HalfOp });
            Users.Add(new User("Test2", "", "") { Rank = UserRank.Voice });
            Users.Add(new User("Test3", "", "") { Rank = UserRank.None });
        }

        private void OnEnter()
        {
            irc.Send(new PrivateMessage("#Pyratron", ChatLineText));
            ChatLineText = string.Empty;
        }
    }
}