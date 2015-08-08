using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Pyratron.PyraChat.IRC.Messages.Receive;

namespace Pyratron.PyraChat.IRC
{
    public class Channel
    {
        /// <summary>
        /// Channel name (with prefix).
        /// </summary>
        public string Name { get; private set; }

        public string Mode { get; private set; }
        public string Topic { get; private set; }
        public ChannelType Type { get; }
        public Client Client { get; private set; }
        public IEnumerable<User> Users => Client.Users.Where(user => user.Channels != null && user.Channels.Contains(this));
        

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

        #region Events

        public delegate void NoticeEventHandler(NoticeMessage message);
        public delegate void MessageEventHandler(PrivateMessage message);
        public delegate void UserJoinEventHandler(JoinMessage message);
        public delegate void UserAddEventHandler(User user);
        public delegate void UserPartEventHandler(PartMessage message);
        public delegate void UserRemoveEventHandler(User user);

        public event NoticeEventHandler Notice;
        public event MessageEventHandler Message;
        public event UserJoinEventHandler UserJoin;
        public event UserAddEventHandler UserAdd;
        public event UserPartEventHandler UserPart;
        public event UserRemoveEventHandler UserRemove;

        internal void OnNotice(NoticeMessage message) => Notice?.Invoke(message);
        internal void OnMessage(PrivateMessage message) => Message?.Invoke(message);
        internal void OnUserJoin(JoinMessage message) => UserJoin?.Invoke(message);
        internal void OnUserPart(PartMessage message) => UserPart?.Invoke(message);
        internal void OnUserAdd(User user) => UserAdd?.Invoke(user);
        internal void OnUserRemove(User user) => UserRemove?.Invoke(user);

        #endregion //Events
    }
}