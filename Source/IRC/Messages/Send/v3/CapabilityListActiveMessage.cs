using System.IO;

namespace Pyratron.PyraChat.IRC.Messages.Send.v3
{
    /// <summary>
    /// IRCv3 CAP LIST message.
    /// </summary>
    /// <see cref="http://ircv3.net/specs/core/capability-negotiation-3.1.html" />
    public class CapabilityListActiveMessage : SendableMessage
    {
        public void Send(StreamWriter writer, Client client)
        {
            writer.WriteLine("CAP LIST");
        }
    }
}