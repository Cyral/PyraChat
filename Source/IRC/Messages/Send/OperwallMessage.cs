using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Operwall message (WALLOPS).
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-4.6" />
    public class OperwallMessage : SendableMessage
    {
        /// <summary>
        /// Message text.
        /// </summary>
        public string Message { get; }

        public OperwallMessage(string message)
        {
            Message = message;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"WALLOPS :{Message}");
        }
    }
}