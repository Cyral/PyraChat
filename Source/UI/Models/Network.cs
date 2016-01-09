using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using Pyratron.PyraChat.IRC;
using Pyratron.PyraChat.IRC.Messages.Send;
using Pyratron.PyraChat.UI.ViewModels;

namespace Pyratron.PyraChat.UI.Models
{
    public class Network : ObservableObject
    {
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        public Client Client { get; }

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

        internal UiUser Me { get; private set; }

        private ObservableCollection<UiChannel> channels;
        private string name;

        private ObservableCollection<UiUser> users, displayUsers;

        public Network(string host, int port, User userirc, string[] initialChannels)
        {
            Name = host; // By default, name is host.
            Users = new ObservableCollection<UiUser>();
            DisplayUsers = new ObservableCollection<UiUser>();
            Channels = new ObservableCollection<UiChannel>();

            Me = new UiUser(userirc, ViewModelLocator.Main.GenColor());

            // Create IRC client and handle connection.
            Client = new Client(host, port, userirc);
            Client.IRCMessage += message => Console.WriteLine(message.Text);
            Client.Connect += () =>
            {
                foreach (var chan in initialChannels)
                    Client.Send(new JoinMessage(chan));
            };
            Client.ReplyISupport += message =>
            {
                // Use the NETWORK= parameter in 005 to set the server name.
                string networkName;
                if (message.TryGetParameter("Network", out networkName))
                    Name = networkName;
            };

            // Update user list on certain events.
            Client.Nick += message => { SortUsers(); };
            Client.RankChange += (user, channel, rank) => { SortUsers(); };
            Client.AwayChange += (user, away) => { SortUsers(); };

            // Subscribe to events when a channel is joined.
            Client.ChannelJoin += message =>
            {
                var channelJoined = new UiChannel(message.Channel, this);
                if (ViewModelLocator.Main.Channel == null)
                    ViewModelLocator.Main.Channel = channelJoined;
                DispatcherHelper.CheckBeginInvokeOnUI(() => Channels.Add(channelJoined));
                message.Channel.Message += privateMessage => //Subscribe to the joined channel's messages
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            var chan = Channels.FirstOrDefault(c => c.Channel == message.Channel);
                            chan?.AddLine(privateMessage);
                        });
                };
                // Show system messages (will be moved to cause/effect system eventually)
                message.Channel.UserJoin +=
                    joinMessage =>
                    {
                        AddSystemLine(
                            $"{joinMessage.User.Nick} ({joinMessage.User.Ident}@{joinMessage.User.Host}) has joined.", channelJoined.Channel);
                    };
                message.Channel.UserPart +=
                    partMessage =>
                    {
                        var reason = string.IsNullOrEmpty(partMessage.Reason) ? string.Empty : $"({partMessage.Reason})";
                        AddSystemLine($"{partMessage.User.Nick} has left. {reason}", channelJoined.Channel);
                    };

                message.Channel.UserAdd += user =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        //Don't add duplicate users to the master list.
                        if (Users.All(u => u.User != user))
                        {
                            Users.Add(new UiUser(user, ViewModelLocator.Main.GenColor()));
                            SortUsers();
                        }
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
            // Show system messages (will be moved to cause/effect system eventually)
            Client.Quit +=
                message =>
                {
                    var reason = string.IsNullOrEmpty(message.Reason) ? string.Empty : $"({message.Reason})";
                    AddSystemLine($"{message.User.Nick} has quit. {reason}", message.User.Channels.ToArray());
                };
            Client.Nick += message =>
            {
                AddSystemLine($"{message.OldNick} is now known as {message.Nick}.", message.User.Channels.ToArray());
            };
            //irc.ReplyMOTDEnd += delegate(MOTDEndMessage message) { Text += message.MOTD + Environment.NewLine; };
            Client.Start();
        }

        public UiUser GetUser(User user)
        {
            return Users.FirstOrDefault(x => x.User == user);
        }

        internal void SortUsers()
        {
            foreach (var user in users)
                user.Rank = user.User.GetRank(ViewModelLocator.Main.Channel.Channel.Name);
            DisplayUsers =
                new ObservableCollection<UiUser>(
                    Users.ToList().Where(u => u.User.Channels.Contains(ViewModelLocator.Main.Channel.Channel))
                        .OrderBy(u => u.Rank)
                        .ThenBy(u => u.User.Nick));
        }

        private void AddSystemLine(string message, params Channel[] channels)
        {
            AddSystemLine(message, Globals.SystemColor, channels);
        }

        private void AddSystemLine(string message, Color color, params Channel[] channels)
        {
            if (channels.Length < 0)
                return;
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    foreach (var chan in channels.Select(channel => Channels.FirstOrDefault(c => c.Channel == channel)))
                        chan?.AddSystemLine(message, color);
                });
        }
    }
}