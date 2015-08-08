using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Private message (PRIVMSG).
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.3.1" />
    public class PrivateMessage : SendableMessage
    {
        /// <summary>
        /// Message target. (Channel or user)
        /// </summary>
        public string Target { get; }

        /// <summary>
        /// Message text.
        /// </summary>
        public string Message { get; }

        public PrivateMessage(string target, string message)
        {
            Target = target;
            Message = message;
        }

        public PrivateMessage(User user, string message)
        {
            Target = user.Nick;
            Message = message;
        }

        public PrivateMessage(Channel channel, string message)
        {
            Target = channel.Name;
            Message = message;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"PRIVMSG {Target} :{Message}");
        }
    }
}