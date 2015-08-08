using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// LUsers message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.4.2" />
    public class LUsersMessage : SendableMessage
    {
        public string Mask { get; }

        /// <summary>
        /// Server target (optional)
        /// </summary>
        public string Target { get; }

        public LUsersMessage()
        {
            
        }

        public LUsersMessage(string mask, string target ="")
        {
            Mask = mask;
            Target = target;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine($"LUSERS {Mask ?? string.Empty} {Target ?? string.Empty}".Trim());
        }
    }
}