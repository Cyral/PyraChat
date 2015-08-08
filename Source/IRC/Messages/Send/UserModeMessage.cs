using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// User mode message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.2.1" />
    public class UserModeMessage : SendableMessage
    {
        public string Nick { get; }

        /// <summary>
        /// User modes as a string, Ex: +o
        /// </summary>
        public string Modes { get; }

        public UserModeMessage(User user, string modes)
        {
            Nick = user.Nick;
            Modes = modes;
        }

        public UserModeMessage(string nickname, string modes)
        {
            Nick = nickname;
            Modes = modes;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"MODE {Nick} {Modes}");
        }
    }
}