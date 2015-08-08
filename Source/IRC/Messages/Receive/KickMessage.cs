using System;
using System.Linq;
using Microsoft.Win32;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Kick message. (KICK)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.8"/>
    public class KickMessage : ReceivableMessage
    {
        /// <summary>
        /// User who was kicked.
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Kick reason/message.
        /// </summary>
        public string Reason { get; private set; }

        public KickMessage(Message msg) : base(msg)
        {
            Reason = msg.Parameters[2];
            User = msg.Client.UserFromNick(msg.Parameters[1]);
            if (User != null)
            {
                msg.Channel.RemoveUser(User);
                msg.Channel.OnKick(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "KICK";
        }
    }
}
