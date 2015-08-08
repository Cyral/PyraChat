using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Version message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.4.3" />
    public class VersionMessage : SendableMessage
    {
        /// <summary>
        /// Server target (optional)
        /// </summary>
        public string Target { get; }

        public VersionMessage()
        {
        }

        public VersionMessage(string target)
        {
            Target = target;
        }

        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine(string.IsNullOrWhiteSpace(Target) ? "VERSION" : $"VERSION {Target}");
        }
    }
}