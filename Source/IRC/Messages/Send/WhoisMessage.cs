using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Whois message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.6.2" />
    public class WhoisMessage : SendableMessage
    {
        /// <summary>
        /// Target mask.
        /// </summary>
        public string Mask { get; }

        /// <summary>
        /// Target server. (Optional)
        /// </summary>
        public string Server { get; }

        public WhoisMessage(string mask, string server = "")
        {
            Mask = mask;
            Server = server;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine(!string.IsNullOrEmpty(Server) ? $"WHOIS {Server} {Mask}" : $"WHOIS {Mask}");
        }
    }
}