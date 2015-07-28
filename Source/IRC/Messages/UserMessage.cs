using System.IO;

namespace Pyratron.PyraChat.IRC.Messages
{
    /// <summary>
    /// User message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.1.3" />
    public class UserMessage : ISendable
    {
        public string Ident { get; }
        public string RealName { get; }
        public int Mode { get; }

        public UserMessage(string ident, string realName, int mode = 0)
        {
            Ident = ident;
            RealName = realName;
            Mode = mode;
        }

        public UserMessage(User user)
        {
            Ident = user.Ident;
            RealName = user.RealName;
            Mode = 0;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"USER {Ident} {Mode} * :{RealName}");
        }
    }
}