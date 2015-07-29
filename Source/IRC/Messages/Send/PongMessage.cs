using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Pong message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.7.3" />
    public class PongMessage : ISendable
    {
        public string Message { get; }

        public PongMessage(string message)
        {
            Message = message;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"PONG :{Message}");
        }
    }
}