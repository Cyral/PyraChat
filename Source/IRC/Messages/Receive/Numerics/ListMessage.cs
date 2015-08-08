using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_LIST message. (322)
    /// </summary>
    public class ListMessage : ReceivableMessage
    {
        public string Channel => BaseMessage.Parameters[1];

        public string Topic => BaseMessage.Parameters[3];

        /// <summary>
        /// Number of visible users in the channel.
        /// </summary>
        public int UsersVisible { get; }

        public ListMessage(Message msg) : base(msg)
        {
            var visible = -1;
            if (int.TryParse(msg.Parameters[2], out visible))
                UsersVisible = visible;
            msg.Client.OnReplyList(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "322";
        }
    }
}