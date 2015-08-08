using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Pyratron.PyraChat.IRC;
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
                    irc.Send(new JoinMessage("#pyrachat"));
                    //irc.Send(new PrivateMessage("#pyrachat", "Testing123"));
                };
                irc.Nick += message =>
                {
                    SortUsers();
                };
                irc.ChannelJoin += message =>
                {
                    ChannelText += "Talking in " + message.Channel.Name + Environment.NewLine;
                    message.Channel.Message += privateMessage => //Subscribe to the joined channel's messages
                    { ChannelText += privateMessage.Text + Environment.NewLine; };

                    message.Channel.UserAdd += user =>
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Users.Add(user);
                            SortUsers();
                        });
                    };

                    message.Channel.UserRemove += user =>
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Users.Remove(user);
                            SortUsers();
                        });
                    };
                };
                //irc.ReplyMOTDEnd += delegate(MOTDEndMessage message) { Text += message.MOTD + Environment.NewLine; };
                irc.Start();
            }
        }

        private void SortUsers()
        {
            Users = new ObservableCollection<User>(Users.OrderBy(u => u.Rank).ThenBy(u => u.Nick));
        }

        private void OnEnter()
        {
            irc.Send(new PrivateMessage("#pyrachat", ChatLineText));
            ChatLineText = string.Empty;
        }
    }
}