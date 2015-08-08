using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Quit message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.1.7" />
    public class QuitMessage : SendableMessage
    {
        /// <summary>
        /// Quit message/reason text.
        /// </summary>
        public string Reason { get; }

        public QuitMessage(string reason = "")
        {
            Reason = reason;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"QUIT :{Reason}");
        }
    }
}