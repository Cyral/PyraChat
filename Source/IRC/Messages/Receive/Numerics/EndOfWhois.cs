using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// End of whois message. (RPL_ENDOFWHOIS/318).
    /// </summary>
    public class EndOfWhoisMessage : ReceivableMessage
    {
        /// <summary>
        /// Nick of user that WHOIS message was performed on.
        /// </summary>
        public string Nick => BaseMessage.Parameters[1];
        public string Info => BaseMessage.Parameters[2];

        public EndOfWhoisMessage(Message msg) : base(msg)
        {
             msg.Client.OnReplyEndOfWhois(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "318";
        }
    }
}