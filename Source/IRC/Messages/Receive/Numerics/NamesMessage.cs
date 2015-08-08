using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// Names message (RPL_NAMEREPLY).
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.5" />
    public class NamesMessage : ReceivableMessage
    {
        private static readonly char[] separator = {' '};

        public Channel Channel { get; }

        public NamesMessage(Message msg) : base(msg)
        {
            Channel = msg.Client.ChannelFromName(msg.Parameters[2]);
            if (msg.Client.UserFromNick(msg.Destination) == msg.Client.User)
            {
                var users = msg.Parameters[3].Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var user in users)
                {
                    var rank = UserRank.FromPrefix(user[0]);
                    var chanUser = msg.Client.UserFromNick(user);
                    if (chanUser == null) //Add new users discovered in response.
                        Channel.AddUser(new User(rank != UserRank.None ? user.Substring(1) : user, rank));
                    else //Update ranks of existing users (ourselves).
                        chanUser.AddRank(msg.Client, rank);
                }
                msg.Client.OnReplyNames(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "353";
        }
    }
}