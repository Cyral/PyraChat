using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Kick message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.8" />
    public class KickMessage : SendableMessage
    {
        public string[] Nicks { get; }
        public string[] Channels { get; }
        public string Reason { get; }

        public KickMessage(string channel, string user, string reason = "")
        {
            Reason = reason;
            Nicks = new[] {user};
            Channels = new[] {channel};
        }

        public KickMessage(string[] channels, string[] users, string reason = "")
        {
            Reason = reason;
            Nicks = users;
            Channels = channels;
        }

        public KickMessage(Channel channel, User user, string reason = "")
        {
            Reason = reason;
            Channels = new[] {channel.Name};
            Nicks = new[] {user.Nick};
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine(string.IsNullOrWhiteSpace(Reason)
                ? $"PART {string.Join(",", Channels)} {string.Join(",", Nicks)}"
                : $"PART {string.Join(",", Channels)} {string.Join(",", Nicks)} :{Reason}");
        }
    }
}