using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// End of RPL_BANLIST message. (RPL_ENDOFBANLIST/368).
    /// </summary>
    public class EndOfBanListMessage : ReceivableMessage
    {
        /// <summary>
        /// Name of channel banlist message was for.
        /// </summary>
        public string Channel => BaseMessage.Parameters[1];
        public string Info => BaseMessage.Parameters[2];

        public EndOfBanListMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyEndOfBanList(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "368";
        }
    }
}