using System.IO;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Send.v3
{
    /// <summary>
    /// IRCv3 CAP REQ message.
    /// </summary>
    /// <see cref="http://ircv3.net/specs/core/capability-negotiation-3.1.html" />
    public class CapabilityRequestMessage : SendableMessage
    {
        public string[] Capabilities { get; }

        public CapabilityRequestMessage(string capability)
        {
            Capabilities = new[] {capability};
        }

        public CapabilityRequestMessage(params string[] capabilities)
        {
            Capabilities = capabilities;
        }

        public CapabilityRequestMessage(Capability capability)
        {
            Capabilities = new[] {capability.Name};
        }

        public CapabilityRequestMessage(params Capability[] capabilities)
        {
            Capabilities = capabilities.Select(c => c.Name).ToArray();
        }

        public void Send(StreamWriter writer, Client client)
        {
            if (Capabilities != null)
                writer.WriteLine($"CAP REQ :{string.Join(" ", Capabilities)}");
        }
    }
}