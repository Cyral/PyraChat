using System.Collections.Generic;
using System.Linq;
using Pyratron.PyraChat.IRC.Messages;
using Pyratron.PyraChat.IRC.Messages.Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;

namespace Pyratron.PyraChat.IRC
{
    public class Channel
    {
        /// <summary>
        /// Channel name (with prefix).
        /// </summary>
        public string Name { get; private set; }

        public List<char> Modes { get; } = new List<char>();
        public ChannelTopic Topic { get; private set; } = new ChannelTopic();
        public ChannelType Type { get; }
        public Client Client { get; }

        public IEnumerable<User> Users
            => Client.Users.Where(user => user.Channels != null && user.Channels.Contains(this));

        public Channel(Client client, string name)
        {
            Client = client;
            Name = name;
            Type = ChannelType.FromPrefix(name[0]);
        }

        /// <summary>
        /// Adds a user to the channel and fires the UserAdd event.
        /// </summary>
        public void AddUser(User user)
        {
            user.Channels.Add(this);
            if (!Client.Users.Contains(user))
                Client.Users.Add(user);
            OnUserAdd(user);
        }

        public void RemoveUser(User user)
        {
            user.Channels.Remove(this);
            Client.Users.Remove(user);
            OnUserRemove(user);
        }

        public int AddMode(Client client, char mode, string parameter = "")
        {
            switch (mode)
            {
                case 'v':
                    {
                        var user = client.UserFromNick(parameter);
                        user.AddRank(client, UserRank.Voice);
                        return 1;
                    }
                case 'h':
                    {
                        var user = client.UserFromNick(parameter);
                        user.AddRank(client, UserRank.HalfOp);
                        return 1;
                    }
                case 'o':
                    {
                        var user = client.UserFromNick(parameter);
                        user.AddRank(client, UserRank.Op);
                        return 1;
                    }
                case 'a':
                    {
                        var user = client.UserFromNick(parameter);
                        user.AddRank(client, UserRank.Admin);
                        return 1;
                    }
                case 'q':
                    {
                        var user = client.UserFromNick(parameter);
                        user.AddRank(client, UserRank.Owner);
                        return 1;
                    }
                default:
                {
                    if (!Modes.Contains(mode))
                        Modes.Add(mode);
                    break;
                }
            }
            return 0;
        }

        public int RemoveMode(Client client, char mode, string parameter = "")
        {
            switch (mode)
            {
                case 'v':
                {
                    var user = client.UserFromNick(parameter);
                    user.RemoveRank(client, UserRank.Voice);
                    return 1;
                }
                case 'h':
                {
                    var user = client.UserFromNick(parameter);
                    user.RemoveRank(client, UserRank.HalfOp);
                    return 1;
                }
                case 'o':
                    {
                        var user = client.UserFromNick(parameter);
                        user.RemoveRank(client, UserRank.Op);
                        return 1;
                    }
                case 'a':
                    {
                        var user = client.UserFromNick(parameter);
                        user.RemoveRank(client, UserRank.Admin);
                        return 1;
                    }
                case 'q':
                    {
                        var user = client.UserFromNick(parameter);
                        user.RemoveRank(client, UserRank.Owner);
                        return 1;
                    }
                default:
                    if (Modes.Contains(mode))
                        Modes.Remove(mode);
                    break;
            }
            return 0;
        }

        #region Events

        public delegate void NoticeEventHandler(NoticeMessage message);

        public delegate void MessageEventHandler(PrivateMessage message);

        public delegate void UserJoinEventHandler(JoinMessage message);

        public delegate void UserAddEventHandler(User user);

        public delegate void UserPartEventHandler(PartMessage message);

        public delegate void UserRemoveEventHandler(User user);

        public delegate void TopicEventHandler(TopicMessage message);

        public delegate void TopicWhoTimeEventHandler(TopicWhoTimeMessage message);

        public delegate void ChannelModeEventHandler(ChannelModeMessage message);

        public event NoticeEventHandler Notice;
        public event MessageEventHandler Message;
        public event UserJoinEventHandler UserJoin;
        public event UserAddEventHandler UserAdd;
        public event UserPartEventHandler UserPart;
        public event UserRemoveEventHandler UserRemove;
        public event TopicEventHandler TopicChange;
        public event TopicWhoTimeEventHandler TopicWhoTime;
        public event ChannelModeEventHandler ChannelMode;

        internal void OnNotice(NoticeMessage message) => Notice?.Invoke(message);
        internal void OnMessage(PrivateMessage message) => Message?.Invoke(message);
        internal void OnUserJoin(JoinMessage message) => UserJoin?.Invoke(message);
        internal void OnUserPart(PartMessage message) => UserPart?.Invoke(message);
        internal void OnUserAdd(User user) => UserAdd?.Invoke(user);
        internal void OnUserRemove(User user) => UserRemove?.Invoke(user);
        internal void OnTopicChange(TopicMessage message) => TopicChange?.Invoke(message);
        internal void OnTopicWhoTime(TopicWhoTimeMessage message) => TopicWhoTime?.Invoke(message);
        internal void OnChannelMode(ChannelModeMessage message) => ChannelMode?.Invoke(message);

        #endregion //Events
    }
}