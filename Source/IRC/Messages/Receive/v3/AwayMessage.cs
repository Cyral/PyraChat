using System;
using System.Linq;

namespace Pyratron.PyraChat.IRC.Messages.Receive.v3
{
    /// <summary>
    /// IRCv3 away message. (AWAY)
    /// </summary>
    /// <remarks>
    /// away-notify capability must be enabled.
    /// </remarks>
    /// <see cref="http://ircv3.net/specs/extensions/away-notify-3.1.html"/>
    public class AwayMessage : ReceivableMessage
    {
        /// <summary>
        /// The user who's away state changed.
        /// </summary>
        public User User => BaseMessage.User;

        /// <summary>
        /// Away message. Empty is no longer away.
        /// </summary>
        public string Message => BaseMessage.Parameters[0];
        
        public AwayMessage(Message msg) : base(msg)
        {
             User.SetIsAway(msg.Client, !string.IsNullOrWhiteSpace(Message), Message);
             msg.Client.OnAway(this);
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "AWAY" && msg.Client.ActiveCapabilities.Contains(Capability.AwayNotify);
        }
    }
}
