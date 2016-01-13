using System.Collections.Generic;
using System.Linq;
using Pyratron.PyraChat.IRC.Messages;
using Pyratron.PyraChat.IRC.Messages.Receive;
using Pyratron.PyraChat.IRC.Messages.Receive.Numerics;
using TopicMessage = Pyratron.PyraChat.IRC.Messages.Receive.Numerics.TopicMessage;

namespace Pyratron.PyraChat.IRC
{
    public class Channel
    {
        /// <summary>
        /// Channel name (with prefix).
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// List of channel modes.
        /// </summary>
        public List<char> Modes { get; } = new List<char>();

        /// <summary>
        /// Channel topic.
        /// </summary>
        public ChannelTopic Topic { get; private set; } = new ChannelTopic();

        /// <summary>
        /// Type of channel.
        /// </summary>
        public ChannelType Type { get; }

        public Client Client { get; }

        /// <summary>
        /// Users in the channel.
        /// </summary>
        public IEnumerable<User> Users
            => Client.Users.Where(user => user.Channels != null && user.Channels.Contains(this));

        /// <summary>
        /// List of masks banned from the channel.
        /// </summary>
        public List<string> BanList { get; } = new List<string>();

        /// <summary>
        /// List of masked allows in the channel even if they are banned.
        /// </summary>
        public List<string> ExceptionList { get; } = new List<string>();

        /// <summary>
        /// List of masks that are allowed in the channel in invite only mode.
        /// </summary>
        public List<string> InviteList { get; } = new List<string>();

        // Channel modes. (See https://www.alien.net.au/irc/chanmodes.html)
        public bool IsInviteOnly => Modes.Contains('i');
        public bool IsModeratedOnly => Modes.Contains('m');
        public bool NoOutsideMessages => Modes.Contains('n');
        public bool IsPrivate => Modes.Contains('p');
        public bool IsSecret => Modes.Contains('s');
        public bool IsTopicLocked => Modes.Contains('t');
        public bool IsAnonymous => Modes.Contains('a');
        public bool IsLimited => Modes.Contains('l');
        public bool IsKeyLocked => Modes.Contains('k');

        /// <summary>
        /// User limit if channel is limited (+l).
        /// </summary>
        public int Userlimit { get; private set; }

        /// <summary>
        /// Channel key if channel is locked (+k).
        /// </summary>
        public string Key { get; private set; }

        public Channel(Client client, string name)
        {
            Client = client;
            Name = name;
            Type = ChannelType.FromPrefix(name[0]);
        }

        public int AddMode(Client client, string channel, char mode, string parameter = "")
        {
            switch (mode)
            {
                case 'v':
                {
                    var user = client.UserFromNick(parameter);
                    user.AddRank(client, channel, UserRank.Voice);
                    return 1;
                }
                case 'h':
                {
                    var user = client.UserFromNick(parameter);
                    user.AddRank(client, channel, UserRank.HalfOp);
                    return 1;
                }
                case 'o':
                {
                    var user = client.UserFromNick(parameter);
                    user.AddRank(client, channel, UserRank.Op);
                    return 1;
                }
                case 'a':
                {
                    var user = client.UserFromNick(parameter);
                    user.AddRank(client, channel, UserRank.Admin);
                    return 1;
                }
                case 'q':
                {
                    var user = client.UserFromNick(parameter);
                    user.AddRank(client, channel, UserRank.Owner);
                    return 1;
                }
                case 'b':
                {
                    AddBan(parameter);
                        OnModeAdd(mode);
                        return 1;
                }
                case 'e':
                {
                    AddException(parameter);
                        OnModeAdd(mode);
                        return 1;
                }
                case 'I':
                {
                    AddInvite(parameter);
                        OnModeAdd(mode);
                        return 1;
                }
                case 'k':
                {
                    Key = parameter;
                    Modes.Add(mode);
                    return 1;
                }
                case 'l':
                {
                    int limit;
                    if (int.TryParse(parameter, out limit))
                    {
                        Userlimit = limit;
                        Modes.Add(mode);
                            OnModeAdd(mode);
                            return 1;
                    }
                    break;
                }
                default:
                {
                    if (!Modes.Contains(mode))
                        Modes.Add(mode);
                    break;
                }
            }
            OnModeAdd(mode);
            return 0;
        }

        /// <summary>
        /// Adds a user to the channel and fires the UserAdd event.
        /// </summary>
        public void AddUser(User user)
        {
            if (!user.Channels.Contains(this))
            {
                user.Channels.Add(this);
                if (!Client.Users.Contains(user))
                    Client.Users.Add(user);
                OnUserAdd(user);
            }
        }

        public int RemoveMode(Client client, string channel, char mode, string parameter = "")
        {
            switch (mode)
            {
                case 'v':
                {
                    var user = client.UserFromNick(parameter);
                    user.RemoveRank(client, channel, UserRank.Voice);
                    return 1;
                }
                case 'h':
                {
                    var user = client.UserFromNick(parameter);
                    user.RemoveRank(client, channel, UserRank.HalfOp);
                    return 1;
                }
                case 'o':
                {
                    var user = client.UserFromNick(parameter);
                    user.RemoveRank(client, channel, UserRank.Op);
                    return 1;
                }
                case 'a':
                {
                    var user = client.UserFromNick(parameter);
                    user.RemoveRank(client, channel, UserRank.Admin);
                    return 1;
                }
                case 'q':
                {
                    var user = client.UserFromNick(parameter);
                    user.RemoveRank(client, channel, UserRank.Owner);
                    return 1;
                }
                case 'b':
                {
                    RemoveBan(parameter);
                    OnModeRemove(mode);
                    return 1;
                }
                case 'e':
                {
                    RemoveException(parameter);
                    OnModeRemove(mode);
                    return 1;
                }
                case 'I':
                {
                    RemoveInvite(parameter);
                    OnModeRemove(mode);
                    return 1;
                }
                case 'k':
                {
                    if (Key.Equals(parameter))
                    {
                        Key = null;
                        if (Modes.Contains(mode))
                            Modes.Remove(mode);
                    }
                    OnModeRemove(mode);
                    return 1;
                }
                case 'l':
                {
                    Userlimit = -1;
                    if (Modes.Contains(mode))
                        Modes.Remove(mode);
                    break;
                }
                default:
                {
                    if (Modes.Contains(mode))
                        Modes.Remove(mode);
                    break;
                }
            }
            OnModeRemove(mode);
            return 0;
        }

        public void RemoveUser(User user)
        {
            user.Channels.Remove(this);
            Client.Users.Remove(user);
            OnUserRemove(user);
        }

        public override string ToString()
        {
            return Name;
        }

        internal void AddBan(string mask)
        {
            if (!BanList.Contains(mask))
            {
                BanList.Add(mask);
                OnBanChange(mask, false);
            }
        }

        internal void AddException(string mask)
        {
            if (!ExceptionList.Contains(mask))
            {
                ExceptionList.Add(mask);
                OnExceptionChange(mask, false);
            }
        }

        internal void AddInvite(string mask)
        {
            if (!InviteList.Contains(mask))
            {
                InviteList.Add(mask);
                OnInviteChange(mask, false);
            }
        }

        private void RemoveBan(string mask)
        {
            if (BanList.Contains(mask))
            {
                BanList.Remove(mask);
                OnBanChange(mask, false);
            }
        }

        private void RemoveException(string mask)
        {
            if (ExceptionList.Contains(mask))
            {
                ExceptionList.Remove(mask);
                OnExceptionChange(mask, false);
            }
        }

        private void RemoveInvite(string mask)
        {
            if (InviteList.Contains(mask))
            {
                InviteList.Remove(mask);
                OnInviteChange(mask, false);
            }
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

        public delegate void BanChangeEventHandler(string mask, bool isBanned);

        public delegate void ExceptionChangeEventHandler(string mask, bool isException);

        public delegate void InviteChangeEventHandler(string mask, bool isInvited);

        public delegate void KickEventHandler(KickMessage message);

        public delegate void ChannelModeIsEventHandler(ChannelModeIsMessage message);

        public delegate void ModeAddEventHandler(char mode);

        public delegate void ModeRemoveEventHandler(char mode);

        public event NoticeEventHandler Notice;
        public event MessageEventHandler Message;
        public event UserJoinEventHandler UserJoin;
        public event UserAddEventHandler UserAdd;
        public event UserPartEventHandler UserPart;
        public event UserRemoveEventHandler UserRemove;
        public event TopicEventHandler TopicChange;
        public event TopicWhoTimeEventHandler TopicWhoTime;
        public event ChannelModeEventHandler ChannelMode;
        public event BanChangeEventHandler BanChange;
        public event ExceptionChangeEventHandler ExceptionChange;
        public event InviteChangeEventHandler InviteChange;
        public event KickEventHandler Kick;
        public event ModeAddEventHandler ModeAdd;
        public event ModeRemoveEventHandler ModeRemove;
        public event ChannelModeIsEventHandler ReplyChannelModeIs;

        internal void OnNotice(NoticeMessage message) => Notice?.Invoke(message);
        internal void OnMessage(PrivateMessage message) => Message?.Invoke(message);
        internal void OnUserJoin(JoinMessage message) => UserJoin?.Invoke(message);
        internal void OnUserPart(PartMessage message) => UserPart?.Invoke(message);
        internal void OnUserAdd(User user) => UserAdd?.Invoke(user);
        internal void OnUserRemove(User user) => UserRemove?.Invoke(user);
        internal void OnModeAdd(char mode) => ModeAdd?.Invoke(mode);
        internal void OnModeRemove(char mode) => ModeRemove?.Invoke(mode);
        internal void OnTopicChange(TopicMessage message) => TopicChange?.Invoke(message);
        internal void OnTopicWhoTime(TopicWhoTimeMessage message) => TopicWhoTime?.Invoke(message);
        internal void OnChannelMode(ChannelModeMessage message) => ChannelMode?.Invoke(message);
        internal void OnBanChange(string mask, bool isBanned) => BanChange?.Invoke(mask, isBanned);
        internal void OnExceptionChange(string mask, bool isException) => ExceptionChange?.Invoke(mask, isException);
        internal void OnInviteChange(string mask, bool isInvited) => BanChange?.Invoke(mask, isInvited);
        internal void OnKick(KickMessage message) => Kick?.Invoke(message);
        internal void OnReplyChannelModeIs(ChannelModeIsMessage message) => ReplyChannelModeIs?.Invoke(message);

        #endregion //Events
    }
}