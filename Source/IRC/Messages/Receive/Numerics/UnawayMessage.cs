using System.Text.RegularExpressions;

namespace Pyratron.PyraChat.IRC.Messages.Receive.Numerics
{
    /// <summary>
    /// RPL_UNAWAY message. (305)
    /// </summary>
    public class UnAwayMessage : ReceivableMessage
    {
        /// <summary>
        /// User who is no longer away.
        /// </summary>
        public User User { get; }

        public UnAwayMessage(Message msg) : base(msg)
        {
            User = msg.Client.UserFromNick(msg.Destination);
            if (User != null)
            {
                User.SetIsAway(msg.Client, false);
                msg.Client.OnReplyUnAway(this);
            }
        }

        public static bool CanProcess(Message msg)
        {
            return msg.Type == "305";
        }
    }
}