using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// End of RPL_INVITELIST message. (RPL_ENDOFINVITELIST/347).
    /// </summary>
    public class EndOfInviteListMessage : ReceivableMessage
    {
        /// <summary>
        /// Name of channel invite list message was for.
        /// </summary>
        public string Channel => BaseMessage.Parameters[1];
        public string Info => BaseMessage.Parameters[2];

        public EndOfInviteListMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyEndOfInviteList(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "347";
        }
    }
}