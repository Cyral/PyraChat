using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Invite message. (INVITE)
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.8"/>
    public class InviteMessage : ReceivableMessage
    {
        /// <summary>
        /// Channel name user is being invited to.
        /// </summary>
        public string Channel => BaseMessage.Parameters[1];

        public InviteMessage(Message msg) : base(msg)
        {
            if (msg.Client.UserFromNick(msg.Parameters[0]) == msg.Client.User)
                msg.Client.OnInvite(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "INVITE";
        }
    }
}
