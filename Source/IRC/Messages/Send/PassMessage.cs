using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Pass message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.1.1" />
    public class PassMessage : SendableMessage
    {
        /// <summary>
        /// Connection password.
        /// </summary>
        public string Password { get; }

        public PassMessage(string password)
        {
            Password = password;
        }

        public PassMessage(User user)
        {
            Password = user.Nick;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"PASS {Password}");
        }
    }
}