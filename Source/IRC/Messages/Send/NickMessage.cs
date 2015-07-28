using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Nick message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.1.2" />
    public class NickMessage : ISendable
    {
        public string NickName { get; }

        public NickMessage(string nickName)
        {
            NickName = nickName;
        }

        public NickMessage(User user)
        {
            NickName = user.Nick;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"NICK {NickName}");
        }
    }
}