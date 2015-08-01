using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        #region Events

        public delegate void NoticeEventHandler(Messages.Receive.NoticeMessage message);
        public delegate void MessageEventHandler(Messages.Receive.PrivateMessage message);

        public event NoticeEventHandler Notice;
        public event MessageEventHandler Message;

        internal void OnNotice(Messages.Receive.NoticeMessage message) => Notice?.Invoke(message);
        internal void OnMessage(Messages.Receive.PrivateMessage message) => Message?.Invoke(message);

        #endregion //Events

        /// <summary>
        /// Returns the user whose nickname is equal to the value specified.
        /// </summary>
        public User UserFromNick(string nick)
        {
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
    }
}