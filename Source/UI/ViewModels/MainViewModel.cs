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
        public RelayCommand<UiChannel> SelectedChannelCommand { get; }

        public string ChatLineText
        {
            get { return chatLineText; }
            set
            {
                chatLineText = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<UiUser> DisplayUsers
        {
            get { return displayUsers; }
            set
            {
                displayUsers = value;
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

        public ObservableCollection<UiChannel> Channels
        {
            get { return channels; }
            set
            {
                channels = value;
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

        private readonly Client irc;
        private readonly UiUser me;

        private UiChannel channel;
        private ObservableCollection<UiChannel> channels;
        private string chatLineText;

        private Color[] colors =
        {
            Color.FromRgb(204, 62, 62),
            Color.FromRgb(204, 204, 62),
            Color.FromRgb(62,204, 62),
            Color.FromRgb(62, 204, 204),
            Color.FromRgb(204, 62, 204),
        };

        private Random random = new Random();
        private ObservableCollection<UiUser> users, displayUsers;

        public MainViewModel(bool designTime)
        {
            Users = new ObservableCollection<UiUser>();
            DisplayUsers = new ObservableCollection<UiUser>();
            Channels = new ObservableCollection<UiChannel>();
            EnterCommand = new RelayCommand(OnEnter);
            SelectedChannelCommand = new RelayCommand<UiChannel>(param => OnSelectedChannelChanged(param));

            var joinChannels = new[]
            {
                "#Pyratron",
                "#Bricklayer",
                "#pyratest",
            };

            if (!designTime)
            {
                var userirc = new User("My_Name", "My Real Name", "Tester_T");
                me = new UiUser(userirc, GenColor());
                irc = new Client("frogbox.es", 6667, userirc);
                irc.IRCMessage += message => Console.WriteLine(message.Text);
                irc.Connect += () =>
                {
                    foreach (var chan in joinChannels)
                        irc.Send(new JoinMessage(chan));
                };
                irc.Nick += message => { SortUsers(); };
                irc.RankChange += (user, channel, rank) => { SortUsers(); };
                irc.AwayChange += (user, away) => { SortUsers(); };
                irc.ChannelJoin += message =>
                {
                    Channel = new UiChannel(message.Channel);
                    DispatcherHelper.CheckBeginInvokeOnUI(() => Channels.Add(Channel));
                    message.Channel.Message += privateMessage => //Subscribe to the joined channel's messages
                    { DispatcherHelper.CheckBeginInvokeOnUI(() => { Channel.AddLine(privateMessage); }); };

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
                            Users.Remove(Users.FirstOrDefault(x => x.User == user));
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
            return colors[random.Next(0, colors.Length)];
        }

        private void OnEnter()
        {
            var msg = new PrivateMessage(Channel.Name, ChatLineText);
            irc.Send(msg);
            Channel.AddLine(msg, me);
            ChatLineText = string.Empty;
        }

        private void OnSelectedChannelChanged(UiChannel channel)
        {
            Channel = channel;
            SortUsers();
        }

        private void SortUsers()
        {
            foreach (var user in users)
                user.Rank = user.User.GetRank(channel.Channel.Name);
            DisplayUsers =
                new ObservableCollection<UiUser>(
                    Users.Where(u => u.User.Channels.Contains(Channel.Channel))
                        .DistinctBy(u => u.User)
                        .OrderBy(u => u.Rank)
                        .ThenBy(u => u.User.Nick));
        }
    }
}