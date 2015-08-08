using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Invite message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.1" />
    public class InviteMessage : SendableMessage
    {
        public string Nick { get; }

        public string Channel { get; }

        public InviteMessage(string channel, string nick)
        {
            Channel = channel;
            Nick = nick;
        }

        public InviteMessage(Channel channel, string nick)
        {
            Channel = channel.Name;
            Nick = nick;
        }

        public InviteMessage(string channel, User user)
        {
            Channel = channel;
            Nick = user.Nick;
        }

        public InviteMessage(Channel channel, User user)
        {
            Channel = channel.Name;
            Nick = user.Nick;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"INVITE {Nick} {Channel}");
        }
    }
}