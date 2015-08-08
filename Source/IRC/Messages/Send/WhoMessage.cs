using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Away message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.6.1" />
    public class WhoMessage : SendableMessage
    {
        /// <summary>
        /// Target mask.
        /// </summary>
        public string Mask { get; }

        /// <summary>
        /// Request query to return only operators.
        /// </summary>
        public bool OnlyOperators { get; }

        public WhoMessage()
        {
        }

        public WhoMessage(string mask, bool onlyOperators = false)
        {
            Mask = mask;
            OnlyOperators = false;
        }

        public void Send(StreamWriter writer, Client client)
        {
            if (OnlyOperators)
                writer.WriteLine($"WHO {Mask} o");
            else if (!string.IsNullOrWhiteSpace(Mask))
                writer.WriteLine($"WHO {Mask}");
            else
                writer.WriteLineAsync("WHO");
        }
    }
}