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

        public string Reason { get; }

        public PartMessage(string channel, string reason = "")
        {
            Reason = reason;
            Channels = new[] {channel};
        }

        public PartMessage(string[] channels, string reason = "")
        {
            Reason = reason;
            Channels = channels;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine(string.IsNullOrWhiteSpace(Reason)
                ? $"PART {string.Join(",", Channels)}"
                : $"PART {string.Join(",", Channels)} :{Reason}");
        }
    }
}