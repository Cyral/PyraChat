using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Time message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.4.6" />
    public class TimeMessage : SendableMessage
    {
        /// <summary>
        /// Server target (optional)
        /// </summary>
        public string Target { get; }

        public TimeMessage()
        {

        }

        public TimeMessage(string target = "")
        {
            Target = target;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"TIME {Target ?? string.Empty}".Trim());
        }
    }
}