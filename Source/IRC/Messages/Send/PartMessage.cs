using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Part message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.2" />
    public class PartMessage : SendableMessage
    {
        public string[] Channels { get; }

        public PartMessage(string channel)
        {
            Channels = new[] {channel};
        }

        public PartMessage(string[] channels)
        {
            Channels = channels;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"PART {string.Join(",", Channels)}");
        }
    }
}