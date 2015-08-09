using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// End of RPL_EXCEPTLIST message. (RPL_ENDOFEXCEPTLIST/349).
    /// </summary>
    public class EndOfExceptListMessage : ReceivableMessage
    {
        /// <summary>
        /// Name of channel exception message was for.
        /// </summary>
        public string Channel => BaseMessage.Parameters[1];
        public string Info => BaseMessage.Parameters[2];

        public EndOfExceptListMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyEndOfExceptList(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "349";
        }
    }
}