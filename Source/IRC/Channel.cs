using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<User> Users { get; }

        public Channel(Client client, string name)
        {
            Users = new List<User>();
            Name = name;
            Type = ChannelType.FromPrefix(name[0]);
        }

        /// <summary>
        /// Adds a user to the channel and fires the UserAdd event.
        /// </summary>
        public void AddUser(User user)
        {
            Users.Add(user);
            OnUserAdd(user);
        }

        /// <summary>
        /// Returns the user whose nickname is equal to the value specified.
        /// </summary>
        public User UserFromNick(string nick)
        {
            //Remove rank from nick
            var rank = UserRank.FromPrefix(nick[0]);
            if (rank != UserRank.None)
                nick = nick.Substring(1);
            return Users.FirstOrDefault(u => u.Nick.Equals(nick, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns the user that best matches the mask provided.
        /// If any fields are blank in the user, they will be filled in with data from the mask.
        /// </summary>
        public User UserFromMask(string mask)
        {
            var match = User.MaskRegex.Match(mask);
            if (!match.Success) return null;

            var user = UserFromNick(match.Groups[1].Value);
            user.Ident = match.Groups[2].Value;
            user.Host = match.Groups[3].Value;
            return user;
        }

        #region Events

        public delegate void NoticeEventHandler(NoticeMessage message);
        public delegate void MessageEventHandler(PrivateMessage message);
        public delegate void UserJoinEventHandler(JoinMessage message);
        public delegate void UserAddEventHandler(User user);

        public event NoticeEventHandler Notice;
        public event MessageEventHandler Message;
        public event UserJoinEventHandler UserJoin;
        public event UserAddEventHandler UserAdd;

        internal void OnNotice(NoticeMessage message) => Notice?.Invoke(message);
        internal void OnMessage(PrivateMessage message) => Message?.Invoke(message);
        internal void OnUserJoin(JoinMessage message) => UserJoin?.Invoke(message);
        internal void OnUserAdd(User user) => UserAdd?.Invoke(user);

        #endregion //Events
    }
}