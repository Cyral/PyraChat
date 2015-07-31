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
        public string Target { get; }

        /// <summary>
        /// Message text.
        /// </summary>
        public string Text { get; }

        public PrivateMessage(Message msg) : base(msg)
        {
            Target = msg.Destination;
            Text = msg.Parameters[1];

            if (msg.IsChannel)
            {

            }
            else
            {
                msg.Client.OnMessage(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "PRIVMSG";
        }
    }
}