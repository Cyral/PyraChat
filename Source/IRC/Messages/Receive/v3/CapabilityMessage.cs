using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.v3
{
    /// <summary>
    /// IRCv3 CAP message and submessages.
    /// </summary>
    /// <see cref="http://ircv3.net/specs/core/capability-negotiation-3.1.html" />
    public class CapabilityMessage : ReceivableMessage
    {
        /// <summary>
        /// CAP submessage. (LS, ACK, etc.)
        /// </summary>
        public string Submessage => BaseMessage.Parameters[1];

        public Capability[] Capabilities { get; }

        public CapabilityMessage(Message msg) : base(msg)
        {
            var capstr = BaseMessage.Parameters[2].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            Capabilities = capstr.Select(Capability.FromName).Where(cap => cap != null).ToArray();

            switch (Submessage)
            {
                case "ACK":
                {
                    // If a capability is acknowledged, it is now activated.
                    msg.Client.ActiveCapabilities.AddRange(
                        Capabilities.Where(cap => !msg.Client.ActiveCapabilities.Contains(cap)));
                    break;
                }
                case "LS":
                {
                    // Add capabilities supported by server to list.
                    msg.Client.SupportedCapabilities.AddRange(
                        Capabilities.Where(cap => !msg.Client.SupportedCapabilities.Contains(cap)));
                    break;
                }
                case "LIST":
                {
                    msg.Client.ActiveCapabilities.AddRange(
                        Capabilities.Where(cap => !msg.Client.ActiveCapabilities.Contains(cap)));
                    break;
                }
                case "NAK":
                {
                    msg.Client.SupportedCapabilities.RemoveAll(cap => msg.Client.SupportedCapabilities.Contains(cap));
                    break;
                }
            }
            msg.Client.OnCapability(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "CAP";
        }
    }
}