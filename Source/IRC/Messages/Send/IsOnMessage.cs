using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// IsOn message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-4.9" />
    public class IsOnMessage : SendableMessage
    {
        public string[] Nicks { get; }

        public IsOnMessage(string nick)
        {
            Nicks = new[] {nick};
        }

        public IsOnMessage(params string[] nicks)
        {
            Nicks = nicks;
        }


        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"ISON {string.Join(" ", Nicks)}");
        }
    }
}