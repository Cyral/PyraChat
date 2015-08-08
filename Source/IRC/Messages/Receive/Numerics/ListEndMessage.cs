using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// End of list messages. (RPL_LISTEND/323)
    /// </summary>
    public class ListEndMessage : ReceivableMessage
    {
        public string Info => BaseMessage.Parameters[1];

        public ListEndMessage(Message msg) : base(msg)
        {
            msg.Client.OnReplyListEnd(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "323";
        }
    }
}