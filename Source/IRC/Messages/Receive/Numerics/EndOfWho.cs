using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// End of who message. (RPL_ENDOFWHOIS/315).
    /// </summary>
    public class EndOfWhoMessage : ReceivableMessage
    {
        /// <summary>
        /// Name of user/channel WHO message was performed on.
        /// </summary>
        public string Name => BaseMessage.Parameters[1];
        public string Info => BaseMessage.Parameters[2];

        public EndOfWhoMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyEndOfWho(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "315";
        }
    }
}