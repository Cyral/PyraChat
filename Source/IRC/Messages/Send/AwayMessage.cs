using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Away message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-4.1" />
    public class AwayMessage : SendableMessage
    {
        public string Reason { get; }

        /// <summary>
        /// Create a message saying the user is NOT away.
        /// </summary>
        public AwayMessage()
        {
            
        }

        /// <summary>
        /// Create a message saying the user is away, with a reason.
        /// </summary>
        public AwayMessage(string reason)
        {
            Reason = reason;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine(string.IsNullOrWhiteSpace(Reason)
                ? "AWAY"
                : $"AWAY :{Reason}");
        }
    }
}