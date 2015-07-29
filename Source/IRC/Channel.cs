﻿using System.Collections.Generic;

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

        public delegate void NoticeEventHandler(Channel channel, User user, string notice);
        public event NoticeEventHandler Notice;
        internal void OnNotice(User user, string notice) => Notice?.Invoke(this, user, notice);

        #endregion //Events
    }
}