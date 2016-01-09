using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.IRC.Messages.Send;
using Pyratron.PyraChat.UI.Models;

namespace Pyratron.PyraChat.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand EnterCommand { get; }

        public string ChatLineText
        {
            get { return chatLineText; }
            set
            {
                chatLineText = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<UiUser> Users
        {
            get { return users; }
            set
            {
                users = value;
                RaisePropertyChanged();
            }
        }

        public UiChannel Channel
        {
            get { return channel; }
            set
            {
                channel = value;
                RaisePropertyChanged();
            }
        }

        private UiChannel channel;
        private readonly Client irc;
        private string logText, channelText, chatLineText;
        private ObservableCollection<UiUser> users;

        public MainViewModel(bool designTime)
        {
            Users = new ObservableCollection<UiUser>();
            EnterCommand = new RelayCommand(OnEnter);

            if (!designTime)
            {
                irc = new Client("frogbox.es", 6667, new User("My_Name", "My Real Name", "Tester_T"));
                irc.IRCMessage += message => Console.WriteLine(message.Text);
                irc.Connect += () =>
                {
                    irc.Send(new JoinMessage("#pyratest"));
                };
                irc.Nick += message =>
                {
                    SortUsers();
                };
                irc.RankChange += (user, channel, rank) =>
                {
                    SortUsers();
                };
                irc.AwayChange += (user, away) =>
                {
                    SortUsers();
                };
                irc.ChannelJoin += message =>
                {
                    Channel = new UiChannel(message.Channel);
                    message.Channel.Message += privateMessage => //Subscribe to the joined channel's messages
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            ViewModelLocator.Chat.AddLine(privateMessage);
                        });
                    };

                    message.Channel.UserAdd += user =>
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Users.Add(new UiUser(user, GenColor()));
                            SortUsers();
                        });
                    };

                    message.Channel.UserRemove += user =>
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Users.Remove(Users.FirstOrDefault(x=> x.User == user));
                            SortUsers();
                        });
                    };
                };
                //irc.ReplyMOTDEnd += delegate(MOTDEndMessage message) { Text += message.MOTD + Environment.NewLine; };
                irc.Start();
            }
        }

        public UiUser GetUser(User user)
        {
            return Users.FirstOrDefault(x => x.User == user);
        }

        private Color GenColor()
        {
            return Colors.Red;
        }

        private void SortUsers()
        {
            Users = new ObservableCollection<UiUser>(Users.OrderBy(u => u.User.GetRank(channel.Channel.Name)).ThenBy(u => u.User.Nick));
        }

        private void OnEnter()
        {
            irc.Send(new PrivateMessage("#pyratest", ChatLineText));
            ChatLineText = string.Empty;
        }
    }
}