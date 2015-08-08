using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send
{
    /// <summary>
    /// Motd message.
    /// </summary>
    /// <see cref="http://tools.ietf.org/html/rfc2812#section-3.4.1" />
    public class MOTDMessage : SendableMessage
    {
        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine("MOTD");
        }
    }
}