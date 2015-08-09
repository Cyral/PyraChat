using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send.v3
{
    /// <summary>
    /// IRCv3 CAP LS message.
    /// </summary>
    /// <see cref="http://ircv3.net/specs/core/capability-negotiation-3.1.html" />
    public class CapabilityListSupportedMessage : SendableMessage
    {
        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine("CAP LS");
        }
    }
}