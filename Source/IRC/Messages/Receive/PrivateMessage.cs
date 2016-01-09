using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Receive
{
    /// <summary>
    /// Private message (PRIVMSG).
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.3.1" />
    public class PrivateMessage : ReceivableMessage
    {
        /// <summary>
        /// Message target, either a channel or a user.
        /// </summary>
        public string Target => BaseMessage.Destination;

        /// <summary>
        /// Message text.
        /// </summary>
        public string Message => BaseMessage.Parameters[1];

        public PrivateMessage(Message msg) : base(msg)
        {
            if (msg.IsChannel)
                msg.Channel.OnMessage(this);
            else
                msg.Client.OnMessage(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "PRIVMSG";
        }
    }
}