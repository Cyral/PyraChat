using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_AWAY message. (301)
    /// </summary>
    public class AwayMessage : ReceivableMessage
    {
        /// <summary>
        /// User who is away.
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Away reason/message.
        /// </summary>
        public string Reason { get; }

        public AwayMessage(Message msg) : base(msg)
        {
            User = msg.Client.UserFromNick(msg.Destination);
            Reason = msg.Parameters[1];
            if (User != null)
            {
                User.SetIsAway(msg.Client, true, Reason);
                msg.Client.OnReplyAway(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "301";
        }
    }
}