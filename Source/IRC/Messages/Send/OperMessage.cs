using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Oper message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.1.4" />
    public class OperMessage : SendableMessage
    {
        public string Name { get; }
        public string Password { get; }

        public OperMessage(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"OPER {Name} {Password}");
        }
    }
}