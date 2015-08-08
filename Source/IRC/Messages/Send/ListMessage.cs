using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// List message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.6" />
    public class ListMessage : SendableMessage
    {
        public string[] Channels { get; }

        /// <summary>
        /// Server target request will be forwarded to. (Optional)
        /// </summary>
        public string Target { get; }

        public ListMessage()
        {
        }

        public ListMessage(string channel, string target = "")
        {
            Channels = new[] {channel};
            Target = target;
        }

        public ListMessage(string[] channels, string target = "")
        {
            Channels = channels;
            Target = target;
        }

        public void Send(StreamWriter writer, Client client)
        {
            if (Channels == null || Channels.Length == 0)
                writer.WriteLine("LIST");
            else
                writer.WriteLine(string.IsNullOrWhiteSpace(Target)
                    ? $"LIST {string.Join(",", Channels)}"
                    : $"LIST {string.Join(",", Channels)} {Target}");
        }
    }
}